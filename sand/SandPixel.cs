using System.Collections.Generic;

namespace HW5_GROUP_PROJECT.sand
{

    public enum PixelId: byte
    {
        INVALID = 0,
        AIR = 1,
        SAND = 2
    }

    public struct SandPixel
    {
        public PixelId id;
        public byte flags;
        public short color;
    }

    public ref struct SrcSandGroup
    {
        public ref SandPixel TopLeft;
        public ref SandPixel TopRight;
        public ref SandPixel BottomLeft;
        public ref SandPixel BottomRight;
    }

    /// <summary>
    /// complete madness.
    /// </summary>
    public struct ReplaceSandGroup
    {
        ReplaceOperation TopLeft;
        ReplaceOperation TopRight;
        ReplaceOperation BottomLeft;
        ReplaceOperation BottomRight;

        public ReplaceSandGroup()
        {
            this.TopLeft = new(0);
            this.TopRight = new(1);
            this.BottomLeft = new(2);
            this.BottomRight = new(3);
        }

        /// <summary>
        /// apply this operation.
        /// </summary>
        /// <param name="src">source sand group</param>
        public void Apply(ref SrcSandGroup src)
        {
            SandPixel newTopLeft = this.TopLeft.Apply(ref src);
            SandPixel newTopRight = this.TopRight.Apply(ref src);
            SandPixel newBottomLeft = this.BottomLeft.Apply(ref src);
            SandPixel newBottomRight = this.BottomRight.Apply(ref src);
            src.TopLeft = newTopLeft;
            src.TopRight = newTopRight;
            src.BottomLeft = newBottomLeft;
            src.BottomRight = newBottomRight;
        }
    }

    public struct LookupTable
    {
        byte cellTypes;
        ReplaceSandGroup[] lookup;
        public LookupTable(byte types)
        {
            this.cellTypes = types;
            // raise types to power of 4 for size of the replace LUT
            int total = types;
            total *= total;
            total *= total;
            lookup = new ReplaceSandGroup[total];
        }

        public void Update(ref SrcSandGroup src, InterpretPixel interpret)
        {
            int i = interpret.Invoke(src.BottomRight);
            i *= this.cellTypes;
            i += interpret.Invoke(src.BottomLeft);
            i *= this.cellTypes;
            i += interpret.Invoke(src.TopRight);
            i *= this.cellTypes;
            i += interpret.Invoke(src.TopLeft);
            this.lookup[i].Apply(ref src);
        }
    }

    // public delegate void UpdateFunction();
    public delegate LookupTable LookupTableTypeSupplier();
    public delegate byte InterpretPixel(in SandPixel pixel);

    public struct SandGrid
    {
        public const ulong CHUNK_WIDTH = 1 << CHUNK_BITS;
        public const ulong INDEX_IN_CHUNK_MASK = CHUNK_BITS * 2 - 1;
        public const int CHUNK_BITS = 10;
        public static ZOrderIndex GetChunk(ZOrderIndex index)
        {
            return index >> (CHUNK_BITS * 2);
        }
        public static ZOrderIndex GetPosInChunk(ZOrderIndex index)
        {
            return index & INDEX_IN_CHUNK_MASK;
        }

        private readonly Dictionary<ZOrderIndex, SandChunk> chunks;
        private ZOrderIndex current = 0;
        private ZOrderIndex max = new(63,63);

        public SandGrid()
        {
            this.chunks = new();
            this.chunks[0] = new();
        }

        public void Update(in LookupTable lut, InterpretPixel interpret, byte offsetStep)
        {
            this.current = 0;
            while (this.current <= this.max)
            {
                SrcSandGroup sourceGroup;
                sourceGroup.TopLeft = ref this.GetPixel(this.current);
                ZOrderIndex right = this.current.IncrX();
                sourceGroup.TopRight = ref this.GetPixel(right);
                sourceGroup.BottomLeft = ref this.GetPixel(this.current.IncrY());
                sourceGroup.BottomRight = ref this.GetPixel(right.IncrY());
                lut.Update(ref sourceGroup, interpret);
                this.current += 4;
            }
        }
        public ref SandPixel GetPixel(ZOrderIndex index)
        {
            return ref this.chunks[GetChunk(index)].pixels[GetPosInChunk(index)];
        }
    }


    public struct SandChunk
    {
        public readonly SandPixel[] pixels;
        public SandChunk()
        {
            pixels = new SandPixel[SandGrid.CHUNK_WIDTH * SandGrid.CHUNK_WIDTH];
        }
    }

    // struct SubpixelPosition
    // {
    //     const byte SubpixelBits = 0xF;
    //     const byte SubpixelCenter = 0b1000;
    //     byte position;
    //     public sbyte x => (sbyte)(position & SubpixelBits - SubpixelCenter);
    //     public sbyte y => (sbyte)((position >> 4) & SubpixelBits - SubpixelCenter);
    // }


}
/*
const SUBPIXEL_CENTER: u8 = 0x77;
const SUBPIXEL_BITS: u8 = 4;
const SUBPIXEL_MASK: u8 = 0xF;

a single falling sand tile, and all data associated
all falling sand tiles will have a velocity, and a subpixel
position.

the id of the tile determines what kind of tile it
is, and how that behaves.
 
#[derive(Clone, Copy)]
pub struct FallingSandTile {
    velocity_x: u8,
    velocity_y: u8,
    id: FallingSandTileId,
    subpixel: u8,
}

#[allow(unused)]
#[repr(u16)]
#[derive(Clone, Copy, PartialEq, Eq, Debug, Default)]
pub enum FallingSandTileId {
    #[default]
    Invalid = 0,
    Air = 1,
    Sand = 2,
}

#[allow(unused)]
impl FallingSandTile {
    pub fn new() -> Self {
        Self::new_from_id(FallingSandTileId::default())
    }
    pub fn new_from_id(id: FallingSandTileId) -> Self {
        Self {
            velocity_x: 0,
            velocity_y: 0,
            id,
            subpixel: SUBPIXEL_CENTER,
        }
    }
    pub fn subpixel_x(self) -> u8 {
        self.subpixel & SUBPIXEL_MASK
    }
    pub fn subpixel_y(self) -> u8 {
        (self.subpixel >> SUBPIXEL_BITS) & SUBPIXEL_MASK
    }
    pub fn id(self) -> FallingSandTileId {
        self.id
    }
}


use crate::falling_sand_2d::falling_sand_tile::FallingSandTileId;
use crate::z_order_2d::z_cut_2d::ZCut;
use crate::z_order_2d::z_index_2d::ZOrderIndex;

use super::falling_sand_tile::FallingSandTile;

pub const CHUNK_WIDTH: usize = 1 << CHUNK_BITS;
pub const INDEX_IN_CHUNK_MASK: u64 = CHUNK_BITS * 2 - 1;
pub const CHUNK_BITS: u64 = 10;

/**
a chunk of space in the falling sand simulation,
with size 1024 * 1024.
values in this are ordered by a z-index curve.

pub struct FallingSandChunk {
    data: Vec<FallingSandTile>,
    #[allow(unused)]
    last_update_range: Option<ZCut>,
}

// static impls for interacting with ZIndex
impl FallingSandChunk {
    pub fn get_chunk_id(index: ZOrderIndex) -> ZOrderIndex {
        ZOrderIndex::new(index.index() >> (CHUNK_BITS * 2))
    }

    /// the index in chunk bit mask is always low enough that this should
    /// never truncate.
    #[allow(clippy::cast_possible_truncation)]
    pub fn get_index_in_chunk(index: ZOrderIndex) -> usize {
        (index.index() & INDEX_IN_CHUNK_MASK) as usize
    }
}

impl FallingSandChunk {
    pub fn new() -> Self {
        Self::new_from_tile_id(FallingSandTileId::Invalid)
    }
    pub fn new_from_tile_id(id: FallingSandTileId) -> Self {
        Self {
            data: vec![FallingSandTile::new_from_id(id); CHUNK_WIDTH * CHUNK_WIDTH],
            last_update_range: None,
        }
    }

    pub fn get_tile(&self, index: ZOrderIndex) -> &FallingSandTile {
        let num_index = Self::get_index_in_chunk(index);
        &self.data[num_index]
    }

    pub fn place_tile(&mut self, index: ZOrderIndex, id: FallingSandTileId) {
        self.data[Self::get_index_in_chunk(index)] = FallingSandTile::new_from_id(id);
    }
}

#[cfg(test)]
mod tests {
    use crate::falling_sand_2d::falling_sand_chunk::FallingSandChunk;
    use crate::falling_sand_2d::falling_sand_tile::FallingSandTileId;
    use crate::z_order_2d::z_index_2d::ZOrderIndex;

    // test to check if the alloc of a chunk itself will trigger stack overflow.
    #[test]
    fn try_alloc_chunk() {
        let test_chunk = FallingSandChunk::new_from_tile_id(FallingSandTileId::Air);
        drop(test_chunk);
    }

    #[test]
    fn verify_set_changes() {
        let mut test_chunk = FallingSandChunk::new_from_tile_id(FallingSandTileId::Air);
        let index = ZOrderIndex::from_coords(10, 10);
        test_chunk.place_tile(index, FallingSandTileId::Sand);
        assert_eq!(test_chunk.get_tile(index).id(), FallingSandTileId::Sand);
    }
}

*/
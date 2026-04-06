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
*/
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
        Self::new_from_tile_id(FallingSandTileId::INVALID)
    }
    pub fn new_from_tile_id(id: FallingSandTileId) -> Self {
        Self {
            data: vec![FallingSandTile::new_from_id(id);CHUNK_WIDTH * CHUNK_WIDTH],
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
mod tests{
    use crate::{falling_sand_2d::{falling_sand_chunk::FallingSandChunk, falling_sand_tile::FallingSandTileId}, z_order_2d::z_index_2d::ZOrderIndex};

    // test to check if the alloc of a chunk itself will trigger stack overflow.
    #[test]
    fn try_alloc_chunk(){
        let test_chunk = FallingSandChunk::new_from_tile_id(FallingSandTileId::AIR);
        drop(test_chunk);
    }

    #[test]
    fn verify_set_changes(){
        let mut test_chunk = FallingSandChunk::new_from_tile_id(FallingSandTileId::AIR);
        let index = ZOrderIndex::from_coords(10,10);
        test_chunk.place_tile(index, FallingSandTileId::SAND);
        assert_eq!(test_chunk.get_tile(index).id(), FallingSandTileId::SAND);
    }
}
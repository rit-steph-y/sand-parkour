use crate::z_order_2d::{z_cut_2d::ZCut, z_index_2d::ZOrderIndex};

use super::falling_sand_tile::FallingSandTile;

pub const CHUNK_WIDTH: usize = 1 << CHUNK_BITS;
pub const INDEX_IN_CHUNK_MASK: u64 = CHUNK_BITS * 2 - 1;
pub const CHUNK_BITS: u64 = 10;

/**
a chunk of space in the falling sand simulation,
with size 1024 * 1024.
values in this are ordered by a z-index curve.
*/
#[repr(C)]
pub struct FallingSandChunk {
    data: [FallingSandTile; CHUNK_WIDTH * CHUNK_WIDTH],
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
    pub fn get_tile(&self, index: ZOrderIndex) -> &FallingSandTile {
        let num_index = Self::get_index_in_chunk(index);
        &self.data[num_index]
    }
}

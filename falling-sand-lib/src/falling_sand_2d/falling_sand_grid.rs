use crate::z_order_2d::z_index_2d::ZIndex;

use super::falling_sand_chunk::FallingSandChunk;

use std::collections::HashMap;

// A grid of chunks of falling sand,
// this is basically minecraft chunks, but 2d.
pub struct FallingSandGrid {
    chunks: HashMap<ZIndex, FallingSandChunk>,
}

impl FallingSandGrid {
    pub fn new() -> Self {
        Self {
            chunks: HashMap::new(),
        }
    }
    pub fn get_tile(&self, index: &ZIndex) -> u16{
        let chunk_id = FallingSandChunk::get_chunk_id(index);
        if let Some(chunk) = self.chunks.get(&chunk_id){
            return chunk.get_tile(index).id();
        }
        return 0;
    }
}

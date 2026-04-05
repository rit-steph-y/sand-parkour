use super::falling_sand_chunk::FallingSandChunk;

use std::collections::HashMap;

// A grid of chunks of falling sand,
// this is basically minecraft chunks, but 2d.
pub struct FallingSandGrid {
    pub count: u8,
    chunks: HashMap<u64, FallingSandChunk>,
}

impl FallingSandGrid {
    pub fn new() -> Self {
        Self {
            count: 0,
            chunks: HashMap::new(),
        }
    }
}

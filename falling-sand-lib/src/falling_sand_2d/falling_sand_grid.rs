use super::falling_sand_chunk::FallingSandChunk;

use std::collections::HashMap;

// A grid of chunks of falling sand,
// this is basically minecraft chunks, but 2d.
pub struct FallingSandGrid {
    chunks: HashMap<u64, FallingSandChunk>,
}

impl FallingSandGrid {
    pub fn new() -> Self {
        Self {
            chunks: HashMap::new(),
        }
    }
}

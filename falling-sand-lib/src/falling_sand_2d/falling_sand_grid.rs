use crate::falling_sand_2d::falling_sand_tile::FallingSandTileId;
use crate::z_order_2d::z_index_2d::ZOrderIndex;

use super::falling_sand_chunk::FallingSandChunk;

use std::collections::HashMap;

/**
A grid of chunks of falling sand,
this is basically minecraft chunks, but 2d.
 */
pub struct FallingSandGrid {
    chunks: HashMap<ZOrderIndex, FallingSandChunk>,
}

impl FallingSandGrid {
    pub fn new() -> Self {
        Self {
            chunks: HashMap::new(),
        }
    }
    pub fn get_tile(&self, index: ZOrderIndex) -> FallingSandTileId {
        let chunk_id = FallingSandChunk::get_chunk_id(index);
        if let Some(chunk) = self.chunks.get(&chunk_id) {
            return chunk.get_tile(index).id();
        }
        FallingSandTileId::INVALID
    }
    pub fn place_tile(&mut self, index: ZOrderIndex, id: FallingSandTileId) {
        let chunk_id = FallingSandChunk::get_chunk_id(index);
        if let Some(chunk) = self.chunks.get_mut(&chunk_id) {
            chunk.place_tile(index, id);
        } else {
            let mut chunk = FallingSandChunk::new();
            chunk.place_tile(index, id);
            self.chunks.insert(chunk_id, chunk);
        }
    }
    pub fn place_tile_at_coords(&mut self, x: u32, y: u32, id: FallingSandTileId) {
        self.place_tile(ZOrderIndex::from_coords(x, y), id);
    }
}

#[cfg(test)]
mod tests{
    use crate::falling_sand_2d::{ falling_sand_grid::FallingSandGrid, falling_sand_tile::FallingSandTileId};

    #[test]
    fn test_place_tile_at_coords(){
        let mut instance = FallingSandGrid::new();
        instance.place_tile_at_coords(3, 3, FallingSandTileId::AIR);
    }
}
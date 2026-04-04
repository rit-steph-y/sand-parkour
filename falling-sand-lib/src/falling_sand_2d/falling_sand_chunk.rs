use super::falling_sand_tile::FallingSandTile;

// a chunk of falling sand, with size 1024 * 1024.
// values in this are ordered by a z-index curve.
pub struct FallingSandChunk {
    data: [FallingSandTile; 1024 * 1024],
}

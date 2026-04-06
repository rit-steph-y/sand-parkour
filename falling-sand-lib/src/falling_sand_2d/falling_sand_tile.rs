/**
a single falling sand tile, and all data associated
all falling sand tiles will have a velocity, and a subpixel
position.

the id of the tile determines what kind of tile it
is, and how that behaves.
 */

const SUBPIXEL_CENTER: u8 = 0b0111_1111;

#[derive(Clone, Copy)]
pub struct FallingSandTile {
    velocity_x: u8,
    velocity_y: u8,
    id: FallingSandTileId,
    subpixel: u8,
}

#[repr(u16)]
#[derive(Clone, Copy, PartialEq, Eq, Debug)]
pub enum FallingSandTileId {
    INVALID = 0,
    AIR = 1,
    SAND = 2,
}

impl Default for FallingSandTileId {
    fn default() -> Self {
        Self::INVALID
    }
}

impl FallingSandTile {
    pub fn new() -> Self {
        Self::new_from_id(FallingSandTileId::default())
    }
    pub fn new_from_id(id: FallingSandTileId) -> Self {
        Self {
            velocity_x: 0,
            velocity_y: 0,
            id: id,
            subpixel: SUBPIXEL_CENTER,
        }
    }
    pub fn id(&self) -> FallingSandTileId {
        self.id
    }
}

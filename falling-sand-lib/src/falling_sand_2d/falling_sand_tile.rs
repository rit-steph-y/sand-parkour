const SUBPIXEL_CENTER: u8 = 0x77;
const SUBPIXEL_BITS: u8 = 4;
const SUBPIXEL_MASK: u8 = 0xF;
/**
a single falling sand tile, and all data associated
all falling sand tiles will have a velocity, and a subpixel
position.

the id of the tile determines what kind of tile it
is, and how that behaves.
 */
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
    // pub fn get_move_dir(&self){

    // }
}

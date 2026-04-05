/**
 * a single falling sand tile, and all data associated
 * all falling sand tiles will have a velocity, and a subpixel
 * position.
 * 
 * the id of the tile determines what kind of tile it
 * is, and how that behaves.
 */
#[repr(C)]
pub struct FallingSandTile {
    velocity_x: u16,
    velocity_y: u16,
    id: u16,
    subpixel_x: u8,
    subpixel_y: u8
}

impl FallingSandTile {
    pub fn id(&self) -> u16{
        self.id
    }
}
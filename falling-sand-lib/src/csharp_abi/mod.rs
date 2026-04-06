use std::cell::Ref;
use std::cell::RefCell;
use std::cell::RefMut;

use crate::falling_sand_2d::falling_sand_grid::FallingSandGrid;
use crate::falling_sand_2d::falling_sand_tile::FallingSandTileId;
use crate::z_order_2d::z_index_2d::ZOrderIndex;

#[repr(C)]
struct FallingSandGridHandle {
    internal_val: Box<RefCell<FallingSandGrid>>,
}

impl FallingSandGridHandle {
    #[unsafe(no_mangle)]
    pub extern "C" fn new_sand_handle() -> Self {
        let grid = FallingSandGrid::new();
        let refcell = RefCell::new(grid);
        Self {
            internal_val: Box::new(refcell),
        }
    }
    #[unsafe(no_mangle)]
    pub extern "C" fn test_sand_handle(&self, position: ZOrderIndex) -> FallingSandTileId {
        self.self_immut(|s| s.get_tile(position))
            .unwrap_or_default()
    }
    #[unsafe(no_mangle)]
    pub extern "C" fn place_tile_at_coords_sand_handle(
        &self,
        x: u32,
        y: u32,
        id: FallingSandTileId,
    ) {
        self.self_mut(|mut s| s.place_tile_at_coords(x, y, id));
    }
    #[unsafe(no_mangle)]
    pub extern "C" fn dispose_sand_handle(self) {
        drop(self);
    }
}

impl FallingSandGridHandle {
    fn self_mut<T>(&self, func: impl Fn(RefMut<'_, FallingSandGrid>) -> T) -> Option<T> {
        match self.internal_val.try_borrow_mut() {
            Ok(mutable_ref) => Some(func(mutable_ref)),
            Err(err) => {
                println!(
                    "error encountered when trying to acquire mutable reference, 
                make sure no other site in the code is holding onto a borrow 
                Error: {err}"
                );
                None
            }
        }
    }
    // apply operation to self if it is able to acquire a reference to it, otherwise
    //
    fn self_immut<T>(&self, func: impl Fn(Ref<'_, FallingSandGrid>) -> T) -> Option<T> {
        match self.internal_val.try_borrow() {
            Ok(reference) => Some(func(reference)),
            Err(err) => {
                println!(
                    "error encountered when trying to acquire a immutable reference, 
                make sure no other site in the code is holding onto a mutable borrow 
                Error: {err}"
                );
                None
            }
        }
    }
}

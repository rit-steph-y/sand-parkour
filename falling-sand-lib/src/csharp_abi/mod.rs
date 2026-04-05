use std::cell::RefCell;

use crate::{falling_sand_2d::falling_sand_grid::FallingSandGrid, z_order_2d::z_index_2d::ZIndex};

#[repr(C)]
struct FallingSandGridHandle {
    internal_val: Box<RefCell<FallingSandGrid>>,
}

impl FallingSandGridHandle {
    #[unsafe(no_mangle)]
    pub extern "C" fn new_falling_sand_handle() -> Self {
        let grid = FallingSandGrid::new();
        let refcell = RefCell::new(grid);
        Self {
            internal_val: Box::new(refcell),
        }
    }
    #[unsafe(no_mangle)]
    pub extern "C" fn test_falling_sand_handle(&self, position: ZIndex) -> u16 {
        let b = self.internal_val.borrow();
        return b.get_tile(&position);
    }
    #[unsafe(no_mangle)]
    pub extern "C" fn dispose_falling_sand_handle(self) {
        drop(self)
    }
}

use std::{cell::RefCell, rc::Rc};

use crate::falling_sand_2d::falling_sand_grid::FallingSandGrid;

#[repr(C)]
#[derive(Clone)]
struct FallingSandGridHandle {
    internal_val: Rc<RefCell<FallingSandGrid>>,
}

impl FallingSandGridHandle {
    #[unsafe(no_mangle)]
    pub unsafe extern "C" fn new_falling_sand_handle() -> Self {
        let grid = FallingSandGrid::new();
        let refcell = RefCell::new(grid);
        Self {
            internal_val: Rc::new(refcell),
        }
    }
    #[unsafe(no_mangle)]
    pub unsafe extern "C" fn test_falling_sand_handle(&mut self) -> i32{
        let mut b = self.internal_val.borrow_mut();
        b.count += 1;
        b.count as i32
    }
    #[unsafe(no_mangle)]
    pub unsafe extern "C" fn dispose_falling_sand_handle(self){
        drop(self)
    }
}
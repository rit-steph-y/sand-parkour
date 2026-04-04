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
    pub unsafe extern "C" fn test_falling_sand_handle(&mut self) -> Vec<i32>{
        vec![0,1,7,9,10]
    }
    #[unsafe(no_mangle)]
    pub fn dispose_falling_sand_handle(self){}
}
// /**
// A shared handle that can be accessed concurrently by multiple threads.

// The interior value can be treated like `&T`.
// */
// #[repr(transparent)]
// pub struct HandleShared<T: ?Sized>(*const T);

// unsafe_impl!(
//     "The handle is semantically `&T`" =>
//     impl<T> Send for HandleShared<T>
//         where T: ?Sized + Sync
//     {
//     }
// );

// impl<T> UnwindSafe for HandleShared<T>
// where
//     T: ?Sized + RefUnwindSafe
// {
// }

// impl<T> HandleShared<T>
// where
//     T: Send + Sync + 'static
// {
//     fn alloc(value: T) -> Self {
//         let v = Box::new(value);

//         HandleShared(Box::into_raw(v))
//     }
// }

// impl<T> HandleShared<T>
// where
//     T: ?Sized + Send + Sync
// {
//     unsafe_fn!(
//         "There are no other live references and the handle won't be used again" =>
//         fn dealloc<R>(handle: Self, f: impl FnOnce(&mut T) -> R) -> R {
//             let mut v = Box::from_raw(handle.0 as *mut T);

//             f(&mut *v)
//         }
//     );
// }

// impl<T> Deref for HandleShared<T>
// where
//     T: ?Sized
// {
//     type Target = T;

//     fn deref(&self) -> &T {
//         unsafe_block!("We own the interior value" => {
//             &*self.0
//         })
//     }
// }
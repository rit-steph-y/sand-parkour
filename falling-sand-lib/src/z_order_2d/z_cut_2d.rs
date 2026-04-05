#![warn(clippy::pedantic)]
use crate::z_order_2d::z_index_2d::ZIndex;

/**
 this type represents a cut in the space represented by the Z-order curve.
 this is used to, in ideal circumstances, traverse the space of a Z-order
 curve more efficiently by skipping unneeded sections in the data.

 we still use a z-order curve since that maintains critical cache locality
 which is kind of important in a performance critical codebase.
*/
#[derive(Debug)]
#[allow(unused)]
pub struct ZCut {
    min: ZIndex,
    max: ZIndex,
}

impl ZCut {
    pub fn new_from_z(min: ZIndex, max: ZIndex) -> Self {
        Self { min, max }
    }
    pub fn min(&self) -> u64 {
        self.min.index()
    }
    pub fn max(&self) -> u64 {
        self.max.index()
    }
    pub fn highest_order(&self) -> u8 {
        let diff = self.min.index() ^ self.max.index();
        if diff == 0 {
            return 0;
        }
        diff.ilog2() as u8
    }
    pub fn highest_order_bit(&self) -> u64 {
        1 << self.highest_order()
    }
    pub fn split(self) -> Result<(Self, Self), Self> {
        let highest_order = self.highest_order();
        if highest_order < 1 {
            return Result::Err(self);
        }
        let preserve_mask = !((self.highest_order_bit() << 1) - 1);
        let reposition_mask = !preserve_mask >> 1;
        let preserve = self.min() & preserve_mask;

        let mut post_cut = ZIndex::new(self.highest_order_bit() | preserve);
        let mut pre_cut = ZIndex::new(post_cut.index() - 1);

        if reposition_mask & self.min() == 0 && reposition_mask & self.max() == reposition_mask {
            return Result::Err(self);
        }
        if highest_order.is_multiple_of(2) {
            //cut dir is x, copy min and max y
            let low_y_bits = self.min.y_bits();
            let high_y_bits = self.max.y_bits();
            unsafe {
                post_cut.set_y_bits(low_y_bits);
                pre_cut.set_y_bits(high_y_bits);
            }
        } else {
            //cut dir is y, copy min and max x
            let low_x_bits = self.min.x_bits();
            let high_x_bits = self.max.x_bits();
            unsafe {
                post_cut.set_x_bits(low_x_bits);
                pre_cut.set_x_bits(high_x_bits);
            }
        }
        Result::Ok((
            ZCut::new_from_z(self.min, pre_cut),
            ZCut::new_from_z(post_cut, self.max),
        ))
    }
    pub fn slice(self) -> Vec<Self> {
        let mut stack: Vec<Self> = vec![];
        let mut add_stack: Vec<Self> = vec![self];
        while let Some(item) = add_stack.pop() {
            stack.push(item);
            loop {
                let current_cut = stack.pop().unwrap();
                match current_cut.split() {
                    Ok((a, b)) => {
                        add_stack.push(b);
                        stack.push(a);
                    }
                    Err(i) => {
                        stack.push(i);
                        break;
                    }
                }
            }
        }
        stack
    }
}

#![warn(clippy::pedantic)]
use std::arch::x86_64::_pdep_u64;
use std::arch::x86_64::_pext_u64;
use std::fmt::Debug;
use std::fmt::Formatter;

/**
z index struct to store 2D coordinates as inherently interleaved bits.
*/
#[repr(transparent)]
#[derive(PartialEq, Eq, Clone, Copy, Hash)]
pub struct ZIndex {
    index: u64,
}

pub const X_BITS: u64 = 0x5555_5555_5555_5555;

pub const Y_BITS: u64 = 0xaaaa_aaaa_aaaa_aaaa;

impl Debug for ZIndex {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        writeln!(f, "(x:{:b},y:{:b})", self.x(), self.y())
    }
}

impl ZIndex {
    // creates a z-index using a provided x and y value (unsigned)
    pub fn from_coords(x: u32, y: u32) -> Self {
        let index = unsafe { _pdep_u64(u64::from(x), X_BITS) | _pdep_u64(u64::from(y), Y_BITS) };
        Self::new(index)
    }
    pub fn new(index: u64) -> Self {
        Self { index }
    }
    /// resets bits from x value and then replaces 
    /// them with the new bits specified.
    /// 
    /// unsafe due to not checking if bits is only in x positions
    pub unsafe fn set_x_bits(&mut self, bits: u64) {
        self.index &= Y_BITS;
        self.index |= bits;
    }
    /// resets bits from y value and then replaces 
    /// them with the new bits specified.
    /// 
    /// unsafe due to not checking if bits is only in y positions
    pub unsafe fn set_y_bits(&mut self, bits: u64) {
        self.index &= X_BITS;
        self.index |= bits;
    }
    /// masks then returns the bits in the index that correspond to x value,
    /// but does not collect them.
    pub fn x_bits(&self) -> u64 {
        self.index & X_BITS
    }
    /// masks then returns the bits in the index that correspond to y value,
    /// but does not collect them.
    pub fn y_bits(&self) -> u64 {
        self.index & Y_BITS
    }
    /// extracts the x value that this index represents.
    pub fn x(&self) -> u32 {
        (unsafe { _pext_u64(self.index, X_BITS) }) as u32
    }
    /// extracts the y value that this index represents.
    pub fn y(&self) -> u32 {
        (unsafe { _pext_u64(self.index, Y_BITS) }) as u32
    }
    /// returns the wrapped index value.
    pub fn index(&self) -> u64 {
        self.index
    }
}

#[cfg(test)]
mod tests{
    use crate::z_order_2d::z_index_2d::ZIndex;

    #[test]
    fn test_construct_from_coords(){
        let index = ZIndex::from_coords(0b10010101, 0b1001000);
        assert_eq!(0b110000110010001, index.index())
    }
}
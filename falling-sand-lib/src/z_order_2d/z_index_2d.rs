#![warn(clippy::pedantic)]
use std::arch::x86_64::_pdep_u64;
use std::arch::x86_64::_pext_u64;
use std::fmt::Debug;
use std::fmt::Formatter;

/**
z index struct to store 2D coordinates as inherently interleaved bits.
*/
#[repr(C)]
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
    //unsafe due to not checking if bits is only in x positions
    pub unsafe fn set_x_bits(&mut self, bits: u64) {
        self.index &= Y_BITS;
        self.index |= bits;
    }
    //unsafe due to not checking if bits is only in y positions
    pub unsafe fn set_y_bits(&mut self, bits: u64) {
        self.index &= X_BITS;
        self.index |= bits;
    }
    pub fn x_bits(&self) -> u64 {
        self.index & X_BITS
    }
    pub fn y_bits(&self) -> u64 {
        self.index & Y_BITS
    }
    pub fn x(&self) -> u64 {
        unsafe { _pext_u64(self.index, X_BITS) }
    }
    pub fn y(&self) -> u64 {
        unsafe { _pext_u64(self.index, Y_BITS) }
    }
    pub fn index(&self) -> u64 {
        self.index
    }
}

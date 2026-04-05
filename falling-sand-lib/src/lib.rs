// mod statements here indicate that I'm including
// code from outside of this file, in this case,
// the mod.rs under the 3 folders specified.
mod csharp_abi;
mod falling_sand_2d;
mod z_order_2d;

// #[cfg(test)] means:
// this code is only included when we are testing the program
// the following will be excluded when we are building for release
// or debugging.
#[cfg(test)]
mod tests {
    use crate::z_order_2d::z_cut_2d::ZCut;
    use crate::z_order_2d::z_index_2d::ZIndex;
    use std::time::SystemTime;

    /**
    this test verifies that the Z-order curve travelsal functions.
    it also should verify that the curve travelsal at least runs
    fast enough to flip about 10,0000,000 bools in the specified
    range from false to true.

    note: I did rip this from my experiment, this test is more
    of a sanity check than anything, as if this fails, that means
    something went seriously wrong.
    */
    #[test]
    fn main() {
        println!("Hello, world!");
        let stack = ZCut::new_from_z(
            ZIndex::from_coords(3, 3),
            ZIndex::from_coords(1024 - 1, 1024 - 1),
        )
        .slice();
        // keep a stack of cuts made
        dbg!(&stack.len());
        let mut storage = vec![];
        storage.resize(1000000000, false);

        let stack = stack;
        let start = SystemTime::now();
        for _ in 0..1 {
            for cut in &stack {
                for index in cut.min()..=cut.max() {
                    let u_size_index =
                        usize::try_from(index).expect("number too large for this platform");
                    storage[u_size_index] = !storage[u_size_index];
                }
            }
        }
        let duration = SystemTime::now().duration_since(start).unwrap();
        println!("total_duration: {}", duration.as_millis());

        let display_size = 64;
        let mut debug_display = vec![];
        debug_display.resize(display_size * display_size, false);

        for c in 0..(display_size * display_size) {
            let index = ZIndex::new(c as u64);
            debug_display[index.x() as usize + index.y() as usize * display_size] = storage[c];
        }

        for y in 0..display_size {
            print!("y:{y:^4}");
            for x in 0..display_size {
                print!(
                    " {}",
                    if debug_display[x + y * display_size] {
                        'c'
                    } else {
                        '_'
                    }
                );
            }
            println!();
        }
    }
}

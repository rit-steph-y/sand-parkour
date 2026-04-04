mod z_cut_2d;
mod z_index_2d;

#[unsafe(no_mangle)]
pub extern "C" fn lib_addition(a: u32, b: u32) -> u32 {
    a + b
}

#[cfg(test)]
mod tests{
    use crate::{z_cut_2d::ZCut, z_index_2d::ZIndex};
    use std::time::SystemTime;

    #[test]
    fn main() {
        println!("Hello, world!");
        let stack = ZCut::new_from_z(
                ZIndex::from_coords(3, 3), 
                ZIndex::from_coords(1024 - 1,1024 - 1 )
            ).slice();
        // keep a stack of cuts made
        dbg!(&stack.len());
        let mut storage = vec![];
        storage.resize(1000000000, false);

        let stack = stack;
        let start = SystemTime::now();
        for _ in 0..1{
            for cut in &stack{
                for index in cut.min()..=cut.max(){
                    let u_size_index = usize::try_from(index).expect("number too large for this platform");
                    storage[u_size_index] = !storage[u_size_index];
                }
            }
        }
        let duration = SystemTime::now().duration_since(start).unwrap();
        println!("total_duration: {}", duration.as_millis());
        
        let display_size = 64;
        let mut debug_display = vec![];
        debug_display.resize(display_size * display_size, false);

        for c in 0..(display_size * display_size){
            let index = ZIndex::new(c as u64);
            debug_display[(index.x() + index.y() * display_size as u64) as usize] = storage[c];
        }

        for y in 0..display_size{
            print!("y:{y:^4}");
            for x in 0..display_size{
                print!(" {}", if debug_display[x + y * display_size] {'c'} else {'_'});
            }
            println!();
        }
    }
}
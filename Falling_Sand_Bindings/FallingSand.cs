using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HW5_GROUP_PROJECT.extern_testing{
    class FallingSand: IDisposable
    {

        [StructLayout(LayoutKind.Sequential, Size = 8, Pack = 1)]
        private struct SandHandle
        {
        }

        [DllImport("falling_sand_lib", EntryPoint="new_sand_handle")]
        private static extern SandHandle CreateHandle();

        [DllImport("falling_sand_lib", EntryPoint ="test_sand_handle")]
        private static extern FallingSandId Test(ref SandHandle handle, ulong index);
        [DllImport("falling_sand_lib", EntryPoint ="place_tile_at_coords_sand_handle")]
        private static extern void SetTileAt(ref SandHandle handle, uint x, uint y, FallingSandId id);

        [DllImport("falling_sand_lib", EntryPoint ="dispose_sand_handle")]
        private static extern bool DisposeHandle(SandHandle handle);
        
        private SandHandle handle;

        public FallingSand()
        {
            handle = CreateHandle();
            Debugger.Log(0, null, $"{Test(ref handle, 0b1111)}\n");
            SetTileAt(ref handle, 3,3, FallingSandId.AIR);
            Debugger.Log(0, null, $"{Test(ref handle, 0b1111)}\n");
        }

        public void Dispose()
        {
            DisposeHandle(this.handle);
            this.handle = new();
        }
    }
}
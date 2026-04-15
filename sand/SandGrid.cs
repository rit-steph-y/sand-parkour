using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HW5_GROUP_PROJECT.sand
{
    public struct SandGrid
    {
        public delegate void DrawHandle(uint x, uint y, SandPixel pixel);

        public const ulong CHUNK_WIDTH = 1 << CHUNK_BITS;
        public const uint INDEX_IN_CHUNK_MASK = (1 << CHUNK_BITS_TOTAL) - 1;
        public const int CHUNK_BITS = 10;
        private const int CHUNK_BITS_TOTAL = CHUNK_BITS * 2;

        public static ZOrderIndex GetChunk(ZOrderIndex index)
        {
            return index >> CHUNK_BITS_TOTAL;
        }
        public static uint GetPosInChunk(ZOrderIndex index)
        {
            uint u = (uint)(ulong)index;
            return u & INDEX_IN_CHUNK_MASK;
        }

        private readonly Dictionary<ZOrderIndex, SandChunk> chunks;
        private ZOrderIndex min = 0;
        private ZOrderIndex max = new(1023,1023);
        private SandChunk lastOptChunk;
        private ZOrderIndex lastHash = 0;

        public SandGrid()
        {
            this.chunks = [];
            this.AddChunk(0);
            this.AddChunk(1);
            this.AddChunk(2);
            this.AddChunk(3);
        }
        private void AddChunk(ZOrderIndex z)
        {
            this.chunks[z] = new();
            this.lastOptChunk = this.chunks[z];
            this.lastHash = z;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly ZOrderIndex GetStart(byte offsetStep)
        {
            return offsetStep switch
            {
                0 => 0b11,
                1 => 0b00,
                2 => 0b01,
                _ => 0b10
            };
        }
        public void Update(in LookupTable lut, LookupTable.InterpretPixel interpret, byte offsetStep)
        {
            ZOrderIndex current = this.min + this.GetStart(offsetStep);
            while (current <= this.max)
            {
                SrcSandGroup sourceGroup;
                ZOrderIndex index = current;
                ZOrderIndex left = index.XBits();
                ZOrderIndex right = index.XBitsPlus1();
                ZOrderIndex top = index.YBits();
                ZOrderIndex bottom = index.YBitsPlus1();
                sourceGroup.TopLeft = ref this.GetPixel(top | left);
                sourceGroup.TopRight = ref this.GetPixel(top | right);
                sourceGroup.BottomLeft = ref this.GetPixel(bottom | left);
                sourceGroup.BottomRight = ref this.GetPixel(bottom | right);
                lut.Update(ref sourceGroup, interpret);
                current += 4;
            }
        }

        public void Draw(DrawHandle handle)
        {
            
            ZOrderIndex current = 0;
            while (current <= this.max)
            {
                SandPixel pixel = this.GetPixel(current);
                handle.Invoke(current.X, current.Y, pixel);
                current ++;
            }
        }

        public void SetPixel(ZOrderIndex index, PixelId id)
        {
            ref SandPixel sandPixel = ref this.GetPixel(index);
            sandPixel.id = id;
            sandPixel.flags = 0;
        }

        /**
        note: the previous implementation seemed to have severely messed
        up the performance, idk why this here just breaks,
        
        I suspect it has something to do with the fact that C# doesn't 
        have very good codegen, and/or that the code here causes a much 
        longer hold up on branching since the two return values are not 
        seen as the same, possibly the newer implementation actually jumps
        in control flow less even in release.

        alternative theory: tail call optimizations here may have been impossible
        to perform for the previous implementation, since the two return branches
        weren't the "same" value. The reason I believe this theory could be the
        case is because it seems like when profiling the release mode this function
        did not show up on the function call chart.

        ```
        if (chunkZ == this.lastHash)
        {
            return ref this.lastOptChunk.pixels[sub_z];
        }
        this.lastOptChunk = this.chunks[chunkZ];
        this.lastHash = chunkZ;
        return ref this.lastOptChunk.pixels[sub_z];
        ```
        */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref SandPixel GetPixel(ZOrderIndex index)
        {
            ToChunkAndSubpos(index, out ZOrderIndex chunkZ, out uint sub_z);
            if (chunkZ != this.lastHash)
            {
                this.lastOptChunk = this.chunks[chunkZ];
                this.lastHash = chunkZ;
            }
            return ref this.lastOptChunk.pixels[sub_z];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ToChunkAndSubpos(ZOrderIndex index, out ZOrderIndex z, out uint sub_z)
        {
            z = GetChunk(index);
            sub_z = GetPosInChunk(index);
        }
    }

    public readonly struct SandChunk
    {
        public readonly SandPixel[] pixels;
        public SandChunk()
        {
            pixels = new SandPixel[SandGrid.CHUNK_WIDTH * SandGrid.CHUNK_WIDTH];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].id = PixelId.INVALID;
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace HW5_GROUP_PROJECT.sand
{
    public struct SandGrid
    {
        public const ulong CHUNK_WIDTH = 1 << CHUNK_BITS;
        public const ulong INDEX_IN_CHUNK_MASK = (1 << CHUNK_BITS_TOTAL) - 1;
        public const int CHUNK_BITS = 10;
        private const int CHUNK_BITS_TOTAL = CHUNK_BITS * 2;

        public static ZOrderIndex GetChunk(ZOrderIndex index)
        {
            return index >> CHUNK_BITS_TOTAL;
        }
        public static ZOrderIndex GetPosInChunk(ZOrderIndex index)
        {
            return index & INDEX_IN_CHUNK_MASK;
        }

        private readonly Dictionary<ZOrderIndex, SandChunk> chunks;
        private ZOrderIndex max = new(1023,1023);
        private SandChunk lastOptChunk;
        private ZOrderIndex lastHash = 0;

        public SandGrid()
        {
            this.chunks = new();
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

        static readonly ZOrderIndex[] starts = [new(1,1),new(0,0),new(0,1),new(1,0)];

        public void Update(in LookupTable lut, LookupTable.InterpretPixel interpret, byte offsetStep)
        {
            ZOrderIndex current = starts[offsetStep];
            while (current <= this.max)
            {
                SrcSandGroup sourceGroup;
                ZOrderIndex index = current;
                ZOrderIndex left = index.XBits();
                ZOrderIndex right = index.IncrXKeepX();
                ZOrderIndex top = index.YBits();
                ZOrderIndex bottom = index.IncrYKeepY();
                // Console.Write($"{top.Y},{bottom.Y},{left.X},{right.X}");
                sourceGroup.TopLeft = ref this.GetPixel(top | left);
                sourceGroup.TopRight = ref this.GetPixel(top | right);
                sourceGroup.BottomLeft = ref this.GetPixel(bottom | left);
                sourceGroup.BottomRight = ref this.GetPixel(bottom | right);
                lut.Update(ref sourceGroup, interpret);
                // ReplaceSandGroup group = new();
                // group = new();
                // group.Apply(ref sourceGroup);
                current += 4;
            }
        }

        public void SetPixel(ZOrderIndex index, PixelId id)
        {
            this.GetPixel(index).id = id;
            this.GetPixel(index).flags = 0;
        }
        public ref SandPixel GetPixel(ZOrderIndex index)
        {
            ZOrderIndex z = GetChunk(index);
            ZOrderIndex sub_z = GetPosInChunk(index);
            if(z == this.lastHash)
            {
                return ref this.lastOptChunk.pixels[sub_z];
            }
            this.lastOptChunk = chunks[z];
            this.lastHash = z;
            return ref this.chunks[z].pixels[sub_z];
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
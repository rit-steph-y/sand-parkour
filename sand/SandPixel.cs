using System.Collections.Generic;

namespace HW5_GROUP_PROJECT.sand
{

    public enum PixelId: byte
    {
        INVALID = 0,
        AIR = 1,
        SAND = 2
    }

    public struct SandPixel
    {
        public PixelId id;
        public byte flags;
        // public short color;
    }

    public ref struct SrcSandGroup
    {
        public ref SandPixel TopLeft;
        public ref SandPixel TopRight;
        public ref SandPixel BottomLeft;
        public ref SandPixel BottomRight;
    }

    /// <summary>
    /// complete madness.
    /// </summary>
    public struct ReplaceSandGroup
    {
        ReplaceOperation TopLeft;
        ReplaceOperation TopRight;
        ReplaceOperation BottomLeft;
        ReplaceOperation BottomRight;

        public ReplaceSandGroup()
        {
            this.TopLeft = new(0);
            this.TopRight = new(1);
            this.BottomLeft = new(2);
            this.BottomRight = new(3);
        }

        /// <summary>
        /// apply this operation.
        /// </summary>
        /// <param name="src">source sand group</param>
        public void Apply(ref SrcSandGroup src)
        {
            SandPixel newTopLeft = this.TopLeft.Apply(ref src);
            SandPixel newTopRight = this.TopRight.Apply(ref src);
            SandPixel newBottomLeft = this.BottomLeft.Apply(ref src);
            SandPixel newBottomRight = this.BottomRight.Apply(ref src);
            src.TopLeft = newTopLeft;
            src.TopRight = newTopRight;
            src.BottomLeft = newBottomLeft;
            src.BottomRight = newBottomRight;
        }
    }

    public struct LookupTable
    {
        byte cellTypes;
        ReplaceSandGroup[] lookup;
        public LookupTable(byte types)
        {
            this.cellTypes = types;
            // raise types to power of 4 for size of the replace LUT
            int total = types;
            total *= total;
            total *= total;
            lookup = new ReplaceSandGroup[total];
        }

        public void Update(ref SrcSandGroup src, InterpretPixel interpret)
        {
            int i = interpret.Invoke(src.BottomRight);
            i *= this.cellTypes;
            i += interpret.Invoke(src.BottomLeft);
            i *= this.cellTypes;
            i += interpret.Invoke(src.TopRight);
            i *= this.cellTypes;
            i += interpret.Invoke(src.TopLeft);
            this.lookup[i].Apply(ref src);
        }
    }

    // public delegate void UpdateFunction();
    public delegate LookupTable LookupTableTypeSupplier();
    public delegate byte InterpretPixel(in SandPixel pixel);

    public struct SandGrid
    {
        public const ulong CHUNK_WIDTH = 1 << CHUNK_BITS;
        public const ulong INDEX_IN_CHUNK_MASK = CHUNK_BITS * 2 - 1;
        public const int CHUNK_BITS = 10;
        public static ZOrderIndex GetChunk(ZOrderIndex index)
        {
            return index >> (CHUNK_BITS * 2);
        }
        public static ZOrderIndex GetPosInChunk(ZOrderIndex index)
        {
            return index & INDEX_IN_CHUNK_MASK;
        }

        private readonly Dictionary<ZOrderIndex, SandChunk> chunks;
        private ZOrderIndex current = 0;
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


        public void Update(in LookupTable lut, InterpretPixel interpret, byte offsetStep)
        {
            this.current = starts[offsetStep];
            while (this.current <= this.max)
            {
                SrcSandGroup sourceGroup;
                ZOrderIndex index = this.current;
                ZOrderIndex left = index.XBits();
                ZOrderIndex right = index.IncrXKeepX();
                ZOrderIndex top = index.YBits();
                ZOrderIndex bottom = index.IncrYKeepY();
                sourceGroup.TopLeft = ref this.GetPixel(top | left);
                sourceGroup.TopRight = ref this.GetPixel(top | right);
                sourceGroup.BottomLeft = ref this.GetPixel(bottom | left);
                sourceGroup.BottomRight = ref this.GetPixel(bottom | right);
                lut.Update(ref sourceGroup, interpret);
                this.current += 4;
            }
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
        }
    }
}
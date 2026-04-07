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
        public ReplaceOperation TopLeft;
        public ReplaceOperation TopRight;
        public ReplaceOperation BottomLeft;
        public ReplaceOperation BottomRight;

        public ReplaceSandGroup(): this(new(0), new(1), new(2), new(3))
        {
        }

        public ReplaceSandGroup(ReplaceOperation tl, ReplaceOperation tr, ReplaceOperation bl, ReplaceOperation br)
        {
            this.TopLeft = tl;
            this.TopRight = tr;
            this.BottomLeft = bl;
            this.BottomRight = br;
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
            this.lookup = new ReplaceSandGroup[total];
        }

        public delegate ReplaceSandGroup GroupGen(byte tl, byte tr, byte bl, byte br);

        public void Build(GroupGen builder){
            for(byte tl = 0; tl < 3; tl++)
            {
                for(byte tr = 0; tr < 3; tr++)
                {
                    for(byte bl = 0; bl < 3; bl++)
                    {
                        for(byte br = 0; br < 3; br++)
                        {
                            this.SetLookup(tl, tr, bl, br, builder.Invoke(tl, tr, bl, br));
                        }
                    }
                }
            }
        }

        public void Update(ref SrcSandGroup src, InterpretPixel interpret)
        {
            uint i = interpret.Invoke(src.BottomRight);
            i *= this.cellTypes;
            i += interpret.Invoke(src.BottomLeft);
            i *= this.cellTypes;
            i += interpret.Invoke(src.TopRight);
            i *= this.cellTypes;
            i += interpret.Invoke(src.TopLeft);
            this.lookup[i].Apply(ref src);
        }

        public void SetLookup(byte tl, byte tr, byte bl, byte br, ReplaceSandGroup group)
        {
            uint i = br;
            i *= this.cellTypes;
            i += bl;
            i *= this.cellTypes;
            i += tr;
            i *= this.cellTypes;
            i += tl;
            this.lookup[i] = group;
        }
    }
    public delegate byte InterpretPixel(in SandPixel pixel);
}
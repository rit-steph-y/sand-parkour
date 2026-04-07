namespace HW5_GROUP_PROJECT.sand
{
    public struct LookupTable
    {
        public delegate byte InterpretPixel(in SandPixel pixel);
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
}
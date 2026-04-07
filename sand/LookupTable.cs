namespace HW5_GROUP_PROJECT.sand
{
    public struct LookupTable
    {
        public delegate byte InterpretPixel(in SandPixel pixel);
        private readonly byte cellTypes;
        private readonly uint cellTypesSquared;
        private ReplaceSandGroup[] lookup;
        public LookupTable(byte types)
        {
            this.cellTypes = types;
            this.cellTypesSquared = this.cellTypes;
            this.cellTypesSquared *= this.cellTypes;
            // raise types to power of 4 for size of the replace LUT
            int total = types;
            total *= total;
            total *= total;
            this.lookup = new ReplaceSandGroup[total];
        }

        public delegate ReplaceSandGroup GroupGen(byte tl, byte tr, byte bl, byte br);

        public void Build(GroupGen builder){
            for(byte tl = 0; tl < this.cellTypes; tl++)
            {
                for(byte tr = 0; tr < this.cellTypes; tr++)
                {
                    for(byte bl = 0; bl < this.cellTypes; bl++)
                    {
                        for(byte br = 0; br < this.cellTypes; br++)
                        {
                            this.SetLookup(tl, tr, bl, br, builder.Invoke(tl, tr, bl, br));
                        }
                    }
                }
            }
        }

        public void Update(ref SrcSandGroup src, InterpretPixel interpret)
        {
            uint i = this.Lookup(
                interpret.Invoke(src.TopLeft),
                interpret.Invoke(src.TopRight),
                interpret.Invoke(src.BottomLeft),
                interpret.Invoke(src.BottomRight)
            );
            this.lookup[i].Apply(ref src);
        }

        public void SetLookup(byte tl, byte tr, byte bl, byte br, ReplaceSandGroup group)
        {
            this.lookup[this.Lookup(tl,tr,bl,br)] = group;
        }
        
        private readonly uint Lookup(byte tl, byte tr, byte bl, byte br)
        {
            uint lower = (uint)br * this.cellTypes + bl;
            uint upper = (uint)tr * this.cellTypes + tl;
            return lower * this.cellTypesSquared + upper;
        }
    }
}
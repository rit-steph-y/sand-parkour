namespace HW5_GROUP_PROJECT.sand
{
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
        public readonly void Apply(ref SrcSandGroup src)
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
}
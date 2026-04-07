using System;

namespace HW5_GROUP_PROJECT.sand{
    /**
    z index struct to store 2D coordinates as inherently interleaved bits.
    */
    public struct ZOrderIndex
    {
        ulong index;

        public ZOrderIndex(ulong i)
        {
            this.index = i;
        }
        public ZOrderIndex(uint x, uint y)
        {
            this.index = 
                System.Runtime.Intrinsics.X86.Bmi2.X64.ParallelBitDeposit(x, X_BITS) |  
                System.Runtime.Intrinsics.X86.Bmi2.X64.ParallelBitDeposit(y, Y_BITS); 
        }

        public const ulong X_BITS = 0x5555_5555_5555_5555;

        public const ulong Y_BITS = 0xaaaa_aaaa_aaaa_aaaa;
        public static implicit operator ulong(ZOrderIndex index) => index.index;
        public static implicit operator ZOrderIndex(ulong u) => new(u);

        public static ZOrderIndex operator <<(ZOrderIndex index, int amount) => new((ulong)index << amount);
        public static ZOrderIndex operator >>(ZOrderIndex index, int amount) => new((ulong)index >> amount);
        

        public readonly ZOrderIndex YBits()
        {
            return this.index & Y_BITS;
        }

        public readonly ZOrderIndex XBits()
        {
            return this.index & X_BITS;
        }
        
        public readonly ZOrderIndex IncrX()
        {
            return this.index & Y_BITS | this.IncrXKeepX();
        }
        public readonly ZOrderIndex IncrXKeepX()
        {
            return ((this.index | Y_BITS) + 1) & X_BITS;
        }
        public readonly ZOrderIndex DecrX()
        {
            return this.index & Y_BITS | (((this.index & X_BITS) - 1) & X_BITS);
        }
        public readonly ZOrderIndex IncrY()
        {
            return this.index & X_BITS | IncrYKeepY();
        }
        public readonly ZOrderIndex IncrYKeepY()
        {
            return ((this.index | X_BITS) + 1) & Y_BITS;            
        }
        public readonly ZOrderIndex DecrY()
        {
            return this.index & X_BITS | (((this.index & Y_BITS) - 1) & Y_BITS);
        } 
    }
}
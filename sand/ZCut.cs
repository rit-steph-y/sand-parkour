namespace HW5_GROUP_PROJECT.sand
{
    public struct ZCut
    {

        public ZCut(ZOrderIndex min, ZOrderIndex max)
        {
            this.min = min;
            this.max = max;
        }
        public ZCut(){}
        public ZOrderIndex min {get; private set;}
        public ZOrderIndex max {get; private set;}

        private readonly ulong HighestOrderDiff() {
            ulong diff = this.min ^ this.max;

            diff |= diff >> 1;
            diff |= diff >> 2;
            diff |= diff >> 4;
            diff |= diff >> 8;
            diff |= diff >> 16;
            diff |= diff >> 32;

            return diff ^ (diff >> 1);
        }
        public bool Split(out ZCut cut){
            cut = new();
            ZOrderIndex highest_order_bit = this.HighestOrderDiff();
            if (highest_order_bit == 0) {
                return false;
            }

            ulong reposition_mask = highest_order_bit - 1;
            ulong preserve_mask = (~reposition_mask) << 1;
            ulong preserved_bits = this.min & preserve_mask;

            ZOrderIndex post_cut = highest_order_bit | preserved_bits;
            ZOrderIndex pre_cut = post_cut - 1;

            if ((reposition_mask & this.min) == 0 && 
                (reposition_mask & this.max) == reposition_mask) {
                return false;
            }
            if (highest_order_bit.YBits() == 0) {
                //cut dir is x, copy min and max y
                ZOrderIndex low_y_bits = this.min.YBits();
                ZOrderIndex high_y_bits = this.max.YBits();
                //not checked
                post_cut.SetYBits(low_y_bits);
                pre_cut.SetYBits(high_y_bits);
                //end not checked
            } else {
                //cut dir is y, copy min and max x
                ZOrderIndex low_x_bits = this.min.XBits();
                ZOrderIndex high_x_bits = this.max.XBits();
                //not checked
                post_cut.SetXBits(low_x_bits);
                pre_cut.SetXBits(high_x_bits);
                //end not checked
            }
            cut.min = post_cut;
            cut.max = this.max;
            this.max = pre_cut;
            //tihs.min = this.min;
            return true;
        }
    }
}
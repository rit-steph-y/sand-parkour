using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HW5_GROUP_PROJECT.sand
{
    public class SandGridComponent
    {
        private SandGrid grid;
        private Texture2D tex;
        private LookupTable fallingSandTable;
        private byte updateCount = 0;

        // public Vector2 scale;
        // public Vector2 offset;

        public SandGridComponent(GraphicsDevice dev)
        {
            this.tex = new Texture2D(dev, 1, 1);
            this.tex.SetData(new[] {Color.White});
            this.grid = new();
            this.Init();
        }

        public void Init()
        {
            this.fallingSandTable = new(3);
            this.fallingSandTable.Build((tl, tr, bl, br) =>
            {
                bool bl_available = bl == 0;
                bool br_available = br == 0;
                ReplaceSandGroup group = new();
                if (bl_available && tl == 1)
                {
                    bl_available = false;
                    (group.TopLeft, group.BottomLeft) = (group.BottomLeft, group.TopLeft);
                }
                if (br_available && tr == 1)
                {
                    br_available = false;
                    (group.TopRight, group.BottomRight) = (group.BottomRight, group.TopRight);
                }
                if (br_available && tl == 1)
                {
                    br_available = false;
                    (group.TopLeft, group.BottomRight) = (group.BottomRight, group.TopLeft);
                }
                if (bl_available && tr == 1)
                {
                    bl_available = false;
                    (group.TopRight, group.BottomLeft) = (group.BottomLeft, group.TopRight);
                }
                return group;
            });

            uint xRange = 800;
            uint yRange = 500;

            Random r = new(0);
            for(uint x = 0; x < xRange; x++)
            {
                for(uint y = 0; y < yRange; y++)
                {
                    grid.SetPixel(new(x + 1,y + 1), r.Next(2) == 0? PixelId.AIR: PixelId.SAND);
                }
            }
        }

        byte FallingSandInterpret(in SandPixel pixel)
        {
            return  pixel.id == PixelId.INVALID?    (byte)2: 
                    pixel.id == PixelId.SAND?       (byte)1: 
                                                    (byte)0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw(SpriteBatch batch)
        {
            this.grid.Draw((x,y,pixel) =>
            {
                if (pixel.id == PixelId.SAND)
                batch.Draw(this.tex, new Rectangle(new((int)x,(int)y), new(1,1)), Color.SandyBrown);
            });
        }

        public void Update()
        {
            grid.Update(this.fallingSandTable, this.FallingSandInterpret, this.updateCount);
            this.updateCount ++;
            this.updateCount %= 4;
        }

        public bool IsSolid(uint x, uint y)
        {
            return this.grid.GetPixel(new(x,y)).id == PixelId.SAND;
        }
    }
}
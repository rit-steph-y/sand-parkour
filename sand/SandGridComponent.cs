using System;
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

        public int xScale = 1;
        public int yScale = 1;
        public int xOffset = 100;
        public int yOffset = 100;

        /// <summary>
        /// the sand grid component
        /// </summary>
        /// <param name="dev">device to init sand tile texture on.</param>
        public SandGridComponent(GraphicsDevice dev)
        {
            this.tex = new Texture2D(dev, 1, 1);
            this.tex.SetData(new[] {Color.White});
            this.grid = new();
            this.Init();
        }

        /// <summary>
        /// initializes the falling sand LUT
        /// </summary>
        private void Init()
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

            uint xRange = 1000;
            uint yRange = 600;

            Random r = new(0);
            for (uint x = 0; x < xRange; x++)
            {
                for (uint y = 0; y < yRange; y++)
                {
                    grid.SetPixel(new(x + 1, y + 1), r.Next(2) == 0 ? PixelId.AIR : PixelId.SAND);
                }
            }
        }

        /// <summary>
        /// map sand pixels to byte values used by the LUT
        /// </summary>
        /// <param name="pixel">pixel to convert</param>
        /// <returns>byte value to use</returns>
        private byte FallingSandInterpret(in SandPixel pixel)
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
                {
                    int x1 = (int)x * this.xScale - this.xOffset;
                    int y1 = (int)y * this.yScale - this.yOffset;
                    batch.Draw(this.tex, new Rectangle(new(x1, y1), new(xScale, yScale)), Color.SandyBrown);
                }
            });
        }

        /// <summary>
        /// updates the state of the current board
        /// </summary>
        public void Update()
        {
            grid.Update(this.fallingSandTable, this.FallingSandInterpret, this.updateCount);
            this.updateCount ++;
            this.updateCount %= 4;
        }

        /// <summary>
        /// method to check if a tile in the grid is a solid tile.
        /// </summary>
        /// <param name="x">x coordinate to check</param>
        /// <param name="y">y coordinate to check</param>
        /// <returns>if the tile is solid</returns>
        public bool IsSolid(uint x, uint y)
        {
            return this.grid.GetPixel(new(x,y)).id == PixelId.SAND;
        }

        /// <summary>
        /// sets a pixel in the targeted positions
        /// </summary>
        /// <param name="x">x to set</param>
        /// <param name="y">y to set</param>
        /// <param name="id">id to set tile to.</param>
        public void SetPixel(uint x, uint y, PixelId id)
        {
            this.grid.SetPixel(new(x,y), id);
        }
    }
}
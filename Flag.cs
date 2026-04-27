using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HW5_GROUP_PROJECT
{
    internal class Flag
    {
        internal static Texture2D FlagTexture { get; set; }

        private int width 
        {
            get
            {
                return FlagTexture.Width;
            }
        }
        private int height
        {
            get
            {
                return FlagTexture.Height;
            }
        }

        private Vector2 position;
        private Vector2 bottomRight => position + new Vector2(width, height);

        internal Flag(Vector2 pos) 
        { 
            position = pos;
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(FlagTexture, camera.FromWorldSpaceRect(position, bottomRight), Color.White);
        }
    }
}

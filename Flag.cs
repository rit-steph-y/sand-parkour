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
        private Rectangle rect;

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

            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(FlagTexture, camera.FromWorldSpaceRect(position, bottomRight), Color.White);
        }

        internal void Update(Player player, SandGame game)
        {
            if (rect.Contains(player.GetTopLeftCorner()) || rect.Contains(player.GetBottomLeftCorner()) ||
                rect.Contains(player.GetTopRightCorner()) || rect.Contains(player.GetBottomRightCorner()))
            {
                game.NextLevel();
            }
        }
    }
}

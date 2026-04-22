using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW5_GROUP_PROJECT.UI
{
    internal class Background : UIElement
    {
        internal Background(Texture2D tex, Microsoft.Xna.Framework.Color color) : base(tex, origin, color) {}
    }
}

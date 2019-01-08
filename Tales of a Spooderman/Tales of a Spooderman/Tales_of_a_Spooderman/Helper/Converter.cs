using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tales_of_a_Spooderman.Helper
{
    public static class Converter
    {
        public static Rectangle[] ConvertRectangleToLines(Rectangle rec)
        {
            Rectangle[] rectangleLines = new Rectangle[4];
            const int RECTWIDTH = 3;
            const int RECTHEIGHT = 3;

            rectangleLines[0] = new Rectangle(rec.X, rec.Y, rec.Width, RECTHEIGHT);
            rectangleLines[1] = new Rectangle(rec.X, rec.Bottom - RECTHEIGHT, rec.Width, RECTHEIGHT);
            rectangleLines[2] = new Rectangle(rec.X, rec.Y, RECTWIDTH, rec.Height);
            rectangleLines[3] = new Rectangle(rec.Right - RECTWIDTH, rec.Y, RECTWIDTH, rec.Height);

            return rectangleLines;
        }
    }
}

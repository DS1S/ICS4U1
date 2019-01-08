using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tales_of_a_Spooderman.Core.Player
{
    static class Camera
    {
        private static Vector2 position;
        private static Matrix screenMatrix = Matrix.Identity;

        public static void Update(GameTime gameTime, Rectangle gameObject, Game game)
        {
            position.X += (gameObject.X - position.X) - (game.GraphicsDevice.Viewport.Width / 2);

            if (position.X <= 0) position.X = 1;            
            if (position.X >= game.GraphicsDevice.Viewport.Width) position.X = game.GraphicsDevice.Viewport.Width - 1;

            screenMatrix = Matrix.CreateTranslation(-position.X, 0, 0);
        }

        public static Matrix GetCameraMatrix()
        {
            return screenMatrix;           
        }

        public static void SetDefaultCameraMatrix()
        {
            screenMatrix = Matrix.Identity;
        }
    }
}

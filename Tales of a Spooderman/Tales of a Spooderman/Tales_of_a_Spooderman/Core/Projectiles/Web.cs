using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tales_of_a_Spooderman.Handlers;

namespace Tales_of_a_Spooderman.Core.Projectiles
{
    class Web : Projectile
    {
        private Vector2 pos;

        public Web(Rectangle _gameObjectRectangle, string _gameObjectTag, GameObjectHandler _hanlder, Game game, float _deathspan, float _velocity, Texture2D _projectileTextures) : base(_gameObjectRectangle, _gameObjectTag, _hanlder, game, _deathspan, _velocity, _projectileTextures)
        {
            pos = new Vector2(initalPos.X, initalPos.Y - projectileTexture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(projectileTexture, pos, new Rectangle(0,0, projectileTexture.Width, projectileTexture.Height), Color.White, 0f, new Vector2(0,0), 1f, direction,0);
        }

        public override void Update(GameTime gameTime, GamePadState pad, GamePadState oldpad)
        {
            gameObjectRectangle.Y -= (int)velocity;
            gameObjectRectangle.X += (int)velocity;
            SelfDestruct(gameTime);
        }
    }
}

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
    class AttackWeb : Projectile
    {
        float rotation;

        public AttackWeb(Rectangle _gameObjectRectangle, string _gameObjectTag, GameObjectHandler _hanlder, Game game, float _deathspan, float _velocity, Texture2D _projectileTexture) : base(_gameObjectRectangle, _gameObjectTag, _hanlder, game, _deathspan, _velocity, _projectileTexture)
        {
            this.rotation = (float)Math.PI / 4;
            damage = 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 Pos = new Vector2(gameObjectRectangle.X, gameObjectRectangle.Y);
            Vector2 origin = new Vector2(projectileTexture.Width / 2, projectileTexture.Height / 2);

            spriteBatch.Draw(projectileTexture, Pos, new Rectangle(0,0, projectileTexture.Width, projectileTexture.Height) , Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime, GamePadState pad, GamePadState oldpad)
        {
            SelfDestruct(gameTime);
            gameObjectRectangle.X += (int)velocity;
            CheckForCollision("goblin", damage);                    
        }
    }
}

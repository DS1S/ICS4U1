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
    class Bomb : Projectile
    {
        Animation bombAnim;

        public Bomb(Rectangle _gameObjectRectangle, string _gameObjectTag, GameObjectHandler _hanlder, Game game, float _deathspan, float _velocity, Texture2D _projectileTexture, Animation animation) : base(_gameObjectRectangle, _gameObjectTag, _hanlder, game, _deathspan, _velocity, _projectileTexture)
        {
            SetBombAnim(animation);
            damage = 15;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            bombAnim.Draw(spriteBatch, gameObjectRectangle, Color.White);
        }

        public override void Update(GameTime gameTime, GamePadState pad, GamePadState oldpad)
        {
            SelfDestruct(gameTime);
            bombAnim.Animate(gameTime, true);
            gameObjectRectangle.X += (int)velocity;
            CheckForCollision("player", damage);
        }

        private void SetBombAnim(Animation animation)
        {
            bombAnim = animation;
        }
    }
}

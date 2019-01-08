using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tales_of_a_Spooderman.Handlers;
using Microsoft.Xna.Framework.Graphics;

namespace Tales_of_a_Spooderman.Core.Projectiles
{
    abstract class Projectile : GameObject
    {
        protected Vector2 initalPos;

        protected float lifespan;
        protected float TTD;
        protected float velocity;

        protected int damage;

        protected SpriteEffects direction;

        protected Texture2D projectileTexture;

        public Projectile(Rectangle _gameObjectRectangle, string _gameObjectTag, GameObjectHandler _hanlder, Game game, float _deathspan, float _velocity, Texture2D _projectileTexture) : base(_gameObjectRectangle, _gameObjectTag, _hanlder, game)
        {
            initalPos.X = gameObjectRectangle.Location.X;
            initalPos.Y = gameObjectRectangle.Location.Y;
            SetupVelocity(_velocity);
            SetLifeSpan(_deathspan);
            SetProjectImg(_projectileTexture);
        }

        private void SetupVelocity(float velocity)
        {
            if (velocity < 0)
            {
                direction = SpriteEffects.FlipHorizontally;
            }
            else if (velocity >= 0)
            {
                direction = SpriteEffects.None;
            }

            this.velocity = velocity;
        }

        private void SetLifeSpan(float _deathspan)
        {
            this.TTD = Math.Abs(_deathspan);
        }

        private void SetProjectImg(Texture2D projectileTexture)
        {
            this.projectileTexture = projectileTexture;
        }

        protected virtual void SelfDestruct(GameTime gameTime)
        {
            lifespan += gameTime.ElapsedGameTime.Milliseconds;

            if(lifespan >= TTD)
            {
                handler.Remove(this);
            }
        }

        protected virtual void CheckForCollision(string charactertargetName, int damage)
        {
            Character target = (Character)handler.GetGameObject(charactertargetName);

            if (gameObjectRectangle.Intersects(target.GetGameObjectRectangle()))
            {
                target.TakeDamage(damage);
                handler.Remove(this);
            }
        }
    }
}

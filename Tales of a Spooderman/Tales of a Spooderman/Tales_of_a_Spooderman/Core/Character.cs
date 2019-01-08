using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tales_of_a_Spooderman.Handlers;

namespace Tales_of_a_Spooderman.Core
{
    abstract class Character : GameObject
    {
        protected int velocity;
        protected int health;

        protected float yLimit;

        protected Animation[] animations;
        protected AnimationHandler animationHandler;

        protected Texture2D ProjectileTexture;
        protected Texture2D characterIcon;

        protected SpriteEffects direction;

        protected bool isDead;

        public Character(Rectangle _gameObjectRectangle, string _gameObjectTag, GameObjectHandler handler, Animation[] _animations, Game game, int _velocity, int _health, int yLimit, Texture2D characterIcon) 
            : base(_gameObjectRectangle, _gameObjectTag, handler, game)
        {
            SetCharacterVelocity(_velocity);
            SetCharacterHealth(_health);
            SetCharacterAnimations(_animations);
            SetCharacterYLimit(yLimit);
            SetCharacterIcon(characterIcon);
            animationHandler = new AnimationHandler(animations);
            isDead = false;
        }

        private void SetCharacterVelocity(int speed)
        {
            if(Math.Abs(speed) <= 10)
            {
                this.velocity = Math.Abs(speed);
            }                   
        }

        private void SetCharacterHealth(int health)
        {
            if(Math.Abs(health) % 25 == 0)
            {
                this.health = Math.Abs(health);
            }
        }

        private void SetCharacterAnimations(Animation[] animations)
        {
            if(animations != null)
            {
                this.animations = animations;
            }
        }

        private void SetCharacterYLimit(int limit)
        {
            yLimit = limit;
        }

        public int GetCharacterHealth()
        {
            return health;
        }

        public Animation[] GetAnimations()
        {
            return animations;
        }

        public void SetProjectileTexture(Texture2D texture)
        {
            ProjectileTexture = texture;
        }

        public float GetYLimit()
        {
            return yLimit;
        }

        public float GetVelocity()
        {
            return velocity;
        }

        public AnimationHandler GetAnimantionHandler()
        {
            return animationHandler;
        }

        public Texture2D GetCharacterIcon()
        {
            return characterIcon;
        }

        public bool GetCharacterHealthState()
        {
            return isDead;
        }

        private void SetCharacterIcon(Texture2D icon)
        {
            this.characterIcon = icon;
        }

        protected void CheckIfDead()
        {
            if(health <= 0)
            {
                isDead = true;
            }
        }

        public abstract void TakeDamage(int damage);
    }
}

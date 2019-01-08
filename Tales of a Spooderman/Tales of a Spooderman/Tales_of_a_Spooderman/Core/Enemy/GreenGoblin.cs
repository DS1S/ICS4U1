using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tales_of_a_Spooderman.Handlers;
using Tales_of_a_Spooderman.Core.Player;
using Tales_of_a_Spooderman.Core.Projectiles;

namespace Tales_of_a_Spooderman.Core.Enemy
{
    class GreenGoblin : Character
    {

        private Character target;
        private Random rng;
        private float elapsedTime;
        private int range;
        private int walkAwayTime;
        private SpriteEffects initalDirection;
        private EnemyStates state;
    
        public GreenGoblin(Rectangle _gameObjectRectangle, string _gameObjectTag, GameObjectHandler handler, Animation[] _animations, Game game, int _velocity, int _health, int yLimit, Texture2D characterIcon) : base(_gameObjectRectangle, _gameObjectTag, handler, _animations, game, _velocity, _health, yLimit, characterIcon)
        {
            Init();
        }

        private void Init()
        {
            target = (Character)handler.GetGameObject("player");
            animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.IDLE));
            rng = new Random();
            range = rng.Next(80,130);
            state = EnemyStates.NAVIGATING;
            walkAwayTime = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animationHandler.Draw(spriteBatch, gameObjectRectangle, direction);
        }

        public override void Update(GameTime gameTime, GamePadState pad, GamePadState oldpad)
        {
            animationHandler.Update(gameTime);
            target = (Character)handler.GetGameObject("player");
            CheckIfDead();
            SelectDesicsion(gameTime);
        }

        public override void TakeDamage(int damage)
        {
            health -= damage;
        }

        private void NavigatePath(GameTime gameTime)
        {
            int diffInDistance = CalculateDifferenceInDistance();

            WalkTowards(diffInDistance);

            if(Math.Abs(diffInDistance) <= range)
            {
                state = EnemyStates.SHOOTING;
                range = rng.Next(80, 300);
                Console.WriteLine(range);
            }            
        }

        private void WalkTowards(int diffInDistance)
        {
            if (diffInDistance < 0)
            {
                direction = SpriteEffects.FlipHorizontally;
                gameObjectRectangle.X -= velocity;
                if (animationHandler.GetCurrentAnimationName() == AnimationStates.IDLE || animationHandler.GetCurrentAnimationName() == AnimationStates.SHOOT)
                {
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.WALK));
                }
            }
            else if (diffInDistance > 0)
            {
                direction = SpriteEffects.None;
                gameObjectRectangle.X += velocity;
                if (animationHandler.GetCurrentAnimationName() == AnimationStates.IDLE || animationHandler.GetCurrentAnimationName() == AnimationStates.SHOOT)
                {
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.WALK));
                }
            }
        }

        private void WalkAway(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime <= walkAwayTime)
            {
                if(animationHandler.GetCurrentAnimationName() != AnimationStates.WALK)
                {
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.WALK));
                }

                if (initalDirection == SpriteEffects.FlipHorizontally)
                {
                    direction = SpriteEffects.None;    
                    gameObjectRectangle.X += velocity;
                }
                else if (initalDirection == SpriteEffects.None)
                {
                    direction = SpriteEffects.FlipHorizontally;
                    gameObjectRectangle.X -= velocity;
                }

                if(gameObjectRectangle.Left <= 0 || gameObjectRectangle.Right >= game.GraphicsDevice.Viewport.Width * 2)
                {
                    state = EnemyStates.NAVIGATING;
                }
                
            }
            else
            {
                state = EnemyStates.NAVIGATING;
                elapsedTime = 0;
            }
        }

        private void ShootEnemy(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;


            if (animationHandler.GetCurrentAnimationName() != AnimationStates.SHOOT)
            {
                animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.SHOOT));
            }

            if (animationHandler.GetCurrentAnimationName() == AnimationStates.SHOOT)
            {
                if (elapsedTime >= animationHandler.GetCurrentAnimation().GetAnimationTimeSpan())
                {
                    elapsedTime = 0;
                    GenerateBomb();
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.IDLE));
                    state = EnemyStates.MOVINGAWAY;
                    walkAwayTime = rng.Next(300, 2000);
                    initalDirection = direction;
                }
            }          
        }

        private int CalculateDifferenceInDistance()
        {
            if (direction == SpriteEffects.None)
            {
                return target.GetGameObjectRectangle().X - gameObjectRectangle.Right;
            }
            else if (direction == SpriteEffects.FlipHorizontally)
            {
                return target.GetGameObjectRectangle().Right - gameObjectRectangle.X;
            }

            return 0;
        }

        private void SelectDesicsion(GameTime gameTime)
        {
            if(state == EnemyStates.NAVIGATING)
            {
                NavigatePath(gameTime);
            }
            else if(state == EnemyStates.SHOOTING)
            {
                ShootEnemy(gameTime);
            }
            else if(state == EnemyStates.MOVINGAWAY)
            {
                WalkAway(gameTime);
            }
        }

        private void GenerateBomb()
        {
            Vector2 Position = Vector2.Zero;
            Rectangle bombRect = Rectangle.Empty;
            int deathSpan = 1500;
            int tempVelo = 15;

            if (direction == SpriteEffects.FlipHorizontally)
            {
                tempVelo *= -1;
                Position = new Vector2(gameObjectRectangle.X, gameObjectRectangle.Y + 20);
            }
            else
            {
                Position = new Vector2(gameObjectRectangle.Center.X, gameObjectRectangle.Y + 20);
            }

            bombRect = new Rectangle((int)Position.X, (int)Position.Y, 30, 30);


            Bomb web = new Bomb(bombRect, "Bomb", handler, game, deathSpan, tempVelo, null, animationHandler.CheckAnimation(AnimationStates.BOMB));

            handler.Add(web);

        }
    }
}

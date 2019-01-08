using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tales_of_a_Spooderman.Handlers;
using Microsoft.Xna.Framework.Input;
using Tales_of_a_Spooderman.Core.Projectiles;

namespace Tales_of_a_Spooderman.Core.Player
{
    class Spiderman : Character
    {       
        private Rectangle punchRect;

        private const float GRAVITY = 11f;
        private float attackTimer;
        private float upperForce;
        private float swingTimer;
        private float sideForce;
        private float SpecialAttack;
        private float initalY;

        private int Ammo;

        private bool canMove;
        private bool isSwinging;
        private bool isJumping;
        private bool canSwing;
    
        public Spiderman(Rectangle _gameObjectRectangle, string _gameObjectTag, GameObjectHandler handler, Animation[] _animations, Game game, int _velocity, int _health, int ylimit, Texture2D characterIcon) 
            : base(_gameObjectRectangle, _gameObjectTag, handler, _animations, game, _velocity, _health, ylimit, characterIcon)
        {        
            Init();
        }
        
        private void Init()
        {
            direction = SpriteEffects.None;
            punchRect = new Rectangle(0, 0, 5, 5);

            attackTimer = 0f;
            upperForce = 0f;
            swingTimer = 0f;
            SpecialAttack = 0;
            Ammo = 0;

            initalY = yLimit - gameObjectRectangle.Height - 1;

            canMove = true;
            isJumping = false;
            isSwinging = false;
            canSwing = false;

            animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.IDLE));
        }

        public override void Update(GameTime gameTime, GamePadState pad, GamePadState oldpad)
        {
            attackTimer += gameTime.ElapsedGameTime.Milliseconds;
            swingTimer += gameTime.ElapsedGameTime.Milliseconds;

            animationHandler.Update(gameTime);
            Camera.Update(gameTime, gameObjectRectangle, game);
            HandleMovement(pad, oldpad);
            HandleAttacks(pad, oldpad);
            UpdateSpecialAttack();
            CheckCollision();
            CheckIfDead();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animationHandler.Draw(spriteBatch, gameObjectRectangle, direction);
        }

        public override void TakeDamage(int damage)
        {
            health -= damage;
        }

        private void UpdateSpecialAttack()
        {
            if (SpecialAttack < 100)
            {
                SpecialAttack += 0.1f;
            }
            else if (SpecialAttack >= 100 && Ammo < 3)
            {
                SpecialAttack = 0;
                Ammo++;
            }
        }

        private void HandleAttacks(GamePadState pad, GamePadState oldpad)
        {
            if(!isJumping && !isSwinging)
            {
                CheckForPunch(pad, oldpad);
                CheckForShoot(pad, oldpad);
            }
        }

        private void HandleMovement(GamePadState pad, GamePadState oldpad)
        {
            if (canMove)
            {
                CheckForRun(pad, oldpad);
                CheckForJump(pad, oldpad);
            }

            CheckForSwing(pad, oldpad);

            if (isSwinging)
            {
                Swing();
            }
            else if (isJumping)
            {
                Jump();
            }
        }

        private void CheckForPunch(GamePadState pad, GamePadState oldpad)
        {
            if (pad.Buttons.RightShoulder == ButtonState.Pressed && oldpad.Buttons.RightShoulder == ButtonState.Released)
            {
                if (attackTimer >= 300)
                {
                    canMove = false;
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.PUNCH));
                    attackTimer = 0;
                    if(direction == SpriteEffects.None)
                    {
                        punchRect.Location = new Point(gameObjectRectangle.Right, gameObjectRectangle.Y + 20);
                    }
                    else if(direction == SpriteEffects.FlipHorizontally)
                    {
                        punchRect.Location = new Point(gameObjectRectangle.X, gameObjectRectangle.Y + 20);
                    }
                }
            }
            else if (animationHandler.GetCurrentAnimationName().Equals(AnimationStates.PUNCH))
            {
                if (attackTimer >= (1000 / 7) * 3)
                {
                    canMove = true;
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.IDLE));
                }
            }
        }

        private void CheckForShoot(GamePadState pad, GamePadState oldpad)
        {
            if(pad.Buttons.LeftShoulder == ButtonState.Pressed && oldpad.Buttons.LeftShoulder == ButtonState.Released)
            {
                if(Ammo > 0 && attackTimer >= 1000)
                {
                    Shoot();
                    canMove = false;
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.SHOOT));
                    attackTimer = 0;
                }
            }
            else if (animationHandler.GetCurrentAnimationName().Equals(AnimationStates.SHOOT))
            {
                if (attackTimer >= animationHandler.GetCurrentAnimation().GetAnimationTimeSpan())
                {
                    canMove = true;
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.IDLE));
                }
            }
        }

        private void CheckForSwing(GamePadState pad, GamePadState oldpad)
        {
            if (pad.Buttons.LeftShoulder == ButtonState.Pressed && oldpad.Buttons.LeftShoulder == ButtonState.Released)
            {
                if (isJumping || isSwinging)
                {
                    if (canSwing)
                    {
                        float differenceFromSurfacePercent = ((yLimit - gameObjectRectangle.Bottom) / yLimit) * 100;

                        if (differenceFromSurfacePercent >= 5 && differenceFromSurfacePercent <= 75)
                        {
                            GenerateWeb();
                            isJumping = false;
                            isSwinging = true;
                            canSwing = false;
                            canMove = false;
                            upperForce = 24f;
                            sideForce = 7f;
                            animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.SWING));
                        }
                    }
                }
            }

            if(swingTimer >= animationHandler.GetCurrentAnimation().GetAnimationTimeSpan() / 1.5)
            {
                canSwing = true;
                swingTimer = 0;
            }
        }

        private void CheckForJump(GamePadState pad, GamePadState oldpad)
        {
            if (pad.Buttons.A == ButtonState.Pressed && oldpad.Buttons.A == ButtonState.Released)
            {
                if (!isJumping)
                {
                    animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.JUMP));
                    upperForce = 32f;
                    isJumping = true;
                    canSwing = true;
                }
            }
        }

        private void Jump()
        {
            if(gameObjectRectangle.Bottom < yLimit)
            {
                if(upperForce > 0)
                {
                    upperForce--;
                }
                                       
                gameObjectRectangle.Y -= (int)(upperForce - GRAVITY);
            }
            else
            {
                animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.IDLE));
                gameObjectRectangle.Y = (int)initalY;
                isJumping = false;
            }
        }

        private void Swing()
        {
            if(gameObjectRectangle.Bottom < yLimit)
            {

                if (upperForce > 0)
                {
                    upperForce -= 0.5f;
                }

                gameObjectRectangle.Y -= (int)(upperForce - GRAVITY);

                if (direction == SpriteEffects.None)
                {
                    gameObjectRectangle.X += (int)sideForce;

                    if (gameObjectRectangle.Right >= game.GraphicsDevice.Viewport.Width * 2)
                    {
                        int offSetAmt = Math.Abs(gameObjectRectangle.Right - game.GraphicsDevice.Viewport.Width * 2);
                        gameObjectRectangle.Offset(-offSetAmt, 0);
                    }
                }
                else if (direction == SpriteEffects.FlipHorizontally)
                {
                    gameObjectRectangle.X -= (int)sideForce;

                    if (gameObjectRectangle.X <= 0)
                    {
                        int offSetAmt = Math.Abs(gameObjectRectangle.Left - 0);
                        gameObjectRectangle.Offset(offSetAmt, 0);
                    }
                }
            }
            else
            {
                animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.IDLE));
                gameObjectRectangle.Y = (int)initalY;
                isJumping = false;
                canMove = true;
                isSwinging = false;
            }
        }

        private void CheckForRun(GamePadState pad, GamePadState oldpad)
        {
            if (pad.ThumbSticks.Left.X > 0)
            {
                if (gameObjectRectangle.Right <= game.GraphicsDevice.Viewport.Width * 2)
                {
                    gameObjectRectangle.X += velocity;
                    if (animationHandler.GetCurrentAnimationName().Equals(AnimationStates.IDLE))
                    {
                        animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.RUN));
                    }
                    direction = SpriteEffects.None;
                }
            }
            else if (pad.ThumbSticks.Left.X < 0)
            {
                if (gameObjectRectangle.Left >= game.GraphicsDevice.Viewport.X)
                {
                    gameObjectRectangle.X -= velocity;
                    if (animationHandler.GetCurrentAnimationName().Equals(AnimationStates.IDLE))
                    {
                        animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.RUN));
                    }
                    direction = SpriteEffects.FlipHorizontally;
                }
            }
            else if (animationHandler.GetCurrentAnimationName().Equals(AnimationStates.RUN))
            {
                animationHandler.SwapAnimations(animationHandler.CheckAnimation(AnimationStates.IDLE));
            }            
        }

        private void GenerateWeb()
        {
            Vector2 Position = Vector2.Zero;
            Rectangle webRect = Rectangle.Empty;
            int deathSpan = animationHandler.GetCurrentAnimation().GetAnimationTimeSpan() / 3;
            int tempVelo = 0;

            if(direction == SpriteEffects.FlipHorizontally)
            {
                tempVelo = -velocity;
                Position = new Vector2(gameObjectRectangle.X - 30, gameObjectRectangle.Y + 20);
            }
            else
            {
                tempVelo = velocity;
                Position = new Vector2(gameObjectRectangle.Right - 60, gameObjectRectangle.Y + 20);
            }

            webRect = new Rectangle((int)Position.X, (int)Position.Y, 10, 10);

            for (int i = 0; i < 10; i++)
            {
                webRect.X += 30 * Math.Sign(tempVelo);
                webRect.Y -= 30;
                Web web = new Web(webRect, "web", handler, game, deathSpan, tempVelo, ProjectileTexture);
                handler.Add(web);
            }
        }

        private void Shoot()
        {
            Vector2 Position = Vector2.Zero;
            Rectangle webRect = Rectangle.Empty;
            int deathSpan = 1500;
            int tempVelo = 10;

            if (direction == SpriteEffects.FlipHorizontally)
            {
                tempVelo *= -1;
                Position = new Vector2(gameObjectRectangle.X, gameObjectRectangle.Y + 20);
            }
            else
            {
                Position = new Vector2(gameObjectRectangle.Center.X, gameObjectRectangle.Y + 20);
            }

            webRect = new Rectangle((int)Position.X, (int)Position.Y, 100, 30);

            AttackWeb web = new AttackWeb(webRect, "attack web", handler, game, deathSpan, tempVelo, ProjectileTexture);

            handler.Add(web);
            Ammo--;
        }

        private void CheckCollision()
        {
            Character target = (Character)handler.GetGameObject("goblin");

            if (punchRect.Intersects(target.GetGameObjectRectangle()))
            {
                target.TakeDamage(3);
                punchRect.Location = new Point(0, 0);
            }
        }

        public int GetAmmoCount()
        {
            return Ammo;
        }

        public float GetSpecialAttackCount()
        {
            return SpecialAttack;
        }
    }
}

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
using Tales_of_a_Spooderman.Helper;
using Tales_of_a_Spooderman.Handlers;
using Tales_of_a_Spooderman.Screens;
using Tales_of_a_Spooderman.Core;

namespace Tales_of_a_Spooderman
{
    public class Animation
    {
        private AnimationStates animationName;

        private int rows;
        private int columns;
        private int spriteWidth;
        private int spriteHeight;
        private int numberOfFrames;
        private int currentFrame;
        private int frameDelay;
        private int elapsedTime;

        private Rectangle sourceRectangle;
        private Rectangle destRec;
        private Texture2D spriteSheet;

        public Animation(AnimationStates animationName, int rows, int columns, Texture2D spriteSheet)
        {
            SetAnimationName(animationName);
            SetSpriteSheet(spriteSheet);
            SetRows(rows);
            SetColumns(columns);
            SetFrameDelay();

            currentFrame = 0;
            sourceRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);                     
        }

        public Animation(AnimationStates animationName, int rows, int columns, int fps, Texture2D spriteSheet)
        {
            SetAnimationName(animationName);
            SetSpriteSheet(spriteSheet);
            SetRows(rows);
            SetColumns(columns);
            SetFrameDelay(fps);

            currentFrame = 0;
            sourceRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
        }

        public void Animate(GameTime gameTime, bool isLooping)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime >= frameDelay)
            {
                if (currentFrame >= numberOfFrames && isLooping)
                {
                    Reset();
                }

                if (currentFrame < numberOfFrames)
                {
                    if(columns != 1)
                    {
                        if (sourceRectangle.X == spriteSheet.Width - spriteWidth)
                        {
                            sourceRectangle.X = 0;
                            sourceRectangle.Y += spriteHeight;
                        }
                    }
                  
                    sourceRectangle.X = (currentFrame - ((sourceRectangle.Y / spriteHeight) * columns)) * spriteWidth;
                    currentFrame++;
                }

                elapsedTime = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destRec, Color color)
        {
            spriteBatch.Draw(spriteSheet, destRec, sourceRectangle, color);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destRec, Color color, SpriteEffects effect)
        {
            spriteBatch.Draw(spriteSheet, destRec, sourceRectangle, color, 0, new Vector2(0, 0), effect, 0);
        }

        public void Reset()
        {          
            sourceRectangle.Y = 0;
            sourceRectangle.X = 0;
            currentFrame = 0;
        }

        public AnimationStates GetAnimationName()
        {
            return animationName;
        }

        private void SetAnimationName(AnimationStates name)
        {
            animationName = name;
        }

        private void SetRows(int rows)
        {
            this.rows = Math.Abs(rows);

            spriteHeight = spriteSheet.Height / rows;         
        }

        private void SetColumns(int columns)
        {
            this.columns = Math.Abs(columns);

            spriteWidth = spriteSheet.Width / columns;
        }

        private void SetFrameDelay()
        {
            numberOfFrames = columns * rows;
            frameDelay = 1000 / numberOfFrames;
        }

        private void SetFrameDelay(int FPS)
        {
            numberOfFrames = columns * rows;
            frameDelay = 1000 / FPS;
        }

        private void SetSpriteSheet(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }

        private void SetDestRec(Rectangle rec)
        {
            this.destRec = rec;
        }

        public int GetAnimationTimeSpan()
        {
            return frameDelay* numberOfFrames;
        }
    }
}

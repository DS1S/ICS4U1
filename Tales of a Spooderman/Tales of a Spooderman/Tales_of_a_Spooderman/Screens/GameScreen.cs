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
using Tales_of_a_Spooderman.Handlers;

namespace Tales_of_a_Spooderman.Screens
{

    enum ScreenState
    {
        Transitioning,
        Hidden,
        Displaying,
        Overlaying,
    }

    abstract class GameScreen
    {    
        private byte alpha;
        protected Color fader;

        protected Texture2D backgroundTexture;
        protected Rectangle backgroundContainer;

        protected ContentManager content;
        protected Game1 game;
        protected ScreenHandler screenHandler;

        protected ScreenState screenState;
        protected string ScreenName;

        protected GamePadState pad;
        protected GamePadState oldpad;

        protected Song soundTrack;

        protected int currentIndex;

        public GameScreen(Game1 _game, ScreenHandler _screenHandler)
        {
            SetScreenHandler(_screenHandler);
            SetGameOfScreen(_game);
            alpha = 255;
            fader = Color.White;
            backgroundContainer = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        }

        public abstract void Init();

        public abstract void Update(GameTime gameTime);
          
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void UnloadContent();

        public void PerformTransition(GameTime gameTime)
        {
            if(alpha > 0)
            {
                if(gameTime.TotalGameTime.Ticks % 2 == 0)
                {
                    alpha-=3;
                }
                fader = new Color(0, 0, 0, alpha);
            }
            else
            {
                screenState = ScreenState.Hidden;
                alpha = 255;
                fader = Color.White;
            }
        }

        public abstract void LoadContent();


        public ScreenState GetScreenState()
        {
            return screenState;
        }

        public string GetScreenName()
        {
            return ScreenName;
        }
       
        public void SetScreenState(ScreenState screenState)
        {
            this.screenState = screenState;
        }

        private void SetGameOfScreen(Game1 game)
        {
            this.game = game;
            content = new ContentManager(game.Content.ServiceProvider);
            content.RootDirectory = "Content";
        }

        protected void SetScreenName(string name)
        {
            ScreenName = name;
        }

        protected int ChangeOptionIndex(int currentIndex, int maxIndex)
        {
            if (pad.DPad.Up == ButtonState.Pressed && oldpad.DPad.Up == ButtonState.Released)
            {
                if (currentIndex == 0)
                {
                    return 1;
                }
                else if (currentIndex != 1)
                {
                    currentIndex--;
                    return currentIndex;
                }
            }
            else if (pad.DPad.Down == ButtonState.Pressed && oldpad.DPad.Down == ButtonState.Released)
            {
                if (currentIndex == 0)
                {
                    return 1;
                }
                else if (currentIndex != maxIndex)
                {
                    currentIndex++;
                    return currentIndex;
                }
            }

            return currentIndex;
        }

        private void SetScreenHandler(ScreenHandler screenHandler)
        {
            this.screenHandler = screenHandler;
        }
    }
}

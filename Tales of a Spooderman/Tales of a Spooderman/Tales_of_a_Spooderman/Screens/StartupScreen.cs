using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tales_of_a_Spooderman.Handlers;
using Tales_of_a_Spooderman;
using System.Reflection;
using Microsoft.Xna.Framework.Input;

namespace Tales_of_a_Spooderman.Screens
{
    class StartupScreen : GameScreen
    {
        private Rectangle logoRect;
        private Rectangle startButtonRect;

        private Texture2D startButtonTexture;
        private Texture2D logoTexture;

        private Vector2 startTextPos;
        private Vector2 versionBuildPos;

        private SpriteFont startTextFont;

        private string version;

        private byte startButtonAlpha;
        private int AlphaChanger;

        public StartupScreen(Game1 game, ScreenHandler handler) : base(game, handler)
        {
            SetScreenName("Startup");
            SetScreenState(ScreenState.Displaying);
        }

        public override void Init()
        {
            const int LOGOWIDTH = 550;
            const int LOGOHEIGHT = 200;
            const int STARTBUTTONWIDTH = 22;
            const int STARTBUTTONHEIGHT = 22;

            logoRect = new Rectangle(game.GraphicsDevice.Viewport.Width / 2 - LOGOWIDTH / 2, 0, LOGOWIDTH, LOGOHEIGHT);
            startButtonRect = new Rectangle(game.GraphicsDevice.Viewport.Width / 2 - (STARTBUTTONWIDTH * 5), 330, STARTBUTTONWIDTH, STARTBUTTONHEIGHT);

            startTextPos = new Vector2(startButtonRect.X + 30, startButtonRect.Y - 6);
            versionBuildPos = new Vector2(0, game.GraphicsDevice.Viewport.Height - 30);

            startButtonAlpha = 255;
            AlphaChanger = -3;

            version = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        public override void Update(GameTime gameTime)
        {
            FadeStartButton();
            MenuInput();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color startButtonColor = new Color(startButtonAlpha, startButtonAlpha, startButtonAlpha, startButtonAlpha);

            spriteBatch.Draw(backgroundTexture, new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2), new Rectangle(0, 0, backgroundTexture.Width, backgroundTexture.Height), fader, 0f, new Vector2(backgroundTexture.Width / 2, backgroundTexture.Height / 2), 0.5f, SpriteEffects.None, 0);
            if(screenState != ScreenState.Transitioning)
            {
                spriteBatch.Draw(logoTexture, logoRect, Color.White);
                spriteBatch.Draw(startButtonTexture, startButtonRect, startButtonColor);
                spriteBatch.DrawString(startTextFont, "Press to Start", startTextPos, startButtonColor);
                spriteBatch.DrawString(startTextFont, version, versionBuildPos, Color.White);
            }
        }

        public override void UnloadContent()
        {
            content.Unload();             
        }

        public override void LoadContent()
        {
            backgroundTexture = content.Load<Texture2D>("Startup Menu\\background");

            logoTexture = content.Load<Texture2D>("Startup Menu\\logo");

            startButtonTexture = content.Load<Texture2D>("Startup Menu\\xButton");

            startTextFont = content.Load<SpriteFont>("Startup Menu\\startFont");
        }

        private void FadeStartButton()
        {
            startButtonAlpha = (byte)(startButtonAlpha + AlphaChanger);

            if(startButtonAlpha == 0 || startButtonAlpha == 255)
            {
                AlphaChanger *= -1;
            }
        }

        private void MenuInput()
        {
            pad = GamePad.GetState(PlayerIndex.One);

            if(pad.Buttons.X == ButtonState.Pressed && oldpad.Buttons.X == ButtonState.Released)
            {
                SetScreenState(ScreenState.Transitioning);
                screenHandler.AddScreen(new MenuScreen(game, screenHandler));
            }

            oldpad = pad;
        }
    }
}

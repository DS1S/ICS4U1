using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tales_of_a_Spooderman.Handlers;
using Microsoft.Xna.Framework.Input;

namespace Tales_of_a_Spooderman.Screens
{
    class EndScreen : GameScreen
    {
        private bool winner;

        private SpriteFont optionFont;
        private SpriteFont gameOverFont;

        private Option[] options;

        private string[] optionTxt;
        private string endTxt;


        private Vector2 gameOverPos;


        public EndScreen(Game1 _game, ScreenHandler _screenHandler, bool isLost) : base(_game, _screenHandler)
        {
            SetScreenName("end screen");
            SetScreenState(ScreenState.Displaying);
            winner = isLost;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, backgroundContainer, fader);
            if(screenState != ScreenState.Transitioning)
            {
                foreach(Option option in options)
                {
                    option.Draw(spriteBatch, optionFont, optionFont);
                    spriteBatch.DrawString(gameOverFont, endTxt, gameOverPos, Color.White);
                }
            }
        }

        public override void Init()
        {
            currentIndex = 0;

            if (winner)
            {
                endTxt = "You Win";
            }
            else
            {
                endTxt = "Game Over";
            }
            gameOverPos = new Vector2(game.GraphicsDevice.Viewport.Width / 2 - gameOverFont.MeasureString(endTxt).X / 2, 150);

            SetupOptions();
        }

        public override void LoadContent()
        {
            backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            backgroundTexture.SetData<Color>(new Color[] { Color.Red });

            optionFont = content.Load<SpriteFont>("Main Menu\\optionFont");
            gameOverFont = content.Load<SpriteFont>("End Menu\\GameOverFont");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            pad = GamePad.GetState(PlayerIndex.One);

            foreach(Option option in options)
            {
                option.Update(gameTime, currentIndex);
            }

            if (pad.IsConnected)
            {
                currentIndex = ChangeOptionIndex(currentIndex, options.Length);
                ChangeOptions();
            }

            oldpad = pad;
        }

        private void SetupOptions()
        {
            options = new Option[2];
            optionTxt = new string[]
            {
                "Continue",
                "Exit"
            };

            for (int i = 0; i < options.Length; i++)
            {
                float wordX = optionFont.MeasureString(optionTxt[i]).X;
                float wordY = optionFont.MeasureString(optionTxt[i]).Y;

                Vector2 pos = new Vector2(game.GraphicsDevice.Viewport.Width / 2 - wordX / 2, (game.GraphicsDevice.Viewport.Height / 2 - wordY / 2) + ((i + 1) * 80));

                options[i] = new Option(pos, optionTxt[i], string.Empty, i + 1, game);
            }
        }

        private void ChangeOptions()
        {
            if(pad.Buttons.A == ButtonState.Pressed && oldpad.Buttons.A == ButtonState.Released)
            {
                switch (currentIndex)
                {
                    case 1:
                        SetScreenState(ScreenState.Transitioning);
                        screenHandler.AddScreen(new MenuScreen(game, screenHandler));
                        break;
                    case 2:
                        game.Exit();
                        break;
                }
            }
        }
    }
}

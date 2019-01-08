using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tales_of_a_Spooderman.Handlers;
using Tales_of_a_Spooderman.Core;
using Microsoft.Xna.Framework.Input;

namespace Tales_of_a_Spooderman.Screens
{
    class MenuScreen : GameScreen
    {

        private Animation backgroundAnim;

        private List<Option> options;

        private string[] optionTxt;
        private string[] optionSelectedTxt;

        private SpriteFont optionFont;
        private SpriteFont optionSelectedFont;

        public MenuScreen(Game1 _game, ScreenHandler _screenHandler) : base(_game, _screenHandler)
        {
            SetScreenName("Main Menu");
            SetScreenState(ScreenState.Displaying);
        }

        public override void Init()
        {
            currentIndex = 0;
            SetupOptions();                   
        }

        public override void Update(GameTime gameTime)
        {
            pad = GamePad.GetState(PlayerIndex.One);

            backgroundAnim.Animate(gameTime, true);

            foreach(Option option in options)
            {
                option.Update(gameTime, currentIndex);
            }

            if (pad.IsConnected)
            {
                currentIndex = ChangeOptionIndex(currentIndex, optionTxt.Length);
                CheckOptionSelection();
            }

            oldpad = pad;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {         
            backgroundAnim.Draw(spriteBatch, backgroundContainer, fader);
            if(screenState != ScreenState.Transitioning)
            {
                foreach (Option option in options)
                {
                    option.Draw(spriteBatch, optionFont, optionSelectedFont);
                }
            }           
        }

        public override void LoadContent()
        {
            backgroundAnim = new Animation(AnimationStates.MAIN_MENU_BACKGROUND, 3, 7, 14, content.Load<Texture2D>("Main Menu\\background"));

            optionFont = content.Load<SpriteFont>("Main Menu\\optionFont");

            optionSelectedFont = content.Load<SpriteFont>("Main Menu\\optionSelectedFont");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        private void SetupOptions()
        {
            options = new List<Option>();
            optionTxt = new string[4];
            optionSelectedTxt = new string[optionTxt.Length];

            optionTxt[0] = "Challenge Mode";
            optionTxt[1] = "Instructions";
            optionTxt[2] = "About";
            optionTxt[3] = "Exit";

            optionSelectedTxt[0] = "Face endless rounds of combative goons";
            optionSelectedTxt[1] = "Hone your skills by learning the mechanics of the game";
            optionSelectedTxt[2] = "Learn about the game & credits";
            optionSelectedTxt[3] = "Close the game";

            for (int i = 0; i < optionTxt.Length; i++)
            {
                float spaceY = optionFont.MeasureString(optionTxt[i]).Y * 1.5f;
                float spaceX = optionFont.MeasureString(optionTxt[i]).X;

                options.Add(new Option
                    (new Vector2(game.GraphicsDevice.Viewport.Width - spaceX, 150 + (i * spaceY)), 
                    optionTxt[i], 
                    optionSelectedTxt[i], 
                    i + 1, 
                    game));
            }
        }

        private void CheckOptionSelection()
        {
            if(pad.Buttons.A == ButtonState.Pressed && oldpad.Buttons.A == ButtonState.Released)
            {
                switch (currentIndex)
                {
                    case 1:
                        SetScreenState(ScreenState.Transitioning);
                        screenHandler.AddScreen(new HUD(game, screenHandler));
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        game.Exit();
                        break;
                    case 5:
                        SetScreenState(ScreenState.Transitioning);
                        screenHandler.AddScreen(new HUD(game, screenHandler));
                        break;             
                }
            }
        }
    }
}

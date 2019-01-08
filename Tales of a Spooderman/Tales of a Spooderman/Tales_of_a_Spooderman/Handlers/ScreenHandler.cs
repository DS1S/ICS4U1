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
using Tales_of_a_Spooderman.Screens;
using Tales_of_a_Spooderman.Core.Player;

namespace Tales_of_a_Spooderman.Handlers
{
    class ScreenHandler : DrawableGameComponent
    {
        List<GameScreen> gameScreens = new List<GameScreen>();

        private SpriteBatch spriteBatch;
        
        public ScreenHandler(Game game) : base (game)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);           
        }

        public override void Update(GameTime gameTime)
        {
            int countTransitioningScreens = 0;

            for(int i = 0; i < gameScreens.Count; i++)
            {
                if (gameScreens[i].GetScreenState() == ScreenState.Displaying || gameScreens[i].GetScreenState() == ScreenState.Overlaying)
                {
                    gameScreens[i].Update(gameTime);
                }
                else if (gameScreens[i].GetScreenState() == ScreenState.Transitioning)
                {
                    gameScreens[i].PerformTransition(gameTime);
                    countTransitioningScreens++;
                    if(countTransitioningScreens == 2)
                    {
                        gameScreens[i].SetScreenState(ScreenState.Hidden);
                    }
                }

                if (gameScreens[i].GetScreenState() == ScreenState.Hidden)
                {
                    this.RemoveScreen(gameScreens[i]);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.GetCameraMatrix());
            for (int i = gameScreens.Count - 1; i >= 0; i--)
            {                  
                if (gameScreens[i].GetScreenState() == ScreenState.Displaying)
                {
                    gameScreens[i].Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin();
            if(GetOverlayScreen() != null)
                GetOverlayScreen().Draw(spriteBatch);
            if (GetTransitioningScreen() != null)
                GetTransitioningScreen().Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
                
        public void AddScreen(GameScreen screen)
        {
            gameScreens.Add(screen);
            screen.LoadContent();
            screen.Init();
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();
            gameScreens.Remove(screen);
            screen = null;
        }

        public GameScreen GetCurrentActiveScreen()
        {
            foreach(GameScreen screen in gameScreens)
            {
                if(screen.GetScreenState() == ScreenState.Displaying)
                {
                    return screen;
                }
            }

            return null;
        }

        public GameScreen GetOverlayScreen()
        {
            foreach (GameScreen screen in gameScreens)
            {
                if (screen.GetScreenState() == ScreenState.Overlaying)
                {
                    return screen;
                }
            }

            return null;
        }

        public GameScreen GetTransitioningScreen()
        {
            foreach (GameScreen screen in gameScreens)
            {
                if (screen.GetScreenState() == ScreenState.Transitioning)
                {
                    return screen;
                }
            }
            return null;
        }
    }
}

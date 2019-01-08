using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tales_of_a_Spooderman.Screens
{
    public class Option
    {
        private Vector2 optionPosition;
        private string optionText;
        private int index;
        private bool isSelected;
        private Game game;

        private Rectangle selectedRectangle;
        private Texture2D selectedTexture;
        private string selectedText;
        
        public Option(Vector2 optionPos, string optionTxt, string selectedTxt, int index, Game game)
        {
            SetGame(game);
            SetOptionPos(optionPos);
            SetOptionText(optionTxt);
            SetOptionIndex(index);
            SetSelectedText(selectedTxt);
            SetSelectedRectangle();

            selectedTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            selectedTexture.SetData<Color>(new Color[] { new Color(0,0,0,175) });

            isSelected = false;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, SpriteFont selectedFont)
        {
            float selectedTextHeight = selectedFont.MeasureString(selectedText).Y;

            if (isSelected)
            {
                spriteBatch.Draw(selectedTexture, selectedRectangle, Color.White);
                spriteBatch.DrawString(selectedFont, selectedText, new Vector2(0, selectedRectangle.Center.Y - selectedTextHeight / 2), Color.White);
            }
            spriteBatch.DrawString(font, optionText, optionPosition, Color.White);
        }

        public void Update(GameTime gameTime, int CurrentIndex)
        {
            if(CurrentIndex == index)
            {
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }
        }

        private void SetOptionPos(Vector2 _optionPosition)
        {
            this.optionPosition = Vector2.Clamp(_optionPosition, new Vector2(0,0), 
                                new Vector2(game.GraphicsDevice.Viewport.Width, 
                                game.GraphicsDevice.Viewport.Height));
        }

        private void SetOptionText(string option)
        {
            this.optionText = option;
        }

        private void SetOptionIndex(int index)
        {
            this.index = index;
        }

        private void SetSelectedText(string txt)
        {
            this.selectedText = txt;
        }

        private void SetGame(Game game)
        {
            this.game = game;
        }

        private void SetSelectedRectangle()
        {
            selectedRectangle = new Rectangle(0, (int)optionPosition.Y, game.GraphicsDevice.Viewport.Width, 34);
        }
    }
}

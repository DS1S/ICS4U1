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

namespace Tales_of_a_Spooderman.Core
{
    abstract class GameObject
    {
        protected Rectangle gameObjectRectangle;
        protected Game game;
        protected GameObjectHandler handler;
        protected string gameObjectTag;

        public GameObject(Rectangle _gameObjectRectangle, string _gameObjectTag, GameObjectHandler _hanlder, Game game)
        {
            SetGameObjectRectangle(_gameObjectRectangle);
            SetGameObjectTag(_gameObjectTag);
            SetGameObjectHandler(_hanlder);
            SetGameObjectGame(game);
        }

        public void SetGameObjectRectangle(Rectangle gameObjectRectangle)
        {
            this.gameObjectRectangle = gameObjectRectangle;
        }

        private void SetGameObjectTag(string gameObjectTag)
        {
            this.gameObjectTag = gameObjectTag;
        }

        private void SetGameObjectHandler(GameObjectHandler handler)
        {
            this.handler = handler;
        }

        private void SetGameObjectGame(Game game)
        {
            this.game = game;
        }

        public Rectangle GetGameObjectRectangle()
        {
            return gameObjectRectangle;
        }

        public string GetGameObjectTag()
        {
            return gameObjectTag;
        }

        public abstract void Update(GameTime gameTime, GamePadState pad, GamePadState oldpad);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}

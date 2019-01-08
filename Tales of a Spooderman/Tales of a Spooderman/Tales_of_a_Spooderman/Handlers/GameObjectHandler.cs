using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tales_of_a_Spooderman.Core;

namespace Tales_of_a_Spooderman.Handlers
{
    class GameObjectHandler
    {
        private List<GameObject> gameObjects = new List<GameObject>();

        public void Update(GameTime gameTime, GamePadState pad, GamePadState oldpad)
        {
            for(int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime, pad, oldpad);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                gameObjects[i].Draw(spriteBatch);
            }
        }

        public void Add(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public void Remove(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
            gameObject = null;
        }

        public GameObject GetGameObject(string gameObjectTag)
        {
            foreach(GameObject gameObj in gameObjects)
            {
                if (gameObj.GetGameObjectTag().ToUpper().Equals(gameObjectTag.ToUpper()))
                {
                    return gameObj;
                }
            }

            return null;
        }
    }
}

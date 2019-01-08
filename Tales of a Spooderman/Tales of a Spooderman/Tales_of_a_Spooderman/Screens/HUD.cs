using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tales_of_a_Spooderman.Handlers;
using Tales_of_a_Spooderman.Core.Player;
using Tales_of_a_Spooderman.Core;
using Microsoft.Xna.Framework.Input;

namespace Tales_of_a_Spooderman.Screens
{
    class HUD : GameScreen
    {
        private GameObjectHandler handler;

        private int healthP1;
        private int ammoCountP1;
        private int playerHealthToDecrease;
        private int enemyHealthToDecrease;
        private int enemyHealth;

        private Rectangle[] playerAmmoCountRects;
        private Rectangle playerHealthBarRect;
        private Rectangle playerHealthBarRectBack;
        private Rectangle playerIconRect;
        private Rectangle playerSpecialRect;
        private Rectangle enemyHealthBarRect;
        private Rectangle enemyHealthBarBackRect;
        private Rectangle enemyIconRect;

        private Texture2D healthBarTexture;
        private Texture2D heathBarBackTexture;
        private Texture2D playerSpecialTexture;
        private Texture2D ammoCountTexture;
        private Texture2D playerIcon;
        private Texture2D enemyIcon;

        private float specialAttackCountP1;

        private const int SCALE = 3;

        private EnduranceMode attachedMode;

        public HUD(Game1 _game, ScreenHandler _screenHandler) : base(_game, _screenHandler)
        {
            SetScreenName("HUD");
            SetScreenState(ScreenState.Overlaying);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(heathBarBackTexture, playerHealthBarRectBack, Color.White);
            spriteBatch.Draw(healthBarTexture, playerHealthBarRect, Color.White);
            spriteBatch.Draw(playerIcon, playerIconRect, Color.White);
            spriteBatch.Draw(playerSpecialTexture, playerSpecialRect, Color.White);
            for(int i = 0; i < ammoCountP1; i++)
            {
                spriteBatch.Draw(ammoCountTexture, playerAmmoCountRects[i], Color.Red);
            }
            spriteBatch.Draw(heathBarBackTexture, enemyHealthBarBackRect, Color.White);
            spriteBatch.Draw(healthBarTexture, enemyHealthBarRect, Color.White);
            spriteBatch.Draw(enemyIcon, enemyIconRect, Color.White);
        }

        public override void Init()
        {
            screenHandler.AddScreen(new EnduranceMode(game, screenHandler));

            attachedMode = (EnduranceMode)screenHandler.GetCurrentActiveScreen();

            handler = attachedMode.GetGameObjectHandler();

            playerHealthToDecrease = 0;
            enemyHealthToDecrease = 0;

            healthP1 = GetHealthOfCharacter("player");
            ammoCountP1 = GetAmmoOfPlayer();
            specialAttackCountP1 = GetSpecialAttackofPlayer();

            playerHealthBarRect = new Rectangle(70, 20, healthP1 * SCALE, 20);
            playerHealthBarRectBack = playerHealthBarRect;
            playerSpecialRect = new Rectangle(playerHealthBarRect.X, playerHealthBarRect.Bottom, (int)specialAttackCountP1 * SCALE, 5);
            playerIcon = GetCharacterIcon("player");
            playerIconRect = new Rectangle(playerHealthBarRect.X - 50, playerHealthBarRect.Y - 10, 40, 40);
            playerAmmoCountRects = new Rectangle[3];
            for(int i = 0; i < playerAmmoCountRects.Length; i++)
            {
                playerAmmoCountRects[i] = new Rectangle(playerHealthBarRect.X + (i * 35), playerSpecialRect.Bottom, 30, 30);
            }

            enemyHealth = GetHealthOfCharacter("goblin");
            enemyHealthBarRect = new Rectangle(game.GraphicsDevice.Viewport.Width - playerHealthBarRect.X - enemyHealth * SCALE, playerHealthBarRect.Y, enemyHealth * SCALE, playerHealthBarRect.Height);
            enemyHealthBarBackRect = enemyHealthBarRect;
            enemyIconRect = new Rectangle(game.GraphicsDevice.Viewport.Width - playerIconRect.X - playerIconRect.Width, playerIconRect.Y, playerIconRect.Width, playerIconRect.Height);
            enemyIcon = GetCharacterIcon("goblin");
        }

        public override void LoadContent()
        {
            healthBarTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            healthBarTexture.SetData<Color>(new Color[] { Color.Green });

            heathBarBackTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            heathBarBackTexture.SetData<Color>(new Color[] { Color.Red });

            playerSpecialTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            playerSpecialTexture.SetData<Color>(new Color[] { Color.Orange });

            ammoCountTexture = content.Load<Texture2D>("Spiderman\\WebCount");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            SmoothHealthDown();
            ammoCountP1 = GetAmmoOfPlayer();
            specialAttackCountP1 = GetSpecialAttackofPlayer();
            CheckIfGameIsDone();

            playerHealthBarRect.Width = (int)(healthP1 * SCALE);
            playerSpecialRect.Width = (int)specialAttackCountP1 * SCALE;
            enemyHealthBarRect.Width = (int)(enemyHealth * SCALE);
   
        }

        private void SmoothHealthDown()
        {
            playerHealthToDecrease = healthP1 - GetHealthOfCharacter("player");
            enemyHealthToDecrease = enemyHealth - GetHealthOfCharacter("goblin");
            if (playerHealthToDecrease > 0)
            {
                healthP1--;
                playerHealthToDecrease--;
            }
            else if (enemyHealthToDecrease > 0)
            {
                enemyHealth--;
                enemyHealthToDecrease--;
            }
        }

        private int GetHealthOfCharacter(string characterName)
        {
            Character character = (Character)handler.GetGameObject(characterName);
            return character.GetCharacterHealth();
        }

        private void CheckIfGameIsDone()
        {
            Character player = (Character)handler.GetGameObject("player");
            Character enemy = (Character)handler.GetGameObject("goblin");

            if (player.GetCharacterHealthState() || enemy.GetCharacterHealthState())
            {
                Camera.SetDefaultCameraMatrix();
                SetScreenState(ScreenState.Hidden);
                attachedMode.SetScreenState(ScreenState.Transitioning);
                screenHandler.AddScreen(new EndScreen(game, screenHandler, enemy.GetCharacterHealthState()));
            }  
        }

        private int GetAmmoOfPlayer()
        {
            Spiderman player = (Spiderman)handler.GetGameObject("player");
            return player.GetAmmoCount();
        }

        private float GetSpecialAttackofPlayer()
        {
            Spiderman player = (Spiderman)handler.GetGameObject("player");
            return player.GetSpecialAttackCount();
        }

        private Texture2D GetCharacterIcon(string chracterName)
        {
            Character character = (Character)handler.GetGameObject(chracterName);
            return character.GetCharacterIcon();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tales_of_a_Spooderman.Handlers;
using Microsoft.Xna.Framework.Input;
using Tales_of_a_Spooderman.Core.Player;
using Tales_of_a_Spooderman.Core;
using Tales_of_a_Spooderman.Helper;
using Tales_of_a_Spooderman.Core.Enemy;

namespace Tales_of_a_Spooderman.Screens
{
    class EnduranceMode : GameScreen
    {
        private Animation[] playerAnimations;
        private Animation[] greenGoblinAnimations;

        private GameObjectHandler handler;

        private Texture2D projectileTexture;
        private Texture2D playerIcon;
        private Texture2D goblinIcon;

        public EnduranceMode(Game1 _game, ScreenHandler _screenHandler) : base(_game, _screenHandler)
        {
            SetScreenName("Dev Mode");
            SetScreenState(ScreenState.Displaying);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {


            spriteBatch.Draw(backgroundTexture, backgroundContainer, fader);
            if(screenState != ScreenState.Transitioning)
            {
                handler.Draw(spriteBatch);
            }
        }

        public override void Init()
        {
            const int FLOORLIMIT = 480;
            const int PLAYERHEIGHT = 120;
            const int PLAYERWIDTH = 100;
            const int GOBLINHEIGHT = 120;
            const int GOBLINWIDTH = 70;
            const int HEALTH = 100;
            const int PLAYERVELO = 4;
            const int ENEMYVELO = 3;

            handler = new GameObjectHandler();

            Rectangle playerRect = new Rectangle(0, FLOORLIMIT - PLAYERHEIGHT, PLAYERWIDTH, PLAYERHEIGHT);
            Spiderman player = new Spiderman(playerRect, "player", handler, playerAnimations, game, PLAYERVELO, HEALTH, FLOORLIMIT + 1, playerIcon);
            player.SetProjectileTexture(projectileTexture);

            Rectangle goblinRect = new Rectangle(700, FLOORLIMIT - GOBLINHEIGHT, GOBLINWIDTH, GOBLINHEIGHT);
            GreenGoblin goblin = new GreenGoblin(goblinRect, "goblin", handler, greenGoblinAnimations, game, ENEMYVELO, HEALTH, FLOORLIMIT + 1, goblinIcon);

            handler.Add(player);
            handler.Add(goblin);

            backgroundContainer = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width * 2, game.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            playerAnimations = new Animation[6];
            playerAnimations[0] = new Animation(AnimationStates.RUN, 1, 7, 11, content.Load<Texture2D>("Spiderman\\Spiderman Run"));
            playerAnimations[1] = new Animation(AnimationStates.IDLE, 1, 3, 7, content.Load<Texture2D>("Spiderman\\Spiderman Idle"));
            playerAnimations[2] = new Animation(AnimationStates.PUNCH, 1, 2, 7, content.Load<Texture2D>("Spiderman\\Spiderman Punch"));
            playerAnimations[3] = new Animation(AnimationStates.JUMP, 1, 1, 2, content.Load<Texture2D>("Spiderman\\Spiderman Jump"));
            playerAnimations[4] = new Animation(AnimationStates.SWING, 1, 10, 8, content.Load<Texture2D>("Spiderman\\Spiderman Swing"));
            playerAnimations[5] = new Animation(AnimationStates.SHOOT, 1, 5, 7, content.Load<Texture2D>("Spiderman\\Spiderman Shoot"));

            projectileTexture = content.Load<Texture2D>("Spiderman\\Spiderman Web");
            playerIcon = content.Load<Texture2D>("Spiderman\\Icon");


            greenGoblinAnimations = new Animation[4];
            greenGoblinAnimations[0] = new Animation(AnimationStates.IDLE, 1, 1, 1, content.Load<Texture2D>("Green Goblin\\GreenGoblin Idle"));
            greenGoblinAnimations[1] = new Animation(AnimationStates.WALK, 1, 8, 9, content.Load<Texture2D>("Green Goblin\\GreenGoblin Walk"));
            greenGoblinAnimations[2] = new Animation(AnimationStates.SHOOT, 1, 7, 9, content.Load<Texture2D>("Green Goblin\\GreenGoblin GroundAttack"));
            greenGoblinAnimations[3] = new Animation(AnimationStates.BOMB, 1, 4, 6, content.Load<Texture2D>("Green Goblin\\bomb"));

            goblinIcon = content.Load<Texture2D>("Green Goblin\\GreenGoblinIcon");

            backgroundTexture = content.Load<Texture2D>("Spiderman\\background");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            pad = GamePad.GetState(PlayerIndex.One);

            handler.Update(gameTime, pad, oldpad);

            oldpad = pad;
        }

        public GameObjectHandler GetGameObjectHandler()
        {
            return handler;
        }
    }
}

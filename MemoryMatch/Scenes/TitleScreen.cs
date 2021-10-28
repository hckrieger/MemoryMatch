using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryMatch.Scenes
{
    class TitleScreen : GameState
    {
        TextGameObject title = new TextGameObject("Fonts/title", 1f, Color.White, TextGameObject.Alignment.Center);
        TextGameObject pressSpace = new TextGameObject("Fonts/pressStart", 1f, Color.White, TextGameObject.Alignment.Center);
        TextGameObject programmedBy = new TextGameObject("Fonts/programmedBy", 1f, Color.White, TextGameObject.Alignment.Center);

        float startTimer, timer = .555f;

        public TitleScreen()
        {
            title.LocalPosition = new Vector2(200, 100);
            title.Text = "Memory Match";
            gameObjects.AddChild(title);

            pressSpace.LocalPosition = new Vector2(200, 300);
            pressSpace.Text = "press space to play";
            gameObjects.AddChild(pressSpace);

            programmedBy.LocalPosition = new Vector2(200, 450);
            programmedBy.Text = "Programmed by Hunter Krieger";
            gameObjects.AddChild(programmedBy);

            startTimer = timer;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (inputHelper.KeyPressed(Keys.Space))
            {
                ExtendedGame.GameStateManager.SwitchTo(Game1.MAIN);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                pressSpace.Visible = !pressSpace.Visible;
                timer = startTimer;
            }
        }
    }
}

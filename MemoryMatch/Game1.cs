using Engine;
using MemoryMatch.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MemoryMatch
{
    public class Game1 : ExtendedGame
    {
        public const string TITLE = "SCENE_TITLE";
        public const string MAIN = "MAIN_SCENE";

        public Game1()
        {
            windowSize = new Point(400, 550);
            worldSize = new Point(400, 550);

            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GameStateManager.AddGameState(TITLE, new TitleScreen());
            GameStateManager.SwitchTo(TITLE);

            GameStateManager.AddGameState(MAIN, new MainScene());
        }

    }
}

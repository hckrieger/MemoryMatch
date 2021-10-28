using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryMatch.GameObjects
{
    class Card : SpriteGameObject
    {
        public bool FacingUp { get; set; }
        public bool IsMatched { get; set; }
        public Color StoredColor { get; set; }
        public Card() : base("Card", 1f)
        {
            FacingUp = false;
            IsMatched = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (FacingUp)
                Color = StoredColor;
            else
                Color = Color.White;

            if (IsMatched)
                FacingUp = true;
            base.Update(gameTime);
        }
    }
}

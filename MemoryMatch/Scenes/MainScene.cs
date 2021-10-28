using Engine;
using MemoryMatch.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryMatch.Scenes
{
    class MainScene : GameState
    {
        GameObjectList playingField = new GameObjectList();

        const int ROWS = 4;
        const int COLS = 4;

        float xSpacing = 70;
        float ySpacing = 85;

        Card[,] cardGrid;

        List<Color> colors;

        List<Color> colorAssign = new List<Color>();
        List<Card> comparedCards = new List<Card>();

        float compareTimer, compareTimerStart;

        int tries = 0, matches = 0, games = 0;


        TextGameObject scoreFont = new TextGameObject("Fonts/Score", 1f, Color.White);
        TextGameObject titleFont = new TextGameObject("Fonts/Title", 1f, Color.White, TextGameObject.Alignment.Center);
        TextGameObject instructionsFont = new TextGameObject("Fonts/Instructions", 1f, Color.White, TextGameObject.Alignment.Center);
        TextGameObject averageFont = new TextGameObject("Fonts/Score", 1f, Color.White, TextGameObject.Alignment.Center);

        public MainScene()
        {

            cardGrid = new Card[ROWS, COLS];


            //Seup up the grid of cards
            for (int x = 0; x < ROWS; x++)
            {
                for (int y = 0; y < COLS; y++)
                {
                    cardGrid[x, y] = new Card();
                    
                    cardGrid[x, y].LocalPosition = new Vector2(x * xSpacing, y * ySpacing);
                    playingField.AddChild(cardGrid[x, y]);
                    
                }
            }
            

            compareTimer = 1f;
            compareTimerStart = compareTimer;

            Reset();

            playingField.LocalPosition = new Vector2(70, 170);
            gameObjects.AddChild(playingField);

            titleFont.Text = "Match It!";
            titleFont.LocalPosition = new Vector2(200, 50);
            gameObjects.AddChild(titleFont);

            instructionsFont.LocalPosition = new Vector2(200, 105);
            gameObjects.AddChild(instructionsFont);


        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            for (int x = 0; x < ROWS; x++)
            {
                for (int y = 0; y < COLS; y++)
                {
                    //If player clicks on a card
                    if (cardGrid[x, y].BoundingBox.Contains(inputHelper.MousePositionWorld)
                        && inputHelper.MouseLeftButtonPressed() &&
                        !cardGrid[x, y].IsMatched)
                    {
                        //if the duration which two incorrect cards face up is still going then flip them over early,
                        //reset the timer, clear the list of cards to be compared and add a try. 
                        if (compareTimer < compareTimerStart)
                        {
                            foreach (Card obj in comparedCards)
                                obj.FacingUp = false;
                            comparedCards.Clear();
                            compareTimer = compareTimerStart;
                            AddTryCount();
                        }

                        //if the card is faced down 
                        if (!cardGrid[x, y].FacingUp)
                        {
                            //Then face it up and add it to the list of cards to be compared
                            comparedCards.Add(cardGrid[x, y]);
                            cardGrid[x, y].FacingUp = true;
                        } else // if only one card is faced up
                        {
                            //Then face it down, add a try and clear the list of cards to be compared
                            foreach (Card obj in comparedCards)
                                obj.FacingUp = false;

                            AddTryCount();
                            comparedCards.Clear();
                        }

                    }

                }
            }

            if (tries > 0)
            {
                instructionsFont.Visible = false;
            } else
            {
                instructionsFont.Text = "Click on the cards\n Match the colors.";
                instructionsFont.Visible = true;
            }
                

            //If you've matched all of the cards
            if (matches == 8)
            {

                instructionsFont.Visible = true;
                instructionsFont.Text = $"You matched them all in {tries} attempts!\n        Press Space to play again.";

                //Then press the spacebar to reset the game 
                if (inputHelper.KeyPressed(Keys.Space))
                    Reset();
            } else
            {
                scoreFont.Visible = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Set the text of the score
            scoreFont.Text = "Attempts: " + tries.ToString();

            //If two selected cards are faced up
            if (comparedCards.Count == 2)
            {
                    //And they are the same color, add a match, add a try and clear the list of cards to be compared
                    if (comparedCards[0].Color == comparedCards[1].Color)
                    {   
                        //Then keep them faced up
                        foreach (Card obj in comparedCards)
                        {
                            //When IsMatched is true it automatically faces them up
                            obj.IsMatched = true;
                        }
                        matches++;
                        AddTryCount();
                        comparedCards.Clear();

                    }
                    //If the cards do not match
                    else if (comparedCards[0].Color != comparedCards[1].Color)
                    {
                        //Then run the timer that keeps the cards up for a set duration
                        compareTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        
                        //If the timer runs out
                        if (compareTimer <= 0)
                        {
                            //Then face the unmatched cards down, add a try, clear the list of cards to be compared and reset the timer
                            foreach (Card obj in comparedCards)
                                obj.FacingUp = false;

                            AddTryCount();
                            comparedCards.Clear();
                            compareTimer = compareTimerStart;

                        }
                    }            
            }

        }

        void AddTryCount()
        {
            tries++;
            //totalTries++;
        }

        //Method that resets the values when a new game starts
        public override void Reset()
        {
            base.Reset();

            //The color list shows a list of 8 maximum available colors that can be added
            //The colorAssign list shows the order of the 16 colors that are assigned to the cards, in which there are two of each color
            
            //If the colorAssign list still has values then clear them.
            if (colorAssign.Count > 0)
                colorAssign.Clear();

            //List of colors
            colors = new List<Color> { Color.DarkRed, Color.MonoGameOrange, Color.Yellow, Color.ForestGreen, Color.MediumBlue, Color.DeepPink, Color.Turquoise, Color.Indigo };

            for (int i = 0; i < (ROWS * COLS); i++)
            {
                //Generate a random number from 0 to the number of values in the color list
                int rnd = ExtendedGame.Random.Next(colors.Count);

                //Use that random number as the index to find a random color in the list of colors
                Color rndColor = colors[rnd];

                //If the colorAssign list contains that random color
                if (colorAssign.Contains(rndColor))
                {
                    //Then add that color to the colorAssign list again
                    colorAssign.Add(rndColor);

                    //And remove that color from the color list so it doesn't add to the colorAssign list any more than twice
                    colors.Remove(rndColor);
                }
                else //If the colorAssign list doesn't contain that color
                {
                    //Then add that color to the colorAssign list
                    colorAssign.Add(rndColor);
                }
            }

            int colorIndex = 0;
            for (int x = 0; x < ROWS; x++)
            {
                for (int y = 0; y < COLS; y++)
                {
                    //Set each value of the colorAssign list to each card in the given order so there's two of each color
                    cardGrid[x, y].StoredColor = colorAssign[colorIndex];
                    cardGrid[x, y].FacingUp = false;
                    cardGrid[x, y].IsMatched = false;
                    colorIndex++;
                }
            }

            //Reset the tries and matches
            tries = 0;
            matches = 0;
            games++;
        }


    }
}

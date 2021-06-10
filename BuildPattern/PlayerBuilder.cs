﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown.BuildPattern
{
    public class PlayerBuilder : IBuilder
    {
        private GameObject1 playerGO;

        public SpriteRenderer playerSR;

        public Texture2D[] walk;
        public float fps;
        public float timeElapsed;
        public int currentIndex;

        public SpriteRenderer Sr
        {
            get { return playerSR; }
            set { playerSR = value; }
        }

        public Player player;

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public void BuildGameObject()
        {
            playerGO = new GameObject1();

            playerSR = new SpriteRenderer("1player");

            playerGO.AddComponent(playerSR);
            playerSR.Origin = new Vector2(playerSR.Sprite.Width / 2, (playerSR.Sprite.Height) / 2);

            player = new Player();

            playerGO.AddComponent(new Collider(playerSR, player) { CheckCollisionEvents = true } );
            playerGO.AddComponent(player);
            /// Adds player to collider list
            GameWorld.Instance.GameState.AddGameObject(playerGO);

            //Load sprite sheet - Frederik
            walk = new Texture2D[6];

            //Loop animaiton
            for (int g = 0; g < walk.Length; g++)
            {
                walk[g] = GameWorld.Instance.Content.Load<Texture2D>(g + 1 + "player");
            }
            //When loop is finished return to first sprite/Sets default sprite
            playerSR.Sprite = walk[0];
        }

        /// <summary>
        /// Animate player - Frederik
        /// </summary>
        /// <param name="gametime"></param>
        public void Animate(GameTime gametime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Gives time that has passed since last update
                timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

                // Calculates currentIndex
                currentIndex = (int)(timeElapsed * fps);
                playerSR.Sprite = walk[currentIndex];

                // Checks if animation needs to restart
                if (currentIndex >= walk.Length - 1)
                {
                    // Resets animation
                    timeElapsed = 0;
                    currentIndex = 0;
                }
            }
        }

        public GameObject1 GetResult()
        {
            return playerGO;
        }
    }
}

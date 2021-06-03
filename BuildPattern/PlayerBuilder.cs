using Microsoft.Xna.Framework;
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

        private SpriteRenderer playerSR;

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

            playerSR = new SpriteRenderer("player");

            playerGO.AddComponent(playerSR);
            playerSR.Origin = new Vector2(playerSR.Sprite.Width /2, (playerSR.Sprite.Height)/2 );

            player = new Player();

            playerGO.AddComponent(new Collider(playerSR, player) { CheckCollisionEvents = true } );
            playerGO.AddComponent(player);
            /// Adds player to collider list
            GameWorld.Instance.gameState.AddGameObject(playerGO);

        }

        public GameObject1 GetResult()
        {
            return playerGO;
        }
    }
}

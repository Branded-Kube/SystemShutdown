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
        private GameObject1 go;

        private SpriteRenderer sr;

        public SpriteRenderer Sr
        {
            get { return sr; }
            set { sr = value; }
        }

        public Player1 player;

        public Player1 Player
        {
            get { return player; }
            set { player = value; }
        }

        public void BuildGameObject()
        {
            go = new GameObject1();

            //go.AddComponent(new Player1);
            //go.AddComponent(new SpriteRenderer());
            sr = new SpriteRenderer("player");

            go.AddComponent(sr);
            //sr.SetSprite("player");
            sr.Origin = new Vector2(sr.Sprite.Width / 2, (sr.Sprite.Height) / 2);
            //sr.Origin = new Vector2(go.Transform.Position.X, go.Transform.Position.Y);

            player = new Player1();
           // sr.rectangle = new Rectangle((int)sr.Origin.X, (int)sr.Origin.Y, sr.Sprite.Width - 10, sr.Sprite.Height - 10);

            go.AddComponent(new Collider(sr, player) { CheckCollisionEvents = true } );
            go.AddComponent(player);
            /// Adds player to collider list
            GameWorld.gameState.AddGameObject(go);

        }

        public GameObject1 GetResult()
        {
            return go;
        }
    }
}

﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown.BuildPattern
{
    class PlayerBuilder : IBuilder
    {
        private GameObject1 go;

        public void BuildGameObject()
        {
            go = new GameObject1();
            SpriteRenderer sr = new SpriteRenderer();
            go.AddComponent(sr);
            sr.SetSprite("Player");
            sr.Origin = new Vector2(sr.Sprite.Width / 2, (sr.Sprite.Height) - 10);
            //sr.Origin = new Vector2(go.Transform.Position.X, go.Transform.Position.Y);

            Player1 player = new Player1();
            go.AddComponent(new Collider(sr, player) { CheckCollisionEvents = true } );
            go.AddComponent(player);
            /// Adds player to collider list
            GameWorld.Instance.AddGameObject(go);
        }

        //public void BuildGameObject()
        //{
        //    throw new NotImplementedException();
        //}

        public GameObject1 GetResult()
        {
            return go;
        }
    }
}

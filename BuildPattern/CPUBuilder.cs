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
    public class CPUBuilder : IBuilder
    {
        private GameObject1 go;

        private SpriteRenderer sr;

        public SpriteRenderer Sr
        {
            get { return sr; }
            set { sr = value; }
        }

        private CPU cpu;

        public CPU Cpu
        {
            get { return cpu; }
            set { cpu = value; }
        }

        public void BuildGameObject()
        {
            go = new GameObject1();

            sr = new SpriteRenderer("Textures/cpu");

            go.AddComponent(sr);
            sr.Origin = new Vector2(sr.Sprite.Width / 2, (sr.Sprite.Height) / 2);

            cpu = new CPU();

            go.AddComponent(new Collider(sr, cpu) { CheckCollisionEvents = true } );
            go.AddComponent(cpu);
            /// Adds CPU to collider list
            GameWorld.Instance.GameState.AddGameObject(go);

        }

        public GameObject1 GetResult()
        {
            return go;
        }
    }
}

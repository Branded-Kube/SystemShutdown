using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown
{
    public class Mods : Component, IGameListener
    {
        private Mods floormod;

        public int Id { get; set; }

        public string Name { get; set; }

        public GameObject1 Create()
        {

            GameObject1 go = new GameObject1();
            SpriteRenderer sr = new SpriteRenderer("laserBlue05");
            go.AddComponent(sr);

            Debug.WriteLine("mod bliver spawnet");


            //sr.SetSprite("1GuyUp");
            floormod = new Mods();
            go.AddComponent(new Collider(sr, floormod) { CheckCollisionEvents = true });
            go.Transform.Position = new Vector2(400, 200);
            go.AddComponent(floormod);
            GameWorld.gameState.AddGameObject(go);
            return go;
        }
        public override void Awake()
        {
            GameObject.Tag = "Pickup";

            Debug.WriteLine("kør awake");

            GameObject.Transform.Position = new Vector2(400, 200);
            // this.position = GameObject.Transform.Position;
            //spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }
        public override string ToString()
        {
            return "Pickup";
        }
        public override void Destroy()
        {
            GameWorld.gameState.Colliders.Remove((Collider)GameObject.GetComponent("Collider"));

        }

        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Player")
            {
                Debug.WriteLine("picked up a mod");
                GameObject.Destroy();
                GameWorld.gameState.ApplyMod();
            }
        }
    }
}

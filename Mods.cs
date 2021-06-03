using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.Components;
using SystemShutdown.GameObjects;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown
{
    public class Mods : Component, IGameListener
    {
        private Mods floormod;

        public int Id { get; set; }
        public int ModFKID { get; set; }

        public int Effect { get; set; }

        public string Name { get; set; }
        //double despawnTimer = 0.0;


        //public GameObject1 Create()
        //{

        //    GameObject1 go = new GameObject1();
        //    SpriteRenderer sr = new SpriteRenderer("laserBlue05");
        //    go.AddComponent(sr);

        //    Debug.WriteLine("mod bliver spawnet");

        //    //sr.SetSprite("1GuyUp");
        //    floormod = new Mods();
        //    go.AddComponent(new Collider(sr, floormod) { CheckCollisionEvents = true });
        //    go.Transform.Position = new Vector2(400, 200);
        //    go.AddComponent(floormod);
        //    GameWorld.gameState.AddGameObject(go);
        //    return go;
        //}
        public override void Awake()
        {
            GameObject.Tag = "Pickup";


            //Random rnd = new Random();
            //int randomnumber = rnd.Next(1, 5);

            //List<Effects> pickupable = new List<Effects>();

            //GameWorld.repo.Open();
            //pickupable = GameWorld.repo.FindEffects(randomnumber);

            ////playerBuilder.player.dmg += pickupAble.Effect;
            //GameWorld.repo.Close();

            //Random rndeffect = new Random();
            //int randomeffect = rndeffect.Next(0, 3);

            //Effects choseneffect = pickupable[randomeffect];

            //Debug.WriteLine($"{choseneffect.Effectname}");
            //Effect = choseneffect.Effect;
            //ModFKID = choseneffect.ModFK;
            //// GameObject.Transform.Position = new Vector2(400, 200);
            //// this.position = GameObject.Transform.Position;
            ////spriteRenderer = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
        }
        public override string ToString()
        {
            return "Pickup";
        }
        //public override void Destroy()
        //{
        //    GameWorld.gameState.Colliders.Remove((Collider)GameObject.GetComponent("Collider"));

        //}
        public override void Update(GameTime gameTime)
        {
            //despawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            //if (despawnTimer > 10)
            //{
            //    GameObject.Destroy();
            //}
        }
        public void ApplyMod()
        {

            Random rnd = new Random();
            int randomnumber = rnd.Next(1, 5);

            List<Effects> pickupable = new List<Effects>();

            GameWorld.repo.Open();
            pickupable = GameWorld.repo.FindEffects(randomnumber);

            //playerBuilder.player.dmg += pickupAble.Effect;
            GameWorld.repo.Close();

            Random rndeffect = new Random();
            int randomeffect = rndeffect.Next(0, 3);

            Effects choseneffect = pickupable[randomeffect];

            Debug.WriteLine($"{choseneffect.Effectname}");
            //Effect = choseneffect.Effect;
            //ModFKID = choseneffect.ModFK;
            if (choseneffect.ModFK == 1)
            {
                GameWorld.gameState.playerBuilder.player.dmg += choseneffect.Effect;
            }
            else if (choseneffect.ModFK == 2)
            {
                GameWorld.gameState.playerBuilder.player.speed += choseneffect.Effect;
            }
            else if (choseneffect.ModFK == 3)
            {
                GameWorld.gameState.playerBuilder.player.cooldown -= choseneffect.Effect;
            }
            else if (choseneffect.ModFK == 4)
            {
                GameWorld.gameState.playerBuilder.player.Health += choseneffect.Effect;
            }


        }
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Player")
            {
                //(Player)component.GameObject.GetComponent("Player")

                //GameWorld.gameState.playerBuilder.player.playersMods.Add(this);
                ApplyMod();
                GameObject.Destroy();
            }
        }
    }
}

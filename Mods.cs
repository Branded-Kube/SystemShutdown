using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SystemShutdown.ComponentPattern;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown
{
    // Hovedforfatter: Lau
    public class Mods : Component, IGameListener
    {

        public int Id { get; set; }
        public int ModFKID { get; set; }

        public int Effect { get; set; }

        public string Name { get; set; }



        double despawnTimer = 0.0;


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
            despawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (despawnTimer > 15)
            {
                GameObject.Destroy();
            }
        }
        public void ApplyMod()
        {

            Random rnd = new Random();
            int randomnumber = rnd.Next(1, 5);

            List<Effects> pickupable = new List<Effects>();

            GameWorld.Instance.Repo.Open();
            pickupable = GameWorld.Instance.Repo.FindEffects(randomnumber);

            //playerBuilder.player.dmg += pickupAble.Effect;
            GameWorld.Instance.Repo.Close();

            Random rndeffect = new Random();
            int randomeffect = rndeffect.Next(0, 3);

            Effects choseneffect = pickupable[randomeffect];

        Debug.WriteLine($"{choseneffect.Effectname}");
            if (choseneffect.ModFK == 1)
            {
                GameWorld.Instance.GameState.PlayerBuilder.player.dmg += choseneffect.Effect;
                //GameWorld.Instance.gameState.DmgColor = Color.GreenYellow;
                //GameWorld.Instance.gameState.ChangeDmgColor();
                GameWorld.Instance.GameState.DmgColorTimer = true;
            }
            else if (choseneffect.ModFK == 2)
            {
                GameWorld.Instance.GameState.PlayerBuilder.player.speed += choseneffect.Effect;
                GameWorld.Instance.GameState.msColorTimer = true;

            }
            else if (choseneffect.ModFK == 3)
            {
                GameWorld.Instance.GameState.PlayerBuilder.player.cooldown -= choseneffect.Effect;
                GameWorld.Instance.GameState.asColorTimer = true;

            }
            else if (choseneffect.ModFK == 4)
            {
                GameWorld.Instance.GameState.PlayerBuilder.player.Health += choseneffect.Effect;
                //GameWorld.Instance.gameState.HealthColor = Color.GreenYellow;
                //GameWorld.Instance.gameState.ChangeHealthColor();
                GameWorld.Instance.GameState.HealthColorTimerGreen = true;

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

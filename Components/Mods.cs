using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SystemShutdown.ComponentPattern;
using SystemShutdown.ObserverPattern;

namespace SystemShutdown
{
    // Lead author: Lau
    public class Mods : Component, IGameListener
    {
        #region Fields
        public int Id { get; set; }
        public string Name { get; set; }
        double despawnTimer = 0.0;
        #endregion

        #region Methods
        public override void Awake()
        {
            GameObject.Tag = "Pickup";

        }
        public override string ToString()
        {
            return "Pickup";
        }
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

            GameWorld.Instance.Repo.Close();

            Random rndeffect = new Random();
            int randomeffect = rndeffect.Next(0, 3);

            Effects choseneffect = pickupable[randomeffect];

            Debug.WriteLine($"{choseneffect.Effectname}");
            if (choseneffect.ModFK == 1)
            {
                GameWorld.Instance.GameState.PlayerBuilder.Player.dmg += choseneffect.Effect;
                GameWorld.Instance.GameState.DmgColorTimer = true;
            }
            else if (choseneffect.ModFK == 2)
            {
                GameWorld.Instance.GameState.PlayerBuilder.Player.Speed += choseneffect.Effect;
                GameWorld.Instance.GameState.msColorTimer = true;

            }
            else if (choseneffect.ModFK == 3)
            {
                GameWorld.Instance.GameState.PlayerBuilder.Player.Cooldown -= choseneffect.Effect;
                GameWorld.Instance.GameState.asColorTimer = true;

            }
            else if (choseneffect.ModFK == 4)
            {
                GameWorld.Instance.GameState.PlayerBuilder.Player.Health += choseneffect.Effect;
                GameWorld.Instance.GameState.HealthColorTimerGreen = true;

            }


        }
        public void Notify(GameEvent gameEvent, Component component)
        {
            if (gameEvent.Title == "Collision" && component.GameObject.Tag == "Player")
            {
                ApplyMod();
                GameObject.Destroy();
            }
        }
        #endregion
    }
}

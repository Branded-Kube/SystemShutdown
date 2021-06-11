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

        /// <summary>
        /// gives the mods the tag (pickup) which enables the collision event
        /// </summary>
        #region Methods
        public override void Awake()
        {
            GameObject.Tag = "Pickup";

        }
        public override string ToString()
        {
            return "Pickup";
        }
        /// <summary>
        /// Lau
        /// despawn the dropped mods after 15 seconds
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            despawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (despawnTimer > 15)
            {
                GameObject.Destroy();
            }
        }
        /// <summary>
        /// Lau
        /// we create a random between 1,5 to be able to pick from the 4 different effects
        /// then we make a new list of pickupable effects
        /// we create a new random from 0,3 so we can get the different strengths from the picked effect
        /// then we make an if else statement to determine what mod/effect was picked, and adds the value to desired stat
        /// </summary>
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
        /// <summary>
        /// uses collision between player and mod to apply the mod and destroy the gameobject
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <param name="component"></param>
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

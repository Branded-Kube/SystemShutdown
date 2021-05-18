using Microsoft.Xna.Framework;
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
    class PlayerBuilder : IBuilder
    {
        private GameObject1 go;

        private SpriteRenderer sr;

        public SpriteRenderer Sr
        {
            get { return sr; }
            set { sr = value; }
        }

        private Player1 player;

        public Player1 Player
        {
            get { return player; }
            set { player = value; }
        }

        protected Texture2D sprite;
        protected Texture2D[] sprites, upWalk;
        protected float fps;
        protected float timeElapsed;
        protected int currentIndex;

        public void BuildGameObject()
        {
            go = new GameObject1();

            //go.AddComponent(new Player1);
            //go.AddComponent(new SpriteRenderer());
            sr = new SpriteRenderer("1GuyUp");

            go.AddComponent(sr);
            sr.SetSprite("1GuyUp");
            //sr.SetSprite("2GuyUp");
            //sr.SetSprite("3GuyUp");
            sr.Origin = new Vector2(sr.Sprite.Width / 2, (sr.Sprite.Height / 2));
            //sr.Origin = new Vector2(go.Transform.Position.X, go.Transform.Position.Y);

            sr.Rotation = new float();

            player = new Player1();
            go.AddComponent(new Collider(sr, player) { CheckCollisionEvents = true } );
            go.AddComponent(player);
            /// Adds player to collider list
            GameWorld.Instance.AddGameObject(go);

            ////Load sprite sheet
            //sr.Sprites = new Texture2D[3];

            ////Loop animaiton
            //for (int g = 0; g < sr.Sprites.Length; g++)
            //{
            //    sr.Sprites[g] = GameWorld.Instance.Content.Load<Texture2D>(g + 1 + "GuyUp");
            //}
            ////When loop is finished return to first sprite/Sets default sprite
            //sr.Sprite = sr.Sprites[0];
        }

        //public void Animate(GameTime gametime)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.W)/*currentDir.Y == -1*/ || Keyboard.GetState().IsKeyDown(Keys.S)/*currentDir.Y == 1*/ || Keyboard.GetState().IsKeyDown(Keys.D)/*currentDir.X == 1*/ || Keyboard.GetState().IsKeyDown(Keys.A)/*currentDir.X == -1*/)
        //    {
        //        //Giver tiden, der er gået, siden sidste update
        //        timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

        //        //Beregner currentIndex
        //        currentIndex = (int)(timeElapsed * fps);
        //        sr.Sprite = sr.Sprites[currentIndex];

        //        //Checks if animation needs to restart
        //        if (currentIndex >= upWalk.Length - 1)
        //        {
        //            //Resets animation
        //            timeElapsed = 0;
        //            currentIndex = 0;
        //        }
        //    }
        //}

        //public void Update(GameTime gameTime)
        //{
        //    Animate(gametime: gameTime);
        //}

        public GameObject1 GetResult()
        {
            return go;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;

namespace SystemShutdown.CommandPattern
{
    class MoveCommand : ICommand
{
        private Vector2 velocity;
       
        public MoveCommand(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        public void Execute(Player player)
        {
            
            //player.previousPosition = player.position;
            player.Move(velocity);

            //foreach (var item in GameWorld.gameState.grid.nodes)
            //{
            //    if (item.Passable == false)
            //    {
            //        if (player.rectangle.Intersects(item.collisionRectangle))
            //            player.position = player.previousPosition;

            //    }

            //}


        }
    }
}

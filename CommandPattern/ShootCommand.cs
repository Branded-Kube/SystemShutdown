using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.ComponentPattern;
using SystemShutdown.GameObjects;

namespace SystemShutdown.CommandPattern
{
    class ShootCommand : ICommand
    {
        private Vector2 velocity;

        public void Execute(Player1 player)
        {
            player.Shoot();
        }

        //public ShootCommand(Vector2 velocity)
        //{
        //    currentDir = velocity;
        //}

        //public void Execute(Player1 player)
        //{

        //    player.previousPosition = player.position;
        //    player.Move(velocity);

        //    player.previousPosition = player.position;
        //    player.Shoot(velocity, position);

        //    foreach (var item in GameWorld.gameState.grid.nodes)
        //    {
        //        if (item.Passable == false)
        //        {
        //            if (player.rectangle.Intersects(item.collisionRectangle))
        //                player.position = player.previousPosition;
        //        }

        //    }
        //}
    }

} 

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;

namespace SystemShutdown.CommandPattern
{
    class ShootCommand : GameObject, ICommand
    {
        //private Vector2 velocity;

        public ShootCommand(Vector2 velocity)
        {
            currentDir = velocity;
        }

        public void Execute(Player player)
        {

            player.previousPosition = player.position;
            player.Move(velocity);

            player.previousPosition = player.position;
            player.Shoot(velocity, position);

            foreach (var item in GameWorld.gameState.grid.nodes)
            {
                if (item.Passable == false)
                {
                    if (player.rectangle.Intersects(item.collisionRectangle))
                        player.position = player.previousPosition;
                }

}
        }
    }
}

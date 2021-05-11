using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;
using SystemShutdown.States;

namespace SystemShutdown
{
    public class InputHandler
    {
        private Dictionary<Keys, ICommand> keybinds = new Dictionary<Keys, ICommand>();
        /// <summary>
        /// adds the specific keybinds to the dictionary
        /// </summary>
        public InputHandler()
        {
            keybinds.Add(Keys.A, new MoveCommand(new Vector2(-1, 0)));
            keybinds.Add(Keys.D, new MoveCommand(new Vector2(1, 0)));
            keybinds.Add(Keys.W, new MoveCommand(new Vector2(0, -1)));
            keybinds.Add(Keys.S, new MoveCommand(new Vector2(0, 1)));

            keybinds.Add(Keys.Left, new MoveCommand(new Vector2(-1, 0)));
            keybinds.Add(Keys.Right, new MoveCommand(new Vector2(1, 0)));
            keybinds.Add(Keys.Up, new MoveCommand(new Vector2(0, -1)));
            keybinds.Add(Keys.Down, new MoveCommand(new Vector2(0, 1)));
        }
   
        public void Execute(GameState player)
        {
            KeyboardState keyState = Keyboard.GetState();
   
            foreach (Keys key in keybinds.Keys)
            {
                if (keyState.IsKeyDown(key))
                {
                    keybinds[key].Execute(player.Player1Test);
                }
            }
        }
    }
}

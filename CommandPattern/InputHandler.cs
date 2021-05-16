using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using SystemShutdown.GameObjects;

namespace SystemShutdown.CommandPattern
{
    public class InputHandler
    {
        private Dictionary<Keys, ICommand> keybinds = new Dictionary<Keys, ICommand>();

        private static InputHandler instance;

        public static InputHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputHandler();
                }
                return instance;
            }
        }

        public Player1 Entity { get; set; }
        
        /// <summary>
        /// adds the specific keybinds to the dictionary
        /// </summary>
        public InputHandler()
        {
            keybinds.Add(Keys.A, new MoveCommand(new Vector2(-1, 0)));
            keybinds.Add(Keys.D, new MoveCommand(new Vector2(1, 0)));
            keybinds.Add(Keys.W, new MoveCommand(new Vector2(0, -1)));
            keybinds.Add(Keys.S, new MoveCommand(new Vector2(0, 1)));

            //keybinds.Add(Keys.Space, new ShootCommand())
        }
   
        public void Execute(Player1 player)
        {
            KeyboardState keyState = Keyboard.GetState();
   
            foreach (Keys key in keybinds.Keys)
            {
                if (keyState.IsKeyDown(key))
                {
                    //keybinds[key].Execute(player);
                    keybinds[key].Execute(Entity);
                }
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Text;

namespace SystemShutdown
{
    // Lead author: Søren
    public class Highscores
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public int Kills { get; set; }

        public int DaysSurvived { get; set; }


        static public StringBuilder PlayerNameInput = new StringBuilder("UserName");
        public static bool user = true;
        static KeyboardState releasedKey;
        static KeyboardState pressedKey;

        public static void CreateUsernameInput(object sender, TextInputEventArgs e)
        {
            pressedKey = Keyboard.GetState();

            int length = PlayerNameInput.Length;
            if (user == true)
            {

                if (pressedKey.IsKeyDown(Keys.Back) && releasedKey.IsKeyUp(Keys.Back))
                {
                    if (length > 0)
                    {
                        PlayerNameInput.Remove(length - 1, 1);
                    }

                }
                else if (e.Key != Keys.Enter && length <= 20)
                {
                    var character = e.Character;
                    PlayerNameInput.Append(character);
                }

                if (pressedKey.IsKeyDown(Keys.Enter) && releasedKey.IsKeyUp(Keys.Enter))
                {
                    user = false;
                    Debug.WriteLine($"{PlayerNameInput}");
                }
                pressedKey = releasedKey;
            }

        }

    }
}

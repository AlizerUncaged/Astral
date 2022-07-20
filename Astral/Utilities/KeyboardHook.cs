using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    // This is too slow.
    public class KeyboardHook : IUtility
    {
        private IKeyboardMouseEvents globalInterfaceHook;

        private List<Keys> pressedKeys = new();

        public KeyboardHook()
        {
            //globalInterfaceHook = Hook.GlobalEvents();

            //Console.WriteLine($"Keyboard hooked.");

            //globalInterfaceHook.KeyDown += KeyboardKeyPressed;
            //globalInterfaceHook.KeyUp += KeyboardKeyReleased;
        }

        private void KeyboardKeyReleased(object? sender, KeyEventArgs e)
        {
            //Console.WriteLine($"Released {e.KeyCode}");

            //while (pressedKeys.Contains(e.KeyCode))
            //    pressedKeys.Remove(e.KeyCode);
        }

        private void KeyboardKeyPressed(object? sender, KeyEventArgs e)
        {
            //Console.WriteLine($"Pressed {e.KeyCode}");
            //if (!pressedKeys.Contains(e.KeyCode))
            //    pressedKeys.Add(e.KeyCode);
        }

        /// <summary>
        /// Checks if a key is pressed.
        /// </summary>
        public bool IsKeyPressed(Keys key) => pressedKeys.Contains(key);
    }
}

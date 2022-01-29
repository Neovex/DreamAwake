using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using SFML.System;
using SFML.Window;
using BlackCoat;
using BlackCoat.InputMapping;
using BlackCoat.Collision.Shapes;

namespace DreamAwake
{
    static class Game
    {
        public static string TITLE => "Dream Awake";


        public static void SetupInput(SimpleInputMap<GameAction> map)
        {
            map.AddKeyboardMapping(Keyboard.Key.A, GameAction.Left);
            map.AddKeyboardMapping(Keyboard.Key.Left, GameAction.Left);

            map.AddKeyboardMapping(Keyboard.Key.D, GameAction.Right);
            map.AddKeyboardMapping(Keyboard.Key.Right, GameAction.Right);

            map.AddKeyboardMapping(Keyboard.Key.W, GameAction.Jump);
            map.AddKeyboardMapping(Keyboard.Key.Up, GameAction.Jump);
            map.AddKeyboardMapping(Keyboard.Key.Space, GameAction.Jump);

            map.AddKeyboardMapping(Keyboard.Key.F, GameAction.Activate);
            map.AddKeyboardMapping(Keyboard.Key.RControl, GameAction.Activate);
        }
    }
}
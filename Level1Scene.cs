using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackCoat;
using BlackCoat.Entities.Shapes;
using BlackCoat.InputMapping;
using SFML.Graphics;
using SFML.System;

namespace DreamAwake
{
    class Level1Scene : Scene
    {
        private SimpleInputMap<GameAction> _InputMap;
        private Player _Player;

        public Level1Scene(Core core) : base(core, "Level1", "Assets")
        {
        }

        protected override bool Load()
        {
            _InputMap = new SimpleInputMap<GameAction>(Input);
            _InputMap.MappedOperationInvoked += HandleInput;
            Game.SetupInput(_InputMap);

            _Player = new Player(_Core, _InputMap);
            Layer_Game.Add(_Player);

            return true;
        }

        private void HandleInput(GameAction action, bool activate)
        {
            switch (action)
            {
                case GameAction.Left:
                    break;
                case GameAction.Right:
                    break;
                case GameAction.Jump:
                    break;
                case GameAction.Activate:
                    break;
                default:
                    break;
            }
        }

        protected override void Update(float deltaT)
        {
        }

        protected override void Destroy()
        {
        }
    }
}

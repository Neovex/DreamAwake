using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackCoat;
using BlackCoat.Entities;
using BlackCoat.Entities.Shapes;
using BlackCoat.InputMapping;
using SFML.System;
using SFML.Graphics;

namespace DreamAwake
{
    class Player : Container
    {

        private SimpleInputMap<GameAction> _InputMap;
        private Circle _Dot;
        private float _DotRadius = 5f;

        private Vector2f _Direction;

        private float _MovementSpeed = 100f;
        private float _JumpForce = 10f;

        private float _GroundLevel = 200;




        public Player(Core core, SimpleInputMap<GameAction> inputMap) : base(core)
        {
            _InputMap = inputMap;
            _InputMap.MappedOperationInvoked += HandleInput;
            
            _Dot = new Circle(_Core, _DotRadius, Color.Blue, Color.Yellow);
            _Dot.Position = new Vector2f(360, 260);

            Add(_Dot);

            
        }


        private void HandleInput(GameAction action, bool activate)
        {
            switch (action)
            {
                case GameAction.Left:
                    if (activate)
                        _Direction = new Vector2f(-1, _Direction.Y);
                    else
                        _Direction = new Vector2f(0, _Direction.Y);
                    break;
                case GameAction.Right:
                    if (activate)
                        _Direction = new Vector2f(1, _Direction.Y);
                    else
                        _Direction = new Vector2f(0, _Direction.Y);
                    break;
                case GameAction.Jump:
                    if (activate)
                        _Direction = new Vector2f(_Direction.X, -1);
                    else
                        _Direction = new Vector2f(_Direction.X, 1);
                    break;
                case GameAction.Activate:
                    break;
                default:
                    break;
            }
        }


        public override void Update(float deltaT)
        {

            _Dot.Position += _Direction * _MovementSpeed * deltaT;

            if (_Dot.Position.Y >= _GroundLevel)
                _Dot.Position = new Vector2f(_Dot.Position.X, _GroundLevel);

        }
    }
}

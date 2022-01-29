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
        private Vector2f _Velocity;

        public float _MovementSpeed { get; set; }
        public float _JumpForce { get; set; }
        public float _GravityForce { get; set; }

        private float _GroundLevel = 500;

        private bool _IsGrounded = true;
        private bool _HasJumped = false;




        public Player(Core core, SimpleInputMap<GameAction> inputMap) : base(core)
        {
            _InputMap = inputMap;
            _InputMap.MappedOperationInvoked += HandleInput;
            
            _Dot = new Circle(_Core, _DotRadius, Color.Blue, Color.Yellow);
            _Dot.Position = new Vector2f(360, 500);

            _JumpForce = 750f;
            _GravityForce = -9.81f;
            _MovementSpeed = 100f;

            Add(_Dot);
            

            
        }


        private void HandleInput(GameAction action, bool activate)
        {
            switch (action)
            {
                case GameAction.Left:
                    if (activate)
                        _Direction = new Vector2f(-_MovementSpeed, _Direction.Y);
                    else
                        _Direction = new Vector2f(0, _Direction.Y);
                    break;
                case GameAction.Right:
                    if (activate)
                        _Direction = new Vector2f(_MovementSpeed, _Direction.Y);
                    else
                        _Direction = new Vector2f(0, _Direction.Y);
                    break;
                case GameAction.Jump:
                    if (activate && (_Dot.Position.Y == _GroundLevel))
                        _HasJumped = true;                    
                    break;    
                case GameAction.Activate:
                    break;
                default:
                    break;  
            }
        }

        public override void Update(float deltaT)
        {

            //IsGrounded();

            if (_HasJumped)
            {
                _Velocity = new Vector2f(0, -_JumpForce);
                _HasJumped = false;

            }
            else
            {
                _Velocity -= new Vector2f(0, _GravityForce);
                
            }


            _Dot.Position += (_Direction + _Velocity) * deltaT;

            if (_Dot.Position.Y >= _GroundLevel)
                _Dot.Position = new Vector2f(_Dot.Position.X, _GroundLevel);
         
        }


        private void IsGrounded()
        {         
            _IsGrounded = _Dot.Position.Y == _GroundLevel;
        }


    }
}

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
using BlackCoat.Entities.Animation;


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


        // animation lists
        private IntRect[] _IdleShadowDefinitionFrames;
        private int _SizeIdleShadowAnim = 2;
        private BlittingAnimation _IdleShadowAnimation;
        private float _durationIdleShadow = 175f;

        private IntRect[] _IdleNormalDefinitionFrames;
        private int _SizeIdleNormalAnim = 2;
        private BlittingAnimation _IdleNormalAnimation;
        private float _durationIdleNormal = 175f;

        private IntRect[] _JumpShadowDefinitionFrames;
        private int _SizeJumpShadowAnim = 4;
        private BlittingAnimation _JumpShadowAnimation;
        private float _durationJumpShadow = 175f;

        private IntRect[] _JumpNormalDefinitionFrames;
        private int _SizeJumpNormalAnim = 4;
        private BlittingAnimation _JumpNormalAnimation;
        private float _durationJumpNormal = 175f;

        private IntRect[] _WalkShadowDefinitionFrames;
        private int _SizeWalkShadowAnim = 4;
        private BlittingAnimation _WalkShadowAnimation;
        private float _durationWalkShadow = 175f;

        private IntRect[] _WalkNormalDefinitionFrames;
        private int _SizeWalkNormalAnim = 4;
        private BlittingAnimation _WalkNormalAnimation;
        private float _durationWalkNormal = 0.175f;


        public Player(Core core, SimpleInputMap<GameAction> inputMap, TextureLoader textureLoader) : base(core)
        {
            _InputMap = inputMap;
            _InputMap.MappedOperationInvoked += HandleInput;

            _Dot = new Circle(_Core, _DotRadius, Color.Blue, Color.Yellow);
            _Dot.Position = new Vector2f(360, 500);

            _JumpForce = 750f;
            _GravityForce = -9.81f;
            _MovementSpeed = 100f;

            Add(_Dot);

            _IdleShadowDefinitionFrames = new IntRect[_SizeIdleShadowAnim];
            IntRectFill(_IdleShadowDefinitionFrames, _SizeIdleShadowAnim);
            _IdleShadowAnimation = new BlittingAnimation(_Core, _durationIdleShadow, textureLoader.Load("Idle"), _IdleShadowDefinitionFrames);
            //_IdleShadowAnimation.Position = new Vector2f(200, 200);
            // Add(_IdleShadowAnimation);


            _IdleNormalDefinitionFrames = new IntRect[_SizeIdleNormalAnim];
            IntRectFill(_IdleNormalDefinitionFrames, _SizeIdleNormalAnim);
            _IdleNormalAnimation = new BlittingAnimation(_Core, _durationIdleNormal, textureLoader.Load("Idle_Normal"), _IdleNormalDefinitionFrames);

            _JumpShadowDefinitionFrames = new IntRect[_SizeJumpShadowAnim];
            IntRectFill(_JumpShadowDefinitionFrames, _SizeJumpShadowAnim);
            _JumpShadowAnimation = new BlittingAnimation(_Core, _durationJumpShadow, textureLoader.Load("Jump"), _JumpShadowDefinitionFrames);

            _JumpNormalDefinitionFrames = new IntRect[_SizeJumpNormalAnim];
            IntRectFill(_JumpNormalDefinitionFrames, _SizeJumpNormalAnim);
            _JumpNormalAnimation = new BlittingAnimation(_Core, _durationJumpNormal, textureLoader.Load("Jump_Normal"), _JumpNormalDefinitionFrames);

            _WalkShadowDefinitionFrames = new IntRect[_SizeWalkShadowAnim];
            IntRectFill(_WalkShadowDefinitionFrames, _SizeWalkShadowAnim);
            _WalkShadowAnimation = new BlittingAnimation(_Core, _durationWalkShadow, textureLoader.Load("Walk"), _WalkShadowDefinitionFrames);
            //_WalkShadowAnimation.Position = new Vector2f(100, 100);
            //Add(_WalkShadowAnimation);


            _WalkNormalDefinitionFrames = new IntRect[_SizeWalkNormalAnim];
            IntRectFill(_WalkNormalDefinitionFrames, _SizeWalkNormalAnim);
            _WalkNormalAnimation = new BlittingAnimation(_Core, _durationWalkNormal, textureLoader.Load("Walk_Normal"), _WalkNormalDefinitionFrames);
            _WalkNormalAnimation.Position = new Vector2f(200, 200);
            Add(_WalkNormalAnimation);
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
            base.Update(deltaT);
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


        private void IntRectFill(IntRect[] DefinitionFrames, int FramesInAnimation )
        {
            for (int i = 0; i < FramesInAnimation; i++)
            {
                DefinitionFrames[i] = new IntRect(i* 32, 0, 32, 32);
            }

        }



        private void IsGrounded()
        {         
            _IsGrounded = _Dot.Position.Y == _GroundLevel;
        }


    }
}

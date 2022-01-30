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
using BlackCoat.AssetHandling;
using SFML.Audio;

namespace DreamAwake
{

    class Player : Container
    {
        public Vector2f PlayerPosition { get => _Dot.Position; set => _Dot.Position = value; }
        public Func<Vector2f, bool> OnGround { get; set; }
        public Func<Vector2f, bool> AtWall { get; set; }

        private SimpleInputMap<GameAction> _InputMap;
        private SfxLoader _SfxLoader;

        private Circle _Dot;
        private float _DotRadius = 5f;

        private Vector2f _Direction;
        private Vector2f _Velocity;
        private Vector2f _CharacterAnimationOffset = new Vector2f(16, -32);

        public float _MovementSpeed { get; set; }
        public float _JumpForce { get; set; }
        public float _GravityForce { get; set; }

        private bool _IsGrounded = true;
        private bool _HasJumped = false;
        private bool _InTheDark = false;


        // animation lists
        private IntRect[] _IdleShadowDefinitionFrames;
        private int _SizeIdleShadowAnim = 2;
        private BlittingAnimation _IdleShadowAnimation;
        private float _durationIdleShadow = 0.175f;

        private IntRect[] _IdleNormalDefinitionFrames;
        private int _SizeIdleNormalAnim = 2;
        private BlittingAnimation _IdleNormalAnimation;
        private float _durationIdleNormal = 0.175f;

        private IntRect[] _JumpShadowDefinitionFrames;
        private int _SizeJumpShadowAnim = 4;
        private BlittingAnimation _JumpShadowAnimation;
        private float _durationJumpShadow = 0.175f;

        private IntRect[] _JumpNormalDefinitionFrames;
        private int _SizeJumpNormalAnim = 4;
        private BlittingAnimation _JumpNormalAnimation;
        private float _durationJumpNormal = 0.175f;

        private IntRect[] _WalkShadowDefinitionFrames;
        private int _SizeWalkShadowAnim = 4;
        private BlittingAnimation _WalkShadowAnimation;
        private float _durationWalkShadow = 0.09f;

        private IntRect[] _WalkNormalDefinitionFrames;
        private int _SizeWalkNormalAnim = 4;
        private BlittingAnimation _WalkNormalAnimation;
        private float _durationWalkNormal = 0.09f;

        // SFX 
        private string _RandomString = "";
        SfxManager _SoundManager;



        public Player(Core core, SimpleInputMap<GameAction> inputMap, TextureLoader textureLoader, SfxLoader sfxLoader) : base(core)
        {
            _InputMap = inputMap;
            _InputMap.MappedOperationInvoked += HandleInput;
            _SfxLoader = sfxLoader;

            _Dot = new Circle(_Core, _DotRadius, Color.Blue, Color.Yellow);
            _Dot.Position = new Vector2f(360, 500);
            _Dot.Visible = false;

            // values for movement speed and jump,gravity forces 
            _JumpForce = 160f;
            _GravityForce = -2f;
            _MovementSpeed = 100f;

            Add(_Dot);

            // animation assets to define and load
            _IdleShadowDefinitionFrames = new IntRect[_SizeIdleShadowAnim];
            IntRectFill(_IdleShadowDefinitionFrames, _SizeIdleShadowAnim);
            _IdleShadowAnimation = new BlittingAnimation(_Core, _durationIdleShadow, textureLoader.Load("Idle"), _IdleShadowDefinitionFrames);
            Add(_IdleShadowAnimation);

            _IdleNormalDefinitionFrames = new IntRect[_SizeIdleNormalAnim];
            IntRectFill(_IdleNormalDefinitionFrames, _SizeIdleNormalAnim);
            _IdleNormalAnimation = new BlittingAnimation(_Core, _durationIdleNormal, textureLoader.Load("Idle_Normal"), _IdleNormalDefinitionFrames);
            Add(_IdleNormalAnimation);

            _JumpShadowDefinitionFrames = new IntRect[_SizeJumpShadowAnim];
            IntRectFill(_JumpShadowDefinitionFrames, _SizeJumpShadowAnim);
            _JumpShadowAnimation = new BlittingAnimation(_Core, _durationJumpShadow, textureLoader.Load("Jump"), _JumpShadowDefinitionFrames);
            Add(_JumpShadowAnimation);

            _JumpNormalDefinitionFrames = new IntRect[_SizeJumpNormalAnim];
            IntRectFill(_JumpNormalDefinitionFrames, _SizeJumpNormalAnim);
            _JumpNormalAnimation = new BlittingAnimation(_Core, _durationJumpNormal, textureLoader.Load("Jump_Normal"), _JumpNormalDefinitionFrames);
            Add(_JumpNormalAnimation);

            _WalkShadowDefinitionFrames = new IntRect[_SizeWalkShadowAnim];
            IntRectFill(_WalkShadowDefinitionFrames, _SizeWalkShadowAnim);
            _WalkShadowAnimation = new BlittingAnimation(_Core, _durationWalkShadow, textureLoader.Load("Walk"), _WalkShadowDefinitionFrames);
            Add(_WalkShadowAnimation);

            _WalkNormalDefinitionFrames = new IntRect[_SizeWalkNormalAnim];
            IntRectFill(_WalkNormalDefinitionFrames, _SizeWalkNormalAnim);
            _WalkNormalAnimation = new BlittingAnimation(_Core, _durationWalkNormal, textureLoader.Load("Walk_Normal"), _WalkNormalDefinitionFrames);
            Add(_WalkNormalAnimation);


            // sets origin for animation in dependance of the player controller 
            _WalkNormalAnimation.Origin = new Vector2f(16, 32);
            _IdleShadowAnimation.Origin = new Vector2f(16, 32);
            _IdleNormalAnimation.Origin = new Vector2f(16, 32);
            _JumpShadowAnimation.Origin = new Vector2f(16, 32);
            _JumpNormalAnimation.Origin = new Vector2f(16, 32);
            _WalkShadowAnimation.Origin = new Vector2f(16, 32);


            _SoundManager = new SfxManager(_SfxLoader, () => 100);
            _SoundManager.LoadFromDirectory("Assets\\SFX", 1);

        }


        private void HandleInput(GameAction action, bool activate)
        {



            switch (action)
            {
                case GameAction.Left:
                    if (activate)
                    { 
                        _Direction = new Vector2f(-_MovementSpeed, _Direction.Y);

                        // changes animation orintation
                        _IdleShadowAnimation.Scale = new Vector2f(-1, 1);
                        _IdleNormalAnimation.Scale = new Vector2f(-1, 1);
                        _JumpShadowAnimation.Scale = new Vector2f(-1, 1);
                        _JumpNormalAnimation.Scale = new Vector2f(-1, 1);
                        _WalkShadowAnimation.Scale = new Vector2f(-1, 1);
                        _WalkNormalAnimation.Scale = new Vector2f(-1, 1);

                    }
                    else
                    {
                        _Direction = new Vector2f(0, _Direction.Y);
                    }
                    break;
                case GameAction.Right:
                    if (activate)
                    {
                        _Direction = new Vector2f(_MovementSpeed, _Direction.Y);

                        // changes animation orintation
                        _IdleShadowAnimation.Scale = new Vector2f(1, 1);
                        _IdleNormalAnimation.Scale = new Vector2f(1, 1);
                        _JumpShadowAnimation.Scale = new Vector2f(1, 1);
                        _JumpNormalAnimation.Scale = new Vector2f(1, 1);
                        _WalkShadowAnimation.Scale = new Vector2f(1, 1);
                        _WalkNormalAnimation.Scale = new Vector2f(1, 1);

                    }
                    else
                    {
                        _Direction = new Vector2f(0, _Direction.Y);
                    }
                    break;
                case GameAction.Jump:
                    if (activate && _IsGrounded)
                    {
                        _HasJumped = true;
                        _SoundManager.Play(RandomSoundStringGenerator());

                    }
                    break;
                case GameAction.Activate:
                    {
                        if (activate)
                        {
                            if (_InTheDark)
                            {
                                _InTheDark = false;
                                _SoundManager.Play("SwitchDarkToLightSound_01");

                            }
                            else
                            {
                                _InTheDark = true;
                                _SoundManager.Play("SwitchLightToDarkSound_01");

                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Update(float deltaT)
        {
            base.Update(deltaT);

            if (_HasJumped)
            {
                _Velocity = new Vector2f(0, -_JumpForce);
                _HasJumped = false;
            }
            else
            {
                _Velocity -= new Vector2f(0, _GravityForce);
            }

            var movement = _Direction + _Velocity;
            if (AtWall(_Dot.Position + new Vector2f(16 * _IdleShadowAnimation.Scale.X, -16)))
                movement = new Vector2f(0, movement.Y);
            if (_IsGrounded = OnGround(_Dot.Position) && movement.Y > 0)
                movement = new Vector2f(movement.X, 0);

            // Perform Movement
            _Dot.Position += movement * deltaT;


            // animation logic
            if (_IsGrounded && (_Direction.X == 0))
            {
                if (_InTheDark)
                {
                    _IdleShadowAnimation.Visible = true;
                    _IdleNormalAnimation.Visible = false;
                    _JumpShadowAnimation.Visible = false;
                    _JumpNormalAnimation.Visible = false;
                    _WalkShadowAnimation.Visible = false;
                    _WalkNormalAnimation.Visible = false;


                }
                else
                {
                    _IdleShadowAnimation.Visible = false;
                    _IdleNormalAnimation.Visible = true;
                    _JumpShadowAnimation.Visible = false;
                    _JumpNormalAnimation.Visible = false;
                    _WalkShadowAnimation.Visible = false;
                    _WalkNormalAnimation.Visible = false;
                }
            }
            else if (_IsGrounded)
            {
                if (_InTheDark)
                {
                    _IdleShadowAnimation.Visible = false;
                    _IdleNormalAnimation.Visible = false;
                    _JumpShadowAnimation.Visible = false;
                    _JumpNormalAnimation.Visible = false;
                    _WalkShadowAnimation.Visible = true;
                    _WalkNormalAnimation.Visible = false;
                    _SoundManager.Play(RandomSoundStringGenerator());

                }
                else
                {
                    _IdleShadowAnimation.Visible = false;
                    _IdleNormalAnimation.Visible = false;
                    _JumpShadowAnimation.Visible = false;
                    _JumpNormalAnimation.Visible = false;
                    _WalkShadowAnimation.Visible = false;
                    _WalkNormalAnimation.Visible = true;
                    _SoundManager.Play(RandomSoundStringGenerator());

                }
            }
            else
            {
                if (_InTheDark)
                {
                    _IdleShadowAnimation.Visible = false;
                    _IdleNormalAnimation.Visible = false;
                    _JumpShadowAnimation.Visible = true;
                    _JumpNormalAnimation.Visible = false;
                    _WalkShadowAnimation.Visible = false;
                    _WalkNormalAnimation.Visible = false;
                }
                else
                {
                    _IdleShadowAnimation.Visible = false;
                    _IdleNormalAnimation.Visible = false;
                    _JumpShadowAnimation.Visible = false;
                    _JumpNormalAnimation.Visible = true;
                    _WalkShadowAnimation.Visible = false;
                    _WalkNormalAnimation.Visible = false;
                }
            }


            _IdleShadowAnimation.Position = _Dot.Position;
            _IdleNormalAnimation.Position = _Dot.Position;
            _JumpShadowAnimation.Position = _Dot.Position;
            _JumpNormalAnimation.Position = _Dot.Position;
            _WalkShadowAnimation.Position = _Dot.Position;
            _WalkNormalAnimation.Position = _Dot.Position;


            PlayerPosition = _Dot.Position;
        }


        private void IntRectFill(IntRect[] DefinitionFrames, int FramesInAnimation )
        {
            for (int i = 0; i < FramesInAnimation; i++)
            {
                DefinitionFrames[i] = new IntRect(i* 32, 0, 32, 32);
            }

        }


        private string RandomSoundStringGenerator()
        {

            Random randomNumber = new Random();

            if (_IsGrounded && (_Direction.X != 0) && !_HasJumped)
            {
                if (_InTheDark)
                {
                    _RandomString = "FootstepDark_0" + randomNumber.Next(1, 7).ToString();
                }
                else
                {
                    _RandomString = "FootstepLight_0" + randomNumber.Next(1, 7).ToString();
                }

            }
            else if (_HasJumped)
            {
                if (_InTheDark)
                {
                    _RandomString = "JumpSoundDark_0" + randomNumber.Next(1, 5).ToString();
                }
                else
                {
                    _RandomString = "JumpSoundLight_0" + randomNumber.Next(1, 5).ToString();
                }
            }
            else
            {
                if (_InTheDark)
                {
                    _RandomString = "LandingSoundDark_0" + randomNumber.Next(1, 5).ToString();
                }
                else
                {
                    _RandomString = "LandingSoundLight_0" + randomNumber.Next(1, 5).ToString();
                }
            }


            return _RandomString;
        }

        public void DeathSoundFX()
        {
            _SoundManager.Play("DeathFallSound_01");
        }

    }


}

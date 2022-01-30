using System;
using System.Collections.Generic;
using System.Linq;
using BlackCoat;
using BlackCoat.Entities;
using BlackCoat.Entities.Shapes;
using SFML.Graphics;
using SFML.System;

namespace DreamAwake
{
    class MainMenuScene : Scene
    {
        private Container _Container;
        private Rectangle _Start;
        private Rectangle _Exit;

        public MainMenuScene(Core core) : base(core, "MainMenu", "Assets")
        {
        }

        protected override bool Load()
        {
            _Core.DeviceResized += HandleDeviceResized;

            var tex = TextureLoader.Load("MainMenue");
            _Container = new Container(_Core);
            _Container.Scale = new Vector2f(4, 4);
            _Container.Texture = tex;
            Layer_Game.Add(_Container);

            _Start = new Rectangle(_Core, new Vector2f(31, 12), Color.Blue);
            _Start.Position = new Vector2f(53, 98);
            _Start.Alpha = 0;
            _Container.Add(_Start);

            _Exit = new Rectangle(_Core, new Vector2f(22, 12), Color.Blue);
            _Exit.Position = new Vector2f(187, 98);
            _Exit.Alpha = 0;
            _Container.Add(_Exit);

            HandleDeviceResized(_Core.DeviceSize);
            return true;
        }

        private void HandleDeviceResized(Vector2f size)
        {
            
        }

        protected override void Update(float deltaT)
        {
            if (Input.LeftMouseButtonPressed)
            {
                if (_Start.CollidesWith(Input.MousePosition / _Container.Scale.X)) Game.LoadNextLevel();
                if (_Exit.CollidesWith(Input.MousePosition / _Container.Scale.X)) _Core.Exit();
                //Log.Debug(Input.MousePosition / _Container.Scale.X, _Exit.Position);
            }
        }

        protected override void Destroy()
        {
        }
    }
}
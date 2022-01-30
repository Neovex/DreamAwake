using System;
using System.Collections.Generic;
using System.Linq;
using BlackCoat;
using BlackCoat.Entities;

namespace DreamAwake
{
    class MainMenuScene : Scene
    {
        public MainMenuScene(Core core) : base(core, "MainMenu", "Assets")
        {
        }

        protected override bool Load()
        {
            Layer_Game.Add(new TextItem(_Core, "Start") { Position = _Core.DeviceSize / 2 });
            return true;
        }

        protected override void Update(float deltaT)
        {
            if (Input.LeftMouseButtonPressed &&
              Layer_Game.GetFirst<TextItem>().CollisionShape.CollidesWith(Input.MousePosition))
                Game.LoadNextLevel();
        }

        protected override void Destroy()
        {
        }
    }
}

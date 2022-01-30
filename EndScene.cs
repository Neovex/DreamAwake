using System;
using System.Collections.Generic;
using System.Linq;
using BlackCoat;
using BlackCoat.Entities;

namespace DreamAwake
{
    class EndScene : Scene
    {
        public EndScene(Core core) : base(core, "WinScreen", "Assets")
        {
        }

        protected override bool Load()
        {
            Layer_Game.Add(new TextItem(_Core, "You Win") { Position = _Core.DeviceSize / 2 });
            return true;
        }

        protected override void Update(float deltaT)
        {
        }

        protected override void Destroy()
        {
        }
    }
}

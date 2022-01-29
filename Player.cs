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
        private float dotRadius = 5f;


        public Player(Core core, SimpleInputMap<GameAction> inputMap) : base(core)
        {
            _InputMap = inputMap;
            _Dot = new Circle(_Core, dotRadius, Color.Blue, Color.Yellow);
            _Dot.Position = new Vector2f(360, 260);
            Add(_Dot);
        }

        public override void Update(float deltaT)
        {
            base.Update(deltaT);
        }
    }
}

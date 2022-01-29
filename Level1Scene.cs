using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackCoat;
using BlackCoat.Entities;
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
        private MapRenderer _Map;

        public Level1Scene(Core core) : base(core, "Level1", "Assets")
        {
        }

        protected override bool Load()
        {
            _InputMap = new SimpleInputMap<GameAction>(Input);
            _InputMap.MappedOperationInvoked += HandleInput;
            Game.SetupInput(_InputMap);

            // Player
            _Player = new Player(_Core, _InputMap);
            Layer_Game.Add(_Player);

            // Tile Map
            var tex = TextureLoader.Load("tiles_28");
            var mapData = new MapData();
            mapData.Load(_Core, "Assets\\test.tmx");
            foreach (var layer in mapData.Layer)
            {
                var mapRenderer = new MapRenderer(_Core, mapData.MapSize, tex, mapData.TileSize);
                for (int i = 0; i < layer.Length; i++)
                {
                    mapRenderer.AddTile(i * 4, layer[i].Pos, layer[i].Cod);
                }
                Layer_Background.Add(mapRenderer);
            }

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

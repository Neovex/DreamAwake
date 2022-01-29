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
        private View _View;
        private CollisionLayer[] _Collisions;

        private bool _Light;

        public bool Light
        {
            get { return _Light; }
            set
            {
                if (_Light = value)
                {
                    _Core.ClearColor = new Color(0xcfe5ff);
                }
                else
                {
                    _Core.ClearColor = new Color(0x272727);
                }
                foreach (var renderer in Layer_Background.GetAll<MapRenderer>())
                {
                    renderer.Visible = renderer.IsLight == _Light;
                }
            }
        }


        public Level1Scene(Core core) : base(core, "Level1", "Assets")
        { }


        protected override bool Load()
        {
            // Input
            _InputMap = new SimpleInputMap<GameAction>(Input);
            _InputMap.MappedOperationInvoked += HandleInput;
            Game.SetupInput(_InputMap);

            // View
            _View = new View(_Core.DeviceSize / 2, _Core.DeviceSize);
            _Core.DeviceResized += s => _View.Size = s;
            Layer_Background.View = _View;
            Layer_Game.View = _View;

            // Tile Map
            var tex = TextureLoader.Load("MasterTileset");
            var mapData = new MapData();
            mapData.Load(_Core, "Assets\\Level1.tmx");
            foreach (var layer in mapData.Layer)
            {
                var mapRenderer = new MapRenderer(_Core, mapData.MapSize, tex, mapData.TileSize);
                mapRenderer.IsLight = layer.IsLight;
                for (int i = 0; i < layer.Tiles.Length; i++)
                {
                    mapRenderer.AddTile(i * 4, layer.Tiles[i].Position, layer.Tiles[i].Coordinates);
                }
                Layer_Background.Add(mapRenderer);
            }

            // Collision Layer
            _Collisions = mapData.CollisionLayer;

            // Player
            _Player = new Player(_Core, _InputMap, TextureLoader)
            {
                PlayerPosition = _Collisions.SelectMany(l => l.Collisions).First(c => c.Type == CollisionType.Start).Shape.Position,
                AtWall = pos => _Collisions.Where(l => l.IsLight == Light).SelectMany(l => l.Collisions).Any(c => c.Type == CollisionType.Normal && c.CollidesWith(pos)),
                OnGround = pos => _Collisions.Where(l => l.IsLight == Light).SelectMany(l => l.Collisions).Any(c => c.Type == CollisionType.Normal && c.CollidesWith(pos)),
            };
            Layer_Game.Add(_Player);
            //OpenInspector(_Player);

            // Start Map
            Light = true;
            return true;
        }

        private void HandleInput(GameAction action, bool activate)
        {
            if (action == GameAction.Activate && activate) Light = !Light;
        }

        protected override void Update(float deltaT)
        {
            foreach (var collision in _Collisions.Where(l => l.IsLight == Light).SelectMany(l => l.Collisions))
            {
                if (collision.CollidesWith(_Player.PlayerPosition))
                {
                    switch (collision.Type)
                    {
                        case CollisionType.Killzone: // Respawn
                            _Player.PlayerPosition = _Collisions.SelectMany(l => l.Collisions).First(c => c.Type == CollisionType.Start).Shape.Position;
                            break;
                        case CollisionType.Goal:
                            // TODO : win - load next level via Game class
                            break;
                    }
                }
            }
            _View.Center = _Player.PlayerPosition;
        }

        protected override void Destroy()
        {
        }
    }
}

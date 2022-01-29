﻿using System;
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
        {
        }

        protected override bool Load()
        {
            // Input
            _InputMap = new SimpleInputMap<GameAction>(Input);
            _InputMap.MappedOperationInvoked += HandleInput;
            Game.SetupInput(_InputMap);

            // View
            _View = new View(_Core.DeviceSize / 2, _Core.DeviceSize);
            _Core.DeviceResized += s => _View.Size = s;
            //Layer_Background.View = _View;
            //Layer_Game.View = _View;

            // Player
            _Player = new Player(_Core, _InputMap, TextureLoader);
            Layer_Game.Add(_Player);
            OpenInspector(_Player);

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


            Light = true;
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
                    if(activate) Light = !Light;
                    break;
            }
        }

        protected override void Update(float deltaT)
        {
            _View.Center = _Player.CameraFocus;
        }

        protected override void Destroy()
        {
        }
    }
}

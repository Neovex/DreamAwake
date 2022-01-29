using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackCoat;
using BlackCoat.Entities;
using BlackCoat.Entities.Shapes;
using BlackCoat.InputMapping;
using SFML.Audio;
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

        private Music _BaseMusic;
        private Music _LightMusic;
        private Music _DarkMusic;
        private Music _AmbienceLight;
        private Music _AmbienceDark;

        public float _BaseMusicVolume { get; set; }
        public float _LightMusicVolume { get; set; }
        public float _DarkMusicVolume { get; set; }
        public float _AmbienceLightVolume { get; set; }
        public float _AmbienceDarkVolume { get; set; }

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
                    
            _BaseMusicVolume = 80;
            _LightMusicVolume = 100;
            _DarkMusicVolume = 100;
            _AmbienceLightVolume = 40;
            _AmbienceDarkVolume = 30;

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

            // Music
            _BaseMusic = MusicLoader.Load("GameJam_DreamWake_BasicLayer002");
            _LightMusic = MusicLoader.Load("GameJam_DreamWake_LightWorld002");
            _DarkMusic = MusicLoader.Load("GameJam_DreamWake_DarkWorld002");
            _AmbienceLight = MusicLoader.Load("AmbienceLoopLight_01");
            _AmbienceDark = MusicLoader.Load("AmbienceLoopDark_01");
            _BaseMusic.Play();
            _LightMusic.Play();
            _DarkMusic.Play();
            _AmbienceLight.Play();
            _AmbienceDark.Play();
            _BaseMusic.Loop = true;
            _LightMusic.Loop = true;
            _DarkMusic.Loop = true;
            _AmbienceLight.Loop = true;
            _AmbienceDark.Loop = true;


            _BaseMusic.Volume = _BaseMusicVolume;
            _LightMusic.Volume = _LightMusicVolume;
            _DarkMusic.Volume = 0;
            _AmbienceLight.Volume = _AmbienceLightVolume;
            _AmbienceDark.Volume = 0;

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

            if(Light)
            {
                _DarkMusic.Volume = 0;
                _LightMusic.Volume = _LightMusicVolume;
                _AmbienceDark.Volume = 0;
                _AmbienceLight.Volume = _AmbienceLightVolume;
            }
            else if (!Light)
            {
                _DarkMusic.Volume = _DarkMusicVolume;
                _LightMusic.Volume = 0;
                _AmbienceDark.Volume = _AmbienceDarkVolume;
                _AmbienceLight.Volume = 0;

            }
        }

        protected override void Destroy()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BlackCoat;
using BlackCoat.Collision.Shapes;
using SFML.Graphics;
using SFML.System;

namespace DreamAwake
{
    class MapData
    {
        public Vector2i MapSize { get; private set; }
        public Vector2i TileSize { get; private set; }
        public (Vector2f Pos, Vector2i Cod)[][] Layer { get; private set; }

        public IEnumerable<(String type, RectangleCollisionShape collision)> Load(Core core, string file)
        {
            var map = new List<(String type, RectangleCollisionShape collision)>();
            var root = XElement.Load(file);
            var entities = root.Element("objectgroup");
            foreach (var item in entities.Elements())
            {
                var pos = new Vector2f((float)item.Attribute("x"), (float)item.Attribute("y"));
                var size = new Vector2f((float)item.Attribute("width"), (float)item.Attribute("height"));
                //map.Add((item.Attribute("type").Value, new RectangleCollisionShape(core.CollisionSystem, pos, size)));
            }


            MapSize = new Vector2i((int)root.Attribute("width"), (int)root.Attribute("height"));
            TileSize = new Vector2i((int)root.Attribute("tilewidth"), (int)root.Attribute("tileheight"));
            var columns = (int)root.Element("tileset").Attribute("columns");

            Layer = root.Elements("layer")
                      .Select(l => l.Element("data").Value)
                      .Select(rawMap => rawMap.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                            .SelectMany((line, y) => line.Trim().Split(',')
                                .Select(gidRaw => (Success: int.TryParse(gidRaw, out int gid), Gid: gid))
                                .Where(exp => exp.Success)
                                .Select((exp, x) => (Pos: new Vector2f(x * TileSize.X, y * TileSize.Y),
                                                    Cod: new Vector2i((exp.Gid - 1) % columns * TileSize.X,
                                                                      (exp.Gid - 1) / columns * TileSize.Y))
                                )
                            ).ToArray()
                        ).ToArray();
            return map;
        }
    }
}

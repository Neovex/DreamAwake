﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BlackCoat;
using BlackCoat.Collision;
using BlackCoat.Collision.Shapes;
using SFML.Graphics;
using SFML.System;

namespace DreamAwake
{
    class MapData
    {
        public Vector2i MapSize { get; private set; }
        public Vector2i TileSize { get; private set; }
        public Layer[] Layer { get; private set; }
        public CollisionLayer[] CollisionLayer { get; private set; }

        public void Load(Core core, string file)
        {
            // Load Collision Layer
            var map = new List<(CollisionType type, RectangleCollisionShape collision)>();
            var root = XElement.Load(file);
            CollisionLayer = root.Elements("objectgroup")
                                 .Select(l => new CollisionLayer()
                                 {
                                     IsLight = l.Element("properties") == null,
                                     Collisions = l.Elements("object")
                                                   .Select(o=> new CollisionObject()
                                                   {
                                                       Type = ParseCollisionType(o.Attribute("type")?.Value),
                                                       Shape = new RectangleCollisionShape(core.CollisionSystem,
                                                            new Vector2f((float)o.Attribute("x"), (float)o.Attribute("y")),
                                                            new Vector2f((float)o.Attribute("width"), (float)o.Attribute("height")))
                                                   }).ToArray()
                                 }).ToArray();


            // Load Tilemap Layer
            MapSize = new Vector2i((int)root.Attribute("width"), (int)root.Attribute("height"));
            TileSize = new Vector2i((int)root.Attribute("tilewidth"), (int)root.Attribute("tileheight"));
            var columns = (int)root.Element("tileset").Attribute("columns");

            Layer = root.Elements("layer").Select(l => new Layer()
            {
                IsLight = l.Element("properties") == null,
                Tiles = l.Element("data").Value.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                         .SelectMany((line, y) => line.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(gidRaw => (Success: int.TryParse(gidRaw, out int gid), Gid: gid))
                                .Where(exp => exp.Success)
                                .Select((exp, x) => new Tile()
                                {
                                    Position = new Vector2f(x * TileSize.X, y * TileSize.Y),
                                    Coordinates = new Vector2i((exp.Gid - 1) % columns * TileSize.X,
                                                                (exp.Gid - 1) / columns * TileSize.Y)
                                })
                        ).ToArray()
            }).ToArray();
        }

        private static CollisionType ParseCollisionType(string type)
        {
            if (type == null) return CollisionType.Normal;
            return (CollisionType)Enum.Parse(typeof(CollisionType), type, true);
        }
    }

    class Layer
    {
        public bool IsLight { get; set; }
        public Tile[] Tiles { get; set; }
    }

    class Tile
    {
        public Vector2f Position { get; set; }
        public Vector2i Coordinates { get; set; }
    }

    class CollisionLayer
    {
        public bool IsLight { get; set; }
        public CollisionObject[] Collisions { get; set; }
    }

    class CollisionObject : ICollisionShape
    {
        public CollisionType Type { get; set; }
        public RectangleCollisionShape Shape { get; set; }

        public Geometry CollisionGeometry => Shape.CollisionGeometry;

        public bool CollidesWith(Vector2f point) => Shape.CollidesWith(point);
        public bool CollidesWith(ICollisionShape other) => Shape.CollidesWith(other);
    }

    enum CollisionType
    {
        Normal,
        Killzone,
        Goal,
        Start
    }
}

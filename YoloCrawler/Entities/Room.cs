namespace YoloCrawler.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Room
    {
        private readonly Tile[,] _tiles;
        private readonly Size _size;
        private readonly Position _startingPosition;
        private readonly Dice _dice;

        public List<Monster> Monsters { get; set; }

        public Room(Size size, Position startingPosition)
        {
            _tiles = new Tile[size.Width, size.Height];
            _size = size;
            _startingPosition = startingPosition;
            _dice = new Dice();
            Monsters = new List<Monster>();
        }

        public Tile[,] Tiles
        {
            get { return _tiles; }
        }

        public Size Size
        {
            get { return _size; }
        }

        public Position StartingPosition
        {
            get { return _startingPosition; }
        }

        public void Draw()
        {
            for (int h = 0; h < _size.Height; h++)
            {
                for (int w = 0; w < _size.Width; w++)
                {
                    Console.Write(_tiles[w, h]);
                }
                Console.WriteLine();
            }
        }

        public void AddLink(Room newRoom)
        {
            var horizontalOrVertical = _dice.RollK100();

            if (horizontalOrVertical < 50) //vertical
            {
                var leftOrRight = _dice.RollK100();
                var y = _dice.RollForPlaceOnTheWall(_size.Height);

                if (leftOrRight < 0)
                {
                    //left
                    Tiles[0, y].AddDoorTo(newRoom);

                    return;
                }

                // right
                Tiles[_size.Width - 1, y].AddDoorTo(newRoom);

                return;
            }

            // horizontal
            var x = _dice.RollForPlaceOnTheWall(_size.Width);

            var upOrDown = _dice.RollK100();
            if (upOrDown < 50) //up
            {
                Tiles[x, 0].AddDoorTo(newRoom);

                return;
            }

            // down
            Tiles[x, _size.Height - 1].AddDoorTo(newRoom);
        }

        public Position GetRandomAvailablePosition()
        {
            var x = _dice.RollForFreeAvailableCoordinateValueBasedOn(_size.Width);
            var y = _dice.RollForFreeAvailableCoordinateValueBasedOn(_size.Height);

            return new Position(x, y);
        }

        public bool MonsterOccupiesPosition(Position position)
        {
            return Monsters.Any(monster => Equals(monster.Position, position));
        }

        public void RemoveDeadMonsters(ConsolePresentation.Logger logger)
        {
            Monsters.RemoveAll(monster => monster.IsDead);
        }
        
        public Tile GetDoorTo(Room room)
        {
            var doors = new List<Tile>();

            foreach (var tile in Tiles)
            {
                if (tile.Type == TileType.Door)
                {
                    doors.Add(tile);
                }
            }

            var door = doors.Single(t => t.HasDoorTo(room));

            return door;
        }
    }
}
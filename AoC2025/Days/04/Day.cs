using System.Diagnostics;

namespace ConsoleApp
{
    internal class Day04 : IDay
    {
        record Coordinate(int X, int Y);

        class Node
        {
            public bool Roll { get; set; }
        }

        private static List<Coordinate> Directions =
        [
            new Coordinate(0, 1),
            new Coordinate(0, -1),
            new Coordinate(1, 0),
            new Coordinate(1, 1),
            new Coordinate(1, -1),
            new Coordinate(-1, 0),
            new Coordinate(-1, 1),
            new Coordinate(-1, -1)
        ];

        private Dictionary<Coordinate, Node> nodes = [];

        private bool[,] nodeGrid;
        private int maxX;
        private int maxY;
        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            maxX = data[0].Length;
            maxY = data.Length;
            nodeGrid = new bool[maxX, maxY];

            for (int y = 0; y < data.Length; y++)
            {
                var row = data[y];
                for (int x = 0; x < row.Length; x++)
                {
                    nodes.Add(new Coordinate(x, y), new Node { Roll = row[x] == '@' });
                    nodeGrid[x, y] = row[x] == '@';
                }
            }

        }

        public decimal Part1()
        {
            var sum = 0;

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if (nodeGrid[x, y] && CountNeighbours(x, y) < 4)
                    {
                        sum++;
                    }
                }
            }

            //foreach (var kvp in nodes2)
            //{
            //    if (kvp.Value.Roll && CountNeighbours(kvp.Key) < 4)
            //    {
            //        sum++;
            //    }
            //}

            return sum;
        }


        public decimal Part2()
        {
            var totalRemoved = 0;

            bool tryAgain = true;

            while (tryAgain)
            {
                tryAgain = false;
                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
                    {
                        if (nodeGrid[x, y] && CountNeighbours(x, y) < 4)
                        {
                            nodeGrid[x, y] = false;
                            totalRemoved++;
                            tryAgain = true;
                        }
                    }
                }

            }
            return totalRemoved;
        }

        [Obsolete]
        private int CountNeighbours(Coordinate coord)
        {
            int sum = 0;
            foreach (var dir in Directions)
            {
                var n = new Coordinate(coord.X + dir.X, coord.Y + dir.Y);
                if (nodes.ContainsKey(n) && nodes[n].Roll)
                {
                    sum++;
                }
            }

            return sum;
        }

        private int CountNeighbours(int x, int y)
        {
            int sum = 0;

            foreach (var dir in Directions)
            {
                var nx = x + dir.X;
                var ny = y + dir.Y;

                if (nx < 0 || ny < 0 || nx == maxX || ny == maxY)
                    continue;

                if (nodeGrid[nx, ny])
                {
                    sum++;
                }
            }

            return sum;
        }
    }
}

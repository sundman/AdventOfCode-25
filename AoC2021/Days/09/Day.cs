using System.Diagnostics;
using System.Net;

namespace ConsoleApp
{
    internal class Day09 : IDay
    {
        private int[,] grid;
        private int xl;
        private int yl;


        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            grid = new int[data[0].Length, data.Length];

            for (int x = 0; x < data[0].Length; x++)
                for (int y = 0; y < data.Length; y++)
                    grid[x, y] = data[y][x] - '0';

            xl = grid.GetLength(0);
            yl = grid.GetLength(1);

        }

        List<(int x, int y)> directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];

        public decimal Part1()
        {
            decimal totalSum = 0;

            for (int x = 0; x < xl; x++)
                for (int y = 0; y < yl; y++)
                {
                    if (!directions.Any(dir =>
                        {
                            var dx = x + dir.x;
                            var dy = y + dir.y;
                            if (dx < 0 || dx == xl || dy < 0 || dy == yl)
                                return false;
                            return grid[dx, dy] <= grid[x, y];
                        }))
                    {
                        totalSum += grid[x, y] + 1;
                    }
                }

            return totalSum;

        }

        private HashSet<(int x, int y)> visited = [];

        private List<int> sizes = [];

        public decimal Part2()
        {
            decimal totalSum = 0;

            var xl = grid.GetLength(0);
            var yl = grid.GetLength(1);

            for (int x = 0; x < xl; x++)
            {
                for (int y = 0; y < yl; y++)
                {
                    if (grid[x, y] == 9 || visited.Contains((x, y)))
                        continue;

                    sizes.Add(calculateSize(x, y));
                }
            }

            sizes.Sort();

            return sizes[^1] * sizes[^2] * sizes[^3];

        }

        private int calculateSize(int x, int y)
        {
            int size = 0;

            Stack<(int x, int y)> stack = [];
            stack.Push((x, y));

            while (stack.Any())
            {
                var item = stack.Pop();

                if (!visited.Add((item.x, item.y)))
                {
                    continue;
                }

                size++;

                foreach (var dir in directions)
                {
                    var dx = item.x + dir.x;
                    var dy = item.y + dir.y;
                    if (dx < 0 || dx == xl || dy < 0 || dy == yl)
                        continue;

                    if (grid[dx, dy] == 9)
                        continue;



                    stack.Push((dx, dy));
                }
            }

            return size;
        }
    }
}

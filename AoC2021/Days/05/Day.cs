using System.Diagnostics;

namespace ConsoleApp
{
    internal class Day05 : IDay
    {


        record Point(int x, int y);

        private List<(Point a, Point b)> input = [];



        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            foreach (var line in data)
            {
                var parts = line.Split("->", StringSplitOptions.TrimEntries);

                var pointA = parts[0].Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
                var pointB = parts[1].Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();

                input.Add(new ValueTuple<Point, Point>(new(pointA.First(), pointA.Last()),
                    new Point(pointB.First(), pointB.Last())));
            }


        }


        public decimal Part1()
        {
            decimal totalSum = 0;


            HashSet<Point> points = [];
            HashSet<Point> collisions = [];

            var horisontal = input.Where(line => line.a.y == line.b.y).ToList();
            var vertical = input.Where(line => line.a.x == line.b.x).ToList();

            foreach (var line in horisontal)
            {
                for (int i = Math.Min(line.a.x, line.b.x); i <= Math.Max(line.a.x, line.b.x); i++)
                {
                    if (!points.Add(line.a with { x = i }))
                        collisions.Add(line.a with { x = i });
                }
            }

            foreach (var line in vertical)
            {
                for (int i = Math.Min(line.a.y, line.b.y); i <= Math.Max(line.a.y, line.b.y); i++)
                {
                    if (!points.Add(line.a with { y = i }))
                        collisions.Add(line.a with { y = i });
                }
            }

            return collisions.Count;
        }

        public decimal Part2()
        {
            HashSet<Point> points = [];
            HashSet<Point> collisions = [];

            foreach (var line in input)
            {

                foreach (var newPoint in BresenhamLine(line.a, line.b))
                {
                    if (!points.Add(newPoint))
                        collisions.Add(newPoint);
                }
            }

            return collisions.Count;

        }



        static List<Point> BresenhamLine(Point p1, Point p2)
        {
            var points = new List<Point>();

            int dx = Math.Abs(p2.x - p1.x);
            int dy = Math.Abs(p2.y - p1.y);
            int sx = p1.x < p2.x ? 1 : -1;
            int sy = p1.y < p2.y ? 1 : -1;

            int err = dx - dy;
            int x = p1.x;
            int y = p1.y;

            while (true)
            {
                points.Add(new(x, y));

                if (x == p2.x && y == p2.y)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }

            return points;
        }

    }
}

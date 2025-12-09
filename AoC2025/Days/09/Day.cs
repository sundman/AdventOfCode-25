using System.Diagnostics;
using System.Drawing;

namespace ConsoleApp
{
    internal class Day09 : IDay
    {

        private List<Point> tiles = [];

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            foreach (var line in data)
            {
                var nums = line.Split(',', StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
                tiles.Add(new Point(nums[0], nums[1]));
            }
        }




        record Point(long X, long Y);


        public decimal Part1()
        {
            long largestSquare = 0;


            for (int a = 0; a < tiles.Count; a++)
            {
                for (int b = 0; b < tiles.Count; b++)
                {
                    if (b <= a)
                        continue;

                    var size = (Math.Abs(tiles[a].X - tiles[b].X) + 1) * (Math.Abs(tiles[a].Y - tiles[b].Y) + 1);
                    if (size > largestSquare)
                    {

                        largestSquare = size;
                    }
                }
            }

            return largestSquare;
        }

        public decimal Part2()
        {
            var horisontalLines = new List<(Point a, Point b)>();
            var verticalLines = new List<(Point a, Point b)>();
            var prev = tiles.Last();

            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].X == prev.X)
                {
                    verticalLines.Add((prev, tiles[i]));
                }
                else
                {
                    horisontalLines.Add((prev, tiles[i]));
                }
                prev = tiles[i];
            }


            var yTiles = tiles.ToList();
            var xTiles = tiles.ToList();
            verticalLines.Sort((l1, l2) => l1.a.Y.CompareTo(l2.a.Y));
            horisontalLines.Sort((l1, l2) => l1.a.X.CompareTo(l2.a.X));

            var yDict = new Dictionary<Point, int>();
            var xDict = new Dictionary<Point, int>();

            for (int i = 0; i < yTiles.Count; i++)
            {
                yDict[yTiles[i]] = i;
            }

            for (int i = 0; i < xTiles.Count; i++)
            {
                xDict[xTiles[i]] = i;
            }

            long largestSquare = 0;

            for (int a = 0; a < xTiles.Count; a++)
            {
                for (int b = 0; b < xTiles.Count; b++)
                {
                    if (b <= a)
                        continue;

                    var size = (Math.Abs(xTiles[a].X - xTiles[b].X) + 1) * (Math.Abs(xTiles[a].Y - xTiles[b].Y) + 1);
                    if (size > largestSquare)
                    {
                        var yA = xTiles[a].Y;
                        var yB = xTiles[b].Y;

                        var squareMaxY = Math.Max(yA, yB);
                        var squareMinY = Math.Min(yA, yB);

                        var squareMinX = Math.Min(xTiles[a].X, xTiles[b].X);
                        var squareMaxX = Math.Max(xTiles[a].X, xTiles[b].X);

                        bool passer = false;

                        // any line passing through us breaks the rule
                        foreach (var line in verticalLines)
                        {
                            // they are sorted... faster ways to do this part
                            if (line.a.X <= squareMinX || line.a.X >= squareMaxX)
                                continue;

                            var y1 = line.a.Y;
                            var y2 = line.b.Y;

                            var minY = Math.Min(y1, y2);
                            var maxY = Math.Max(y1, y2);

                            if (minY < squareMaxY && maxY >= squareMaxY
                                || minY <= squareMinY && maxY > squareMinY
                                )
                            {
                                passer = true;
                                break;
                            }
                        }

                        if (passer)
                            continue;


                        foreach (var line in horisontalLines)
                        {
                            if (line.a.Y <= squareMinY || line.a.Y >= squareMaxY)
                                continue;

                            var x1 = line.a.X;
                            var x2 = line.b.X;

                            var minX = Math.Min(x1, x2);
                            var maxX = Math.Max(x1, x2);

                            if (minX < squareMaxX && maxX >= squareMaxX
                                || minX <= squareMinX && maxX > squareMinX)
                            {
                                passer = true;
                                break;
                            }
                        }

                        if (passer)
                            continue;

                        largestSquare = size;
                    }
                }
            }

            return largestSquare;
        }
    }
}

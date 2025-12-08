using System.Diagnostics;
using System.Drawing;

namespace ConsoleApp
{
    internal class Day08 : IDay
    {

        private List<Point3D> junctionBoxes = [];

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            foreach (var line in data)
            {
                var nums = line.Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
                junctionBoxes.Add(new(nums[0], nums[1], nums[2]));
            }
        }


        decimal CalculateDistance(Point3D p1, Point3D p2)
        {
            return (decimal)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }

        record Point3D(int X, int Y, int Z);
        record Distance(Point3D a, Point3D b, decimal distance);

        List<Distance> distanceList = [];

        public decimal Part1()
        {

            Dictionary<Point3D, List<Point3D>> circuits = new Dictionary<Point3D, List<Point3D>>();

            foreach (var node in junctionBoxes)
                circuits.Add(node, new List<Point3D>());

            for (int a = 0; a < junctionBoxes.Count; a++)
            {
                for (int b = 0; b < junctionBoxes.Count; b++)
                {
                    if (b <= a)
                        continue;
                    var dist = CalculateDistance(junctionBoxes[a], junctionBoxes[b]);
                    distanceList.Add(new(junctionBoxes[a], junctionBoxes[b], dist));
                }
            }

            distanceList.Sort((a, b) => a.distance.CompareTo(b.distance));

            int connectionsToMake = Debugger.IsAttached ? 10 : 1000;

            for (int i = 0; i < connectionsToMake; i++)
            {
                circuits[distanceList[i].a].Add(distanceList[i].b);
                circuits[distanceList[i].b].Add(distanceList[i].a);
            }

            HashSet<Point3D> visited = [];

            List<int> sizeList = [];

            foreach (var kvp in circuits)
            {
                if (visited.Add(kvp.Key))
                {
                    Stack<Point3D> neighbours = [];

                    kvp.Value.ForEach(x => neighbours.Push(x));

                    int currentSize = 1;
                    while (neighbours.TryPop(out var next))
                    {
                        if (visited.Add(next))
                        {
                            currentSize++;
                            circuits[next].ForEach(x => neighbours.Push(x));
                        }
                    }

                    sizeList.Add(currentSize);
                }

            }

            sizeList.Sort();

            var l = sizeList.Count - 1;

            return sizeList[l] * sizeList[l - 1] * sizeList[l - 2];
        }

        public decimal Part2()
        {

            Dictionary<Point3D, List<Point3D>> circuits = new Dictionary<Point3D, List<Point3D>>();

            foreach (var node in junctionBoxes)
                circuits.Add(node, new List<Point3D>());


            // "cheating" by using the list from part 1
            //List<Distance> distanceList = [];
            //for (int a = 0; a < junctionBoxes.Count; a++)
            //{
            //    for (int b = 0; b < junctionBoxes.Count; b++)
            //    {
            //        if (b <= a)
            //            continue;
            //        var dist = CalculateDistance(junctionBoxes[a], junctionBoxes[b]);
            //        distanceList.Add(new(junctionBoxes[a], junctionBoxes[b], dist));
            //    }
            //}

            //distanceList.Sort((a, b) => a.distance.CompareTo(b.distance));

            int i = 0;
            while (circuits.Any(x => !x.Value.Any()))
            {
                circuits[distanceList[i].a].Add(distanceList[i].b);
                circuits[distanceList[i].b].Add(distanceList[i].a);
                i++;
            }

            return (decimal)distanceList[i - 1].a.X * distanceList[i - 1].b.X;

        }
    }
}

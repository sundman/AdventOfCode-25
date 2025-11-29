using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace ConsoleApp
{
    internal class Box
    {
        public int H;
        public int W;
        public int L;

        public int WrappingNeeded
        {
            get
            {
                var sides = H * W + H * L + L * W;

                List<int> order = [H, W, L];
                order.Sort();
                return sides * 2 + order[0] * order[1];

            }

        }

        public long RibbonNeeded
        {
            get
            {
                var volume = H * W * L;

                List<int> order = [H, W, L];
                order.Sort();

                var circum = (order[0] + order[1]) * 2;
                return volume + circum;
            }
        }
    }

    internal class Day02 : IDay
    {
        private List<Box> boxes;

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            boxes = data.Select(line =>
            {
                var nums = line.Split('x');
                return new Box()
                {
                    H = int.Parse(nums[0]),
                    W = int.Parse(nums[1]),
                    L = int.Parse(nums[2])
                };
            }).ToList();


        }

        public decimal Part1()
        {
            return boxes.Sum(x => x.WrappingNeeded);

        }

        public decimal Part2()
        {

            return boxes.Sum(x => x.RibbonNeeded);
            return -1;
        }
    }
}

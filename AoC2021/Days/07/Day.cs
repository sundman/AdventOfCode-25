using System.Diagnostics;

namespace ConsoleApp
{
    internal class Day07 : IDay
    {
        private List<int> input;



        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");


            input = data[0].Split(',', StringSplitOptions.TrimEntries).Select(int.Parse).ToList();

        }

        public decimal Part1()
        {
            decimal totalSum = 0;

            var min = input.Min();
            var max = input.Max();

            var optimal = input.Sum(x => x - min);

            for (int i = min; i < max; i++)
            {
                var cost = input.Sum(x => Math.Abs(x - i));
                if (cost < optimal)
                    optimal = cost;
            }



            return optimal;
        }

        public decimal Part2()
        {

            decimal totalSum = 0;

            var min = input.Min();
            var max = input.Max();
            
            var optimal = input.Sum(x => (x - min) * (x - min + 1) / 2);

            for (int i = min; i < max; i++)
            {
                var cost = input.Sum(x => Math.Abs(x - i) * (Math.Abs(x - i) + 1) / 2); 
                if (cost < optimal)
                    optimal = cost;
            }

            return optimal;

        }
    }
}

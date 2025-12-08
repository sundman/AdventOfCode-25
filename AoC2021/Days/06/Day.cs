using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp
{
    internal class Day06 : IDay
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

            var fishes = new int[9];

            foreach (var num in input)
                fishes[num]++;

            for (int i = 0; i < 80; i++)
            {
                var spawners = fishes[0];

                for (int x = 0; x < 8; x++)
                    fishes[x] = fishes[x + 1];

                fishes[6] += spawners;
                fishes[8] = spawners;
            }



            return fishes.Sum();
        }

        public decimal Part2()
        {

            decimal totalSum = 0;

            var fishes = new long[9];

            foreach (var num in input)
                fishes[num]++;

            for (int i = 0; i < 256; i++)
            {
                var spawners = fishes[0];

                for (int x = 0; x < 8; x++)
                    fishes[x] = fishes[x + 1];

                fishes[6] += spawners;
                fishes[8] = spawners;
            }



            return fishes.Sum();

        }
    }
}

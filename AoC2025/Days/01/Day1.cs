using System.Diagnostics;

namespace ConsoleApp
{
    internal class Day01 : IDay
    {
        enum Dir
        {
            Left = -1,
            Right = 1
        }

        private List<(Dir direction, int value)> numbers = [];

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            foreach (var line in data)
            {
                var num = int.Parse(line.Substring(1));
                if (line[0] == 'R')
                    numbers.Add(new(Dir.Right, num));
                else
                    numbers.Add(new(Dir.Left, num));
            }

        }

        public decimal Part1()
        {
            int timesZero = 0;

            var current = 50;
            foreach (var num in numbers)
            {
                current += num.value * (int)num.direction;

                if (current % 100 == 0)
                    timesZero++;
            }

            return timesZero;
        }

        public decimal Part2()
        {
            var timesZero = 0;
            var current = 50;

            foreach (var num in numbers)
            {
                timesZero += num.value / 100;
                var newNum = num.value % 100;

                var extraPass = num.direction == Dir.Right ?
                    current + newNum >= 100 :
                    current != 0 && current - newNum <= 0;

                if (extraPass)
                    timesZero++;

                current = (current + newNum * (int)num.direction).Mod(100);
            }

            return timesZero;
        }
    }

    public static class Extensions
    {
        public static int Mod(this int num, int modulo)
        {
            return ((num % modulo) + modulo) % modulo;
        }
    }
}

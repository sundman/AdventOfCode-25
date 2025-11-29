using System.Diagnostics;

namespace ConsoleApp
{
    internal class Day01 : IDay
    {
        private string input;

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            input = data[0];

        }

        public decimal Part1()
        {
            decimal totalDiff = 0;

            return input.Sum(ch => ch == '(' ? 1 : -1);

        }

        public decimal Part2()
        {
            int level = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                    ++level;
                else
                    --level;

                if (level == -1)
                    return i+1;
            }

            return -1;
        }
    }
}

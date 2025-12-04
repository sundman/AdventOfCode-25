using System.Diagnostics;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp
{
    internal class Day03 : IDay
    {


        private List<string> lines = [];

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            lines = data.ToList();

        }

        public decimal Part1()
        {
            return lines.Sum(x => LineSum(x, 2));
        }

        public decimal Part2()
        {
            return lines.Sum(x => LineSum(x, 12));
        }

       
        private long LineSum(string line, int digitLength)
        {
            var maxDigit = new int[digitLength];

            for (var i = 0; i < line.Length; i++)
            {
                for (var max = 0; max < digitLength; max++)
                {
                    if (i <= line.Length - digitLength + max && line[i] > maxDigit[max])
                    {
                        maxDigit[max] = line[i];

                        for (var reset = max + 1; reset < digitLength; reset++)
                            maxDigit[reset] = 0;
                        break;
                    }
                }
            }

            long toReturn = 0;
            for (var i = 0; i < digitLength; i++)
            {
                toReturn += (maxDigit[i] - '0') * (long)Math.Pow(10, digitLength - i - 1);
            }

            return toReturn;
        }

        [Obsolete]
        private int lineSumPart1(string line)
        {
            int maxFirstDigit = 0;
            int maxSecondDigit = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (i < line.Length - 1 && line[i] > maxFirstDigit)
                {
                    maxFirstDigit = line[i];
                    maxSecondDigit = 0;
                }

                else if (line[i] > maxSecondDigit)
                    maxSecondDigit = line[i];
            }

            return (maxFirstDigit - '0') * 10 + (maxSecondDigit - '0');
        }


    }


}

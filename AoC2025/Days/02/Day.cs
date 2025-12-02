using System.Diagnostics;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp
{
    internal class Day02 : IDay
    {


        private List<(long start, long end)> ranges = [];

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            var split = data[0].Split(',');
            foreach (var pairs in split)
            {
                var numParts = pairs.Split('-');

                ranges.Add(new(long.Parse(numParts[0]), long.Parse(numParts[1])));
            }

        }

        public decimal Part1()
        {
            List<long> badNumbers = [];
            foreach (var range in ranges)
            {
                long numberToInvestigate = range.start;
                while (numberToInvestigate <= range.end)
                {
                    var length = numberToInvestigate.CountDigits();
                    if (length.IsOdd())
                    {
                        numberToInvestigate = (long)Math.Pow(10, length);
                        continue;
                    }

                    var mid = length / 2;

                    long pow = (long)Math.Pow(10, mid);        // 10^mid
                    long firstHalf = numberToInvestigate / pow;   // integer division gives the first half
                    long secondHalf = numberToInvestigate % pow;  // remainder gives the second half

                    if (firstHalf == secondHalf)
                    {
                        badNumbers.Add(numberToInvestigate);
                        numberToInvestigate += pow;
                    }
                    else if (secondHalf < firstHalf)
                        numberToInvestigate += firstHalf - secondHalf;
                    else
                    {
                        numberToInvestigate += pow-secondHalf+firstHalf;
                    }

                }

            }

        
            return badNumbers.Sum();
        }

        public decimal Part2()
        {
            var timesZero = 0;


            return timesZero;
        }
    }

    static class Extensionss
    {
        public static int CountDigits(this long number)
        {
            return (int)Math.Floor(Math.Log10(number)) + 1;
        }

        public static bool IsOdd(this int number)
        {
            return number % 2 == 1;
        }

    }
}

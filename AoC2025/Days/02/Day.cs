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
            long badNumbers = 0;
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

                    long pow = (long)Math.Pow(10, mid);
                    long firstHalf = numberToInvestigate / pow;
                    long secondHalf = numberToInvestigate % pow;

                    if (firstHalf == secondHalf)
                    {
                        badNumbers += numberToInvestigate;
                        numberToInvestigate += pow;
                    }
                    else if (secondHalf < firstHalf)
                        numberToInvestigate += firstHalf - secondHalf;
                    else
                    {
                        numberToInvestigate += pow - secondHalf + firstHalf;
                    }

                }
            }

            return badNumbers;
        }

        public decimal Part2()
        {
            List<long> badNumbers = [];

            HashSet<(int length, long factor)> lolol = [];
            foreach (var range in ranges)
            {
                for (long i = range.start; i <= range.end; i++)
                {
                    var numAsString = i.ToString();
                    for (int pos = 1; pos < numAsString.Length / 2 + 1; pos++)
                    {
                        var firstPart = numAsString.Substring(0, pos);

                        if (numAsString.Length % firstPart.Length != 0)
                        {
                            continue;
                        }

                        bool invalid = true;
                        for (var check = pos; check < numAsString.Length; check += pos)
                        {
                            var toCompare = numAsString.Substring(check, pos);

                            if (toCompare != firstPart)
                            {
                                invalid = false;
                                break;
                            }
                        }

                        if (invalid)
                        {
                            lolol.Add(new(numAsString.Length, i / int.Parse(firstPart)));
                            badNumbers.Add(i);
                            break;
                        }

                    }
                }
            }

            Console.WriteLine("only possible factors:");
            foreach (var entry in lolol.ToList().OrderBy(x => x.length))
            {
                Console.WriteLine($"length: {entry.length}, factor: {entry.factor}");
            }


            return badNumbers.Sum();
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

using System.Diagnostics;
using System.Net;

namespace ConsoleApp
{
    internal class Day08 : IDay
    {
        private List<int> input;

        private List<List<string>> output = [];
        private List<(List<string> complete, List<string> output)> completeLine = [];


        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            foreach (var line in data)
            {
                var parts = line.Split('|', StringSplitOptions.TrimEntries);
                output.Add(parts[1].Split(' ').ToList());
                completeLine.Add((parts[1].Split(' ').Concat(parts[0].Split(' ')).ToList(), parts[1].Split(' ').ToList()));

            }

        }

        public decimal Part1()
        {
            List<int> validLengt = [2, 3, 4, 7];
            return output.Sum(x => x.Count(y => validLengt.Contains(y.Length)));


        }

        //  aaaa    ....    aaaa    aaaa    ....
        // b    c  .    c  .    c  .    c  b    c
        // b    c  .    c  .    c  .    c  b    c
        //  ....    ....    dddd    dddd    dddd
        // e    f  .    f  e    .  .    f  .    f
        // e    f  .    f  e    .  .    f  .    f
        //  gggg    ....    gggg    gggg    ....
        //    6      2       5        5      4 

        //   5:      6:      7:      8:      9:
        //  aaaa    aaaa    aaaa    aaaa    aaaa
        // b    .  b    .  .    c  b    c  b    c
        // b    .  b    .  .    c  b    c  b    c
        //  dddd    dddd    ....    dddd    dddd
        // .    f  e    f  .    f  e    f  .    f
        // .    f  e    f  .    f  e    f  .    f
        //  gggg    gggg    ....    gggg    gggg
        //   5        6      3        7      6

        public decimal Part2()
        {
            decimal totalSum = 0;
            foreach (var complete in completeLine)
            {
                var line = complete.complete;
                var charsForOne = line.First(x => x.Length == 2);
                var charsForSeven = line.First(x => x.Length == 3);
                var charsForFour = line.First(x => x.Length == 4);
                var charsForEight = line.First(x => x.Length == 7);

                var a = charsForSeven.First(ch => !charsForOne.Contains(ch));
                var d = charsForFour.First(ch => line.Where(x => x.Length == 5).All(y => y.Contains(ch)));
                var b = charsForFour.First(ch => ch != d && !charsForOne.Contains(ch));

                var charsForNine = line.First(str => str.Length == 6 
                                                     && str.Contains(d)
                                                     && charsForOne.All(str.Contains));
                var g = charsForNine.First(ch => !charsForOne.Contains(ch) && ch != a && ch != b && ch != d);
                var e = charsForEight.First(ch =>
                    !charsForOne.Contains(ch) && ch != a && ch != b && ch != d && ch != g);

                List<int> digits = [];
                foreach (var num in complete.output)
                {
                    int digit;

                    // 1
                    if (num.Length == 2)
                        digit = 1;
                    else if (num.Length == 3)
                    {
                        digit = 7;
                    }
                    else if (num.Length == 4)
                    {
                        digit = 4;
                    }
                    else if (num.Length == 7)
                    {
                        digit = 8;
                    }
                    // 2, 5, 3 
                    else if (num.Length == 5)
                    {
                        if (num.Contains(e))
                            digit = 2;
                        else if (num.Contains(b))
                            digit = 5;
                        else
                            digit = 3;
                    }
                    // 0,6,9
                    else
                    {
                        if (!num.Contains(d))
                            digit = 0;
                        else if (num.Contains(e))
                            digit = 6;
                        else
                            digit = 9;

                    }
                    digits.Add(digit);
                }

                totalSum += digits.Aggregate(0, (current, d) => current * 10 + d);
            }




            return totalSum;

        }
    }
}

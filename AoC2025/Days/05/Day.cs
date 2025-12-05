using System.Diagnostics;

namespace ConsoleApp
{
    internal class Day05 : IDay
    {
        private record Range(long start, long end);
        List<Range> ranges = [];

        private List<long> ingredients = [];

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            int i = 0;
            while (data[i] != "")
            {
                var nums = data[i].Split('-', StringSplitOptions.TrimEntries).Select(long.Parse).ToList();

                ranges.Add(new Range(nums.First(), nums.Last()));
                i++;
            }

            i++;
            while (i < data.Length)
            {
                ingredients.Add(long.Parse(data[i]));
                i++;
            }

        }

        public decimal Part1()
        {
            return ingredients.Count(ingredient =>
                  ranges.Any(range => range.start <= ingredient && range.end >= ingredient));

        }

        public decimal Part2()
        {
            ranges.Sort((a, b) => a.start.CompareTo(b.start));

            decimal sum = 0;
            decimal lastEnd = 0;
            foreach (var range in ranges)
            {
                if (range.end < lastEnd)
                    continue;

                var start = lastEnd >= range.start ? lastEnd + 1 : range.start;

                sum += range.end - start + 1;
                lastEnd = range.end > lastEnd ? range.end : lastEnd;
            }

            return sum;

        }
    }
}

using System.Diagnostics;

namespace ConsoleApp
{
    internal class Day07 : IDay
    {


        int startX = 0;
        private int maxX = 0;
        private List<List<int>> splitters = [];
        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");
            maxX = data[0].Length;
            foreach (var line in data)
            {
                var splitter = new List<int>();
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'S')
                        startX = x;

                    if (line[x] == '^')
                        splitter.Add(x);
                }

                splitters.Add(splitter);
            }

        }

        public decimal Part1()
        {
            decimal totalSum = 0;

            HashSet<int> incoming =
            [
                startX
            ];

            foreach (var line in splitters)
            {
                HashSet<int> outgoing = new HashSet<int>();
                foreach (var x in incoming)
                {
                    if (line.Contains(x))
                    {
                        totalSum++;
                        outgoing.Add(x - 1);
                        outgoing.Add(x + 1);
                    }
                    else
                    {
                        outgoing.Add(x);
                    }
                }
                incoming = outgoing;
            }

            return totalSum;
        }

        public decimal Part2()
        {
            long[] timeLineCounter = new long[maxX];
            timeLineCounter[startX] = 1;

            var outGoingTimeline = new long[maxX];
            foreach (var line in splitters)
            {
                Array.Clear(outGoingTimeline);
                for (int x = 0; x < maxX; x++)
                {
                    if (timeLineCounter[x] > 0)
                    {
                        if (line.Contains(x))
                        {
                            outGoingTimeline[x - 1]+= timeLineCounter[x];
                            outGoingTimeline[x + 1] += timeLineCounter[x];
                        }

                        else
                        {
                            outGoingTimeline[x]+=  timeLineCounter[x];
                        }
                    }
                }

                for (int x = 0; x < maxX; x++)
                {
                    timeLineCounter[x] = outGoingTimeline[x] ;
                }
            }

            return timeLineCounter.Sum();

        }
    }
}

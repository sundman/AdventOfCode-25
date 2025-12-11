using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using Google.OrTools.ConstraintSolver;

namespace ConsoleApp
{
    internal class Day10 : IDay
    {
        private class IntArrayComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[]? x, int[]? y)
            {
                if (ReferenceEquals(x, y)) return true;
                return x.AsSpan().SequenceEqual(y);
            }

            public int GetHashCode(int[] obj)
            {
                var hc = new HashCode();
                foreach (var v in obj)
                    hc.Add(v);
                return hc.ToHashCode();
            }
        }

        private List<Machine> machines = [];

        public class Machine
        {
            public bool[] Lights;
            public List<bool[]> Buttons;

            public int[] joltages;
            public int LightNumber;

            public List<int> Switches = [];

            public Machine(bool[] lights, List<bool[]> buttons, int[] joltages)
            {
                Lights = lights;
                Buttons = buttons;

                LightNumber = BoolToInt(lights);
                Switches = buttons.Select(BoolToInt).ToList();
                this.joltages = joltages;
            }

            int BoolToInt(bool[] bits)
            {
                int result = 0;
                for (int i = 0; i < bits.Length; i++)
                {
                    if (bits[i])
                    {
                        result |= (1 << i); // Set the i-th bit
                    }
                }
                return result;
            }


            private int bestClicks = int.MaxValue;
            private Dictionary<int[], int> seenState = new Dictionary<int[], int>(new IntArrayComparer());

            private int[] clicks;
            private int clicksum;
            private void Dfs(int[] state)
            {
                if (clicksum + state.Max() >= bestClicks)
                    return;

                if (seenState.ContainsKey(state) && seenState[state] <= clicksum)
                    return;

                seenState[state] = clicksum;

                if (state.All(x => x == 0))
                {
                    if (clicksum < bestClicks)
                    {
                        bestClicks = clicksum;
                        Console.WriteLine($"{string.Join(',', clicksum)}");
                        return;
                    }
                }

                if (state.All(x => x != -1))
                {
                    var maxStateIndex = -1;
                    var maxStateValue = -1;
                    var minStateIndex = -1;
                    var minStateValue = int.MaxValue;

                    List<int> nils = [];
                    for (int i = 0; i < state.Length; i++)
                    {
                        if (state[i] == 0)
                        {
                            nils.Add(i);
                            continue;
                        }
                        if (state[i] > maxStateValue)
                        {
                            maxStateValue = state[i];
                            maxStateIndex = i;
                        }

                        if (state[i] < minStateValue)
                        {
                            minStateValue = state[i];
                            minStateIndex = i;
                        }
                    }

                    foreach (var button in Buttons
                                  .Where(x => !nils.Any(nil => x[nil]) && x[minStateIndex])
                                 .OrderByDescending(x => x.Count(y => y)))
                    {
                        var newState = (int[])state.Clone();
                        for (int i = 0; i < joltages.Length; i++)
                        {
                            if (button[i])
                            {
                                --newState[i];
                            }
                        }

                        clicksum++;
                        clicks[Buttons.IndexOf(button)]++;
                        Dfs(newState);
                        clicksum--;
                        clicks[Buttons.IndexOf(button)]--;
                    }
                }
            }

            public int FindPathToJoltages()
            {
                bestClicks = joltages.Sum() / Buttons.Min(x => x.Count(yes => yes)) + 1;
                clicks = new int[Buttons.Count];
                Dfs(joltages);

                return bestClicks;
            }

            public int FindPathToZero()
            {
                HashSet<int> possibleState = [LightNumber];

                int clicks = 0;
                while (true)
                {
                    clicks++;
                    HashSet<int> nextState = [];
                    foreach (var state in possibleState)
                    {
                        foreach (var button in Switches)
                        {
                            var next = state ^ button;
                            if (next == 0)
                                return clicks;
                            nextState.Add(next);
                        }
                    }
                    possibleState = nextState;
                }
            }
        }

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            foreach (var line in data)
            {
                var tokens = line.Split(' ');
                var lights = tokens[0].Skip(1).Take(tokens[0].Length - 2).Select(ch => ch == '#').ToArray();

                List<bool[]> buttons = [];
                for (int i = 1; i < tokens.Length - 1; i++)
                {
                    var indexes = tokens[i].Substring(1, tokens[i].Length - 2).Split(',').Select(int.Parse);
                    var button = new bool[lights.Length];
                    foreach (var index in indexes)
                    {
                        button[index] = true;
                    }
                    buttons.Add(button);
                }

                var joltageLevels = tokens.Last().Substring(1, tokens.Last().Length - 2).Split(',').Select(int.Parse).ToArray();
                machines.Add(new Machine(lights, buttons, joltageLevels));
            }
        }



        public decimal Part1()
        {
            var sum = 0;
            foreach (var machine in machines)
            {
                var clicks = machine.FindPathToZero();

                sum += clicks;
            }

            return sum;
        }

        public decimal Part2()
        {
            decimal sum = 0;
            var solver = new OrToolsMinSumOnly();

            Stopwatch timer = new Stopwatch();
            foreach (var machine in machines)
            {
                int[,] a2 = new int[machine.joltages.Length, machine.Buttons.Count];
                int[] b2 = machine.joltages.Select(x => x).ToArray();
                var upperBoundPerVar = new int[machine.Buttons.Count];

                for (int x = 0; x < machine.Buttons.Count; x++)
                {

                    var upperBound = machine.joltages.Max();
                    for (int y = 0; y < machine.joltages.Length; y++)
                    {
                        a2[y, x] = machine.Buttons[x][y] ? 1 : 0;

                        if (machine.Buttons[x][y] && machine.joltages[y] < upperBound)
                            upperBound = machine.joltages[y];
                    }

                    upperBoundPerVar[x] = upperBound;
                }


                timer.Restart();
                var mySolver = machine.FindPathToJoltages();
                var myTime = timer.ElapsedMilliseconds;
                timer.Restart();
                var google = solver.SolveMinSumOnly(a2, b2, upperBoundPerVar) ?? 0;
                var googleTime = timer.ElapsedMilliseconds;
                Console.WriteLine($"My solver: {mySolver} vs Google: {google} ... {myTime} ms vs {googleTime} ms");

                sum += google;

            }

            return sum;
        }

        void PrintArray(int[,] a, int[] b)
        {
            for (int y = 0; y < b.Length; y++)
            {
                for (int x = 0; x < a.GetLength(1); x++)
                {
                    Console.Write($"{a[y, x]} ");

                }
                Console.WriteLine($"| {b[y]}");
            }
        }

        void PrintArray(double[,] a, double[] b)
        {
            for (int y = 0; y < b.Length; y++)
            {
                for (int x = 0; x < a.GetLength(1); x++)
                {
                    Console.Write($"{a[y, x]} ");

                }
                Console.WriteLine($"| {b[y]}");
            }
        }
    }
}

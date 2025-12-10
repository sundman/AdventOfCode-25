using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;

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

            public int FindPathToJoltages()
            {
                HashSet<int[]> possibleState = new HashSet<int[]>(new IntArrayComparer()) { joltages };

                int clicks = 0;
                int length = joltages.Length;
                while (possibleState.Any())
                {
                    clicks++;
                    var nextState = new HashSet<int[]>(new IntArrayComparer());
                    foreach (var state in possibleState)
                    {
                        foreach (var button in Buttons)
                        {
                            var newState = state.ToArray();
                            for (int i = 0; i < length; i++)
                            {
                                if (button[i])
                                {
                                    --newState[i];
                                }
                            }

                            if (newState.All(x => x == 0))
                                return clicks;

                            if (newState.All(x => x != -1))
                                nextState.Add(newState);
                        }
                    }
                    possibleState = nextState;

                }

                throw new Exception("path not found");
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
            var sum = 0;
            Stopwatch timer = new Stopwatch();
            foreach (var machine in machines)
            {
                timer.Restart();
                var clicks = machine.FindPathToJoltages();
                Console.WriteLine($"Found path in {clicks} clicks after {timer.ElapsedMilliseconds} ms.");
                sum += clicks;
            }

            return sum;
        }
    }
}

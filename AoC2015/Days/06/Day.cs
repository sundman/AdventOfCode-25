using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ConsoleApp
{

    internal class Day06 : IDay
    {
        enum Command
        {
            Off,
            On,
            Toggle
        }

        readonly record struct Coordinate(int X, int Y);
        readonly record struct Instruction(Command Command, Coordinate Start, Coordinate Stop);

        private List<Instruction> instructions = [];

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");


            //turn off 301,3 through 808,453
            // turn on 351,678 through 951,908
            // toggle 720,196 through 897,994

            foreach (var line in data)
            {
                MatchCollection nums = Regex.Matches(line, @"\d+");
                Command command;
                if (line.StartsWith("turn off"))
                {
                    command = Command.Off;

                }
                else if (line.StartsWith("turn on"))
                {
                    command = Command.On;

                }
                else
                {
                    command = Command.Toggle;
                }

                instructions.Add(new Instruction(command,
                    new Coordinate(int.Parse(nums[0].Value), int.Parse(nums[1].Value)),
                    new Coordinate(int.Parse(nums[2].Value), int.Parse(nums[3].Value))
                ));
            }


        }

        private readonly record struct Area(Coordinate Start, Coordinate End, bool Status);

        private class Segment
        {
            public Segment prev;
            public Segment next;
            public int start;
            public int end;
            public bool status;
        }

        public decimal Part1()
        {
            var area = new bool[1000, 1000];
            foreach (var instruction in instructions)
            {
                for (int x = instruction.Start.X; x <= instruction.Stop.X; x++)
                {
                    for (int y = instruction.Start.Y; y <= instruction.Stop.Y; y++)
                    {
                        switch (instruction.Command)
                        {
                            case Command.Off:
                                area[x, y] = false; break;
                            case Command.On:
                                area[x, y] = true; break;
                            case Command.Toggle:
                                area[x, y] = !area[x, y]; break;

                        }
                    }
                }
            }

            return area.Cast<bool>().Count(b => b);

        }

        public decimal Part2()
        {
            var area = new int[1000, 1000];
            foreach (var instruction in instructions)
            {
                for (int x = instruction.Start.X; x <= instruction.Stop.X; x++)
                {
                    for (int y = instruction.Start.Y; y <= instruction.Stop.Y; y++)
                    {
                        switch (instruction.Command)
                        {
                            case Command.Off:
                                if (area[x, y] > 0)
                                    area[x, y] -= 1;
                                break;
                            case Command.On:
                                area[x, y] += 1; break;
                            case Command.Toggle:
                                area[x, y] += 2; break;

                        }
                    }
                }
            }

            return area.Cast<int>().Sum(b => b);
        }
    }
}

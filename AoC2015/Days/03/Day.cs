using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace ConsoleApp
{

    internal class Day03 : IDay
    {
        private List<int[]> moves;

        private const int X = 0;
        const int Y = 1;


        public static Dictionary<char, int[]> Directions = new Dictionary<char, int[]>
        {
            { '>', [1, 0] },
            { '<', [-1, 0] },
            { '^', [0, 1] },
            { 'v', [0, -1] }
        };


        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            moves = data[0].Select(x => Directions[x]).ToList();


        }

        public decimal Part1()
        {

            var visited = new HashSet<Tuple<int, int>> { new(0, 0) };

            int[] currentPosition = [0, 0];

            foreach (var move in moves)
            {
                currentPosition[X] += move[X];
                currentPosition[Y] += move[Y];

                visited.Add(new(currentPosition[X], currentPosition[Y]));
            }

            return visited.Count;
        }

        public decimal Part2()
        {

            var visited = new HashSet<Tuple<int, int>> { new(0, 0) };

            int[] currentPositionSanta = [0, 0];
            int[] currentPositionRobot = [0, 0];

            for (int i = 0; i < moves.Count; i += 2)
            {
                currentPositionSanta[X] += moves[i][X];
                currentPositionSanta[Y] += moves[i][Y];

                currentPositionRobot[X] += moves[i+1][X];
                currentPositionRobot[Y] += moves[i+1][Y];

                visited.Add(new(currentPositionSanta[X], currentPositionSanta[Y]));
                visited.Add(new(currentPositionRobot[X], currentPositionRobot[Y]));
            }

            return visited.Count;
        }
    }
}

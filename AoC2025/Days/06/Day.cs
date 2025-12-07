using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp
{
    internal class Day06 : IDay
    {
        enum Operator
        {
            Addition,
            Multipliction
        }

        class Problem
        {
            public List<long> Nums = [];
            public Operator Op;

           
        }

        private Problem[] Problems;
        private Problem[] ProblemsPart2;




        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");


            var l = data[data.Length - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            Problems = new Problem[l];
            ProblemsPart2 = new Problem[l];

            for (int i = 0; i < l; i++)
            {
                Problems[i] = new Problem();
                ProblemsPart2[i] = new Problem();
            }


            foreach (var row in data)
            {
                var nums = row.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (int.TryParse(nums[0], out _))
                {
                    for (int i = 0; i < nums.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(nums[i]))
                            Problems[i].Nums.Add(long.Parse(nums[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < nums.Length; i++)
                    {
                        Operator op = nums[i] == "+" ? Operator.Addition : Operator.Multipliction;
                        Problems[i].Op = op;
                    }

                }
            }


            var currentProblem = 0;
            ProblemsPart2[currentProblem].Op = Problems[currentProblem].Op;
            for (int i = 0; i < data[0].Length; i++)
            {
                List<int> digits = new List<int>();
                for (int y = 0; y < data.Length - 1; y++)
                {
                    if (data[y][i] != ' ')
                        digits.Add(data[y][i] - '0');
                }

                if (digits.Any())
                {
                    ProblemsPart2[currentProblem].Nums.Add(digits.Aggregate(0, (current, d) => current * 10 + d));
                }
                else
                {
                    currentProblem++;
                    ProblemsPart2[currentProblem].Op = Problems[currentProblem].Op;

                }


            }
        }

        public decimal Part1()
        {
            decimal totalSum = 0;

            foreach (var problem in Problems)
            {
                if (problem.Op == Operator.Addition)
                {
                    totalSum += problem.Nums.Sum();
                }
                else
                {
                    totalSum += problem.Nums.Aggregate((long)1, (a, b) => a * b);
                }
            }

            return totalSum;
        }

        public decimal Part2()
        {

            decimal totalSum = 0;
            foreach (var problem in ProblemsPart2)
            {
                if (problem.Op == Operator.Addition)
                {
                    totalSum += problem.Nums.Sum();
                }
                else
                {
                     totalSum += problem.Nums.Aggregate((long)1, (a, b) => a * b);
                }
            }

            return totalSum;

        }
    }
}

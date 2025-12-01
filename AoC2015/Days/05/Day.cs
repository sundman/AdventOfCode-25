using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace ConsoleApp
{
    internal static class StringVerifier
    {
        //private readonly string _toCheck;

        //public StringVerifier(string toCheck)
        //{
        //    _toCheck = toCheck;
        //}


        public static bool CheckNice(this string _toCheck)
        {
            char[] vovels = ['a', 'e', 'i', 'o', 'u'];
            int vovelCount = 0;


            List<Tuple<char, char>> badCombo =
            [
                new('a', 'b'), new('c', 'd'),
                new('p', 'q'), new('x', 'y')
            ];



            bool foundDouble = false;
            for (int i = 0; i < _toCheck.Length; i++)
            {
                var ch = _toCheck[i];
                if (vovels.Contains(ch))
                    vovelCount++;


                if (i != _toCheck.Length - 1)
                {
                    if (_toCheck[i] == _toCheck[i + 1])
                        foundDouble = true;

                    var firstBad = badCombo.FirstOrDefault(x => x.Item1 == ch);
                    if (firstBad != null && firstBad.Item2 == _toCheck[i + 1])
                        return false;
                }
            }

            return foundDouble && vovelCount >= 3;
        }

        public static bool CheckNicePart2(this string _toCheck)
        {
            List<Tuple<char, char>> badCombo =
            [
                new('a', 'b'), new('c', 'd'),
                new('p', 'q'), new('x', 'y')
            ];

            HashSet<Tuple<char, char>> tuples = new HashSet<Tuple<char, char>>();

            bool foundDouble = false;
            bool foundTuple = false;
            bool skipOne = false;
            for (int i = 0; i < _toCheck.Length - 1; i++)
            {
                Tuple<char, char> tuple = new(_toCheck[i], _toCheck[i + 1]);

                if (!skipOne)
                {
                    if (!tuples.Add(tuple))
                    {
                        foundTuple = true;
                    }
                }

                if (i != _toCheck.Length - 2)
                {
                    if (!skipOne && tuple.Item1 == tuple.Item2 && _toCheck[i + 2] == tuple.Item1)
                        skipOne = true;
                    else 
                        skipOne = false;

                    if (_toCheck[i] == _toCheck[i + 2])
                        foundDouble = true;
                }

            }

            return foundDouble && foundTuple;
        }

    }

    internal class Day05 : IDay
    {

        private string[] input;

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");
            input = data;

        }

        public decimal Part1()
        {
            return input.Count(x => x.CheckNice());
        }

        public decimal Part2()
        {

            return input.Count(x => x.CheckNicePart2());
        }
    }
}

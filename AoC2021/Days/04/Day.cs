using System.Diagnostics;

namespace ConsoleApp
{
    internal class Day04 : IDay
    {
        private List<int> _numbers = [];
        private readonly Dictionary<int, int> _numberDict = [];
        private readonly List<Bingo> _bingoSheets = [];

        private class Bingo
        {
            private List<int> Numbers = [];

            private List<int>[] RowsAndColumns = new List<int>[10];

            public Bingo(int[,] numbers)
            {
                for (var i = 0; i < 10; i++)
                {
                    RowsAndColumns[i] = [];
                }

                for (var x = 0; x < 5; x++)
                {
                    for (var y = 0; y < 5; y++)
                    {
                        RowsAndColumns[x].Add(numbers[x, y]);
                        RowsAndColumns[5 + y].Add(numbers[x, y]);
                        Numbers.Add(numbers[x, y]);
                    }
                }
            }

            public int Score(List<int> drawnNumbers)
            {
                return Numbers.Where(x => !drawnNumbers.Contains(x)).Sum();
            }

            public int CalculateBingoRound(Dictionary<int, int> numberDict)
            {
                return RowsAndColumns.Min(list => list.Max(number => numberDict[number]));
            }
        }

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            _numbers = data[0].Split(',').Select(int.Parse).ToList();

            for (int x = 0; x < _numbers.Count; x++)
            {
                _numberDict[_numbers[x]] = x;
            }

            var i = 2;
            while (data.Length > i)
            {
                int[,] numbers = new int[5, 5];
                for (int y = 0; y < 5; y++)
                {
                    var rowNumbers = data[i + y].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    for (int x = 0; x < 5; x++)
                    {
                        numbers[x, y] = rowNumbers[x];
                    }
                }
                _bingoSheets.Add(new Bingo(numbers));
                i += 6;
            }

        }



        public decimal Part1()
        {
            Bingo? winning = null;
            int? winningRound = null;
            foreach (var sheet in _bingoSheets)
            {
                var result = sheet.CalculateBingoRound(_numberDict);
                if (winningRound == null || result < winningRound)
                {
                    winningRound = result;
                    winning = sheet;
                }

            }

            return (decimal)_numbers[winningRound.Value] * winning.Score(_numbers.GetRange(0, winningRound.Value + 1));
        }

        public decimal Part2()
        {
            Bingo? winning = null;
            int? winningRound = null;
            foreach (var sheet in _bingoSheets)
            {
                var result = sheet.CalculateBingoRound(_numberDict);
                if (winningRound == null || result > winningRound)
                {
                    winningRound = result;
                    winning = sheet;
                }

            }

            return (decimal)_numbers[winningRound.Value] * winning.Score(_numbers.GetRange(0, winningRound.Value + 1));
            
        }
     
    }
}

using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace ConsoleApp
{

    internal class Day04 : IDay
    {
        private string input;

        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");
            input = data[0];

        }

        public decimal Part1()
        {
            int i = 0;
            var md5 = MD5.Create();
            while (true)
            {

                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input + i);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                if (hashBytes[0] == 0 && hashBytes[1] == 0 && hashBytes[2] < 16)
                {
                    return i;
                }
                i++;
            }

            return -1;
        }

        public decimal Part2()
        {
            int i = 0;
            var md5 = MD5.Create();
            while (true)
            {

                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input + i);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                if (hashBytes[0] == 0 && hashBytes[1] == 0 && hashBytes[2] == 0)
                {
                    return i;
                }
                i++;
            }

            return -1;
        }
    }
}

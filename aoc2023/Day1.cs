using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day1
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day1.txt");

            var sum = 0;

            foreach (var line in data)
            {
                int first = line.First(Char.IsDigit);
                int last = line.Last(Char.IsDigit);

                int num = 10 * (first - '0') + (last - '0');

                sum += num;
            }


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day1.txt");

            /*
            data = new []
            {
                "two1nine",
                "eightwothree",
                "abcone2threexyz",
                "xtwone3four",
                "4nineeightseven2",
                "zoneight234",
                "7pqrstsixteen",
            };
            */

            var replacements = new[]
            {
                new[] { "zero", "0" },
                new[] { "one", "1" },
                new[] { "two", "2" },
                new[] { "three", "3" },
                new[] { "four", "4" },
                new[] { "five", "5" },
                new[] { "six", "6" },
                new[] { "seven", "7" },
                new[] { "eight", "8" },
                new[] { "nine", "9" },
            };

            var sum = 0;

            foreach (var origLine in data)
            {
                var line = origLine;

                var p = 0;
                for (p = 0; p < line.Length; p++)
                {
                    foreach (var r in replacements)
                    {
                        if (line.Substring(p).StartsWith(r[0]))
                        {
                            line = line.Substring(0, p) + r[1] + line.Substring(p + 1);
                        }

                    }
                }

                int first = line.First(Char.IsDigit);
                int last = line.Last(Char.IsDigit);

                int num = 10 * (first - '0') + (last - '0');

                sum += num;
            }


            Console.WriteLine($"Answer is {sum}");
        }

    }
}

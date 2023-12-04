using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day4
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day4.txt");

            /*
            data = new[]
            {
                "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
                "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
                "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
                "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
                "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
                "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
            };
            */

            var values = data.Select(r => r.Split(new []{':','|'}).Select(k => k.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray()).ToArray();

            int sum = 0;

            foreach (var value in values)
            {
                int nMatch = 0;

                nMatch = value[2].Select(v => value[1].Any(n => n == v) ? 1 : 0).Sum();

                if (nMatch > 0)
                {
                    sum += 1 << (nMatch - 1);
                }
            }


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day4.txt");

            /*
            data = new[]
            {
                "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
                "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
                "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
                "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
                "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
                "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
            };
            */

            var values = data.Select(r => r.Substring(4).Split(new[] { ':', '|' }).Select(k => k.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray()).ToArray()).ToArray();

            int sum = 0;

            int maxCard = values.Select(v => v[0][0]).Max();

            var extraCopies = new int[values.Length + 1];

            foreach (var value in values)
            {
                int nMatch = 0;

                nMatch = value[2].Select(v => value[1].Any(n => n == v) ? 1 : 0).Sum();

                for (int i = 0; i < nMatch; i++)
                {
                    extraCopies[value[0][0] + i + 1] += 1 + extraCopies[value[0][0]];
                }
            }


            Console.WriteLine($"Answer is {values.Length + extraCopies.Sum()}");
        }

    }
}

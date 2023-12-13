using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day13
    {
        long CalcPart1(List<string> cave)
        {
            // Check horz
            for (int i = 1; i < cave.Count; i++)
            {
                bool match = true;

                for (int ofs = 0; match && ofs < Math.Min(i, cave.Count - i); ofs++)
                {
                    if (cave[i + ofs] != cave[i - 1 - ofs])
                    {
                        match = false;
                    }
                }

                if (match)
                {
                    return 100 * i;
                }
            }

            // Check vert
            for (int i = 1; i < cave[0].Length; i++)
            {
                bool match = true;

                for (int ofs = 0; match && ofs < Math.Min(i, cave[0].Length - i); ofs++)
                {
                    foreach (var line in cave)
                    {
                        if (line[i + ofs] != line[i - 1 - ofs])
                        {
                            match = false;
                        }
                    }
                }

                if (match)
                {
                    return i;
                }
            }

            throw new Exception("Could not find a match");
        }

        long CalcPart2(List<string> cave)
        {
            // Check horz
            for (int i = 1; i < cave.Count; i++)
            {
                int diffs = 0;

                for (int ofs = 0; ofs < Math.Min(i, cave.Count - i); ofs++)
                {
                    for (int idx = 0; idx < cave[0].Length; idx++)
                    {
                        if (cave[i + ofs][idx] != cave[i - 1 - ofs][idx])
                        {
                            diffs++;
                        }
                    }
                }

                if (diffs == 1)
                {
                    return 100 * i;
                }
            }

            // Check vert
            for (int i = 1; i < cave[0].Length; i++)
            {
                int diffs = 0;

                for (int ofs = 0; ofs < Math.Min(i, cave[0].Length - i); ofs++)
                {
                    foreach (var line in cave)
                    {
                        if (line[i + ofs] != line[i - 1 - ofs])
                        {
                            diffs++;
                        }
                    }
                }

                if (diffs == 1)
                {
                    return i;
                }
            }

            throw new Exception("Could not find a match");
        }


        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day13.txt");

            /*
            data = new[]
            {
                "#.##..##.",
                "..#.##.#.",
                "##......#",
                "##......#",
                "..#.##.#.",
                "..##..##.",
                "#.#.##.#.",
                "",
                "#...##..#",
                "#....#..#",
                "..##..###",
                "#####.##.",
                "#####.##.",
                "..##..###",
                "#....#..#",
            };
            */

            List<string> cave = new List<string>();

            long sum = 0;

            foreach (var line in data)
            {
                if (line.Length > 0)
                {
                    cave.Add(line);
                }
                else
                {
                    var ll = CalcPart1(cave);
                    Console.WriteLine($"Found at {ll}");
                    sum += ll;
                    cave = new List<string>();
                }
            }

            var l2 = CalcPart1(cave);
            Console.WriteLine($"Found at {l2}");
            sum += l2;


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day13.txt");

            /*
            data = new[]
            {
                "#.##..##.",
                "..#.##.#.",
                "##......#",
                "##......#",
                "..#.##.#.",
                "..##..##.",
                "#.#.##.#.",
                "",
                "#...##..#",
                "#....#..#",
                "..##..###",
                "#####.##.",
                "#####.##.",
                "..##..###",
                "#....#..#",
            };
            */

            List<string> cave = new List<string>();

            long sum = 0;

            foreach (var line in data)
            {
                if (line.Length > 0)
                {
                    cave.Add(line);
                }
                else
                {
                    var ll = CalcPart2(cave);
                    Console.WriteLine($"Found at {ll}");
                    sum += ll;
                    cave = new List<string>();
                }
            }

            var l2 = CalcPart2(cave);
            Console.WriteLine($"Found at {l2}");
            sum += l2;


            Console.WriteLine($"Answer is {sum}");
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc2023.Structs;

namespace aoc2023
{
    internal class Day11
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day11.txt");

            /*
            data = new[]
            {
                "...#......",
                ".......#..",
                "#.........",
                "..........",
                "......#...",
                ".#........",
                ".........#",
                "..........",
                ".......#..",
                "#...#.....",
            };
            */

            var vv = data.Select(r => r.Select(c => c).ToList()).ToList();

            var ls = vv[0].Select(c => c).ToArray();

            foreach (var v in vv)
            {
                Console.WriteLine(new string(v.ToArray()));
            }
            Console.WriteLine();

            // rows
            for (int i = vv.Count - 1; i >= 0; i--)
            {
                if (vv[i].All(c => c != '#'))
                {
                    vv.Insert(i, vv[i].Select(c => c).ToList());
                }
                else
                {
                    // Collect all rows with galaxies
                    for (int k = 0; k < vv[i].Count; k++)
                    {
                        if (vv[i][k] == '#')
                        {
                            ls[k] = '#';
                        }
                    }

                }

            }

            // cols
            // Do it backwards for simplicity
            for (int i = ls.Length - 1; i >= 0; i--)
            {
                if (ls[i] == '.')
                {
                    foreach (var v in vv)
                    {
                        v.Insert(i, '.');
                    }
                }
            }

            foreach (var v in vv)
            {
                Console.WriteLine(new string(v.ToArray()));
            }
            Console.WriteLine();

            // Create a list of all galaxies
            var galaxies = new List<Point>();

            for (int i = 0; i < vv.Count; i++)
            {
                for (int k = 0; k < vv[i].Count; k++)
                {
                    if (vv[i][k] == '#')
                    {
                        galaxies.Add(new Point(k, i));
                    }
                }
            }

            long sum = 0;
            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                for (int k = i + 1; k < galaxies.Count; k++)
                {
                    sum += galaxies[i].ManhattanDist(galaxies[k]);
                }

            }


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day11.txt");

            /*
            data = new[]
            {
                "...#......",
                ".......#..",
                "#.........",
                "..........",
                "......#...",
                ".#........",
                ".........#",
                "..........",
                ".......#..",
                "#...#.....",
            };
            */
            

            var vv = data.Select(r => r.Select(c => c).ToList()).ToList();

            var ls = vv[0].Select(c => c).ToArray();

            var emptyRows = new List<int>();
            var emptyCols = new List<int>();

            foreach (var v in vv)
            {
                Console.WriteLine(new string(v.ToArray()));
            }
            Console.WriteLine();

            // rows
            for (int i = vv.Count - 1; i >= 0; i--)
            {
                if (vv[i].All(c => c != '#'))
                {
                    emptyRows.Add(i);
                }
                else
                {
                    // Collect all rows with galaxies
                    for (int k = 0; k < vv[i].Count; k++)
                    {
                        if (vv[i][k] == '#')
                        {
                            ls[k] = '#';
                        }
                    }

                }

            }

            // cols
            // Do it backwards for simplicity
            for (int i = ls.Length - 1; i >= 0; i--)
            {
                if (ls[i] == '.')
                {
                    emptyCols.Add(i);
                }
            }

            // Create a list of all galaxies
            var galaxies = new List<Point>();

            for (int i = 0; i < vv.Count; i++)
            {
                for (int k = 0; k < vv[i].Count; k++)
                {
                    if (vv[i][k] == '#')
                    {
                        galaxies.Add(new Point(k, i));
                    }
                }
            }

            long expandConst = (1000000 - 1);

            long sum = 0;
            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                for (int k = i + 1; k < galaxies.Count; k++)
                {
                    long d = galaxies[i].ManhattanDist(galaxies[k]);
                    var minx = Math.Min(galaxies[i].X, galaxies[k].X);
                    var maxx = Math.Max(galaxies[i].X, galaxies[k].X);
                    var miny = Math.Min(galaxies[i].Y, galaxies[k].Y);
                    var maxy = Math.Max(galaxies[i].Y, galaxies[k].Y);

                    for (int r = minx + 1; r < maxx; r++)
                    {
                        if (emptyCols.Contains(r))
                        {
                            d += expandConst;
                        }
                    }
                    for (int r = miny + 1; r < maxy; r++)
                    {
                        if (emptyRows.Contains(r))
                        {
                            d += expandConst;
                        }
                    }

                    sum += d;
                }

            }


            Console.WriteLine($"Answer is {sum}");
        }

    }
}

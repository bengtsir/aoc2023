using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day14
    {
        internal const long HashSize = 16387000000;

        internal long Hash(ref char[][] board)
        {
            long sum = 0;

            for (int r = 1; r < board.Length - 1; r++)
            {
                for (int k = 1; k < board[0].Length - 1; k++)
                {
                    if (board[r][k] == 'O')
                    {
                        sum += r * board.Length + k;
                    }
                }

            }

            return sum % HashSize;
        }

        internal void Tilt(ref char[][] board, int direction = 0)
        {
            if (direction == 0) // up
            {
                for (int i = 1; i < board[0].Length - 1; i++)
                {
                    for (int r = 1; r < board.Length - 1; r++)
                    {
                        if (board[r][i] == 'O')
                        {
                            int p = r;

                            while (board[p - 1][i] == '.')
                            {
                                board[p - 1][i] = 'O';
                                board[p][i] = '.';
                                p--;
                            }
                        }
                    }
                }
            }
            else if (direction == 1) // left
            {
                for (int r = 1; r < board.Length - 1; r++)
                {
                    for (int i = 1; i < board[0].Length - 1; i++)
                    {
                        if (board[r][i] == 'O')
                        {
                            int p = i;

                            while (board[r][p - 1] == '.')
                            {
                                board[r][p - 1] = 'O';
                                board[r][p] = '.';
                                p--;
                            }
                        }
                    }
                }
            }
            else if (direction == 2) // down
            {
                for (int i = 1; i < board[0].Length - 1; i++)
                {
                    for (int r = board.Length - 1; r >= 1; r--)
                    {
                        if (board[r][i] == 'O')
                        {
                            int p = r;

                            while (board[p + 1][i] == '.')
                            {
                                board[p + 1][i] = 'O';
                                board[p][i] = '.';
                                p++;
                            }
                        }
                    }
                }
            }
            else if (direction == 3) // right
            {
                for (int r = 1; r < board.Length - 1; r++)
                {
                    for (int i = board[0].Length - 1; i >= 1; i--)
                    {
                        if (board[r][i] == 'O')
                        {
                            int p = i;

                            while (board[r][p + 1] == '.')
                            {
                                board[r][p + 1] = 'O';
                                board[r][p] = '.';
                                p++;
                            }
                        }
                    }
                }
            }
        }

        int Weight(ref char[][] board)
        {
            int sum = 0;
            var rowval = board.Length - 2;

            for (int r = 1; r < board.Length - 1; r++)
            {
                for (int i = 1; i < board[0].Length - 1; i++)
                {
                    if (board[r][i] == 'O')
                    {
                        sum += rowval;
                    }
                }

                rowval--;
            }

            return sum;
        }


        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day14.txt");

            var values = ArrayMethods.AddBorder(1, '#', data).Select(s => s.Select(c => c).ToArray()).ToArray();

            Tilt(ref values, 0);


            var sum = Weight(ref values);


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day14.txt");

            /*
            data = new[]
            {
                "O....#....",
                "O.OO#....#",
                ".....##...",
                "OO.#O....O",
                ".O.....O#.",
                "O.#..O.#.#",
                "..O..#O..O",
                ".......O..",
                "#....###..",
                "#OO..#....",
            };
            */

            var values = ArrayMethods.AddBorder(1, '#', data).Select(s => s.Select(c => c).ToArray()).ToArray();

            var hashes = new List<long>();

            for (long c = 0; c < 2000; c++)
            {
                for (int d = 0; d < 4; d++)
                {
                    Tilt(ref values, d);
                }

                hashes.Add(Hash(ref values));
            }

            // Find period
            int skip = 1000;
            var periods = new List<long>();

            for (int p = 2; p < 150 + 2; p++)
            {
                var pp = 3*hashes[skip] - hashes[skip + p] - hashes[skip + 2*p] - hashes[skip + 3*p];
                periods.Add(pp);
            }

            var period = 0;
            while (period < periods.Count && periods[period] != 0)
            {
                period++;
            }

            period += 2; // It's offsetted...

            var periodsString = string.Join(" ", periods.Select(v => v.ToString()).ToArray());
            Console.WriteLine($"Periods: {periodsString}");
            Console.WriteLine($"Detected period = {period}");

            // Calculate number of cycles to 1000000000, reduced

            var nCycles = ((1000000000 - skip) % period) + skip;

            // Redo the whole stuff

            values = ArrayMethods.AddBorder(1, '#', data).Select(s => s.Select(c => c).ToArray()).ToArray();

            for (long c = 0; c < nCycles; c++)
            {
                for (int d = 0; d < 4; d++)
                {
                    Tilt(ref values, d);
                }

                //Console.WriteLine($"Weight: {Weight(ref values)}");
            }


            Console.WriteLine($"Answer is {Weight(ref values)}");
        }

    }
}

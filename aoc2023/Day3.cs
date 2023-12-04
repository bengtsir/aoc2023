using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class GearNumber
    {
        public int Number { get; set; }
        public int StarX { get; set; }
        public int StarY { get; set; }
    }

    internal class Day3
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day3.txt");

            /*
            data = new[]
            {
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598..",
            };
            */

            var filler = new string('.', data[0].Length);

            // Add stopper at top and bottom
            var t = new List<string>();
            t.Add(filler);
            t.AddRange(data);
            t.Add(filler);

            data = t.ToArray();

            int sum = 0;

            for (int i = 1; i < data.Length-1; i++)
            {
                bool inNumber = false;
                bool symbolFound = false;
                int number = 0;

                for (int k = 0; k < data[i].Length; k++)
                {
                    if (!Char.IsDigit(data[i][k]))
                    {
                        if (inNumber)
                        {
                            if (symbolFound)
                            {
                                sum += number;
                            }

                            inNumber = false;
                            symbolFound = false;
                            number = 0;
                        }
                    }
                    else
                    {
                        number = (number * 10) + (data[i][k] - '0');
                        inNumber = true;

                        if (!Char.IsDigit(data[i - 1][k]) && data[i - 1][k] != '.')
                        {
                            symbolFound = true;
                        }
                        if (!Char.IsDigit(data[i + 1][k]) && data[i + 1][k] != '.')
                        {
                            symbolFound = true;
                        }
                        if (k > 0)
                        {
                            if (!Char.IsDigit(data[i-1][k-1]) && data[i - 1][k - 1] != '.')
                            {
                                symbolFound = true;
                            }
                            if (!Char.IsDigit(data[i][k-1]) && data[i][k-1] != '.')
                            {
                                symbolFound = true;
                            }
                            if (!Char.IsDigit(data[i + 1][k - 1]) && data[i + 1][k - 1] != '.')
                            {
                                symbolFound = true;
                            }
                        }
                        if (k < data[i].Length - 1)
                        {
                            if (!Char.IsDigit(data[i - 1][k + 1]) && data[i - 1][k + 1] != '.')
                            {
                                symbolFound = true;
                            }
                            if (!Char.IsDigit(data[i][k + 1]) && data[i][k + 1] != '.')
                            {
                                symbolFound = true;
                            }
                            if (!Char.IsDigit(data[i + 1][k + 1]) && data[i + 1][k + 1] != '.')
                            {
                                symbolFound = true;
                            }
                        }
                    }
                }

                if (inNumber)
                {
                    if (symbolFound)
                    {
                        sum += number;
                    }
                }

            }
            
            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day3.txt");

            /*
            data = new[]
            {
                "467..114..",
                "...*......",
                "..35..633.",
                "......#...",
                "617*......",
                ".....+.58.",
                "..592.....",
                "......755.",
                "...$.*....",
                ".664.598..",
            };
            */

            var filler = new string('.', data[0].Length);

            // Add stopper at top and bottom
            var t = new List<string>();
            t.Add(filler);
            t.AddRange(data);
            t.Add(filler);

            data = t.ToArray();

            var gearCandidates = new List<GearNumber>();

            int sum = 0;

            for (int i = 1; i < data.Length - 1; i++)
            {
                bool inNumber = false;
                bool symbolFound = false;
                int number = 0;
                int gearx = 0;
                int geary = 0;

                for (int k = 0; k < data[i].Length; k++)
                {
                    if (!Char.IsDigit(data[i][k]))
                    {
                        if (inNumber)
                        {
                            if (symbolFound)
                            {
                                gearCandidates.Add(new GearNumber(){Number = number, StarX = gearx, StarY = geary});
                            }

                            inNumber = false;
                            symbolFound = false;
                            number = 0;
                            gearx = 0;
                            geary = 0;
                        }
                    }
                    else
                    {
                        number = (number * 10) + (data[i][k] - '0');
                        inNumber = true;

                        if (data[i - 1][k] == '*')
                        {
                            symbolFound = true;
                            gearx = k;
                            geary = i - 1;
                        }
                        if (data[i + 1][k] == '*')
                        {
                            symbolFound = true;
                            gearx = k;
                            geary = i + 1;
                        }
                        if (k > 0)
                        {
                            if (data[i - 1][k - 1] == '*')
                            {
                                symbolFound = true;
                                gearx = k-1;
                                geary = i - 1;
                            }
                            if (data[i][k - 1] == '*')
                            {
                                symbolFound = true;
                                gearx = k-1;
                                geary = i;
                            }
                            if (data[i + 1][k - 1] == '*')
                            {
                                symbolFound = true;
                                gearx = k - 1;
                                geary = i + 1;
                            }
                        }
                        if (k < data[i].Length - 1)
                        {
                            if (data[i - 1][k + 1] == '*')
                            {
                                symbolFound = true;
                                gearx = k + 1;
                                geary = i - 1;
                            }
                            if (data[i][k + 1] == '*')
                            {
                                symbolFound = true;
                                gearx = k + 1;
                                geary = i;
                            }
                            if (data[i + 1][k + 1] == '*')
                            {
                                symbolFound = true;
                                gearx = k + 1;
                                geary = i + 1;
                            }
                        }
                    }
                }

                if (inNumber)
                {
                    if (symbolFound)
                    {
                        gearCandidates.Add(new GearNumber() { Number = number, StarX = gearx, StarY = geary });
                    }
                }

            }

            sum = 0;

            while (gearCandidates.Count > 0)
            {
                var c = gearCandidates.First();
                gearCandidates.RemoveAt(0);

                var count = gearCandidates.Count(cc => cc.StarX == c.StarX && cc.StarY == c.StarY);
                if (count > 1)
                {
                    gearCandidates.RemoveAll(cc => cc.StarX == c.StarX && cc.StarY == c.StarY);
                }
                else if (count == 1)
                {
                    var other = gearCandidates.First(cc => cc.StarX == c.StarX && cc.StarY == c.StarY);

                    gearCandidates.Remove(other);

                    sum += c.Number * other.Number;
                }
            }

            Console.WriteLine($"Answer is {sum}");
        }

    }
}

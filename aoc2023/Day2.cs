using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day2
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day2.txt");

            /*
            data = new[]
            {
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green;, 1 blue",
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green,; 1 red",
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
            };
            */

            var values = data.Select(r => r.Split(' ')).ToArray();

            int sum = 0;

            foreach (var game in values)
            {
                var gameno = Convert.ToInt32(game[1].Substring(0, game[1].Length - 1));

                int blue = 0;
                int red = 0;
                int green = 0;

                var i = 2;

                bool possible = true;

                while (i < game.Length)
                {
                    int num = Convert.ToInt32(game[i]);

                    char sep = game[i + 1].Last();

                    switch (game[i+1][0])
                    {
                        case 'b':
                            blue = num;
                            break;
                        case 'r':
                            red = num;
                            break;
                        case 'g':
                            green = num;
                            break;
                    }

                    if (sep != ',')
                    {
                        if (red > 12 || green > 13 || blue > 14)
                        {
                            possible = false;
                        }
                    }

                    i += 2;
                }

                if (possible)
                {
                    sum += gameno;
                }
            }


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day2.txt");

            /*
            data = new[]
            {
                "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
                "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green;, 1 blue",
                "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green,; 1 red",
                "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
                "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
            };
            */

            var values = data.Select(r => r.Split(' ')).ToArray();

            int sum = 0;

            foreach (var game in values)
            {
                var gameno = Convert.ToInt32(game[1].Substring(0, game[1].Length - 1));

                int blue = 0;
                int red = 0;
                int green = 0;

                var i = 2;

                while (i < game.Length)
                {
                    int num = Convert.ToInt32(game[i]);

                    char sep = game[i + 1].Last();

                    switch (game[i + 1][0])
                    {
                        case 'b':
                            if (blue < num)
                            {
                                blue = num;
                            }
                            break;
                        case 'r':
                            if (red < num)
                            {
                                red = num;
                            }
                            break;
                        case 'g':
                            if (green < num)
                            {
                                green = num;
                            }
                            break;
                    }

                    i += 2;
                }

                sum += blue * red * green;
            }


            Console.WriteLine($"Answer is {sum}");
        }

    }
}

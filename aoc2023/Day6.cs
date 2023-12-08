using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day6
    {
        // Finds the roots of ax^2 + bx + c = 0
        public void Roots(double a, double b, double c, out double r1, out double r2)
        {
            r1 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
            r2 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day6.txt");

            /*
            data = new[]
            {
                "Time:      7  15   30",
                "Distance:  9  40  200",
            };
            */

            var values = data.Select(r => r.Substring(10).Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray()).ToArray();

            long prod = 1;

            for (int i = 0; i < values[0].Length; i++)
            {
                int sum = 0;
                for (int t = 1; t < values[0][i]; t++)
                {
                    if (t * (values[0][i] - t) > values[1][i])
                    {
                        sum++;
                    }
                }

                prod = prod * sum;
            }


            Console.WriteLine($"Answer is {prod}");

            // Solution 2

            prod = 1;

            for (int i = 0; i < values[0].Length; i++)
            {
                Roots(1, -values[0][i], values[1][i], out var r1, out var r2);
                var sum = (long)(Math.Ceiling(r2)) - (long)(Math.Floor(r1)) - 1;

                prod *= sum;
            }

            Console.WriteLine($"Answer is {prod}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day6.txt");

            /*
            data = new[]
            {
                "Time:      7  15   30",
                "Distance:  9  40  200",
            };
            */

            var values = data.Select(r => Int64.Parse(r.Substring(10).Replace(" ", ""))).ToArray();

            long sum = 0;

            for (long i = 1; i < values[0]-1; i++)
            {
                if (i * (values[0] - i) > values[1])
                {
                    sum++;
                }
            }
            
            Console.WriteLine($"Answer is {sum}");

            // Solution 2

            Roots(1, -values[0], values[1], out var r1, out var r2);
            sum = (long)(Math.Ceiling(r2)) - (long)(Math.Floor(r1)) - 1;

            Console.WriteLine($"Answer is {sum}");
        }

    }
}

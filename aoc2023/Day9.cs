using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day9
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day9.txt");

            /*
            data = new[]
            {
                "0 3 6 9 12 15",
                "1 3 6 10 15 21",
                "10 13 16 21 30 45",
            };
            */

            var values = data.Select(r => r.Split(' ').Select(s => Int64.Parse(s)).ToArray()).ToArray();

            long sum = 0;

            foreach (var line in values)
            {
                var currVals = line;

                var history = new List<long[]>();

                while (currVals.Any(vv => vv != 0))
                {
                    history.Add(currVals);
                    
                    var newVals = new List<long>();

                    for (int i = 1; i < currVals.Length; i++)
                    {
                        newVals.Add(currVals[i] - currVals[i - 1]);
                    }

                    currVals = newVals.ToArray();
                }

                history.Reverse();

                long v = 0;

                foreach (var h in history)
                {
                    v = v + h[h.Length - 1];
                }

                //Console.WriteLine(v);

                sum += v;
            }


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day9.txt");

            /*
            data = new[]
            {
                "0 3 6 9 12 15",
                "1 3 6 10 15 21",
                "10 13 16 21 30 45",
            };
            */

            var values = data.Select(r => r.Split(' ').Select(s => Int64.Parse(s)).ToArray()).ToArray();

            long sum = 0;

            foreach (var line in values)
            {
                var currVals = line;

                var history = new List<long[]>();

                while (currVals.Any(vv => vv != 0))
                {
                    history.Add(currVals);

                    var newVals = new List<long>();

                    for (int i = 1; i < currVals.Length; i++)
                    {
                        newVals.Add(currVals[i] - currVals[i - 1]);
                    }

                    currVals = newVals.ToArray();
                }

                history.Reverse();

                long v = 0;

                foreach (var h in history)
                {
                    v = h[0] - v;
                }

                //Console.WriteLine(v);

                sum += v;
            }


            Console.WriteLine($"Answer is {sum}");
        }

    }
}

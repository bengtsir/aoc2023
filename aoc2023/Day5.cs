using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc2023.Structs;

namespace aoc2023
{
    internal class Day5
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day5.txt");

            /*
            data = new[]
            {
                "seeds: 79 14 55 13",
                "",
                "seed-to-soil map:",
                "50 98 2",
                "52 50 48",
                "",
                "soil-to-fertilizer map:",
                "0 15 37",
                "37 52 2",
                "39 0 15",
                "",
                "fertilizer-to-water map:",
                "49 53 8",
                "0 11 42",
                "42 0 7",
                "57 7 4",
                "",
                "water-to-light map:",
                "88 18 7",
                "18 25 70",
                "",
                "light-to-temperature map:",
                "45 77 23",
                "81 45 19",
                "68 64 13",
                "",
                "temperature-to-humidity map:",
                "0 69 1",
                "1 0 69",
                "",
                "humidity-to-location map:",
                "60 56 37",
                "56 93 4",
            };
            */
            
            var seeds = data[0].Substring(6).Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt64(s)).ToArray();

            var locs = new List<long>();

            // Add an empty string at end to simplify things
            var tempList = data.ToList();
            tempList.Add("");

            data = tempList.ToArray();

            var map = new List<long[]>();

            foreach (var line in data.Skip(2))
            {
                if (line.Contains("map:"))
                {
                    map.Clear();
                }
                else if (line.Length < 3)
                {
                    // Do the stuff
                    for (int i = 0; i < seeds.Length; i++)
                    {
                        var repl = false;

                        foreach (var m in map)
                        {
                            if (!repl && seeds[i] >= m[1] && seeds[i] < m[1] + m[2])
                            {
                                seeds[i] = m[0] + seeds[i] - m[1];
                                repl = true;
                            }
                        }
                    }
                }
                else
                {
                    map.Add(line.Split(' ').Select(s => Convert.ToInt64(s)).ToArray());
                }
            }
            
            Console.WriteLine($"Answer is {seeds.Min()}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day5.txt");

            /*
            data = new[]
            {
                "seeds: 79 14 55 13",
                "",
                "seed-to-soil map:",
                "50 98 2",
                "52 50 48",
                "",
                "soil-to-fertilizer map:",
                "0 15 37",
                "37 52 2",
                "39 0 15",
                "",
                "fertilizer-to-water map:",
                "49 53 8",
                "0 11 42",
                "42 0 7",
                "57 7 4",
                "",
                "water-to-light map:",
                "88 18 7",
                "18 25 70",
                "",
                "light-to-temperature map:",
                "45 77 23",
                "81 45 19",
                "68 64 13",
                "",
                "temperature-to-humidity map:",
                "0 69 1",
                "1 0 69",
                "",
                "humidity-to-location map:",
                "60 56 37",
                "56 93 4",
            };
            */

            var seeds = data[0].Substring(6).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt64(s)).ToList();

            var locs = new List<Segment>();

            for (int i = 0; i < seeds.Count; i += 2)
            {
                locs.Add(new Segment(seeds[i], seeds[i] + seeds[i + 1] - 1));
            }

            // Add an empty string at end to simplify things
            var tempList = data.ToList();
            tempList.Add("");

            data = tempList.ToArray();

            var map = new List<long[]>();

            foreach (var line in data.Skip(2))
            {
                if (line.Contains("map:"))
                {
                    map.Clear();
                }
                else if (line.Length < 3)
                {
                    // Do the stuff

                    var newList = new List<Segment>();

                    while (locs.Any())
                    {
                        Segment curr = locs.First();
                        locs.RemoveAt(0);

                        var repl = false;

                        foreach (var m in map)
                        {
                            Segment ms = new Segment(m[1], m[1] + m[2] - 1);

                            if (!repl && curr.Intersects(ms))
                            {
                                var splits = ArrayMethods.SplitSegment(curr, ms);

                                foreach (var seg in splits)
                                {
                                    if (seg.Intersects(ms))
                                    {
                                        seg.Shift(m[0] - m[1]);
                                    }
                                    newList.Add(seg);
                                }

                                repl = true;
                            }
                        }

                        if (!repl)
                        {
                            newList.Add(curr);
                        }
                    }

                    locs = newList;
                }
                else
                {
                    map.Add(line.Split(' ').Select(s => Convert.ToInt64(s)).ToArray());
                }
            }

            var min = locs.Min(l => l.Start);

            Console.WriteLine($"Answer is {min}");
        }

    }
}

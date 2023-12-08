using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class MapNode
    {
        public string Name { get; }

        public string Left { get; }
        public string Right { get; }

        public bool IsStarter { get; }
        public bool IsEnder { get; }

        public MapNode(string s)
        {
            var vals = s.Split(new[] { ' ', '=', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            Name = vals[0];
            Left = vals[1];
            Right = vals[2];

            IsStarter = Name[2] == 'A';
            IsEnder = Name[2] == 'Z';
        }
    }

    internal class Day8
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day8.txt");

            var instructions = data[0];

            var nodeList = data.Skip(2).Select(s => new MapNode(s)).ToArray();

            var nodes = new Dictionary<string, MapNode>();
            foreach (var mapNode in nodeList)
            {
                nodes.Add(mapNode.Name, mapNode);
            }

            var currentNode = nodes["AAA"];

            int ip = 0;
            int steps = 0;

            while (currentNode.Name != "ZZZ")
            {
                if (instructions[ip] == 'L')
                {
                    currentNode = nodes[currentNode.Left];
                }
                else
                {
                    currentNode = nodes[currentNode.Right];
                }

                steps++;
                ip++;
                if (ip >= instructions.Length)
                {
                    ip = 0;
                }
            }



            Console.WriteLine($"Answer is {steps}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day8.txt");

            /*
            data = new[]
            {
                "LR",
                "",
                "11A = (11B, XXX)",
                "11B = (XXX, 11Z)",
                "11Z = (11B, XXX)",
                "22A = (22B, XXX)",
                "22B = (22C, 22C)",
                "22C = (22Z, 22Z)",
                "22Z = (22B, 22B)",
                "XXX = (XXX, XXX)",
            };
            */

            var instructions = data[0];

            var nodeList = data.Skip(2).Select(s => new MapNode(s)).ToArray();

            var nodes = new Dictionary<string, MapNode>();
            foreach (var mapNode in nodeList)
            {
                nodes.Add(mapNode.Name, mapNode);
            }

            var currentNodes = nodeList.Where(n => n.IsStarter).ToArray();

            int ip = 0;
            int steps = 0;

            var starters = currentNodes.Select(n => 0).ToArray();
            var periods = currentNodes.Select(n => 0).ToArray();
            

            for (int i = 0; i < currentNodes.Length; i++)
            {
                var nn = currentNodes[i];
                var counts = new List<int>();

                ip = 0;
                steps = 0;

                while (counts.Count < 5)
                {
                    if (nn.IsEnder)
                    {
                        counts.Add(steps);
                        steps = 0;
                    }

                    nn = instructions[ip] == 'L' ? nodes[nn.Left] : nodes[nn.Right];
                    ip++;
                    steps++;
                    if (ip >= instructions.Length)
                    {
                        ip = 0;
                    }
                }
                Console.WriteLine($"Counts: {counts[0]} {counts[1]} {counts[2]} {counts[3]} {counts[4]}");

                starters[i] = counts[0];
                periods[i] = counts[1];
            }

            var breakdowns = periods.Select(p => ArrayMethods.PrimeFactors(p)).ToArray();

            for (int i = 0; i < breakdowns.Length; i++)
            {
                var bs = string.Join(" ", breakdowns[i].Select(e => e.ToString()).ToList());
                Console.WriteLine($"{periods[i]}: {bs}");
            }

            var minFactor = breakdowns.Select(b => b.Min()).Min();
            var maxFactor = breakdowns.Select(b => b.Max()).Max();

            var resultFactors = new List<long>();

            for (long i = minFactor; i <= maxFactor; i++)
            {
                var mf = breakdowns.Select(b => b.Where(e => e == i).Count()).Max();
                if (mf > 0)
                {
                    for (int k = 0; k < mf; k++)
                    {
                        resultFactors.Add(i);
                    }
                }
            }

            var rf = string.Join(" ", resultFactors.Select(e => e.ToString()).ToList());
            Console.WriteLine($"factors: {rf}");

            long prod = 1;
            foreach (var factor in resultFactors)
            {
                prod *= factor;
            }


            Console.WriteLine($"Answer is {prod}");
        }

    }
}

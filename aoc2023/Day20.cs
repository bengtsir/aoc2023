using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day20Node
    {
        public string Name { get; }
        public bool IsBroadcast => Name == "broadcaster";
        public bool IsFlipFlop { get; } = false;
        public bool IsConjunction { get; } = false;

        public bool IsOn { get; set; } = false;

        public List<string> Outputs { get; } = new List<string>();

        public Dictionary<string, bool> InputValues { get; } = new Dictionary<string, bool>();

        public List<int> FireIndexes { get; } = new List<int>();

        public Day20Node(string[] values)
        {
            if (values[0][0] == '%')
            {
                Name = values[0].Substring(1);
                IsFlipFlop = true;
            }
            else if (values[0][0] == '&')
            {
                Name = values[0].Substring(1);
                IsConjunction = true;
            }
            else
            {
                Name = values[0];
            }

            Outputs = values.Skip(1).ToList();
        }
    }

    internal class Day20Pulse
    {
        public string Source { get; set; }
        public string Dest { get; set; }
        public bool IsHigh { get; set; }

        public Day20Pulse(string source, string dest, bool isHigh)
        {
            Source = source;
            Dest = dest;
            IsHigh = isHigh;
        }
    }

    internal class Day20
    {
        internal Dictionary<string, Day20Node> Nodes = new Dictionary<string, Day20Node>();

        internal long LowPulsesSent = 0;
        internal long HighPulsesSent = 0;

        internal int Iterate(int nCycles)
        {
            for (int i = 0; i < nCycles; i++)
            {
                var pulseInput = new List<Day20Pulse> { new Day20Pulse("button", "broadcaster", false) };

                while (pulseInput.Any())
                {
                    var pulseOutput = new List<Day20Pulse>();

                    foreach (var p in pulseInput)
                    {
                        if (p.IsHigh)
                        {
                            HighPulsesSent++;
                        }
                        else
                        {
                            LowPulsesSent++;
                        }

                        if (!Nodes.ContainsKey(p.Dest))
                        {
                            if (nCycles > 10000 && !p.IsHigh)
                            {
                                return i + 1; // For part 2
                            }
                            continue; // This is an end node
                        }

                        var n = Nodes[p.Dest];

                        if (n.IsFlipFlop)
                        {
                            if (p.IsHigh)
                            {
                                // Do nothing
                            }
                            else
                            {
                                n.IsOn = !n.IsOn;
                                foreach (var o in n.Outputs)
                                {
                                    pulseOutput.Add(new Day20Pulse(n.Name, o, n.IsOn));
                                }
                            }
                        }
                        else if (n.IsConjunction)
                        {
                            n.InputValues[p.Source] = p.IsHigh;
                            bool outputVal = !n.InputValues.Values.All(v => v);
                            foreach (var o in n.Outputs)
                            {
                                pulseOutput.Add(new Day20Pulse(n.Name, o, outputVal));
                            }

                            if (!outputVal)
                            {
                                if (n.FireIndexes.Count < 10)
                                {
                                    n.FireIndexes.Add(i);
                                }
                            }
                        }
                        else
                        {
                            foreach (var o in n.Outputs)
                            {
                                pulseOutput.Add(new Day20Pulse(n.Name, o, p.IsHigh));
                            }
                        }
                    }

                    pulseInput = pulseOutput;
                }

                /*
                foreach (var n in Nodes.Values)
                {
                    if (n.IsConjunction)
                    {
                        Console.WriteLine(
                            $"{n.Name}: {new string(n.InputValues.Values.Select(b => b ? '1' : '0').ToArray())}");
                    }
                }
                Console.WriteLine();
                */
            }

            return 0;
        }


        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day20.txt");

            var values = data.Select(r => r.Split(new []{' ',',','-','>'}, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToList();

            foreach (var line in values)
            {
                var n = new Day20Node(line);
                Nodes[n.Name] = n;
            }

            // Fix inputs
            foreach (var n in Nodes.Values)
            {
                foreach (var o in n.Outputs)
                {
                    if (Nodes.ContainsKey(o))
                    {
                        Nodes[o].InputValues[n.Name] = false;
                    }
                }
            }


            Iterate(1000);


            Console.WriteLine($"Low pulses: {LowPulsesSent} High pulses: {HighPulsesSent} Answer is {LowPulsesSent * HighPulsesSent}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day20.txt");

            var values = data.Select(r => r.Split(new[] { ' ', ',', '-', '>' }, StringSplitOptions.RemoveEmptyEntries).ToArray()).ToList();

            foreach (var line in values)
            {
                var n = new Day20Node(line);
                Nodes[n.Name] = n;
            }

            // Fix inputs
            foreach (var n in Nodes.Values)
            {
                foreach (var o in n.Outputs)
                {
                    if (Nodes.ContainsKey(o))
                    {
                        Nodes[o].InputValues[n.Name] = false;
                    }
                }
            }


            var cycles = Iterate(100000);

            var product = (long) 1;

            foreach (var n in Nodes.Values.Where(n => n.IsConjunction))
            {
                var tail = string.Join(", ", n.FireIndexes.Select(i => i.ToString()));
                var inputs = string.Join(", ", n.InputValues.Keys);
                Console.WriteLine($"{n.Name}: {inputs}");
                Console.WriteLine($"{n.Name}: {tail}");
                string diff = "";
                for (int i = 1; i < n.FireIndexes.Count; i++)
                {
                    diff += $" {n.FireIndexes[i] - n.FireIndexes[i - 1]} ";
                }

                if (n.FireIndexes.Count > 2)
                {
                    var d = n.FireIndexes[2] - n.FireIndexes[1];

                    if (d > 10)
                    {
                        product *= (long)d;
                    }
                }

                Console.WriteLine(diff);
            }


            Console.WriteLine($"Low pulses: {LowPulsesSent} High pulses: {HighPulsesSent} Answer is {product}");
        }

    }
}

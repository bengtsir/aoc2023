using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day19Rule
    {
        public string Name { get; }
        public List<string> Rules { get; } = new List<string>();

        public Day19Rule(string name, string rules)
        {
            Name = name;
            Rules = rules.Replace("{", "").Replace("}", "").Split(',').ToList();
        }

        public string ApplyRules(int x, int m, int a, int s)
        {
            foreach (var r in Rules)
            {
                if (!r.Contains(':'))
                {
                    return r;
                }

                var rr = r.Split(':').ToArray();
                var dest = rr[1];
                var val = Convert.ToInt32(rr[0].Substring(2));
                var op = rr[0][1];
                var operand = rr[0][0];
                int cmp = 0;

                switch (operand)
                {
                    case 'x':
                        cmp = x;
                        break;
                    case 'm':
                        cmp = m;
                        break;
                    case 'a':
                        cmp = a;
                        break;
                    case 's':
                        cmp = s;
                        break;
                }

                if (op == '<')
                {
                    if (cmp < val)
                    {
                        return dest;
                    }
                }
                else
                {
                    if (cmp > val)
                    {
                        return dest;
                    }
                }
            }

            throw new Exception("No rule!");
        }
    }

    internal class Day19
    {
        internal Dictionary<string, Day19Rule> Rules = new Dictionary<string, Day19Rule>();

        private long sum = 0;

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day19.txt");

            /*
            data = new[]
            {
                "px{a<2006:qkq,m>2090:A,rfg}",
                "pv{a>1716:R,A}",
                "lnx{m>1548:A,A}",
                "rfg{s<537:gd,x>2440:R,A}",
                "qs{s>3448:A,lnx}",
                "qkq{x<1416:A,crn}",
                "crn{x>2662:A,R}",
                "in{s<1351:px,qqz}",
                "qqz{s>2770:qs,m<1801:hdj,R}",
                "gd{a>3333:R,R}",
                "hdj{m>838:A,pv}",
                "",
                "{x=787,m=2655,a=1222,s=2876}",
                "{x=1679,m=44,a=2067,s=496}",
                "{x=2036,m=264,a=79,s=2244}",
                "{x=2461,m=1339,a=466,s=291}",
                "{x=2127,m=1623,a=2188,s=1013}",
            };
            */

            bool inRules = true;

            for (int i = 0; i < data.Length; i++)
            {
                if (inRules)
                {
                    if (data[i].Length == 0)
                    {
                        inRules = false;
                    }
                    else
                    {
                        var ss = data[i].Split('{').ToArray();
                        Rules.Add(ss[0], new Day19Rule(ss[0], ss[1]));
                    }
                }
                else
                {
                    Console.WriteLine(data[i]);

                    // Do the stuff
                    var values = data[i].Split(new[] { '{', '=', ',', '}' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    int x = Int32.Parse(values[1]);
                    int m = Int32.Parse(values[3]);
                    int a = Int32.Parse(values[5]);
                    int s = Int32.Parse(values[7]);

                    var res = "in";
                    var rl = new List<string>();
                    while (res != "A" && res != "R")
                    {
                        rl.Add(res);
                        res = Rules[res].ApplyRules(x, m, a, s);
                    }
                    rl.Add(res);
                    Console.WriteLine(string.Join(" - ", rl));

                    if (res == "A")
                    {
                        var ss = x + m + a + s;
                        Console.WriteLine($"Adding {ss}");
                        sum += ss;
                    }
                }
            }

            Console.WriteLine($"Answer is {sum}");
        }

        internal long Part2Sum = 0;

        internal void AddSums(string rule, long x1, long x2, long m1, long m2, long a1, long a2, long s1, long s2)
        {
            Console.WriteLine($"{rule}: [{x1},{x2}] [{m1},{m2}] [{a1},{a2}] [{s1},{s2}]");
            if (rule == "A")
            {
                long ss = (x2 - x1 + 1) * (m2 - m1 + 1) * (a2 - a1 + 1) * (s2 - s1 + 1);
                Console.WriteLine($"Accepted, adding {ss}");
                Part2Sum += ss;
                return;
            }
            else if (rule == "R")
            {
                return;
            }

            // Do the heavy lifting

            foreach (var r in Rules[rule].Rules)
            {
                if (!r.Contains(':'))
                {
                    AddSums(r, x1, x2, m1, m2, a1, a2, s1, s2);
                    return;
                }

                var rr = r.Split(':').ToArray();
                var dest = rr[1];
                var val = Convert.ToInt64(rr[0].Substring(2));
                var op = rr[0][1];
                var operand = rr[0][0];

                switch (operand)
                {
                    case 'x':
                        if (op == '<')
                        {
                            if (x1 >= val)
                            {
                                continue;
                            }
                            else if (x2 < val)
                            {
                                // whole range
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, s2);
                                return;
                            }
                            else
                            {
                                // The trickier case
                                AddSums(dest, x1, val - 1, m1, m2, a1, a2, s1, s2);
                                x1 = val; // Continue processing with new range
                            }
                        }
                        else
                        {
                            if (x2 < val)
                            {
                                continue;
                            }
                            else if (x1 > val)
                            {
                                // whole range
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, s2);
                                return;
                            }
                            else
                            {
                                // The trickier case
                                AddSums(dest, val + 1, x2, m1, m2, a1, a2, s1, s2);
                                x2 = val; // Continue
                            }
                        }
                        break;
                    case 'm':
                        if (op == '<')
                        {
                            if (m1 >= val)
                            {
                                continue;
                            }
                            else if (m2 < val)
                            {
                                // whole range
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, s2);
                                return;
                            }
                            else
                            {
                                // The trickier case
                                AddSums(dest, x1, x2, m1, val - 1, a1, a2, s1, s2);
                                m1 = val; // Continue
                            }
                        }
                        else
                        {
                            if (m2 < val)
                            {
                                continue;
                            }
                            else if (m1 > val)
                            {
                                // whole range
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, s2);
                                return;
                            }
                            else
                            {
                                // The trickier case
                                AddSums(dest, x1, x2, val + 1, m2, a1, a2, s1, s2);
                                m2 = val; // Continue
                            }
                        }
                        break;
                    case 'a':
                        if (op == '<')
                        {
                            if (a1 >= val)
                            {
                                continue;
                            }
                            else if (a2 < val)
                            {
                                // whole range
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, s2);
                                return;
                            }
                            else
                            {
                                // The trickier case
                                AddSums(dest, x1, x2, m1, m2, a1, val - 1, s1, s2);
                                a1 = val; // Continue
                            }
                        }
                        else
                        {
                            if (a2 < val)
                            {
                                continue;
                            }
                            else if (a1 > val)
                            {
                                // whole range
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, s2);
                                return;
                            }
                            else
                            {
                                // The trickier case
                                AddSums(dest, x1, x2, m1, m2, val + 1, a2, s1, s2);
                                a2 = val; // Continue
                            }
                        }
                        break;
                    case 's':
                        if (op == '<')
                        {
                            if (s1 >= val)
                            {
                                continue;
                            }
                            else if (s2 < val)
                            {
                                // whole range
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, s2);
                                return;
                            }
                            else
                            {
                                // The trickier case
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, val - 1);
                                s1 = val; // Continue
                            }
                        }
                        else
                        {
                            if (s2 < val)
                            {
                                continue;
                            }
                            else if (s1 > val)
                            {
                                // whole range
                                AddSums(dest, x1, x2, m1, m2, a1, a2, s1, s2);
                                return;
                            }
                            else
                            {
                                // The trickier case
                                AddSums(dest, x1, x2, m1, m2, a1, a2, val + 1, s2);
                                s2 = val; // Continue
                            }
                        }
                        break;
                }
            }

            throw new Exception("Rulelessness!");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day19.txt");

            /*
            data = new[]
            {
                "px{a<2006:qkq,m>2090:A,rfg}",
                "pv{a>1716:R,A}",
                "lnx{m>1548:A,A}",
                "rfg{s<537:gd,x>2440:R,A}",
                "qs{s>3448:A,lnx}",
                "qkq{x<1416:A,crn}",
                "crn{x>2662:A,R}",
                "in{s<1351:px,qqz}",
                "qqz{s>2770:qs,m<1801:hdj,R}",
                "gd{a>3333:R,R}",
                "hdj{m>838:A,pv}",
                "",
                "{x=787,m=2655,a=1222,s=2876}",
                "{x=1679,m=44,a=2067,s=496}",
                "{x=2036,m=264,a=79,s=2244}",
                "{x=2461,m=1339,a=466,s=291}",
                "{x=2127,m=1623,a=2188,s=1013}",
            };
            */

            bool inRules = true;

            for (int i = 0; (i < data.Length) && inRules; i++)
            {
                if (inRules)
                {
                    if (data[i].Length == 0)
                    {
                        inRules = false;
                    }
                    else
                    {
                        var ss = data[i].Split('{').ToArray();
                        Rules.Add(ss[0], new Day19Rule(ss[0], ss[1]));
                    }
                }
            }

            // Do the stuff
            int x1 = 1;
            int m1 = 1;
            int a1 = 1;
            int s1 = 1;

            int x2 = 4000;
            int m2 = 4000;
            int a2 = 4000;
            int s2 = 4000;

            AddSums("in", x1, x2, m1, m2, a1, a2, s1, s2);


            Console.WriteLine($"Answer is  {Part2Sum}");
            Console.WriteLine($"(Training data should be 167409079868000)");
        }

    }
}

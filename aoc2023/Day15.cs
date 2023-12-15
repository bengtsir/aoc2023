using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Lens
    {
        public string Id { get; }
        public int Value { get; set; }
        public int Box { get; }

        public Lens(string id, int value)
        {
            Id = id;
            Value = value;
            Box = id.Aggregate(0, (a, b) => ((a + b) * 17) % 256);
        }
    }

    internal class Day15
    {
        internal long Hash(string s)
        {
            return s.Aggregate((long)0, (a, c) => ((a + (long)c) * 17) % 256);
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day15.txt");

            var values = data.Select(r => r.Split(',').ToArray()).ToArray();

            long sum = 0;

            sum = values[0].Select(v => Hash(v)).Sum();

            
            Console.WriteLine($"Answer is {sum}");
        }

        internal List<Lens>[] Boxes = new List<Lens>[256];

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day15.txt");

            var values = data.Select(r => r.Split(',').ToArray()).ToArray();

            for (int i = 0; i < 256; i++)
            {
                Boxes[i] = new List<Lens>();
            }

            foreach (var val in values[0])
            {
                if (val[val.Length - 1] == '-')
                {
                    var id = val.Substring(0, val.Length - 1);
                    var boxId = Hash(id);

                    Boxes[boxId].RemoveAll(lens => lens.Id == id);
                }
                else
                {
                    int focusVal = val[val.Length - 1] - '0';
                    if (val[val.Length - 2] != '=')
                    {
                        throw new Exception("klsdlfls");
                    }

                    var id = val.Substring(0, val.Length - 2);

                    var lens = new Lens(id, focusVal);

                    if (Boxes[lens.Box].Any(l => l.Id == lens.Id))
                    {
                        Boxes[lens.Box].First(l => l.Id == lens.Id).Value = lens.Value;
                    }
                    else
                    {
                        Boxes[lens.Box].Add(lens);
                    }
                }
            }

            long sum = 0;

            for (int box = 0; box < 256; box++)
            {
                for (int lens = 0; lens < Boxes[box].Count; lens++)
                {
                    sum += (box + 1) * (lens + 1) * Boxes[box][lens].Value;
                }
            }


            Console.WriteLine($"Answer is {sum}");
        }

    }
}

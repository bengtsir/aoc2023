using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day12
    {
        internal bool TestCombo(string inp, string toTest)
        {
            if (inp.Length != toTest.Length)
            {
                throw new ArgumentException("Mismatched lengths");
            }

            for (int i = 0; i < inp.Length; i++)
            {
                if (toTest[i] == '#' && inp[i] == '.')
                {
                    return false;
                }

                if (toTest[i] == '.' && inp[i] == '#')
                {
                    return false;
                }
            }
            return true;
        }

        internal IEnumerable<string> GenerateCombos(int spacesToDistribute, int[] streaks)
        {
            if (spacesToDistribute == 0)
            {
                yield return string.Join(".", streaks.Select(s => new string('#', s)));
            }
            else if (streaks.Length == 0)
            {
                yield return new string('.', spacesToDistribute);
            }
            else
            {
                for (int i = 0; i <= spacesToDistribute; i++)
                {
                    var ss = new string('.', i) + new String('#', streaks[0]);
                    if (streaks.Length > 1)
                    {
                        ss += ".";
                    }

                    foreach (var e in GenerateCombos(spacesToDistribute - i, streaks.Skip(1).ToArray()))
                    {
                        yield return ss + e;
                    }
                }
            }
        }

        internal int MissingSprings(string inp, int[] streaks)
        {
            var inpSprings = inp.Count(c => c == '#');
            var streakSum = streaks.Sum();

            return streakSum - inpSprings;
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day12.txt");

            var values = data.Select(r => r.Split(' ').ToArray()).ToArray();

            long combos = 0;

            foreach (var value in values)
            {
                var streaks = value[1].Split(',').Select(Int32.Parse).ToArray();

                int minLength = streaks.Sum() + streaks.Length - 1;

                if (value[0].Length < minLength)
                {
                    // Do nothing
                }
                else if (value[0].Length == minLength)
                {
                    combos++;
                }
                else
                {
                    int spacesToInsert = value[0].Length - minLength;

                    foreach (var t in GenerateCombos(spacesToInsert, streaks))
                    {
                        if (TestCombo(value[0], t))
                        {
                            combos++;
                        }
                    }
                }

            }


            Console.WriteLine($"Answer is {combos}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day12.txt");

            var values = data.Select(r => r.Split(' ').ToArray()).ToArray();

            long combos = 0;

            foreach (var value in values)
            {
                var inputString = value[0];

                for (int i = 0; i < 4; i++)
                {
                    inputString += "?" + value[0];
                }

                var streaks = value[1].Split(',').Select(Int32.Parse).ToArray();

                var ss = streaks.Select(v => v).ToList();
                for (int i = 0; i < 4; i++)
                {
                    ss.AddRange(streaks.Select(v => v).ToList());
                }

                streaks = ss.ToArray();


                int minLength = streaks.Sum() + streaks.Length - 1;

                if (inputString.Length < minLength)
                {
                    // Do nothing
                }
                else if (inputString.Length == minLength)
                {
                    combos++;
                }
                else
                {
                    int spacesToInsert = inputString.Length - minLength;

                    foreach (var t in GenerateCombos(spacesToInsert, streaks))
                    {
                        if (TestCombo(inputString, t))
                        {
                            combos++;
                        }
                    }
                }

            }


            Console.WriteLine($"Answer is {combos}");
        }

    }
}

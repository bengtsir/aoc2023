using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Card : IComparable
    {
        public string Hand { get; }
        public int Value { get; }
        public int Rank { get; }
        public bool IsPart2 { get; }

        public Card(string input, bool isPart2 = false)
        {
            var s = input.Split(' ').ToArray();
            Hand = s[0];
            Value = Int32.Parse(s[1]);
            IsPart2 = isPart2;
            if (IsPart2)
            {
                Rank = HandRank2();
            }
            else
            {
                Rank = HandRank();
            }
        }

        internal int CardValue(char c)
        {
            var vals = "23456789TJQKA";

            if (IsPart2)
            {
                vals = "J23456789TQKA";
            }

            return vals.IndexOf(c);
        }

        private int HandRank()
        {
            var vals = new int[13];
            for (int i = 0; i < Hand.Length; i++)
            {
                vals[CardValue(Hand[i])]++;
            }

            if (vals.Any(v => v == 5))
            {
                return 7;
            }

            if (vals.Any(v => v == 4))
            {
                return 6;
            }

            if (vals.Any(v => v == 3) && vals.Any(v => v == 2))
            {
                return 5;
            }

            if (vals.Any(v => v == 3))
            {
                return 4;
            }

            if (vals.Count(v => v == 2) == 2)
            {
                return 3;
            }

            if (vals.Any(v => v == 2))
            {
                return 2;
            }

            return 1;
        }

        private int HandRank2()
        {
            var vals = new int[13];
            for (int i = 0; i < Hand.Length; i++)
            {
                vals[CardValue(Hand[i])]++;
            }

            if (vals.Skip(1).Any(v => v + vals[0] == 5))
            {
                return 7;
            }

            if (vals.Skip(1).Any(v => v + vals[0] == 4))
            {
                return 6;
            }

            if ((vals.Skip(1).Any(v => v == 3) && vals.Skip(1).Any(v => v == 2)) ||
                (vals.Skip(1).Count(v => v == 2) == 2 && vals[0] == 1))
            {
                    return 5;
            }

            if (vals.Skip(1).Any(v => v + vals[0] == 3))
            {
                return 4;
            }

            if (vals.Count(v => v == 2) == 2)
            {
                return 3;
            }

            if (vals.Skip(1).Any(v => v + vals[0] == 2))
            {
                return 2;
            }

            return 1;
        }

        public int CompareTo(object obj)
        {
            var other = obj as Card;

            if (Rank > other.Rank)
            {
                return 1;
            }
            else if (Rank < other.Rank)
            {
                return -1;
            }

            for (int i = 0; i < 5; i++)
            {
                if (Hand[i] != other.Hand[i])
                {
                    return CardValue(Hand[i]) > CardValue(other.Hand[i]) ? 1 : -1;
                }
            }

            return 0;
        }
    }

    internal class Day7
    {
        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day7.txt");

            /*
            data = new[]
            {
                "32T3K 765",
                "T55J5 684",
                "KK677 28",
                "KTJJT 220",
                "QQQJA 483",
            };
            */

            var values = data.Select(r => new Card(r)).ToArray();


            Array.Sort(values);

            long sum = 0;

            for (int i = 0; i < values.Length; i++)
            {
                sum += (i + 1) * values[i].Value;
            }


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day7.txt");

            /*
            data = new[]
            {
                "32T3K 765",
                "T55J5 684",
                "KK677 28",
                "KTJJT 220",
                "QQQJA 483",
            };
            */

            var values = data.Select(r => new Card(r, true)).ToArray();


            Array.Sort(values);

            long sum = 0;

            for (int i = 0; i < values.Length; i++)
            {
                sum += (i + 1) * values[i].Value;
            }


            Console.WriteLine($"Answer is {sum}");
        }

    }
}

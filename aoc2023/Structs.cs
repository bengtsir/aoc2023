using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023.Structs
{
    internal struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(string pair)
        {
            var tuple = pair.Split(',').Select(Int32.Parse).ToArray();
            if (tuple.Length != 2)
            {
                throw new ArgumentException($"Invalid pair: {pair}", nameof(pair));
            }
            X = tuple[0];
            Y = tuple[1];
        }
    }

    internal class Segment
    {
        public long Start { get; set; }
        public long End { get; set; }
        public long Length => End - Start + 1;

        public Segment(long start, long end)
        {
            Start = start;
            End = end;
        }

        public bool Intersects(Segment other)
        {
            if (Start >= other.Start && Start <= other.End ||
                End >= other.Start && End <= other.End)
            {
                return true;
            }

            if (other.Start >= Start && other.Start <= End ||
                other.End >= Start && other.End <= End)
            {
                return true;
            }

            return false;
        }

        public void Shift(long offset)
        {
            Start += offset;
            End += offset;
        }
    }
}

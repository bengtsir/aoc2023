using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc2023.Structs;

namespace aoc2023
{
    static internal class ArrayMethods
    {
        public static IEnumerable<string> AddBorder(int count, char c, string[] data)
        {
            var rowLength = data[0].Length;

            var res = new List<string>();

            for (int i = 0; i < count; i++)
            {
                res.Add(new string(c, rowLength));
            }
            res.AddRange(data);
            for (var i = 0; i < count; i++)
            {
                res.Add(new string(c, rowLength));
            }

            return res.Select(r => new string(c, count) + r + new string(c, count));
        }

        // Splits the "orig" segment into one, two or three segments depending
        // on how it intersects with the "subRange" segment.
        //
        // Zero or one of the returned segments are entirely within the subrange,
        // the others are outside.
        public static List<Segment> SplitSegment(Segment orig, Segment subRange)
        {
            if (!orig.Intersects(subRange))
            {
                return new List<Segment>() { orig };
            }

            var l = new List<Segment>();

            if (orig.Start < subRange.Start)
            {
                l.Add(new Segment(orig.Start, subRange.Start - 1));

                if (orig.End > subRange.End)
                {
                    l.Add(new Segment(subRange.Start, subRange.End));
                    l.Add(new Segment(subRange.End, orig.End));
                }
                else
                {
                    l.Add(new Segment(subRange.Start, orig.End));
                }
            }
            else
            {
                if (orig.End > subRange.End)
                {
                    l.Add(new Segment(orig.Start, subRange.End));
                    l.Add(new Segment(subRange.End + 1, orig.End));
                }
                else
                {
                    l.Add(new Segment(orig.Start, orig.End));
                }
            }

            return l;
        }
    }
}

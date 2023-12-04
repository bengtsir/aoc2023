using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

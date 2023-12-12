using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    enum Direction
    {
        UP,
        RIGHT,
        LEFT,
        DOWN
    };

    internal class Day10
    {
        internal void NextIdx(ref Direction dir, char c, ref int curY, ref int curX)
        {
            switch (c)
            {
                case 'L':
                    if (dir == Direction.DOWN)
                    {
                        curX++;
                        dir = Direction.RIGHT;
                    }
                    else
                    {
                        curY--;
                        dir = Direction.UP;
                    }

                    break;

                case '|':
                    if (dir == Direction.DOWN)
                    {
                        curY++;
                        dir = Direction.DOWN;
                    }
                    else
                    {
                        curY--;
                        dir = Direction.UP;
                    }

                    break;

                case '-':
                    if (dir == Direction.RIGHT)
                    {
                        curX++;
                        dir = Direction.RIGHT;
                    }
                    else
                    {
                        curX--;
                        dir = Direction.LEFT;
                    }

                    break;

                case 'J':
                    if (dir == Direction.RIGHT)
                    {
                        curY--;
                        dir = Direction.UP;
                    }
                    else
                    {
                        curX--;
                        dir = Direction.LEFT;
                    }

                    break;

                case '7':
                    if (dir == Direction.RIGHT)
                    {
                        curY++;
                        dir = Direction.DOWN;
                    }
                    else
                    {
                        curX--;
                        dir = Direction.LEFT;
                    }

                    break;

                case 'F':
                    if (dir == Direction.UP)
                    {
                        curX++;
                        dir = Direction.RIGHT;
                    }
                    else
                    {
                        curY++;
                        dir = Direction.DOWN;
                    }

                    break;
            }
        }

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day10.txt");

            ArrayMethods.AddBorder(1, '.', data);

            var values = ArrayMethods.AddBorder(1, '.', data).ToArray();

            int startx = 0;
            int starty = 0;

            int curX = 0;
            int curY = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Contains('S'))
                {
                    starty = i;
                    startx = values[i].IndexOf('S');
                }
            }

            curX = startx;
            curY = starty;

            Direction dir = Direction.DOWN;

            int steps = 0;

            if ("|JL".Contains(values[starty+1][startx]))
            {
                dir = Direction.DOWN;
                curY++;
            }
            else if ("-J7".Contains(values[starty][startx + 1]))
            {
                dir = Direction.RIGHT;
                curX++;
            }
            else if ("|7F".Contains(values[starty - 1][startx]))
            {
                dir = Direction.UP;
                curY--;
            }

            steps++;

            while ((curX != startx) || (curY != starty))
            {
                NextIdx(ref dir, values[curY][curX], ref curY, ref curX);

                steps++;
            }

            Console.WriteLine($"Answer is {steps/2}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day10.txt");

            ArrayMethods.AddBorder(1, '.', data);

            var values = ArrayMethods.AddBorder(1, '.', data).ToArray();
            var marks = values.Select(s => s.Select(c => ' ').ToArray()).ToArray();

            int startx = 0;
            int starty = 0;

            int curX = 0;
            int curY = 0;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Contains('S'))
                {
                    starty = i;
                    startx = values[i].IndexOf('S');
                }
            }

            curX = startx;
            curY = starty;

            marks[curY][curX] = values[curY][curX];

            Direction dir = Direction.DOWN;

            int steps = 0;

            if ("|JL".Contains(values[starty + 1][startx]))
            {
                dir = Direction.DOWN;
                curY++;
            }
            else if ("-J7".Contains(values[starty][startx + 1]))
            {
                dir = Direction.RIGHT;
                curX++;
            }
            else if ("|7F".Contains(values[starty - 1][startx]))
            {
                dir = Direction.UP;
                curY--;
            }

            steps++;

            while ((curX != startx) || (curY != starty))
            {
                marks[curY][curX] = values[curY][curX];

                NextIdx(ref dir, values[curY][curX], ref curY, ref curX);

                steps++;
            }

            // Everything inside the loop is on the left hand side as we travel (for this problem)

            curX = startx;
            curY = starty;

            if ("|JL".Contains(values[starty + 1][startx]))
            {
                dir = Direction.DOWN;
                curY++;
            }
            else if ("-J7".Contains(values[starty][startx + 1]))
            {
                dir = Direction.RIGHT;
                curX++;
            }
            else if ("|7F".Contains(values[starty - 1][startx]))
            {
                dir = Direction.UP;
                curY--;
            }

            while ((curX != startx) || (curY != starty))
            {
                if (values[curY][curX] == '|')
                {
                    if (dir == Direction.UP)
                    {
                        if (marks[curY][curX - 1] == ' ')
                        {
                            marks[curY][curX - 1] = '+';
                        }
                    }
                    else
                    {
                        if (marks[curY][curX + 1] == ' ')
                        {
                            marks[curY][curX + 1] = '+';
                        }
                    }

                }
                else if (values[curY][curX] == '-')
                {
                    if (dir == Direction.LEFT)
                    {
                        if (marks[curY + 1][curX] == ' ')
                        {
                            marks[curY + 1][curX] = '+';
                        }
                    }
                    else
                    {
                        if (marks[curY - 1][curX] == ' ')
                        {
                            marks[curY - 1][curX] = '+';
                        }
                    }
                }
                else if (values[curY][curX] == 'J')
                {
                    if (dir == Direction.RIGHT)
                    {
                        if (marks[curY - 1][curX - 1] == ' ')
                        {
                            marks[curY - 1][curX - 1] = '+';
                        }
                    }
                    else
                    {
                        if (marks[curY + 1][curX] == ' ')
                        {
                            marks[curY + 1][curX] = '+';
                        }   

                        if (marks[curY + 1][curX + 1] == ' ')
                        {
                            marks[curY + 1][curX + 1] = '+';
                        }
                            
                        if (marks[curY][curX + 1] == ' ')
                        {
                            marks[curY][curX + 1] = '+';
                        }
                    }
                }
                else if (values[curY][curX] == 'L')
                {
                    if (dir == Direction.LEFT)
                    {
                        if (marks[curY + 1][curX] == ' ')
                        {
                            marks[curY + 1][curX] = '+';
                        }

                        if (marks[curY + 1][curX - 1] == ' ')
                        {
                            marks[curY + 1][curX - 1] = '+';
                        }

                        if (marks[curY][curX - 1] == ' ')
                        {
                            marks[curY][curX - 1] = '+';
                        }
                    }
                    else
                    {
                        if (marks[curY - 1][curX + 1] == ' ')
                        {
                            marks[curY - 1][curX + 1] = '+';
                        }
                    }
                }
                else if (values[curY][curX] == '7')
                {
                    if (dir == Direction.RIGHT)
                    {
                        if (marks[curY - 1][curX] == ' ')
                        {
                            marks[curY - 1][curX] = '+';
                        }

                        if (marks[curY - 1][curX + 1] == ' ')
                        {
                            marks[curY - 1][curX + 1] = '+';
                        }

                        if (marks[curY][curX + 1] == ' ')
                        {
                            marks[curY][curX + 1] = '+';
                        }
                    }
                    else
                    {
                        if (marks[curY + 1][curX - 1] == ' ')
                        {
                            marks[curY + 1][curX - 1] = '+';
                        }
                    }
                }
                else if (values[curY][curX] == 'F')
                {
                    if (dir == Direction.UP)
                    {
                        if (marks[curY - 1][curX] == ' ')
                        {
                            marks[curY - 1][curX] = '+';
                        }

                        if (marks[curY - 1][curX - 1] == ' ')
                        {
                            marks[curY - 1][curX - 1] = '+';
                        }

                        if (marks[curY][curX - 1] == ' ')
                        {
                            marks[curY][curX - 1] = '+';
                        }
                    }
                    else
                    {
                        if (marks[curY + 1][curX + 1] == ' ')
                        {
                            marks[curY + 1][curX + 1] = '+';
                        }
                    }
                }

                NextIdx(ref dir, values[curY][curX], ref curY, ref curX);
            }

            
            for (int i = 0; i < marks.Length; i++)
            {
                Console.WriteLine(new string(marks[i]));
            }
            

            bool changed = true;

            while (changed)
            {
                changed = false;
                for (int i = 1; i < marks.Length - 1; i++)
                {
                    for (int j = 1; j < marks[i].Length - 1; j++)
                    {
                        var c = marks[i][j];
                        if (c == '+')
                        {
                            if (marks[i+1][j] == ' ')
                            {
                                marks[i + 1][j] = c;
                                changed = true;
                            }
                            if (marks[i - 1][j] == ' ')
                            {
                                marks[i - 1][j] = c;
                                changed = true;
                            }
                            if (marks[i][j + 1] == ' ')
                            {
                                marks[i][j + 1] = c;
                                changed = true;
                            }
                            if (marks[i][j - 1] == ' ')
                            {
                                marks[i][j - 1] = c;
                                changed = true;
                            }
                        }
                    }
                }
            }

            var replacements = new List<char[]>
            {
                new[] { '-', '\u2500' },
                new[] { '|', '\u2502' },
                new[] { 'L', '\u2514' },
                new[] { 'J', '\u2518' },
                new[] { 'F', '\u250c' },
                new[] { '7', '\u2510' }
            };

            for (int i = 0; i < marks.Length; i++)
            {
                var line = new string(marks[i]);
                foreach (var repl in replacements )
                {
                    line = line.Replace(repl[0], repl[1]);
                }
                Console.WriteLine(line);
            }

            int sum = marks.Sum(s => s.Count(c => c == '+'));


            Console.WriteLine($"Answer is {sum}");
        }

    }
}

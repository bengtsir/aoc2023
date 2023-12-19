using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aoc2023.Structs;

namespace aoc2023
{
    internal class LagoonSegment
    {
        public Point Start;
        public Point End;

        public Direction Dir { get; }

        public int Width => Math.Abs(End.X - Start.X);

        public int Height => Math.Abs(End.Y - Start.Y);

        public LagoonSegment(Point start, Point end)
        {
            Start = start;
            End = end;

            if (End.Y < Start.Y)
            {
                Dir = Direction.UP;
            }
            else if (End.Y > Start.Y)
            {
                Dir = Direction.DOWN;
            }
            else if (End.X < Start.X)
            {
                Dir = Direction.LEFT;
            }
            else
            {
                Dir = Direction.RIGHT;
            }
        }

        public override string ToString()
        {
            return $"({Start.X}, {Start.Y}) - ({End.X}, {End.Y}), W: {Width} H: {Height} Dir: {Dir.ToString()}";
        }
    }

    internal class Day18
    {
        internal int[][] Board;
        internal const int BoardSize = 1000;

        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day18.txt");

            /*
            data = new[]
            {
                "R 6 (#70c710)",
                "D 5 (#0dc571)",
                "L 2 (#5713f0)",
                "D 2 (#d2c081)",
                "R 2 (#59c680)",
                "D 2 (#411b91)",
                "L 5 (#8ceee2)",
                "U 2 (#caa173)",
                "L 1 (#1b58a2)",
                "U 2 (#caa171)",
                "R 2 (#7807d2)",
                "U 3 (#a77fa3)",
                "L 2 (#015232)",
                "U 2 (#7a21e3)",
            };
            */

            var values = data.Select(r => r.Split(' ')).ToArray();

            Board = new int[BoardSize][];
            for (int i = 0; i < BoardSize; i++)
            {
                Board[i] = new int[BoardSize];
            }

            int CurrentX = BoardSize / 2;
            int CurrentY = BoardSize / 2;

            Board[CurrentY][CurrentX] = 1;

            foreach (var tuple in values)
            {
                int xofs = 0;
                int yofs = 0;

                switch (tuple[0][0])
                {
                    case 'U':
                        yofs = -1;
                        break;
                    case 'D':
                        yofs = 1;
                        break;
                    case 'L':
                        xofs = -1;
                        break;
                    case 'R':
                        xofs = 1;
                        break;
                }

                for (int i = 0; i < Convert.ToInt32(tuple[1]); i++)
                {
                    CurrentX += xofs;
                    CurrentY += yofs;
                    if (CurrentX < 0 || CurrentX >= BoardSize || CurrentY < 0 || CurrentY >= BoardSize)
                    {
                        throw new Exception("Board size too small");
                    }
                    Board[CurrentY][CurrentX] = 1;
                }
            }

            // Flood fill from edges

            for (var i = 0; i < BoardSize; i++)
            {
                if (Board[0][i] == 0)
                {
                    Board[0][i] = 2;
                }
                if (Board[BoardSize - 1][i] == 0)
                {
                    Board[BoardSize - 1][i] = 2;
                }
                if (Board[i][0] == 0)
                {
                    Board[i][0] = 2;
                }
                if (Board[i][BoardSize - 1] == 0)
                {
                    Board[i][BoardSize - 1] = 2;
                }
            }

            var changed = true;

            while (changed)
            {
                changed = false;

                for (var r = 1; r < BoardSize - 1; r++)
                {
                    for (var k = 1; k < BoardSize - 1; k++)
                    {
                        if (Board[r][k] == 0)
                        {
                            if (Board[r - 1][k] == 2 ||
                                Board[r + 1][k] == 2 ||
                                Board[r][k - 1] == 2 ||
                                Board[r][k + 1] == 2)
                            {
                                Board[r][k] = 2;
                                changed = true;
                            }
                        }
                    }
                }
            }

            var sum = Board.Select(vv => vv.Count(v => v != 2)).Sum();


            Console.WriteLine($"Answer is {sum}");
        }

        internal List<LagoonSegment> segments = new List<LagoonSegment>();

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day18.txt");

            /*
            data = new[]
            {
                "R 6 (#70c710)",
                "D 5 (#0dc571)",
                "L 2 (#5713f0)",
                "D 2 (#d2c081)",
                "R 2 (#59c680)",
                "D 2 (#411b91)",
                "L 5 (#8ceee2)",
                "U 2 (#caa173)",
                "L 1 (#1b58a2)",
                "U 2 (#caa171)",
                "R 2 (#7807d2)",
                "U 3 (#a77fa3)",
                "L 2 (#015232)",
                "U 2 (#7a21e3)",
            };
            */

            /*
            data = new[]
            {
                "R 6 (#000060)",
                "D 5 (#000051)",
                "L 2 (#000022)",
                "D 2 (#000021)",
                "R 2 (#000020)",
                "D 2 (#000021)",
                "L 5 (#000052)",
                "U 2 (#000023)",
                "L 1 (#000012)",
                "U 2 (#000023)",
                "R 2 (#000020)",
                "U 3 (#000033)",
                "L 2 (#000022)",
                "U 2 (#000023)",
            };
            */


            var values = data.Select(r => r.Split(' ')).ToArray();

            int CurrentX = 0;
            int CurrentY = 0;

            foreach (var tuple in values)
            {
                int distance = Convert.ToInt32(tuple[2].Substring(2, 5), 16);
                char dir = tuple[2][7];
                Point from = new Point(CurrentX, CurrentY);
                switch (dir)
                {
                    case '0':
                        CurrentX += distance;
                        break;
                    case '1':
                        CurrentY += distance;
                        break;
                    case '2':
                        CurrentX -= distance;
                        break;
                    case '3':
                        CurrentY -= distance;
                        break;
                }
                segments.Add(new LagoonSegment(from, new Point(CurrentX, CurrentY)));
            }


            /*
            foreach (var s in segments)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine();
            */

            long area = 0;

            while (segments.Count > 4)
            {
                var seg = -1;
                Direction dir = Direction.UP;

                // Find a "bump" and remove it
                for (int i = 0; i < segments.Count - 2; i++)
                {
                    // Topward bump
                    if (segments[i].Dir == Direction.UP && segments[i + 1].Dir == Direction.RIGHT &&
                        segments[i + 2].Dir == Direction.DOWN)
                    {
                        // Check for snake backs
                        int maxY = Math.Min(segments[i].Start.Y, segments[i + 2].End.Y);
                        if (segments.Skip(i+2).All(s => s.End.X >= segments[i+2].End.X || s.End.X < segments[i].Start.X || s.End.Y >= maxY))
                        {
                            seg = i;
                            dir = Direction.UP;
                            break;
                        }
                    }
                    // Rightward bump
                    if (segments[i].Dir == Direction.RIGHT && segments[i + 1].Dir == Direction.DOWN &&
                        segments[i + 2].Dir == Direction.LEFT)
                    {
                        // Check for snake backs
                        int minX = Math.Max(segments[i].Start.X, segments[i + 2].End.X);
                        if (segments.Skip(i + 2).All(s => s.End.Y <= segments[i].Start.Y || s.End.Y >= segments[i+2].End.Y || s.End.X <= minX))
                        {
                            seg = i;
                            dir = Direction.RIGHT;
                            break;
                        }
                    }
                    // Downward bump
                    if (segments[i].Dir == Direction.DOWN && segments[i + 1].Dir == Direction.LEFT &&
                        segments[i + 2].Dir == Direction.UP)
                    {
                        // Check for snake backs
                        int minY = Math.Max(segments[i].Start.Y, segments[i + 2].End.Y);
                        if (segments.Skip(i + 2).All(s => s.End.X >= segments[i].Start.X || s.End.X <= segments[i+2].End.X || s.End.Y <= minY))
                        {
                            seg = i;
                            dir = Direction.DOWN;
                            break;
                        }
                    }
                    // Rightward bump
                    if (segments[i].Dir == Direction.LEFT && segments[i + 1].Dir == Direction.UP &&
                        segments[i + 2].Dir == Direction.RIGHT)
                    {
                        // Check for snake backs
                        int maxX = Math.Min(segments[i].Start.X, segments[i + 2].End.X);
                        if (segments.Skip(i + 2).All(s => s.End.Y >= segments[i].Start.Y || s.End.Y <= segments[i+2].End.Y || s.End.X >= maxX))
                        {
                            seg = i;
                            dir = Direction.LEFT;
                            break;
                        }
                    }
                }

                if (seg < 0)
                {
                    throw new Exception("klsdfl");
                }

                Console.WriteLine($"Removing seg {seg}");

                if (dir == Direction.UP)
                {
                    long xSize = segments[seg + 1].Width;
                    long ySize = Math.Min(segments[seg].Height, segments[seg + 2].Height);
                    var add = (xSize + 1) * ySize;

                    Console.WriteLine($"Adding {add}");

                    area += add;

                    int newY = segments[seg].End.Y + (int)ySize;

                    segments[seg].End.Y = newY;
                    segments[seg + 1].Start.Y = newY;
                    segments[seg + 1].End.Y = newY;
                    segments[seg + 2].Start.Y = newY;
                }
                else if (dir == Direction.RIGHT)
                {
                    long xSize = Math.Min(segments[seg].Width, segments[seg + 2].Width);
                    long ySize = segments[seg + 1].Height;
                    var add = xSize * (ySize + 1);

                    Console.WriteLine($"Adding {add}");

                    area += add;

                    int newX = segments[seg].End.X - (int)xSize;

                    segments[seg].End.X = newX;
                    segments[seg + 1].Start.X = newX;
                    segments[seg + 1].End.X = newX;
                    segments[seg + 2].Start.X = newX;
                }
                else if (dir == Direction.DOWN)
                {
                    long xSize = segments[seg + 1].Width;
                    long ySize = Math.Min(segments[seg].Height, segments[seg + 2].Height);
                    var add = (xSize + 1) * ySize;

                    Console.WriteLine($"Adding {add}");

                    area += add;

                    int newY = segments[seg].End.Y - (int)ySize;

                    segments[seg].End.Y = newY;
                    segments[seg + 1].Start.Y = newY;
                    segments[seg + 1].End.Y = newY;
                    segments[seg + 2].Start.Y = newY;
                }
                else if (dir == Direction.LEFT)
                {
                    long xSize = Math.Min(segments[seg].Width, segments[seg + 2].Width);
                    long ySize = segments[seg + 1].Height;
                    var add = xSize * (ySize + 1);

                    Console.WriteLine($"Adding {add}");

                    area += add;

                    int newX = segments[seg].End.X + (int)xSize;

                    segments[seg].End.X = newX;
                    segments[seg + 1].Start.X = newX;
                    segments[seg + 1].End.X = newX;
                    segments[seg + 2].Start.X = newX;
                }

                /*
                foreach (var s in segments)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine();
                */

                /*
                Console.WriteLine("Removing zero longth entries");
                */

                segments.RemoveAll(s => (s.Width + s.Height) == 0);

                /*
                foreach (var s in segments)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine();
                */

                bool consolidated = true;
                while (consolidated)
                {
                    consolidated = false;

                    for (int i = 0; (!consolidated) && (i < segments.Count - 1); i++)
                    {
                        if (segments[i].Dir == segments[i+1].Dir)
                        {
                            Console.WriteLine($"Concatenating segs {i} and {i + 1}");
                            if ((segments[i].Dir == Direction.RIGHT) || (segments[i].Dir == Direction.LEFT))
                            {
                                segments[i].End.X = segments[i + 1].End.X;
                                segments[i + 1].Start.X = segments[i + 1].End.X;
                            }
                            else if ((segments[i].Dir == Direction.DOWN) || (segments[i].Dir == Direction.UP))
                            {
                                segments[i].End.Y = segments[i + 1].End.Y;
                                segments[i + 1].Start.Y = segments[i + 1].End.Y;
                            }
                            consolidated = true;
                        }
                        else if ((segments[i].Dir == Direction.UP) && (segments[i + 1].Dir == Direction.DOWN))
                        {
                            if (segments[i].Start.Y < segments[i + 1].End.Y)
                            {
                                int add = segments[i].Height;
                                Console.WriteLine($"Small strip: Adding {add}");
                                area += add;

                                segments[i].End.Y = segments[i].Start.Y;
                                segments[i + 1].Start.Y = segments[i].Start.Y;
                            }
                            else
                            {
                                int add = segments[i + 1].Height;
                                Console.WriteLine($"Small strip: Adding {add}");
                                area += add;

                                segments[i].End.Y = segments[i + 1].End.Y;
                                segments[i + 1].Start.Y = segments[i + 1].End.Y;
                            }
                            consolidated = true;
                        }
                        else if ((segments[i].Dir == Direction.DOWN) && (segments[i + 1].Dir == Direction.UP))
                        {
                            if (segments[i].Start.Y < segments[i + 1].End.Y)
                            {
                                int add = segments[i + 1].Height;
                                Console.WriteLine($"Small strip: Adding {add}");
                                area += add;

                                segments[i].End.Y = segments[i+1].End.Y;
                                segments[i + 1].Start.Y = segments[i + 1].End.Y;
                            }
                            else
                            {
                                int add = segments[i].Height;
                                Console.WriteLine($"Small strip: Adding {add}");
                                area += add;

                                segments[i].End.Y = segments[i].Start.Y;
                                segments[i + 1].Start.Y = segments[i].End.Y;
                            }
                            consolidated = true;
                        }
                        else if ((segments[i].Dir == Direction.LEFT) && (segments[i + 1].Dir == Direction.RIGHT))
                        {
                            if (segments[i].Start.X < segments[i + 1].End.X)
                            {
                                int add = segments[i].Width;
                                Console.WriteLine($"Small strip: Adding {add}");
                                area += add;

                                segments[i].End.X = segments[i].Start.X;
                                segments[i + 1].Start.X = segments[i].Start.X;
                            }
                            else
                            {
                                int add = segments[i + 1].Width;
                                Console.WriteLine($"Small strip: Adding {add}");
                                area += add;

                                segments[i].End.X = segments[i + 1].End.X;
                                segments[i + 1].Start.X = segments[i + 1].End.X;
                            }
                            consolidated = true;
                        }
                        else if ((segments[i].Dir == Direction.RIGHT) && (segments[i + 1].Dir == Direction.LEFT))
                        {
                            if (segments[i].Start.X < segments[i + 1].End.X)
                            {
                                int add = segments[i + 1].Width;
                                Console.WriteLine($"Small strip: Adding {add}");
                                area += add;

                                segments[i].End.X = segments[i + 1].End.X;
                                segments[i + 1].Start.X = segments[i + 1].End.X;
                            }
                            else
                            {
                                int add = segments[i].Width;
                                Console.WriteLine($"Small strip: Adding {add}");
                                area += add;

                                segments[i].End.X = segments[i].Start.X;
                                segments[i + 1].Start.X = segments[i].End.X;
                            }
                            consolidated = true;
                        }
                    }

                    if (consolidated)
                    {
                        segments.RemoveAll(s => (s.Width + s.Height) == 0);

                        /*
                        foreach (var s in segments)
                        {
                            Console.WriteLine(s);
                        }

                        Console.WriteLine();
                        */
                    }
                }
            }

            // Check direction of final square
            if (segments[0].Dir == Direction.LEFT || segments[0].Dir == Direction.RIGHT)
            {
                area += (long)(segments[0].Width + 1) * (segments[1].Height + 1);
            }
            else
            {
                area += (long)(segments[0].Height + 1) * (segments[1].Width + 1);
            }

            long trainResult = 952408144115;

            Console.WriteLine($"Answer is {area}");
            Console.WriteLine($"(training data should be {trainResult}, diff = {area - trainResult})");
        }

    }
}

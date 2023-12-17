using aoc2023.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class DistanceInfo
    {
        public Direction Dir { get; }
        public int StepsLeft { get; }
        public bool Visited { get; set; } = false;
        public int Value { get; set; }

        public DistanceInfo(Direction from, int stepsLeft)
        {
            Dir = from;
            StepsLeft = stepsLeft;
        }

    }

    internal class Day17
    {
        internal int[][] Board;
        internal int[][][] Distances;
        internal bool[][][] Visited;
        internal int Width = 0;
        internal int Height = 0;

        internal int Inf = 99999999;

        internal const int LEFTRIGHT = 0;
        internal const int UPDOWN = 1;

        void Traverse(Point point, int maindir)
        {
            int pathVal;

            for (int i = 1; i <= 3; i++)
            {
                if (maindir == UPDOWN)
                {
                    // Down
                    if (point.Y - i >= 0)
                    {
                        pathVal = Distances[point.Y][point.X][UPDOWN];

                        if (i == 3)
                        {
                            pathVal += Board[point.Y - 3][point.X];
                        }
                        if (i >= 2)
                        {
                            pathVal += Board[point.Y - 2][point.X];
                        }
                        pathVal += Board[point.Y - 1][point.X];

                        if (!Visited[point.Y - i][point.X][LEFTRIGHT])
                        {
                            if (pathVal < Distances[point.Y - i][point.X][LEFTRIGHT])
                            {
                                Distances[point.Y - i][point.X][LEFTRIGHT] = pathVal;
                            }
                        }
                    }

                    // Up
                    if (point.Y + i < Height)
                    {
                        pathVal = Distances[point.Y][point.X][UPDOWN];

                        if (i == 3)
                        {
                            pathVal += Board[point.Y + 3][point.X];
                        }
                        if (i >= 2)
                        {
                            pathVal += Board[point.Y + 2][point.X];
                        }
                        pathVal += Board[point.Y + 1][point.X];

                        if (!Visited[point.Y + i][point.X][LEFTRIGHT])
                        {
                            if (pathVal < Distances[point.Y + i][point.X][LEFTRIGHT])
                            {
                                Distances[point.Y + i][point.X][LEFTRIGHT] = pathVal;
                            }
                        }
                    }

                }
                else
                {
                    // Left
                    if (point.X - i >= 0)
                    {
                        pathVal = Distances[point.Y][point.X][LEFTRIGHT];

                        if (i == 3)
                        {
                            pathVal += Board[point.Y][point.X - 3];
                        }
                        if (i >= 2)
                        {
                            pathVal += Board[point.Y][point.X - 2];
                        }
                        pathVal += Board[point.Y][point.X - 1];

                        if (!Visited[point.Y][point.X - i][UPDOWN])
                        {
                            if (pathVal < Distances[point.Y][point.X - i][UPDOWN])
                            {
                                Distances[point.Y][point.X - i][UPDOWN] = pathVal;
                            }
                        }
                    }

                    // Right
                    if (point.X + i < Width)
                    {
                        pathVal = Distances[point.Y][point.X][LEFTRIGHT];

                        if (i == 3)
                        {
                            pathVal += Board[point.Y][point.X + 3];
                        }
                        if (i >= 2)
                        {
                            pathVal += Board[point.Y][point.X + 2];
                        }
                        pathVal += Board[point.Y][point.X + 1];

                        if (!Visited[point.Y][point.X + i][UPDOWN])
                        {
                            if (pathVal < Distances[point.Y][point.X + i][UPDOWN])
                            {
                                Distances[point.Y][point.X + i][UPDOWN] = pathVal;
                            }
                        }
                    }
                }
            }

            Visited[point.Y][point.X][maindir] = true;
        }

        void Traverse2(Point point, int maindir)
        {
            int pathVal;

            for (int i = 4; i <= 10; i++)
            {
                if (maindir == UPDOWN)
                {
                    // Down
                    if (point.Y - i >= 0)
                    {
                        pathVal = Distances[point.Y][point.X][UPDOWN];

                        for (int pp = 1; pp <= i; pp++)
                        {
                            pathVal += Board[point.Y - pp][point.X];
                        }
                        
                        if (!Visited[point.Y - i][point.X][LEFTRIGHT])
                        {
                            if (pathVal < Distances[point.Y - i][point.X][LEFTRIGHT])
                            {
                                Distances[point.Y - i][point.X][LEFTRIGHT] = pathVal;
                            }
                        }
                    }

                    // Up
                    if (point.Y + i < Height)
                    {
                        pathVal = Distances[point.Y][point.X][UPDOWN];

                        for (int pp = 1; pp <= i; pp++)
                        {
                            pathVal += Board[point.Y + pp][point.X];
                        }

                        if (!Visited[point.Y + i][point.X][LEFTRIGHT])
                        {
                            if (pathVal < Distances[point.Y + i][point.X][LEFTRIGHT])
                            {
                                Distances[point.Y + i][point.X][LEFTRIGHT] = pathVal;
                            }
                        }
                    }

                }
                else
                {
                    // Left
                    if (point.X - i >= 0)
                    {
                        pathVal = Distances[point.Y][point.X][LEFTRIGHT];

                        for (int pp = 1; pp <= i; pp++)
                        {
                            pathVal += Board[point.Y][point.X - pp];
                        }

                        if (!Visited[point.Y][point.X - i][UPDOWN])
                        {
                            if (pathVal < Distances[point.Y][point.X - i][UPDOWN])
                            {
                                Distances[point.Y][point.X - i][UPDOWN] = pathVal;
                            }
                        }
                    }

                    // Right
                    if (point.X + i < Width)
                    {
                        pathVal = Distances[point.Y][point.X][LEFTRIGHT];

                        for (int pp = 1; pp <= i; pp++)
                        {
                            pathVal += Board[point.Y][point.X + pp];
                        }

                        if (!Visited[point.Y][point.X + i][UPDOWN])
                        {
                            if (pathVal < Distances[point.Y][point.X + i][UPDOWN])
                            {
                                Distances[point.Y][point.X + i][UPDOWN] = pathVal;
                            }
                        }
                    }
                }
            }

            Visited[point.Y][point.X][maindir] = true;
        }

        void Dijkstra(bool part2 = false)
        {
            while (true)
            {
                var currVal = Inf;

                Point selected = new Point()
                {
                    X = -1,
                    Y = -1
                };
                int selectedDir = 0;

                for (int r = 0; r < Height; r++)
                {
                    for (int k = 0; k < Width; k++)
                    {
                        for (int dir = 0; dir < 2; dir++)
                        {
                            if (!Visited[r][k][dir] && Distances[r][k][dir] < currVal)
                            {
                                selected = new Point()
                                {
                                    X = k,
                                    Y = r
                                };
                                selectedDir = dir;
                                currVal = Distances[r][k][dir];
                            }
                        }
                    }
                }

                if (selected.X < 0)
                {
                    break;
                }

                if (part2)
                {
                    Traverse2(selected, selectedDir);
                }
                else
                {
                    Traverse(selected, selectedDir);
                }
            }
        }


        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day17.txt");

            /*
            data = new[]
            {
                "2413432311323",
                "3215453535623",
                "3255245654254",
                "3446585845452",
                "4546657867536",
                "1438598798454",
                "4457876987766",
                "3637877979653",
                "4654967986887",
                "4564679986453",
                "1224686865563",
                "2546548887735",
                "4322674655533",
            };
            */

            Board = data.Select(r => r.Select(c => c - '0').ToArray()).ToArray();
            Distances = Board.Select(r => r.Select(v => new int[]{Inf, Inf}).ToArray()).ToArray();
            Visited = Board.Select(r => r.Select(c => new[]{false, false}).ToArray()).ToArray();

            Width = Board[0].Length;
            Height = Board.Length;

            // Good ol' Dijkstra

            // Setup end
            int lastVal = Board[Height - 1][Width - 1];
            for (int i = 0; i < 2; i++)
            {
                Distances[Height - 1][Width - 1][i] = Board[Height - 1][Width - 1];
            }
            //Visited[Height - 1][Width - 1] = true;

            Dijkstra(false);

            foreach (var line in Distances)
            {
                Console.WriteLine(string.Join(" ", line.Select(d => $"{d.Min(),3:D}")));
            }

            Console.WriteLine($"Answer is {Distances[0][0].Min() - Board[0][0]}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day17.txt");

            /*
            data = new[]
            {
                "2413432311323",
                "3215453535623",
                "3255245654254",
                "3446585845452",
                "4546657867536",
                "1438598798454",
                "4457876987766",
                "3637877979653",
                "4654967986887",
                "4564679986453",
                "1224686865563",
                "2546548887735",
                "4322674655533",
            };
            */

            Board = data.Select(r => r.Select(c => c - '0').ToArray()).ToArray();
            Distances = Board.Select(r => r.Select(v => new int[] { Inf, Inf }).ToArray()).ToArray();
            Visited = Board.Select(r => r.Select(c => new[] { false, false }).ToArray()).ToArray();

            Width = Board[0].Length;
            Height = Board.Length;

            // Good ol' Dijkstra

            // Setup end
            int lastVal = Board[Height - 1][Width - 1];
            for (int i = 0; i < 2; i++)
            {
                Distances[Height - 1][Width - 1][i] = Board[Height - 1][Width - 1];
            }
            //Visited[Height - 1][Width - 1] = true;

            Dijkstra(true);

            foreach (var line in Distances)
            {
                Console.WriteLine(string.Join(" ", line.Select(d => $"{d.Min(),3:D}")));
            }

            Console.WriteLine($"Answer is {Distances[0][0].Min() - Board[0][0]}");
        }

    }
}

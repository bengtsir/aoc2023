using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc2023
{
    internal class Day16
    {
        internal char[][] Board;
        internal int[][] Mark;

        private const int RIGHT = 1;
        private const int LEFT = 2;
        private const int UP = 4;
        private const int DOWN = 8;

        void Beam(int x, int y, int direction)
        {
            while (true)
            {
                if ((Mark[y][x] & direction) != 0)
                {
                    break;
                }

                Mark[y][x] |= direction;

                var newdir = direction;

                switch (Board[y][x])
                {
                    case '#':
                        break; // wall
                    
                    case '.':
                        break; // Do nothing
                    
                    case '\\':
                        switch (direction)
                        {
                            case RIGHT:
                                newdir = DOWN;
                                break;
                            case LEFT:
                                newdir = UP;
                                break;
                            case UP:
                                newdir = LEFT;
                                break;
                            case DOWN:
                                newdir = RIGHT;
                                break;
                        }

                        direction = newdir;
                        break;

                    case '/':
                        switch (direction)
                        {
                            case RIGHT:
                                newdir = UP;
                                break;
                            case LEFT:
                                newdir = DOWN;
                                break;
                            case UP:
                                newdir = RIGHT;
                                break;
                            case DOWN:
                                newdir = LEFT;
                                break;
                        }

                        direction = newdir;
                        break;

                    case '|':
                        switch (direction)
                        {
                            case RIGHT:
                            case LEFT:
                                newdir = UP;
                                Beam(x, y, DOWN);
                                break;
                            case UP:
                            case DOWN:
                                break;
                        }

                        direction = newdir;
                        break;

                    case '-':
                        switch (direction)
                        {
                            case UP:
                            case DOWN:
                                newdir = LEFT;
                                Beam(x, y, RIGHT);
                                break;
                            case RIGHT:
                            case LEFT:
                                break;
                        }

                        direction = newdir;
                        break;

                }

                switch (direction)
                {
                    case RIGHT:
                        x++;
                        break;
                    case LEFT:
                        x--;
                        break;
                    case UP:
                        y--;
                        break;
                    case DOWN:
                        y++;
                        break;
                }

            }

        }


        public void Part1()
        {
            var data = File.ReadAllLines(@"data\day16.txt");

            /*
            data = new[]
            {
                @".|...\....",
                @"|.-.\.....",
                @".....|-...",
                @"........|.",
                @"..........",
                @".........\",
                @"..../.\\..",
                @".-.-/..|..",
                @".|....-|.\",
                @"..//.|....",
            };
            */

            Board = ArrayMethods.AddBorder(1, '#', data).Select(r => r.Select(c => c).ToArray()).ToArray();

            Mark = Board.Select(x => x.Select(c => c == '#' ? 0xff :  0).ToArray()).ToArray();

            Beam(1, 1, RIGHT);

            foreach (var r in Mark)
            {
                Console.WriteLine(new string(r.Select(c => c == 0 ? '.' : '#').ToArray()));
            }


            var sum = Mark.Select(r => r.Count(m => ((m & 0x0f) > 0) && ((m & 0xf0) == 0))).Sum();


            Console.WriteLine($"Answer is {sum}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day16.txt");

            /*
            data = new[]
            {
                @".|...\....",
                @"|.-.\.....",
                @".....|-...",
                @"........|.",
                @"..........",
                @".........\",
                @"..../.\\..",
                @".-.-/..|..",
                @".|....-|.\",
                @"..//.|....",
            };
            */

            Board = ArrayMethods.AddBorder(1, '#', data).Select(r => r.Select(c => c).ToArray()).ToArray();

            int max = 0;

            for (int i = 1; i < Board[0].Length - 1; i++)
            {
                Mark = Board.Select(x => x.Select(c => c == '#' ? 0xff : 0).ToArray()).ToArray();
                Beam(i, 1, DOWN);
                max = Math.Max(max, Mark.Select(r => r.Count(m => ((m & 0x0f) > 0) && ((m & 0xf0) == 0))).Sum());

                Mark = Board.Select(x => x.Select(c => c == '#' ? 0xff : 0).ToArray()).ToArray();
                Beam(i, Board.Length - 1, UP);
                max = Math.Max(max, Mark.Select(r => r.Count(m => ((m & 0x0f) > 0) && ((m & 0xf0) == 0))).Sum());
            }

            for (int i = 1; i < Board.Length - 1; i++)
            {
                Mark = Board.Select(x => x.Select(c => c == '#' ? 0xff : 0).ToArray()).ToArray();
                Beam(1, i, RIGHT);
                max = Math.Max(max, Mark.Select(r => r.Count(m => ((m & 0x0f) > 0) && ((m & 0xf0) == 0))).Sum());

                Mark = Board.Select(x => x.Select(c => c == '#' ? 0xff : 0).ToArray()).ToArray();
                Beam(Board[0].Length - 1, i, LEFT);
                max = Math.Max(max, Mark.Select(r => r.Count(m => ((m & 0x0f) > 0) && ((m & 0xf0) == 0))).Sum());
            }

            

            Console.WriteLine($"Answer is {max}");
        }
        
    }
}

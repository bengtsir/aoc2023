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
        internal string CurrentPattern = "";

        internal int[] CurrentStreak = new[] { 0 };

        internal long Cutoffs = 0;

        internal int[] PossibleComboMemo = new int[] { 0 };

        internal long[] ComboGeneratorMemo = new long[] { 0 };

        internal void ClearPossibleComboMemo()
        {
            int nEntries = 15000;

            PossibleComboMemo = new int[nEntries];
            for (int i = 0; i < nEntries; i++)
            {
                PossibleComboMemo[i] = -1;
            }
        }

        internal void ClearComboGeneratorMemo()
        {
            int nEntries = 150 * 50 * 100;

            ComboGeneratorMemo = new long[nEntries];
            for (int i = 0; i < nEntries; i++)
            {
                ComboGeneratorMemo[i] = -1;
            }
        }

        internal long GetComboGeneratorMemo(int a, int b, int c)
        {
            return ComboGeneratorMemo[(a * 50 * 100) + (b * 100) + c];
        }

        internal void SetComboGeneratorMemo(int a, int b, int c, long v)
        {
            ComboGeneratorMemo[(a * 50 * 100) + (b * 100) + c] = v;
        }

        internal bool TestCombo(string inp, string toTest)
        {
            if (inp.Length < toTest.Length)
            {
                throw new ArgumentException("Mismatched lengths");
            }

            for (int i = 0; i < toTest.Length; i++)
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

            if (inp.Length > toTest.Length)
            {
                return inp[toTest.Length] != '#';
            }

            return true;
        }

        private long PcMemoHits = 0;

        internal bool PossibleCombo(int startPos, int seqIdx)
        {
            int i = startPos;

            if (PossibleComboMemo[startPos * 100 + seqIdx] >= 0)
            {
                PcMemoHits++;
                return PossibleComboMemo[startPos * 100 + seqIdx] == 1;
            }

            if (seqIdx >= CurrentStreak.Length)
            {
                for (var p = startPos; p < CurrentPattern.Length; p++)
                {
                    if (CurrentPattern[p] == '#')
                    {
                        PossibleComboMemo[startPos * 100 + seqIdx] = 0;
                        return false;
                    }
                }
                PossibleComboMemo[startPos * 100 + seqIdx] = 1;
                return true;
            }

            while (i < CurrentPattern.Length)
            {
                while (i < CurrentPattern.Length && CurrentPattern[i] == '.')
                {
                    i++;
                }
                int streak = 0;
                while (i < CurrentPattern.Length && CurrentPattern[i] != '.')
                {
                    i++;
                    streak++;

                    if (streak >= CurrentStreak[seqIdx])
                    {
                        streak = 0;
                        seqIdx++;

                        if (seqIdx >= CurrentStreak.Length)
                        {
                            PossibleComboMemo[startPos * 100 + seqIdx] = 1;
                            return true;
                        }

                        // Increase one for separator between groups
                        i++;
                    }
                }
            }

            PossibleComboMemo[startPos * 100 + seqIdx] = 0;
            return false;
        }

        internal long GenerateCombos(int spacesToDistribute, int seqIdx, int initialLength = 0)
        {
            long mm = GetComboGeneratorMemo(spacesToDistribute, seqIdx, initialLength);

            if (mm >= 0)
            {
                return mm;
            }

            if (seqIdx >= CurrentStreak.Length)
            {
                var found = false;
                for (int i = initialLength; !found && (i < CurrentPattern.Length); i++)
                {
                    if (CurrentPattern[i] == '#')
                    {
                        found = true;
                    }
                }

                var retval = found ? 0 : 1;

                SetComboGeneratorMemo(spacesToDistribute, seqIdx, initialLength, retval);
                return retval;
            }
            else if (initialLength >= CurrentPattern.Length - 1 && seqIdx < CurrentStreak.Length - 1)
            {
                SetComboGeneratorMemo(spacesToDistribute, seqIdx, initialLength, 0);
                return 0;
            }
            else
            {
                long combos = 0;

                for (int i = 0; i <= spacesToDistribute; i++)
                {
                    var possible = true;
                    var p = 0;
                    while (possible && p < (i))
                    {
                        if (CurrentPattern[initialLength + p] == '#')
                        {
                            possible = false;
                        }
                        p++;
                    }
                    while (possible && (p < (i + CurrentStreak[seqIdx])))
                    {
                        if (CurrentPattern[initialLength + p] == '.')
                        {
                            possible = false;
                        }
                        p++;
                    }
                    if (seqIdx < (CurrentStreak.Length - 1))
                    {
                        if (possible && CurrentPattern[initialLength + p] == '#')
                        {
                            possible = false;
                        }
                        p++;
                    }

                    if (!possible)
                    {
                        continue;
                    }

                    if (true || PossibleCombo(initialLength + p, seqIdx + 1))
                    {
                        combos += GenerateCombos(spacesToDistribute - i, seqIdx + 1, initialLength + p);
                    }
                }

                SetComboGeneratorMemo(spacesToDistribute, seqIdx, initialLength, combos);

                return combos;
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
                ClearPossibleComboMemo();
                ClearComboGeneratorMemo();

                var streaks = value[1].Split(',').Select(Int32.Parse).ToArray();

                int minLength = streaks.Sum() + streaks.Length - 1;

                CurrentPattern = value[0];
                CurrentStreak = streaks;

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

                    combos += GenerateCombos(spacesToInsert, 0);
                }

            }


            Console.WriteLine($"Answer is {combos}");
        }

        public void Part2()
        {
            var data = File.ReadAllLines(@"data\day12.txt");

            /*
            data = new[]
            {
                "???.### 1,1,3",
                ".??..??...?##. 1,1,3",
                "?#?#?#?#?#?#?#? 1,3,1,6",
                "????.#...#... 4,1,1",
                "????.######..#####. 1,6,5",
                "?###???????? 3,2,1",
            };
            */

            var values = data.Select(r => r.Split(' ').ToArray()).ToArray();

            long combos = 0;

            int patternIndex = 0;

            foreach (var value in values)
            {
                ClearPossibleComboMemo();
                ClearComboGeneratorMemo();

                var inputString = value[0];
                patternIndex++;

                for (int i = 0; i < 4; i++)
                {
                    inputString += "?" + value[0];
                }

                CurrentPattern = inputString;

                var streaks = value[1].Split(',').Select(Int32.Parse).ToArray();

                var ss = streaks.Select(v => v).ToList();
                for (int i = 0; i < 4; i++)
                {
                    ss.AddRange(streaks.Select(v => v).ToList());
                }

                streaks = ss.ToArray();

                CurrentStreak = streaks;

                Console.WriteLine($"Testing ({patternIndex}/{values.Length}) {CurrentPattern} against {string.Join(",", streaks.Select(v => v.ToString()).ToArray())}");

                int minLength = streaks.Sum() + streaks.Length - 1;

                if (inputString.Length < minLength)
                {
                    // Do nothing
                }
                else if (inputString.Length == minLength)
                {
                    combos++;
                    Console.WriteLine($"Adding one combo, for a total so far of {combos} combos");
                }
                else
                {
                    int spacesToInsert = inputString.Length - minLength;

                    Console.WriteLine($"Spaces to allocate = {spacesToInsert}");

                    long currCombos = GenerateCombos(spacesToInsert, 0);
                    combos += currCombos;

                    Console.WriteLine($"Adding {currCombos} combos, for a total so far of {combos} combos");
                }

            }


            Console.WriteLine($"Answer is {combos}");
        }

    }
}

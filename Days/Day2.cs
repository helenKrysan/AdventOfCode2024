using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day2 : IDay<int>
    {
        public async Task<int> SolvePart1Async()
        {
            var input = await ReadFileUtils.ReadFileAsync(2);

            var safeCount = 0;
            foreach (var line in input)
            {
                var row = line.Split(' ').Select((c, i) =>
                {
                    return int.Parse(c);
                }).ToList();

                var isAsc = false;
                if (row[0] < row[1])
                {
                    isAsc = true;
                }
                
                if (IsRowSafe(row,isAsc))
                {
                    safeCount++;
                }
            }
            return safeCount;
        }

        public async Task<int> SolvePart2Async()
        {
            var input = await ReadFileUtils.ReadFileAsync(2);
            var safeCount = 0;
            foreach (var line in input)
            {
                var row = line.Split(' ').Select((c, i) =>
                {
                    return int.Parse(c);
                }).ToList();
                var isAsc = false;
                if (row[0] < row[1])
                {
                    isAsc = true;
                }

                if (IsRowSafe(row, isAsc))
                {
                    safeCount++;
                }
                else
                {
                    for (int j = 0; j < row.Count; j++)
                    {
                        var newRow = new List<int>(row);
                        newRow.RemoveAt(j);
                        if (newRow[0] < newRow[1])
                        {
                            isAsc = true;
                        }
                        else
                        {
                            isAsc = false;
                        }                        
                        if (IsRowSafe(newRow, isAsc))
                        {
                            safeCount++;
                            break;
                        }
                    }
                }
            }
            return safeCount;
        }
        private bool IsRowSafe(List<int> row, bool isAsc)
        {
            var isSafe = true;
            for (int i = 1; i < row.Count; i++)
            {
                var dist = row[i] - row[i - 1];
                if (isAsc)
                {
                    if (dist > 3 || dist <= 0)
                    {
                        isSafe = false;
                        break;
                    }
                }
                else
                {
                    if (dist < -3  || dist >= 0)
                    {
                        isSafe = false;
                        break;
                    }
                }
            }
            return isSafe;
        }
    }  
}

using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day4 : IDay<int>
    {
        private List<List<char>> XMASchars = [];
        public async Task<int> SolvePart1Async()
        {
            var input = await ReadFileUtils.ReadFileAsync(4);
            XMASchars = [];
            foreach (var line in input)
            {
                var list = new List<char>();
                foreach (var c in line)
                {
                    list.Add(c);
                }
                XMASchars.Add(list);
            }
            var count = 0;
            for (int i = 0; i < XMASchars.Count; i++)
            {
                for (int j = 0; j < XMASchars[i].Count; j++)
                {
                    count += SearchXMAS(i, j);
                }
            }
            return count;
        }

        private int SearchXMAS(int y, int x)
        {
            var finded = 0;
            if (XMASchars[y][x] != 'X')
            {
                return finded;
            }
            if (x + 3 < XMASchars[y].Count)
            {
                if (CheckLetter(y, x + 1, 'M') &&
                    CheckLetter(y, x + 2, 'A') &&
                    CheckLetter(y, x + 3, 'S'))
                {
                    finded++;
                }
            }
            if (x - 3 >= 0)
            {
                if (CheckLetter(y, x - 1, 'M') &&
                    CheckLetter(y, x - 2, 'A') &&
                    CheckLetter(y, x - 3, 'S'))
                {
                    finded++;
                }
            }
            if (y + 3 < XMASchars.Count)
            {
                if (CheckLetter(y + 1, x, 'M') &&
                    CheckLetter(y + 2, x, 'A') &&
                    CheckLetter(y + 3, x, 'S'))
                {
                    finded++;
                }
            }
            if (y - 3 >= 0)
            {
                if (CheckLetter(y - 1, x, 'M') &&
                    CheckLetter(y - 2, x, 'A') &&
                    CheckLetter(y - 3, x, 'S'))
                {
                    finded++;
                }
            }
            if (y + 3 < XMASchars.Count && x + 3 < XMASchars[y].Count)
            {
                if (CheckLetter(y + 1, x + 1, 'M') &&
                    CheckLetter(y + 2, x + 2, 'A') &&
                    CheckLetter(y + 3, x + 3, 'S'))
                {
                    finded++;
                }
            }
            if (y + 3 < XMASchars.Count && x - 3 >= 0)
            {
                if (CheckLetter(y + 1, x - 1, 'M') &&
                    CheckLetter(y + 2, x - 2, 'A') &&
                    CheckLetter(y + 3, x - 3, 'S'))
                {
                    finded++;
                }
            }
            if (y - 3 >= 0 && x + 3 < XMASchars[y].Count)
            {
                if (CheckLetter(y - 1, x + 1, 'M') &&
                    CheckLetter(y - 2, x + 2, 'A') &&
                    CheckLetter(y - 3, x + 3, 'S'))
                {
                    finded++;
                }
            }
            if (y - 3 >= 0 && x - 3 >= 0)
            {
                if (CheckLetter(y - 1, x - 1, 'M') &&
                    CheckLetter(y - 2, x - 2, 'A') &&
                    CheckLetter(y - 3, x - 3, 'S'))
                {
                    finded++;
                }
            }
            return finded;
        }

        private bool SearchMAS(int y, int x)
        {
            if (XMASchars[y][x] != 'A')
            {
                return false;
            }
            if(CheckLetter(y-1,x-1,'M')
                && CheckLetter(y + 1, x + 1, 'S')
                && CheckLetter(y - 1, x + 1, 'M')
                && CheckLetter(y + 1, x - 1, 'S'))
            {     
                    return true;
            }
            if (CheckLetter(y - 1, x - 1, 'M')
                && CheckLetter(y + 1, x + 1, 'S')
                && CheckLetter(y - 1, x + 1, 'S')
                && CheckLetter(y + 1, x - 1, 'M'))
            {
                return true;
            }
            if (CheckLetter(y - 1, x - 1, 'S')
                && CheckLetter(y + 1, x + 1, 'M')
                && CheckLetter(y - 1, x + 1, 'M')
                && CheckLetter(y + 1, x - 1, 'S'))
            {
                return true;
            }
            if (CheckLetter(y - 1, x - 1, 'S')
                && CheckLetter(y + 1, x + 1, 'M')
                && CheckLetter(y - 1, x + 1, 'S')
                && CheckLetter(y + 1, x - 1, 'M'))
            {
                return true;
            }
            return false;
        }

        private bool CheckLetter(int y, int x, char letter)
        {
            if (XMASchars[y][x] == letter)
            {
                return true;
            }
            return false;
        }

        public async Task<int> SolvePart2Async()
        {
            var input = await ReadFileUtils.ReadFileAsync(4);
            XMASchars = [];
            foreach (var line in input)
            {
                var list = new List<char>();
                foreach (var c in line)
                {
                    list.Add(c);
                }
                XMASchars.Add(list);
            }
            var count = 0;
            for (int i = 1; i < XMASchars.Count-1; i++)
            {
                for (int j = 1; j < XMASchars[i].Count-1; j++)
                {
                    if(SearchMAS(i,j))
                    {
                        count++;
                    }
                 }
            }
            return count;
        }
    }
}

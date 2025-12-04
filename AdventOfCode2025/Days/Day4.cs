
using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Days
{
    internal class Day4 : IDay<long>
    {
        private List<List<char>> _papers;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            int res = 0;
            var height = _papers.Count;
            var width = _papers[0].Count;
            for(int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if(_papers[i][j] == '.')
                        continue;
                    var current = _papers[i][j];
                    var neighborsCount = GetNeighborsCount(i, j, height, width, current);
                    if (neighborsCount < 4)
                        res++;
                }
            }
            return res;
        }

        private int GetNeighborsCount(int x, int y, int height, int width, char current)
        {
            int count = 0;
            var directions = new (int, int)[]
            {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),          (0, 1),
                (1, -1),  (1, 0),  (1, 1)
            };
            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < height && ny >= 0 && ny < width)
                {
                    if (_papers[nx][ny] == current)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput(); 
            int res = 0;
            var height = _papers.Count;
            var width = _papers[0].Count;
            bool accessed = true;
            while (accessed)
            {
                accessed = false;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (_papers[i][j] == '.')
                            continue;
                        var current = _papers[i][j];
                        var neighborsCount = GetNeighborsCount(i, j, height, width, current);
                        if (neighborsCount < 4)
                        {
                            _papers[i][j] = '.';
                            accessed = true;
                            res++;
                        }
                    }
                }
            }
            return res;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(4);
            _papers = new List<List<char>>();
            foreach (var line in input)
            {
                _papers.Add(line.ToList());
            }
        }
    }
}


using AdventOfCode2024.Utils;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode2024.Days
{
    internal class Day20 : IDay<long>
    {
        private List<List<char>> _mapChar;
        private List<List<int>> _map;
        private (int,int) _start;
        private (int, int) _end;
        private int _pathSize;
        private HashSet<(int, int, int)> _allCheats;

        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            _allCheats = new HashSet<(int, int, int)>();
            for (int i = 1; i < _map.Count - 1; i++)
            {
                for (int j = 1; j < _map[0].Count - 1; j++)
                {
                    if (_map[i][j] > 0)
                    {
                        if (_map[i + 1][j] == -1)
                        {
                            if (i + 2 < _map.Count && _map[i + 2][j] > 0)
                            {
                                var newSize = _pathSize - (Math.Abs(_map[i][j] - _map[i + 2][j]) - 2);
                                _allCheats.Add((i + 1, j, newSize));
                            }
                        }
                        if (_map[i - 1][j] == -1)
                        {
                            if (i - 2 >= 0 && _map[i - 2][j] > 0)
                            {
                                var newSize = _pathSize - (Math.Abs(_map[i][j] - _map[i - 2][j]) - 2);
                                _allCheats.Add((i - 1, j, newSize));
                            }
                        }
                        if (_map[i][j + 1] == -1)
                        {
                            if (j + 2 < _map[0].Count && _map[i][j + 2] > 0)
                            {
                                var newSize = _pathSize - (Math.Abs(_map[i][j] - _map[i][j + 2]) - 2);
                                _allCheats.Add((i, j + 1, newSize));
                            }
                        }
                        if (_map[i][j - 1] == -1)
                        {
                            if (j - 2 >= 0 && _map[i][j - 2] > 0)
                            {
                                var newSize = _pathSize - (Math.Abs(_map[i][j] - _map[i][j - 2]) - 2);
                                _allCheats.Add((i, j - 1, newSize));
                            }
                        }
                    }
                }
            }
            var saves = _allCheats.Select(x => _pathSize - x.Item3).GroupBy(x => x);
            var sum = 0L;
            foreach(var save in saves)
            {
                if(save.Key >= 100)
                {
                    sum += save.Count();
                }
            }
            return sum;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            return 0L;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(20);
            //var input = ReadFileUtils.ReadTestFile(20);
            _mapChar = new List<List<char>>();
            _map = new List<List<int>>();
            foreach (var line in input)
            {
                var replaced = line.Replace('S', '.').Replace('E', '.');
                _mapChar.Add(replaced.ToCharArray().ToList());
                _map.Add(replaced.Select(c => c == '#' ? -1 : 0).ToList());
                if (line.Contains("S"))
                {
                    var index = line.IndexOf('S');
                    _start.Item1 = _map.Count - 1;
                    _start.Item2 = index;
                }
                if (line.Contains("E"))
                {
                    var index = line.IndexOf('E');
                    _end.Item1 = _map.Count - 1;
                    _end.Item2 = index;
                }

            }

            var currentY = _start.Item1;
            var currentX = _start.Item2;
            _pathSize = 0;
            var visited = new List<List<bool>>();
            for(int i = 0; i < _mapChar.Count; i++)
            {
                visited.Add(Enumerable.Repeat(false, _mapChar[0].Count).ToList());
            }
            while (currentY != _end.Item1 || currentX != _end.Item2)
            {
                if(_mapChar[currentY][currentX] == '.') 
                {
                    _map[currentY][currentX] = _pathSize;
                }
                _pathSize++;
                visited[currentY][currentX] = true;
                if (currentY + 1 < _map.Count && !visited[currentY + 1][currentX] && _mapChar[currentY + 1][currentX] == '.')
                {
                    currentY++;
                }
                else if (currentY - 1 >= 0 && !visited[currentY - 1][currentX] && _mapChar[currentY - 1][currentX] == '.')
                {
                    currentY--;
                }
                else if (currentX + 1 < _mapChar[0].Count && !visited[currentY][currentX + 1] && _mapChar[currentY][currentX + 1] == '.')
                {
                    currentX++;
                }
                else if (currentX - 1 >= 0 && !visited[currentY][currentX - 1] && _mapChar[currentY][currentX - 1] == '.')
                {
                    currentX--;
                }
            }
            _map[currentY][currentX] = _pathSize;
        }
    }
}

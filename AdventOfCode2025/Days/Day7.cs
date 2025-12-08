
using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Days
{
    internal class Day7 : IDay<long>
    {
        List<List<char>> _manifold;
        int _width;
        int _height;

        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            long res = 0;
            for (int i = 0; i < _manifold.Count; i++)
            {
                for (int j = 0; j < _manifold[i].Count; j++)
                {
                    if (i - 1 >= 0)
                    {
                        if (_manifold[i][j] == '^' && _manifold[i - 1][j] == '|')
                        {
                            res++;
                            if (j - 1 >= 0 && _manifold[i][j - 1] != '|' && _manifold[i][j - 1] != '^')
                            {
                                _manifold[i][j - 1] = '|';
                            }
                            if (j + 1 < _width && _manifold[i][j + 1] != '|' && _manifold[i][j - 1] != '^')
                            {
                                _manifold[i][j + 1] = '|';
                            }
                        }
                        else if (_manifold[i - 1][j] == '|')
                        {
                            _manifold[i][j] = '|';
                        }
                    }
                    else
                    {
                        if (_manifold[i][j] == 'S')
                        {
                            _manifold[i][j] = '|';
                        }
                    }
                }
            }
            return res;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            long res = 0;
            var start = _manifold[0].FindIndex(c => c == 'S');
            _manifold[0][start] = '|';
            var array = _manifold.Select(c => c.ToArray()).ToArray();
            var totalTimeLineCount = TimeLineCountDFS(0,start,array);
            return totalTimeLineCount;
        }

        Dictionary<(int, int), long> _timelineCache = new Dictionary<(int, int), long>();
        private long TimeLineCountDFS(int i, int j, char[][] manifold)
        {
            if (i >= _height)
            {
                return 1;
            }
            else if (_timelineCache.TryGetValue((i, j), out var tc))
            {
                return tc;
            }
            if (manifold[i][j] == '^' && manifold[i - 1][j] == '|')
            {
                long tc = 0;
                if (j - 1 >= 0 && manifold[i][j - 1] != '|' && manifold[i][j - 1] != '^')
                {
                    manifold[i][j - 1] = '|';
                    var tcl = TimeLineCountDFS(i + 1, j - 1, manifold);
                    _timelineCache[(i, j - 1)] = tcl;
                    tc += tcl;
                    manifold[i][j - 1] = '.';
                }
                if (j + 1 < _width && manifold[i][j + 1] != '|' && manifold[i][j - 1] != '^')
                {
                    manifold[i][j + 1] = '|';
                    var tcr = TimeLineCountDFS(i + 1, j + 1, manifold);
                    _timelineCache[(i, j + 1)] = tcr;
                    tc += tcr;
                    manifold[i][j + 1] = '.';
                }
                return tc;
            }
            else
            {
                manifold[i][j] = '|';
                var tc = TimeLineCountDFS(i + 1, j, manifold);
                manifold[i][j] = '.';
                return tc;
            }
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(7);
            //var input = ReadFileUtils.ReadTestFile(7);
            _manifold = new List<List<char>>();
            foreach (var line in input)
            {
                _manifold.Add(line.ToList());
            }
            _width = _manifold[0].Count;
            _height = _manifold.Count;
        }
    }
}

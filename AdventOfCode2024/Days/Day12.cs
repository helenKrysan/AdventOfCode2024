
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day12 : IDay<int>
    {
        private List<List<char>> _garden = new List<List<char>>();
        private List<List<bool>> _visited = new List<List<bool>>();
        private List<List<(int, int, int, char)>> _regions = new List<List<(int, int, int, char)>>();
        int _currentRegion = 0;
        public async Task<int> SolvePart1Async()
        {
            await ReadInput();
            _regions = new List<List<(int, int, int, char)>>();
            _currentRegion = 0;
            for (var i = 0; i < _garden.Count; i++)
            {
                for (var j = 0; j < _garden[i].Count; j++)
                {
                    if (!_visited[i][j])
                    {
                        _regions.Add(new List<(int, int, int, char)>());
                        GetRegion(_garden[i][j], i, j);
                        _currentRegion++;
                    }
                }
            }
            return _regions.Sum(region => region.Count * region.Sum(elem => elem.Item3));
        }

        private void GetRegion(char currentRegion, int y, int x)
        {
            if (currentRegion != _garden[y][x])
            {
                return;
            }
            _visited[y][x] = true;
            var perimiter = 4;
            if (x - 1 >= 0)
            {
                if (!_visited[y][x - 1])
                {
                    GetRegion(currentRegion, y, x - 1);
                }
                if (currentRegion == _garden[y][x - 1])
                {
                    perimiter--;
                }
            }
            if (x + 1 < _garden[y].Count)
            {
                if (!_visited[y][x + 1])
                {
                    GetRegion(currentRegion, y, x + 1);
                }
                if (currentRegion == _garden[y][x + 1])
                {
                    perimiter--;
                }
            }
            if (y - 1 >= 0)
            {
                if (!_visited[y - 1][x])
                {
                    GetRegion(currentRegion, y - 1, x);
                }
                if (currentRegion == _garden[y - 1][x])
                {
                    perimiter--;
                }
            }
            if (y + 1 < _garden.Count)
            {
                if (!_visited[y + 1][x])
                {
                    GetRegion(currentRegion, y + 1, x);
                }
                if (currentRegion == _garden[y + 1][x])
                {
                    perimiter--;
                }
            }
            _regions[_currentRegion].Add((y, x, perimiter, currentRegion));
        }

        public async Task<int> SolvePart2Async()
        {
            //solve first part to get all regions
            await SolvePart1Async();
            var total = 0;
            foreach (var region in _regions)
            {
                var sum = 0;
                //split region by rows
                var rows = new List<List<(int, int, int, char)>>();
                foreach (var group in region.GroupBy(elem => elem.Item1))
                {
                    var list = group.ToList();
                    list.Sort(new Comparison<(int, int, int, char)>((a, b) =>
                    {
                        return a.Item2.CompareTo(b.Item2);
                    }));
                    rows.Add(list);
                }

                for (var i = 0; i < rows.Count; i++)
                {
                    //up
                    var isBorder = false;
                    var lastElement = (-1, -1, -1, '\0');
                    if (rows[i].Count > 1)
                    {
                        lastElement = rows[i][0];
                        lastElement.Item2--;
                    }
                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        var elem = rows[i][j];
                        if (elem.Item2 != lastElement.Item2 + 1)
                        {
                            if (isBorder)
                            {
                                sum++;
                            }
                            isBorder = false;
                        }
                        if (elem.Item1 - 1 < 0)
                        {
                            isBorder = true;
                        }
                        else if (_garden[elem.Item1 - 1][elem.Item2] != elem.Item4)
                        {
                            isBorder = true;
                        }
                        else
                        {
                            if (isBorder)
                            {
                                sum++;
                            }
                            isBorder = false;
                        }
                        lastElement = elem;
                    }
                    if (isBorder)
                    {
                        sum++;
                    }
                    //down
                    isBorder = false;
                    lastElement = (-1, -1, -1, '\0');
                    if (rows[i].Count > 1)
                    {
                        lastElement = rows[i][0];
                        lastElement.Item2--;
                    }
                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        var elem = rows[i][j];
                        if (elem.Item2 != lastElement.Item2 + 1)
                        {
                            if (isBorder)
                            {
                                sum++;
                            }
                            isBorder = false;
                        }
                        if (elem.Item1 + 1 >= _garden.Count)
                        {
                            isBorder = true;
                        }
                        else if (_garden[elem.Item1 + 1][elem.Item2] != elem.Item4)
                        {
                            isBorder = true;
                        }
                        else
                        {
                            if (isBorder)
                            {
                                sum++;
                            }
                            isBorder = false;
                        }
                        lastElement = elem;
                    }
                    if (isBorder)
                    {
                        sum++;
                    }
                }

                //split region by columns
                var columns = new List<List<(int, int, int, char)>>();
                foreach (var group in region.GroupBy(elem => elem.Item2))
                {
                    var list = group.ToList();
                    list.Sort(new Comparison<(int, int, int, char)>((a, b) =>
                    {
                        return a.Item1.CompareTo(b.Item1);
                    }));
                    columns.Add(list);
                }

                for (var i = 0; i < columns.Count; i++)
                {
                    //left
                    var isBorder = false;
                    var lastElement = (-1, -1, -1, '\0');
                    if (columns[i].Count > 1)
                    {
                        lastElement = columns[i][0];
                        lastElement.Item1--;
                    }
                    for (int j = 0; j < columns[i].Count; j++)
                    {
                        var elem = columns[i][j];
                        if (elem.Item1 != lastElement.Item1 + 1)
                        {
                            if (isBorder)
                            {
                                sum++;
                            }
                            isBorder = false;
                        }
                        if (elem.Item2 - 1 < 0)
                        {
                            isBorder = true;
                        }
                        else if (_garden[elem.Item1][elem.Item2 - 1] != elem.Item4)
                        {
                            isBorder = true;
                        }
                        else
                        {
                            if (isBorder)
                            {
                                sum++;
                            }
                            isBorder = false;
                        }
                        lastElement = elem;
                    }
                    if (isBorder)
                    {
                        sum++;
                    }
                    //right
                    isBorder = false;
                    lastElement = (-1, -1, -1, '\0');
                    if (columns[i].Count > 1)
                    {
                        lastElement = columns[i][0];
                        lastElement.Item1--;
                    }
                    for (int j = 0; j < columns[i].Count; j++)
                    {
                        var elem = columns[i][j];
                        if (elem.Item1 != lastElement.Item1 + 1)
                        {
                            if (isBorder)
                            {
                                sum++;
                            }
                            isBorder = false;
                        }
                        if (elem.Item2 + 1 >= _garden[0].Count)
                        {
                            isBorder = true;
                        }
                        else if (_garden[elem.Item1][elem.Item2 + 1] != elem.Item4)
                        {
                            isBorder = true;
                        }
                        else
                        {
                            if (isBorder)
                            {
                                sum++;
                            }
                            isBorder = false;
                        }
                        lastElement = elem;
                    }
                    if (isBorder)
                    {
                        sum++;
                    }
                }
                total += sum * region.Count;
            }
            return total;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(12);
            _garden = new List<List<char>>();
            _visited = new List<List<bool>>();

            input.ForEach(line =>
            {
                var list = new List<char>();
                var visitedList = new List<bool>();
                foreach (var c in line)
                {
                    list.Add(c);
                    visitedList.Add(false);
                }
                _garden.Add(list);
                _visited.Add(visitedList);
            });
        }
    }
}

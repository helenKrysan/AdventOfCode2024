
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    public enum DirectionGraph
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
    internal class Day16 : IDay<long>
    {
        private List<List<int>> _map;
        private long[,] _graph;
        private int _start;
        private int _end;
        private Dictionary<long, (int, int)> _nodesCoord;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            return Dijkstra(_start, _end, out _);
        }

        private long Dijkstra(int start, int end, out List<List<long>> distmap)
        {
            var distances = new List<(long, DirectionGraph)>(_graph.GetLength(0));
            var visited = new bool[_graph.GetLength(0)];
            for (var i = 0; i < _graph.GetLength(0); i++)
            {
                distances.Add((long.MaxValue, DirectionGraph.None));
            }
            distances[start] = (0, DirectionGraph.Right);
            for (var i = 0; i < distances.Count; i++)
            {
                var min = long.MaxValue;
                var minIndex = -1;
                for (var j = 0; j < distances.Count; j++)
                {
                    if (!visited[j] && distances[j].Item1 <= min)
                    {
                        min = distances[j].Item1;
                        minIndex = j;
                    }
                }
                for (var j = 0; j < distances.Count; j++)
                {
                    if (!visited[j] && _graph[minIndex, j] != 0)
                    {
                        var dist = distances[minIndex].Item1 + 1;
                        if (IsTurn(minIndex, j, distances[minIndex].Item2, out DirectionGraph turn))
                        {
                            dist += 1000;
                            if (dist < distances[j].Item1)
                            {
                                distances[j] = (dist, turn);
                            }
                        }
                        else
                        {
                            if (dist < distances[j].Item1)
                            {
                                distances[j] = (dist, distances[minIndex].Item2);
                            }
                        }
                    }
                }
                visited[minIndex] = true;
            }
            distmap = _map.Select(l => l.Select(i => i == -1 ? int.MaxValue : distances[i].Item1).ToList()).ToList();
            return distances[end].Item1;
        }

        private bool IsTurn(long from, long to, DirectionGraph currentDirection, out DirectionGraph turn)
        {
            turn = DirectionGraph.None;
            if (!_nodesCoord.ContainsKey(from) || !_nodesCoord.ContainsKey(to))
            {
                return false;
            }
            switch (currentDirection)
            {
                case DirectionGraph.Right:
                    {
                        if (_nodesCoord[from].Item1 < _nodesCoord[to].Item1)
                        {
                            turn = DirectionGraph.Down;
                            return true;
                        }
                        else if (_nodesCoord[from].Item1 > _nodesCoord[to].Item1)
                        {
                            turn = DirectionGraph.Up;
                            return true;
                        }
                        break;
                    }
                case DirectionGraph.Left:
                    {
                        if (_nodesCoord[from].Item1 < _nodesCoord[to].Item1)
                        {
                            turn = DirectionGraph.Up;
                            return true;
                        }
                        else if (_nodesCoord[from].Item1 > _nodesCoord[to].Item1)
                        {
                            turn = DirectionGraph.Down;
                            return true;
                        }
                        break;
                    }
                case DirectionGraph.Up:
                    {
                        if (_nodesCoord[from].Item2 < _nodesCoord[to].Item2)
                        {
                            turn = DirectionGraph.Right;
                            return true;
                        }
                        else if (_nodesCoord[from].Item2 > _nodesCoord[to].Item2)
                        {
                            turn = DirectionGraph.Left;
                            return true;
                        }
                        break;
                    }
                case DirectionGraph.Down:
                    {
                        if (_nodesCoord[from].Item2 < _nodesCoord[to].Item2)
                        {
                            turn = DirectionGraph.Left;
                            return true;
                        }
                        else if (_nodesCoord[from].Item2 > _nodesCoord[to].Item2)
                        {
                            turn = DirectionGraph.Right;
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }

        //this solution is not working fully correct
        //it counts all stops, but have some extra tiles that should be removed manually
        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            Dijkstra(_start, _end, out var distmap);
            var visited = new List<List<bool>>();
            foreach(var line in distmap)
            {
                visited.Add(new List<bool>(line.Select(i => false)));
            }
            var sum = CountStops(_nodesCoord[_end].Item1, _nodesCoord[_end].Item2, distmap, visited);
            for (int i = 0; i < visited.Count; i++)
            {
                for (int j = 0; j < visited[i].Count; j++)
                {
                    if (visited[i][j])
                    {
                        Console.Write("O");
                    }
                    else
                    {
                        if (_map[i][j] == -1)
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                }

                Console.WriteLine();
            }
            return sum;
        }

        private int CountStops(int Y, int X, List<List<long>> distmap, List<List<bool>> visited)
        {
            var sum = 0;
            if (visited[Y][X])
            {
                return sum;
            }
            sum++;
            visited[Y][X] = true;
            if (_nodesCoord[_start].Item1 == Y && _nodesCoord[_start].Item2 == X)
            {
                return sum;
            }
            var currentSize = distmap[Y][X];
            if(distmap[Y - 1][X] < currentSize)
            {
                sum += CountStops(Y - 1, X, distmap, visited);
            }
            else if (Y > 1 && distmap[Y - 1][X] == currentSize + 999)
            {
                if ((distmap[Y][X - 1] != int.MaxValue || distmap[Y][X + 1] != int.MaxValue) && distmap[Y + 1][X] != int.MaxValue)
                {
                    sum += CountStops(Y - 1, X, distmap, visited);
                }
            }
            if (distmap[Y + 1][X] < currentSize)
            {
                sum += CountStops(Y + 1, X, distmap, visited);
            }
            else if (Y < distmap.Count - 2 && distmap[Y + 1][X] == currentSize + 999)
            {
                if ((distmap[Y][X - 1] != int.MaxValue || distmap[Y][X + 1] != int.MaxValue) && distmap[Y - 1][X] != int.MaxValue)
                {
                    sum += CountStops(Y + 1, X, distmap, visited);
                }
            }

            if (distmap[Y][X - 1] < currentSize)
            {
                sum += CountStops(Y, X - 1, distmap, visited);
            }

            else if (X > 1 && distmap[Y][X - 1] == currentSize + 999)
            {
                if ((distmap[Y - 1][X] != int.MaxValue || distmap[Y + 1][X] != int.MaxValue) && distmap[Y][X+1] != int.MaxValue)
                {
                    sum += CountStops(Y, X - 1, distmap, visited);
                }
            }
            if (distmap[Y][X + 1] < currentSize)
            {
                sum += CountStops(Y, X + 1, distmap, visited);
            }
            else if (X < distmap[Y].Count - 2 && distmap[Y][X + 1] == currentSize + 999)
            {
                if ((distmap[Y - 1][X] != int.MaxValue || distmap[Y + 1][X] != int.MaxValue) && distmap[Y][X - 1] != int.MaxValue)
                {
                    sum += CountStops(Y, X + 1, distmap, visited);
                }
            }
            return sum;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(16);
            var dotcount = input.Select(line => line.Where(c => c == '.').Count()).Aggregate((sum, count) => sum + count);
            dotcount += 2;
            _graph = new long[dotcount, dotcount];
            _map = new List<List<int>>();
            _nodesCoord = new Dictionary<long, (int, int)>();
            var currentVertex = 0;
            var startX = 0;
            var startY = 0;
            var visited = new List<List<Dictionary<Direction, bool>>>();
            foreach (var line in input)
            {
                var newList = new List<Dictionary<Direction, bool>>(line.Length);
                for (var i = 0; i < line.Length; i++)
                {
                    newList.Add(new Dictionary<Direction, bool>()
                    {
                    { Direction.Right, false },
                    { Direction.Left, false },
                    { Direction.Up, false },
                    { Direction.Down, false}
                    });
                }
                visited.Add(newList);
                _map.Add(line.Select((c, index) =>
                {
                    if (c != '#')
                    {
                        _nodesCoord.Add(currentVertex, (_map.Count, index));
                    }
                    return c == '#' ? -1 : currentVertex++;
                }).ToList());
                if (line.Contains("S"))
                {
                    var index = line.IndexOf('S');
                    _start = _map.Last()[index];
                    startY = _map.Count - 1;
                    startX = index;
                }
                if (line.Contains("E"))
                {
                    var index = line.IndexOf('E');
                    _end = _map.Last()[index];
                }
            }
            FillGraph(startX, startY);
        }

        private void FillGraph(int x, int y)
        {
            for (int i = 0; i < _map.Count; i++)
            {
                for (int j = 0; j < _map[i].Count; j++)
                {
                    if (_map[i][j] != -1)
                    {
                        if (i > 0 && _map[i - 1][j] != -1)
                        {
                            _graph[_map[i][j], _map[i - 1][j]] = 1;
                            _graph[_map[i - 1][j], _map[i][j]] = 1;
                        }
                        if (i < _map.Count - 1 && _map[i + 1][j] != -1)
                        {
                            _graph[_map[i][j], _map[i + 1][j]] = 1;
                            _graph[_map[i + 1][j], _map[i][j]] = 1;
                        }
                        if (j > 0 && _map[i][j - 1] != -1)
                        {
                            _graph[_map[i][j], _map[i][j - 1]] = 1;
                            _graph[_map[i][j - 1], _map[i][j]] = 1;
                        }
                        if (j < _map[i].Count - 1 && _map[i][j + 1] != -1)
                        {
                            _graph[_map[i][j], _map[i][j + 1]] = 1;
                            _graph[_map[i][j + 1], _map[i][j]] = 1;
                        }
                    }
                }
            }
        }
    }
}

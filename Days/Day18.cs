
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day18 : IDay<long>
    {
        private List<(int, int)> _fallingBytes;
        private const int _size = 71;
        private int[,] _map;
        private bool[,] _graph;
        private int _fallingByte;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            SimulateFall();
            FillGraph();
            return Dijkstra(0, _size*_size - 1);
        }

        private void SimulateFall()
        {
            for (var i = 0; i < 1024; i++)
            {
                _map[_fallingBytes[i].Item1, _fallingBytes[i].Item2] = -1;
                _fallingByte++;
            }
        }

        private void FillGraph()
        {
            _graph = new bool[_size * _size, _size * _size];
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_map[i, j] != -1)
                    {
                        if (i > 0 && _map[i - 1, j] != -1)
                        {
                            _graph[_map[i, j], _map[i - 1, j]] = true;
                            _graph[_map[i - 1, j], _map[i, j]] = true;
                        }
                        if (i < _size - 1 && _map[i + 1, j] != -1)
                        {
                            _graph[_map[i, j], _map[i + 1, j]] = true;
                            _graph[_map[i + 1, j], _map[i, j]] = true;
                        }
                        if (j > 0 && _map[i, j - 1] != -1)
                        {
                            _graph[_map[i, j], _map[i, j - 1]] = true;
                            _graph[_map[i, j - 1], _map[i, j]] = true;
                        }
                        if (j < _size - 1 && _map[i, j + 1] != -1)
                        {
                            _graph[_map[i, j], _map[i, j + 1]] = true;
                            _graph[_map[i, j + 1], _map[i, j]] = true;
                        }
                    }
                }
            }
        }

        private long Dijkstra(int start, int end)
        {
            var distances = new List<long>(_graph.GetLength(0));
            var visited = new bool[_graph.GetLength(0)];
            for (var i = 0; i < _graph.GetLength(0); i++)
            {
                distances.Add(long.MaxValue);
            }
            distances[start] = 0;
            for (var i = 0; i < distances.Count; i++)
            {
                var min = long.MaxValue;
                var minIndex = -1;
                for (var j = 0; j < distances.Count; j++)
                {
                    if (!visited[j] && distances[j] <= min)
                    {
                        min = distances[j];
                        minIndex = j;
                    }
                }
                for (var j = 0; j < distances.Count; j++)
                {
                    if (!visited[j] && _graph[minIndex, j])
                    {
                        var dist = distances[minIndex] + 1;

                        if (dist < distances[j])
                        {
                            distances[j] = dist;
                        }
                    }
                }
                visited[minIndex] = true;
            }
            return distances[end];
        }

        //it should be solved using binary search not direct bruteforce
        //but solution works under 10 minutes so i just left it as it is
        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            SimulateFall();
            FillGraph();
            var coords = (-1, -1);
            while (_fallingByte < _fallingBytes.Count)
            {
                var vertex = _map[_fallingBytes[_fallingByte].Item1, _fallingBytes[_fallingByte].Item2];
                if (vertex % _size != _size - 1)
                {
                    _graph[vertex, vertex + 1] = false;
                    _graph[vertex + 1, vertex] = false;
                }
                if (vertex % _size != 0)
                {
                    _graph[vertex, vertex - 1] = false;
                    _graph[vertex - 1, vertex] = false;
                }
                if (vertex >= _size)
                {
                    _graph[vertex, vertex - _size] = false;
                    _graph[vertex - _size, vertex] = false;
                }
                if (vertex < _size * (_size - 1))
                {
                    _graph[vertex, vertex + _size] = false;
                    _graph[vertex + _size, vertex] = false;
                }
                _map[_fallingBytes[_fallingByte].Item1, _fallingBytes[_fallingByte].Item2] = -1;
                var path = Dijkstra(0, _size * _size - 1);
                if (path == long.MaxValue)
                {
                    coords = (_fallingBytes[_fallingByte].Item2, _fallingBytes[_fallingByte].Item1); //reverse coords
                    break;
                }
                _fallingByte++;
            }
            Console.WriteLine(coords);
            return 0;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(18);
            _fallingBytes = new List<(int, int)>(); 
            _map = new int[_size, _size];
            _fallingByte = 0;
            foreach (var item in input)
            {
                var splitted = item.Split(',');
                _fallingBytes.Add((int.Parse(splitted[1]), int.Parse(splitted[0])));
            }

            var vertex = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    _map[i, j] = vertex;
                    vertex++;
                }
            }

        }
    }
}

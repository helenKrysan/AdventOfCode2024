
using AdventOfCode2024.Utils;
using System.Xml.Linq;

namespace AdventOfCode2024.Days
{
    internal class Day21 : IDay<long>
    {
        private List<List<int>> _codes;
        private Dictionary<char, int> _buttonToNode;

        private bool[,] _graphKeyboard;
        private List<List<int>> _mapKeyboard = new List<List<int>>()
        {
            new List<int> { 7,8,9},
            new List<int> { 4,5,6},
            new List<int> { 1,2,3},
            new List<int> { -1,0,10}
        };

        private bool[,] _graphRobotKeyboard;
        private List<List<int>> _mapRobotKeyboard = new List<List<int>>()
        {
            new List<int> { -1,1,0},
            new List<int> { 3,2,4 }
        };

        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            var total = 0L;
            foreach (var code in _codes)
            {
                var current = 10;
                var sum = 0L;
                var path = "";
                var bestPathKeyboard = new List<string>();
                foreach (var node in code)
                {   
                    Dijkstra(current, node, _graphKeyboard);
                    var startY = _mapKeyboard.Select(l => l.Contains(current)).ToList().IndexOf(true);
                    var startX = _mapKeyboard[startY].IndexOf(current);
                    var endY = _mapKeyboard.Select(l => l.Contains(node)).ToList().IndexOf(true);
                    var endX = _mapKeyboard[endY].IndexOf(node);
                    var visited = new List<List<bool>>();
                    for (var i = 0; i < _mapKeyboard.Count; i++)
                    {
                        visited.Add(new List<bool>());
                        for (var j = 0; j < _mapKeyboard[i].Count; j++)
                        {
                            visited[i].Add(false);
                        }
                    }
                    var bestPath = new List<string>();
                    BacktrackBestPathKeyboard(startY, startX, endY, endX,"",_mapKeyboard,visited, bestPath);
                    var old = new List<string>(bestPathKeyboard);
                    bestPathKeyboard.Clear();
                    foreach (var p in bestPath)
                    {
                        if (old.Count == 0)
                        {
                            bestPathKeyboard.Add(p);
                        }
                        else
                        {
                            foreach (var pp in old)
                            {
                                bestPathKeyboard.Add(pp + p);
                            }
                        }
                    }
                    current = node;
                }
                foreach(var p in bestPathKeyboard)
                {
                    Console.WriteLine(p);
                }
                Console.WriteLine("_____________________________________");

                current = 0;
                var allSecondPath = new List<string>(); 
                foreach (var bestPathK in bestPathKeyboard)
                {
                    var bestRobotKeyboard = new List<string>();
                    foreach (var bpK in bestPathK)
                    {
                        var node = _buttonToNode[bpK];
                        Dijkstra(current, node, _graphRobotKeyboard);
                        var startY = _mapRobotKeyboard.Select(l => l.Contains(current)).ToList().IndexOf(true);
                        var startX = _mapRobotKeyboard[startY].IndexOf(current);
                        var endY = _mapRobotKeyboard.Select(l => l.Contains(node)).ToList().IndexOf(true);
                        var endX = _mapRobotKeyboard[endY].IndexOf(node);
                        var visited = new List<List<bool>>();
                        for (var i = 0; i < _mapKeyboard.Count; i++)
                        {
                            visited.Add(new List<bool>());
                            for (var j = 0; j < _mapKeyboard[i].Count; j++)
                            {
                                visited[i].Add(false);
                            }
                        }
                        var bestPath = new List<string>();
                        BacktrackBestPathKeyboard(startY, startX, endY, endX, "", _mapRobotKeyboard, visited, bestPath);
                        current = node;
                        var old = new List<string>(bestRobotKeyboard);
                        bestRobotKeyboard.Clear();
                        foreach (var p in bestPath)
                        {
                            if (old.Count == 0)
                            {
                                bestRobotKeyboard.Add(p);
                            }
                            else
                            {
                                foreach (var pp in old)
                                {
                                    bestRobotKeyboard.Add(pp + p);
                                }
                            }
                        }
                    }
                    allSecondPath.AddRange(bestRobotKeyboard);
                }
                var minCount = allSecondPath.Min(s => s.Length);
                allSecondPath = allSecondPath.Where(s => s.Length == minCount).Distinct().ToList();


                current = 0;
                var allThirdPath = new List<string>();
                foreach (var secondPath in allSecondPath)
                {
                    var bestRobotKeyboard = new List<string>();
                    foreach (var sp  in secondPath)
                    {
                        var node = _buttonToNode[sp];
                        sum += Dijkstra(current, node, _graphRobotKeyboard) + 1;
                        var startY = _mapRobotKeyboard.Select(l => l.Contains(current)).ToList().IndexOf(true);
                        var startX = _mapRobotKeyboard[startY].IndexOf(current);
                        var endY = _mapRobotKeyboard.Select(l => l.Contains(node)).ToList().IndexOf(true);
                        var endX = _mapRobotKeyboard[endY].IndexOf(node);
                        var visited = new List<List<bool>>();
                        for (var i = 0; i < _mapKeyboard.Count; i++)
                        {
                            visited.Add(new List<bool>());
                            for (var j = 0; j < _mapKeyboard[i].Count; j++)
                            {
                                visited[i].Add(false);
                            }
                        }
                        var bestPath = new List<string>();
                        BacktrackBestPathKeyboard(startY, startX, endY, endX, "", _mapRobotKeyboard, visited, bestPath);
                        current = node;
                        var old = new List<string>(bestRobotKeyboard);
                        bestRobotKeyboard.Clear();
                        foreach (var p in bestPath)
                        {
                            if (old.Count == 0)
                            {
                                bestRobotKeyboard.Add(p);
                            }
                            else
                            {
                                foreach (var pp in old)
                                {
                                    bestRobotKeyboard.Add(pp + p);
                                }
                            }
                        }
                    }
                    allThirdPath.AddRange(bestRobotKeyboard);
                }

                minCount = allThirdPath.Min(s => s.Length);

                var s = minCount * code.Aggregate((sum, numb) =>
                {
                    if (numb == 10) return sum;
                    return sum * 10 + numb;
                });
                total += s;
                Console.WriteLine(s);
            }
            return total;
        }


        private void BacktrackBestPathKeyboard(int Y, int X, int endY, int endX, string path, List<List<int>> map, List<List<bool>> visited, List<string> bestKey)
        {
            if (map[Y][X] == -1 || visited[Y][X])
            {
                return;
            }
            visited[Y][X] = true;
            if (X == endX && Y == endY)
            {
                bestKey.Add(path + 'A');
                return;
            }
            else
            {
                if (X - 1 >= endX)
                {
                    BacktrackBestPathKeyboard(Y, X - 1, endY, endX, path + '<', map, visited, bestKey);
                    visited[Y][X - 1] = false;
                }
                if (Y + 1 <= endY)
                {
                    BacktrackBestPathKeyboard(Y + 1, X, endY, endX, path + 'v', map, visited, bestKey);
                    visited[Y + 1][X] = false;
                }
                if(X + 1 <= endX)
                {
                    BacktrackBestPathKeyboard(Y, X+1, endY, endX, path + '>', map, visited, bestKey);
                    visited[Y][X + 1] = false;
                }
                if (Y - 1 >= endY)
                {
                    BacktrackBestPathKeyboard(Y - 1, X, endY, endX, path + '^', map, visited, bestKey);
                    visited[Y - 1][X] = false;
                }
            }
        }

        private long Dijkstra(int start, int end, bool[,] graph)
        {
            var distances = new List<long>(graph.GetLength(0));
            var visited = new bool[graph.GetLength(0)];
            for (var i = 0; i < graph.GetLength(0); i++)
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
                    if (!visited[j] && graph[minIndex, j])
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
            //distmap = _mapKeyboard.Select(l => l.Select(i => i == -1 ? int.MaxValue : distances[i]).ToList()).ToList();
            return distances[end];
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            return 0;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(21);
            //var input = ReadFileUtils.ReadTestFile(21);
            _codes = new List<List<int>>();
            foreach (var code in input)
            {
                _codes.Add([.. code.Select(c =>
                {
                    if(c == 'A') return 10;
                    return int.Parse(c.ToString());
                })]);
            }

            _buttonToNode = new Dictionary<char, int>
            {
                { 'A', 0 },
                { '^', 1 },
                { 'v', 2 },
                { '<', 3 },
                { '>', 4 }
            };
            _graphRobotKeyboard = new bool[5, 5];
            _graphRobotKeyboard[0, 1] = true;
            _graphRobotKeyboard[0, 4] = true;
            _graphRobotKeyboard[1, 0] = true;
            _graphRobotKeyboard[1, 2] = true;
            _graphRobotKeyboard[2, 1] = true;
            _graphRobotKeyboard[2, 3] = true;
            _graphRobotKeyboard[2, 4] = true;
            _graphRobotKeyboard[3, 2] = true;
            _graphRobotKeyboard[4, 0] = true;
            _graphRobotKeyboard[4, 2] = true;

            _graphKeyboard = new bool[11, 11];
            _graphKeyboard[0, 10] = true;
            _graphKeyboard[0, 2] = true;
            _graphKeyboard[1, 2] = true;
            _graphKeyboard[1, 4] = true;
            _graphKeyboard[2, 0] = true;
            _graphKeyboard[2, 1] = true;
            _graphKeyboard[2, 3] = true;
            _graphKeyboard[2, 5] = true;
            _graphKeyboard[3, 2] = true;
            _graphKeyboard[3, 6] = true;
            _graphKeyboard[3, 10] = true;
            _graphKeyboard[4, 1] = true;
            _graphKeyboard[4, 5] = true;
            _graphKeyboard[4, 7] = true;
            _graphKeyboard[5, 2] = true;
            _graphKeyboard[5, 4] = true;
            _graphKeyboard[5, 6] = true;
            _graphKeyboard[5, 8] = true;
            _graphKeyboard[6, 3] = true;
            _graphKeyboard[6, 5] = true;
            _graphKeyboard[6, 9] = true;
            _graphKeyboard[7, 4] = true;
            _graphKeyboard[7, 8] = true;
            _graphKeyboard[8, 5] = true;
            _graphKeyboard[8, 7] = true;
            _graphKeyboard[8, 9] = true;
            _graphKeyboard[9, 6] = true;
            _graphKeyboard[9, 8] = true;
            _graphKeyboard[10, 0] = true;
            _graphKeyboard[10, 3] = true;
        }
    }
}

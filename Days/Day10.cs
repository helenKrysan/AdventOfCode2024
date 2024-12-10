using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day10 : IDay<int>
    {
        private List<List<int>> _map = new List<List<int>>();
        private Dictionary<int, List<(int, int)>> _positions = new Dictionary<int, List<(int, int)>>();

        public int Solve<T>() where T : ICollection<int>, IEnumerable<int>
        {
            List<List<T>> scores = new List<List<T>>();
            ReadInput(scores);
            var id = 0;
            foreach (var nine in _positions[9])
            {
                scores[nine.Item1][nine.Item2].Add(id);
                id++;
            }
            for (int i = 8; i >= 0; i--)
            {
                foreach (var number in _positions[i])
                {
                    var score = (T)Activator.CreateInstance(typeof(T));
                    if (number.Item1 + 1 < _map.Count && _map[number.Item1 + 1][number.Item2] == i + 1)
                    {
                        foreach (var s in scores[number.Item1 + 1][number.Item2])
                        {
                            score?.Add(s);
                        }
                    }
                    if (number.Item2 + 1 < _map[number.Item1].Count && _map[number.Item1][number.Item2 + 1] == i + 1)
                    {
                        foreach (var s in scores[number.Item1][number.Item2 + 1])
                        {
                            score?.Add(s);
                        }
                    }
                    if (number.Item1 - 1 >= 0 && _map[number.Item1 - 1][number.Item2] == i + 1)
                    {
                        foreach (var s in scores[number.Item1 - 1][number.Item2])
                        {
                            score?.Add(s);
                        }
                    }
                    if (number.Item2 - 1 >= 0 && _map[number.Item1][number.Item2 - 1] == i + 1)
                    {
                        foreach (var s in scores[number.Item1][number.Item2 - 1])
                        {
                            score?.Add(s);
                        }
                    }
                    scores[number.Item1][number.Item2] = score;
                }
            }
            return _positions[0].Sum(zero => scores[zero.Item1][zero.Item2].Count);
        }

        public int SolvePart1()
        {
            return Solve<HashSet<int>>();
        }

        public int SolvePart2()
        {
            return Solve<List<int>>();
        }

        public void ReadInput<T>(List<List<T>> scores) where T : ICollection<int>, IEnumerable<int>
        {
            var input = ReadFileUtils.ReadFile(10);
            _map = new List<List<int>>();
            _positions = new Dictionary<int, List<(int, int)>>();
            var _lineNumber = 0;
            foreach (var line in input)
            {
                scores.Add(new List<T>());
                var row = line.Select((c, i) =>
                {
                    scores[_lineNumber].Add((T)Activator.CreateInstance(typeof(T)));
                    var number = int.Parse(c.ToString());
                    if (_positions.ContainsKey(number))
                    {
                        _positions[number].Add((_lineNumber, i));
                    }
                    else
                    {
                        _positions.Add(number, new List<(int, int)> { (_lineNumber, i) });
                    }
                    return number;
                }).ToList();
                _map.Add(row);
                _lineNumber++;
            }

        }
    }
}
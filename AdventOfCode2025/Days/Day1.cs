using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Days
{
    internal class Day1 : IDay<int>
    {
        const int _startPoint = 50;
        const int _minPoint = 0;
        const int _maxPoint = 99;
        private List<(string, int)> _rotations;
        public async Task<int> SolvePart1Async()
        {
            await ReadInput();
            int currentPoint = _startPoint;
            int res = 0;
            foreach (var rotation in _rotations)
            {
                if (rotation.Item1 == "R")
                {
                    currentPoint += rotation.Item2;
                    while (currentPoint > _maxPoint)
                    {
                        currentPoint = _minPoint + (currentPoint - _maxPoint - 1);
                    }
                }
                else if (rotation.Item1 == "L")
                {
                    currentPoint -= rotation.Item2;
                    while (currentPoint < _minPoint)
                    {
                        currentPoint = _maxPoint - (_minPoint - currentPoint - 1);
                    }
                }
                if (currentPoint == 0)
                {
                    res++;
                }
            }
            return res;
        }

        public async Task<int> SolvePart2Async()
        {
            await ReadInput();
            int currentPoint = _startPoint;
            int res = 0;
            foreach (var rotation in _rotations)
            {
                if (rotation.Item1 == "R")
                {
                    currentPoint += rotation.Item2;
                    while (currentPoint > _maxPoint)
                    {
                        res++;
                        currentPoint = _minPoint + (currentPoint - _maxPoint - 1);
                    }
                }
                else if (rotation.Item1 == "L")
                {
                    currentPoint -= rotation.Item2;
                    while (currentPoint < _minPoint)
                    {
                        res++;
                        currentPoint = _maxPoint - (_minPoint - currentPoint - 1);
                    }
                }
            }
            return res;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(1);
            _rotations = new List<(string, int)>();
            foreach (var line in input)
            {
                var splited = line.Substring(1);

                _rotations.Add((line[0].ToString(), int.Parse(splited)));
            }

        }
    }
}
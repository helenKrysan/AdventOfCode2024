using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day8 : IDay<int>
    {
        private Dictionary<char, List<(int,int)>> _antennaLocations = new Dictionary<char, List<(int,int)>>();
        private int _maxY = 0;
        private int _maxX = 0;
        public int SolvePart1()
        {
            ReadInput();
            var antinodeLocations = new HashSet<(int,int)>();
            foreach (var antenna in _antennaLocations)
            {
                for(int i = 0; i < antenna.Value.Count; i++)
                {
                    for(int j = i + 1; j < antenna.Value.Count; j++)
                    {
                        var deltay = antenna.Value[i].Item1 - antenna.Value[j].Item1;
                        var deltax = antenna.Value[i].Item2 - antenna.Value[j].Item2;
                        var firstAntinode = (antenna.Value[i].Item1 + deltay, antenna.Value[i].Item2 + deltax);
                        if (firstAntinode.Item1 >= 0 &&
                            firstAntinode.Item1 < _maxY &&
                            firstAntinode.Item2 >= 0 &&
                            firstAntinode.Item2 < _maxX)
                        {
                            antinodeLocations.Add(firstAntinode);
                        }
                        var secondAntinode = (antenna.Value[j].Item1 - deltay, antenna.Value[j].Item2 - deltax);
                        if (secondAntinode.Item1 >= 0 &&
                            secondAntinode.Item1 < _maxY &&
                            secondAntinode.Item2 >= 0 &&
                            secondAntinode.Item2 < _maxX)
                        {
                            antinodeLocations.Add(secondAntinode);
                        }
                    }
                }
            }
            return antinodeLocations.Count;
        }

        public int SolvePart2()
        {
            ReadInput();
            var antinodeLocations = new HashSet<(int, int)>();
            foreach (var antenna in _antennaLocations)
            {
                for (int i = 0; i < antenna.Value.Count; i++)
                {
                    for (int j = i + 1; j < antenna.Value.Count; j++)
                    {
                        var deltay = antenna.Value[i].Item1 - antenna.Value[j].Item1;
                        var deltax = antenna.Value[i].Item2 - antenna.Value[j].Item2;
                        var firstAntinode = (antenna.Value[i].Item1, antenna.Value[i].Item2);
                        while (firstAntinode.Item1 >= 0 &&
                            firstAntinode.Item1 < _maxY &&
                            firstAntinode.Item2 >= 0 &&
                            firstAntinode.Item2 < _maxX)
                        {
                            antinodeLocations.Add(firstAntinode);
                            firstAntinode = (firstAntinode.Item1 + deltay, firstAntinode.Item2 + deltax);
                        }
                        var secondAntinode = (antenna.Value[j].Item1, antenna.Value[j].Item2);
                        while (secondAntinode.Item1 >= 0 &&
                            secondAntinode.Item1 < _maxY &&
                            secondAntinode.Item2 >= 0 &&
                            secondAntinode.Item2 < _maxX)
                        {
                            antinodeLocations.Add(secondAntinode);
                            secondAntinode = (secondAntinode.Item1 - deltay, secondAntinode.Item2 - deltax);
                        }
                    }
                }
            }
            return antinodeLocations.Count;
        }

        private void ReadInput()
        {
            var input = ReadFileUtils.ReadFile(8);
            _antennaLocations = new Dictionary<char, List<(int, int)>>();
            _maxX = input[0].Length;
            _maxY = input.Count;
            for(int i = 0; i < input.Count; i++)
            {
                var line = input[i];
                for(int j = 0; j < line.Length; j++)
                {
                    if (line[j] == '.')
                    {
                        continue;
                    }
                    if (!_antennaLocations.ContainsKey(line[j]))
                    {
                        _antennaLocations.Add(line[j], new List<(int, int)>());
                    }
                    _antennaLocations[line[j]].Add((i,j));
                }
            }
        }
    }
}

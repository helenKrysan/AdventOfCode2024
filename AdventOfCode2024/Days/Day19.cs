using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day19 : IDay<long>
    {
        private List<string> _towels;
        private List<string> _designs;
        private Dictionary<string, long> _cache = new Dictionary<string, long>();

        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            var sum = 0;
            foreach (var design in _designs)
            {
                if (IsMatch(design))
                {
                    sum++;
                }
            }
            return sum;
        }

        private bool IsMatch(string design)
        {
            if (string.IsNullOrWhiteSpace(design))
            {
                return true;
            }
            foreach (var towel in _towels)
            {
                if (design.StartsWith(towel))
                {
                    var rest = design.Substring(towel.Length);
                    if (IsMatch(rest))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            var sum = 0L;
            foreach (var design in _designs)
            {
                if (IsMatch(design))
                {
                    sum += AllMatchOld(design, 0L);
                }
            }
            return sum;
        }

        private long AllMatch(string design, long matches)
        {
            if (_cache.ContainsKey(design))
            {
                return _cache[design];
            }
            if (string.IsNullOrWhiteSpace(design))
            {
                return matches + 1;
            }
            foreach (var towel in _towels)
            {
                if (design.StartsWith(towel))
                {
                    var rest = design.Substring(towel.Length);
                    matches+= AllMatch(rest, 0L);
                }
            }
            if (matches > 0)
            {
                _cache.Add(design, matches);
            }
            return matches;
        }

        private long AllMatchOld(string design, long matches)
        {           
            if (string.IsNullOrWhiteSpace(design))
            {
                return matches + 1;
            }
            foreach (var towel in _towels)
            {
                if (design.StartsWith(towel))
                {
                    var rest = design.Substring(towel.Length);
                    if (_cache.ContainsKey(rest))
                    {
                        matches += _cache[rest];
                    }
                    else
                    {
                        matches = AllMatchOld(rest, matches);
                        _cache.Add(rest, matches);
                    }
                }
            }
            return matches;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(19);
            _towels = new List<string>();
            _designs = new List<string>();

            _towels = input[0].Split(", ").ToList();
            for (int i = 2; i < input.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(input[i]))
                {
                    _designs.Add(input[i]);
                }
            }
            _towels.Sort(new Comparison<string>((x, y) => y.Length.CompareTo(x.Length)));
        }
    }
}
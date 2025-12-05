
using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Days
{
    internal class Day5 : IDay<long>
    {
        private List<(long, long)> _ranges;
        private List<long> _ids;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            int res = 0;
            _ranges = _ranges.OrderBy(r => r.Item1).ToList();
            var mergedRanges = MergeRanges();
            foreach (var id in _ids)
            {
                bool inRange = false;
                foreach (var range in mergedRanges)
                {
                    if (id >= range.Item1 && id <= range.Item2)
                    {
                        inRange = true;
                        break;
                    }
                }
                if (inRange)
                {
                    res++;
                }
            }
            return res;
        }

        private List<(long,long)> MergeRanges()
        {
            var mergedRanges = new List<(long, long)>();
            foreach (var range in _ranges)
            {
                if (mergedRanges.Count == 0 || mergedRanges.Last().Item2 < range.Item1 - 1)
                {
                    mergedRanges.Add(range);
                }
                else
                {
                    var lastRange = mergedRanges.Last();
                    mergedRanges[mergedRanges.Count - 1] = (lastRange.Item1, Math.Max(lastRange.Item2, range.Item2));
                }
            }
            return mergedRanges;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            int res = 0;
            _ranges = _ranges.OrderBy(r => r.Item1).ToList();
            var mergedRanges = MergeRanges();

            long totalFreshIds = 0;
            foreach (var item in mergedRanges)
            {
                totalFreshIds += item.Item2 - item.Item1 + 1;
            }
            return totalFreshIds;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(5);
            _ranges = new List<(long, long)>();
            _ids = new List<long>();
            var isIds = false;
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line) || 
                    string.IsNullOrEmpty(line))
                {
                    isIds = true;
                    continue;
                }
                if (!isIds)
                {
                    var parts = line.Split('-');
                    var start = long.Parse(parts[0]);
                    var end = long.Parse(parts[1]);
                    _ranges.Add((start, end));
                }
                else
                {
                    _ids.Add(long.Parse(line));
                }

            }

        }
    }
}

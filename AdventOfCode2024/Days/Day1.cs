using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day1 : IDay<int>
    {
        public async Task<int> SolvePart1Async()
        {
            var input = await ReadFileUtils.ReadFileAsync(1);

            var first = new List<int>();
            var second = new List<int>();
            foreach (var line in input)
            {
                var splited = line.Split("   ");
                first.Add(int.Parse(splited.First()));
                second.Add(int.Parse(splited.Last()));
            }

            first.Sort();
            second.Sort();

            var distances = new List<int>();
            for (int i = 0; i < first.Count; i++)
            {
                distances.Add(Math.Abs(second[i] - first[i]));
            }

            return distances.Sum();
        }

        public async Task<int> SolvePart2Async()
        {
            var input = await ReadFileUtils.ReadFileAsync(1);

            var first = new List<int>();
            var second = new List<int>();
            foreach (var line in input)
            {
                var splited = line.Split("   ");
                first.Add(int.Parse(splited.First()));
                second.Add(int.Parse(splited.Last()));
            }

            var similar = new List<int>();
            for (int i = 0; i < first.Count; i++)
            {
                similar.Add(first[i] * second.FindAll(item => item == first[i]).Count);
            }

            return similar.Sum();
        }
    }
}

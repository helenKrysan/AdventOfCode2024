using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day11 : IDay<long>
    {
        private Dictionary<string, long> _stones;
        public async Task<long> SolvePart1Async()
        {
            return await Solve(25);
        }
        public async Task<long> Solve(int blinks)
        {
            await ReadInput();
            for (long i = 0; i < blinks; i++)
            {
                var newStones = new Dictionary<string, long>();
                foreach (var stone in _stones)
                {
                    if (stone.Key == "0")
                    {
                        AddStone(newStones, "1", stone.Value);
                    }
                    else if (stone.Key.Length % 2 == 0)
                    {
                        var originalStone = stone.Key;
                        var firstHalf = new string(originalStone.Substring(0, originalStone.Length / 2).SkipWhile(c => c == '0').ToArray());
                        var secondHalf = new string(originalStone.Substring(originalStone.Length / 2).SkipWhile(c => c == '0').ToArray());
                        firstHalf = firstHalf == "" ? "0" : firstHalf;
                        secondHalf = secondHalf == "" ? "0" : secondHalf;
                        AddStone(newStones, firstHalf, stone.Value);
                        AddStone(newStones, secondHalf, stone.Value);
                    }
                    else
                    {
                        var number = long.Parse(stone.Key);
                        var newKey = "" + number * 2024;
                        AddStone(newStones, newKey, stone.Value);
                    }
                }
                _stones = newStones;
            }
            return _stones.Sum(stone => stone.Value);
        }

        private void AddStone(Dictionary<string,long> stones, string key, long value)
        {
            if (stones.ContainsKey(key))
            {
                stones[key] += value;
            }
            else
            {
                stones.Add(key, value);
            }
        }

        public async Task<long> SolvePart2Async()
        {
            return await Solve(75);
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(11);
            _stones = new Dictionary<string, long>();
            var inputSplited = input[0].Split(' ');
            foreach (var item in inputSplited)
            {
                _stones.Add(item, 1);
            }
        }
    }
}

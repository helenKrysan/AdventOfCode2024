
using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Days
{
    internal class Day3 : IDay<long>
    {
        private List<List<int>> _batteries;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            int res = 0;
            foreach (var batterie in _batteries)
            {
                var maxBatterie = batterie.Max();
                var index = batterie.IndexOf(maxBatterie);
                if (index == batterie.Count - 1) // max is last
                {
                    var beforeMax = batterie.Take(index).ToList();
                    maxBatterie = beforeMax.Max();
                    index = batterie.IndexOf(maxBatterie);                   
                }
                var afterMax = batterie.Skip(index + 1).ToList();
                var nextMax = afterMax.Max();
                res += maxBatterie * 10 + nextMax;
            }
            return res;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            long res = 0;
            foreach (var batterie in _batteries)
            { 
                var currentBatterie = new List<int>(batterie);
                var changeCount = 12;
                long number = 0;
                while (changeCount > 0)
                {
                    var findMax = currentBatterie.Take(currentBatterie.Count - changeCount + 1).ToList();
                    var maxBatterie = findMax.Max();
                    var index = findMax.IndexOf(maxBatterie);
                    number += maxBatterie * (long)Math.Pow(10, changeCount - 1);
                    currentBatterie = currentBatterie.Skip(index + 1).ToList();
                    changeCount--;
                }
                res += number;
            }
            return res;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(3);
            _batteries = new List<List<int>>();
            foreach (var item in input)
            {
                var batterie = new List<int>();
                foreach (var item2 in item)
                {
                   batterie.Add(int.Parse(item2.ToString()));
                }
                _batteries.Add(batterie);
            }
        }
    }
}

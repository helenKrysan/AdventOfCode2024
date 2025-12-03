
using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Days
{
    internal class Day2 : IDay<long>
    {
        private List<(string, string)> _ids;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            long res = 0;
            foreach (var item in _ids)
            {
                var item1 = long.Parse(item.Item1);
                var item2 = long.Parse(item.Item2);
                for (long i = item1; i <= item2; i++)
                {
                    var current = i.ToString();
                    if (current.Length % 2 == 0)
                    {
                        var half1 = current.Substring(0, current.Length / 2);
                        var half2 = current.Substring(current.Length / 2);
                        if (half1 == half2)
                        {
                            res += i;
                        }
                    }
                }
            }
            return res;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            long res = 0;
            foreach (var item in _ids)
            {
                var item1 = long.Parse(item.Item1);
                var item2 = long.Parse(item.Item2);
                for (long i = item1; i <= item2; i++)
                {
                    var current = i.ToString();
                    var length = current.Length;
                    for (int j = 1; j <= length / 2; j++)
                    {   
                        var splitted = new List<string>();
                        for (int k = 0; k < length; k += j)
                        {
                            if (k + j <= length)
                            {
                                splitted.Add(current.Substring(k, j));
                            }
                            else
                            {
                                splitted.Add(current.Substring(k));
                            }
                        }
                        if (splitted.All(elem => elem == splitted[0]))
                        {
                            res += i;
                            break;
                        }
                    }
                }
            }
            return res;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(2);
            var idsRange = input[0].Split(",");
            _ids = new List<(string, string)>();
            foreach (var idRange in idsRange)
            {
                var ids = idRange.Split("-");
                _ids.Add((ids[0], ids[1]));
            }
        }
    }
}

using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day7 : IDay<long>
    {
        Dictionary<long, List<long>> _equations;
        public long SolvePart1()
        {
            ReadInput();
            var sum = 0L;
            foreach (var equation in _equations)
            {
                var result = Calculate(equation.Key, equation.Value, 0, 0, false);
                if (result)
                {
                    sum += equation.Key;
                }
            }
            return sum;
        }
        private bool Calculate(long key, List<long> equation, long sum, int currpos, bool isPart2)
        {
            if (equation.Count == currpos)
            {
                return sum == key;
            }
            var resultPlus = Calculate(key, equation, sum + equation[currpos], currpos + 1, isPart2);
            if (resultPlus)
            {
                return resultPlus;
            }
            var resultMul = Calculate(key, equation, sum * equation[currpos], currpos + 1, isPart2);
            if (resultMul)
            {
                return resultMul;
            }
            if (isPart2)
            {
                var resultConcat = Calculate(key, equation, long.Parse(sum.ToString() + equation[currpos]), currpos + 1, isPart2);
                if (resultConcat)
                {
                    return resultConcat;
                }
            }
            return false;
        }

        public long SolvePart2()
        {
            ReadInput();
            var sum = 0L;
            foreach (var equation in _equations)
            {
                var result = Calculate(equation.Key, equation.Value, 0, 0, true);
                if (result)
                {
                    sum += equation.Key;
                }
            }
            return sum;
        }

        private void ReadInput()
        {
            var input = ReadFileUtils.ReadFile(7);
            _equations = new Dictionary<long, List<long>>();
            foreach (var line in input)
            {
                var splited = line.Split(":");
                var key = long.Parse(splited.First());
                var equation = new List<long>();
                foreach (var item in splited.Last().Trim().Split(" "))
                {
                    equation.Add(long.Parse(item));
                }
                _equations.Add(key, equation);
            }
        }
    }
}

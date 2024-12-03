using AdventOfCode2024.Utils;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days
{
    internal class Day3 : IDay<int>
    {
        public int SolvePart1()
        {
            var input = ReadFileUtils.ReadFile(3);
            var pattern = "mul\\(\\d*\\,\\d*\\)";
            var sum = 0;
            var matches = Regex.Matches(input[0], pattern).Select(result => result.Value);
            foreach (var match in matches)
            {
                var splitted = match.Substring(4, match.Length - 5).Split(",");
                sum += int.Parse(splitted[0]) * int.Parse(splitted[1]);
            }
            return sum;
        }

        public int SolvePart2()
        {
            var input = ReadFileUtils.ReadFile(3);
            var pattern = "mul\\(\\d*\\,\\d*\\)";
            var preprocessed = PreprocessDo(input[0]);
            var sum = 0;
            for (int i = 0; i < preprocessed.Count; i++)
            {
                var matches = Regex.Matches(preprocessed[i], pattern).Select(result => result.Value);
                foreach (var match in matches)
                {
                    var splitted = match.Substring(4, match.Length - 5).Split(",");
                    sum += int.Parse(splitted[0]) * int.Parse(splitted[1]);
                }
            }
            return sum;
        }

        private List<string> PreprocessDo(string input) {
            var preprocessed = new List<string>();
            var dontpart = input.Split("don't()").ToList();
            preprocessed.Add(dontpart[0]);
            for(int i = 1; i < dontpart.Count; i++)
            {
                var part = dontpart[i];
                if (part.Contains("do()"))
                {
                    var indexdo = part.IndexOf("do()");
                    var dopart = part.Substring(indexdo);
                    preprocessed.Add(dopart);
                }
            }
            return preprocessed;
        }
    }
}

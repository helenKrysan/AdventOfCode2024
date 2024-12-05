using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    public class PageComparer(Dictionary<int, List<int>> rules) : IComparer<int>
    {
        private readonly Dictionary<int, List<int>> rules = rules;

        public int Compare(int x, int y)
        {
            if (rules.ContainsKey(x))
            {
                if (rules[x].Contains(y))
                {
                    return 1;
                }
            }
            if (rules.ContainsKey(y))
            {
                if (rules[y].Contains(x))
                {
                    return -1;
                }
            }
            return 0;
        }
    }

    internal class Day5 : IDay<int>
    {
        Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>();
        List<List<int>> pages = new List<List<int>>();
        public int SolvePart1()
        {
            ReadInput();
            var result = 0;
            foreach (var page in pages)
            {
                if (IsCorrectOrder(page))
                {
                    result += page[(page.Count - 1) / 2];
                }
            }
            return result;
        }    

        public int SolvePart2()
        {
            ReadInput();
            var result = 0;
            foreach (var page in pages)
            {
                if (!IsCorrectOrder(page))
                {
                    page.Sort(new PageComparer(rules));
                    result += page[(page.Count - 1) / 2];
                }
            }
            return result;
        }

        private bool IsCorrectOrder(List<int> page)
        {
            var processed = new List<int>();
            for (int i = 0; i < page.Count; i++)
            {
                if (rules.ContainsKey(page[i]))
                {
                    foreach (var p in processed)
                    {
                        if (rules[page[i]].Contains(p))
                        {
                            return false;
                        }
                    }

                }
                processed.Add(page[i]);
            }
            return true;
        }

        private void ReadInput()
        {
            var input = ReadFileUtils.ReadFile(5);
            rules = new Dictionary<int, List<int>>();
            pages = new List<List<int>>();
            var mode = 0;
            foreach (var line in input)
            {
                if (line == "")
                {
                    mode = 1;
                    continue;
                }
                if (mode == 0)
                {
                    var splitted = line.Split("|");
                    var first = int.Parse(splitted[0]);
                    var second = int.Parse(splitted[1]);
                    if (!rules.ContainsKey(first))
                    {
                        rules.Add(first, new List<int>());

                    }
                    if (!rules[first].Contains(second))
                    {
                        rules[first].Add(second);
                    }
                }
                else
                {
                    var numbers = line.Split(",");
                    pages.Add(new List<int>());
                    foreach (var n in numbers)
                    {
                        pages[pages.Count - 1].Add(int.Parse(n));
                    }
                }
            }
        }
    }
}

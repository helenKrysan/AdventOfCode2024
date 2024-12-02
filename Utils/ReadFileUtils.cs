namespace AdventOfCode2024.Utils
{
    internal static class ReadFileUtils
    {
        public static List<string> ReadFile(int day)
        {
            var path = $"Input/Day{day}.txt";
            return [.. File.ReadAllLines(path)];
        }
    }
}

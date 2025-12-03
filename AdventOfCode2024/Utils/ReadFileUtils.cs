using System.Text.Json;

namespace AdventOfCode2024.Utils
{
    internal static class ReadFileUtils
    {
        public static List<string> ReadTestFile(int day)
        {
            var path = $"Input/Day{day}.txt";
            return [.. File.ReadAllLines(path)];
        }

        public static async Task<List<string>> ReadFileAsync(int day)
        {
            var httpClient = new HttpClient();
            var configStream = File.OpenRead("config.json");
            var config = JsonSerializer.Deserialize<Config>(configStream);
            httpClient.DefaultRequestHeaders.Add("Cookie", $"session={config?.SessionKey}");
            using var response = await httpClient.GetAsync($"https://adventofcode.com/2024/day/{day}/input");
            var content = await response.Content.ReadAsStringAsync();
            return [.. content.Trim().Split("\n")];
        }
    }
}

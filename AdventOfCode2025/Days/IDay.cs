namespace AdventOfCode2025.Days
{
    internal interface IDay<T>
    {
        Task<T> SolvePart1Async();
        Task<T> SolvePart2Async();
    }
}

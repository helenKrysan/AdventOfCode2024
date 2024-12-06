using AdventOfCode2024.Utils;
using System;

namespace AdventOfCode2024.Days
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Guard()
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }
    }
    internal class Day6 : IDay<int>
    {
        private List<List<char>> _map = new List<List<char>>();
        public int SolvePart1()
        {
            var guard = ReadInput();
            while (true)
            {
                _map[guard.Y][guard.X] = 'X';
                if (guard.X == 0 || guard.Y == 0 || guard.Y == _map.Count - 1 || guard.X == _map[0].Count - 1)
                {
                    break;
                }
                var moved = TryMove(guard);
                if (!moved)
                {
                    TurnRight(guard);
                }
            }
            return _map.Sum(line => line.Where(x => x == 'X').Count());
        }

        private void TurnRight(Guard guard)
        {
            switch (guard.Direction)
            {
                case Direction.Up:
                    {
                        guard.Direction = Direction.Right;
                        break;
                    }
                case Direction.Down:
                    {
                        guard.Direction = Direction.Left;
                        break;
                    }
                case Direction.Left:
                    {
                        guard.Direction = Direction.Up;
                        break;
                    }
                case Direction.Right:
                    {
                        guard.Direction = Direction.Down;
                        break;
                    }
            }
        }

        private bool TryMove(Guard guard)
        {
            switch (guard.Direction)
            {
                case Direction.Up:
                    {
                        if (_map[guard.Y - 1][guard.X] == '#')
                        {
                            return false;
                        }
                        guard.Y--;
                        break;
                    }
                case Direction.Down:
                    {
                        if (_map[guard.Y + 1][guard.X] == '#')
                        {
                            return false;
                        }
                        guard.Y++;
                        break;
                    }
                case Direction.Left:
                    {
                        if (_map[guard.Y][guard.X - 1] == '#')
                        {
                            return false;
                        }
                        guard.X--;
                        break;
                    }
                case Direction.Right:
                    {
                        if (_map[guard.Y][guard.X + 1] == '#')
                        {
                            return false;
                        }
                        guard.X++;
                        break;
                    }
            }
            return true;
        }

        public int SolvePart2()
        {
            var guard = ReadInput();
            var count = 0;
            var uniqueObstacle = new HashSet<(int, int)>();
            while (true)
            {
                if (guard.X == 0 || guard.Y == 0 || guard.Y == _map.Count - 1 || guard.X == _map[0].Count - 1)
                {
                    break;
                }
                var lastX = guard.X;
                var lastY = guard.Y;
                var moved = TryMove(guard);
                if (!moved)
                {
                    TurnRight(guard);
                }
                else
                {
                    var isAdded = uniqueObstacle.Add((guard.X, guard.Y));
                    if (isAdded)
                    {
                        //step back
                        var newGuard = new Guard()
                        {
                            X = lastX,
                            Y = lastY,
                            Direction = guard.Direction
                        };
                        _map[guard.Y][guard.X] = '#';
                        var isLoop = CheckLoop(newGuard);
                        if (isLoop)
                        {
                            count++;
                        }
                        _map[guard.Y][guard.X] = '.';
                    }
                }
            }
            return count;
        }

        private bool CheckLoop(Guard guard)
        {
            var visited = new HashSet<(int, int, Direction)>();
            while (true)
            {
                var added = visited.Add((guard.X, guard.Y, guard.Direction));
                if (!added)
                {
                    return true;
                }
                if (guard.X == 0 || guard.Y == 0 || guard.Y == _map.Count - 1 || guard.X == _map[0].Count - 1)
                {
                    return false;
                }
                var moved = TryMove(guard);
                if (!moved)
                {
                    TurnRight(guard);
                }
            }
        }

        private Dictionary<char, Direction> available =
            new Dictionary<char, Direction>()
            {
                { '^', Direction.Up },
                { 'v', Direction.Down },
                { '<', Direction.Left },
                { '>', Direction.Right }
            };

        private Guard ReadInput()
        {
            var input = ReadFileUtils.ReadFile(6);
            _map = new List<List<char>>();
            var guard = new Guard() { };
            foreach (var line in input)
            {
                _map.Add(line.ToCharArray().ToList());
            }
            for (int i = 0; i < _map.Count; i++)
            {
                for (int j = 0; j < _map[i].Count; j++)
                {
                    if (available.ContainsKey(_map[i][j]))
                    {
                        guard.X = j;
                        guard.Y = i;
                        guard.Direction = available[_map[i][j]];
                        break;
                    }
                }
            }
            return guard;
        }
    }
}

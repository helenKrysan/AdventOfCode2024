using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    public class Submarine
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    internal class Day15 : IDay<long>
    {
        private List<List<char>> _map;
        private List<Direction> _moves;
        private Submarine _submarine;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            foreach (var move in _moves)
            {
                TryMove(move, _submarine.X, _submarine.Y);
            }
            var sum = 0;
            for (int i = 0; i < _map.Count; i++)
            {
                for (int j = 0; j < _map[i].Count; j++)
                {
                    if (_map[i][j] == 'O')
                    {
                        sum += i * 100 + j;
                    }
                }
            }
            return sum;
        }

        private bool TryMove(Direction move, int X, int Y)
        {
            switch (_map[Y][X])
            {
                case '.':
                    return true;
                case '#':
                    return false;
                default:
                    {
                        switch (move)
                        {
                            case Direction.Right:
                                {
                                    var isMoved = TryMove(Direction.Right, X + 1, Y);
                                    if (isMoved)
                                    {
                                        _map[Y][X + 1] = _map[Y][X];
                                        if (_map[Y][X] == '@')
                                        {
                                            _map[Y][X] = '.';
                                            _submarine.X = X + 1;
                                        }
                                    }
                                    return isMoved;
                                }
                            case Direction.Left:
                                {
                                    var isMoved = TryMove(Direction.Left, X - 1, Y);
                                    if (isMoved)
                                    {
                                        _map[Y][X - 1] = _map[Y][X];
                                        if (_map[Y][X] == '@')
                                        {
                                            _map[Y][X] = '.';
                                            _submarine.X = X - 1;
                                        }
                                    }
                                    return isMoved;
                                }
                            case Direction.Up:
                                {
                                    var isMoved = TryMove(Direction.Up, X, Y - 1);
                                    if (isMoved)
                                    {
                                        _map[Y - 1][X] = _map[Y][X];
                                        if (_map[Y][X] == '@')
                                        {
                                            _map[Y][X] = '.';
                                            _submarine.Y = Y - 1;
                                        }
                                    }
                                    return isMoved;
                                }
                            case Direction.Down:
                                {
                                    var isMoved = TryMove(Direction.Down, X, Y + 1);
                                    if (isMoved)
                                    {
                                        _map[Y + 1][X] = _map[Y][X];
                                        if (_map[Y][X] == '@')
                                        {
                                            _map[Y][X] = '.';
                                            _submarine.Y = Y + 1;
                                        }
                                    }
                                    return isMoved;
                                }
                        }
                        break;
                    }
            }
            return false;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            var wideMap = new List<List<char>>();
            foreach (var line in _map)
            {
                var newLine = new List<char>();
                line.Select(c =>
                {
                    var s = "";
                    if (c == '@')
                    {
                        s = "@.";
                    }
                    else if (c == 'O')
                    {
                        s = "[]";
                    }
                    else
                    {
                        s = "" + c + c;
                    }
                    newLine.AddRange(s);
                    return s;
                }).ToList();
                wideMap.Add(newLine);
            }
            _submarine.X *= 2;
            _map = wideMap;


            foreach (var move in _moves)
            {
                _goodsToMove = new HashSet<(int, int)>();
                var isMoved = TryMove2(move, _submarine.X, _submarine.Y);
                if(isMoved && move == Direction.Up)
                {
                    var sorted = _goodsToMove.ToList();
                    sorted.Sort(new Comparison<(int, int)>((a, b) =>
                    {
                        return a.Item1.CompareTo(b.Item1);
                    }));
                    foreach (var item in sorted)
                    {
                        _map[item.Item1 - 1][item.Item2] = _map[item.Item1][item.Item2];
                        _map[item.Item1][item.Item2] = '.';
                    }
                    _submarine.Y--;
                }
                else if (isMoved && move == Direction.Down)
                {
                    var sorted = _goodsToMove.ToList();
                    sorted.Sort(new Comparison<(int, int)>((a, b) =>
                    {
                        return a.Item1.CompareTo(b.Item1)*-1;
                    }));
                    foreach (var item in sorted)
                    {
                        _map[item.Item1 + 1][item.Item2] = _map[item.Item1][item.Item2];
                        _map[item.Item1][item.Item2] = '.';
                    }
                    _submarine.Y++;
                }
            }
            var sum = 0;
            for (int i = 0; i < _map.Count; i++)
            {
                for (int j = 0; j < _map[i].Count; j++)
                {
                    if (_map[i][j] == '[')
                    {
                        sum += i * 100 + j;
                    }
                }
            }
            return sum;
        }

        private HashSet<(int, int)> _goodsToMove = new HashSet<(int, int)>();
        private bool TryMove2(Direction move, int X, int Y)
        {
            switch (_map[Y][X])
            {
                case '.':
                    return true;
                case '#':
                    return false;
                default:
                    {
                        switch (move)
                        {
                            case Direction.Right:
                                {
                                    var isMoved = TryMove2(Direction.Right, X + 1, Y);
                                    if (isMoved)
                                    {
                                        _map[Y][X + 1] = _map[Y][X];
                                        if (_map[Y][X] == '@')
                                        {
                                            _map[Y][X] = '.';
                                            _submarine.X = X + 1;
                                        }
                                    }
                                    return isMoved;
                                }
                            case Direction.Left:
                                {
                                    var isMoved = TryMove2(Direction.Left, X - 1, Y);
                                    if (isMoved)
                                    {
                                        _map[Y][X - 1] = _map[Y][X];
                                        if (_map[Y][X] == '@')
                                        {
                                            _map[Y][X] = '.';
                                            _submarine.X = X - 1;
                                        }
                                    }
                                    return isMoved;
                                }
                            case Direction.Up:
                                {
                                    var isMoved = false;
                                    if (_map[Y][X] == '@')
                                    {
                                        isMoved = TryMove2(Direction.Up, X, Y - 1);
                                    }
                                    else if (_map[Y][X] == '[')
                                    {
                                        isMoved = TryMove2(Direction.Up, X, Y - 1) && TryMove2(Direction.Up, X + 1, Y - 1);
                                        _goodsToMove.Add((Y, X + 1));
                                    }
                                    else if (_map[Y][X] == ']')
                                    {
                                        isMoved = TryMove2(Direction.Up, X, Y - 1) && TryMove2(Direction.Up, X - 1, Y - 1);
                                        _goodsToMove.Add((Y, X - 1));
                                    }
                                    _goodsToMove.Add((Y, X));
                                    return isMoved;
                                }
                            case Direction.Down:
                                {
                                    var isMoved = false;
                                    if (_map[Y][X] == '@')
                                    {
                                        isMoved = TryMove2(Direction.Down, X, Y + 1);
                                    }
                                    else if (_map[Y][X] == '[')
                                    {
                                        isMoved = TryMove2(Direction.Down, X, Y + 1) && TryMove2(Direction.Down, X + 1, Y + 1);
                                        _goodsToMove.Add((Y, X + 1));
                                    }
                                    else if (_map[Y][X] == ']')
                                    {
                                        isMoved = TryMove2(Direction.Down, X, Y + 1) && TryMove2(Direction.Down, X - 1, Y + 1);
                                        _goodsToMove.Add((Y, X - 1));
                                    }
                                    _goodsToMove.Add((Y, X));
                                    return isMoved;
                                }
                        }
                        break;
                    }
            }
            return false;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(15);
            _map = new List<List<char>>();
            _moves = new List<Direction>();
            var isMap = true;
            foreach (var line in input)
            {
                if (line == "")
                {
                    isMap = false;
                    continue;
                }
                if (isMap)
                {
                    if (line.Contains('@'))
                    {
                        _submarine = new Submarine
                        {
                            X = line.IndexOf('@'),
                            Y = _map.Count
                        };
                    }
                    _map.Add(line.ToList());
                }
                else
                {
                    foreach (var c in line)
                    {
                        if (c == '>' || c == '<' || c == '^' || c == 'v')
                        {
                            _moves.Add(c switch
                            {
                                '>' => Direction.Right,
                                '<' => Direction.Left,
                                '^' => Direction.Up,
                                'v' => Direction.Down
                            });
                        }
                    }
                }

            }
        }
    }
}

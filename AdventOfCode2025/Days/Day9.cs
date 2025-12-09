using AdventOfCode2025.Utils;
using System.Data;

namespace AdventOfCode2025.Days
{
    internal class Day9 : IDay<long>
    {
        private List<(int, int)> _rectangles = new List<(int, int)>();
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            var square = new List<long>();
            foreach (var rect in _rectangles)
            {
                foreach (var otherRect in _rectangles)
                {
                    if (rect != otherRect)
                    {
                        long width = (long)Math.Max(rect.Item1, otherRect.Item1) - (long)Math.Min(rect.Item1, otherRect.Item1) + 1;
                        long height = (long)Math.Max(rect.Item2, otherRect.Item2) - (long)Math.Min(rect.Item2, otherRect.Item2) + 1;
                        square.Add(width * height);
                    }
                }
            }
            square.Sort();
            square.Reverse();
            return square.First();
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            CreateGrid();
            var square = new List<long>();
            foreach (var rect in _rectangles)
            {
                foreach (var otherRect in _rectangles)
                {
                    if (rect != otherRect)
                    {
                        if (CheckIfAllowed(rect, otherRect) == false)
                        {
                            continue;
                        }
                        long width = (long)Math.Max(rect.Item1, otherRect.Item1) - (long)Math.Min(rect.Item1, otherRect.Item1) + 1;
                        long height = (long)Math.Max(rect.Item2, otherRect.Item2) - (long)Math.Min(rect.Item2, otherRect.Item2) + 1;
                        square.Add(width * height);
                    }
                }
            }
            square.Sort();
            square.Reverse();
            return square.First();
        }

        private bool CheckIfAllowed((int, int) r1, (int, int) r2)
        {
            int top = Math.Min(r1.Item2, r2.Item2);
            int bottom = Math.Max(r1.Item2, r2.Item2);
            int left = Math.Min(r1.Item1, r2.Item1);
            int right = Math.Max(r1.Item1, r2.Item1);
            //check top
            bool result = CheckHorizontal(top, left, right);
            if (result == false)
            {
                return false;
            }
            //check bottom
            result = CheckHorizontal(bottom, left, right);
            if (result == false)
            {
                return false;
            }
            //check left
            result = CheckVertical(left, top, bottom);
            if (result == false)
            {
                return false;
            }
            //check right
            result = CheckVertical(right, top, bottom);
            if (result == false)
            {
                return false;
            }
            return true;
        }
        private bool CheckVertical(int column, int top, int bottom)
        {
            for (int r = top; r <= bottom; r++)
            {
                if (_filledgrid[r][column] == '.')
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckHorizontal(int row, int left, int right)
        {
            for (int c = left; c <= right; c++)
            {
                if (_filledgrid[row][c] == '.')
                {
                    return false;
                }
            }
            return true;
        }

        List<List<char>> _grid = new List<List<char>>();
        List<List<char>> _filledgrid = new List<List<char>>();
        private void CreateGrid()
        {
            var maxColumn = _rectangles.Max(r => r.Item1);
            var maxRow = _rectangles.Max(r => r.Item2);
            for (int r = 0; r <= maxRow + 1; r++)
            {
                var row = new List<char>(maxColumn + 1);
                row.AddRange(Enumerable.Repeat('.', maxColumn + 1));
                _grid.Add(row);
                _filledgrid.Add(new List<char>(row));
            }
            var lastRec = _rectangles.First();
            _rectangles.Add(lastRec);
            foreach (var rect in _rectangles)
            {
                _grid[rect.Item2][rect.Item1] = '#';
                _filledgrid[lastRec.Item2][lastRec.Item1] = '#';
                if (rect.Item1 == lastRec.Item1)
                {
                    if (lastRec.Item2 > rect.Item2)
                    {
                        for (int r = rect.Item2; r < lastRec.Item2; r++)
                        {
                            _grid[r][rect.Item1] = '#';
                            _filledgrid[r][rect.Item1] = '#';
                        }
                    }
                    else if (rect.Item2 > lastRec.Item2)
                    {
                        for (int r = lastRec.Item2; r < rect.Item2; r++)
                        {
                            _grid[r][rect.Item1] = '#';
                            _filledgrid[r][rect.Item1] = '#';
                        }
                    }

                }
                else if (rect.Item2 == lastRec.Item2)
                {
                    if (lastRec.Item1 > rect.Item1)
                    {
                        for (int r = rect.Item1; r < lastRec.Item1; r++)
                        {
                            _grid[rect.Item2][r] = '#';
                            _filledgrid[rect.Item2][r] = '#';
                        }
                    }
                    else if (rect.Item1 > lastRec.Item1)
                    {
                        for (int r = lastRec.Item1; r < rect.Item1; r++)
                        {
                            _grid[rect.Item2][r] = '#';
                            _filledgrid[rect.Item2][r] = '#';
                        }
                    }
                }
                lastRec = rect;
            }
            FillGrid();
        }

        private void FillGrid()
        {
            for (int r = 0; r < _grid.Count; r++)
            {
                bool inside = false;
                bool isRib = false;
                bool enteredUp = false;
                for (int c = 0; c < _grid[r].Count; c++)
                {
                    if (_grid[r][c] == '#')
                    {
                        if (c + 1 < _grid[r].Count && _grid[r][c + 1] == '#')
                        {
                            isRib = true;
                            if (_grid[r][c - 1] == '.' && _grid[r - 1][c] == '#')
                            {
                                enteredUp = true;
                            }
                            continue;
                        }
                        else if (isRib)
                        {
                            isRib = false;
                            if (_grid[r - 1][c] == '#')
                            {
                                if (!enteredUp)
                                {
                                    inside = !inside;
                                }
                            }
                            if(_grid[r + 1][c] == '#')
                            {
                                if (enteredUp)
                                {
                                    inside = !inside;
                                }

                            }
                            enteredUp = false;
                        }
                        else
                        {
                            inside = !inside;
                        }
                    }
                    else if (inside)
                    {
                        _filledgrid[r][c] = '#';
                    }
                }
            }
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(9);

            //var input = ReadFileUtils.ReadTestFile(9);
            _rectangles = new List<(int, int)>();
            foreach (var line in input)
            {
                var splitted = line.Split(',');
                var column = int.Parse(splitted[0]);
                var row = int.Parse(splitted[1]);
                _rectangles.Add((column, row));
            }
        }
    }
}
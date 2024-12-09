using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day9 : IDay<long>
    {
        private string _diskMap;
        public long SolvePart1()
        {
            ReadInput();
            var sum = 0L;
            var blockWithSpaces = new List<int>();
            var isFreeSpace = false;
            var currentId = 0;
            for (var i = 0; i < _diskMap.Length; i++)
            {
                var iNumber = int.Parse(_diskMap[i].ToString());
                for (int j = 0; j < iNumber; j++)
                {
                    blockWithSpaces.Add(isFreeSpace ? -1 : currentId);
                }
                if(!isFreeSpace)
                {
                    currentId++;
                }
                isFreeSpace = !isFreeSpace;
            }
            var lastNotDot = blockWithSpaces.Count - 1;
            for (var i = 0; i < blockWithSpaces.Count; i++)
            {
                if (lastNotDot <= i)
                {
                    break;
                }
                if (blockWithSpaces[i] == -1)
                {
                    while (lastNotDot > i && blockWithSpaces[lastNotDot] == -1)
                    {
                        lastNotDot--;
                    }
                    blockWithSpaces[i] = blockWithSpaces[lastNotDot];
                    blockWithSpaces[lastNotDot] = -1;
                }
            }
            for (var i = 0; i < blockWithSpaces.Count; i++)
            {
                if (blockWithSpaces[i] == -1)
                {
                    break;
                }
                sum += blockWithSpaces[i] * i;
            }
            return sum;
        }

        public long SolvePart2()
        {
            ReadInput();
            var sum = 0L;
            var blockWithSpaces = new List<int>();
            var isFreeSpace = false;
            var currentId = 0;
            for (var i = 0; i < _diskMap.Length; i++)
            {
                var iNumber = int.Parse(_diskMap[i].ToString());
                if (isFreeSpace)
                {
                    for (int j = 0; j < iNumber; j++)
                    {
                        blockWithSpaces.Add(-1 * iNumber);
                    }
                }
                else
                {
                    for (int j = 0; j < iNumber; j++)
                    {
                        blockWithSpaces.Add(currentId);
                    }
                    currentId++;
                }
                isFreeSpace = !isFreeSpace;
            }
            var possibleSpace = 0;
            var lastNotDot = blockWithSpaces.Count - 1;
            var numberToSwap = 0;
            while (lastNotDot > 0)
            {
                while (blockWithSpaces[lastNotDot] < 0)
                {
                    lastNotDot--;
                }
                var current = blockWithSpaces[lastNotDot];
                while (blockWithSpaces[lastNotDot] == current && lastNotDot > 0)
                {
                    lastNotDot--;
                    numberToSwap++;
                }
                while (lastNotDot > possibleSpace)
                {
                    if (blockWithSpaces[possibleSpace] < 0)
                    {
                        var size = 0 - blockWithSpaces[possibleSpace];
                        if (size >= numberToSwap)
                        {
                            while (numberToSwap > 0)
                            {
                                blockWithSpaces[possibleSpace] = current;
                                blockWithSpaces[lastNotDot+numberToSwap] = -1;
                                possibleSpace++;
                                numberToSwap--;
                                size--;
                            }
                            for(int i = size; i > 0; i--)
                            {
                                blockWithSpaces[possibleSpace] = -1*size;
                                possibleSpace++;
                            }
                            break;
                        }
                    }
                    possibleSpace++;
                }
                possibleSpace = 0;
                numberToSwap = 0;
            }
            for (var i = 0; i < blockWithSpaces.Count; i++)
            {
                if (blockWithSpaces[i] < 0)
                {
                    continue;
                }
                sum += blockWithSpaces[i] * i;
            }
            return sum;
        }

        private void ReadInput()
        {
            var input = ReadFileUtils.ReadFile(9);
            _diskMap = input[0];
        }
    }
}

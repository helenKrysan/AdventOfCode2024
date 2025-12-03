
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day25 : IDay<long>
    {
        private Dictionary<int, List<int>> _keys;
        private Dictionary<int, List<int>> _locks;

        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            var pairCount = 0;
            foreach (var key in _keys)
            {
                foreach (var _lock in _locks)
                {
                    var isPair = true;
                    for (var i = 0; i < key.Value.Count; i++)
                    {
                        if (key.Value[i] + _lock.Value[i] > 5)
                            {
                                isPair = false;
                                break;
                            }
                    }
                    if (isPair)
                    {
                        pairCount++;
                    }
                }
            }
            return pairCount;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            return 0;
        }

        public async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(25);
            _keys = new Dictionary<int, List<int>>();
            _locks = new Dictionary<int, List<int>>();
            var elementEnded = false;
            var elementBlock = new List<List<char>>();
            var isLock = false;
            var keyId = 0;
            var lockId = 0;
            var heights = new List<int>();
            foreach (var line in input)
            {
                if (line.Trim() == "")
                {
                    elementEnded = true;
                }
                if (elementEnded)
                {
                    heights = new List<int>();
                    if(elementBlock[0][0] == '#')
                    {
                        isLock = true;
                    }

                    //pasrse block
                    if (isLock)
                    {
                        heights.AddRange(Enumerable.Repeat(0, elementBlock[0].Count));
                        for(int row = 1; row < elementBlock.Count - 1; row++)
                        {
                            for (var i = 0; i < elementBlock[row].Count; i++)
                            {
                                if (elementBlock[row][i] == '#')
                                {
                                    heights[i]++;
                                }
                            }
                        }
                        _locks.Add(lockId, heights);
                        lockId++;
                    }
                    else
                    {
                        heights.AddRange(Enumerable.Repeat(0, elementBlock[0].Count));
                        for (int row = elementBlock.Count - 2; row > 0; row--)
                        {
                            for (var i = 0; i < elementBlock[row].Count; i++)
                            {
                                if (elementBlock[row][i] == '#')
                                {
                                    heights[i]++;
                                }
                            }
                        }
                        _keys.Add(keyId, heights);
                        keyId++;
                    }
                    elementBlock = new List<List<char>>();
                    isLock = false;
                    elementEnded = false;
                }
                else
                {
                    elementBlock.Add([.. line.ToCharArray()]);
                }
            }
            //add last element
            if (elementBlock[0][0] == '#')
            {
                isLock = true;
            }
            heights = new List<int>();
            //pasrse block
            if (isLock)
            {
                heights.AddRange(Enumerable.Repeat(0, elementBlock[0].Count));
                for (int row = 1; row < elementBlock.Count - 1; row++)
                {
                    for (var i = 0; i < elementBlock[row].Count; i++)
                    {
                        if (elementBlock[row][i] == '#')
                        {
                            heights[i]++;
                        }
                    }
                }
                _locks.Add(lockId, heights);
                lockId++;
            }
            else
            {
                heights.AddRange(Enumerable.Repeat(0, elementBlock[0].Count));
                for (int row = elementBlock.Count - 2; row > 0; row--)
                {
                    for (var i = 0; i < elementBlock[row].Count; i++)
                    {
                        if (elementBlock[row][i] == '#')
                        {
                            heights[i]++;
                        }
                    }
                }
                _keys.Add(keyId, heights);
                keyId++;
            }

        }
    }
}

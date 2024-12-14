using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    public class Robot
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long SpeedX { get; set; }
        public long SpeedY { get; set; }
    }
    internal class Day14 : IDay<long>
    {
        private List<Robot> _robots = new List<Robot>();
        private int _maxX = 101;
        private int _maxY = 103;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            foreach (var robot in _robots)
            {
                robot.X += robot.SpeedX * 100;
                robot.Y += robot.SpeedY * 100;
                robot.X = robot.X % _maxX;
                robot.Y = robot.Y % _maxY;
                if (robot.X < 0)
                {
                    robot.X += _maxX;
                }
                if (robot.Y < 0)
                {
                    robot.Y += _maxY;
                }
            }
            var q1 = 0L;
            var q2 = 0L;
            var q3 = 0L;
            var q4 = 0L;
            foreach (var robot in _robots)
            {
                if (robot.X < _maxX / 2 && robot.Y < _maxY / 2)
                {
                    q1++;
                }
                else if (robot.X > _maxX / 2 && robot.Y < _maxY / 2)
                {
                    q2++;
                }
                else if (robot.X < _maxX / 2 && robot.Y > _maxY / 2)
                {
                    q3++;
                }
                else if (robot.X > _maxX / 2 && robot.Y > _maxY / 2)
                {
                    q4++;
                }
            }
            return q1 * q2 * q3 * q4;
        }

        //solution never return result, it will display every tree and count of seconds
        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            var count = 0;
            while (true)
            {
                var _map = new int[_maxY, _maxX];
                //move robots
                foreach (var robot in _robots)
                {
                    robot.X += robot.SpeedX;
                    robot.Y += robot.SpeedY;
                    if (robot.X < 0)
                    {
                        robot.X += _maxX;
                    }
                    if (robot.Y < 0)
                    {
                        robot.Y += _maxY;
                    }
                    if (robot.X >= _maxX)
                    {
                        robot.X -= _maxX;
                    }
                    if (robot.Y >= _maxY)
                    {
                        robot.Y -= _maxY;
                    }
                    _map[robot.Y, robot.X]++;
                }
                count++;

                //we don`t know exact position of tree, so we need to check almost all positions
                for (var i = 10; i < _maxY - 10; i++)
                {
                    for (var j = 10; j < _maxX - 10; j++)
                    {
                        if (isClaster(_map, new bool[_maxY, _maxX], j, i, 0))
                        {
                            //display tree
                            for (var k = 0; k < _maxY; k++)
                            {
                                for (var l = 0; l < _maxX; l++)
                                {
                                    if (_map[k, l] == 0)
                                    {
                                        Console.Write(". ");
                                    }
                                    else
                                    {
                                        Console.Write(_map[k, l] + " ");
                                    }
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine(count);
                            Console.ReadLine();
                            goto skipthistree;
                        }
                    }
                }
                skipthistree: continue;
            }
            return count;
        }
        private int strangeAmountOfRobotsTogeteher = 25;
        private bool isClaster(int[,] map, bool[,] visited, int currx, int curry, int size)
        {

            if (size >= strangeAmountOfRobotsTogeteher)
            {
                return true;
            }
            if (currx < 0 || currx >= _maxX || curry < 0 || curry >= _maxY)
            {
                return false;
            }
            if (visited[curry, currx])
            {
                return false;
            }
            visited[curry, currx] = true;
            if (map[curry, currx] == 0)
            {
                return false;
            }
            var isClaster = this.isClaster(map, visited, currx + 1, curry, size + 1);
            if (isClaster)
            {
                return true;
            }
            isClaster = this.isClaster(map, visited, currx - 1, curry, size + 1);
            if (isClaster)
            {
                return true;
            }
            isClaster = this.isClaster(map, visited, currx, curry + 1, size + 1);
            if (isClaster)
            {
                return true;
            }
            isClaster = this.isClaster(map, visited, currx, curry - 1, size + 1);
            if (isClaster)
            {
                return true;
            }
            return false;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(14);
            foreach (var line in input)
            {
                var robot = new Robot();
                var parts = line.Split(" ");
                var xy = parts[0].Split(",");
                robot.X = long.Parse(xy[0].Substring(2));
                robot.Y = long.Parse(xy[1]);
                var speed = parts[1].Split(",");
                robot.SpeedX = long.Parse(speed[0].Substring(2));
                robot.SpeedY = long.Parse(speed[1]);
                _robots.Add(robot);
            }
        }
    }
}

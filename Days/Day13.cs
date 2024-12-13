using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    public class Button
    {
        public long XIncrease { get; set; }
        public long YIncrease { get; set; }
    }
    public class Machine()
    {
        public Button ButtonA { get; set; }
        public Button ButtonB { get; set; }
        public long PrizeX { get; set; }
        public long PrizeY { get; set; }
    }
    internal class Day13 : IDay<long>
    {
        private List<Machine> _machines;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput(false);            
            return Solve();
        }

        public long Solve()
        {
            var result = 0L;
            foreach (var machine in _machines)
            {
                var determinant = (double)(machine.ButtonA.XIncrease * machine.ButtonB.YIncrease - machine.ButtonA.YIncrease * machine.ButtonB.XIncrease);
                if (determinant != 0)
                {
                    var detA = (double)(machine.ButtonB.YIncrease * machine.PrizeX - machine.ButtonB.XIncrease * machine.PrizeY);
                    var detB = (double)(machine.ButtonA.XIncrease * machine.PrizeY - machine.ButtonA.YIncrease * machine.PrizeX);
                    var a = detA / determinant;
                    var b = detB / determinant;
                    if ((a % 1) == 0 && (b % 1) == 0)
                    {
                        if (a >= 0 && b >= 0)
                        {
                            result += (long)(a * 3 + b);
                        }
                    }
                }
            }
            return result;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput(true);
            return Solve();
        }

        private async Task ReadInput(bool isPart2)
        {
            var input = await ReadFileUtils.ReadFileAsync(13);
            _machines = new List<Machine>();
            var machine = new Machine();
            foreach (var line in input)
            {                
                var splited = line.Split(":");
                if (splited[0] =="Button A")
                {
                    var xy = splited[1].Trim().Split(",");
                    machine.ButtonA = new Button
                    {
                        XIncrease = int.Parse(xy[0].Trim().Substring(2)),
                        YIncrease = int.Parse(xy[1].Trim().Substring(2))
                    };
                }
                if (splited[0] =="Button B")
                {
                    var xy = splited[1].Trim().Split(",");
                    machine.ButtonB = new Button
                    {
                        XIncrease = int.Parse(xy[0].Trim().Substring(2)),
                        YIncrease = int.Parse(xy[1].Trim().Substring(2))
                    };
                }
                if (splited[0] =="Prize")
                {
                    var xy = splited[1].Trim().Split(",");
                    machine.PrizeX = int.Parse(xy[0].Trim().Substring(2));
                    machine.PrizeY = int.Parse(xy[1].Trim().Substring(2));
                    if(isPart2)
                    {
                        machine.PrizeX += 10000000000000;
                        machine.PrizeY += 10000000000000;
                    }
                    _machines.Add(machine);
                    machine = new Machine();
                }
            }
        }
    }
}

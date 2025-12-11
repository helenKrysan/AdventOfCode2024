using AdventOfCode2025.Utils;
using System.Data;
using System.Diagnostics;

namespace AdventOfCode2025.Days
{
    internal class Day10 : IDay<long>
    {
        private List<Machine> machines = new List<Machine>();
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            long presses = 0;
            foreach (var machine in machines)
            {
                var res = 0;
                var finded = false;
                Queue<(int, List<bool>)> states = new Queue<(int, List<bool>)>();
                foreach (var switches in machine.Switches)
                {
                    var newLights = new List<bool>(machine.CurrentLight);
                    foreach (var index in switches)
                    {
                        newLights[index] = !newLights[index];
                    }
                    if (CheckFinded(machine, newLights))
                    {
                        finded = true;
                        res = 1;
                    }
                    states.Enqueue((1, newLights));
                }
                while (!finded)
                {
                    while (states.TryDequeue(out var state))
                    {
                        foreach (var switches in machine.Switches)
                        {
                            var newLights = new List<bool>(state.Item2);
                            foreach (var index in switches)
                            {
                                newLights[index] = !newLights[index];
                            }
                            if(CheckFinded(machine,newLights))
                            {
                                finded = true;
                                res = state.Item1 + 1;
                                break;
                            }
                            states.Enqueue((state.Item1 + 1, newLights));
                        }
                        if(finded)
                        {
                            break;
                        }
                    }
                }
                presses += res;
            }
            return presses;
        }

        private bool CheckFinded(Machine machine, List<bool> lights)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i] != machine.TurnOnLight[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            long res = 0;
            return res;
        }



        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(10);

            //var input = ReadFileUtils.ReadTestFile(10);
            machines = new List<Machine>();
            foreach (var line in input)
            {
                var machine = new Machine();
                var split = line.Split(" ");
                foreach (var sp in split)
                {
                    if (sp[0] == '[')
                    {
                        foreach (var ch in sp)
                        {
                            if (ch == '#')
                            {
                                machine.TurnOnLight.Add(true);
                                machine.CurrentLight.Add(false);
                            }
                            else if (ch == '.')
                            {
                                machine.TurnOnLight.Add(false);
                                machine.CurrentLight.Add(false);
                            }
                        }
                    }
                    else if (sp[0] == '(')
                    {
                        var switches = new List<int>();
                        var switchStr = sp.Trim('(', ')');
                        var switchSplit = switchStr.Split(',');
                        foreach (var s in switchSplit)
                        {
                            switches.Add(int.Parse(s));
                        }
                        machine.Switches.Add(switches);
                    }
                }
                machines.Add(machine);
            }
        }

        public class Machine
        {
            public List<bool> CurrentLight { get; set; } = new List<bool>();
            public List<bool> TurnOnLight { get; set; } = new List<bool>();
            public List<List<int>> Switches { get; set; } = new List<List<int>>();
        }
    }
}
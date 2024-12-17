using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    public enum Instruction
    {
        Adv = 0,
        Bxl,
        Bst,
        Jnz,
        Bxc,
        Out,
        Bdv,
        Cdv
    }
    internal class Day17 : IDay<string>
    {
        private long _a;
        private long _b;
        private long _c;
        private int _currentInstruction;
        private bool _currentInstructionChanged;

        private List<(Instruction, int)> nonTuringMachine;
        private List<int> machineList;
        private List<long> _output;
        public async Task<string> SolvePart1Async()
        {
            await ReadInput();
            while (_currentInstruction < nonTuringMachine.Count)
            {
                Run(nonTuringMachine[_currentInstruction].Item1, nonTuringMachine[_currentInstruction].Item2);
                if (!_currentInstructionChanged)
                {
                    _currentInstruction++;
                }
                else
                {
                    _currentInstructionChanged = false;
                }
            }
            return _output.Select(item => item.ToString()).Aggregate((conc, item) => conc + ',' + item);
        }

        private void Run(Instruction inst, int op)
        {
            switch (inst)
            {
                case Instruction.Adv:
                    _a = _a / (int)Math.Pow(2, GetComboOp(op));
                    break;
                case Instruction.Bxl:
                    _b = _b ^ op;
                    break;
                case Instruction.Bst:
                    _b = GetComboOp(op) % 8;
                    break;
                case Instruction.Jnz:
                    if (_a != 0)
                    {
                        _currentInstruction = op / 2;
                        _currentInstructionChanged = true;
                    }
                    break;
                case Instruction.Bxc:
                    _b = _b ^ _c;
                    break;
                case Instruction.Out:
                    _output.Add(GetComboOp(op) % 8);
                    break;
                case Instruction.Bdv:
                    _b = _a / (long)Math.Pow(2, GetComboOp(op));
                    break;
                case Instruction.Cdv:
                    _c = _a / (long)Math.Pow(2, GetComboOp(op));
                    break;
            }
        }

        private long GetComboOp(int op)
        {
            switch (op)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3;
                case 4:
                    return _a;
                case 5:
                    return _b;
                case 6:
                    return _c;
                default:
                    throw new Exception("Invalid op");
            }
        }

        public async Task<string> SolvePart2Async()
        {
            await ReadInput();
            var maybeA = new List<long>() { 1,2,3,4,5,6,7 };
            var iteration = 15;
            while (iteration >= 0)
            {          
                var nextMaybeA = new List<long>();
                foreach (var a in maybeA)
                {
                    _currentInstruction = 0;
                    _currentInstructionChanged = false;
                    _output = new List<long>();
                    _a = a;
                    _b = 0L;
                    _c = 0L;
                    var isCorrect = false;
                    while (_currentInstruction < nonTuringMachine.Count)
                    {
                        RunOne(nonTuringMachine[_currentInstruction].Item1, nonTuringMachine[_currentInstruction].Item2);
                        if (!_currentInstructionChanged)
                        {
                            _currentInstruction++;
                        }
                        else
                        {
                            _currentInstructionChanged = false;
                        }
                    }
                    if (_output[0] == machineList[iteration])
                    {
                        for (var i = 0; i < 8; i++)
                        {
                            nextMaybeA.Add(a*8 + i);
                        }
                    }
                }
                maybeA = nextMaybeA;
                iteration--;
            }
            return (maybeA[0]/8).ToString();
        }

        private void RunOne(Instruction inst, int op)
        {
            switch (inst)
            {
                case Instruction.Adv:
                    _a = _a / (int)Math.Pow(2, GetComboOp(op));
                    break;
                case Instruction.Bxl:
                    _b = _b ^ op;
                    break;
                case Instruction.Bst:
                    _b = GetComboOp(op) % 8;
                    break;
                case Instruction.Jnz:
                    //ignore this for one cycle
                    break;
                case Instruction.Bxc:
                    _b = _b ^ _c;
                    break;
                case Instruction.Out:
                    _output.Add(GetComboOp(op) % 8);
                    break;
                case Instruction.Bdv:
                    _b = _a / (long)Math.Pow(2, GetComboOp(op));
                    break;
                case Instruction.Cdv:
                    _c = _a / (long)Math.Pow(2, GetComboOp(op));
                    break;
            }
        }

        public async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(17);
            _currentInstruction = 0;
            _currentInstructionChanged = false;
            _output = new List<long>();
            _a = long.Parse(input[0].Split(":")[1].Trim());
            _b = long.Parse(input[1].Split(":")[1].Trim());
            _c = long.Parse(input[2].Split(":")[1].Trim());
            nonTuringMachine = new List<(Instruction, int)>();
            machineList = new List<int>();
            for (int i = 3; i < input.Count; i++)
            {
                if (input[i].Trim() == "")
                {
                    continue;
                }
                var line = input[i].Split(":")[1].Trim().Split(",");
                var pos = 0;
                while (pos < line.Length - 1)
                {
                    machineList.Add(int.Parse(line[pos]));
                    Instruction inst = (Instruction)int.Parse(line[pos]);
                    pos++;
                    machineList.Add(int.Parse(line[pos]));
                    var op = int.Parse(line[pos]);
                    pos++;
                    nonTuringMachine.Add((inst, op));
                }
            }
        }
    }
}

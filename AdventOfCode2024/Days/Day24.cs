
using AdventOfCode2024.Utils;

namespace AdventOfCode2024.Days
{
    internal class Day24 : IDay<long>
    {
        private Dictionary<string, int> _values;
        private Dictionary<string, (string, string, string)> _valuesToCalc;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            while (_valuesToCalc.Count > 0)
            {
                ConvertValues(_valuesToCalc.First().Key);
            }
            var zvalues = _values.Where(x => x.Key.StartsWith("z")).OrderByDescending(x => x.Key).Select(x => x.Value).ToList();
            var currentBase = 1L;
            var result = 0L;
            for (int i = zvalues.Count - 1; i >= 0; i--)
            {
                result += zvalues[i] * currentBase;
                currentBase *= 2;
            }
            return result;
        }

        private void ConvertValues(string key)
        {
            string op = _valuesToCalc[key].Item1;
            string firstValue = _valuesToCalc[key].Item2;
            string secondValue = _valuesToCalc[key].Item3;
            if (_valuesToCalc.Count == 0)
            {
                return;
            }
            if (!_values.ContainsKey(firstValue))
            {
                ConvertValues(firstValue);
            }
            if (!_values.ContainsKey(secondValue))
            {
                ConvertValues(secondValue);
            }
            if (_values.ContainsKey(firstValue) && _values.ContainsKey(secondValue))
            {
                switch (op)
                {
                    case "AND":
                        _values.Add(key, _values[firstValue] & _values[secondValue]);
                        break;
                    case "OR":
                        _values.Add(key, _values[firstValue] | _values[secondValue]);
                        break;
                    case "XOR":
                        _values.Add(key, _values[firstValue] ^ _values[secondValue]);
                        break;
                }
                _valuesToCalc.Remove(key);
            }
        }

        private List<int> zsum = new List<int>();
        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            var xvalues = _values.Where(x => x.Key.StartsWith("x")).OrderByDescending(x => x.Key).Select(x => x.Value).ToList();
            var currentBase = 1L;
            var result = 0L;
            for (int i = xvalues.Count - 1; i >= 0; i--)
            {
                result += xvalues[i] * currentBase;
                currentBase *= 2;
            }
            Console.WriteLine($"X: {result}");
            var yvalues = _values.Where(x => x.Key.StartsWith("y")).OrderByDescending(x => x.Key).Select(x => x.Value).ToList();
            currentBase = 1L;
            result = 0L;
            for (int i = yvalues.Count - 1; i >= 0; i--)
            {
                result += yvalues[i] * currentBase;
                currentBase *= 2;
            }
            Console.WriteLine($"Y: {result}");
            zsum.AddRange(Enumerable.Repeat(0, xvalues.Count + 1));
            for (int i = xvalues.Count - 1; i >= 0; i--)
            {
                zsum[i + 1] = zsum[i + 1] + xvalues[i] + yvalues[i];
                if (zsum[i + 1] > 1)
                {
                    zsum[i + 1] = zsum[i + 1] - 2;
                    zsum[i] = 1;
                }
            }
            var allZValues = _valuesToCalc.Where(x => x.Key.StartsWith("z")).OrderBy(x => x.Key).Select(x => x.Key).ToList();
            var xc = 0;
            var zindex = allZValues.Count - 1;
            var correct100 = new HashSet<string>(); 
            foreach (var z in allZValues)
            {
                HashSet<string> keysUsed = new HashSet<string>();
                ConvertValuesSecond(z, keysUsed);
                if(_values[z] == zsum[zindex])
                {
                    foreach (var key in keysUsed)
                    {
                        correct100.Add(key);
                    }
                    keysUsed = new HashSet<string>();
                }
                else
                {
                    var toSwap = new HashSet<string>();
                    foreach(var k in keysUsed)
                    {
                        if(!correct100.TryGetValue(k, out var value))
                        {
                            toSwap.Add(k);
                        }

                    }
                    var s = 0;
                }
                zindex--;
                xc++;
            }
            return 0;
        }

        private void ConvertValuesSecond(string key, HashSet<string> keysUsed)
        {
            string op = _valuesToCalc[key].Item1;
            string firstValue = _valuesToCalc[key].Item2;
            string secondValue = _valuesToCalc[key].Item3;
            if (_valuesToCalc.Count == 0)
            {
                return;
            }
            if (!_values.ContainsKey(firstValue))
            {
                ConvertValuesSecond(firstValue, keysUsed);
            }
            if (!_values.ContainsKey(secondValue))
            {
                ConvertValuesSecond(secondValue, keysUsed);
            }
            if (_values.ContainsKey(firstValue) && _values.ContainsKey(secondValue))
            {
                switch (op)
                {
                    case "AND":
                        _values.Add(key, _values[firstValue] & _values[secondValue]);
                        break;
                    case "OR":
                        _values.Add(key, _values[firstValue] | _values[secondValue]);
                        break;
                    case "XOR":
                        _values.Add(key, _values[firstValue] ^ _values[secondValue]);
                        break;
                }
                keysUsed.Add(firstValue);
                keysUsed.Add(secondValue);
                _valuesToCalc.Remove(key);
            }
        }

        public async Task ReadInput()
        {
            //var input = await ReadFileUtils.ReadFileAsync(24);
            var input = ReadFileUtils.ReadTestFile(24);
            _values = new Dictionary<string, int>();
            _valuesToCalc = new Dictionary<string, (string, string, string)>();
            var firstSection = true;
            foreach (var line in input)
            {
                if (line.Trim() == "")
                {
                    firstSection = false;
                    continue;
                }
                if (firstSection)
                {
                    var splitted = line.Split(":");
                    _values.Add(splitted[0].Trim(), byte.Parse(splitted[1].Trim()));
                }
                else
                {
                    var splitted = line.Split("->");
                    var values = splitted[0].Split(" ");
                    _valuesToCalc.Add(splitted[1].Trim(), (values[1].Trim(), values[0].Trim(), values[2].Trim()));
                }
            }
        }
    }
}

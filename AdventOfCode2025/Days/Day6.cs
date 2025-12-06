
using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Days
{
    internal class Day6 : IDay<long>
    {
        List<Column> columns = new List<Column>();
        public async Task<long> SolvePart1Async()
        {
            await ReadInput1();
            long res = 0;
            foreach (var column in columns)
            {
                res += column.Calculate();
            }
            return res;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput2();
            long res = 0;
            foreach (var column in columns)
            {
                column.ConvertAOCRTL();
                res += column.Calculate();
            }
            return res;
        }

        private async Task ReadInput1()
        {
            var input = await ReadFileUtils.ReadFileAsync(6);
            columns = new List<Column>();
            input[0].Split(' ').ToList().ForEach(c =>
            {
                if (c != "")
                {
                    columns.Add(new Column());
                }
            });
            foreach (var line in input)
            {
                var columnNumber = 0;
                line.Split(' ').ToList().ForEach(c =>
                {
                    if (c != "")
                    {
                        if (c != "*" && c != "+")
                        {
                            columns[columnNumber].Values.Add(int.Parse(c));
                        }
                        else
                        {
                            columns[columnNumber].Operator = c[0];
                        }
                        columnNumber++;
                    }
                });
            }
        }

        private async Task ReadInput2()
        {
            var input = await ReadFileUtils.ReadFileAsync(6);
            columns = new List<Column>();
            input[0].Split(' ').ToList().ForEach(c =>
            {
                if (c != "")
                {
                    columns.Add(new Column());
                }
            });
            var lastLine = input[input.Count - 1];
            foreach (var line in input)
            {
                var columnNumber = 0;
                var numberStr = "";
                var numberStarted = false;
                var numberFinished = false;
                var i = 0;
                foreach (var c in line)
                {
                    if (c != ' ')
                    {
                        if (numberFinished)
                        {
                            if (numberStr.Trim() != "*" && numberStr.Trim() != "+")
                            {
                                columns[columnNumber].StringValues.Add(numberStr.Substring(0, numberStr.Length - 1));
                            }
                            else
                            {
                                columns[columnNumber].Operator = numberStr.Trim()[0];
                            }
                            columnNumber++;
                            numberStr = "";
                            numberFinished = false;
                        }
                        else
                        {
                            numberStarted = true;
                        }
                    }
                    else
                    {
                        if (numberStarted)
                        {
                            numberFinished = true;
                            if (lastLine[i] != ' ')
                            {
                                if (numberFinished)
                                {
                                    if (numberStr.Trim() != "*" && numberStr.Trim() != "+")
                                    {
                                        columns[columnNumber].StringValues.Add(numberStr.Substring(0, numberStr.Length - 1));
                                    }
                                    else
                                    {
                                        columns[columnNumber].Operator = numberStr.Trim()[0];
                                    }
                                    columnNumber++;
                                    numberStr = "";
                                    numberStarted = false;
                                    numberFinished = false;
                                }
                            }
                        }
                    }
                    numberStr += c;
                    i++;
                }
                if (numberStr.Trim() != "*" && numberStr.Trim() != "+")
                {
                    columns[columnNumber].StringValues.Add(numberStr.Substring(0, numberStr.Length));
                }
                else
                {
                    columns[columnNumber].Operator = numberStr.Trim()[0];
                }
            }
        }
    }

    internal class Column
    {
        public List<int> Values { get; set; }
        public List<string> StringValues { get; set; }
        public char Operator { get; set; }
        public Column()
        {
            Values = new List<int>();
            StringValues = new List<string>();
        }
        public void ConvertAOCRTL()
        {
            var newValues = new List<int>();
            var maxLength = StringValues.Max(s => s.Length);

            while (maxLength > 0)
            {
                var newNumber = "";
                for (int i = 0; i < StringValues.Count; i++)
                {
                    if (StringValues[i][maxLength-1] != ' ')
                    {
                        newNumber += StringValues[i][maxLength - 1];
                    }
                }
                newValues.Add(int.Parse(newNumber));
                maxLength--;
            }
            Values = newValues;
        }
        public long Calculate()
        {
            if (Operator == '*')
            {
                return Values.Aggregate<int, long>(1, (seed, elem) => seed *= elem);
            }
            else
            {
                return Values.Aggregate<int, long>(0, (seed, elem) => seed += elem);
            }
        }
    }
}

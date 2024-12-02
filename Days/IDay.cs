using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Days
{
    internal interface IDay<T>
    {
        T SolvePart1();
        T SolvePart2();
    }
}

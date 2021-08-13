using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdventOfCode.Core;
using AdventOfCode.Core.Extensions;

namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Operation Order")]
    public sealed class Day18 : SolutionBase
    {
        private const char Space = ' ';
        private const char Add = '+';
        private const char Multiply = '*';
        private const char OpenBracket = '(';
        private const char CloseBracket = ')';

        protected override string Part1(string input)
        {
            return input.GetLines<string>()
                .Select(line => ReversePolishNotation(line, false))
                .Sum()
                .ToString(CultureInfo.InvariantCulture);
        }

        protected override string Part2(string input)
        {
            return input.GetLines<string>()
                .Select(line => ReversePolishNotation(line, true))
                .Sum()
                .ToString(CultureInfo.InvariantCulture);
        }
        
        private static double ReversePolishNotation(string line, bool part2)
        {
            var values = new Stack<double>();
            var ops = new Stack<char>();
            ops.Push(OpenBracket);
            
            foreach (var c in line)
            {
                switch (c)
                {
                    case Space:
                        break;
                    case Add:
                        (ops, values) = Calculate(ops, values, part2);
                        ops.Push(Add);
                        break;
                    case Multiply:
                        (ops, values) = Calculate(ops, values);
                        ops.Push(c);
                        break;
                    case OpenBracket:
                        ops.Push(c);
                        break;
                    case CloseBracket:
                        (ops, values) = Calculate(ops, values);
                        ops.Pop();
                        break;
                    default:
                        values.Push(char.GetNumericValue(c));
                        break;
                }
            }
            (_, values) = Calculate(ops, values);
            return values.Single();
        }

        private static (Stack<char> ops, Stack<double> values) Calculate(Stack<char> ops, Stack<double> values, bool part2 = false)
        {
            while (!(ops.Peek() == OpenBracket || part2 && ops.Peek() == Multiply))
            {
                switch (ops.Pop())
                {
                    case Add:
                        values.Push(values.Pop() + values.Pop());
                        break;
                    case Multiply:
                        values.Push(values.Pop() * values.Pop());
                        break;
                }
            }

            return (ops, values);
        }
    }
}
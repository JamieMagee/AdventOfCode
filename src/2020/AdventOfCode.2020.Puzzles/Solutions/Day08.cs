namespace AdventOfCode._2020.Puzzles.Solutions;

[Puzzle("Handheld Halting")]
public sealed class Day08 : SolutionBase
{
    protected override string Part1(string input)
    {
        var instructions = ParseInput(input);
        var (_, acc) = RunProgram(instructions);
        return acc.ToString();
    }

    protected override string Part2(string input)
    {
        var instructions = ParseInput(input);
        var res = 0;
        foreach (var t in instructions)
        {
            var command = t.Command;
            t.Command = command switch
            {
                "jmp" => "nop",
                "nop" => "jmp",
                _ => command,
            };
            var (halts, acc) = RunProgram(instructions);
            if (halts)
            {
                res = acc;
                break;
            }

            t.Command = command;
        }

        return res.ToString();
    }

    private static (bool Halts, int Acc) RunProgram(IList<Instruction> instructions)
    {
        var seen = new HashSet<int>();
        var acc = 0;
        var pos = 0;
        while (pos < instructions.Count)
        {
            if (seen.Contains(pos))
            {
                return (false, acc);
            }

            seen.Add(pos);
            switch (instructions[pos].Command)
            {
                case "acc":
                    acc += instructions[pos].Argument;
                    pos++;
                    break;
                case "jmp":
                    pos += instructions[pos].Argument;
                    break;
                case "nop":
                    pos++;
                    break;
            }
        }

        return (true, acc);
    }

    private static IList<Instruction> ParseInput(string input) => [.. GetLines(input).Select(x => x.Split(' '))
        .Select(x => new Instruction
        {
            Command = x[0],
            Argument = int.Parse(x[1]),
        })];

    private sealed record Instruction
    {
        public string Command { get; set; }

        public int Argument { get; set; }
    }
}

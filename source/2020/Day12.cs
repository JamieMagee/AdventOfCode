using AdventOfCode.Core.Extensions;

namespace AdventOfCode._2020.Puzzles.Solutions;

using System.Diagnostics.CodeAnalysis;

[Puzzle("Rain Risk")]
public sealed class Day12 : SolutionBase
{
    protected override string Part1(string input)
    {
        var lines = input.GetLines<string>();
        var ship = new Ship();
        foreach (var (op, val) in lines.Select(l => (l[0], int.Parse(l[1..]))))
        {
            switch (op)
            {
                case 'F':
                    ship.MoveForward(val);
                    break;
                case 'E':
                    ship.Move(0, val);
                    break;
                case 'S':
                    ship.Move(1, val);
                    break;
                case 'W':
                    ship.Move(2, val);
                    break;
                case 'N':
                    ship.Move(3, val);
                    break;
                case 'R':
                    ship.Rotate(val / 90);
                    break;
                case 'L':
                    ship.Rotate(4 - (val / 90));
                    break;
            }
        }

        return ship.ManhattanDistance();
    }

    protected override string Part2(string input)
    {
        var lines = input.GetLines<string>();
        var ship = new Ship();
        foreach (var (op, val) in lines.Select(l => (l[0], int.Parse(l[1..]))))
        {
            switch (op)
            {
                case 'F':
                    ship.MoveForwardWaypoint(val);
                    break;
                case 'E':
                    ship.MoveWaypoint(0, val);
                    break;
                case 'S':
                    ship.MoveWaypoint(1, val);
                    break;
                case 'W':
                    ship.MoveWaypoint(2, val);
                    break;
                case 'N':
                    ship.MoveWaypoint(3, val);
                    break;
                case 'R':
                    ship.RotateWaypoint(val / 90);
                    break;
                case 'L':
                    ship.RotateWaypoint(4 - (val / 90));
                    break;
            }
        }

        return ship.ManhattanDistance();
    }
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Justification")]
public sealed class Ship
{
    private int Dir { get; set; }

    private int Wayx { get; set; } = 10;

    private int Wayy { get; set; } = 1;

    private int X { get; set; }

    private int Y { get; set; }

    public void Move(int dir, int val) => (this.X, this.Y) = dir switch
    {
        0 => (this.X + val, this.Y),
        1 => (this.X, this.Y - val),
        2 => (this.X - val, this.Y),
        3 => (this.X, this.Y + val),
        _ => (this.X, this.Y),
    };

    public void MoveWaypoint(int dir, int val) => (this.Wayx, this.Wayy) = dir switch
    {
        0 => (this.Wayx + val, this.Wayy),
        1 => (this.Wayx, this.Wayy - val),
        2 => (this.Wayx - val, this.Wayy),
        3 => (this.Wayx, this.Wayy + val),
        _ => (this.Wayx, this.Wayy),
    };

    public void MoveForward(int val) => this.Move(this.Dir, val);

    public void MoveForwardWaypoint(int val)
    {
        this.X += this.Wayx * val;
        this.Y += this.Wayy * val;
    }

    public void Rotate(int val) => this.Dir = (this.Dir + val) % 4;

    public void RotateWaypoint(int val) => (this.Wayx, this.Wayy) = val switch
    {
        0 => (this.Wayx, this.Wayy),
        1 => (this.Wayy, -this.Wayx),
        2 => (-this.Wayx, -this.Wayy),
        3 => (-this.Wayy, this.Wayx),
        _ => (this.Wayx, this.Wayy),
    };

    public string ManhattanDistance() => (Math.Abs(this.X) + Math.Abs(this.Y)).ToString();
}

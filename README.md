# Advent of Code

[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/JamieMagee/AdventOfCode/GitHub%20Pages?style=for-the-badge)](https://github.com/JamieMagee/AdventOfCode/actions?query=workflow%3A%22GitHub+Pages%22)

My solutions for [Advent of Code](https://adventofcode.com/) written in C# and Blazor WebAssembly

Check it out at https://jamiemagee.github.io/AdventofCode.

Based on [sanraith/aoc2019](https://github.com/sanraith/aoc2019)

## Project Structure

| Folder                             | Description                                                 |
|------------------------------------|-------------------------------------------------------------|
| `AdventOfCode.Core`                | Interfaces and classes for solving puzzles                  |
| `AdventOfCode.Core.Test`           | Interfaces and classes for puzzle tests                     |
| `AdventOfCode.{Year}.Puzzles`      | Inputs and solutions for that year's Advent of Code puzzles |
| `AdventOfCode.{Year}.Puzzles.Test` | Inputs and solutions for that year's Advent of Code puzzles |
| `AdventOfCode.Console`             | Console application to prepare and run the puzzle solutions |
| `AdventOfCode.Web`                 | Blazor WebAssembly application to run the puzzle solutions  |

## Setup

This project requires [.NET SDK 5.0](https://dotnet.microsoft.com/download/dotnet/5.0).

To run the Blazor WebAssembly application:

- `dotnet run -p src/AdventOfCode.Web`
- Open `[http://localhost:5000](http://localhost:5000)`

To run all puzzle solutions in your console:

- `dotnet run -p src/AdventOfCode.Console --all`

To run the last solution in your console:

- `dotnet run -p src/AdventOfCode.Console --last`

To run a specific solution in your console:

- `dotnet run -p src/AdventOfCode.Console --day [number of day]`

To setup the environment for a new puzzle solution:

- Set your adventofcode.com session cookie for `AdventOfCode.Console` as a user secret:
    - `dotnet user-secrets -p AdventOfCode.Console set "SessionCookie" "[your session cookie]"`
- Run setup to create source, test, input and description files for the given day:
    - `dotnet run -p aoc2019.ConsoleApp --setup [number of day]`

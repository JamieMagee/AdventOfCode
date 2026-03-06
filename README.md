# Advent of Code

[![GitHub Workflow Status][workflow-shield]][workflow-link]

My [Advent of Code](https://adventofcode.com/) solutions in C# and Blazor WebAssembly.

Live at <https://jamiemagee.github.io/AdventOfCode>. Based on [sanraith/aoc2019](https://github.com/sanraith/aoc2019).

## Project structure

| Folder | What's in it |
| --- | --- |
| `AdventOfCode.Core` | Interfaces and base classes for solving puzzles |
| `AdventOfCode.Core.Test` | Corresponding test infrastructure |
| `AdventOfCode.{Year}.Puzzles` | Solutions and inputs for a given year |
| `AdventOfCode.{Year}.Puzzles.Test` | Tests for a given year |
| `AdventOfCode.Console` | CLI for running and scaffolding solutions |
| `AdventOfCode.Web` | Blazor WebAssembly frontend |

## Setup

Requires [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0).

Run the web app:

```sh
dotnet run -p src/AdventOfCode.Web
# then open http://localhost:5000
```

Run solutions from the terminal:

```sh
# all puzzles
dotnet run -p src/AdventOfCode.Console --all

# just the latest one
dotnet run -p src/AdventOfCode.Console --last

# a specific day
dotnet run -p src/AdventOfCode.Console --day 12
```

Scaffold a new day:

First, stash your adventofcode.com session cookie:

```sh
dotnet user-secrets -p AdventOfCode.Console set "SessionCookie" "your-session-cookie"
```

Then generate the source, test, input, and description files:

```sh
dotnet run -p src/AdventOfCode.Console --setup 12
```

[workflow-shield]: https://img.shields.io/github/actions/workflow/status/JamieMagee/AdventOfCode/github-pages.yml?style=for-the-badge
[workflow-link]: https://github.com/JamieMagee/AdventOfCode/actions/workflows/pages/pages-build-deployment

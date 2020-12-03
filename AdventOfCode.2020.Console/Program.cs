﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode._2020.Puzzles.Core;
using CommandLine;
using CommandLine.Text;
using HtmlAgilityPack;

namespace AdventOfCode._2020.Console
{
    internal class Program
    {
        private readonly Configuration _myConfig;

        private readonly Options _myOptions;
        private readonly SolutionHandler _mySolutionHandler;

        private Program(string[] args)
        {
            Options options = null;
            if (!args.Any())
            {
                System.Console.WriteLine("Running last available solution. Use --help for available options.");
                System.Console.WriteLine();
                options = new Options {RunLastDay = true};
            }
            else
            {
                Parser.Default.ParseArguments<Options>(args).WithParsed(o => options = o);
            }

            _myConfig = Configuration.Load();
            _myOptions = options;
            _mySolutionHandler = new SolutionHandler();
        }

        public static async Task Main(string[] args)
        {
            await new Program(args).Run();
        }

        private async Task Run()
        {
            if (_myOptions == null)
            {
                return;
            }

            if (_myOptions.DayToSetup.HasValue)
            {
                await SetupDay(_myOptions.DayToSetup.Value);
            }

            if (_myOptions.RunAllDays)
            {
                await SolveAllDays();
                return;
            }

            if (_myOptions.RunLastDay)
            {
                await SolveLastDay();
            }

            if (_myOptions.DayToRun.HasValue)
            {
                await SolveDay(_myOptions.DayToRun.Value);
            }
        }

        private async Task SolveAllDays()
        {
            var count = 0;
            foreach (var day in _mySolutionHandler.Solutions.Keys.OrderBy(x => x))
            {
                await SolveDay(day);
                if (++count < _mySolutionHandler.Solutions.Count)
                {
                    System.Console.WriteLine();
                }
            }
        }

        private async Task SolveLastDay()
        {
            var lastSolutionDay = _mySolutionHandler.Solutions.Keys.LastOrDefault(x => x >= 1 && x <= 25);
            if (lastSolutionDay > 0)
            {
                await SolveDay(lastSolutionDay);
            }
            else
            {
                System.Console.WriteLine("No solution is available yet.");
            }
        }

        private async Task SolveDay(int day)
        {
            var solutionMetadata = _mySolutionHandler.Solutions[day];
            var solution = solutionMetadata.CreateInstance();

            var dayString = day.ToString().PadLeft(2, '0');
            var rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var input = await File.ReadAllTextAsync(Path.Combine(rootDir, "Input", $"day{dayString}.txt"));

            System.Console.WriteLine($"Day {day}: {solutionMetadata.Title}");
            await SolvePart(1, input, solution.Part1Async, solution);
            await SolvePart(2, input, solution.Part2Async, solution);
        }

        private static async Task SolvePart(int partNumber, string input, Func<string, Task<string>> action,
            ISolution solution)
        {
            var emptyWaitingMessage = $"Calculating Part {partNumber}... ";
            var waitingMessage = emptyWaitingMessage;

            System.Console.Write(waitingMessage);

            string result = null;
            try
            {
                solution.ProgressUpdated += ProgressUpdated;
                result = await action(input);
            }
            catch (NotImplementedException)
            {
                result = "Not implemented.";
            }
            catch (Exception ex)
            {
                result = $"({ex.GetType().Name}) {ex.Message}";
            }
            finally
            {
                solution.ProgressUpdated -= ProgressUpdated;
            }

            System.Console.Write($"\r{new string(' ', waitingMessage.Length)}\r");
            System.Console.Write($"Part {partNumber}: ");
            if (result.Contains(Environment.NewLine))
            {
                result = Environment.NewLine + result;
            }

            System.Console.WriteLine(result);

            void ProgressUpdated(object _, SolutionProgressEventArgs args)
            {
                if (args.Progress.Percentage > 0)
                {
                    var prevLength = waitingMessage.Length;
                    waitingMessage = $"\r{emptyWaitingMessage}{Math.Min(99.99, args.Progress.Percentage):0.00}%";
                    System.Console.Write(waitingMessage +
                                         new string(' ', Math.Max(0, prevLength - waitingMessage.Length)));
                }
            }
        }

        private async Task SetupDay(int day)
        {
            var dayString = day.ToString().PadLeft(2, '0');
            var consoleProjectBinPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var puzzleProjectPath = Path.Combine(consoleProjectBinPath, _myConfig.PuzzleProjectPath);
            System.Console.WriteLine($"Setting up input and description for {_myConfig.Year}/12/{dayString}...");

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie("session", _myConfig.SessionCookie, "/", "adventofcode.com"));
            using var httpClientHandler = new HttpClientHandler {CookieContainer = cookieContainer};
            using var httpClient = new HttpClient(httpClientHandler);

            await SaveInputAsync(day, dayString, puzzleProjectPath, httpClient);
            var puzzleTitle = await SaveDescriptionAsync(day, dayString, puzzleProjectPath, httpClient);
            await CreateSolutionSourceAsync(day, dayString, consoleProjectBinPath, puzzleProjectPath, puzzleTitle);

            System.Console.WriteLine("Done.");
        }

        private async Task SaveInputAsync(int day, string dayString, string puzzleProjectPath, HttpClient httpClient)
        {
            var inputAddress = $"https://adventofcode.com/{_myConfig.Year}/day/{day}/input";
            var inputFile = new FileInfo(Path.Combine(puzzleProjectPath, "Input", $"day{dayString}.txt"));
            System.Console.WriteLine($"Downloading input from {inputAddress}");
            var input = await httpClient.GetStringAsync(inputAddress);

            System.Console.WriteLine($"Saving input to {inputFile.FullName}");
            await File.WriteAllTextAsync(inputFile.FullName, input, Encoding.UTF8);
        }

        private async Task<string> SaveDescriptionAsync(int day, string dayString, string puzzleProjectPath,
            HttpClient httpClient)
        {
            var descriptionAddress = $"https://adventofcode.com/{_myConfig.Year}/day/{day}";
            var descriptionFile = new FileInfo(Path.Combine(puzzleProjectPath, "Descriptions", $"day{dayString}.html"));
            var puzzleTitleRegex = new Regex(@"---.*: (?'title'.*) ---");

            System.Console.WriteLine($"Downloading description from {descriptionAddress}");
            var descriptionPageSource = await httpClient.GetStringAsync(descriptionAddress);
            var descriptionPage = new HtmlDocument();
            descriptionPage.LoadHtml(descriptionPageSource);
            var articleNodes = descriptionPage.DocumentNode.SelectNodes("//article[@class='day-desc']");

            var titleNode = articleNodes.First().SelectSingleNode("//h2");
            var puzzleTitle = puzzleTitleRegex.Match(titleNode.InnerText).Groups["title"].Value;
            titleNode.InnerHtml = "--- Part One ---";
            System.Console.WriteLine($"Found {articleNodes.Count} parts. Title: {puzzleTitle}");
            var description = articleNodes.Aggregate(string.Empty, (result, node) => result + node.OuterHtml);

            System.Console.WriteLine($"Saving description to {descriptionFile.FullName}");
            await File.WriteAllTextAsync(descriptionFile.FullName, description, Encoding.UTF8);

            return puzzleTitle;
        }

        private static async Task CreateSolutionSourceAsync(int day, string dayString, string consoleProjectBinPath,
            string puzzleProjectPath, string puzzleTitle)
        {
            var solutionSourceFile = new FileInfo(Path.Combine(consoleProjectBinPath, "Template", "Day_DAYSTRING_.cs"));
            var solutionTargetFile = new FileInfo(Path.Combine(puzzleProjectPath, "Solutions", $"Day{dayString}.cs"));
            var testSourceFile = new FileInfo(Path.Combine(consoleProjectBinPath, "Template", "Day_DAYSTRING_Test.cs"));
            var testTargetFile =
                new FileInfo(Path.Combine($"{puzzleProjectPath}.Test", "Solutions", $"Day{dayString}Test.cs"));

            if (solutionTargetFile.Exists)
            {
                System.Console.WriteLine($"Source file already exists at {solutionTargetFile.FullName}");
            }
            else
            {
                System.Console.WriteLine($"Saving source file to {solutionTargetFile.FullName}");
                var sourceContent = await File.ReadAllTextAsync(solutionSourceFile.FullName);
                sourceContent = sourceContent
                    .Replace("_DAYNUMBER_", day.ToString())
                    .Replace("_DAYSTRING_", dayString)
                    .Replace("_PUZZLETITLE_", puzzleTitle);
                await File.WriteAllTextAsync(solutionTargetFile.FullName, sourceContent, Encoding.UTF8);
            }

            if (testTargetFile.Exists)
            {
                System.Console.WriteLine($"Test file already exists at {solutionTargetFile.FullName}");
            }
            else
            {
                System.Console.WriteLine($"Saving test file to {testTargetFile.FullName}");
                var testContent = await File.ReadAllTextAsync(testSourceFile.FullName);
                testContent = testContent.Replace("_DAYSTRING_", dayString);
                await File.WriteAllTextAsync(testTargetFile.FullName, testContent, Encoding.UTF8);
            }
        }

        private sealed class Options
        {
            [Option('a', "all", HelpText = "Run all available solutions.")]
            public bool RunAllDays { get; private set; }

            [Option('l', "last", HelpText = "Run the last available solution.")]
            public bool RunLastDay { get; set; }

            [Option('d', "day", HelpText = "[Number of day] Run the solution for the given day.")]
            public int? DayToRun { get; private set; }

            [Option('s', "setup",
                HelpText =
                    "[Number of day] Download input and description for given day, and add it to AdventOfCode._2020.Puzzles along with an empty solution .cs file.")]
            public int? DayToSetup { get; private set; }

            [Usage(ApplicationAlias = "AdventOfCode.2020.Console")]
            public static IEnumerable<Example> Examples => new[]
            {
                new Example("Run all available solutions", new Options {RunAllDays = true}),
                new Example("Run the last available solution", new Options {RunLastDay = true}),
                new Example("Run solution for day 12", new UnParserSettings {PreferShortName = true},
                    new Options {DayToRun = 12}),
                new Example(
                    "Add input and description for day 23 to AdventOfCode._2020.Puzzles along with an empty test and solution .cs file",
                    new Options {DayToSetup = 23})
            };
        }
    }
}
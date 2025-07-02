namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Lobby Layout")]
    public sealed class Day24 : SolutionBase
    {
        public override Task<string> Part1Async(string input)
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var blackTiles = new HashSet<(int x, int y)>();

            foreach (var line in lines)
            {
                var position = GetTilePosition(line);

                if (!blackTiles.Remove(position))
                {
                    blackTiles.Add(position); // Flip to black
                }
                // If Remove returned true, the tile flipped back to white
            }

            return Task.FromResult(blackTiles.Count.ToString());
        }

        public override Task<string> Part2Async(string input)
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var blackTiles = new HashSet<(int x, int y)>();

            // Initial setup - same as Part 1
            foreach (var line in lines)
            {
                var position = GetTilePosition(line);

                if (!blackTiles.Remove(position))
                {
                    blackTiles.Add(position);
                }
            }

            // Simulate 100 days
            for (int day = 0; day < 100; day++)
            {
                blackTiles = SimulateDay(blackTiles);
            }

            return Task.FromResult(blackTiles.Count.ToString());
        }

        private static (int x, int y) GetTilePosition(string directions)
        {
            int x = 0, y = 0;
            int i = 0;

            while (i < directions.Length)
            {
                if (directions[i] == 'e')
                {
                    x++;
                    i++;
                }
                else if (directions[i] == 'w')
                {
                    x--;
                    i++;
                }
                else if (i + 1 < directions.Length)
                {
                    var twoChar = directions.Substring(i, 2);
                    switch (twoChar)
                    {
                        case "ne":
                            x++;
                            y++;
                            break;
                        case "nw":
                            y++;
                            break;
                        case "se":
                            y--;
                            break;
                        case "sw":
                            x--;
                            y--;
                            break;
                    }
                    i += 2;
                }
                else
                {
                    i++;
                }
            }

            return (x, y);
        }

        private static HashSet<(int x, int y)> SimulateDay(HashSet<(int x, int y)> blackTiles)
        {
            var newBlackTiles = new HashSet<(int x, int y)>();
            var tilesToCheck = new HashSet<(int x, int y)>();

            // Add all current black tiles and their neighbors to check
            foreach (var tile in blackTiles)
            {
                tilesToCheck.Add(tile);
                foreach (var neighbor in GetNeighbors(tile))
                {
                    tilesToCheck.Add(neighbor);
                }
            }

            foreach (var tile in tilesToCheck)
            {
                var blackNeighborCount = GetNeighbors(tile).Count(neighbor => blackTiles.Contains(neighbor));
                var isCurrentlyBlack = blackTiles.Contains(tile);

                if (isCurrentlyBlack)
                {
                    // Black tile stays black if it has 1 or 2 black neighbors
                    if (blackNeighborCount == 1 || blackNeighborCount == 2)
                    {
                        newBlackTiles.Add(tile);
                    }
                }
                else
                {
                    // White tile becomes black if it has exactly 2 black neighbors
                    if (blackNeighborCount == 2)
                    {
                        newBlackTiles.Add(tile);
                    }
                }
            }

            return newBlackTiles;
        }

        private static IEnumerable<(int x, int y)> GetNeighbors((int x, int y) position)
        {
            var (x, y) = position;

            // Six hexagonal directions: e, w, ne, nw, se, sw
            yield return (x + 1, y);     // e
            yield return (x - 1, y);     // w
            yield return (x + 1, y + 1); // ne
            yield return (x, y + 1);     // nw
            yield return (x, y - 1);     // se
            yield return (x - 1, y - 1); // sw
        }
    }
}
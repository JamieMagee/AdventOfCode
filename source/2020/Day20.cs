namespace AdventOfCode._2020.Puzzles.Solutions
{
  [Puzzle("Jurassic Jigsaw")]
  public sealed class Day20 : SolutionBase
  {
    public override Task<string> Part1Async(string input)
    {
      var tiles = ParseTiles(input);
      var borderMap = BuildBorderMap(tiles);
      var cornerTiles = FindCornerTiles(tiles, borderMap);

      return Task.FromResult(cornerTiles.Aggregate(1L, (acc, tile) => acc * tile.Id).ToString());
    }

    public override Task<string> Part2Async(string input)
    {
      var tiles = ParseTiles(input);
      var borderMap = BuildBorderMap(tiles);
      var assembledGrid = AssemblePuzzle(tiles, borderMap);
      var image = BuildFinalImage(assembledGrid);

      var seaMonster = new[]
      {
        "                  # ",
        "#    ##    ##    ###",
        " #  #  #  #  #  #   "
      };

      return Task.FromResult(CountWaterRoughness(image, seaMonster).ToString());
    }

    private static List<Tile> ParseTiles(string input)
    {
      var tiles = new List<Tile>();
      var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

      for (int i = 0; i < lines.Length; i++)
      {
        if (lines[i].StartsWith("Tile", StringComparison.Ordinal))
        {
          var id = int.Parse(lines[i].Split(' ')[1].TrimEnd(':'));
          var grid = new List<string>();

          i++;
          while (i < lines.Length && !lines[i].StartsWith("Tile", StringComparison.Ordinal))
          {
            grid.Add(lines[i]);
            i++;
          }
          i--;

          tiles.Add(new Tile(id, grid.ToArray()));
        }
      }

      return tiles;
    }

    private static Dictionary<string, List<Tile>> BuildBorderMap(List<Tile> tiles)
    {
      var borderMap = new Dictionary<string, List<Tile>>();

      foreach (var tile in tiles)
      {
        foreach (var border in tile.GetAllBorders())
        {
          if (!borderMap.ContainsKey(border))
            borderMap[border] = new List<Tile>();
          borderMap[border].Add(tile);
        }
      }

      return borderMap;
    }

    private static List<Tile> FindCornerTiles(List<Tile> tiles, Dictionary<string, List<Tile>> borderMap)
    {
      var cornerTiles = new List<Tile>();

      foreach (var tile in tiles)
      {
        var unmatchedBorders = 0;
        foreach (var border in tile.GetBorders())
        {
          var reversed = new string(border.Reverse().ToArray());
          if (borderMap[border].Count == 1 && borderMap[reversed].Count == 1)
          {
            unmatchedBorders++;
          }
        }

        if (unmatchedBorders == 2)
        {
          cornerTiles.Add(tile);
        }
      }

      return cornerTiles;
    }

    private Tile[,] AssemblePuzzle(List<Tile> tiles, Dictionary<string, List<Tile>> borderMap)
    {
      var size = (int)Math.Sqrt(tiles.Count);
      var grid = new Tile[size, size];
      var used = new HashSet<int>();

      var cornerTiles = FindCornerTiles(tiles, borderMap);
      var topLeft = cornerTiles[0];

      for (int rotation = 0; rotation < 4; rotation++)
      {
        for (int flip = 0; flip < 2; flip++)
        {
          var borders = topLeft.GetBorders();
          var topBorder = borders[0];
          var leftBorder = borders[3];

          var topReversed = new string(topBorder.Reverse().ToArray());
          var leftReversed = new string(leftBorder.Reverse().ToArray());

          if (borderMap[topBorder].Count == 1 && borderMap[topReversed].Count == 1 &&
              borderMap[leftBorder].Count == 1 && borderMap[leftReversed].Count == 1)
          {
            grid[0, 0] = topLeft;
            used.Add(topLeft.Id);
            goto foundOrientation;
          }

          topLeft = topLeft.FlipHorizontal();
        }
        topLeft = topLeft.Rotate90();
      }

    foundOrientation:

      for (int row = 0; row < size; row++)
      {
        for (int col = 0; col < size; col++)
        {
          if (row == 0 && col == 0) continue;

          string? targetBorder = null;
          bool isTopBorder = false;

          if (row > 0)
          {
            targetBorder = grid[row - 1, col].GetBorders()[2];
            isTopBorder = true;
          }
          else if (col > 0)
          {
            targetBorder = grid[row, col - 1].GetBorders()[1];
          }

          foreach (var candidate in tiles.Where(t => !used.Contains(t.Id)))
          {
            if (targetBorder == null) continue;

            var orientedTile = FindCorrectOrientation(candidate, targetBorder, isTopBorder);
            if (orientedTile != null)
            {
              if (row > 0 && col > 0)
              {
                var leftBorder = grid[row, col - 1].GetBorders()[1];
                if (orientedTile.GetBorders()[3] != leftBorder) continue;
              }

              grid[row, col] = orientedTile;
              used.Add(candidate.Id);
              break;
            }
          }
        }
      }

      return grid;
    }

    private static Tile? FindCorrectOrientation(Tile tile, string targetBorder, bool isTopBorder)
    {
      var current = tile;

      for (int rotation = 0; rotation < 4; rotation++)
      {
        for (int flip = 0; flip < 2; flip++)
        {
          var borders = current.GetBorders();
          var checkBorder = isTopBorder ? borders[0] : borders[3];

          if (checkBorder == targetBorder)
          {
            return current;
          }

          current = current.FlipHorizontal();
        }
        current = current.Rotate90();
      }

      return null;
    }

    private static string[] BuildFinalImage(Tile[,] grid)
    {
      var size = grid.GetLength(0);
      var tileSize = grid[0, 0].Grid.Length - 2;
      var imageSize = size * tileSize;
      var image = new string[imageSize];

      for (int row = 0; row < imageSize; row++)
      {
        var line = "";
        var tileRow = row / tileSize;
        var innerRow = row % tileSize + 1;

        for (int col = 0; col < imageSize; col++)
        {
          var tileCol = col / tileSize;
          var innerCol = col % tileSize + 1;

          line += grid[tileRow, tileCol].Grid[innerRow][innerCol];
        }

        image[row] = line;
      }

      return image;
    }

    private static int CountWaterRoughness(string[] image, string[] seaMonster)
    {
      var totalHashes = image.Sum(line => line.Count(c => c == '#'));
      var seaMonsterHashes = seaMonster.Sum(line => line.Count(c => c == '#'));

      var seaMonsterCount = 0;
      var currentImage = image;

      for (int rotation = 0; rotation < 4; rotation++)
      {
        for (int flip = 0; flip < 2; flip++)
        {
          seaMonsterCount = CountSeaMonsters(currentImage, seaMonster);
          if (seaMonsterCount > 0) break;

          currentImage = FlipImageHorizontal(currentImage);
        }
        if (seaMonsterCount > 0) break;

        currentImage = RotateImage90(currentImage);
      }

      return totalHashes - (seaMonsterCount * seaMonsterHashes);
    }

    private static int CountSeaMonsters(string[] image, string[] seaMonster)
    {
      var count = 0;
      var imageHeight = image.Length;
      var imageWidth = image[0].Length;
      var monsterHeight = seaMonster.Length;
      var monsterWidth = seaMonster[0].Length;

      for (int row = 0; row <= imageHeight - monsterHeight; row++)
      {
        for (int col = 0; col <= imageWidth - monsterWidth; col++)
        {
          if (IsSeaMonsterAt(image, seaMonster, row, col))
          {
            count++;
          }
        }
      }

      return count;
    }

    private static bool IsSeaMonsterAt(string[] image, string[] seaMonster, int startRow, int startCol)
    {
      for (int row = 0; row < seaMonster.Length; row++)
      {
        for (int col = 0; col < seaMonster[row].Length; col++)
        {
          if (seaMonster[row][col] == '#' && image[startRow + row][startCol + col] != '#')
          {
            return false;
          }
        }
      }
      return true;
    }

    private static string[] FlipImageHorizontal(string[] image)
    {
      return image.Select(line => new string(line.Reverse().ToArray())).ToArray();
    }

    private static string[] RotateImage90(string[] image)
    {
      var height = image.Length;
      var width = image[0].Length;
      var rotated = new string[width];

      for (int col = 0; col < width; col++)
      {
        var newRow = "";
        for (int row = height - 1; row >= 0; row--)
        {
          newRow += image[row][col];
        }
        rotated[col] = newRow;
      }

      return rotated;
    }
  }

  public class Tile
  {
    public int Id { get; }
    public string[] Grid { get; }

    public Tile(int id, string[] grid)
    {
      Id = id;
      Grid = grid;
    }

    public string[] GetBorders()
    {
      var top = Grid[0];
      var bottom = Grid[Grid.Length - 1];
      var left = new string(Grid.Select(row => row[0]).ToArray());
      var right = new string(Grid.Select(row => row[row.Length - 1]).ToArray());

      return new[] { top, right, bottom, left };
    }

    public string[] GetAllBorders()
    {
      var borders = GetBorders();
      var reversed = borders.Select(b => new string(b.Reverse().ToArray())).ToArray();
      return borders.Concat(reversed).ToArray();
    }

    public Tile Rotate90()
    {
      var size = Grid.Length;
      var rotated = new string[size];

      for (int col = 0; col < size; col++)
      {
        var newRow = "";
        for (int row = size - 1; row >= 0; row--)
        {
          newRow += Grid[row][col];
        }
        rotated[col] = newRow;
      }

      return new Tile(Id, rotated);
    }

    public Tile FlipHorizontal()
    {
      var flipped = Grid.Select(row => new string(row.Reverse().ToArray())).ToArray();
      return new Tile(Id, flipped);
    }
  }
}
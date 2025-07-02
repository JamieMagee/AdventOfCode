namespace AdventOfCode._2020.Puzzles.Solutions;

  [Puzzle("Combo Breaker")]
  public sealed class Day25 : SolutionBase
  {
      private const int SubjectNumber = 7;
      private const int Modulus = 20201227;

      public override Task<string> Part1Async(string input)
      {
          var publicKeys = input.Trim().Split('\n').Select(long.Parse).ToArray();
          var cardPublicKey = publicKeys[0];
          var doorPublicKey = publicKeys[1];

          // Find the loop sizes by brute force
          var cardLoopSize = FindLoopSize(cardPublicKey);
          var doorLoopSize = FindLoopSize(doorPublicKey);

          // Calculate encryption key using either device's loop size with the other's public key
          var encryptionKey = Transform(doorPublicKey, cardLoopSize);

          return Task.FromResult(encryptionKey.ToString());
      }

      private static int FindLoopSize(long publicKey)
      {
          var value = 1L;
          var loopSize = 0;

          while (value != publicKey)
          {
              value = Transform(value, SubjectNumber, 1);
              loopSize++;
          }

          return loopSize;
      }

      private static long Transform(long subjectNumber, int loopSize)
      {
          var value = 1L;
          for (var i = 0; i < loopSize; i++)
          {
              value = (value * subjectNumber) % Modulus;
          }
          return value;
      }

      private static long Transform(long value, long subjectNumber, int iterations)
      {
          for (var i = 0; i < iterations; i++)
          {
              value = (value * subjectNumber) % Modulus;
          }
          return value;
      }
  }
namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Crab Cups")]
    public sealed class Day23 : SolutionBase
    {
        public override Task<string> Part1Async(string input)
        {
            var cups = input.Trim().Select(c => int.Parse(c.ToString())).ToList();

            // Simulate 100 moves
            var result = SimulateCrabCups(cups, 100);

            // Collect labels clockwise from cup 1 (excluding cup 1 itself)
            var labels = new List<int>();
            int current = result[1]; // Start from the cup after cup 1
            for (int i = 1; i < cups.Count; i++)
            {
                labels.Add(current);
                current = result[current];
            }

            return Task.FromResult(string.Join("", labels));
        }

        public override Task<string> Part2Async(string input)
        {
            var initialCups = input.Trim().Select(c => int.Parse(c.ToString())).ToList();

            // Extend to 1 million cups
            var cups = new List<int>(initialCups);
            int maxInitial = initialCups.Max();
            for (int i = maxInitial + 1; i <= 1_000_000; i++)
            {
                cups.Add(i);
            }

            var result = SimulateCrabCups(cups, 10_000_000);

            // Find the two cups immediately clockwise of cup 1
            int firstCup = result[1];
            int secondCup = result[firstCup];

            return Task.FromResult(((long)firstCup * secondCup).ToString());
        }

        private static Dictionary<int, int> SimulateCrabCups(List<int> initialCups, int moves)
        {
            // Create a circular linked list using dictionary (cup -> next cup)
            var nextCup = new Dictionary<int, int>();

            // Set up the circular linked list
            for (int i = 0; i < initialCups.Count; i++)
            {
                int current = initialCups[i];
                int next = initialCups[(i + 1) % initialCups.Count];
                nextCup[current] = next;
            }

            int currentCup = initialCups[0];
            int minCup = 1;
            int maxCup = initialCups.Max();

            for (int move = 0; move < moves; move++)
            {
                // Pick up three cups
                int cup1 = nextCup[currentCup];
                int cup2 = nextCup[cup1];
                int cup3 = nextCup[cup2];

                // Remove the three cups from the circle
                nextCup[currentCup] = nextCup[cup3];

                // Find destination cup
                int destinationCup = currentCup - 1;
                while (destinationCup == cup1 || destinationCup == cup2 || destinationCup == cup3 || destinationCup < minCup)
                {
                    destinationCup--;
                    if (destinationCup < minCup)
                        destinationCup = maxCup;
                }

                // Insert the three cups after destination
                int afterDestination = nextCup[destinationCup];
                nextCup[destinationCup] = cup1;
                nextCup[cup3] = afterDestination;

                // Move to next current cup
                currentCup = nextCup[currentCup];
            }

            return nextCup;
        }
    }
}
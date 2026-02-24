namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Crab Combat")]
    public sealed class Day22 : SolutionBase
    {
        public override Task<string> Part1Async(string input)
        {
            var (player1Deck, player2Deck) = ParseInput(input);

            // Play the game
            while (player1Deck.Count > 0 && player2Deck.Count > 0)
            {
                var card1 = player1Deck.Dequeue();
                var card2 = player2Deck.Dequeue();

                if (card1 > card2)
                {
                    // Player 1 wins the round
                    player1Deck.Enqueue(card1);
                    player1Deck.Enqueue(card2);
                }
                else
                {
                    // Player 2 wins the round
                    player2Deck.Enqueue(card2);
                    player2Deck.Enqueue(card1);
                }
            }

            // Calculate the winning score
            var winningDeck = player1Deck.Count > 0 ? player1Deck : player2Deck;
            var score = CalculateScore(winningDeck);

            return Task.FromResult(score.ToString());
        }

        public override Task<string> Part2Async(string input)
        {
            var (player1Deck, player2Deck) = ParseInput(input);

            // Convert to lists for easier manipulation in recursive combat
            var player1Cards = new List<int>(player1Deck);
            var player2Cards = new List<int>(player2Deck);

            var (winner, winningCards) = PlayRecursiveCombat(player1Cards, player2Cards);
            var score = CalculateScoreFromList(winningCards);

            return Task.FromResult(score.ToString());
        }

        private static (Queue<int> player1, Queue<int> player2) ParseInput(string input)
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var player1Deck = new Queue<int>();
            var player2Deck = new Queue<int>();

            bool isPlayer1 = true;

            foreach (var line in lines)
            {
                if (line.StartsWith("Player 1:", StringComparison.Ordinal))
                {
                    isPlayer1 = true;
                    continue;
                }
                else if (line.StartsWith("Player 2:", StringComparison.Ordinal))
                {
                    isPlayer1 = false;
                    continue;
                }

                if (int.TryParse(line, out int card))
                {
                    if (isPlayer1)
                        player1Deck.Enqueue(card);
                    else
                        player2Deck.Enqueue(card);
                }
            }

            return (player1Deck, player2Deck);
        }

        private static int CalculateScore(Queue<int> deck)
        {
            var cards = deck.ToArray();
            var score = 0;

            for (int i = 0; i < cards.Length; i++)
            {
                // Bottom card (last in queue) has multiplier 1, top card has highest multiplier
                var multiplier = cards.Length - i;
                score += cards[i] * multiplier;
            }

            return score;
        }

        private static (int winner, List<int> winningCards) PlayRecursiveCombat(List<int> player1Cards, List<int> player2Cards)
        {
            var seenConfigurations = new HashSet<string>();

            while (player1Cards.Count > 0 && player2Cards.Count > 0)
            {
                // Check for infinite loop prevention
                var configuration = GetConfiguration(player1Cards, player2Cards);
                if (seenConfigurations.Contains(configuration))
                {
                    // Player 1 wins if we've seen this configuration before
                    return (1, player1Cards);
                }
                seenConfigurations.Add(configuration);

                // Draw cards
                var card1 = player1Cards[0];
                var card2 = player2Cards[0];
                player1Cards.RemoveAt(0);
                player2Cards.RemoveAt(0);

                int roundWinner;

                // Check if we should play a sub-game
                if (player1Cards.Count >= card1 && player2Cards.Count >= card2)
                {
                    // Create sub-decks for recursive game
                    var subDeck1 = player1Cards.Take(card1).ToList();
                    var subDeck2 = player2Cards.Take(card2).ToList();

                    var (subWinner, _) = PlayRecursiveCombat(subDeck1, subDeck2);
                    roundWinner = subWinner;
                }
                else
                {
                    // Regular combat - higher card wins
                    roundWinner = card1 > card2 ? 1 : 2;
                }

                // Winner takes both cards
                if (roundWinner == 1)
                {
                    player1Cards.Add(card1);
                    player1Cards.Add(card2);
                }
                else
                {
                    player2Cards.Add(card2);
                    player2Cards.Add(card1);
                }
            }

            // Return winner and their cards
            return player1Cards.Count > 0 ? (1, player1Cards) : (2, player2Cards);
        }

        private static string GetConfiguration(List<int> player1Cards, List<int> player2Cards)
        {
            return $"P1:{string.Join(",", player1Cards)}|P2:{string.Join(",", player2Cards)}";
        }

        private static int CalculateScoreFromList(List<int> cards)
        {
            var score = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                // Bottom card (last in list) has multiplier 1, top card has highest multiplier
                var multiplier = cards.Count - i;
                score += cards[i] * multiplier;
            }
            return score;
        }
    }
}
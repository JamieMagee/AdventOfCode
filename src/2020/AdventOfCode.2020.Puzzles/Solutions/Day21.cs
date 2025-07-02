namespace AdventOfCode._2020.Puzzles.Solutions
{
    [Puzzle("Allergen Assessment")]
    public sealed class Day21 : SolutionBase
    {
        public override Task<string> Part1Async(string input)
        {
            var foods = ParseFoods(input);
            var allergenCandidates = FindAllergenCandidates(foods);
            var safeIngredients = FindSafeIngredients(foods, allergenCandidates);

            // Count occurrences of safe ingredients in all foods
            int count = 0;
            foreach (var food in foods)
            {
                count += food.Ingredients.Count(ingredient => safeIngredients.Contains(ingredient));
            }

            return Task.FromResult(count.ToString());
        }

        public override Task<string> Part2Async(string input)
        {
            var foods = ParseFoods(input);
            var allergenCandidates = FindAllergenCandidates(foods);
            var allergenToIngredient = ResolveAllergens(allergenCandidates);

            // Sort by allergen name and join ingredient names
            var sortedAllergens = allergenToIngredient.Keys.OrderBy(x => x).ToList();
            var dangerousIngredients = sortedAllergens.Select(allergen => allergenToIngredient[allergen]);

            return Task.FromResult(string.Join(",", dangerousIngredients));
        }

        private static List<Food> ParseFoods(string input)
        {
            var foods = new List<Food>();
            var lines = input.Trim().Split('\n');

            foreach (var line in lines)
            {
                var parts = line.Split(" (contains ");
                var ingredients = parts[0].Split(' ').ToHashSet();
                var allergens = new HashSet<string>();

                if (parts.Length > 1)
                {
                    var allergensPart = parts[1].TrimEnd(')');
                    allergens = allergensPart.Split(", ").ToHashSet();
                }

                foods.Add(new Food { Ingredients = ingredients, Allergens = allergens });
            }

            return foods;
        }

        private static Dictionary<string, HashSet<string>> FindAllergenCandidates(List<Food> foods)
        {
            var allergenCandidates = new Dictionary<string, HashSet<string>>();

            foreach (var food in foods)
            {
                foreach (var allergen in food.Allergens)
                {
                    if (!allergenCandidates.TryGetValue(allergen, out var candidates))
                    {
                        allergenCandidates[allergen] = new HashSet<string>(food.Ingredients);
                    }
                    else
                    {
                        // Intersect with current candidates - only ingredients present in ALL foods with this allergen
                        candidates.IntersectWith(food.Ingredients);
                    }
                }
            }

            return allergenCandidates;
        }

        private static HashSet<string> FindSafeIngredients(List<Food> foods, Dictionary<string, HashSet<string>> allergenCandidates)
        {
            var allIngredients = new HashSet<string>();
            var possibleDangerousIngredients = new HashSet<string>();

            foreach (var food in foods)
            {
                allIngredients.UnionWith(food.Ingredients);
            }

            foreach (var candidates in allergenCandidates.Values)
            {
                possibleDangerousIngredients.UnionWith(candidates);
            }

            var safeIngredients = new HashSet<string>(allIngredients);
            safeIngredients.ExceptWith(possibleDangerousIngredients);

            return safeIngredients;
        }

        private static Dictionary<string, string> ResolveAllergens(Dictionary<string, HashSet<string>> allergenCandidates)
        {
            var resolved = new Dictionary<string, string>();
            var candidates = new Dictionary<string, HashSet<string>>();

            // Create a copy to work with
            foreach (var kvp in allergenCandidates)
            {
                candidates[kvp.Key] = new HashSet<string>(kvp.Value);
            }

            // Keep resolving until all allergens are mapped to exactly one ingredient
            while (candidates.Count > 0)
            {
                // Find allergens with only one possible ingredient
                var singleCandidates = candidates.Where(kvp => kvp.Value.Count == 1).ToList();

                foreach (var kvp in singleCandidates)
                {
                    var allergen = kvp.Key;
                    var ingredient = kvp.Value.First();

                    resolved[allergen] = ingredient;
                    candidates.Remove(allergen);

                    // Remove this ingredient from all other allergen candidates
                    foreach (var otherCandidates in candidates.Values)
                    {
                        otherCandidates.Remove(ingredient);
                    }
                }
            }

            return resolved;
        }

        private class Food
        {
            public HashSet<string> Ingredients { get; set; } = new HashSet<string>();
            public HashSet<string> Allergens { get; set; } = new HashSet<string>();
        }
    }
}
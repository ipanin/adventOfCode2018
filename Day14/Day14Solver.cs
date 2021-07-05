using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day14
{
    public static class Day14Solver
    {
        public static string Part1(int nRecipes) {
            var scores = new List<int>() {3, 7};
            int a = 0;
            int b = 1;
            
            do {
                var newRecipeScore = scores[a] + scores[b];
                if (newRecipeScore >= 10)
                    scores.Add(1);
                scores.Add(newRecipeScore % 10);
                a = (a + 1 + scores[a]) % scores.Count;
                b = (b + 1 + scores[b]) % scores.Count;
            } while (scores.Count < nRecipes + 10);

            return string.Join("", scores.Skip(nRecipes).Take(10).Select(r => r.ToString()));
        }

        public static int Part2(string requiredSequence) {
            var target = requiredSequence.Select(c => c - '0').ToArray();
            var targetLength = target.Length;
            var scores = new List<int>() {3, 7};
            int a = 0;
            int b = 1;
            
            while (true) {
                var newRecipeScore = scores[a] + scores[b];
                
                if (newRecipeScore >= 10) {
                    scores.Add(1);
                    if (target.SequenceEqual(scores.TakeLast(targetLength)))
                        return scores.Count - requiredSequence.Length;
                }

                scores.Add(newRecipeScore % 10);
                if (target.SequenceEqual(scores.TakeLast(targetLength)))
                    return scores.Count - requiredSequence.Length;

                a = (a + 1 + scores[a]) % scores.Count;
                b = (b + 1 + scores[b]) % scores.Count;
            } 
        }
    }
    
    class Day14Tests {
        [TestCase(5, "0124515891")]
        [TestCase(9, "5158916779")]
        [TestCase(18, "9251071085")]
        [TestCase(2018, "5941429882")]
        [TestCase(540561, "1413131339")]
        public void Part1Test(int nRecipes, string expectedRecipes) {
            string answer = Day14Solver.Part1(nRecipes);
            Assert.AreEqual(expectedRecipes, answer, "Wrong answer");
        }
		
        [TestCase("01245", 5)]
        [TestCase("51589", 9)]
        [TestCase("92510", 18)]
        [TestCase("59414", 2018)]
        [TestCase("540561", 20254833)] // < 844410531
        public void Part2Test(string recipesSequence, int expectedRecipesLeft) {
            var sw = Stopwatch.StartNew();
            int answer = Day14Solver.Part2(recipesSequence);
            sw.Stop();
            Console.WriteLine($"\n[{sw.Elapsed}] ");
            Assert.AreEqual(expectedRecipesLeft, answer, "Wrong answer");
        }
    }
}
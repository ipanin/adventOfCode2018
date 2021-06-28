using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day05 {
	static class Day05Solver {
		static string React(string formula) {
			while (formula.Length > 1) {
				var newFormula = new StringBuilder();
				int i;
				for (i = 1; i < formula.Length; i++) {
					var a = formula[i - 1];
					var b = formula[i];
					if (a != b && char.ToLower(a) == char.ToLower(b))
						i++;
					else
						newFormula.Append(a);
				}

				if (i == formula.Length)
					newFormula.Append(formula[formula.Length - 1]);

				if (newFormula.Length == formula.Length)
					break; // no reaction

				formula = newFormula.ToString();
			}

			return formula;
		}

		public static int Part1(string formula) {
			// var formula = "EfFMZFfzrRqQHhzpJjRrsSPZFfmVvAacOxXkKCcdxXcCulLeGgEIiZyGgYAaFfaAzaFuUfKkVvFNDdnGgnNGoOFfChaATtvVHcNgAaoOGJjrRq";
			var f1 = React(formula);
			//Console.WriteLine($"Part 1. Formula length={f1.Length}"); // 11242
			return f1.Length;
		}
		
		public static int Part2(string formula) {
			string RemoveCaseInsensitive(string source, char c) {
				return new string(source.Where(s => char.ToLower(s) != c).ToArray());
			}
			
			var f1 = React(formula);
			int min = f1.Length;
			for (char c = 'a'; c <= 'z'; c++) {
				if (!f1.Contains(c) && !f1.Contains(char.ToUpper(c)))
					continue;
				var f2 = RemoveCaseInsensitive(f1, c);
				var r = React(f2);
				if (r.Length < min)
					min = r.Length;
			}

			//Console.WriteLine($"Part 2. Min={min}"); // 5492
			return min;
		}
	}
	
	public class Day05Tests {
		[TestCase("Day05/sample.txt", 10)]
		[TestCase("Day05/input.txt", 11242)]
		public void Part1Test(string fileName, long expected) {
			var formula = Utils.GetTestString(fileName);
			var answer = Day05Solver.Part1(formula);
			Assert.AreEqual(expected, answer, "Wrong answer");
		}
        
		[TestCase("Day05/sample.txt", 4)]
		[TestCase("Day05/input.txt", 5492)]
		public void Part2Test(string fileName, long expected) {
			var formula = Utils.GetTestString(fileName);
			var answer = Day05Solver.Part2(formula);
			Assert.AreEqual(expected, answer, "Wrong answer");
		}
	}
}
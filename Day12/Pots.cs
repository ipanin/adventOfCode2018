using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day12
{
	class Day12Solver
	{
		public static (BitArray initial, bool[] rules) ParseInput(string[] rawInput) {
			var input = rawInput.Select(s => 
				s.Replace('.', '0').Replace('#', '1'));
			
			var initial = GetInitialState(input.First());
			var rules = GetRules(input.Skip(2));
			return (initial, rules);
		}

		// initial state: #..######..#....#####..
		private static BitArray GetInitialState(string state) {
			var s = state.Split(' ')[2];
			var source = new BitArray(s.Select(c => (c - '0') == 1).ToArray());
			return source;
		}

		// rule format: "00011 => 1"
		private static bool[] GetRules(IEnumerable<string> rules) {
			var result = new bool[32];
			foreach (var r in rules) {
				var words = r.Split(' ');
				var source = Convert.ToByte(words[0], 2);
				var next = words[2] == "1";
				result[source] = next;
			}

			return result;
		}

		public static long Solve(BitArray initial, bool[] rules, long numGenerations) {
			const long SustainableGeneration = 125; // visually found that after g=125 picture only moves right

			int maxLen = initial.Length;
			var generation = new BitArray(maxLen * 6, false);
			for (int i = 0; i < maxLen; i++)
				generation[i + maxLen] = initial[i];
			
			PrintGeneration(0, generation, maxLen);

			for (long g = 1; g <= numGenerations; g++) {
				generation = NextGeneration(rules, generation);
				PrintGeneration(g, generation, maxLen);
				if (g == SustainableGeneration)
					break;
			}

			long sum = 0;
			var positiveCount = 0;
			for (int i = 0; i < generation.Length; i++)
				if (generation[i]) {
					sum += i - maxLen;
					positiveCount++;
				}

			if (numGenerations > 20)
				sum = sum + (numGenerations - SustainableGeneration) * positiveCount;

			return sum;
		}

		static void PrintGeneration(long num, BitArray generation, int maxLen) {
			Console.Write($"{num}: ");
			for (int i = maxLen; i < 4 * maxLen; i++)
				Console.Write(generation[i] ? '|' : '.');
			Console.WriteLine();
		}

		static BitArray NextGeneration(bool[] rules, BitArray gen) {
			var result = (BitArray) gen.Clone();
			for (int i = 2; i < gen.Length - 2; i++) {
				var pattern = new BitVector32(0);
				for (int j = 0; j < 5; j++)
					pattern[1 << j] = gen[4 - j + i - 2];
				result[i] = rules[pattern.Data];
			}

			return result;
		}
	}
	
	class Day12Tests {
		[TestCase("Day12/sample.txt", 20, 325)]
		[TestCase("Day12/input.txt", 20, 2736)] // part1
		[TestCase("Day12/input.txt", 50_000_000_000, 3150000000905)] // part2
		public void Part1Test(string fileName, long numGenerations, long expectedSum) {
			var rawInput = Utils.GetTestStrings(fileName);
			var input = Day12Solver.ParseInput(rawInput);
			var answer = Day12Solver.Solve(input.initial, input.rules, numGenerations);
			Assert.AreEqual(expectedSum, answer, "Wrong answer");
		}
	}
}
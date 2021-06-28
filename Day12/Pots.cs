using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day12
{
	class Pots
	{
		private BitArray _initial;
		private bool[] _rules;
		private int _maxLen;
		private BitArray _generation;
		private long _generationNumber;

		public Pots(string[] rawInput) {
			var input = rawInput.Select(s => 
				s.Replace('.', '0').Replace('#', '1'));
			
			_initial = GetInitialState(input.First());
			_maxLen = _initial.Length;
			_rules = GetRules(input.Skip(2));
			
			InitFirstGeneration();
		}

		public long Solve(long numGenerations) {
			const long SustainableGeneration = 125; // visually found that after g=125 picture only moves right

			PrintGeneration();

			for (long gen = 1; gen <= numGenerations; gen++) {
				NextGeneration();
				PrintGeneration();
				if (gen == SustainableGeneration)
					break;
			}

			long sum = 0;
			var positiveCount = 0;
			for (int i = 0; i < _generation.Length; i++)
				if (_generation[i]) {
					sum += i - _maxLen;
					positiveCount++;
				}

			if (numGenerations > 20)
				sum = sum + (numGenerations - SustainableGeneration) * positiveCount;

			return sum;
		}

		private void InitFirstGeneration() {
			_generationNumber = 0;
			_generation = new BitArray(_maxLen * 6, false);
			for (int i = 0; i < _maxLen; i++)
				_generation[i + _maxLen] = _initial[i];
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
		
		private void PrintGeneration() {
			Console.Write($"{_generationNumber}: ");
			for (int i = _maxLen; i < 4 * _maxLen; i++)
				Console.Write(_generation[i] ? '|' : '.');
			Console.WriteLine();
		}

		private void NextGeneration() {
			var result = (BitArray) _generation.Clone();
			for (int i = 2; i < _generation.Length - 2; i++) {
				var pattern = new BitVector32(0);
				for (int j = 0; j < 5; j++)
					pattern[1 << j] = _generation[4 - j + i - 2];
				result[i] = _rules[pattern.Data];
			}

			_generation = result;
			_generationNumber++;
		}
	}
	
	class Day12Tests {
		[TestCase("Day12/sample.txt", 20, 325)]
		[TestCase("Day12/input.txt", 20, 2736)] // part1
		[TestCase("Day12/input.txt", 50_000_000_000, 3150000000905)] // part2
		public void Test(string fileName, long numGenerations, long expectedSum) {
			var rawInput = Utils.GetTestStrings(fileName);
			var pots = new Pots(rawInput);
			var answer = pots.Solve(numGenerations);
			Assert.AreEqual(expectedSum, answer, "Wrong answer");
		}
	}
}
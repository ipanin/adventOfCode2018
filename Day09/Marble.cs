using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Aoc.Day09
{
	public struct TestCase
	{
		public int Players;
		public int Points;
		public long HighScore;
			
		// test case sample:
		// "10 players; last marble is worth 1618 points: high score is 8317"
		public static TestCase Parse(string testCase) {
			var words = testCase.Split(' ').ToArray();
			return new TestCase {
				Players = int.Parse(words[0]),
				Points = int.Parse(words[6]),
				HighScore = words.Length == 12 ? int.Parse(words[11]) : 0
			};
		}
	}
	
	static class Day09Solver
	{
		public static long Solve(int numPlayers, int numMarbles) {
			Console.Write($"[{DateTime.Now.ToLongTimeString()}] Calculate {numMarbles}. ");

			var watch = Stopwatch.StartNew();
			var hiScore = CalculateHiScore(numPlayers, numMarbles);
			watch.Stop();

			Console.WriteLine($"Elapsed: {watch.Elapsed}. HiScore = {hiScore}");
			return hiScore;
		}

		static long CalculateHiScore(int numPlayers, int numMarbles) {
			var players = new long[numPlayers];
			var circle = new LinkedList<int>();

			circle.AddLast(0);
			var currentMarble = circle.First;

			for (int marble = 1; marble <= numMarbles; marble++) {
				players[marble % numPlayers] += DoMove(circle, ref currentMarble, marble);
			}

			return players.Max();
		}

		static int DoMove(LinkedList<int> circle, ref LinkedListNode<int> currentMarble, int marble) {
			if (marble % 23 == 0) {
				return marble + TakeMarble(circle, ref currentMarble);
			}

			InsertMarble(circle, ref currentMarble, marble);
			return 0;
		}

		static void InsertMarble(LinkedList<int> circle, ref LinkedListNode<int> currentMarble, int marble) {
			currentMarble = circle.AddAfter(
				currentMarble.Next ?? circle.First,
				marble);
		}

		static int TakeMarble(LinkedList<int> circle, ref LinkedListNode<int> currentMarble) {
			for (int i = 0; i < 6; i++) {
				currentMarble = currentMarble.Previous ?? circle.Last;
			}

			var removeNode = currentMarble.Previous ?? circle.Last;
			circle.Remove(removeNode);

			return removeNode.Value;
		}
	}

	class Day09Tests
	{
		[Test]
		public void SampleTest() {
			var rawInput = Utils.GetTestStrings("Day09/sample.txt");
			foreach (var str in rawInput) {
				var testCase = TestCase.Parse(str);
				var answer = Day09Solver.Solve(testCase.Players, testCase.Points);
				Assert.AreEqual(testCase.HighScore, answer, "Wrong answer");
			}
		}

		// 428 players; last marble is worth 72061 points
		[TestCase("Day09/input.txt", 409832)]
		public void Part1Test(string fileName, long expected) {
			var rawInput = Utils.GetTestString(fileName);
			var testCase = TestCase.Parse(rawInput);
			var answer = Day09Solver.Solve(testCase.Players, testCase.Points);
			Assert.AreEqual(expected, answer, "Wrong answer");
		}

		// 428 players; last marble is worth 7206100 points
		[TestCase("Day09/input.txt", 3469562780)]
		public void Part2Test(string fileName, long expected) {
			var rawInput = Utils.GetTestString(fileName);
			var testCase = TestCase.Parse(rawInput);
			var answer = Day09Solver.Solve(testCase.Players, testCase.Points * 100);
			Assert.AreEqual(expected, answer, "Wrong answer");
		}
	}
}
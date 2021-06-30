using NUnit.Framework;

namespace AdventOfCode.Y2018.Day13
{
	class Map {
		private string[] _map;
		private (int x, int y, char direction)[] _carts;
		
		public Map(string[] data) {
			foreach(var s in data) {
                //if (pos = s.IndexOf('<') != -1) {
                //}
            }
		}
	}
	
	public class Day13Solver
	{
		public static (int x, int y) Part1(string[] rawInput) {
			var map = new Map(rawInput);
			var answer = (0,0);
			return answer;
		}
	}
	
	class Day13Tests {
		[TestCase("Day13/sample.txt", 7, 3)]
		[TestCase("Day13/input.txt", 0, 0)]
		public void Part1Test(string fileName, int x, int y) {
			var expected = (x, y);
			var rawInput = Utils.GetTestStrings(fileName);
			var answer = Day13Solver.Part1(rawInput);
			Assert.AreEqual(expected, answer, "Wrong answer");
		}
	}
}




using NUnit.Framework;
using System;
using System.IO;

namespace Aoc
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1Simple()
        {
			var simpleData = Advent.GetTestData(Advent.TestData2018CachePath + "13_simple_input.txt");
			var map = new Map(simpleData);
			Test1("Test with sample data", map, (7, 3));
        }

        [Test]
        public void Test1My()
        {
			var data = Advent.GetTestData("https://adventofcode.com/2018/day/13/input",
				Advent.TestData2018CachePath + "13_input.txt");
			var map = new Map(data);
			Test1("Part 1", map, (0, 0));
        }

		private void Test1(string testName, Map map, (int, int) expectedCoord) {
			Console.WriteLine($"\n{testName}.");

			var coord = (0, 0);
			
			Assert(coord, expectedCoord, "coord");
		}

		private void Assert(object actual, object expected, string name) {
			Console.WriteLine($"{name}={actual}");
			//Assert()
			if (actual != expected)
				Console.WriteLine($"Error: invalid {name}. Expected {expected}");
		}

    }

	class Map {
		string[] _map;
		(int x, int y, char direction)[] _carts;
		
		public Map(string[] data) {
			foreach(var s in data) {
                if (pos = s.IndexOf('<') != -1) {
                    
                }
            }
		}
	}
}




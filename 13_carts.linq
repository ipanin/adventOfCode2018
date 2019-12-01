<Query Kind="Program">
  <Namespace>System.Collections.Specialized</Namespace>
</Query>

void Main() {
	var simpleData = Advent.GetTestData(Advent.TestData2018CachePath + "13_simple_input.txt");
	var map = new Map(simpleData);
	Test("Test with sample data", map, (7, 3));

	var data = Advent.GetTestData("https://adventofcode.com/2018/day/13/input",
			Advent.TestData2018CachePath + "13_input.txt");
	map = new Map(data);
	Test("Part 1", map, (0, 0));
}

class Map {
	string[] _map;
	(int x, int y, char direction)[] _carts;
	
	public Map(string[] data) {
		
	}
}

void Assert(object actual, object expected, string name) {
	Console.WriteLine($"{name}={actual}");
	if (actual != expected)
		Console.WriteLine($"Error: invalid {name}. Expected {expected}");
}

void Test(string testName, Map map, (int, int) expectedCoord) {
	Console.WriteLine($"\n{testName}.");

	var coord = (0, 0);
	
	Assert(coord, expectedCoord, "coord");
}

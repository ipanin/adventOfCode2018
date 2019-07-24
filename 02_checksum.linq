<Query Kind="Statements">
  <Namespace>System.Net</Namespace>
</Query>

// luojygedpvsthptkxiwnaorzmq
bool ContainsSame(string test, int count)
{
	var dict = new Dictionary<char, int>();
	foreach(var c in test) {
		if (dict.TryGetValue(c, out var value))
			dict[c] = ++value;
		else
			dict[c] = 1;
	}
	
	foreach (var key in dict) {
		if(key.Value == count)
			return true;
	}
	
	return false;
}

string[] testData = Advent.GetTestData(
	"https://adventofcode.com/2018/day/2/input",
	Advent.TestData2018CachePath + "02_input.txt");

// count A*B, where
// A = number of strings that contains letter which appears exactly twice
// B = number of strings that contains letter which appears exactly 3 times
var A = 0;
var B = 0;
foreach (var s in testData)
{
	if (ContainsSame(s, 2))
		A++;
	if (ContainsSame(s, 3))
		B++;
}

Console.WriteLine($"Checksum is: {A*B}"); //4712
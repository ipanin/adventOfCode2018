<Query Kind="Statements">
  <Namespace>System.Net</Namespace>
</Query>

// s1=luojygedpvsthptkxiwnaorzmq
// Diff at one letter
bool Diff1(string s1, string s2, out string diff)
{
	var sb = new StringBuilder();
	int diffLetters = 0;
	for (int i=0; i < s1.Length; i++) {
		if (s1[i] == s2[i]) {
			sb.Append(s1[i]);			
		}
		else if (++diffLetters > 1) {
			diff = "";
			return false;
		}
	}
	
	diff = sb.ToString();
	return true;
}

string[] testData = Advent.GetTestData(
	"https://adventofcode.com/2018/day/2/input",
	Advent.TestData2018CachePath + "02_input.txt");

foreach (var s1 in testData) {
	Console.Write('.');
	foreach (var s2 in testData) {
		if (s1 != s2 && Diff1(s1, s2, out var diff)) {
			Console.WriteLine($"Diff is: {diff}"); // lufjygedpvfbhftxiwnaorzmq
			return;
		}
	}
}
<Query Kind="Statements">
  <Namespace>System.Net</Namespace>
</Query>

long[] testData = Advent.GetTestData(
	"https://adventofcode.com/2018/day/1/input",
	Advent.TestData2018CachePath + "01_input.txt")
	.Select(long.Parse)
	.ToArray();

testData.Sum().Dump("Sum"); // Part 1 answer is 576

// part 2

var sums = new HashSet<long>();
long sum = 0;
var scans = 0;
for(var i=0;;) {
	sum += testData[i];
	if (!sums.Add(sum)) {
		break; // already exist
	}

	if (++i >= testData.Length)
	{
		scans++;
		i = 0;
		Console.WriteLine($"Sum={sum}");
	}
};

Console.WriteLine($"Found: {sum} after {scans} search(es)"); // 77674
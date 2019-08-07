<Query Kind="Program" />

int[] ParseData(string data) {
	return data.Split(' ').Select(Int32.Parse).ToArray();
}

IEnumerable<int> GetMetadata(int[] license, ref int pos) {
	var result = new List<int>();
	var nodeCount = license[pos++];
	var metadataCount = license[pos++];
	
	for (int i = 0; i < nodeCount; i++)
		result.AddRange(GetMetadata(license, ref pos));
				
	for (int j = 0; j < metadataCount; j++)
		result.Add(license[pos++]);

	return result;
}

int GetValue(int[] license, ref int pos) {
	int result = 0;
	var nodeCount = license[pos++];
	var metadataCount = license[pos++];
	if (nodeCount == 0) {
		for (int j = 0; j < metadataCount; j++) {
			result += license[pos++];
		}
	}
	else {
		var childNodes = new int[nodeCount];

		for (int i = 0; i < nodeCount; i++)
			childNodes[i] = GetValue(license, ref pos);

		for (int j = 0; j < metadataCount; j++) {
			var data = license[pos++];
			if (data <= nodeCount && data >= 1) // numbers from 1
				result += childNodes[data-1];
		}
	}

	return result;
}

void TestPart1(int[] input, long expectedSum) {
	int pos = 0;
	var sum = GetMetadata(input, ref pos).Sum();
	
	Console.WriteLine($"Part 1. Metadata sum={sum}");
	if (sum != expectedSum)
		Console.WriteLine($"Error: expected sum={expectedSum}");
}

void TestPart2(int[] input, long expected) {
	int pos = 0;
	var sum = GetValue(input, ref pos);
	
	Console.WriteLine($"Part 2. Node sum={sum}");
	if (sum != expected)
		Console.WriteLine($"Error: expected sum={expected}");
}

void Main() {
	var simpleData = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
	var simpleInput = ParseData(simpleData);

	TestPart1(simpleInput, 138);
	TestPart2(simpleInput, 66);

	var data = Advent.GetTestString("https://adventofcode.com/2018/day/8/input",
			Advent.TestData2018CachePath + "08_input.txt");
			
	var input = ParseData(data);
	TestPart1(input, 42768);
	TestPart2(input, 34348);
}
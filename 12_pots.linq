<Query Kind="Program">
  <Namespace>System.Collections.Specialized</Namespace>
</Query>

void Main() {
	var simpleData = Advent.GetTestData(
		Advent.TestData2018CachePath + "12_simple_input.txt");
	
	var simpleInput = simpleData.Select(s => s.Replace('.', '0').Replace('#', '1'));
	var initial = GetInitialState(simpleInput.First());
	var rules = GetRules(simpleInput.Skip(2));
	
	Test("Test with sample data", initial, rules, 20, 325);

	var data = Advent.GetTestData("https://adventofcode.com/2018/day/12/input",
			Advent.TestData2018CachePath + "12_input.txt");

	var input = data.Select(s => s.Replace('.', '0').Replace('#', '1'));
	initial = GetInitialState(input.First());
	rules = GetRules(input.Skip(2));
	Test("Part 1", initial, rules, 20, 2736);

	Test("Part 2", initial, rules, 50000000000, 3150000000905);
}

// initial state: #..######..#....#####..
BitArray GetInitialState(string state) {
	var s = state.Split(' ')[2];
	var source = new BitArray(s.Select(c => (c - '0') == 1).ToArray());
	return source;
}

// rule format: "00011 => 1"
bool[] GetRules(IEnumerable<string> rules) {
	var result = new bool[32];
	foreach (var r in rules) {
		var words = r.Split(' ');
		var source = Convert.ToByte(words[0], 2);
		var next = words[2] == "1";
		result[source] = next;
	}
	return result;
}

void Test(string testName, BitArray initial, bool[] rules, long numGenerations, long expectedSum) {
	Console.WriteLine($"\n{testName}. Generations {numGenerations}.");

	int Len = initial.Length;
	var generation = new BitArray(Len * 6, false);
	for (int i = Len; i < 2 * Len; i++)
		generation[i] = initial[i - Len];
	PrintGeneration(0, generation, Len);

	for (long g = 1; g <= numGenerations; g++) {
		generation = NextGeneration(rules, generation);
		PrintGeneration(g, generation, Len);
		if (g == 125)
			break;
	}

	long sum = 0; 
	var count = 0;
	for (int i = 0; i < generation.Length; i++)
		if (generation[i]) {
			sum += i - Len;
			count++;
		}

	if (numGenerations > 20)
		sum = sum + (numGenerations - 125) * count;

	Console.WriteLine($"Sum={sum}");
	if (sum != expectedSum)
		Console.WriteLine($"Error: expected sum={expectedSum}");

}

void PrintGeneration(long num, BitArray generation, int Len) {
	Console.Write($"{num}: ");
	for (int i = Len; i < 4 * Len; i++)
		Console.Write(generation[i] ? '|' : '.');
	Console.WriteLine();
}

BitArray NextGeneration(bool[] rules, BitArray gen) {
	var result = (BitArray)gen.Clone();
	for (int i = 2; i < gen.Length - 2; i++) {
		var pattern = new BitVector32(0);
		for (int j = 0; j < 5; j++)
			pattern[1 << j] = gen[4 - j + i - 2];
		result[i] = rules[pattern.Data];
	}
	return result;
}


<Query Kind="Program" />

class Link {
	public Link(char start, char end) {
		Start = start;
		End = end;
	}
	
	// Line sample: Step C must be finished before step A can begin.
	public static Link Parse(string instruct) {
		var c = instruct.Split(' ');
		return new Link(char.Parse(c[1]), char.Parse(c[7]));
	}
	
	public char Start;
	public char End;
}

Link[] ParseData(string[] data) {
	return data.Select(Link.Parse).ToArray();
}

void TestPart1(IEnumerable<Link> input, string expected) {
	var links = input.ToList();
	var instruct = new StringBuilder();
	
	while (links.Count() > 1)
	{
		var node = FindFirstFreeNode(links);
		instruct.Append(node);
		RemoveLinksStartingFromNode(links, node);
	}
	
	instruct.Append(links[0].Start);
	instruct.Append(links[0].End);
	
	var answer = instruct.ToString();
	Console.WriteLine($"Part 1. Instruction={answer}");

	if (answer != expected)
		Console.WriteLine($"Error: expected instruction={expected}");
}

static char FindFirstFreeNode(List<Link> links) {
	for (char c = 'A'; c <= 'Z'; c++) {
		if (links.Any(l => l.Start == c) && !links.Any(l => l.End == c))
			return c;
	}
	throw new Exception("Cycled graph.");
}

static void RemoveLinksStartingFromNode(List<Link> links, char node) {
	var count = links.RemoveAll(l => l.Start == node);
}

void Main() {
	var simpleData = new string[] {
			"Step C must be finished before step A can begin.",
			"Step C must be finished before step F can begin.",
			"Step A must be finished before step B can begin.",
			"Step A must be finished before step D can begin.",
			"Step B must be finished before step E can begin.",
			"Step D must be finished before step E can begin.",
			"Step F must be finished before step E can begin."
	};
	
	var simpleInput = ParseData(simpleData);
	
	TestPart1(simpleInput, "CABDFE");
//	TestPart2(simpleInput, 0, 2,"CABFDE", 15);

	var data = Advent.GetTestData("https://adventofcode.com/2018/day/7/input",
			Advent.TestData2018CachePath + "07_input.txt");
	var input = ParseData(data);
	TestPart1(input, "DFOQPTELAYRVUMXHKWSGZBCJIN");
//	TestPart2(input, 60, 5, "", 0);

}
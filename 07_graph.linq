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

class Worker{
	public Worker() { RemainingWork = 0; NodeName = (char)0; }
	public int RemainingWork;
	public char NodeName;
/*
	public void TryGetWork(List<Link> links, int weight) {
		NodeName = GetFreeNodeInWork(links);
		if (NodeName != 0)
			RemainingWork = weight + NodeName;
	}

	private char GetFreeNodeInWork(List<Link> links) {
		for (char c = 'A'; c <= 'Z'; c++) {
			if (links.Any(l => l.Start == c) && !links.Any(l => l.End == c))
				return c;
		}
		throw new Exception("Cycled graph.");
	}
*/
}

class WorkerPool {
	private List<Link> _links;
	private Worker[] _workers;
	private int _weight;
	public string Instructions = "";
	public int TotalEffort = 0;
	
	public WorkerPool(IEnumerable<Link> input, int number, int weight) {
		_links = input.ToList();
		//_workers = new Worker[number];
		_workers =  Enumerable.Range(1, number).Select(t => new Worker()).ToArray();
		_weight = weight;
	}
	
	public void Tick() {
		foreach (var w in _workers.Where(i=>i.NodeName == 0)) {
			TryGetWork(w);
		}
	}

	public void TryGetWork(Worker w) {
		w.NodeName = GetFreeNodeInWork();
		if (w.NodeName != 0)
			w.RemainingWork = _weight + w.NodeName;
	}

	private char GetFreeNodeInWork() {
		for (char c = 'A'; c <= 'Z'; c++) {
			if (_links.Any(l => l.Start == c) && !_links.Any(l => l.End == c) && !_workers.Any(w=>w.NodeName == c))
				return c;
		}
		return (char)0;
	}

	public void Tok() {
		TotalEffort++;
		foreach (var w in _workers.Where(i=>i.NodeName != 0).OrderBy(i=>i.NodeName)) {
			if (--w.RemainingWork == 0)
				CompleteWork(w);
		}
	}

	private void CompleteWork(Worker w) {
		Instructions += w.NodeName;
		if (_links.Count() > 1) {
			var count = _links.RemoveAll(l => l.Start == w.NodeName);
			w.NodeName = (char)0;
		}
		else {
			RemoveLinksStartingFromNode(_links, w.NodeName);
			w.NodeName = (char)0;
		}

	}
	
	public bool Done() {
		return !_links.Any();
	}
}

void TestPart2(IEnumerable<Link> input, int weight, int numWorkers, string expectedInstruction, int expectedDuration) {
/*	var links = new List<Link>(input);
	var workers = new int[numWorkers];
	var instruct = new StringBuilder();

	while (links.Count() > 1) {
		var nodes = FindFreeNodes(links);
		AssignToWorkers(workers, nodes);
		
		var effort = weight + (node - 'A' + 1);
		instruct.Append(node);
		RemoveLinksStartingFromNode(links, node);
	}

	instruct.Append(links[0].Start);
	instruct.Append(links[0].End);
*/
	var workers = new WorkerPool(input, numWorkers, weight);
	while (!workers.Done())
	{
		workers.Tick();
		workers.Tok();
	}
	var instruct = workers.Instructions;

	Console.WriteLine($"Part 2. Instruction={instruct}");
	if (instruct.ToString() != expectedInstruction)
		Console.WriteLine($"Error: expected Instruction={expectedInstruction}");

	Console.WriteLine($"Part 2. Duration={workers.TotalEffort}");
	if (workers.TotalEffort != expectedDuration)
		Console.WriteLine($"Error: expected duration={expectedDuration}");
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
	TestPart2(simpleInput, 0, 2,"CABFDE", 15);
/*
	var data = Advent.GetTestData("https://adventofcode.com/2018/day/7/input",
			Advent.TestData2018CachePath + "07_input.txt");
	var input = ParseData(data);
	TestPart1(input, "DFOQPTELAYRVUMXHKWSGZBCJIN");
	TestPart2(input, 60, 5, "", 0);
*/
}
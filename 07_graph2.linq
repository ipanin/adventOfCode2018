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

class Node {
	public char Name;
	public int RemainingWork;
	public bool WorkInProgress;
	public List<char> PrevNodes;
	
	public Node(char name, int weight) {
		Name = name;
		RemainingWork = weight + name;
		WorkInProgress = false;
		PrevNodes = new List<char>();
	}
}

class Graph {
	public Graph(int weight) {
		_weight = weight;
		Nodes = new Dictionary<char, Node>();
	}
	
	public void AddLink(Link link) {
//		var startNode = new Node(link.Start, _weight);
//		Nodes.TryGetValue(link.Start, out startNode)
//		Nodes[link.Start] = startNode;
		
		if (!Nodes.ContainsKey(link.Start))
			Nodes[link.Start] = new Node(link.Start, _weight);

		if (!Nodes.TryGetValue(link.End, out var endNode)) {
			endNode = new Node(link.End, _weight);
			Nodes[link.End] = endNode;
		}
		endNode.PrevNodes.Append(link.Start)
	}
	
	public Dictionary<char, Node> Nodes;
	private int _weight;
}

Link[] ParseData(string[] data) {
	return data.Select(Link.Parse).ToArray();
}

class WorkerPool {
	private Graph _graph;
	private char[] _workers;
	public string Instructions = "";
	public int TotalEffort = 0;
	
	public WorkerPool(Graph input, int number) {
		_graph = input;
		_workers = new char[number];
	}

	public void Tick() {
		var nodes = GetFreeNodes().OrderBy(nameof=>nameof.Name);
		if (!nodes.Any())
			return;
		for(int i=0; i<_workers.Length; i++) {
			if (_workers[i] == 0)
				GetWork(i, nodes);
		}
	}

	public void Tok() {
		TotalEffort++;
		for (int i = 0; i < _workers.Length; i++) {
			if (_workers[i] != 0) {
				var nodeName = _workers[i];
				if (--_graph.Nodes[nodeName].RemainingWork == 0) {
					CompleteWork(nodeName);
					_workers[i] = 0;
				}
			}
		}

		foreach (var w in _workers.Where(i => i.NodeName != 0).OrderBy(i => i.NodeName)) {
			if (--w.RemainingWork == 0)
				CompleteWork(w);
		}
	}

	public bool Done() {
		return !_links.Any();
	}

	private char GetFreeNode() {
		for (char c = 'A'; c <= 'Z'; c++) {
			if (_links.Any(l => l.Start == c) && !_links.Any(l => l.End == c) && !_workers.Any(w => w.NodeName == c))
				return c;
		}
		return (char)0;
	}

	private void GetWork(int worker) {
		if (w.NodeName != 0)
			w.RemainingWork = _weight + w.NodeName;
	}

	private void CompleteWork(Worker w) {
		Instructions += w.NodeName;
		if (_links.Count() > 1) {
			var count = _links.RemoveAll(l => l.Start == w.NodeName);
			w.NodeName = (char)0;
		}
		else {
			//RemoveLinksStartingFromNode(_links, w.NodeName);
			w.NodeName = (char)0;
		}

	}
}

void TestPart2(IEnumerable<Link> input, int weight, int numWorkers, string expectedInstruction, int expectedDuration) 
{
	var graph = new Graph(weight);
	foreach(var link in input) {
		graph.AddLink(link);
	}
	
	var workers = new WorkerPool(graph, numWorkers);
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
	
	TestPart2(simpleInput, 0, 2,"CABFDE", 15);

	var data = Advent.GetTestData("https://adventofcode.com/2018/day/7/input",
			Advent.TestData2018CachePath + "07_input.txt");
	var input = ParseData(data);
	TestPart2(input, 60, 5, "", 0);

}
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

class Node {
	public char Name;
	public int RemainingWork;
	public bool WorkInProgress;
	public List<char> PrevNodes;
	
	public Node(char name, int weight) {
		Name = name;
		RemainingWork = weight + name - 'A' + 1;
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
		if (!Nodes.ContainsKey(link.Start))
			Nodes[link.Start] = new Node(link.Start, _weight);

		if (!Nodes.TryGetValue(link.End, out var endNode)) {
			endNode = new Node(link.End, _weight);
			Nodes[link.End] = endNode;
		}
		endNode.PrevNodes.Add(link.Start);
	}
	
	public Dictionary<char, Node> Nodes;
	private int _weight;
}

class WorkerPool {
	private Graph _graph;
	private List<Node> _workers;
	private int _maxWorkersNumber;
	public string Instructions = "";
	public int TotalEffort = 0;
	
	public WorkerPool(Graph input, int number) {
		_graph = input;
		_workers = new List<Node>(number);
		for (int i=0; i<number; i++)
			_workers.Add(null);
		_maxWorkersNumber = number;
	}

	public void Tick() {
		var availableNodesEnumerator = GetAvailableNodes().OrderBy(nameof=>nameof.Name).GetEnumerator();

		for (int i = 0; i < _maxWorkersNumber; i++) {
			if (_workers[i] == null || _workers[i].RemainingWork == 0) {
				if (!availableNodesEnumerator.MoveNext())
					return;
				_workers[i] = availableNodesEnumerator.Current;
				_workers[i].WorkInProgress = true;
			}
		}
	}

	public void Tok() {
		TotalEffort++;
		foreach(var node in _workers.Where(w=> w != null && w.RemainingWork != 0).OrderBy(w => w.Name)) {
			if (--node.RemainingWork == 0) {
				CompleteWork(node);
			}
		}
	}

	public bool Done() {
		return !_graph.Nodes.Any(n => n.Value.RemainingWork > 0);
	}

	private IEnumerable<Node> GetAvailableNodes() {
		return _graph.Nodes.Values.Where(n => !n.PrevNodes.Any() && !n.WorkInProgress && n.RemainingWork > 0);
	}

	private void CompleteWork(Node node) {
		Instructions += node.Name;
		_graph.Nodes.Values.ToList().ForEach(n => n.PrevNodes.Remove(node.Name));
	}
}

void TestPart2(List<Link> input, int weight, int numWorkers, string expectedInstruction, int expectedDuration) 
{
	var graph = new Graph(weight);
	input.ForEach(graph.AddLink);

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
	
	var simpleInput = ParseData(simpleData).ToList();
	
	TestPart2(simpleInput, 0, 2,"CABFDE", 15);

	var data = Advent.GetTestData("https://adventofcode.com/2018/day/7/input",
			Advent.TestData2018CachePath + "07_input.txt");
	var input = ParseData(data).ToList();
	TestPart2(input, 60, 5, "DFOYQTRVELPAUMXHKWSGZBCJIN", 1036);
}
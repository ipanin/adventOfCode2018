using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day07 {
	class Link {
		public Link(char start, char end) {
			Start = start;
			End = end;
		}

		// Line sample:
		// Step C must be finished before step A can begin.
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
			RemainingWork = weight + name - 'A' + 1;
			WorkInProgress = false;
			PrevNodes = new List<char>();
		}
	}

	
	class Graph {
		public Graph(int startWeight) {
			this._startWeight = startWeight;
			Nodes = new Dictionary<char, Node>();
		}

		public void AddLink(Link link) {
			if (!Nodes.ContainsKey(link.Start))
				Nodes[link.Start] = new Node(link.Start, _startWeight);

			if (!Nodes.TryGetValue(link.End, out var endNode)) {
				endNode = new Node(link.End, _startWeight);
				Nodes[link.End] = endNode;
			}

			endNode.PrevNodes.Add(link.Start);
		}

		public Dictionary<char, Node> Nodes;
		private readonly int _startWeight;
	}

	
	class WorkerPool {
		private readonly Graph _graph;
		private readonly List<Node> _workers;
		private readonly int _maxWorkersNumber;
		public string Instructions = "";
		public int TotalEffort;

		public WorkerPool(Graph input, int number) {
			_graph = input;
			_workers = new List<Node>(number);
			for (int i = 0; i < number; i++)
				_workers.Add(null);
			_maxWorkersNumber = number;
		}

		public void Tick() {
			using var availableNodesEnumerator = GetAvailableNodes()
				.OrderBy(nameof => nameof.Name)
				.GetEnumerator();

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
			foreach (var node in _workers.Where(w => w != null && w.RemainingWork != 0).OrderBy(w => w.Name)) {
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

	static class Day07Solver {
		public static List<Link> LoadData(string fileName) {
			var testData = Utils.GetTestStrings(fileName);
			return testData.Select(Link.Parse).ToList();
		}

		public static string Part1(List<Link> input) {
			static char FindFirstFreeNode(List<Link> links) {
				for (char c = 'A'; c <= 'Z'; c++) {
					var c1 = c;
					if (links.Any(l => l.Start == c1) && links.All(l => l.End != c1))
						return c;
				}
				throw new Exception("Cycled graph.");
			}
			
			var instruct = new StringBuilder();
	
			while (input.Count > 1)
			{
				var node = FindFirstFreeNode(input);
				instruct.Append(node);
				input.RemoveAll(l => l.Start == node);
			}
	
			instruct.Append(input[0].Start);
			instruct.Append(input[0].End);
	
			return instruct.ToString();
		}

		public static (int, string) Part2(List<Link> input, int startWeight, int numWorkers) {
			var graph = new Graph(startWeight);
			input.ForEach(graph.AddLink);

			var workers = new WorkerPool(graph, numWorkers);
			while (!workers.Done()) {
				workers.Tick();
				workers.Tok();
			}
			
			var duration = workers.TotalEffort;
			var instruct = workers.Instructions;
			
			//Console.WriteLine($"Part 2. Duration={duration}");
			//Console.WriteLine($"Part 2. Instruction={instruct}");
			
			return (duration, instruct);
		}
	}
	
	
	class Day07Tests {
		[TestCase("Day07/sample.txt", "CABDFE")]
		[TestCase("Day07/input.txt", "DFOQPTELAYRVUMXHKWSGZBCJIN")]
		public void Part1Test(string fileName, string expectedInstruction) {
			var input = Day07Solver.LoadData(fileName);
			var instruction = Day07Solver.Part1(input);
			Assert.AreEqual(expectedInstruction, instruction, "Wrong instruction");
		}
		
		[TestCase("Day07/sample.txt", 0, 2, 15, "CABFDE")]
		[TestCase("Day07/input.txt", 60, 5, 1036, "DFOYQTRVELPAUMXHKWSGZBCJIN")]
		public void Part2Test(string fileName, int startWeight, int numWorkers, int expectedDuration, string expectedInstruction) {
			var input = Day07Solver.LoadData(fileName);
			var (duration, instruction) = Day07Solver.Part2(input, startWeight, numWorkers);
			Assert.AreEqual(expectedDuration, duration, "Wrong duration");
			Assert.AreEqual(expectedInstruction, instruction, "Wrong instruction");
		}
	}
}
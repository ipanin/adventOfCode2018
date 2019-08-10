<Query Kind="Program" />

int[] ParseData(string data) {
	// data sample:
	// "10 players; last marble is worth 1618 points: high score is 8317"
	var words = data.Split(' ').ToArray();
	return new [] { 
		Int32.Parse(words[0]),
		Int32.Parse(words[6]),
		words.Length == 12 ? Int32.Parse(words[11]) : 0
	};
}

void Test(int numPlayers, int numMarbles, long expectedScore) {
	Console.Write($"[{DateTime.Now.ToLongTimeString()}] Calculate {numMarbles}. ");
	
	var watch = Stopwatch.StartNew();
	var hiScore = CalculateHiScore(numPlayers, numMarbles);
	watch.Stop();
	
	Console.WriteLine($"Elapsed: {watch.Elapsed}. HiScore = {hiScore}");
	if (hiScore != expectedScore)
		Console.WriteLine($"Error: expected HiScore = {expectedScore}");
}

long CalculateHiScore(int numPlayers, int numMarbles) {
	var players = new long[numPlayers];
	var circle = new LinkedList<int>();

	circle.AddLast(0);
	var currentMarble = circle.First;

	for (int marble = 1; marble <= numMarbles; marble++) {
		players[marble % numPlayers] += DoMove(circle, ref currentMarble, marble);
	}

	return players.Max();
}

int DoMove(LinkedList<int> circle, ref LinkedListNode<int> currentMarble, int marble) {
	if (marble % 23 == 0) {
		return marble + TakeMarble(circle, ref currentMarble);
	}

	InsertMarble(circle, ref currentMarble, marble);
	return 0;
}

void InsertMarble(LinkedList<int> circle, ref LinkedListNode<int> currentMarble, int marble) {
	currentMarble = circle.AddAfter(
		currentMarble.Next ?? circle.First,
		marble);
}

int TakeMarble(LinkedList<int> circle, ref LinkedListNode<int> currentMarble) {
	for (int i = 0; i < 6; i++) { 
		currentMarble = currentMarble.Previous ?? circle.Last;
	}
	
	var removeNode = currentMarble.Previous ?? circle.Last;
	circle.Remove(removeNode);
	
	return removeNode.Value;
}

void Main() {
	var simpleDataSet = new[] {
		"9 players; last marble is worth 25 points: high score is 32",
		"10 players; last marble is worth 1618 points: high score is 8317",
		"13 players; last marble is worth 7999 points: high score is 146373",
		"17 players; last marble is worth 1104 points: high score is 2764",
		"21 players; last marble is worth 6111 points: high score is 54718",
		"30 players; last marble is worth 5807 points: high score is 37305"
	};

	foreach (var i in simpleDataSet) {
		var simpleInput = ParseData(i);
		Test(simpleInput[0], simpleInput[1], simpleInput[2]);
	}

	var data = Advent.GetTestString("https://adventofcode.com/2018/day/9/input",
			Advent.TestData2018CachePath + "09_input.txt");
			
	var input = ParseData(data);
	// 428 players; last marble is worth 72061 points
	Test(input[0], input[1], 409832);
	
	// 428 players; last marble is worth 7 206 100 points
	Test(input[0], input[1]*100, 0); 
}
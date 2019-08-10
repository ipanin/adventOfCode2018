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

void TestPart1(int numPlayers, int last, int expectedScore) {
	var players = new int[numPlayers];
	var circle = new List<int>(last);
	
	//var currentPlayer = 0;
	var currentMarble = 0;

	for (int marble=0; marble <= last; marble++) {
		players[marble % numPlayers] += DoMove(circle, ref currentMarble, marble);
		if (marble % 100000 == 0)
			Console.Write(".");
	}
	
	var hiScore = players.Max();
	Console.WriteLine($"Part 1. HiScore = {hiScore}");
	if (hiScore != expectedScore)
		Console.WriteLine($"Error: expected HiScore = {expectedScore}");
}

int DoMove(List<int> circle, ref int currentMarble, int marble) {
	var result = 0;
	
	if (marble == 0) {
		circle.Add(marble);
		currentMarble = 0;
	}
	else if (marble % 23 == 0) {
		result = marble;
		result += TakeMarble(circle, ref currentMarble);
	}
	else {
		InsertMarble(circle, marble, ref currentMarble);
	}
	return result;
}

void InsertMarble(List<int> circle, int marble, ref int currentMarble) {
	currentMarble += 2;
//	if (currentMarble == circle.Count)
//		circle.Add(marble)
	if (currentMarble > circle.Count)
		currentMarble = currentMarble % circle.Count;

	circle.Insert(currentMarble, marble);
}

int TakeMarble(List<int> circle, ref int currentMarble) {
	currentMarble -= 7;
	//	if (currentMarble == circle.Count)
	//		circle.Add(marble)
	if (currentMarble < 0)
		currentMarble = circle.Count + currentMarble;

	var result = circle[currentMarble];
	circle.RemoveAt(currentMarble);
	return result;
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
		TestPart1(simpleInput[0], simpleInput[1], simpleInput[2]);
	}

	var data = Advent.GetTestString("https://adventofcode.com/2018/day/9/input",
			Advent.TestData2018CachePath + "09_input.txt");
			
	var input = ParseData(data);
	TestPart1(input[0], input[1], 409832);
//	TestPart1(input[0], input[1]*100, 0);
}
<Query Kind="Program" />

const int W = 300;
const int H = 300;

int[][] Grid(int serial) {
	var output = new int[H][];
	for (int y = 1; y <= H; y++) {
		output[y-1] = new int[W];
		for (int x = 1; x <= W; x++) {
			var rackId = x + 10;
			output[y-1][x-1] = (rackId*y + serial) * rackId % 1000 / 100 - 5;
		}
	}
	
	return output;
}

(int, int) FindLargest(int[][] grid) {
	int MaxPower = Int32.MinValue;
	var result = (0, 0);
	for (int i=0; i < H-3; i++) {
		for (int j = 0; j < W-3; j++) {
			int power = SumPower(grid, i, j);
			if (power > MaxPower) {
				MaxPower = power;
				result = (j+1, i+1);
			}
		}
	}
	return result;
}

int SumPower(int[][] grid, int ii, int jj) {
	int power = 0;
	for (int i = ii; i < ii+3; i++) {
		for (int j = jj; j < jj+3; j++) {
			power += grid[i][j];
		}
	}
	return power;
}

void TestGridPowerLevel(string testName, int serial, (int, int) coord, int expectedPower) {
	Console.WriteLine($"\n{testName}. Serial {serial}.");
	var grid = Grid(serial);
	var power = grid[coord.Item2-1][coord.Item1-1];
	Console.WriteLine($"coord={coord} power={power}");
	if (power != expectedPower)
		Console.WriteLine($"Error: expected power={expectedPower}");
}

void Test(string testName, int serial, (int, int) expected) {
	Console.WriteLine($"\n{testName}. Serial {serial}.");
	var grid = Grid(serial);
	var coord = FindLargest(grid);
	Console.WriteLine($"coord={coord}");
	if (coord != expected)
		Console.WriteLine($"Error: expected coord={expected}");
}


void Main() {
	TestGridPowerLevel("Test grid power", 57, (122, 79), -5);
	TestGridPowerLevel("Test grid power", 39, (217,196), 0);
	TestGridPowerLevel("Test grid power", 71, (101,153), 4);

	Test("Test with sample data", 18, (33, 45));
	Test("Test with sample data", 42, (21, 61));

	Test("My test", 5719, (21,34));
}
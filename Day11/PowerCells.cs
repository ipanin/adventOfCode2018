using System;
using System.Diagnostics;

namespace AdventOfCode.Y2018.Day11
{
	class Day11Solver
	{
		const int W = 300;
		const int H = 300;

		struct PowerSquare
		{
			public (int, int) coord;
			public int size;
			public int power;
		}

		int[][] Grid(int serial) {
			var output = new int[H][];
			for (int y = 1; y <= H; y++) {
				output[y - 1] = new int[W];
				for (int x = 1; x <= W; x++) {
					var rackId = x + 10;
					output[y - 1][x - 1] = (rackId * y + serial) * rackId % 1000 / 100 - 5;
				}
			}

			return output;
		}

		PowerSquare FindLargest(int[][] grid) {
			var result = new PowerSquare {power = int.MinValue, coord = (0, 0), size = 0};

			for (int i = 0; i < H; i++) {
				for (int j = 0; j < W; j++) {
					int prevPower = 0;
					for (int cellSize = 1; cellSize < H - i && cellSize < W - j; cellSize++) {
						int power = prevPower + AnglePower(grid, i, j, cellSize);

						if (power > result.power) {
							result.power = power;
							result.coord = (j + 1, i + 1);
							result.size = cellSize;
						}

						prevPower = power;
					}
				}

				if (i % 3 == 0)
					Console.Write('.');
			}

			return result;
		}

		int AnglePower(int[][] grid, int pos_i, int pos_j, int cellSize) {
			int power = 0;
			for (int i = pos_i; i < pos_i + cellSize; i++) {
				power += grid[i][pos_j + cellSize - 1];
			}

			for (int j = pos_j; j < pos_j + cellSize - 1; j++) {
				power += grid[pos_i + cellSize - 1][j];
			}

			return power;
		}

		void TestGrid(string testName, int serial, (int, int) coord, int expectedPower) {
			Console.WriteLine($"\n{testName}. Serial {serial}.");
			var grid = Grid(serial);
			var power = grid[coord.Item2 - 1][coord.Item1 - 1];
			Console.WriteLine($"coord={coord} power={power}");
			if (power != expectedPower)
				Console.WriteLine($"Error: expected power={expectedPower}");
		}

		void Part2(string testName, int serial, (int, int) expected, int expectedSize) {
			Console.WriteLine($"\n{testName}. Serial {serial}.");
			var grid = Grid(serial);
			var sw = Stopwatch.StartNew();
			var result = FindLargest(grid);
			sw.Stop();
			Console.WriteLine($"\n[{sw.Elapsed}] coord={result.coord}, size={result.size}");

			if (result.coord != expected)
				Console.WriteLine($"Error: expected coord={expected}");
			if (result.size != expectedSize)
				Console.WriteLine($"Error: expected size={expectedSize}");
		}


		void Main() {
			TestGrid("Test grid power", 57, (122, 79), -5);
			TestGrid("Test grid power", 39, (217, 196), 0);
			TestGrid("Test grid power", 71, (101, 153), 4);

			Part2("Test with sample data", 18, (90, 269), 16);
			Part2("Test with sample data", 42, (232, 251), 12);

			Part2("My test", 5719, (90, 244), 16);
		}
	}
}
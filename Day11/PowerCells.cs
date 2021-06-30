using System;
using System.Diagnostics;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day11
{
	public struct PowerSquare
	{
		public (int, int) coord;
		public int size;
		public int power;
	}
	
	public class PowerGrid
	{
		private static int[,] _powerLevels;
		private const int W = 300;
		private const int H = 300;
		
		public PowerGrid(int serial) {
			_powerLevels = new int[H,W];
			
			for (int y = 1; y <= H; y++) {
				for (int x = 1; x <= W; x++) {
					var rackId = x + 10;
					_powerLevels[y-1, x-1] = (rackId * y + serial) * rackId % 1000 / 100 - 5;
				}
			}
		}

		public int GetPowerLevel(int x, int y) {
			Assert.IsTrue( x is >= 1 and <= W && 
			               y is >= 1 and <= H );
			
			return _powerLevels[y - 1, x - 1];
		}
		
		public PowerSquare FindMaxPowerSquare(int sideLength) {
			int PowerSum(int x, int y, int sideLength) {
				int sum = 0;
				for (int i = y-1; i < y-1+sideLength; i++) {
					for (int j = x-1; j < x-1+sideLength; j++) {
						sum += _powerLevels[i,j];
					}
				}
				return sum;
			}
			
			var result = new PowerSquare {
				coord = (0,0),
				power = int.MinValue,
				size = sideLength
			};
			
			for (int i=0; i < H-sideLength; i++) {
				for (int j = 0; j < W-sideLength; j++) {
					int power = PowerSum(j+1, i+1, sideLength);
					if (power > result.power) {
						result.power = power;
						result.coord = (j+1, i+1);
					}
				}
			}
			
			return result;
		}
		
		public PowerSquare FindLargestSquare() {
			int AnglePowerSum(int pos_i, int pos_j, int cellSize) {
				int power = 0;
				for (int i = pos_i; i < pos_i + cellSize; i++) {
					power += _powerLevels[i, pos_j + cellSize - 1];
				}

				for (int j = pos_j; j < pos_j + cellSize - 1; j++) {
					power += _powerLevels[pos_i + cellSize - 1, j];
				}

				return power;
			}
			
			var result = new PowerSquare {power = int.MinValue, coord = (0, 0), size = 0};

			for (int i = 0; i < H; i++) {
				for (int j = 0; j < W; j++) {
					int prevPower = 0;
					for (int cellSize = 1; cellSize < H - i && cellSize < W - j; cellSize++) {
						int power = prevPower + AnglePowerSum(i, j, cellSize);

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
	}
	
	class Day11Tests {
		[TestCase(57, 122, 79, -5)]
		[TestCase(39, 217, 196, 0)]
		[TestCase(71, 101, 153, 4)] 
		public void PowerLevelTest(int serial, int x, int y, int expectedPowerLevel) {
			var grid = new PowerGrid(serial);
			Assert.AreEqual(expectedPowerLevel, grid.GetPowerLevel(x, y), "Wrong power level");
		}
		
		[TestCase(18, "33,45", 29)]
		[TestCase(42, "21,61", 30)]
		[TestCase(5719, "21,34", 29)]
		public void Part1Test(int serial, string expectedAnswer, int expectedPowerLevel) {
			var grid = new PowerGrid(serial);
			var max = grid.FindMaxPowerSquare(3);
			string answer = $"{max.coord.Item1},{max.coord.Item1}";
			Assert.AreEqual(expectedAnswer, answer, "Wrong answer");
			Assert.AreEqual(expectedPowerLevel, max.power, "Wrong power sum");
		}

		[TestCase(18, "90,269,16", 113)]
		[TestCase(42, "232,251,12", 119)]
		[TestCase(5719, "90,244,16", 124)]
		public void Part2Test(int serial, string expectedAnswer, int expectedPowerLevel) {
			Console.WriteLine($"\nTest serial {serial}.");
			
			var sw = Stopwatch.StartNew();
			var grid = new PowerGrid(serial);
			var result = grid.FindLargestSquare();
			sw.Stop();
			
			Console.WriteLine($"\n[{sw.Elapsed}] ");

			string answer = $"{result.coord.Item1},{result.coord.Item2},{result.size}";
			Assert.AreEqual(expectedAnswer, answer, "Wrong answer");
			Assert.AreEqual(expectedPowerLevel, result.power, "Wrong power sum");
		}
	}
}
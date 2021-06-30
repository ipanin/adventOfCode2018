using System;
using System.Diagnostics;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day11
{
	public struct PowerSquare
	{
		public (int x, int y) coord;
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
			
			for (int i = 0; i < H; i++) {
				for (int j = 0; j < W; j++) {
					int y = i + 1;
					int x = j + 1;
					var rackId = x + 10;
					_powerLevels[i, j] = (rackId * y + serial) * rackId % 1000 / 100 - 5;
				}
			}
		}

		public int GetPowerLevel(int x, int y) {
			Assert.IsTrue( x is >= 1 and <= W && 
			               y is >= 1 and <= H );
			
			return _powerLevels[y - 1, x - 1];
		}
		
		public PowerSquare FindMaxPowerSquare(int sideLength) {
			int PowerSum(int startx, int starty, int sideLength) {
				int i0 = starty - 1;
				int j0 = startx - 1;
				int sum = 0;
				for (int i = i0; i < i0+sideLength; i++) {
					for (int j = j0; j < j0+sideLength; j++) {
						sum += _powerLevels[i, j];
					}
				}
				return sum;
			}
			
			var result = new PowerSquare {
				coord = (0,0),
				power = int.MinValue,
				size = sideLength
			};
			
			for (int i = 0; i < H-sideLength; i++) {
				for (int j = 0; j < W-sideLength; j++) {
					int x = j + 1;				
					int y = i + 1;
					int power = PowerSum(x, y, sideLength);
					if (power > result.power) {
						result.power = power;
						result.coord = (x, y);
					}
				}
			}
			
			return result;
		}
		
		public PowerSquare FindLargestSquare() {
			int AnglePowerSum(int pos_i, int pos_j, int cellSize, int prevPowerSum) {
				int power = prevPowerSum;
				int lastCol = pos_j + cellSize - 1;
				int lastRow = pos_i + cellSize - 1;
				
				// sum right column
				for (int i = pos_i; i <= lastRow; i++) {
					power += _powerLevels[i, lastCol];
				}

				// sum bottom row without last element
				for (int j = pos_j; j < lastCol; j++) {
					power += _powerLevels[lastRow, j];
				}

				return power;
			}
			
			var result = new PowerSquare {
				power = int.MinValue, 
				coord = (0, 0), 
				size = 0
			};

			for (int i = 0; i < H; i++) {
				for (int j = 0; j < W; j++) {
					int power = 0;
					for (int cellSize = 1; cellSize < Math.Min(H - i, W - j); cellSize++) {
						power = AnglePowerSum(i, j, cellSize, power);

						if (power > result.power) {
							result.power = power;
							result.coord = (j + 1, i + 1);
							result.size = cellSize;
						}
					}
				}
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
		public void Part1Test(int input, string expectedAnswer, int expectedPowerLevel) {
			var grid = new PowerGrid(input);
			var max = grid.FindMaxPowerSquare(3);
			string answer = $"{max.coord.x},{max.coord.y}";
			Assert.AreEqual(expectedAnswer, answer, "Wrong answer");
			Assert.AreEqual(expectedPowerLevel, max.power, "Wrong power sum");
		}

		[TestCase(18, "90,269,16", 113)]
		[TestCase(42, "232,251,12", 119)]
		[TestCase(5719, "90,244,16", 124)]
		public void Part2Test(int input, string expectedAnswer, int expectedPowerLevel) {
			Console.WriteLine($"\nTest serial {input}.");
			
			var sw = Stopwatch.StartNew();
			var grid = new PowerGrid(input);
			var result = grid.FindLargestSquare();
			sw.Stop();
			
			Console.WriteLine($"\n[{sw.Elapsed}] ");

			string answer = $"{result.coord.x},{result.coord.y},{result.size}";
			Assert.AreEqual(expectedAnswer, answer, "Wrong answer");
			Assert.AreEqual(expectedPowerLevel, result.power, "Wrong power sum");
		}
	}
}
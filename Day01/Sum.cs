using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aoc
{
	class Day01Solver
	{
		private static long[] LoadData(string fileName)
		{
			long[] testData = Utils.GetTestStrings(fileName)
				.Select(long.Parse)
				.ToArray();
			
			return testData;
		}

		[TestCase("day01/input.txt", 576)]
		public void Part1Test(string fileName, long expected)
		{
			long[] testData = LoadData(fileName);
			var answer = testData.Sum();
			Assert.AreEqual(expected, answer, "Wrong answer");
		}
		
		[TestCase("day01/input.txt", 77674)]
		public void Part2Test(string fileName, long expected)
		{
			long[] testData = LoadData(fileName);
			
			var sums = new HashSet<long>();
			long sum = 0;
			var scans = 0;
			for (var i = 0;;)
			{
				sum += testData[i];
				if (!sums.Add(sum))
				{
					break; // already exist
				}

				if (++i >= testData.Length)
				{
					scans++;
					i = 0;
					//Console.WriteLine($"Sum={sum}");
				}
			}

			//Console.WriteLine($"Found: {sum} after {scans} search(es)"); // 77674

			Assert.AreEqual(expected, sum, "Wrong answer");
		}
	}
}
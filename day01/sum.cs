using System.Collections.Generic;
using System.Linq;

namespace Aoc
{
	class Day01Solver : Solver<long[], long>
	{
		protected override long[] LoadData()
		{
			long[] testData = Advent.GetTestStrings(WorkingDir()+"/input.txt")
				.Select(long.Parse)
				.ToArray();
			return testData;
		}

		protected override long Part1(long[] testData)
		{
			return testData.Sum(); // Part 1 answer is 576
		}

		protected override long Part2(long[] testData)
		{
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
			return sum;
		}
	}
}
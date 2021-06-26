using System.Collections.Generic;
using System.Text;

namespace Aoc
{
	internal class Day02Solver : Solver<string[], string>
	{
		protected override string[] LoadData()
		{
			var testData = Advent.GetTestStrings(WorkingDir() + "/input.txt");
			return testData;
		}

		// luojygedpvsthptkxiwnaorzmq
		static bool ContainsSame(string test, int count)
		{
			var dict = new Dictionary<char, int>();
			foreach (var c in test)
			{
				if (dict.TryGetValue(c, out var value))
					dict[c] = ++value;
				else
					dict[c] = 1;
			}

			foreach (var key in dict)
			{
				if (key.Value == count)
					return true;
			}

			return false;
		}

		protected override string Part1(string[] testData)
		{
			// count a*b, where
			// a = number of strings that contains letter which appears exactly twice
			// b = number of strings that contains letter which appears exactly 3 times
			var a = 0;
			var b = 0;
			foreach (var s in testData)
			{
				if (ContainsSame(s, 2))
					a++;
				if (ContainsSame(s, 3))
					b++;
			}

			//Console.WriteLine($"Checksum is: {A * B}"); //4712
			return (a * b).ToString();
		}

		// s1=luojygedpvsthptkxiwnaorzmq
		// Diff at one letter
		bool Diff1(string s1, string s2, out string diff)
		{
			var sb = new StringBuilder();
			int diffLetters = 0;
			for (int i=0; i < s1.Length; i++) {
				if (s1[i] == s2[i]) {
					sb.Append(s1[i]);			
				}
				else if (++diffLetters > 1) {
					diff = "";
					return false;
				}
			}
	
			diff = sb.ToString();
			return true;
		}
		
		protected override string Part2(string[] testData)
		{
			foreach (var s1 in testData)
			{
				//Console.Write('.');
				foreach (var s2 in testData)
				{
					if (s1 != s2 && Diff1(s1, s2, out var diff))
					{
						return diff; // lufjygedpvfbhftxiwnaorzmq
					}
				}
			}

			return "";
		}
	}
}
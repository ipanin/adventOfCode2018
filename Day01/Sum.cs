using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day01
{
    static class Day01Solver
    {
        public static long[] LoadData(string fileName) {
            long[] testData = Utils.GetTestStrings(fileName)
                .Select(long.Parse)
                .ToArray();

            return testData;
        }

        public static long Part1(long[] testData) => testData.Sum();

        public static long Part2(long[] testData) {
            var sums = new HashSet<long>();
            long sum = 0;
            //var scans = 0;

            for (var i = 0;;) {
                sum += testData[i];
                if (!sums.Add(sum)) {
                    break; // already exist
                }

                if (++i >= testData.Length) {
                    i = 0;
                    //scans++;
                    //Console.WriteLine($"Sum={sum}");
                }
            }

            //Console.WriteLine($"Found: {sum} after {scans} search(es)"); // 77674
            return sum;
        }
    }

    class Day01Tests
    {
        [TestCase("Day01/input.txt", 576)]
        public void Part1Test(string fileName, long expected) {
            long[] testData = Day01Solver.LoadData(fileName);
            var answer = Day01Solver.Part1(testData);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }

        [TestCase("Day01/input.txt", "77674")]
        public void Part2Test(string fileName, long expected) {
            long[] testData = Day01Solver.LoadData(fileName);
            var answer = Day01Solver.Part2(testData);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day08
{
    static class Day08Solver
    {
        public static int[] ParseInput(string rawInput) {
            return rawInput.Split(' ').Select(Int32.Parse).ToArray();
        }

        private static IEnumerable<int> GetMetadata(int[] license, ref int pos) {
            var result = new List<int>();
            var nodeCount = license[pos++];
            var metadataCount = license[pos++];

            for (int i = 0; i < nodeCount; i++)
                result.AddRange(GetMetadata(license, ref pos));

            for (int j = 0; j < metadataCount; j++)
                result.Add(license[pos++]);

            return result;
        }

        private static int GetValue(int[] license, ref int pos) {
            int result = 0;
            var nodeCount = license[pos++];
            var metadataCount = license[pos++];
            if (nodeCount == 0) {
                for (int j = 0; j < metadataCount; j++) {
                    result += license[pos++];
                }
            }
            else {
                var childNodes = new int[nodeCount];

                for (int i = 0; i < nodeCount; i++)
                    childNodes[i] = GetValue(license, ref pos);

                for (int j = 0; j < metadataCount; j++) {
                    var data = license[pos++];
                    if (data <= nodeCount && data >= 1) // numbers from 1
                        result += childNodes[data - 1];
                }
            }

            return result;
        }

        public static int Part1(int[] input) {
            int pos = 0;
            return GetMetadata(input, ref pos).Sum();
        }

        public static long Part2(int[] input) {
            int pos = 0;
            var nodeSum = GetValue(input, ref pos);
            return nodeSum;
        }
    }
    
    class Day08Tests {
        [TestCase("Day08/sample.txt", 138)]
        [TestCase("Day08/input.txt", 42768)]
        public void Part1Test(string fileName, int expected) {
            var rawInput = Utils.GetTestString(fileName);
            var input = Day08Solver.ParseInput(rawInput);
            var answer = Day08Solver.Part1(input);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
		
        [TestCase("Day08/sample.txt", 66)]
        [TestCase("Day08/input.txt", 34348)]
        public void Part2Test(string fileName, int expected) {
            var rawInput = Utils.GetTestString(fileName);
            var input = Day08Solver.ParseInput(rawInput);
            var answer = Day08Solver.Part2(input);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
    }
}
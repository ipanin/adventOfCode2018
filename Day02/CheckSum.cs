using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day02
{
    static class Day02Solver
    {
        public static string[] LoadData(string fileName) {
            var testData = Utils.GetTestStrings(fileName);
            return testData;
        }

        public static long Part1(IEnumerable<string> testData) {
            // luojygedpvsthptkxiwnaorzmq
            bool ContainsSame(string test, int count) {
                var dict = new Dictionary<char, int>();
                foreach (var c in test) {
                    if (dict.TryGetValue(c, out var value))
                        dict[c] = ++value;
                    else
                        dict[c] = 1;
                }

                foreach (var key in dict) {
                    if (key.Value == count)
                        return true;
                }

                return false;
            }

            // count a*b, where
            // a = number of strings that contains letter which appears exactly twice
            // b = number of strings that contains letter which appears exactly 3 times
            var a = 0;
            var b = 0;
            foreach (var s in testData) {
                if (ContainsSame(s, 2))
                    a++;
                if (ContainsSame(s, 3))
                    b++;
            }

            //Console.WriteLine($"Checksum is: {A * B}"); //4712
            return a * b;
        }

        public static string Part2(string[] testData) {
            // s1=luojygedpvsthptkxiwnaorzmq
            // Diff at one letter
            bool Diff1(string s1, string s2, out string diff) {
                var sb = new StringBuilder();
                int diffLetters = 0;
                for (int i = 0; i < s1.Length; i++) {
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

            foreach (var s1 in testData) {
                //Console.Write('.');
                foreach (var s2 in testData) {
                    if (s1 != s2 && Diff1(s1, s2, out var diff)) {
                        return diff; // lufjygedpvfbhftxiwnaorzmq
                    }
                }
            }

            return "";
        }
    }

    class Day02Tests
    {
        [TestCase("Day02/input.txt", 4712)]
        public void Part1Test(string fileName, long expected) {
            string[] testData = Day02Solver.LoadData(fileName);
            var answer = Day02Solver.Part1(testData);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }

        [TestCase("Day02/input.txt", "lufjygedpvfbhftxiwnaorzmq")]
        public void Part2Test(string fileName, string expected) {
            string[] testData = Day02Solver.LoadData(fileName);
            var answer = Day02Solver.Part2(testData);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
    }
}
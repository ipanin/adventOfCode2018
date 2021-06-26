using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Aoc.Day04 {
    public enum GuardState {
        Begins,
        Asleep,
        Wakeup
    };

    public struct Record {
        public string date;
        public int minute;
        public GuardState state;
        public int guardId;
    }

    public class Parser {
        private static readonly Regex LineRegex = new Regex(@"\[(.+)\s\d\d:(.+)\]\s(.+)",
            RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex StateRegex =
            new Regex(@"Guard #(\d+).+", RegexOptions.Singleline | RegexOptions.Compiled);

        private int _currentGuardId;

        // [1518-11-01 00:05] falls asleep
        public Record ParseRecord(string row) {
            /*	var nums = row
                    .Split(new[]{'[', ']'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                return new Record { guardId = nums[0], min = nums[1], state = nums[2] };
            */
            var match = LineRegex.Match(row);

            var result = new Record {
                date = match.Groups[1].Value,
                minute = int.Parse(match.Groups[2].Value),
                guardId = _currentGuardId
            };

            string stateString = match.Groups[3].Value;
            switch (stateString) {
                case "falls asleep":
                    result.state = GuardState.Asleep;
                    break;
                case "wakes up":
                    result.state = GuardState.Wakeup;
                    break;
                default:
                    result.state = GuardState.Begins;
                    _currentGuardId = int.Parse(StateRegex.Match(stateString).Groups[1].Value);
                    result.guardId = _currentGuardId;
                    break;
            }

            return result;
        }
    }

    public static class Day04Solver {
        private static Record[] LoadData(string fileName) {
            var parser = new Parser();
            var testData = Utils.GetTestStrings(fileName);
            return testData.OrderBy(r => r).Select(parser.ParseRecord).ToArray();
        }
        
        private static Dictionary<int, int[]> FillTable(Record[] records) {
            static void AddSleep(Dictionary<int, int[]> table, int guardId, int start, int end) {
                if (!table.TryGetValue(guardId, out var guardSleep))
                    guardSleep = new int[60];

                for (int i = start; i < end; i++)
                    guardSleep[i]++;

                table[guardId] = guardSleep;
            }
            
            var table = new Dictionary<int, int[]>();
            int startSleep = 0;
            foreach (var record in records) {
                if (record.state == GuardState.Asleep)
                    startSleep = record.minute;
                if (record.state == GuardState.Wakeup) {
                    AddSleep(table, record.guardId, startSleep, record.minute);
                }
            }

            return table;
        }
        
        public static int Part1(string fileName) {
            var records = LoadData(fileName);
            var table = FillTable(records);

            var guard = table.Aggregate((l, r)
                => (l.Value.Sum() > r.Value.Sum()) ? l : r);
            //guard.Value.Sum().Dump("Guard sleep");

            var vector = table[guard.Key];
            var minute = vector.ToList().IndexOf(vector.Max());
            var answer = guard.Key * minute;
            Console.WriteLine($"Part1: Guard {guard.Key}. Best minute {minute}. Answer {answer}.");
            // Part1: Guard 1777. Best minute 48. Answer 85296
            return answer;
        }
        
        public static int Part2(string fileName) {
            var records = LoadData(fileName);
            var table = FillTable(records);
            
            var guard = table.Aggregate((l, r) 
                => (l.Value.Max() > r.Value.Max()) ? l : r);
            var vector = table[guard.Key];
            var minute = vector.ToList().IndexOf(vector.Max());
            var answer = guard.Key * minute;
            Console.WriteLine($"Part2: Guard {guard.Key}. Best minute {minute}. Answer {answer}.");
            // Part2: Guard 1889. Best minute 31. Answer 58559.
            return answer;
        }
    }

    public class Day04Tests {
        [TestCase("day04/sample.txt", 240)]
        [TestCase("day04/input.txt", 85296)]
        public void Part1Test(string fileName, long expected) {
            var answer = Day04Solver.Part1(fileName);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
        
        [TestCase("day04/sample.txt", 4455)]
        [TestCase("day04/input.txt", 58559)]
        public void Part2Test(string fileName, long expected) {
            var answer = Day04Solver.Part2(fileName);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
    }
}
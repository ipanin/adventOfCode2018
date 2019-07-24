<Query Kind="Program">
  <Namespace>System.Net</Namespace>
</Query>

public enum GuardState {Begins, Asleep, Wakeup};

public struct Record {
	public string date;
	public int minute;
	public GuardState state;
	public int guardId;
}

public class Parser {
	private static Regex LineRegex = new Regex(@"\[(.+)\s\d\d:(.+)\]\s(.+)", RegexOptions.Singleline | RegexOptions.Compiled );
	private static Regex StateRegex = new Regex(@"Guard #(\d+).+", RegexOptions.Singleline| RegexOptions.Compiled );

	private int _currentGuardId = 0;
	
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

string[] GetTestData(bool simple)
{
	if (simple)
		return new[] {
			"[1518-11-01 00:00] Guard #10 begins shift",
			"[1518-11-01 00:05] falls asleep",
			"[1518-11-01 00:25] wakes up",
			"[1518-11-01 00:30] falls asleep",
			"[1518-11-01 00:55] wakes up",
			"[1518-11-01 23:58] Guard #99 begins shift",
			"[1518-11-02 00:40] falls asleep",
			"[1518-11-02 00:50] wakes up",
			"[1518-11-03 00:05] Guard #10 begins shift",
			"[1518-11-03 00:24] falls asleep",
			"[1518-11-03 00:29] wakes up",
			"[1518-11-04 00:02] Guard #99 begins shift",
			"[1518-11-04 00:36] falls asleep",
			"[1518-11-04 00:46] wakes up",
			"[1518-11-05 00:03] Guard #99 begins shift",
			"[1518-11-05 00:45] falls asleep",
			"[1518-11-05 00:55] wakes up"
		};

	return Advent.GetTestData(
			"https://adventofcode.com/2018/day/4/input",
			Advent.TestData2018CachePath + "04_input.txt");
}

void AddSleep(Dictionary<int, int[]> table, int guardId, int start, int end) {
	if (!table.TryGetValue(guardId, out var guardSleep))
		guardSleep = new int[60];

	for (int i = start; i < end; i++)
		guardSleep[i]++;
		
	table[guardId] = guardSleep;
}

void Main() {
	var parser = new Parser();
	var records = GetTestData(false).OrderBy(r => r).Select(parser.ParseRecord);

	var table = new Dictionary<int, int[]>();
	int startSleep = 0;
	foreach (var record in records) {
		if (record.state == GuardState.Asleep)
			startSleep = record.minute;
		if (record.state == GuardState.Wakeup) {
			AddSleep(table, record.guardId, startSleep, record.minute);
		}
	}

	var guard = table.Aggregate((l,r) => (l.Value.Sum() > r.Value.Sum()) ? l : r);
	//guard.Value.Sum().Dump("Guard sleep");
	
	var vector = table[guard.Key];
	var minute = vector.ToList().IndexOf(vector.Max());
	Console.WriteLine($"Part1: Guard {guard.Key}. Best minute {minute}. Answer {guard.Key*minute}.");
	// Part1: Guard 1777. Best minute 48. Answer 85296

	guard = table.Aggregate((l, r) => (l.Value.Max() > r.Value.Max()) ? l : r);
	vector = table[guard.Key];
	minute = vector.ToList().IndexOf(vector.Max());
	Console.WriteLine($"Part2: Guard {guard.Key}. Best minute {minute}. Answer {guard.Key * minute}.");
	// Part2: Guard 1889. Best minute 31. Answer 58559.
}
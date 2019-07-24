<Query Kind="Program">
  <Namespace>System.Net</Namespace>
</Query>

public struct Claim {
	public int id;
	public int left;
	public int top;
	public int width;
	public int height;
}

// #1281 @ 755,745: 10x19
Claim parseRecord(string row){
	var nums = row
		.Split(new[]{' ', '#', '@', ',', ':', 'x'}, StringSplitOptions.RemoveEmptyEntries)
		.Select(int.Parse)
		.ToArray();
	return new Claim { id = nums[0], left = nums[1], top = nums[2], width = nums[3], height = nums[4]};
}

string[] SimpleTestData()
{
	return new[] {
		"#1 @ 1,3: 4x4",
		"#2 @ 3,1: 4x4",
		"#3 @ 5,5: 2x2" };
}

void Main() {
	// string[] testData = SimpleTestData();
	string[] testData = Advent.GetTestData(
		"https://adventofcode.com/2018/day/3/input",
		Advent.TestData2018CachePath + "03_input.txt");

	// parseRecord("#1281 @ 755,745: 10x19").Dump();
	var claims = testData.Select(parseRecord);

	int H = 0;
	int W = 0;
	foreach (var claim in claims) {
		var w = claim.left + claim.width;
		var h = claim.top + claim.height;
		if (w > W) W = w;
		if (h > H) H = h;
	}
	Console.WriteLine($"Full size: {W}x{H}"); //1000x999

	var list = new int[W, H];
	foreach (var claim in claims)
		for (int i = claim.left; i < claim.left + claim.width; i++)
			for (int j = claim.top; j < claim.top + claim.height; j++)
				list[i, j]++;

	long square = 0;
	for (int i = 0; i < W; i++)
		for (int j = 0; j < H; j++)
			if (list[i, j] > 1)
				square++;

	Console.WriteLine($"Intersection square= {square}"); // 105231

	// part 2
	foreach (var claim in claims) {
		var single = true;
		for (int i = claim.left; i < claim.left + claim.width && single; i++)
			for (int j = claim.top; j < claim.top + claim.height && single; j++)
				if (list[i, j] != 1)
					single = false;

		if (single)
			Console.WriteLine($"Claim {claim.id} is OK"); // 164
	}
}
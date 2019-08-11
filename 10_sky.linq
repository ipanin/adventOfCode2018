<Query Kind="Program" />

class Point {
	public Point(int x, int y, int vx, int vy) {
		this.x = x; 
		this.y = y;
		this.vx = vx;
		this.vy = vy;
	}

	public static Point Parse(string position_velocity) {
		// data sample: "position=<-41933,  10711> velocity=< 4, -1>"
		var words = position_velocity.Split(new[] { '<', '>', ','}).ToArray();
		return new Point(
			Int32.Parse(words[1]),
			Int32.Parse(words[2]),
			Int32.Parse(words[4]),
			Int32.Parse(words[5])
		);
	}
	
	public void Move(int sec) {
		x += vx * sec;
		y += vy * sec;
	}

	public int x;
	public int y;
	public int vx;
	public int vy;
}

IEnumerable<Point> ParseData(string[] data) {
	return data.Select(Point.Parse);
}

class Sky {
	List<Point> _points;
	public Sky(IEnumerable<Point> points) {
		_points = points.ToList();
	}

	public void Move(int sec) {
		_points.ForEach(p => p.Move(sec));
	}

	public long GetSquareNorm() {
		long minX = _points.Min(p => p.x);
		long minY = _points.Min(p => p.y);
		long maxX = _points.Max(p => p.x);
		long maxY = _points.Max(p => p.y);
		return (maxY - minY) * (maxY - minY) + (maxX - minX) * (maxX - minX);
	}

	public void Print() {
		long minX = _points.Min(p => p.x);
		long minY = _points.Min(p => p.y);
		long maxX = _points.Max(p => p.x);
		long maxY = _points.Max(p => p.y);
		int W = (int)(maxX - minX + 1);
		int H = (int)(maxY - minY + 1);

		Console.WriteLine($"min X: {minX}; min Y: {minY}");

		var output = new char[H][];

		for (int i = 0; i < H; i++) {
			output[i] = new char[W];
			for (int j = 0; j < W; j++) {
				output[i][j] = '_';
			}
		}

		foreach (var p in _points) {
			output[p.y-minY][p.x-minX] = '*';
		}

		for (int i = 0; i < H; i++)
			Console.WriteLine(new string(output[i]));
	}
}


void Test(string testName, IEnumerable<Point> input, int expectedTimeToWait) {
	Console.WriteLine($"\n{testName}. {input.Count()} points");
	
	var sky = new Sky(input);
	int maxTimeToWait = 20000;

	// find when Norm is minimum
	long prevNorm = Int64.MaxValue;
	int elapsed;
	for (elapsed=1; elapsed <= maxTimeToWait; elapsed++) {
		sky.Move(1);
		var norm = sky.GetSquareNorm();
		if (norm > prevNorm)
			break;
		prevNorm = norm;
	}

	// return to prev step
	elapsed--;
	sky.Move(-1);

	Console.WriteLine($"Elapsed={elapsed} sec. Norm^2={prevNorm}");
	if (elapsed != expectedTimeToWait)
		Console.WriteLine($"Error: expected time = {expectedTimeToWait}");

	sky.Print();
}


void Main() {
	var simpleData = Advent.GetTestData(
		Advent.TestData2018CachePath + "10_simple_input.txt");
	var simpleInput = ParseData(simpleData);
	Test("Test with sample data", simpleInput, 3);


	var data = Advent.GetTestData(
		"https://adventofcode.com/2018/day/10/input",
		Advent.TestData2018CachePath + "10_input.txt");
	var input = ParseData(data);
	Test("My test", input, 10519);
}
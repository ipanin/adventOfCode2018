<Query Kind="Program" />

class Point {
	public Point(int x, int y) { this.x = x; this.y = y; }
	
	public static Point Parse(string coord) {
		var c = coord.Split(',');
		return new Point(int.Parse(c[0]), int.Parse(c[1]));
	}
	
	public int x;
	public int y;
}

Point[] ParseData(string[] data) {
	return data.Select(Point.Parse).ToArray();
}

class FieldWithDistance {
	private int[,] _field;
	private int[,] _distance;
	private int _sizeX;
	private int _sizeY;

	public FieldWithDistance(int maxX, int maxY) {
		_sizeX = maxX + 1;
		_sizeY = maxY + 1;
		_field = new int[_sizeY, _sizeX];
		for (var dy = 0; dy < _sizeY; dy++) {
			for (var dx = 0; dx < _sizeX; dx++) {
				_field[dy, dx] = -1;
			}
		}
		_distance = new int[_sizeY, _sizeX];
	}

	public void FillManhattanCircle(Point c, int coordN, int distance) {
		for (var dx = -distance; dx <= distance; dx++) {
			var dy = distance - Math.Abs(dx);
			var x = c.x + dx;
			var y = c.y + dy;
			FillPoint(x, y, coordN, distance);

			dy = -distance + Math.Abs(dx);
			x = c.x + dx;
			y = c.y + dy;
			FillPoint(x, y, coordN, distance);
		}
	}

	private void FillPoint(int x, int y, int coordN, int distance) {
		if (x >= 0 && x < _sizeX && y >= 0 && y < _sizeY) {
			if (_field[y, x] == -1) {
				_field[y, x] = coordN;
				_distance[y, x] = distance;
			}
			else if (_field[y, x] != coordN && _distance[y, x] == distance)
				_field[y, x] = 0;
		}
	}

	public void Print() {
		for (var dy = 0; dy < _sizeY; dy++) {
			for (var dx = 0; dx < _sizeX; dx++) {
				Console.Write(_field[dy, dx]);
			}
			Console.WriteLine();
		}
	}
	
	public void PrintAlpha() {
		for (var dy = 0; dy < _sizeY; dy++) {
			for (var dx = 0; dx < _sizeX; dx++) {
				Console.Write((char)('`' + _field[dy, dx]));
			}
			Console.WriteLine();
		}
	}
	
	public void PrintDebug() {
		_field.Dump();
	}

	public int GetLagestAreaSize() {
		var areaSizes = new Dictionary<int, int>(); // coordN, size. size==-1 => inf
		for (var dy = 0; dy < _sizeY; dy++) {
			for (var dx = 0; dx < _sizeX; dx++) {
				var coordN = _field[dy, dx];
				if (coordN == 0) continue;
				if (coordN == -1) throw new Exception($"uniniialized point ({dx},{dy})");
				if (!areaSizes.TryGetValue(coordN, out var sizeN))
					sizeN = 0;
					
				if (sizeN == -1) continue;
				if (dy == 0 || dy == _sizeY-1 || dx == 0 || dx == _sizeX-1)
					sizeN = -1; // Inf
				else
					sizeN++;
				
				areaSizes[coordN] = sizeN;
			}
		}
		
		return areaSizes.Max(a => a.Value);
	}
}

class Field {
	private int[,] _field;
	private int _sizeX;
	private int _sizeY;

	public Field(int maxX, int maxY) {
		_sizeX = maxX + 1;
		_sizeY = maxY + 1;
		_field = new int[_sizeY, _sizeX];
	}

	public void FillArea(Point[]coords, int maxDistance) {
		for (var dy = 0; dy < _sizeY; dy++) {
			for (var dx = 0; dx < _sizeX; dx++) {
				var p = new Point(dx, dy);
				var d = GetDistanceSum(p, coords);
				if (d < maxDistance)
					_field[dy, dx] = 1;
				else
					_field[dy, dx] = 0;
			}
		}
	}

	public static int GetDistanceSum(Point p, Point[] coords) {
		return coords.Sum(c => GetDistance(p, c));
	}

	private static int GetDistance(Point p, Point c) {
		return Math.Abs(p.x - c.x) + Math.Abs(p.y - c.y);
	}
	
	public int GetAreaSize() {
		return _field.Cast<int>().Count(c => c == 1);
	}
}


void TestPart1(Point[] coords, int expected) {
	var maxX = coords.Max(c => c.x);
	var maxY = coords.Max(c => c.y);

	var field = new FieldWithDistance(maxX, maxY);
	
	for (int distance = 0; distance < 2*Math.Max(maxX, maxY); distance++) {
		for (int coordN = 1; coordN <= coords.Length; coordN++) {
			var c = coords[coordN-1];
			field.FillManhattanCircle(c, coordN, distance);
		}
	}

	//field.PrintAlpha();
	var answer = field.GetLagestAreaSize();
	Console.WriteLine($"Part 1. lagest size={answer}");

	if (answer != expected)
		Console.WriteLine($"Error: expected={expected}, actual={answer}");
}

void TestDistance(Point p, Point[] coords, int expected){
	var answer = Field.GetDistanceSum(p, coords);
	Console.WriteLine($"Part 2. distance={answer}");

	if (answer != expected)
		Console.WriteLine($"Error: expected={expected}, actual={answer}");
}

void TestPart2(Point[] coords, int maxDistance, int expected) {
	var maxX = coords.Max(c => c.x);
	var maxY = coords.Max(c => c.y);

	var field = new Field(maxX, maxY);
	field.FillArea(coords, maxDistance);
	var answer = field.GetAreaSize();

	Console.WriteLine($"Part 2. Size={answer}");
	if (answer != expected)
		Console.WriteLine($"Error: expected={expected}, actual={answer}");
}

void Main() {
	var simpleData = new string[] {
			"1, 1",
			"1, 6",
			"8, 3",
			"3, 4",
			"5, 5",
			"8, 9"
		};
	var simpleInput = ParseData(simpleData);
	
	TestPart1(simpleInput, 17);
	TestDistance(new Point(4, 3), simpleInput, 30);
	TestPart2(simpleInput, 32, 16);

	var data = Advent.GetTestData("https://adventofcode.com/2018/day/6/input",
			Advent.TestData2018CachePath + "06_input.txt");
	var input = ParseData(data);
	TestPart1(input, 4011);
	TestPart2(input, 10000, 46054);
}
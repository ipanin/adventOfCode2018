<Query Kind="Program" />

string GetTestData(bool simple) {
	if (simple)
		return "dabAcCaCBAcCcaDA";
		
	return Advent.GetTestString("https://adventofcode.com/2018/day/5/input",
			Advent.TestData2018CachePath + "05_input.txt");
}

string React(string formula)
{
	while (formula.Length > 1) {
		var newFormula = new StringBuilder();
		int i;
		char b = '\0';
		for (i = 1; i < formula.Length; i++) {
			var a = formula[i - 1];
			b = formula[i];
			if (a != b && char.ToLower(a) == char.ToLower(b))
				i++;
			else
				newFormula.Append(a);
		}
		if (i == formula.Length)
			newFormula.Append(formula[formula.Length-1]);
			
		if (newFormula.Length == formula.Length)
			break; // no reaction
			
		formula = newFormula.ToString();
	}
	return formula;
}

string RemoveCaseInsensitive(string source, char c)
{
	return new string(source.Where(s=>char.ToLower(s)!=c).ToArray());
}

void Main() {
	// var formula = "EfFMZFfzrRqQHhzpJjRrsSPZFfmVvAacOxXkKCcdxXcCulLeGgEIiZyGgYAaFfaAzaFuUfKkVvFNDdnGgnNGoOFfChaATtvVHcNgAaoOGJjrRq";
	var formula = GetTestData(simple:false);
	var f1 = React(formula);
	Console.WriteLine($"Part 1. Formula length={f1.Length}"); // 11242
	
	int min = f1.Length;
	for (char c='a'; c<='z'; c++) {
		if (!f1.Contains(c) && !f1.Contains(char.ToUpper(c)))
			continue;
		var f2 = RemoveCaseInsensitive(f1, c);
		var r = React(f2);
		if (r.Length < min)
			min = r.Length;
	}
	Console.WriteLine($"Part 2. Min={min}"); // 5492
}
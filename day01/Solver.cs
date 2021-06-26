using System.IO;
using System.Text.RegularExpressions;

namespace Aoc
{
    public interface ISolver
    {
        void Solve();
    }
    
    abstract class Solver<TInput, TOutput> : ISolver
    {
        public int Day()
        {
            var re = new Regex(@"\d+");
            var match = re.Match(GetType().FullName);
            return int.Parse(match.Value);
        }

        public string WorkingDir()
        {
            return Path.Combine("day" + Day().ToString("00"));
        }

        protected abstract TInput LoadData();

        string[] LoadExpected()
        {
            return Advent.GetTestStrings(WorkingDir() + "/expected.txt");
        }

        protected abstract TOutput Part1(TInput input);

        protected abstract TOutput Part2(TInput input);

        public void Solve()
        {

            var data = LoadData();
            var expected = LoadExpected();

            var res = Part1(data);
            var day = Day();
            Advent.Assert(expected[0], res.ToString(), $"Day{day}. Part1");

            res = Part2(data);
            Advent.Assert(expected[1], res.ToString(), $"Day{day}. Part2");
        }
    }
}
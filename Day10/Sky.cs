using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day10
{
    public class Point
    {
        public Point(int x, int y, int vx, int vy) {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }

        public static Point Parse(string positionVelocity) {
            // data sample: "position=<-41933,  10711> velocity=< 4, -1>"
            var words = positionVelocity.Split(new[] {'<', '>', ','}).ToArray();
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

    public class Sky
    {
        private readonly List<Point> _points;

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
            int W = (int) (maxX - minX + 1);
            int H = (int) (maxY - minY + 1);

            Console.WriteLine($"min X: {minX}; min Y: {minY}");

            var output = new char[H][];

            for (int i = 0; i < H; i++) {
                output[i] = new char[W];
                for (int j = 0; j < W; j++) {
                    output[i][j] = '_';
                }
            }

            foreach (var p in _points) {
                output[p.y - minY][p.x - minX] = '*';
            }

            for (int i = 0; i < H; i++)
                Console.WriteLine(new string(output[i]));
        }
    }

    class Day10Solver
    {
        public static IEnumerable<Point> ParseInput(string[] rawInput) {
            return rawInput.Select(Point.Parse);
        }
        
        public static int Solve(IEnumerable<Point> input) {
            var sky = new Sky(input);
            int maxTimeToWait = 20000;

            // find when Norm is minimum
            long prevNorm = long.MaxValue;

            int elapsed;
            for (elapsed = 1;
                elapsed <= maxTimeToWait;
                elapsed++) {
                sky.Move(1);
                var norm = sky.GetSquareNorm();
                if (norm > prevNorm)
                    break;
                prevNorm = norm;
            }

            // return to prev step
            elapsed--;
            sky.Move(-1);
            //Console.WriteLine($"Elapsed={elapsed} sec. Norm^2={prevNorm}");
            sky.Print();
            return elapsed;
        }
    }
    
    
    class Day08Tests {
        [TestCase("Day10/sample.txt", 3)]
        [TestCase("Day10/input.txt", 10519)]
        public void Part2Test(string fileName, int expected) {
            var rawInput = Utils.GetTestStrings(fileName);
            var input = Day10Solver.ParseInput(rawInput);
            var answer = Day10Solver.Solve(input);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
    }
}
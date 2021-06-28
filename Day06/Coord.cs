using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day06
{
    class Point
    {
        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public static Point Parse(string coord) {
            var c = coord.Split(',');
            return new Point(int.Parse(c[0]), int.Parse(c[1]));
        }

        public int x;
        public int y;
    }

    class FieldWithDistance
    {
        private readonly int[,] _field;
        private readonly int[,] _distance;
        private readonly int _sizeX;
        private readonly int _sizeY;

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
                    Console.Write((char) ('`' + _field[dy, dx]));
                }

                Console.WriteLine();
            }
        }

        public int GetLargestAreaSize() {
            var areaSizes = new Dictionary<int, int>(); // coordN, size. size==-1 => inf
            for (var dy = 0; dy < _sizeY; dy++) {
                for (var dx = 0; dx < _sizeX; dx++) {
                    var coordN = _field[dy, dx];
                    if (coordN == 0) continue;
                    if (coordN == -1) throw new Exception($"uninitialized point ({dx},{dy})");
                    if (!areaSizes.TryGetValue(coordN, out var sizeN))
                        sizeN = 0;

                    if (sizeN == -1) continue;
                    if (dy == 0 || dy == _sizeY - 1 || dx == 0 || dx == _sizeX - 1)
                        sizeN = -1; // Inf
                    else
                        sizeN++;

                    areaSizes[coordN] = sizeN;
                }
            }

            return areaSizes.Max(a => a.Value);
        }
    }

    class Field
    {
        private readonly int[,] _field;
        private readonly int _sizeX;
        private readonly int _sizeY;

        public Field(int maxX, int maxY) {
            _sizeX = maxX + 1;
            _sizeY = maxY + 1;
            _field = new int[_sizeY, _sizeX];
        }

        public void FillArea(Point[] coords, int maxDistance) {
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

    static class Day06Solver
    {
        public static Point[] LoadData(string fileName) {
            var testData = Utils.GetTestStrings(fileName);
            return testData.Select(Point.Parse).ToArray();
        }

        public static int Part1(Point[] coords) {
            var maxX = coords.Max(c => c.x);
            var maxY = coords.Max(c => c.y);

            var field = new FieldWithDistance(maxX, maxY);

            for (int distance = 0; distance < 2 * Math.Max(maxX, maxY); distance++) {
                for (int coordN = 1; coordN <= coords.Length; coordN++) {
                    var c = coords[coordN - 1];
                    field.FillManhattanCircle(c, coordN, distance);
                }
            }

            //field.PrintAlpha();
            var answer = field.GetLargestAreaSize();
            //Console.WriteLine($"Part 1. largest size={answer}");
            return answer;
        }


        public static int Part2(Point[] coords, int maxDistance) {
            var maxX = coords.Max(c => c.x);
            var maxY = coords.Max(c => c.y);

            var field = new Field(maxX, maxY);
            field.FillArea(coords, maxDistance);
            var answer = field.GetAreaSize();

            //Console.WriteLine($"Part 2. Size={answer}");
            return answer;
        }
    }


    public class Day06Tests
    {
        [TestCase("Day06/sample.txt", 17)]
        [TestCase("Day06/input.txt", 4011)]
        public void Part1Test(string fileName, int expected) {
            var coords = Day06Solver.LoadData(fileName);
            var answer = Day06Solver.Part1(coords);

            Assert.AreEqual(expected, answer, "Wrong answer");
        }

        [TestCase("Day06/sample.txt", 30)]
        public void Part2DistanceTest(string fileName, int expected) {
            var coords = Day06Solver.LoadData(fileName);
            var distance = Field.GetDistanceSum(new Point(4, 3), coords);
            Assert.AreEqual(expected, distance, "Error in GetDistanceSum() calculation");
        }

        [TestCase("Day06/sample.txt", 32, 16)]
        [TestCase("Day06/input.txt", 10000, 46054)]
        public void Part2Test(string fileName, int maxDistance, int expected) {
            var coords = Day06Solver.LoadData(fileName);
            var answer = Day06Solver.Part2(coords, maxDistance);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
    }
}
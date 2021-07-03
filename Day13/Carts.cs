using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AdventOfCode.Y2018.Day13
{
    public class Coord : IComparable<Coord>
    {
        public int x;
        public int y;

        public Coord(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int CompareTo(Coord? other) {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;
            return GetHashCode() - other.GetHashCode();
        }

        public override bool Equals(Object obj) {
            if (obj == null || !(obj is Coord))
                return false;

            var c = (Coord) obj;
            return x == c.x && y == c.y;
        }

        public override int GetHashCode() {
            return x + y * 1000; // for x in [0..999]
        }

        public override string ToString() {
            return $"({x},{y})";
        }
    }

    public class Cart : IComparable
    {
        public Cart(char c, Coord coord) {
            direction = c switch {
                '^' => Direction.North,
                '>' => Direction.East,
                'v' => Direction.South,
                '<' => Direction.West,
                _ => throw new ArgumentException($"Invalid argument {c} passed to Cart ctor")
            };
            this.coord = coord;
        }

        public char ToChar() {
            return "^>v<"[(int)direction];
        }

        public int CompareTo(object? other) {
            if (other is not Cart cart) return 1;
            return coord.GetHashCode() - cart.coord.GetHashCode();
        }

        public void Move() {
            var vel = _velocity[(int)direction];
            coord.x += vel.x;
            coord.y += vel.y;
        }

        public void Rotate(char map) {
            int directionChange = map switch {
                '+' => _nextTurn,
                '/' => (direction is Direction.North or Direction.South)? 1 : -1,
                '\\' => (direction is Direction.North or Direction.South)? -1 : 1,
                '-' or '|' => 0,
                _ => throw new Exception($"unexpected map char '{map}'")
            };
            
            direction = (Direction)(((int)direction + directionChange + 4) % 4);

            if (map == '+') {
                _nextTurn = (_nextTurn + 2) % 3 - 1;
            }
        }

        public bool Alive = true;
        private readonly Coord[] _velocity = { new(0,-1), new(1,0), new(0,1), new(-1,0) };
        public enum Direction { North=0, East=1, South=2, West=3 }
        public Direction direction; // '^'=0 '>'=1 'v'=2 '<'=3
        private int _nextTurn = -1; // -1 = left, 0 = direct, 1 = right
        public Coord coord;
    }

    public class Map
    {
        private readonly char[][] _rails;
        public readonly List<Cart> Carts;
        public Coord FirstCrashPos;

        public Map(string[] data) {
            Carts = new List<Cart>();
            FirstCrashPos = null;
            _rails = new char[data.Length][];

            int y = 0;
            foreach (var s in data) {
                var row = s.ToArray();
                int x = 0;
                foreach (var c in s) {
                    switch (c) {
                        case '^':
                        case 'v':
                            Carts.Add(new Cart(c, new Coord(x, y)));
                            row[x] = '|';
                            break;
                        case '>':
                        case '<':
                            Carts.Add(new Cart(c, new Coord(x, y)));
                            row[x] = '-';
                            break;
                    }

                    x++;
                }

                _rails[y++] = row;
            }
        }

        public void Tick() {
            Carts.Sort(); //(x, y) => x.coord.CompareTo(y.coord);

            foreach (var cart in Carts.Where(c=>c.Alive)) {
                cart.Move();
                cart.Rotate(_rails[cart.coord.y][cart.coord.x]);

                var collision = Carts.Find(c => c.Alive 
                                                && c.coord.x == cart.coord.x
                                                && c.coord.y == cart.coord.y
                                                && c.direction != cart.direction);
                
                if (collision != null) {
                    cart.Alive = false;
                    collision.Alive = false;
                    FirstCrashPos ??= cart.coord;
                }
            }
        }
/*
        public StringBuilder[] Get() {
            var res = new StringBuilder[_rails.GetLength(0)];
            for (int i = 0; i < _rails.GetLength(0); i++)
                res[i] = new StringBuilder(new String(_rails[i]));

            foreach (var cart in Carts) {
                res[cart.coord.y][cart.coord.x] = cart.ToChar();
            }

            return res;
        }

        public void Print(int n) {
            Console.WriteLine($"Tick {n}");
            foreach (var str in Get()) {
                Console.WriteLine(str);
            }
        }
        */
    }

    public static class Day13Solver
    {
        public static Coord Part1(string[] rawInput) {
            var map = new Map(rawInput);
            int tick = 0;
            //Console.Clear();
            do {
                //map.Print(tick);
                map.Tick();
                tick++;
                //Thread.Sleep(100);
                //Console.Clear();
            } while (map.FirstCrashPos == null);

            //map.Print(tick);
            Console.WriteLine($"Ticks {tick}");
            return map.FirstCrashPos;
        }
        
        public static Coord Part2(string[] rawInput) {
            var map = new Map(rawInput);
            do {
                map.Tick();
            } while (map.Carts.Count(c => c.Alive) > 1);
            
            return map.Carts.First(c => c.Alive).coord;
        }
    }

    class Day13Tests
    {
        [TestCase("Day13/sample.txt", 7, 3)]
        [TestCase("Day13/input.txt", 26, 92)]
        public void Part1Test(string fileName, int x, int y) {
            var expected = new Coord(x, y);
            var rawInput = Utils.GetTestStrings(fileName);
            var answer = Day13Solver.Part1(rawInput);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }

        [TestCase("Day13/sample2.txt", 6, 4)]
        [TestCase("Day13/input.txt", 86,18)]
        public void Part2Test(string fileName, int x, int y) {
            var expected = new Coord(x, y);
            var rawInput = Utils.GetTestStrings(fileName);
            var answer = Day13Solver.Part2(rawInput);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
    }
}
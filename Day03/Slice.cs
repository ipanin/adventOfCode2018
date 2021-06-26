using System;
using System.Linq;
using NUnit.Framework;

namespace Aoc.Day03 {
    class Day03Solver {
        private struct Claim {
            public int Id;
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }

        // #1281 @ 755,745: 10x19
        private static Claim ParseRecord(string row) {
            var nums = row
                .Split(new[] {' ', '#', '@', ',', ':', 'x'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            return new Claim {Id = nums[0], Left = nums[1], Top = nums[2], Width = nums[3], Height = nums[4]};
        }

        private static Claim[] LoadData(string fileName) {
            var testData = Utils.GetTestStrings(fileName);
            return testData.Select(ParseRecord).ToArray();
        }

        private static int[,] CreateFabric(Claim[] claims) {
            int H = 0;
            int W = 0;
            foreach (var claim in claims) {
                var w = claim.Left + claim.Width;
                var h = claim.Top + claim.Height;
                if (w > W) W = w;
                if (h > H) H = h;
            }

            Console.WriteLine($"Full size: {W}x{H}"); //1000x999

            var list = new int[W, H];
            foreach (var claim in claims)
                for (int i = claim.Left; i < claim.Left + claim.Width; i++)
                for (int j = claim.Top; j < claim.Top + claim.Height; j++)
                    list[i, j]++;
            return list;
        }

        private static long Part1(Claim[] claims) {
            // parseRecord("#1281 @ 755,745: 10x19").Dump();
            var list = CreateFabric(claims);

            long square = 0;
            for (int i = 0; i < list.GetLength(0); i++) // W
            for (int j = 0; j < list.GetLength(1); j++) // H
                if (list[i, j] > 1)
                    square++;

            //Console.WriteLine($"Intersection square= {square}"); // 105231
            return square;
        }
        
        private static int Part2(Claim[] claims) {
            var list = CreateFabric(claims);

            foreach (var claim in claims) {
                var single = true;
                for (int i = claim.Left; i < claim.Left + claim.Width && single; i++)
                for (int j = claim.Top; j < claim.Top + claim.Height && single; j++)
                    if (list[i, j] != 1)
                        single = false;

                if (single) {
                    //Console.WriteLine($"Claim {claim.id} is OK"); // 164
                    var answer = claim.Id;
                    return answer;
                }
            }

            Assert.Fail("Claim not found");
            return 0;
        }
        
        [TestCase("Day03/sample.txt", 4)]
        [TestCase("Day03/input.txt", 105231)]
        public void Part1Test(string fileName, long expected) {
            var claims = LoadData(fileName);
            var answer = Part1(claims);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }

        [TestCase("Day03/sample.txt", 3)]
        [TestCase("Day03/input.txt", 164)]
        public void Part2Test(string fileName, int expected) {
            var claims = LoadData(fileName);
            var answer = Part2(claims);
            Assert.AreEqual(expected, answer, "Wrong answer");
        }
    }
}
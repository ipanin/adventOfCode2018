using System;
using System.Net;
using System.IO;

namespace AdventOfCode.Y2018 {
    public static class Utils {
        static string YearDir {
            get {
                var currentDir = Directory.GetCurrentDirectory();

                while (!currentDir.EndsWith("2018")) {
                    currentDir = Directory.GetParent(currentDir).FullName;
                }

                return currentDir;
            }
        }

        public static string[] GetTestStrings(string fileName) {
            var fullName = Path.Combine(YearDir, fileName);
            if (!File.Exists(fullName)) {
                throw new ArgumentException($"File '{fullName}' doesn't exist.", nameof(fileName));
            }

            return File.ReadAllLines(fullName);
        }

        public static string GetTestString(string fileName) {
            var fullName = Path.Combine(YearDir, fileName);
            if (!File.Exists(fullName)) {
                throw new ArgumentException($"File '{fullName}' doesn't exist.", nameof(fileName));
            }

            return File.ReadAllText(fullName).TrimEnd();
        }

        public static string[] DownloadTestStringsWithCache(string url, string fileName) {
            var fullName = Path.Combine(YearDir, fileName);
            if (!File.Exists(fullName)) {
                var dir = Path.GetDirectoryName(fullName);
                if (!System.IO.Directory.Exists(dir))
                    throw new ArgumentException($"Directory '{dir}' doesn't exist.", nameof(fileName));
                DownloadContent(url, fullName);
            }

            return File.ReadAllLines(fullName);
        }

        private static void DownloadContent(string address, string fileName) {
            WebClient client = new WebClient();
            client.Headers.Add(
                HttpRequestHeader.Cookie,
                "_ga=GA1.2.54792643.1562059585; session=53616c7465645f5f1913c9226fb1e79f2e48080883194ea8ddbb3d04495db75275184a2fb104b7f2d59eeaf5b1c17e77");
            // replace with actual values

            client.DownloadFile(address, fileName);
        }

        public static void Assert<T>(T expected, T actual, string message) {
            var output = expected.Equals(actual)
                ? $"{message}: {actual} - OK."
                : $"{message} check failed. Expected: {expected}, actual: {actual}.";

            Console.WriteLine(output);
        }
    }
}
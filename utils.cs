using System;
using System.Net;
using System.IO;

namespace Aoc {
	public static class Advent
	{
		private static void DownloadContent(string address, string fileName) {
			WebClient client = new WebClient();
			client.Headers.Add(
				HttpRequestHeader.Cookie,
				"_ga=GA1.2.54792643.1562059585; session=53616c7465645f5f1913c9226fb1e79f2e48080883194ea8ddbb3d04495db75275184a2fb104b7f2d59eeaf5b1c17e77");

			client.DownloadFile(address, fileName);
		}

		public const string TestData2018CachePath = @"C:\Users\ip\Documents\LINQPad Queries\adventOfCode2018\";
		
		public static string[] GetTestData(string url, string fileName) {
			if (!File.Exists(fileName)) {
				var dir = Path.GetDirectoryName(fileName);
				if (!System.IO.Directory.Exists(dir))
					throw new ArgumentException($"Directory '{dir}' doesn't exist.", nameof(fileName));
				DownloadContent(url, fileName);
			}
			return File.ReadAllLines(fileName);
		}

		public static string[] GetTestData(string fileName) {
			if (!File.Exists(fileName)) {
				throw new ArgumentException($"File '{fileName}' doesn't exist.", nameof(fileName));
			}
			return File.ReadAllLines(fileName);
		}

		public static string GetTestString(string url, string fileName) {
			if (!File.Exists(fileName)) {
				var dir = Path.GetDirectoryName(fileName);
				if (!System.IO.Directory.Exists(dir))
					throw new ArgumentException($"Directory '{dir}' doesn't exist.", nameof(fileName));
				DownloadContent(url, fileName);
			}
			return File.ReadAllText(fileName).TrimEnd();
		}
	}
}

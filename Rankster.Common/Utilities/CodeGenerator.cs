using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rankster.Common.Utilities
{
	/// <summary>
	/// Instances of this class are used to generate alpha-numeric strings.
	/// </summary>
	public sealed class AlphaNumericStringGenerator
	{
		private static char[] possibleAlphaNumericValues =
				new char[]{'A','B','C','D','E','F','G','H','J','K','L',
				'M','N','P','Q','R','S','T','U','V','W','X','Y',
				'Z','2','3','4','5','6','7','8','9'};
		/// <summary>
		/// The synchronization lock.
		/// </summary>
		private readonly object _lock = new object();

		/// <summary>
		/// The cryptographically-strong random number generator.
		/// </summary>
		private readonly Random _random = new();

		/// <summary>
		/// Return a string of the provided length comprised of only uppercase alpha-numeric characters each of which are
		/// selected randomly.
		/// </summary>
		/// <param name="ofLength">The length of the string which will be returned.</param>
		/// <returns>Return a string of the provided length comprised of only uppercase alpha-numeric characters each of which are
		/// selected randomly.</returns>
		public string GetRandomUppercaseAlphaNumericValue(int ofLength)
		{
			lock (_lock)
			{
				var builder = new StringBuilder();

				for (int i = 1; i <= ofLength; i++)
				{
					builder.Append(GetRandomUppercaseAphanumericCharacter());
				}

				return builder.ToString();
			}
		}

		/// <summary>
		/// Return a randomly-generated uppercase alpha-numeric character (A-Z or 0-9).
		/// </summary>
		/// <returns>Return a randomly-generated uppercase alpha-numeric character (A-Z or 0-9).</returns>
		private char GetRandomUppercaseAphanumericCharacter()
		{
			return possibleAlphaNumericValues[GetRandomInteger(0, possibleAlphaNumericValues.Length - 1)];
		}

		/// <summary>
		/// Return a random integer between a lower bound and an upper bound.
		/// </summary>
		/// <param name="lowerBound">The lower-bound of the random integer that will be returned.</param>
		/// <param name="upperBound">The upper-bound of the random integer that will be returned.</param>
		/// <returns> Return a random integer between a lower bound and an upper bound.</returns>
		private int GetRandomInteger(int lowerBound, int upperBound)
		{
			return _random.Next(lowerBound, upperBound + 1);
		}
	}
}

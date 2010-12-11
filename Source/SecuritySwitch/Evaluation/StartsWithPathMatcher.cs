﻿using System.Globalization;


namespace SecuritySwitch.Evaluation {
	/// <summary>
	/// An implementation of IPathMatcher that matches the pattern if the path starts with it; accounting for variances in case if indicated.
	/// </summary>
	public class StartsWithPathMatcher : IPathMatcher {
		/// <summary>
		/// Determines whether the specified path is a match to the provided pattern.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="pattern">The pattern to match against.</param>
		/// <param name="ignoreCase">A flag that indicates whether or not to ignore the case of the path and pattern when matching.</param>
		/// <returns>
		/// 	<c>true</c> if the specified path is a match with the pattern; otherwise, <c>false</c>.
		/// </returns>
		public bool IsMatch(string path, string pattern, bool ignoreCase) {
			return path.StartsWith(pattern, ignoreCase, CultureInfo.InvariantCulture);
		}
	}
}
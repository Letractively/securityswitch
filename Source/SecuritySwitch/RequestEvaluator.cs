﻿using System;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;


namespace SecuritySwitch {
	/// <summary>
	/// The default implementation of IRequestEvaluator.
	/// </summary>
	public class RequestEvaluator : IRequestEvaluator {
		/// <summary>
		/// Evaluates the specified request for the need to switch its security.
		/// </summary>
		/// <param name="request">The request to evaluate.</param>
		/// <param name="settings">The settings to use for evaluation.</param>
		public void Evaluate(HttpRequestBase request, Settings settings) {
			// Test if the request matches the configured mode.
			if (!RequestMatchesMode(request, settings.Mode)) {
				return;
			}

			// Any non-matching request should default to Insecure.
			var security = RequestSecurity.Insecure;

			// Find any matching path setting for the request.
			var requestPath = request.Path;
			foreach (PathSetting pathSetting in settings.Paths) {
				// Get an appropriate path matcher and test the request's path for a match.
				var matcher = PathMatcherFactory.GetPathMatcher(pathSetting.MatchType);
				if (matcher.IsMatch(requestPath, pathSetting.Path, pathSetting.IgnoreCase)) {
					security = pathSetting.Security;
				}
			}

			// Ensure the request matches the determined security.
			EnsureRequestMatchesSecurity(request, security, settings);
		}


		/// <summary>
		/// Ensures the specified request is being accessed by the proper protocol; redirecting as necessary.
		/// </summary>
		/// <param name="request">The request to ensure proper access for.</param>
		/// <param name="security">The security setting to match.</param>
		/// <param name="settings">The settings used for any redirection.</param>
		private static void EnsureRequestMatchesSecurity(HttpRequestBase request, RequestSecurity security, Settings settings) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Tests the given request to see if it matches the specified mode.
		/// </summary>
		/// <param name="request">An HttpRequestBase to test.</param>
		/// <param name="mode">The Mode used for the test.</param>
		/// <returns>
		///		Returns true if the request matches the mode as follows:
		///		<list type="disc">
		///			<item>If mode is On.</item>
		///			<item>If mode is set to RemoteOnly and the request is from a computer other than the server.</item>
		///			<item>If mode is set to LocalOnly and the request is from the server.</item>
		///		</list>
		///	</returns>
		private static bool RequestMatchesMode(HttpRequestBase request, Mode mode) {
			switch (mode) {
				case Mode.On:
					return true;

				case Mode.RemoteOnly:
					return !request.IsLocal;

				case Mode.LocalOnly:
					return request.IsLocal;

				default:
					return false;
			}
		}
	}
}
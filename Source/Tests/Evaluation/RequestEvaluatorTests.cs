// =================================================================================
// Copyright � 2004-2011 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================

using Moq;

using SecuritySwitch.Abstractions;
using SecuritySwitch.Configuration;
using SecuritySwitch.Evaluation;

using Xunit;


namespace SecuritySwitch.Tests.Evaluation {
	public class RequestEvaluatorTests : IUseFixture<RequestEvaluatorTestFixture> {
		private RequestEvaluatorTestFixture _fixture;

		
		/// <summary>
		/// Called on the test class just before each test method is run,
		/// passing the fixture data so that it can be used for the test.
		/// All test runs share the same instance of fixture data.
		/// </summary>
		/// <param name="data">The fixture data</param>
		public void SetFixture(RequestEvaluatorTestFixture data) {
			_fixture = data;
		}


		[Fact]
		public void EvaluateReturnsIgnoreWhenModeIsOff() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			var settings = new Settings {
				Mode = Mode.Off
			};
			var requestEvaluator = new RequestEvaluator();

			// Act.
			var security = requestEvaluator.Evaluate(mockRequest.Object, settings);

			// Assert.
			Assert.Equal(RequestSecurity.Ignore, security);
		}

		[Fact]
		public void EvaluateReturnsIgnoreWhenModeIsRemoteOnlyAndRequestIsLocal() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.IsLocal).Returns(true);
			var settings = new Settings {
				Mode = Mode.RemoteOnly
			};
			var requestEvaluator = new RequestEvaluator();

			// Act.
			var security = requestEvaluator.Evaluate(mockRequest.Object, settings);

			// Assert.
			Assert.Equal(RequestSecurity.Ignore, security);
		}

		[Fact]
		public void EvaluateReturnsIgnoreWhenModeIsLocalOnlyAndRequestIsRemote() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.IsLocal).Returns(false);
			var settings = new Settings {
				Mode = Mode.LocalOnly
			};
			var requestEvaluator = new RequestEvaluator();

			// Act.
			var security = requestEvaluator.Evaluate(mockRequest.Object, settings);

			// Assert.
			Assert.Equal(RequestSecurity.Ignore, security);
		}

		[Fact]
		public void EvaluateReturnsInsecureWhenNoSettingsPathsMatchRequestPath() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.Path).Returns("/Info/AboutUs.aspx");
			var requestEvaluator = new RequestEvaluator();

			// Act.
			var security = requestEvaluator.Evaluate(mockRequest.Object, _fixture.Settings);

			// Assert.
			Assert.Equal(RequestSecurity.Insecure, security);
		}

		[Fact]
		public void EvaluateReturnsSecureWhenASecureSettingsPathMatchesRequestPath() {
			// Arrange.
			var mockRequest = new Mock<HttpRequestBase>();
			mockRequest.SetupGet(req => req.Path).Returns("/login/");
			var requestEvaluator = new RequestEvaluator();

			// Act.
			var security = requestEvaluator.Evaluate(mockRequest.Object, _fixture.Settings);

			// Assert.
			Assert.Equal(RequestSecurity.Secure, security);
		}
	}
}
using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Reporting
{
    /// <summary>
    /// Base class for plain NUnit tests (non-Reqnroll) that automatically
    /// creates an ExtentReports entry per test method and logs pass/fail/skip.
    /// </summary>
    public abstract class ReportedTestBase
    {
        private static readonly ReportManager _report = ReportManager.Instance;
        private ExtentTest _currentTest = null!;

        [SetUp]
        public void ReportSetUp()
        {
            var testName = TestContext.CurrentContext.Test.FullName;
            _currentTest = _report.CreateTest(testName);
        }

        /// <summary>The ExtentTest node for the currently executing test.</summary>
        protected ExtentTest CurrentTest => _currentTest;

        /// <summary>Log an informational message into the report for the current test.</summary>
        protected void LogInfo(string message) => _currentTest.Info(message);

        [TearDown]
        public void ReportTearDown()
        {
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;

            switch (outcome)
            {
                case TestStatus.Passed:
                    _currentTest.Pass("Test passed.");
                    break;
                case TestStatus.Failed:
                    var message = TestContext.CurrentContext.Result.Message ?? "Unknown failure";
                    var stackTrace = TestContext.CurrentContext.Result.StackTrace ?? "";
                    _currentTest.Fail(message);
                    if (!string.IsNullOrWhiteSpace(stackTrace))
                        _currentTest.Fail($"<pre>{stackTrace}</pre>");
                    break;
                case TestStatus.Skipped:
                    _currentTest.Skip("Test was skipped.");
                    break;
                case TestStatus.Inconclusive:
                    _currentTest.Warning("Test was inconclusive.");
                    break;
            }

            _report.Flush();
        }
    }
}
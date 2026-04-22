using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Reporting
{
    public sealed class ReportManager
    {
        private static readonly Lazy<ReportManager> _instance = new(() => new ReportManager());
        private readonly ExtentReports _extent;
        private readonly string _reportDir;
        private readonly string _reportPath;

        private ReportManager()
        {
            _reportDir = Path.Combine(
                Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName,
                "Reports");
            Directory.CreateDirectory(_reportDir);

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            _reportPath = Path.Combine(_reportDir, $"TestReport_{timestamp}.html");

            var spark = new ExtentSparkReporter(_reportPath)
            {
                Config =
                {
                    DocumentTitle = "SauceDemo Test Report",
                    ReportName = "SauceDemoFramework — Test Results",
                    Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard
                }
            };

            _extent = new ExtentReports();
            _extent.AttachReporter(spark);
            _extent.AddSystemInfo("Environment", "QA");
            _extent.AddSystemInfo("Base URL", "https://www.saucedemo.com");
            _extent.AddSystemInfo("Framework", ".NET 8");
            _extent.AddSystemInfo("Runner", "NUnit + Reqnroll");
        }

        public static ReportManager Instance => _instance.Value;

        /// <summary>Full path of the generated HTML report.</summary>
        public string ReportPath => _reportPath;

        /// <summary>The underlying ExtentReports instance.</summary>
        public ExtentReports Extent => _extent;

        /// <summary>Create a new test node in the report.</summary>
        public ExtentTest CreateTest(string name, string? description = null)
        {
            return description is null
                ? _extent.CreateTest(name)
                : _extent.CreateTest(name, description);
        }

        /// <summary>Flush buffered data to the HTML file.</summary>
        public void Flush()
        {
            _extent.Flush();
        }
    }
}
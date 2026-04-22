using AventStack.ExtentReports;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemoFramework.Reporting
{
    /// <summary>
    /// Reqnroll hooks that automatically create test entries in the ExtentReports
    /// report for every scenario and log step-level pass/fail/skip status.
    /// </summary>
    [Binding]
    public sealed class ReportingHooks
    {
        private static readonly ReportManager _report = ReportManager.Instance;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;

        public ReportingHooks(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        // ?? Feature-level ?????????????????????????????????????????

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            // Create a parent node per feature so scenarios are grouped visually
            var featureNode = _report.CreateTest(featureContext.FeatureInfo.Title,
                featureContext.FeatureInfo.Description);
            featureContext.Set(featureNode, "extentFeature");
        }

        // ?? Scenario-level ????????????????????????????????????????

        [BeforeScenario(Order = 1)]
        public void BeforeScenario()
        {
            ExtentTest scenarioNode;

            if (_featureContext.TryGetValue<ExtentTest>("extentFeature", out var featureNode))
            {
                scenarioNode = featureNode.CreateNode(_scenarioContext.ScenarioInfo.Title);
            }
            else
            {
                scenarioNode = _report.CreateTest(_scenarioContext.ScenarioInfo.Title);
            }

            // Tag each scenario with its Gherkin tags
            foreach (var tag in _scenarioContext.ScenarioInfo.Tags)
            {
                scenarioNode.AssignCategory(tag);
            }

            _scenarioContext.Set(scenarioNode, "extentScenario");
        }

        [AfterStep]
        public void AfterStep()
        {
            if (!_scenarioContext.TryGetValue<ExtentTest>("extentScenario", out var scenarioNode))
                return;

            var stepInfo = _scenarioContext.StepContext.StepInfo;
            var stepText = $"{stepInfo.StepDefinitionType} {stepInfo.Text}";

            if (_scenarioContext.TestError is null)
            {
                scenarioNode.Log(Status.Pass, stepText);
            }
            else
            {
                scenarioNode.Log(Status.Fail, stepText);
                scenarioNode.Log(Status.Fail, _scenarioContext.TestError.Message);

                if (_scenarioContext.TestError.StackTrace is not null)
                {
                    scenarioNode.Log(Status.Fail,
                        $"<pre>{_scenarioContext.TestError.StackTrace}</pre>");
                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (!_scenarioContext.TryGetValue<ExtentTest>("extentScenario", out var scenarioNode))
                return;

            // Mark skipped scenarios
            if (_scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.Skipped)
            {
                scenarioNode.Skip("Scenario was skipped.");
            }

            // If there was an unhandled error that wasn't caught in AfterStep
            if (_scenarioContext.TestError is not null
                && scenarioNode.Status != Status.Fail)
            {
                scenarioNode.Fail(_scenarioContext.TestError);
            }
        }

        // ?? Test-run level (flush) ????????????????????????????????

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _report.Flush();

            Console.WriteLine($"[Report] HTML report generated at: {_report.ReportPath}");
        }
    }
}
// <copyright file="ProjectTestBase.cs" company="Team">
// Copyright (c) Team. All rights reserved.
// </copyright>

namespace Ocaramba.Tests.NUnit
{
    using System;
    using System.IO;
    using AventStack.ExtentReports;
    using AventStack.ExtentReports.Reporter;
    using global::NUnit.Framework;
    using global::NUnit.Framework.Interfaces;
    using Ocaramba;
    using Ocaramba.Helpers;
    using Ocaramba.Logger;

    /// <summary>
    /// The base class for all tests <see href="https://github.com/ObjectivityLtd/Ocaramba/wiki/ProjectTestBase-class">More details on wiki</see>
    /// </summary>
    public class ProjectTestBase : TestBase
    {
        private readonly DriverContext driverContext = new DriverContext();
        private ExtentHtmlReporter htmlReporter;
        private ExtentReports extent;
        private ExtentTest test;

        /// <summary>
        /// Gets or sets logger instance for driver
        /// </summary>
        public TestLogger LogTest
        {
            get
            {
                return this.DriverContext.LogTest;
            }

            set
            {
                this.DriverContext.LogTest = value;
            }
        }

        /// <summary>
        /// Gets or Sets the driver context
        /// </summary>
        protected DriverContext DriverContext
        {
            get
            {
                return this.driverContext;
            }
        }

        protected ExtentTest Test
        {
            get
            {
                return this.test;
            }

            set
            {
                this.test = value;
            }
        }

        /// <summary>
        /// Before the class.
        /// </summary>
        [OneTimeSetUp]
        public void BeforeClass()
        {
            this.htmlReporter = new ExtentHtmlReporter("C:\\Builds\\test.html");
            this.DriverContext.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            this.DriverContext.Start();
            this.extent = new ExtentReports();
            this.extent.AttachReporter(this.htmlReporter);
        }

        /// <summary>
        /// After the class.
        /// </summary>
        [OneTimeTearDown]
        public void AfterClass()
        {
            PrintPerformanceResultsHelper.PrintAverageDurationMillisecondsInAppVeyor(this.DriverContext.PerformanceMeasures);
            PrintPerformanceResultsHelper.PrintPercentiles90DurationMillisecondsInAppVeyor(this.DriverContext.PerformanceMeasures);
            PrintPerformanceResultsHelper.PrintAverageDurationMillisecondsInTeamcity(this.DriverContext.PerformanceMeasures);
            PrintPerformanceResultsHelper.PrintPercentiles90DurationMillisecondsinTeamcity(this.DriverContext.PerformanceMeasures);
            this.DriverContext.Stop();
            this.extent.Flush();
        }

        /// <summary>
        /// Before the test.
        /// </summary>
        [SetUp]
        public void BeforeTest()
        {
            this.DriverContext.TestTitle = TestContext.CurrentContext.Test.Name;
            this.test = this.extent.CreateTest(TestContext.CurrentContext.Test.Name);
            this.LogTest.LogTestStarting(this.driverContext);
        }

        /// <summary>
        /// After the test.
        /// </summary>
        [TearDown]
        public void AfterTest()
        {
            this.DriverContext.IsTestFailed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || !this.driverContext.VerifyMessages.Count.Equals(0);
            var filePaths = this.SaveTestDetailsIfTestFailed(this.driverContext);
            this.SaveAttachmentsToTestContext(filePaths);
            this.LogTest.LogTestEnding(this.driverContext);
            var javaScriptErrors = this.DriverContext.LogJavaScriptErrors();
            if (this.IsVerifyFailedAndClearMessages(this.driverContext) && TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            {
                this.test.Fail("errors found. See the logs for details");
            }

            if (javaScriptErrors)
            {
                this.test.Fail("JavaScript errors found. See the logs for details");
            }
        }

        private void SaveAttachmentsToTestContext(string[] filePaths)
        {
            if (filePaths != null)
            {
                foreach (var filePath in filePaths)
                {
                    this.LogTest.Info("Uploading file [{0}] to test context", filePath);
                    TestContext.AddTestAttachment(filePath);
                }
            }
        }
    }
}

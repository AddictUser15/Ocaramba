// <copyright file="HerokuappTestsNUnit.cs" company="Team">
// Copyright (c) Team. All rights reserved.
// </copyright>

namespace Ocaramba.Tests.CloudProviderCrossBrowser.Tests
{
    using global::NUnit.Framework;
    using Ocaramba;
    using Ocaramba.Tests.PageObjects.PageObjects.TheInternet;

    [TestFixture("ChromeWindows")]
    [TestFixture("Android")]
    [TestFixture("Iphone")]
    [TestFixture("FirefoxWindows")]
    [TestFixture("SafariMac")]
    [TestFixture("EdgeWindows")]
    [TestFixture("IEWindows")]
    [Parallelizable(ParallelScope.Fixtures)]
    public class HerokuappTestsNUnit : ProjectTestBase
    {
        public HerokuappTestsNUnit(string environment)
            : base(environment)
        {
        }

        [Test]
        public void ContextMenuTest()
        {
            const string H3Value = "Context Menu";
            var browser = BaseConfiguration.TestBrowser;
            if (browser.Equals(BrowserType.Firefox))
            {
                var contextMenuPage = new InternetPage(this.DriverContext)
                    .OpenHomePage()
                    .GoToContextMenuPage()
                    .SelectTheInternetOptionFromContextMenu();

                Assert.AreEqual("You selected a context menu", contextMenuPage.JavaScriptText);
                Assert.True(contextMenuPage.ConfirmJavaScript().IsH3ElementEqualsToExpected(H3Value), "h3 element is not equal to expected {0}", H3Value);
            }
        }

        [Test]
        public void SlowResourcesTest()
        {
            int timeout = 35;
            new InternetPage(this.DriverContext)
                .OpenHomePage()
                .GoToSlowResources()
                .WaitForIt(timeout);
        }

        [Test]
        public void MultipleWindowsTest()
        {
            const string PageTitle = "New Window";

            var newWindowPage = new InternetPage(this.DriverContext)
                .OpenHomePage()
                .GoToMultipleWindowsPage()
                .OpenNewWindowPage();

            Assert.True(newWindowPage.IsPageTile(PageTitle), "wrong page title, should be {0}", PageTitle);
            Assert.True(newWindowPage.IsNewWindowH3TextVisible(PageTitle), "text is not equal to {0}", PageTitle);
        }
    }
}

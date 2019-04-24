// <copyright file="JavaScriptAlertsTestsNUnit.cs" company="Team">
// Copyright (c) Team. All rights reserved.
// </copyright>

namespace Ocaramba.Tests.NUnit.Tests
{
    using global::NUnit.Framework;
    using Ocaramba.Tests.PageObjects.PageObjects.TheInternet;

    [TestFixture]
    public class JavaScriptAlertsTestsNUnit : ProjectTestBase
    {
        [Test]
        public void ClickJsAlertTest()
        {
            var internetPage = new InternetPage(this.DriverContext).OpenHomePage();
            var jsAlertsPage = internetPage.GoToJavaScriptAlerts();
            jsAlertsPage.OpenJsAlert();
            jsAlertsPage.AcceptAlert();
            Assert.AreEqual("You successfuly clicked an alert", jsAlertsPage.ResultText);
        }

        [Test]
        public void AcceptJsConfirmTest()
        {
            var internetPage = new InternetPage(this.DriverContext).OpenHomePage();
            var jsAlertsPage = internetPage.GoToJavaScriptAlerts();
            jsAlertsPage.OpenJsConfirm();
            jsAlertsPage.AcceptAlert();
            Assert.AreEqual("You clicked: Ok", jsAlertsPage.ResultText);
        }

        [Test]
        public void DismissJsConfirmTest()
        {
            var internetPage = new InternetPage(this.DriverContext).OpenHomePage();
            var jsAlertsPage = internetPage.GoToJavaScriptAlerts();
            jsAlertsPage.OpenJsConfirm();
            jsAlertsPage.DismissAlert();
            Assert.AreEqual("You clicked: Cancel", jsAlertsPage.ResultText);
        }

        [Test]
        public void TypeTextAndAcceptJsPromptTest()
        {
            var text = "Sample text";
            var internetPage = new InternetPage(this.DriverContext).OpenHomePage();
            var jsAlertsPage = internetPage.GoToJavaScriptAlerts();
            jsAlertsPage.OpenJsPrompt();
            jsAlertsPage.TypeTextOnAlert(text);
            jsAlertsPage.AcceptAlert();
            Assert.AreEqual("You entered: " + text, jsAlertsPage.ResultText);
        }

        [Test]
        public void DismissJsPromptTest()
        {
            var internetPage = new InternetPage(this.DriverContext).OpenHomePage();
            var jsAlertsPage = internetPage.GoToJavaScriptAlerts();
            jsAlertsPage.OpenJsPrompt();
            jsAlertsPage.DismissAlert();
            Assert.AreEqual("You entered: null", jsAlertsPage.ResultText);
        }
    }
}

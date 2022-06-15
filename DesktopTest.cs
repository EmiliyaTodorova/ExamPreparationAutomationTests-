using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace ContactBook.AndroidTests
{
    public class DesktopTest
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string ContactBookUrl = "https://contactbook-2.emiliyatodorova.repl.co/api";
        private const string appLocation = @"C:\work\ContactBook-DesktopClient.exe";
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

  
        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test1()
        {
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(ContactBookUrl);
            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();
            string windowName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowName);
            var searchField = driver.FindElementByAccessibilityId("textBoxSearch");
            searchField.SendKeys("steve");
            var buttonSearch = driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();
            Thread.Sleep(2000);
            var searchLabel = driver.FindElementByAccessibilityId("labelResult").Text;
            Assert.That(searchLabel, Is.EqualTo("Contacts found: 1"));
            var firstName = driver.FindElement(By.XPath("//Edit[@Name=\"FirstName Row 0, Not sorted.\"]"));
            var lastName = driver.FindElement(By.XPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]"));
            Assert.That(firstName.Text, Is.EqualTo("Steve"));
            Assert.That(lastName.Text, Is.EqualTo("Jobs"));
        }
    }
}
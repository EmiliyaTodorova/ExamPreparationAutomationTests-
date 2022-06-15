using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace ContactBookWebDriverTests
{
    public class UITests
    {
        private const string url = "https://contactbook.emiliyatodorova.repl.co/";
        private WebDriver driver;

        [SetUp]
        public void ÓpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
        }

        [Test]
        public void Test_ListContacts_CheckFirstContact()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("li:nth-of-type(2) > a")).Click();
            var firstName = driver.FindElement(By.CssSelector("a:nth-of-type(1) > .contact-entry  .fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("a:nth-of-type(1) > .contact-entry  .lname > td")).Text;
            Assert.That(firstName, Is.EqualTo("Steve"));
            Assert.That(lastName, Is.EqualTo("Jobs"));

        }
        [Test]
        public void Test_SearchContacts_CheckFirstResults()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a")).Click();
            driver.FindElement(By.CssSelector("input#keyword")).SendKeys("Albert");
            driver.FindElement(By.CssSelector("button#search")).Click();
            var firstName = driver.FindElement(By.CssSelector("table#contact3  .fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector(".lname > td")).Text;
            Assert.That(firstName, Is.EqualTo("Albert"));
            Assert.That(lastName, Is.EqualTo("Einstein"));
        }
        [Test]
        public void Test_SearchMissingContacts_EmptyResult()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a")).Click();
            driver.FindElement(By.CssSelector("input#keyword")).SendKeys("invalid2635");
            driver.FindElement(By.CssSelector("button#search")).Click();
            var result = driver.FindElement(By.CssSelector("div#searchResult")).Text;
            Assert.That(result, Is.EqualTo("No contacts found."));
        }
        [Test]
        public void Test_CreateContacts_InvalidData()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("li:nth-of-type(3) > a")).Click();
            var firstNameField = driver.FindElement(By.CssSelector("input#firstName"));
            firstNameField.SendKeys("bbb");
            driver.FindElement(By.CssSelector("button#create")).Click();
            var errorMassage = driver.FindElement(By.CssSelector("body > main > div")).Text;
            Assert.That(errorMassage, Is.EqualTo("Error: Last name cannot be empty!"));
        }
        [Test]
        public void Test_CreateContacts_ValidData()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("li:nth-of-type(3) > a")).Click();
            var firstNameField = driver.FindElement(By.CssSelector("input#firstName"));
            firstNameField.SendKeys("Emiliya" + DateTime.Now.Ticks);
            var lastNameField = driver.FindElement(By.CssSelector("input#lastName"));
            lastNameField.SendKeys("Todorova" + DateTime.Now.Ticks);
            var emailField = driver.FindElement(By.CssSelector("input#email"));
            emailField.SendKeys(DateTime.Now.Ticks + "emiliyatodorova@abb.bg");
            var phoneField = driver.FindElement(By.CssSelector("input#phone"));
            phoneField.SendKeys("12345678");
            driver.FindElement(By.CssSelector("button#create")).Click();
            var allContacts = driver.FindElements(By.CssSelector("body > main > div")); ;
            var lastContact = allContacts.Last();

            var firstNameLabel = lastContact.FindElement(By.CssSelector("#contact4 > tbody > tr.fname")).Text;
            var lastNameLabel = lastContact.FindElement(By.CssSelector("#contact4 > tbody > tr.lname")).Text;

            Assert.That(firstNameLabel, Is.EqualTo(firstNameField));
            Assert.That(lastNameLabel, Is.EqualTo(lastNameField));
        }
    }
}
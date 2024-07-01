using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using NUnit.Framework.Legacy;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;

namespace WebUITest
{
    [TestFixture]
    public class Tests 
    {
        private IWebDriver driver;

        private OnlinerCPUPage page;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driver = new ChromeDriver();
            
            page = new OnlinerCPUPage(driver);
            
            driver.Navigate().GoToUrl("https://catalog.onliner.by/cpu");
            
            driver.Manage().Window.Maximize();
        }

        [Test, Order(1)]
        public void CPUListDefaultSizeTest()
        {
            int actualProductCount = page.GetQuantityCPU();

            Assert.That(actualProductCount, Is.EqualTo(30), "The number of products must be 30");
        }

        [Test, Order(2)]
        public void LoadNextCPUBatchByButtonClickTest()
        {
            page.ClickNextPageButton();
        }

        [Test, Order(3)]
        public void FilterCPUByManufacturerTest()
        {
            page.SelectManufacturerCheckbox();

            Thread.Sleep(5000);

            var cpuNames = page.GetCpuNames();

            foreach (var cpuName in cpuNames)
            {
                StringAssert.Contains("Intel", cpuName.Text, "Not all CPUs are from Intel");
            }
            if (!page.ChekForIntelButton())
            {
                Assert.Fail("No button");
            }
            Thread.Sleep(1000);
        }

        [Test, Order(4)]
        public void SelectCPUAndVerifyComparisonPageTest()
        {
            var cpuList = page.GetCpuList();

            for (int i = 0; i < 3; i++)
            {
                var jsExecutor = (IJavaScriptExecutor)driver;
                
                jsExecutor.ExecuteScript("arguments[0].click();", cpuList[i]);
            }

            Thread.Sleep(3000);

            page.ClickCompareButton();

            Assert.That(driver.Url.Contains("https://catalog.onliner.by/compare/"), Is.True, "Comparison page not available");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Dispose();
        }
    }
}
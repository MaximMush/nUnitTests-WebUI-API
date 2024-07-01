using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebUITest
{
    public class OnlinerCPUPage
    {
        private IWebDriver driver;

        public OnlinerCPUPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private IReadOnlyCollection<IWebElement> ProductElements => driver
            .FindElements(By.CssSelector(".catalog-form__offers-unit.catalog-form__offers-unit_primary"));

        private IWebElement NextPageButton => driver
            .FindElement(By.CssSelector(".catalog-pagination.catalog-pagination_visible"));

        private By intelCheckboxLocator = By
            .XPath("//label[contains(., 'Intel') and @class='catalog-form__checkbox-label']//div[@class='i-checkbox__faux']");

        private By cpuNamesLocator = By
            .XPath("//div[@class='catalog-form__offers-part catalog-form__offers-part_data']//a[contains(@class, 'catalog-form__link_primary-additional') and contains(@class, 'catalog-form__link_font-weight_semibold')]");
        
        private By intelButton = By
            .XPath("//div[@class='button-style button-style_either button-style_small catalog-form__button catalog-form__button_tag' ]");

        private By listCPULocator = By
            .XPath("//label[contains(@class, 'catalog-form__checkbox-label') and ./div[contains(@class, 'i-checkbox_warning')]]//input[@type='checkbox']");

        private By comparisonCheckboxLocator = By
            .XPath("//a[contains(@href, '/compare') and contains(@class, 'catalog-interaction__sub_main')]");

        public int GetQuantityCPU()
        {
            return ProductElements.Count;
        }

        public void ClickNextPageButton()
        {
            NextPageButton.Click();
        }

        public void SelectManufacturerCheckbox()
        {
            driver.FindElement(intelCheckboxLocator).Click();
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetCpuNames()
        {
            return driver.FindElements(cpuNamesLocator);
        }

        public bool ChekForIntelButton()
        {
            return driver.FindElement(intelButton).Displayed;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetCpuList()
        {
            return driver.FindElements(listCPULocator);
        }

        public void ClickCompareButton()
        {
            driver.FindElement(comparisonCheckboxLocator).Click();
        }
    }
}

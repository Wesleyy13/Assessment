using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Playwright;

namespace Assessment.UiTesting.Basepages
{
    public class BasePOM
    {
        public readonly IPage page;

        protected const string BASE_ADRESS = "https://practicesoftwaretesting.com/";

        public BasePOM(IPage page)
        {
            this.page = page;
        }

        public async Task NavigateToUrlAsync() => await page.GotoAsync(BASE_ADRESS);
    }
}

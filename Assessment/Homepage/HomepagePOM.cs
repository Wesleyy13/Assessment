using Assessment.BasePages;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assessment.HomePage
{
    public class HomepagePOM : BasePOM
    {
        public HomepagePOM(IPage page) : base(page)
        { }

        public async Task<string> GetPageTitleAsync()
        {
            return await page.TitleAsync();
        }
    }
}

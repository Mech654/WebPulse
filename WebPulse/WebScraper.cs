using System;
using PuppeteerSharp;
using System.Threading.Tasks;
using WebPulse;
using System.Diagnostics;

namespace WebPulse
{
    internal class WebScraper
    {
        private BrowserFetcher _browserFetcher;

        public WebScraper(BrowserFetcher browserFetcher)
        {
            _browserFetcher = browserFetcher;
        }

        public async Task<bool> ScrapeWebsiteAsync(string url)
        {
            try
            {
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
                var page = await browser.NewPageAsync();

                var response = await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);

                if (response != null)
                {
                    Debug.WriteLine($"Response Status: {response.Status}");
                    if ((int)response.Status >= 200 && (int)response.Status < 300)
                    {
                        Debug.WriteLine("Request was successful.");
                        await browser.CloseAsync();
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("Request failed.");
                    }
                }
                else
                {
                    Debug.WriteLine("No response received.");
                }
                await browser.CloseAsync();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ScrapeWebsiteAsyncCode(string url, string code)
        {
            try
            {
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
                var page = await browser.NewPageAsync();

                var response = await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
                await page.EvaluateFunctionAsync(code);

                var navigationOptions = new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                };

                await page.WaitForNavigationAsync(navigationOptions);

                var newResponse = await page.GoToAsync(page.Url);
                bool resourceExists = (int)newResponse.Status >= 200 && (int)newResponse.Status < 300;

                await browser.CloseAsync();

                return resourceExists;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

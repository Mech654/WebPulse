using PuppeteerSharp;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebPulse
{
    internal class WebScraper
    {
        private IBrowser _browser;

        public WebScraper(IBrowser browser)
        {
            _browser = browser;
        }

        public async Task<bool> ScrapeWebsiteAsync(string url)
        {
            Debug.WriteLine("Looking for " + url + "...");
            IPage page = null;
            try
            {
                page = await _browser.NewPageAsync();
                var response = await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
                if (response != null)
                {
                    Debug.WriteLine($"Response Status: {response.Status}");
                    if ((int)response.Status >= 200 && (int)response.Status < 300)
                    {
                        Debug.WriteLine("Request was successful.");
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
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                if (page != null)
                {
                    await page.CloseAsync();
                }
            }
        }

        public async Task<bool> ScrapeWebsiteAsyncCode(string url, string code)
        {
            IPage page = null;
            try
            {
                page = await _browser.NewPageAsync();
                var response = await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
                await page.EvaluateFunctionAsync(code);
                var navigationOptions = new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                };
                await page.WaitForNavigationAsync(navigationOptions);
                var newResponse = await page.GoToAsync(page.Url);
                bool resourceExists = (int)newResponse.Status >= 200 && (int)newResponse.Status < 300;
                return resourceExists;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                if (page != null)
                {
                    await page.CloseAsync();
                }
            }
        }
    }
}

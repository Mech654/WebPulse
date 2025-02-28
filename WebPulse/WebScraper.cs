using PuppeteerSharp;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebPulse
{
    internal class WebScraper(IBrowser browser)
    {
        public async Task<(bool, string)> ScrapeWebsiteAsync(string url)
        {
            Debug.WriteLine("Looking for " + url + "...");
            IPage page = null;
            try
            {
                page = await browser.NewPageAsync();
                var response = await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
                if (response != null && (int)response.Status >= 200 && (int)response.Status < 300)
                {
                    Debug.WriteLine("Request was successful.");
                    string title = await page.EvaluateExpressionAsync<string>("document.title");
                    return (true, title);
                }
                else
                {
                    Debug.WriteLine("Request failed.");
                }
                return (false, " ");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return (false, " ");
            }
            finally
            {
                if (page != null)
                {
                    await page.CloseAsync();
                }
            }
        }

        public async Task<(bool, string, string)> ScrapeWebsiteAsyncCode(string url, string code)
        {
            IPage page = null;
            try
            {
                page = await browser.NewPageAsync();
                var response = await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
                await page.EvaluateFunctionAsync(code);
                var navigationOptions = new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                };
                await page.WaitForNavigationAsync(navigationOptions);
                var newResponse = await page.GoToAsync(page.Url);
                if ((int)newResponse.Status >= 200 && (int)newResponse.Status < 300)
                {
                    string title = await page.EvaluateExpressionAsync<string>("document.title");
                    string currentUrl = page.Url;
                    return (true, title, currentUrl);
                }
                return (false, " ", " ");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return (false, " ", " ");
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

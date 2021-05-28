// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace Tools.Wasc
{
    public class WascDataCralwer
    {
        private static readonly HttpClient httpClient = new();

        public async Task<IList<WascV2Item>> CrawlWascItemsAsync(Uri sourceUri)
        {
            string htmlCode = await this.ReadWebpageContentAsync(sourceUri);

            var doc = new HtmlDocument();
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Empty;
            doc.OptionWriteEmptyNodes = true;
            doc.LoadHtml(htmlCode);
            HtmlNode tableNode = doc.DocumentNode.SelectSingleNode("//table[@class='pbNotSortable']");

            IEnumerable<IEnumerable<HtmlNode>> rows = tableNode
                .Descendants("tr")
                .Skip(1)  // skip header
                .Select(tr => tr.Elements("td"));

            List<WascV2Item> viewItems = this.ParseHtmlNodeData(rows, sourceUri).ToList();
            viewItems.ForEach(async item => item.Description = await this.CrawlWascThreatAsync(item));

            return viewItems;
        }

        private IEnumerable<WascV2Item> ParseHtmlNodeData(IEnumerable<IEnumerable<HtmlNode>> rows, Uri sourceUri)
        {
            // table format:
            // WASC ID | Name ^ | CWE ID ^ | CAPEC ID ^ | SANS/CWE Top 25 2009 ^ | OWASP Top Ten 2010 * | OWASP Top Ten 2007 * | OWASP Top Ten 2004 * |
            // ^ - column includes anchor <a> elements.
            // * - column includes comma separated contents.
            string rootUrl = sourceUri.GetLeftPart(UriPartial.Authority);
            foreach (IEnumerable<HtmlNode> row in rows)
            {
                int index = 0;
                var wascViewItem = new WascV2Item();
                foreach (HtmlNode col in row)
                {
                    switch (index)
                    {
                        case 0: // WASC ID
                            wascViewItem.WascId = removeHtmlSpace(col.InnerText);
                            break;
                        case 1: // Name
                            HtmlNode anchor = col.Elements("a").FirstOrDefault();
                            wascViewItem.WascName = removeHtmlSpace(anchor.InnerText);
                            wascViewItem.WascLink = $"{rootUrl}{anchor.Attributes["href"].Value}";
                            break;
                        case 2: // CWE ID
                            wascViewItem.CweIds = removeHtmlSpace(col.InnerText).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            break;
                        case 3: // CAPEC ID
                            wascViewItem.CapecIds = removeHtmlSpace(col.InnerText).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            break;
                        case 4: // SANS/CWE Top 25 2009
                            wascViewItem.Sans_Cwe_Top_25_2009 = removeHtmlSpace(col.InnerText).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            break;
                        case 5: // OWASP Top Ten 2010
                            wascViewItem.Owasp_Top_Ten_2010 = removeHtmlSpace(col.InnerText).Split(',', StringSplitOptions.RemoveEmptyEntries);
                            break;
                        case 6: // OWASP Top Ten 2007
                            wascViewItem.Owasp_Top_Ten_2007 = removeHtmlSpace(col.InnerText).Split(',', StringSplitOptions.RemoveEmptyEntries);
                            break;
                        case 7: // OWASP Top Ten 2004
                            wascViewItem.Owasp_Top_Ten_2004 = removeHtmlSpace(col.InnerText).Split(',', StringSplitOptions.RemoveEmptyEntries);
                            break;
                    }
                    index++;
                }
                yield return wascViewItem;
            }
        }

        private string removeHtmlSpace(string input)
        {
            return input.Replace("&nbsp;", " ", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<string> CrawlWascThreatAsync(WascV2Item item)
        {
            if (!Uri.TryCreate(item.WascLink, UriKind.Absolute, out Uri sourceUri)
                || (!sourceUri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                    && !sourceUri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("SourceFilePath is not a valid URL");
                return null;
            }

            string htmlCode = await this.ReadWebpageContentAsync(sourceUri);

            var doc = new HtmlDocument();
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Empty;
            doc.OptionWriteEmptyNodes = true;
            doc.LoadHtml(htmlCode);
            HtmlNode divNode = doc.GetElementbyId("wikipage-inner"); // <div id="wikipage-inner">

            // get 5th not empty paragragh, which is description of the threat
            int index = 0;
            foreach (HtmlNode child in divNode.Descendants(0).Where(n => n.Name == "p" || n.Name == "h2"))
            {
                string text = this.removeHtmlSpace(child.InnerText);
                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }

                if (index++ == 4)
                {
                    return text;
                }
            }
            return null;
        }

        private async Task<string> ReadWebpageContentAsync(Uri url)
        {
            string html = null;
            await RetryInvokeAsync(
                    async () => html = await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync(),
                    retryInterval: TimeSpan.FromMilliseconds(1000),
                    maxAttemptCount: 5);
            return html;
        }

        private static async Task RetryInvokeAsync(Func<Task> func, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            var exceptions = new List<Exception>();
            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        await Task.Delay(retryInterval);
                    }

                    await func();
                    return;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);
        }
    }
}

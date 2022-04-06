using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler_Project
{
    class csExternalScan : csHelperMethods //2021112210,2021112209
    {
        private string MainUrl { get; set; }
        private string Host { get; set; }
        public csExternalScan(string target)
        {
            MainUrl = target;
            Host = new Uri(target).Host;
        }

        private List<string> GetUrl(string Url) //2021112223
        {
            List<string> li = new List<string>();
            try
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(Task.Run(() => { return GetSourceAsync(Url); }).Result);

                var vrnodes = document.DocumentNode.SelectNodes("//a");
                if (vrnodes != null)
                {
                    foreach (HtmlNode link in vrnodes)
                    {

                        string hrefValue = link.GetAttributeValue("href", string.Empty);
                        if (Uri.TryCreate(hrefValue, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                        {
                            if (new Uri(hrefValue).Host == Host)
                            {
                                li.Add(hrefValue);
                            }
                        }
                    }
                }
                li = li.Distinct().ToList();
                li.Sort();
                return li;
            }
            catch
            {
                return li;
            }

        }

        private csResult CreateData(string url) //2021112223
        {
            try
            {
                string sourcecode = Task.Run(() => { return GetSourceAsync(url); }).Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(sourcecode);
                csResult result = new csResult();
                result.UrlHash = ComputeSha256Hash(url);
                result.Url = url;
                result.ParentUrl = MainUrl;
                result.Host = Host;
                if (sourcecode != null)
                {
                    result.SourceCode = sourcecode;
                    result.CompressedSC = csCompress.Zip(result.SourceCode);
                    if (document.DocumentNode.SelectSingleNode("html")?.InnerText != null)
                    {
                        result.InnerText = document.DocumentNode.SelectSingleNode("html")?.InnerText;
                        result.CompressedIT = csCompress.Zip(result.InnerText);
                    }
                    else
                    {
                        result.InnerText = string.Empty;
                    }
                }
                else
                {
                    sourcecode = string.Empty;

                }

                return result;
            }
            catch
            {
                return null;
            }

        }
        public void Scanner()
        {
            csDatabase db = new csDatabase();
            List<string> list = GetUrl(MainUrl);//2021112250
            if (list.Count > 0)
            {
                foreach (string item in list)
                {
                    var temp = CreateData(item);
                    if (temp != null)
                    {
                        db.InsertData(temp);
                    }
                }
            }
        }
    }
}

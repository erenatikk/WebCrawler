using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;


namespace WebCrawler_Project
{
    class csGetSource
    {

        public static string GetSource(string Url) //2021112229
        {
            using (WebClient sc = new WebClient())
            {
                string data = sc.DownloadString(Url);

                return data;
            }
        }
        public static List<string> GetUrl(string Url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(Url);

            List<string> li = new List<string>();
            var vrnodes = document.DocumentNode.SelectNodes("//a");
            if (vrnodes != null)
            {
                foreach (HtmlNode link in vrnodes)
                {
                    string hrefValue = link.GetAttributeValue("href", string.Empty);
                    if (Uri.TryCreate(hrefValue, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp||uriResult.Scheme == Uri.UriSchemeHttps))
                    {
                        li.Add(hrefValue);
                    }                    
                }
            }
            li = li.Distinct().ToList();
            li.Sort();
            li.Add("==============================================================================");
            return li;
        }
    }
}

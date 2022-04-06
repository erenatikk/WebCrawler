using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;



namespace WebCrawler_Project
{
    class csInternalScan : csHelperMethods //2021112208, 2021112210
    {
        private string MainUrl { get; set; } //2021112201
        private string Host { get; set; }
        public csInternalScan(string target)
        {
            MainUrl = target;
            Host = new Uri(target).Host;
        }

        private  List<string> GetUrl(string Url) //2021112209
        {
            List<string> li = new List<string>();//2021112250
            try
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(Task.Run(() => { return GetSourceAsync(Url); }).Result);
                
                var vrnodes = document.DocumentNode.SelectNodes("//a");
                if (vrnodes != null)
                {
                    foreach (HtmlNode link in vrnodes)
                    {

                        string hrefValue = link.Attributes["href"]?.Value;
                        if(hrefValue != null)
                        {
                            li.Add(hrefValue);
                                                       
                                
                            
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
        
        private csResult CreateData(string url) //2021112223 , 2021112209
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
            List<string> list = GetUrl(MainUrl);
            if(list.Count >0)
            {
                foreach(string item in list)
                {
                    var temp = CreateData(item);
                    if(temp != null)
                    {
                        db.InsertData(temp);
                    }
                    
                }
            }           
        }
    }
}

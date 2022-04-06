using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler_Project
{
    class csResult //2021112208
    {
        public string UrlHash { get; set; }
        public string Url { get; set; }
        public string ParentUrl { get; set; }
        public string SourceCode { get; set; }
        public string InnerText { get; set; }
        public string Host { get; set; }
        public byte[] CompressedSC { get; set; }
        public byte[] CompressedIT { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler_Project
{
    static class csCompress //2021112208
    {
        public static byte[] Zip(string uncompressed)
        {
            using (var outputMemory = new MemoryStream()) //2021112230
            {
                using (var gz = new GZipStream(outputMemory, CompressionLevel.Optimal))
                {
                    using (var sw = new StreamWriter(gz, Encoding.UTF8))
                    {
                        sw.Write(uncompressed);
                    }
                }
                return outputMemory.ToArray();
            }
        }

        public static string Unzip(byte[] compressed)
        {
            string ret = string.Empty;
            using (var inputMemory = new MemoryStream(compressed)) //2021112230
            {
                using (var gz = new GZipStream(inputMemory, CompressionMode.Decompress))
                {
                    using (var sr = new StreamReader(gz, Encoding.UTF8))
                    {
                        ret = sr.ReadToEnd();
                    }
                }
            }
            return ret;
        }
    }
}

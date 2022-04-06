using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler_Project
{
    class csDatabase
    {
        private static string ConnectionString = "Server = DESKTOP-8835U45; Database=WebCrawlerDB;Integrated Security = true";

        public void CreateDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(@"CREATE TABLE  CrawlerResult (ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY, UrlHash VARCHAR(64)UNIQUE NOT NULL, Url VARCHAR(MAX) NOT NULL, ParentUrl VARCHAR(MAX) NOT NULL, SourceCode VARCHAR(MAX), InnerText VARCHAR(MAX), Host VARCHAR(MAX), CrawledDate DATETIME DEFAULT GETDATE() NOT NULL, CompressedSC VARBINARY(MAX) , CompressedIT VARBINARY(MAX) )", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }

        }
        public void InsertData(csResult result)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO CrawlerResult (UrlHash, Url, ParentUrl, SourceCode, InnerText, Host, CompressedSC, CompressedIT) VALUES (@0, @1, @2, @3, @4, @5, @6, @7)", connection))
                    {
                        command.Parameters.AddWithValue("@0", result.UrlHash);
                        command.Parameters.AddWithValue("@1", result.Url);
                        command.Parameters.AddWithValue("@2", result.ParentUrl);
                        command.Parameters.AddWithValue("@3", result.SourceCode);
                        command.Parameters.AddWithValue("@4", result.InnerText);
                        command.Parameters.AddWithValue("@5", result.Host);
                        command.Parameters.AddWithValue("@6", result.CompressedSC);
                        command.Parameters.AddWithValue("@7", result.CompressedIT);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }

        }
        public List<csResult> ReadData()
        {
            List<csResult> results = new List<csResult>();
            try
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT  Url, ParentUrl,  Host  FROM CrawlerResult ORDER BY CrawledDate DESC", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                csResult temp = new csResult();
                               // temp.UrlHash = reader["UrlHash"].ToString();
                                temp.Url = reader["Url"].ToString();
                                temp.ParentUrl = reader["ParentUrl"].ToString();
                              //  temp.SourceCode = reader["SourceCode"].ToString();
                              //  temp.InnerText = reader["InnerText"].ToString();
                                temp.Host = reader["Host"].ToString();
                                results.Add(temp);
                            }
                        }
                    }
                }
                System.Diagnostics.Debug.WriteLine(results.Count());
                return results;
            }
            catch
            {
                return results;

            }

        }
    }


}

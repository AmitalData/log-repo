using System;
using System.Net;
using System.ComponentModel;
using System.Net.Http.Headers;

using System.Threading.Tasks;
using System.Timers;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using SharpCompress.Archives;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using Simplog.Data.InfrastructureModel;
using System.Data.Entity;
using Simplog.Data.ShipmentsModel;
using Logitude.Customs.Data;
using System.Data.Entity.Infrastructure;
using System.Configuration;
using System.Linq;
using NLog;
using System.Threading;

namespace CustomsBook
{
    internal class TaskScheduler
    {

        static Logger logger = LogManager.GetCurrentClassLogger();

        static async void DownloadFile()
        {
        // Define the URL of the ZIP file to download
        string url = "https://shaarolami-query.customs.mof.gov.il/CustomspilotWeb/he/CustomsBook/Home/DownloadFile";
            string downloadedFilePath = @"C:\CustomsBook\Download\fullCustomsBookData.zip";  // Adjust the download path as needed
            string extractFolder = @"C:\CustomsBook\ExtractedFiles";
        
            try
            {
                if (!Directory.Exists(@"C:\CustomsBook\Download"))
                {
                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(@"C:\CustomsBook\Download");
                }

                // Download the ZIP file

                int maxAttempts = 5;
                int attempts = 0;
                bool success = false;

                while (!success && attempts < maxAttempts)
                {
                    
                        // Code to download the ZIP file
                        await DownloadFiles(url, downloadedFilePath);

                        // Check if the downloaded file size is 0 KB
                        long fileSize = new FileInfo(downloadedFilePath).Length;
                        if (fileSize == 0)
                        {
                            attempts++;
                        }
                        else
                        {
                            success = true; // If the file size is not 0 KB, consider the download successful
                        }

                }

                if (File.Exists(downloadedFilePath) && success)
                {
                    await ExtractZipFile(downloadedFilePath, extractFolder);
                    string fileName = FindFileName();
                    if(fileName != null)
                    {
                        MapAccessTables(fileName);

                    }
                }
                else
                {
                    // File is not a valid ZIP file

                    logger.Debug("The file is not a valid ZIP file.");
                    logger.Debug("The file is not a valid ZIP file.");
                }
               
            }
            catch (Exception ex)
            {
                logger.Debug("Error: " + ex.Message);
            }
            finally
            {

                //string[] folders = { "C:\\CustomsBook\\Download", "C:\\CustomsBook\\ExtractedFiles" }; 

                //foreach (string folder in folders)
                //{
                //    Directory.GetFiles(folder).ToList().ForEach(File.Delete);
                //}
            }
        }

         static string FindFileName()
         {

            DirectoryInfo directory = new DirectoryInfo("C:\\CustomsBook\\ExtractedFiles");
            FileInfo[] files = directory.GetFiles();

            // עבור על כל הקובצים
            foreach (FileInfo file in files)
            {
                // בדוק אם הקובץ מסתיים בסיומת accdb
                if (file.Extension == ".accdb")
                {
                    // מצא את שם הקובץ
                    string filename = file.Name;

                    return filename;
                }
            }
            return null;
         }
        static async Task DownloadFiles(string fileUrl, string savePath)
        {

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);

                // Create a cancellation token source
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                // Download the file
                using (HttpResponseMessage response = await client.GetAsync(fileUrl, cancellationToken))
                {
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        using (FileStream fileStream = File.Create(savePath))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }
        
        static async Task ExtractZipFile(string zipFilePath, string extractFolder)
        {
            using (IArchive archive = ArchiveFactory.Open(zipFilePath))
            {
                // Perform extraction asynchronously
                await Task.Run(() => archive.ExtractToDirectory(extractFolder));
            }
        }


        static void MapAccessTables(string fileName)
        { // Get the paths to the Access database and the SQL database
            var accessConnectionString ="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\CustomsBook\\ExtractedFiles\\" + fileName;
            string sqlConnectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {


                try
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.TruncateCustomsBookTables", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.ExecuteNonQuery();
                    }

                    logger.Debug($"Tables Truncate successfully.");
                }

                catch (Exception ex)
                {
                    logger.Debug($"Error occurred: {ex.Message}");
                }
                finally 
                {
                    sqlConnection.Close(); 
                }

            }
                

                
            using (OleDbConnection accessConnection = new OleDbConnection(accessConnectionString))
            {
                accessConnection.Open();

                DataTable schema = accessConnection.GetSchema("Tables");

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    sqlConnection.Open();
                   
                    foreach (DataRow table in schema.Rows)
                    {
                        try
                        {
                            string accessTableName = table["TABLE_NAME"].ToString();
                            string sqlTableName = GetSqlTableName(accessTableName); // Map Access table name to SQL table name
                            if (sqlTableName == null)
                            {
                                continue;
                            }

                            // Delete existing data in SQL table
                            //using (SqlCommand deleteCommand = new SqlCommand($"DELETE FROM {sqlTableName}", sqlConnection))
                            //{
                            //    deleteCommand.ExecuteNonQuery();
                            //}
                            using (OleDbCommand accessCommand = new OleDbCommand($"SELECT * FROM [{accessTableName}]", accessConnection))
                            {
                                using (OleDbDataReader accessReader = accessCommand.ExecuteReader())
                                {
                                    DataTable accessSchema = accessReader.GetSchemaTable();
                                    List<string> sqlSchema = GetColumnNames(sqlTableName);

                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                                    {
                                        //bulkCopy.BulkCopyTimeout = 240;
                                        bulkCopy.DestinationTableName = sqlTableName;
                                        bulkCopy.BatchSize = 1000; // Set desired segment size


                                        foreach (DataRow accessColumn in accessSchema.Rows)
                                        {
                                            string columnName = accessColumn["ColumnName"].ToString();
                                            if (sqlSchema.Contains(columnName))
                                            {
                                                bulkCopy.ColumnMappings.Add(columnName, columnName);
                                            }
                                            else if(columnName == "Connected_CustomsItemDetailsHistoryID")
                                            {
                                                bulkCopy.ColumnMappings.Add(columnName, "Connect_CustItemDetailsHistID");
                                            }
                                            else if(columnName == "WithoutQuota_ComputationMethodDataID")
                                            {
                                                bulkCopy.ColumnMappings.Add(columnName, "WithoutQuota_ComputMethDataID");
                                            }
                                            else if(columnName == "WithinQuota_ComputationMethodDataID")
                                            {
                                                bulkCopy.ColumnMappings.Add(columnName, "WithinQuota_ComputMethDataID");
                                            }
                                        }

                                        bulkCopy.WriteToServer(accessReader);
                                    }
                                }
                            }

                            logger.Debug($"Table {accessTableName} migrated to {sqlTableName} successfully.");
                        }

                        catch (Exception ex)
                        {
                            logger.Debug($"Error occurred: {ex.Message}");
                            continue;
                        }
                    }

                    sqlConnection.Close();
                }

                accessConnection.Close();
            }
        }

        static string GetSqlTableName(string accessTableName)
        {
            // Map Access table names to SQL table names
            switch (accessTableName)
            {
                case "CustomsItem":
                    return "Customs.CB_CustomsItems";
                case "PropertiesDetailsHistory":
                    return "Customs.CB_PropertiesDetailsHistorys";
                case "CustomsItemDetailsHistory":
                    return "Customs.CB_CustomsItemDetailsHistorys";
                case "Rule":
                    return "Customs.CB_Rules";
                case "RuleDetailsHistory":
                    return "Customs.CB_RuleDetailsHistorys";
                case "Tariff_777":
                    return "Customs.CB_Tariffs";
                case "TariffDetailsHistory_777":
                    return "Customs.CB_TariffDetailsHistorys";
                case "ComputationMethodData":
                    return "Customs.CB_ComputationMethodDatas";
                case "Quota":
                    return "Customs.CB_Quotas";
                case "QuotaDetailsHistory":
                    return "Customs.CB_QuotaDetailsHistorys";
                case "QuotaRenewal":
                    return "Customs.CB_QuotaRenewals";
                case "TradeAgreementDetailsHistory_777":
                    return "Customs.CB_TradeAgreementHistories";
                case "TradeAgreement":
                    return "Customs.CB_TradeAgreements";
                case "RegularityRequirement":
                    return "Customs.CB_RegularityRequirements";
                case "CustomsItemExclusion":
                    return "Customs.CB_CustomsItemExclusion";
                case "CountriesExclusion":
                    return "Customs.CB_CountriesExclusions";
                case "RegularityInception":
                    return "Customs.CB_RegularityInceptions";
                case "RegularityRequiredCertificate":
                    return "Customs.CB_RegularityRequiredCertificates";
                case "CustomsItemLinkage":
                    return "Customs.CB_CustomsItemLinkages";
                case "TradeLevy":
                    return "Customs.CB_TradeLevys";
                case "LevyCondition":
                    return "Customs.CB_LevyConditions";
                case "LevyExclusion":
                    return "Customs.CB_LevyExclusions"; 
                case "Vendor":
                    return "Customs.CB_Vendors";
                case "CustomsBookAddition":
                    return "Customs.CB_CustomsBookAdditions";
                case "CustomsBookAdditionsDetailsHistory":
                    return "Customs.CB_CustomsBookAdditionsDetailsHistorys";
                case "AdditionRulesDetailsHistory":
                    return "Customs.CB_AdditionRulesDetailsHistorys";
                

                // Add additional mappings as needed
                default:
                    return null; // Use the same name if no mapping is defined
            }
        }
        
        static List<string> GetColumnNames(string tableName)
        {
            List<string> columns = new List<string>();
            string tableNameAfterDot = tableName.Substring(tableName.LastIndexOf('.') + 1);

            string strConnect = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConnect))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand(@"SELECT COLUMN_NAME 
                                 FROM INFORMATION_SCHEMA.COLUMNS 
                                 WHERE TABLE_NAME = @yourtableName", con))
                {

                    com.Parameters.AddWithValue("@yourtableName", tableNameAfterDot);
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        
                        while (reader.Read()) // Iterate over each row in the result set
                        {
                            string columnName = reader.GetString(0); // Access the first column (index 0) as a string
                            columns.Add(columnName);
                        }
                    }
                }
            }
            return columns;
        }



        static List<string> SqlTablesList()
        {
            List<string> tablesname = new List<string>
            {
                "Customs.CB_TradeAgreementHistories",
                "Customs.CB_TariffDetailsHistorys",
                "Customs.CB_Tariffs",
                "Customs.CB_RegularityRequiredCertificates",
                "Customs.CB_QuotaRenewals",
                "Customs.CB_PropertiesDetailsHistorys",
                "Customs.CB_LevyExclusions",
                "Customs.CB_LevyConditions",
                "Customs.CB_AdditionRulesDetailsHistorys",
                "Customs.CB_CustomsItemExclusion",
                "Customs.CB_CustomsBookAdditionsDetailsHistorys",
                "Customs.CB_CustomsItemLinkages",
                "Customs.CB_CountriesExclusions",
                "Customs.CB_RuleDetailsHistorys",
                "Customs.CB_CustomsBookAdditions",
                "Customs.CB_Vendors",
                "Customs.CB_RegularityInceptions",
                "Customs.CB_CustomsItemDetailsHistorys",
                "Customs.CB_CustomsItems",
                "Customs.CB_RegularityRequirements",
                "Customs.CB_QuotaDetailsHistorys",
                "Customs.CB_Quotas",
                "Customs.CB_Rules",
                "Customs.CB_TradeAgreements",
                "Customs.CB_TradeLevys",
                "Customs.CB_ComputationMethodDatas"

            };

            return tablesname;
        }



        static void Main(string[] args)
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine("C:\\LWC_NEW\\Logitude\\CustomsBook\\NLog.config"));
            logger.Debug("Start TaskScheduler");
            MapAccessTables("AccessDBTamplate20240201.accdb");
            //// Set the desired time for downloading files
            //int desiredHour = 2; // Set the desired hour here
            //int desiredMinute = 0; // Set the desired minute here
            //int desiredSecond = 0; // Set the desired second here

            //// Get the current time
            //DateTime now = DateTime.Now;

            //// Calculate the time until the desired time for the first execution
            //DateTime desiredTime = new DateTime(now.Year, now.Month, now.Day, desiredHour, desiredMinute, desiredSecond);
            //TimeSpan timeUntilDesiredTime = desiredTime - now;
            //if (timeUntilDesiredTime.TotalMilliseconds < 0)
            //{
            //    // If the desired time has already passed for the day, add 24 hours to the desired time
            //    desiredTime = desiredTime.AddDays(1);
            //    timeUntilDesiredTime = desiredTime - now;
            //}

            //// Create a timer with the interval set to the time until the desired time
            ////צריך Timer timer = new Timer(timeUntilDesiredTime.TotalMilliseconds);
            //Timer timer = new Timer(0.5 * 60 * 1000);
            //bool hasDownloaded = false;

            //// Hook up the Elapsed event to the DownloadFiles method
            //timer.Elapsed += (sender, e) =>
            //{
            //    if (!hasDownloaded)
            //    {
            //        DownloadFile();
            //        hasDownloaded = true;
            //    }
            //};


            //// Set the timer's interval to 24 hours
            ////צריך timer.Interval = 24 * 60 * 60 * 1000; // 24 hours * 60 minutes * 60 seconds * 1000 milliseconds

            //// Start the timer
            //timer.Start();

            Console.ReadLine();
        }
    }
}

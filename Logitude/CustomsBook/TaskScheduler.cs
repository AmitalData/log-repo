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
using Simplog.Data.ShipmentsModel;
using Logitude.Customs.Data;
using System.Configuration;
using System.Linq;
using NLog;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;

namespace CustomsBook
{
    internal class TaskScheduler
    {

        public static Logger logger = LogManager.GetCurrentClassLogger();
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
                    TruncateTables();
                    List<string> fileNames = FindFileNames();
                    foreach (string fileName in fileNames)
                    {
                        if (fileName != null)
                        {
                            MapXmlToTables(fileName);
                        }
                    }
                }
                else
                {
                    // File is not a valid ZIP file
                    logger.Debug("The file is not a valid ZIP file.");
                }

            }
            catch (Exception ex)
            {
                logger.Debug("Error: " + ex.Message);
            }
            finally
            {

                string[] folders = { "C:\\CustomsBook\\Download", "C:\\CustomsBook\\ExtractedFiles" };

                foreach (string folder in folders)
                {
                    Directory.GetFiles(folder).ToList().ForEach(File.Delete);
                }
            }
        }
        static List<string> FindFileNames()
        {
            DirectoryInfo directory = new DirectoryInfo("C:\\CustomsBook\\ExtractedFiles");
            FileInfo[] files = directory.GetFiles("*.xml");

            return files.Select(file => file.Name).ToList();
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

        static string GetNameFromFilename(string filename)
        {
            // Check if the file name contains "_"
            int index = filename.IndexOf('_');
            if (index != -1)
            {
                // If "_" exists, take only the part before it
                return filename.Substring(0, index);
            }
            else
            {
                // If no "_", just return the file name without extension
                return System.IO.Path.GetFileNameWithoutExtension(filename);
            }
        }

        static void TruncateTables()
        {
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

                    logger.Debug($"Tables truncated successfully.");
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

        }

        static void MapXmlToTables(string fileName)
        {

            // Get the path to the XML file and the SQL database
            string xmlFilePath = Path.Combine("C:\\CustomsBook\\ExtractedFiles\\", fileName);
            string sqlConnectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            string xmlTableName = GetNameFromFilename(fileName);
            string sqlTableName = GetSqlTableName(xmlTableName); // Map XML table name to SQL table name
            XDocument xmlDoc = new XDocument();
            if (sqlTableName != null)
            {
                try
                {
            // Load the XML document
                    xmlDoc = XDocument.Load(xmlFilePath);

                }
                catch (Exception ex)
                {
                    logger.Debug($"Exception in Table {xmlTableName} migrated to {sqlTableName} --- Error --- \n Error occurred: {ex.Message}");
                    Console.WriteLine($"Exception in Table {xmlTableName} migrated to {sqlTableName} --- Error --- \n Error occurred: {ex.Message}");
                    return;
                }
            }
            else {
                logger.Debug($"Table {fileName} name not found in mapping.");
                return;
            }
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                sqlConnection.Open();

                // Iterate through each table element in the XML document
                foreach (XElement tableElement in xmlDoc.Root.Elements())
                {
                    try
                    {
                      

                        DataTable dataTable = new DataTable(sqlTableName);

                        // Define columns in DataTable based on XML data
                        foreach (XElement rowElement in tableElement.Elements())
                        {
                            DataRow row = dataTable.NewRow();
                            foreach (XElement columnElement in rowElement.Elements())
                            {
                                if (!dataTable.Columns.Contains(columnElement.Name.LocalName))
                                {
                                    dataTable.Columns.Add(columnElement.Name.LocalName);
                                }
                                row[columnElement.Name.LocalName] = columnElement.Value;
                            }
                            dataTable.Rows.Add(row);
                        }
                        List<string> sqlSchema = GetColumnNames(sqlTableName);

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                        {
                            bulkCopy.DestinationTableName = sqlTableName;
                            bulkCopy.BatchSize = 1000; // Set desired segment size

                            // Add column mappings
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                string columnName = column.ColumnName;
                                if (columnName == "ID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "CB_ID");
                                }
                                if (sqlSchema.Contains(columnName))
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, columnName);
                                }
                                else if (columnName == "Connected_CustomsItemDetailsHistoryID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "Connect_CustItemDetailsHistID");
                                }
                                else if (columnName == "Valid_CustomsItemDetailsHistoryID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "CustomsItemDetailsHistoryID");
                                }
                                else if (columnName == "Valid_PropertiesDetailsHistoryID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "PropertiesDetailsHistoryID");
                                }
                                else if (columnName == "CI_CustomsItemHierarchicLocationIDNum")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "ItemHierarchicLocationID");
                                } 
                                else if (columnName == "CIH_CustomsItemEntityStatusIDNum")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "CustomsItemEntityStatusIDNum");
                                }
                                else if (columnName == "ValidQuotaDetailsHistoryID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "ValidQuotaDetailsHistoryID");
                                }
                                if (sqlTableName == "Customs.CB_TariffComputedDatas")
                                {
                                    if (columnName == "WithoutQuota_ComputationMethodDataID")
                                {
                                        bulkCopy.ColumnMappings.Add(columnName, "WithoutQuota_ComputationID");
                                    }
                                    else if (columnName == "WithinQuota_ComputationMethodDataID")
                                    {
                                        bulkCopy.ColumnMappings.Add(columnName, "WithinQuota_ComputationID");
                                    }
                                }
                                else
                                {
                                    if (columnName == "WithoutQuota_ComputationMethodDataID")
                                    {
                                    bulkCopy.ColumnMappings.Add(columnName, "WithoutQuota_ComputMethDataID");
                                }
                                else if (columnName == "WithinQuota_ComputationMethodDataID")
                                {
                                    bulkCopy.ColumnMappings.Add(columnName, "WithinQuota_ComputMethDataID");
                                }
                            }
                            }
                            bulkCopy.WriteToServer(dataTable);
                        }

                        logger.Debug($"Table {xmlTableName} migrated to {sqlTableName} successfully.");
                        Console.WriteLine($"Table {fileName} migrated to {sqlTableName} successfully.");
                    }
                    catch (Exception ex)
                    {
                        logger.Debug($"Error occurred: {ex.Message}");
                        continue;
                    }
                }

                sqlConnection.Close();
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
                case "Tariff":
                    return "Customs.CB_Tariffs";
                case "TariffDetailsHistory":
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
                case "CustomsItemComputedData":
                    return "Customs.CB_CustomsItemComputedDatas";
                case "TariffComputedData":
                    return "Customs.CB_TariffComputedDatas";
                case "QuotaComputedData":
                    return "Customs.CB_QuotaComputedDatas";
                case "RegularityRequirementComputedData":
                    return "Customs.CB_RequirementComputedDatas";


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



        static void Main(string[] args)
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "NLog.config"));
            
            logger.Debug("Start TaskScheduler");

            //DownloadFile();

            UpdateAzureSearchAIData.Update();

            Console.ReadLine();
        }
    }
}

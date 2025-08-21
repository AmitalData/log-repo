using NLog;
using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Globalization;
using Logitude.SystemLogs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.TreeFilterQuery;
using System.Web.Caching;
using System.Web;
using System.Net.Mail;
using System.Collections.Specialized;

namespace CustomsBook
{
    internal class UpdateCustomsBook
    {

        static readonly Logger logger = Program.logger;

        static List<string> tempTables = new List<string>();
        static string sqlConnectionString = ConfigurationManager.ConnectionStrings["LogitudeStr"].ConnectionString;

        public static async Task Run()
        {            
            await DownloadFile();
        }
        static async Task DownloadFile()
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

                    // Download the ZIP file
                    await DownloadFiles(url, downloadedFilePath);

                    long fileSize = new FileInfo(downloadedFilePath).Length;
                    if (fileSize == 0)
                    {
                        attempts++;
                    }
                    else
                    {
                        success = true; 
                    }

                }

                if (File.Exists(downloadedFilePath) && success)
                {
                    await ExtractZipFile(downloadedFilePath, extractFolder);

                    //Delete temp tables
                    CustomsBookRepository.TruncateTables();
                    
                    // Get rules from WS 8319
                    GetRulesFromWS8319();
                    
                    List<string> fileNames = FindFileNames();
                    
                    foreach (string fileName in fileNames)
                    {
                        if (fileName != null)
                        {
                            MapXmlTempTable(fileName);
                        }
                    }

                    // Swap temp tables to main tables
                    foreach (string tempTable in tempTables)
                    {
                        CustomsBookRepository.SwapTempToMainTable(tempTable);
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
                SendEmailAlert(ex);
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
                client.Timeout = TimeSpan.FromMinutes(60);

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

        public static HttpRuntime _httpRuntime { get; set; }

       
        static void GetRulesFromWS8319()
        {

            List<int> customsItemIds = CustomsBookRepository.GetCustomsItemIdWithRules();
            
            StartStatic();

            int tenant = CustomsBookRepository.GetTenantFromCustomsSettings();

            foreach (var id in customsItemIds)
            {
                CustomItemRuleRequestParams requestParamsData = new CustomItemRuleRequestParams()
                {
                    Tenant = tenant,
                    customsItemId = id,
                    validToDate = DateTime.Now,
                };
                DCAInGet_CB_MSG_8319_CustomItemRuleMessagingService messagingService = new DCAInGet_CB_MSG_8319_CustomItemRuleMessagingService();
                CustomItemRuleResponseData responseData = messagingService.Send(requestParamsData);
                if (responseData != null && !responseData.Succeeded)
                {
                    throw new Exception(responseData.UserMessage);
                }
            }
            tempTables.Add("TEMP_CB_RuleClassifications");
        }

        static void MapXmlTempTable(string fileName)
        {
            // Get the path to the XML file and the SQL database
            string xmlFilePath = Path.Combine("C:\\CustomsBook\\ExtractedFiles\\", fileName);
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
                    SendEmailAlert(ex);
                    throw ex;
                }
            }
            else
            {
                logger.Debug(message: $"Table {fileName} name not found in mapping.");
                return;
            }
            

                string tempTableName = $"TEMP_{sqlTableName.Split('.').Last()}";

                if (!tempTables.Contains(tempTableName))
                {
                    tempTables.Add(tempTableName);
                }

              
                foreach (XElement tableElement in xmlDoc.Root.Elements())
                {
                    
                        DataTable dataTable = new DataTable(sqlTableName);

                        foreach (XElement rowElement in tableElement.Elements())
                        {
                            DataRow row = dataTable.NewRow();
                            foreach (XElement columnElement in rowElement.Elements())
                            {
                                if (!dataTable.Columns.Contains(columnElement.Name.LocalName))
                                {
                                    dataTable.Columns.Add(columnElement.Name.LocalName); // Add new column if necessary
                                }
                                row[columnElement.Name.LocalName] = columnElement.Value;
                            }
                            dataTable.Rows.Add(row);
                        }


                        List<string> sqlSchema = CustomsBookRepository.GetColumnNames(sqlTableName);
                        
                        // Insert data into the temp table
                        CustomsBookRepository.InsertDataToTempTable(dataTable, tempTableName, sqlSchema, sqlTableName);

                        logger.Debug(message: $"Table {xmlTableName} migrated to {sqlTableName} successfully.");
                        Console.WriteLine($"Table {fileName} migrated to {sqlTableName} successfully.");
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

        public static void EnsureHttpRuntime()
        {
           
                if (null == _httpRuntime)
                {
                    
                        if (null == _httpRuntime)
                        {
                            _httpRuntime = new HttpRuntime();
                        }
              
                }
           
        }
        public static Cache Cache
        {
            get
            {
                    EnsureHttpRuntime();
                    return HttpRuntime.Cache;
                
            }



        }
        public static void StartStatic(Action<bool, bool,int> BuildObjectTablesZipFilesDataAction = null, string ProductInfo = null)
        {
            InjectionUtil.Init(CreateAmitalRestrictOwnerModelService: null, null, null, () => (new ByteCompressorUtil()) as IByteCompressorUtil, null, null, null, null, () => (new TreeFilterQueryService()) as ITreeFilterQueryService);

            if (string.IsNullOrEmpty(LogitudeSettings.DeploymentStage))
            {
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                LogitudeSettings.DatabaseManagementSystem = dbms;
                LogitudeSettings.DebugKey = System.Configuration.ConfigurationManager.AppSettings.Get("DebugKey");
                SettingRepository settingRepository = new SettingRepository();
                Setting setting = settingRepository.GetSingleSetting("1");
                LogitudeSettings.Id = setting.Id;
                LogitudeSettings.ChampEnv = setting.ChampEnv;
                LogitudeSettings.ChampURL = setting.ChampURL;
                LogitudeSettings.ChampTestAPIURL = setting.ChampTestAPIURL;
                LogitudeSettings.ChampTestAPIPassword = setting.ChampTestAPIPassword;
                LogitudeSettings.ChampProdAPIURL = setting.ChampProdAPIURL;
                LogitudeSettings.ChampProdAPIPassword = setting.ChampProdAPIPassword;
                LogitudeSettings.CustomerCareIP = setting.CustomerCareIP;
                LogitudeSettings.DeploymentStage = setting.DeploymentStage;
                LogitudeSettings.IsLogEnabled = setting.IsLogEnabled;
                LogitudeSettings.LogitudeURL = setting.LogitudeURL;
                LogitudeSettings.TotangoServiceId = setting.TotangoServiceId;
                LogitudeSettings.UsingAzure = setting.UsingAzure;
                LogitudeSettings.StorageAccountKey = setting.StorageAccountKey;
                LogitudeSettings.StorageAccountName = setting.StorageAccountName;
                LogitudeSettings.StorageType = setting.StorageType;
                LogitudeSettings.LogitudeCRMTenantNumber = setting.LogitudeCRMTenantNumber;
                LogitudeSettings.AutoSignupEmail = setting.AutoSignupEmail;
                LogitudeSettings.AutoSignupPassword = setting.AutoSignupPassword;
                LogitudeSettings.ForceHttps = setting.ForceHttps;
                LogitudeSettings.CheckConnectionURL = setting.CheckConnectionURL;
                LogitudeSettings.IOSSharedAppMinimumVersion = setting.IOSSharedAppMinimumVersion;
                LogitudeSettings.WorkEnvironment = setting.WorkEnvironment;
                LogitudeSettings.LogoCode = setting.LogoCode;
                LogitudeSettings.EnableHybridQueue = setting.EnableHybridQueue;

                //LogitudeSettings.IsCostomsDeploy = Logitude.Customs.BL.Utils.CustomsSettingUtil.ForceDownloadXapFromIIS();
                //LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;
                LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;

                LogitudeSettings.HandleDbExceptionInject = ExceptionHandler.HandleDbException;
                LogitudeSettings.HandleBuildObjectTablesZipFilesData_Inject = BuildObjectTablesZipFilesDataAction;
                LogitudeSettings.GetUserNameInject = AuthenticationUtil.ResolveUserIdentityName;

                LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;
                LogitudeSettings.QueueServiceMode = setting.QueueServiceMode;
                LogitudeSettings.ABMProductId = setting.ABMProductId;
                LogitudeSettings.AzureFolderName = setting.AzureFolderName;
                LogitudeSettings.CPUIntensiveWebServicesURL = setting.CPUIntensiveWebServicesURL;


            }

            //  CommunicationWorkerRole.ThreadedRoleEntryPoint.SetWorkerRoleName();


            if (LogitudeSettings.IsCostomsDeploy)
            {
                LogitudeSettings.ProductInfo = ProductInfo;

                var he = new CultureInfo("he-IL");// '("en-US") '    "he-IL")
                he.DateTimeFormat.DateSeparator = ".";
                he.DateTimeFormat.ShortDatePattern = "dd-MM-yy";// ' "yyyy/MM/dd" '  ' "DD/MM/YYYY"
                System.Threading.Thread.CurrentThread.CurrentCulture = he;



                LogitudeSettings.RunWorkerRoleAutomaticBreakPoint = false;

                LogitudeSettings.WorkerRoleName = LogitudeSettings.WorkerRoleName ?? "production";
            }
            // string storageServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("StorageServiceMode");
            //string queueServiceMode = System.Configuration.ConfigurationManager.AppSettings.Get("QueueServiceMode");
            ContainerAccessor.InitContainer();
            Dictionary<int, string> globalDBs = new Dictionary<int, string>();
            //List<GlobalTenant> globalTenants = new GlobalDomainService().GetActiveTenants();
            //foreach (var item in globalTenants)
            //{
            //    globalDBs.Add(item.Id, item.GlobalDBId);
            //}
            CacheManager.CacheWrapper = CacheManager.CacheWrapper ?? new CacheWrapper(Cache, globalDBs);
            var storageAccount = StorageAcountDetails.StorageAccount; 
            LogitudeSettings.HandleLogMe = new Action<string, bool, string, DateTime>((mess, err, suffix, stopLogAt) =>
            {
                if (DateTime.Now > stopLogAt) return;
            });


        }


        public static void SendEmailAlert(Exception ex)
        {
            try
            {
                string config = System.Configuration.ConfigurationManager.AppSettings.Get("Settings");

                var emailSection = (NameValueCollection)ConfigurationManager.GetSection("emailSettings");

                string smtpServer = emailSection["smtp"];
                string smtpUser = emailSection["userName"];
                string smtpPass = emailSection["password"];
                string subject = emailSection["subject"];
                string[] recipients = emailSection["recipients"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                using (var client = new SmtpClient(smtpServer))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    client.EnableSsl = true;

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(smtpUser);

                        foreach (var r in recipients)
                        {
                            if (!string.IsNullOrWhiteSpace(r))
                                message.To.Add(r.Trim());
                        }

                        message.Subject = subject;
                        message.Body = $"An error occurred:\n\n{ex}";

                        client.Send(message);
                    }
                }

                Console.WriteLine($"Email sent successfully to: {string.Join(", ", recipients)}");
            }
            catch (Exception sendEx)
            {
                Console.WriteLine($"Failed to send email: {sendEx.Message}");
                SendEmailAlert(sendEx);
            }
        }
    }
}

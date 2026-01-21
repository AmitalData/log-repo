using CommunicationWorkerRole.Tasks;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.FTP;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CommunicationWorkerRole.Services
{
    public class SFTPSchedulerTaskService: FTPSchedulerTaskServiceBase
    {
        public SFTPSchedulerTaskService()
        {

        }
        public SFTPSchedulerTaskService(TaskManagerBase task) : base(task)
        {
        }

        public void ReadSFTPFilesBySchedulerDetailsToAnalyzeQueue(SchedulerDetails schedulerDetails)
        {
            string p_status = "";
            string p_message = "";
            SFTPService sftpService = new SFTPService();

            sftpService.Logon(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password, "22", schedulerDetails.FTPDetails.Folder, out p_status, out p_message);

            if (p_status == "-1")
            {
                AddWarning("SFTP upload file failed: " + p_message);
                return;
            }


            List<string> directoryFiles = GetFilteredDirectoryFileNamesByFTPDetails(schedulerDetails, sftpService);

            this.DownloadedFilesCount = 0;
            this.FailedFilesCount = 0;

            foreach (string fileName in directoryFiles)
            {
                string extention = Path.GetExtension(fileName);
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
                {
                    string p_read_status = "";
                    byte[] fileData = DownloadFTPFile(schedulerDetails, sftpService, fileName, out p_read_status);
                    if (p_read_status != "-1")
                    {
                        if(schedulerDetails?.FTPDetails?.Subject == "DocumentSFTP")
                        {
							AddToAzureQueue(fileName, fileData, schedulerDetails);
						}
						else
                        {
							AddToAnalyzeQueue(fileName, fileData, schedulerDetails);
						}
						DeleteFTPFile(schedulerDetails, sftpService, fileName);
                    }
                }
            }


            AddMessage(FTPLogBuilder.BuildLogLine(this.GetFilesDownloadingSummery()));

        }

        public byte[] DownloadFTPFile(SchedulerDetails schedulerDetails, SFTPService sftpService, string fileName, out string p_status)
        {
            string p_message;
          
             
            string filePath = GetFilePath(schedulerDetails, fileName, out p_message);
            byte[] fileData = sftpService.DownloadFile(fileName, out p_status, out p_message);
            if (p_status == "-1")
            {
                this.FailedFilesCount++;
            }
            else
            {
                this.DownloadedFilesCount++;
            }

            AddStatusMessage(p_message, p_status);

            return fileData;
        }

        public void DeleteFTPFile(SchedulerDetails schedulerDetails, SFTPService sftpService, string fileName)
        {
            string p_message;
            string p_status = "";
            string filePath = GetFilePath(schedulerDetails, fileName, out p_message);
            sftpService.DeleteFile(fileName, out p_status, out p_message);
            AddStatusMessage(p_message, p_status);
        }


       

        private string GetFilePath(SchedulerDetails schedulerDetails, string fileName, out string p_message)
        {
            p_message = "";
            string filePath = fileName;
            if (!string.IsNullOrEmpty(schedulerDetails.FTPDetails.Folder) && !fileName.Contains(schedulerDetails.FTPDetails.Folder))
            {
                filePath = schedulerDetails.FTPDetails.Folder + "/" + fileName;
            }

            return filePath;
        }

        public List<string> GetFilteredDirectoryFileNamesByFTPDetails(SchedulerDetails schedulerDetails, SFTPService sftpService)
        {
            List<string> directoryFiles = new List<string>();
            //SFTPService sftpService = new SFTPService();
            string p_message;
            string p_status = "";
            string pattern = "*";
            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Extension))
            //{
            //    pattern += "." + schedulerDetails.FTPDetails.Extension.TrimStart('.');
            //}

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
            //{
            //    pattern = schedulerDetails.FTPDetails.Prefix + pattern;
            //    //directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    //f.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)))
            //    //.ToList();
            //}

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Suffix))
            //{

            //    pattern = pattern + schedulerDetails.FTPDetails.Suffix;
            //    //directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    //f.Replace(Path.GetExtension(f), "")
            //    //.EndsWith(schedulerDetails.FTPDetails.Suffix, StringComparison.CurrentCultureIgnoreCase)))
            //    //.ToList();
            //}

            pattern = (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix) ? schedulerDetails.FTPDetails.Prefix : "") + "*" + (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Suffix) ? schedulerDetails.FTPDetails.Suffix : "") +
                (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Extension) ? "." + schedulerDetails.FTPDetails.Extension.TrimStart('.') : "");

                           directoryFiles = sftpService.DirList(pattern, true, false, out p_status, out p_message);
            AddStatusMessage(p_message, p_status);

            //.Where(f => !string.IsNullOrWhiteSpace(f) && !string.IsNullOrWhiteSpace(Path.GetExtension(f))).ToList();

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Extension))
            //{
            //    directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    Path.GetExtension(f).TrimStart('.')
            //    .Equals(schedulerDetails.FTPDetails.Extension, StringComparison.CurrentCultureIgnoreCase)))
            //    .ToList();
            //}

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Prefix))
            //{
            //    directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    f.StartsWith(schedulerDetails.FTPDetails.Prefix, StringComparison.CurrentCultureIgnoreCase)))
            //    .ToList();
            //}

            //if (!string.IsNullOrWhiteSpace(schedulerDetails.FTPDetails.Suffix))
            //{
            //    directoryFiles = directoryFiles.Where(f => (!string.IsNullOrEmpty(f) &&
            //    f.Replace(Path.GetExtension(f), "")
            //    .EndsWith(schedulerDetails.FTPDetails.Suffix, StringComparison.CurrentCultureIgnoreCase)))
            //    .ToList();
            //}

            return directoryFiles;
        }

		private void AddToAnalyzeQueue(string fileName, byte[] fileData, SchedulerDetails schedulerDetails)
		{
			if (schedulerDetails.FTPDetails.Subject == "fail test")
			{
				throw new Exception("failure testing!");
			}

			var createdate = TenantServerConfigration.GetCurrentDateTime(0);
			using (TransactionScope scope = TransactionFactory.GetNewTransaction())
			{
				fileName = fileName.Split('/')[fileName.Split('/').Length - 1].ToLower();
				AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

				AnalyzeQueue analyzeQueue = new AnalyzeQueue()
				{
					Subject = schedulerDetails.FTPDetails.Subject,
					CreateDate = createdate,
					From = schedulerDetails.FTPDetails.From,
					Id = IdCounter.GetNumber("AnalyzeQueue", 0),
					MessageBody = fileData,
					Status = "W",
					Retries = 0,
					ConnectedToEntity = false,
					ConnectedToTenant = false,
					Tenant = schedulerDetails.Tenant,
					FileSize = fileData != null ? fileData.Length : 0,
					FileName = fileName,
				};

				analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
				analyzeQueueReposiory.Add(analyzeQueue);
				analyzeQueueReposiory.SubmitChanges();

				scope.Complete();
			}
		}
		private async void  AddToAzureQueue(string fileName, byte[] fileData, SchedulerDetails schedulerDetails)
		{
			string response = await login("a0ee73e1-34c4-469a-a3cf-4db8850df683", "q328Q~Jo2-kD_7SJxun6VOkK.3s1cV3PT1BqFbjx");
			var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response);
			var accessToken = tokenResponse.access_token;
			var expires_in = tokenResponse.expires_in;
			string partner_token = "2c12d723-5cbf-4be0-8c70-3ca1bf275b91";
			string api_token = "15fe2753-875a-4e6b-80e1-f70f7fdfc133";
			string caller_objectid = "c2728789-eec7-441d-8a5b-07038362ead5";
			
			string customsDocumentTypeCode = string.Empty;
			string hawb = string.Empty;
			DocumentType documentType = null;
			var createdate = TenantServerConfigration.GetCurrentDateTime(0);
			using (TransactionScope scope = TransactionFactory.GetNewTransaction())
			{
				fileName = fileName.Split('/')[fileName.Split('/').Length - 1].ToLower();
				GetcustomsDocumentTypeAndHawbFromFileName(fileName, schedulerDetails.Tenant, out customsDocumentTypeCode, out hawb, out documentType);

				string documentJson = CreateExampleDocument(hawb, customsDocumentTypeCode, schedulerDetails?.FTPDetails?.From);

				string responseText;
				bool success = false;
				SendResponseToken sendResponseToken = null;
				(responseText, success) = await SendDocument(partner_token,  api_token,  caller_objectid, accessToken,fileName, fileData, schedulerDetails, documentJson);
				if (success)
				{
					sendResponseToken = JsonConvert.DeserializeObject<SendResponseToken>(responseText);
				}

				if (success)
				{
					if (sendResponseToken.success)
					{
						var correlation = sendResponseToken.correlationID;
						
						AddMessage($"{fileName} sent Successfully, correlationID : {correlation}");
					}
					else
					{
						var erroreMessage = sendResponseToken.message;
						AddMessage($"*** ERROR *** {fileName} did not sent, error message : {erroreMessage}");
					}
				}
				else
				{
					AddMessage($"*** ERROR *** {fileName} did not sent, error message : {responseText}");
				}
			}
		}
		static readonly string baseUri1 = "https://apim-amital-api.azure-api.net/amital-api-10/v1";
		public static async Task<string> login(string client_id, string client_secret)
		{

			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/491f0986-ecef-4c36-b29a-85c3d479172a/oauth2/v2.0/token");

			request.Headers.Add("Cookie", "fpc=AtVcCAKBQhxCnithyTU3xnrA7fEMAQAAAEOyRN0OAAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");

			var collection = new List<KeyValuePair<string, string>>();

			collection.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
			collection.Add(new KeyValuePair<string, string>("client_id", client_id));
			collection.Add(new KeyValuePair<string, string>("client_secret", client_secret));
			collection.Add(new KeyValuePair<string, string>("scope", "api://0f1197ba-f79a-4432-b9fd-e4655e1646f6/.default"));

			var content = new FormUrlEncodedContent(collection);
			request.Content = content;

			var response = await client.SendAsync(request);

			response.EnsureSuccessStatusCode();

			Console.WriteLine(await response.Content.ReadAsStringAsync());
			return await response.Content.ReadAsStringAsync();
		}
		public static async Task<(string, bool)> SendDocument(string partner_token, string api_token, string caller_objectid, string authorizationToken,string fileName,byte[] fileData, SchedulerDetails schedulerDetails,string documentJson)
		{
			var client = new HttpClient();
			var endpoint = $"{baseUri1}/UploadDocument";

			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
			var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

			//request.Headers.Add("partner-token", "6c68e744-0104-4ca6-9049-89077b2608af");
			//request.Headers.Add("api-token", "f7797efb-604e-400f-b975-045c836d6152");
			//request.Headers.Add("caller-objectid", "c0a66682-82dc-4a1a-83ac-e11a097969b0");
			client.DefaultRequestHeaders.Add("partner-token", partner_token);
			client.DefaultRequestHeaders.Add("api-token", api_token);
			client.DefaultRequestHeaders.Add("caller-objectid", caller_objectid);

			request.Headers.Add("x-functions-key", "FnDaCtzap5CTgEBFTDXJAKBiRqL29Xnj2HGpSju3PfKtAzFu9Q7CEA==");
			request.Headers.Add("Authorization", $"Bearer {authorizationToken}");

			
				var content = new MultipartFormDataContent();

				var fileContent = new ByteArrayContent(fileData);
				fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				content.Add(fileContent, "file", fileName);
				content.Add(new StringContent(documentJson), "request");
				request.Content = content;

				try
				{
					bool async = false;
					if (async)
					{
						var response = await client.SendAsync(request);
						response.EnsureSuccessStatusCode();						
						Console.WriteLine(await response.Content.ReadAsStringAsync());
						return (await response.Content.ReadAsStringAsync(), response.IsSuccessStatusCode);
					}
					else
					{
						var response = client.SendAsync(request);
						response.Result.EnsureSuccessStatusCode();						
						string response_content = await response.Result.Content.ReadAsStringAsync();
						Console.WriteLine(response_content);
						return (response_content, response.Result.IsSuccessStatusCode);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred: {ex.Message}");
					return (ex.ToString(), false);
				}
			
		}		
		static string CreateExampleDocument(string parcelTrackingNumber, string documentType,string partnerCode)
		{

			var exampleDocument = new
			{
				sender = partnerCode,
				dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
				messageCount = 1,
				messages = new[]
				{
				new
				{
					messageType = "Document",
					messageCount = 1,
					document = new
					{
						documentType = documentType,
						parcelTrackingNumber = parcelTrackingNumber,
						parcelID = "ZZZZ",
						parcelDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
						docReference = Guid.NewGuid().ToString(),
						fileName = $"{documentType}_{parcelTrackingNumber}.PDF"
					}
				}
			}
			};

			return JsonConvert.SerializeObject(exampleDocument, Formatting.Indented);
		}
		private void GetcustomsDocumentTypeAndHawbFromFileName(string fileName, int tenant, out string customsDocumentType, out string hawb, out DocumentType documentType)
		{
			customsDocumentType = string.Empty;
			hawb = string.Empty;
			documentType = null;

			try
			{
				if (string.IsNullOrWhiteSpace(fileName))
					return;

				string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
				string[] parts = nameWithoutExtension.Split('_');

				if (parts.Length == 2)
				{
					documentType = new DocumentTypeQuery(tenant).GetDocumentTypeByCode(parts[0], tenant);
					if (documentType != null)
					{
						customsDocumentType = parts[0]?.ToUpper();
						hawb = parts[1]?.ToUpper();
						return;
					}

					documentType = new DocumentTypeQuery(tenant).GetDocumentTypeByCode(parts[1], tenant);
					if (documentType != null)
					{
						customsDocumentType = parts[1]?.ToUpper();
						hawb = parts[0]?.ToUpper();
						return;
					}
				}
				else if (parts.Length == 3)
				{
					customsDocumentType = "OTH";
					hawb = parts[2]?.ToUpper();
				}
			}
			catch (Exception ex)
			{

			}
		}
		public class TokenResponse
		{
			public string token_type { get; set; }
			public int expires_in { get; set; }
			public int ext_expires_in { get; set; }
			public string access_token { get; set; }
		}
		public class SendResponseToken
		{
			public string correlationID { get; set; }
			public bool success { get; set; }
			public string message { get; set; }
			public int statusCode { get; set; }
		}
	}
}


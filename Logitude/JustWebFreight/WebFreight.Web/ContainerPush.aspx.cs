using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using Logitude.BL.ShipmentsModel.EntityQueries;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;  
using Simplog.Data.CommonDataModel.EntityPOCOs;  
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Azure;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Logitude.SystemLogs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.Server.Tools.QueueService;

namespace WebFreight.Web
{
    public partial class ContainerPush : System.Web.UI.Page
    {
        string ftpuser = "AMITAL";
        string ftppassword = "Bangkok2007";
        protected void Page_Load(object sender, EventArgs e)
        { 
            string tempfolder = Server.MapPath(@"~\Temp");
            int tenant = 0;
            string ComId = "";
            string ComStatusCode = "";
            try
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Request Form");
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("=================");
                foreach (string key in Request.Form.AllKeys)
                {
                    var val = Request.Form[key];
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug(key + " :" + Request.Form[key]);
                }
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Request Header");
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("=================");
                foreach (string key in Request.Headers.AllKeys)
                {
                   NetCommonHelper.Logger.DevLog.Instance.WriteDebug(key + " :" + Request.Headers[key]);
                }
                string data = getData(Request.InputStream);
                WriteData(data);

				AnalyzeContainerStatus(data);

			}
            catch (Exception ex)
            {

                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                }
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    errorMessage += Environment.NewLine + ex.StackTrace;
                }
                AzureLog.SaveLogsInStorage("Container Push error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorMessage, errorMessage, 0, null, null, null);
                Communications.UpdateCommunicationLogStatus(ComId, tenant, null, ComStatusCode, "Exception occured while adding adding status " + DateTime.Now.ToString(), errorMessage);
               
            }

        }
        public void AnalyzeContainerStatus(string data)
        {
			int tenant = 0;
			string ComId = "";
			string ComStatusCode = "";
			try
			{
			
				if (!string.IsNullOrEmpty(data))
				{
 					XmlDocument xmldoc = new XmlDocument();
					xmldoc.LoadXml(data);
				 
					XmlNodeList eventList = xmldoc.GetElementsByTagName("event");
					foreach (XmlNode item in eventList)
					{
						foreach (XmlNode item1 in item.ChildNodes)
						{
							if (item1.Name == "code" && item1.InnerText == "999")
							{
								return;
							}
						}
					}

 
					XmlNodeList nodeList = xmldoc.GetElementsByTagName("shipmentsubscription_id");
					string Id = string.Empty;
 					if (nodeList[0] != null)
					{
						Id = nodeList[0].InnerText;
					}

					 
					OceanInsightsRequestQuery query = new OceanInsightsRequestQuery(0);
					var TempRecs = query.GetAllByOceanInsightsId(Id);
					if (TempRecs == null || TempRecs.Count == 0)
					{

						return;
					}
					try
					{
 						int MyTenant = 0;
						if (TempRecs != null && TempRecs.Count > 0)
						{
							MyTenant = TempRecs.FirstOrDefault().Tenant;
						}
						string OIId = string.Empty;
						string ContainerNo = string.Empty;
						XmlNodeList MynodeList = xmldoc.GetElementsByTagName("shipmentsubscription_id");

						if (MynodeList[0] != null)
						{
							OIId = MynodeList[0].InnerText;
						}

						XmlNodeList MyContainerList = xmldoc.GetElementsByTagName("container_number");

						if (MyContainerList[0] != null)
						{
							ContainerNo = MyContainerList[0].InnerText;
						}

						OceanInsightsRequestsCountQuery Newquery = new OceanInsightsRequestsCountQuery(0);
						var OIRCount = Newquery.GetSinglePMByOceanInsightsByContainerNoOceanInsigntId(ContainerNo, OIId);
						if (OIRCount == null)
						{

 							string BLNumber = "";
						 
							XmlNodeList blnumbernodeList = xmldoc.GetElementsByTagName("bl_number");
							if (blnumbernodeList != null)
							{
								foreach (XmlNode item in blnumbernodeList)
								{
									BLNumber = item.InnerText;
								}
							}
						 
							var TempRequestsCount = new OceanInsightsRequestsCountPM();
							IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
							OceanInsightsRequestsCountService service = new OceanInsightsRequestsCountService(objectContext, 0);
							TempRequestsCount.ContainerNumber = ContainerNo;
							TempRequestsCount.Tenant = MyTenant;
							TempRequestsCount.OceanInsigntId = OIId;
							TempRequestsCount.BLNumber = BLNumber;
							TempRequestsCount.Type = string.IsNullOrEmpty(TempRequestsCount.BLNumber) ? "c_id" : "m_bl";
							TempRequestsCount.CreateDate = DateTime.Now;
							service.Create(TempRequestsCount);
							OIRCount = TempRequestsCount;
 						}
					}
					catch (Exception ex)
					{

					}
 					foreach (var TempRec in TempRecs)
					{
						if (TempRec.Type == "m_bl")
						{
							XmlNodeList containershipments = xmldoc.GetElementsByTagName("shipment");
							foreach (XmlNode item in containershipments)
							{
								string newId = "";
								string newcontainernumber = "";
								string newblnumber = "";
								string newcarrierscac = "";
                                string mpty_return_actual = "";
                                foreach (XmlNode item1 in item.ChildNodes)
								{
									if (item1.Name == "shipmentsubscription_id")
									{
										newId = item1.InnerText;
									}
									else if (item1.Name == "container_number")
									{
										newcontainernumber = item1.InnerText;
									}
									else if (item1.Name == "bl_number")
									{
										newblnumber = item1.InnerText;
									}
									else if (item1.Name == "carrier_scac")
									{
										newcarrierscac = item1.InnerText;
									}
                                    else if (item1.Name == "mpty_return_actual")
                                    {
                                        mpty_return_actual = item1.InnerText;
                                    }

                                }
						 
								var TempReq = query.GetSinglePMByOceanInsightsId(Id);
                                IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
                                OceanInsightsRequestService service = new OceanInsightsRequestService(objectContext, tenant);

                                if (TempReq != null)
                                {
                                    TempReq.IsClosed = TempReq.System == SystemType.Export && !string.IsNullOrEmpty(mpty_return_actual);
                                    service.Create(TempReq);
                                    UpdateStatus(TempRec, data, (!string.IsNullOrEmpty(TempReq.ContainerNumber) ? TempReq.ContainerNumber : TempReq.BLNumber));
								}
								else
								{
									TempReq = new OceanInsightsRequestPM();
									TempReq.ContainerNumber = newcontainernumber;
									TempReq.SCACCode = newcarrierscac;
									TempReq.Tenant = TempRec.Tenant;
									TempReq.OceanInsigntId = Id;
									TempReq.Type = "BLS";
									TempReq.BLNumber = newblnumber;
									TempReq.FromPushPage = true;
                                 
                                    service.Create(TempReq);
									UpdateStatus(TempReq, data, (!string.IsNullOrEmpty(TempReq.ContainerNumber) ? TempReq.ContainerNumber : TempReq.BLNumber));
								}
								//}
							}
						}
						if (TempRec != null)
						{
							UpdateStatus(TempRec, data, TempRec.ContainerNumber);
						}
					}

				}
			}
			catch (Exception ex)
			{

				string errorMessage = ex.Message;
				if (ex.InnerException != null)
				{

					errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

				}
				if (!string.IsNullOrEmpty(ex.StackTrace))
				{
					errorMessage += Environment.NewLine + ex.StackTrace;
				}
				AzureLog.SaveLogsInStorage("Container Push error  " + Environment.NewLine + errorMessage, "E", DateTime.Now, errorMessage, errorMessage, 0, null, null, null);
				Communications.UpdateCommunicationLogStatus(ComId, tenant, null, ComStatusCode, "Exception occured while adding adding status " + DateTime.Now.ToString(), errorMessage);
				//throw (ex);
			}
		}
        
        private void UpdateStatus(OceanInsightsRequestPM TempRec, string data, string Reference)
        {
			WriteOceanInsightsStatusLog(data, TempRec.OceanInsigntId, TempRec.Tenant);
			using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                IShipmentsContext objectContext = ShipmentsContext.GetContext(TempRec.Tenant);
                OceanInsightsStatusesRepository OceanInsightsStatusesRepository = new OceanInsightsStatusesRepository(objectContext);
                OceanInsightsStatuses OceanInsightsRequest = new OceanInsightsStatuses()
                {
                    Id = IdCounter.GetNumber("OceanInsightsStatuses", TempRec.Tenant),
                    Tenant = TempRec.Tenant,
                    OceanInsightsRequestId = TempRec.Id,
                    CreateDate = DateTime.Now,
					XML = data

				};
                

                int tenant = OceanInsightsRequest.Tenant;
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
                DocumentRepository documentrepository = new DocumentRepository(commonContext);
                ObjectTableRepository objecttableRep = new ObjectTableRepository(tenant);
                ObjectTable objectTable = null;

                objectTable = objecttableRep.GetObjectTableByName("OceanInsightsStatuses", 0, true);
                byte[] DataToWrite = GetBytes(data);// new byte[Request.InputStream.Length];
                List<QueueTask> tasks = new List<QueueTask>();
                tasks.Add(new QueueTask() { Action = "OceanInsights.PushUpdate", Parameters = new List<Logitude.Server.Tools.Parameter>() { new Logitude.Server.Tools.Parameter { Order = 1, Value = data } } });
                var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                Document document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = ByteData.Length,
                    Tenant = Convert.ToInt32(tenant),
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = "ExternalTasksQueue",
                };
                documentrepository.Add(document);
                documentrepository.SubmitChanges();
                var commLog = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    InOut = "O",
                    EntityId = OceanInsightsRequest.Id,
                    ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                    Subject = "Ocean Insights Status",
                    Tenant = tenant,
                    CommunicationLogTypeCode = "Q",
                    CommunicationStatusTypeCode = (tenant == 0 ? "F":"W"),
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    DocumentId = document.Id,
                    CreateDateUTC = DateTime.UtcNow,
                    LastStatusDateUTC = DateTime.UtcNow,
                    QueueName = "externaltasksqueue" + tenant + 1,
                    Priority = 1,
                    AWBNumber = Reference

                };

                communicationLogRepository.Add(commLog);
                communicationLogRepository.SubmitChanges();
                OceanInsightsRequest.ContentDocumentId = document.Id;
                OceanInsightsRequest.CommunicationLogId = commLog.Id;
                OceanInsightsStatusesRepository.Add(OceanInsightsRequest);
                OceanInsightsStatusesRepository.SubmitChanges();
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();


                string filename = document.Id + "." + document.Extension;
                string filePath = "tenant" + commLog.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = ByteData.Length,

                };
                storageservice.Write(ByteData, fileInfo);


                stopwatch.Stop();
                Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("SetBolb:" + filePath + ":Took:" + stopwatch.Elapsed.ToString());


                if (!string.IsNullOrEmpty(commLog.QueueName))
                {
                    try
                    {
                        Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Before adding message to queue " + DateTime.Now.ToString(), null);
                        SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);
                        Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "after adding message to queue " + DateTime.Now.ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = ex.Message;

                        if (!string.IsNullOrEmpty(ex.StackTrace))
                        {
                            errorMessage += Environment.NewLine + ex.StackTrace;
                        }

                        Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, null, commLog.CommunicationStatusTypeCode, "Exception occured while adding message to queue " + DateTime.Now.ToString(), errorMessage);

                    }
                }
                scope.Complete();
            }
        }
		private void WriteOceanInsightsStatusLog(dynamic data,string oceanInsightsRequestId, int tenant)
		{
			try
			{
				OceanInsightsStatusLogPM oceanInsightsStatusLog = new OceanInsightsStatusLogPM();
				IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
				OceanInsightsStatusLogService service = new OceanInsightsStatusLogService(objectContext, tenant);
				oceanInsightsStatusLog.Tenant = tenant;
				oceanInsightsStatusLog.OceanInsigntRequestId = oceanInsightsRequestId;
				oceanInsightsStatusLog.XML = data;

				service.Create(oceanInsightsStatusLog);
			}
			catch(Exception ex)
			{

			}

		}
		private void WriteData(dynamic data)
		{
			try
			{
				string WindWardSettings = LogitudeSettings.WindWardSettings;
				var WindWardSettingsArray = WindWardSettings?.Split(',');
				string IsWriteData = (WindWardSettingsArray != null && WindWardSettingsArray.Count() > 3) ? WindWardSettingsArray[3] : "0";
				if (IsWriteData == "1")
				{
                    if(!string.IsNullOrEmpty(data)) { 
					    XmlDocument xmldoc = new XmlDocument();
					    xmldoc.LoadXml(data);

						XmlNodeList nodeList = xmldoc.GetElementsByTagName("shipmentsubscription_id");
						string id = string.Empty;
						if (nodeList[0] != null)
						{
							id = nodeList[0].InnerText;
						}
						string containerNumber = string.Empty;
						XmlNodeList requestkeynodeList = xmldoc.GetElementsByTagName("container_number");
						foreach (XmlNode item in requestkeynodeList)
						{
							containerNumber = item.InnerText;
						}
						string BLNumber = string.Empty;
						XmlNodeList blnumbernodeList = xmldoc.GetElementsByTagName("bl_number");
						if (blnumbernodeList != null)
						{
							foreach (XmlNode item in blnumbernodeList)
							{
								BLNumber = item.InnerText;
							}
						}


						string filename = string.Format("{0}_{1}_{2}_{3}.json", containerNumber, BLNumber, id, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff"));
                        
                        
					    if (!System.IO.Directory.Exists("c:\\temp\\oceanInsight"))
					    {
					    	System.IO.Directory.CreateDirectory("c:\\temp\\oceanInsight");
					    }
					    System.IO.File.WriteAllText(Path.Combine("c:\\temp\\oceanInsight", filename), JsonConvert.SerializeObject(data));
					}
				}
			}
			catch (Exception ex)
			{

			}
		}
		private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private void WriteError(string data)
        {
            string tempfolder = Server.MapPath(@"~\Temp");
            string filename = System.IO.Path.Combine(tempfolder, "PushEvent_" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".Err.TXT");
            File.WriteAllText(filename, data);
        }

        private bool UploadFileToFtp(string username, string password, string localFilePath)
        {
            string filename = Path.GetFileName(localFilePath);
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(username, password);
                    client.UploadFile("ftp://192.116.221.106/PushEvent/" + filename, "STOR", localFilePath);
                }
                return (true);
            }
            catch (Exception ex)
            {
                WriteError(ex.ToString());
                return (false);
            }
        }

        private string getData(Stream str)
        {
            try
            {
                StreamReader stream = new StreamReader(str);
                string x = stream.ReadToEnd();
                if (string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x))
                    return (string.Empty);
                string jsonData = HttpUtility.UrlDecode(x);
                XmlDocument doc = JsonConvert.DeserializeXmlNode("{\"container\":" + jsonData, "Root");

                string data = System.Xml.Linq.XElement.Parse(doc.OuterXml).ToString();
                return (data);
            }
            catch (Exception ex)
            {
                WriteError("Failed to getData" + Environment.NewLine + ex.ToString());
               throw;
            }
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "Tenant", tenant.ToString() }, { "CommunicationLogId", communicationLogId } }, tenant);

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SendCommunicationLogMessageToQueue", null, null);
            }
        }

        private QueueClient GetQueueClient(string queuename)
        {
            queuename = WebFreightEntryPoint.GetQueueByEnviroment(queuename);

            if (!StorageAcountDetails.NameSpaceManager.QueueExists(queuename))
            {
                QueueDescription queueDescription = new QueueDescription(queuename);
                queueDescription.MaxSizeInMegabytes = 5120;
                queueDescription.MaxDeliveryCount = 99999;
                queueDescription.LockDuration = new TimeSpan(0, 5, 0);


                StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
            }

            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(queuename, ReceiveMode.PeekLock);

            return client;
        }

    }

   
}
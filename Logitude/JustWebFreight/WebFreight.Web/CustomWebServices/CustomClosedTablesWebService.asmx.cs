using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using UnifreightIIG.Common.ClientSdk;
using Logitude.Customs.BL;
using WebFreight.Web.CustomModel;
using Logitude.CustomsMessaging.Helpers;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Reflection;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml.Serialization;
using Logitude.Server.Tools.Helpers;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.QueueService;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for CustomClosedTablesWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CustomClosedTablesWebService : System.Web.Services.WebService
    {

		[WebMethod]
		public void FillCustomClosedTablesData(string closedTableId, bool updateAll, int tenant)
		{
			//LoadCustomClosedTables.FillCustomClosedTablesData();

			var doViaComm = true;
			if (doViaComm)
			{
				if (updateAll)
				{
					LoadCustomClosedTables.UpdateAllClosedTables(tenant);

				}
				else
				{
					SYSTBL_NG_9000_MSG_SystemTableRequestMessageService.SendIt(closedTableId, tenant);
				}
				return;
			}

			Dictionary<string, string> messageProperties = new Dictionary<string, string>();
			BrokeredMessage message = new BrokeredMessage();
			if (updateAll)
			{

				message.Properties["type"] = "all";
				message.Properties["closedtableid"] = null;
				message.Properties["tenant"] = tenant;

				messageProperties.Add("type", "all");
				messageProperties.Add("closedtableid", null);
				messageProperties.Add("tenant", tenant.ToString());
			}
			else
			{
				message.Properties["type"] = "single";
				message.Properties["closedtableid"] = closedTableId;
				message.Properties["tenant"] = tenant;

				messageProperties.Add("type", "single");
				messageProperties.Add("closedtableid", closedTableId);
				messageProperties.Add("tenant", tenant.ToString());
			}

			string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment(SBQueueNames.updateclosedtables.ToString());
			QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);

			client.Send(message);

			DbQueueService queueservice = new DbQueueService("EmailQueue", tenant);
			//IQueueService queueservice = QueueServiceManager.GetQueueService("EmailQueue", tenant);
			queueservice.Send(messageProperties);

		}

       

        [WebMethod]
        public byte[] FillCustomClosedTables(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(SystemTableRequestParams));
            var requestParams = (SystemTableRequestParams)serializer.Deserialize(memorystream);
            var  responseData= new SystemTableResponseData();;;
            if (requestParams.UpdateAllTables)
            {
                try
                {
                    
                    LoadCustomClosedTables.UpdateAllClosedTables(requestParams.Tenant, requestParams);
                    responseData.Succeeded = true;
                }
                catch (Exception e)
                {
                    responseData.Succeeded = false; ;
                    responseData.HasException = true;
                    responseData.UserMessage = e.ToString();
                }
            }
            else
            {
                var messagingService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
                responseData = messagingService.Send(requestParams);
            }

            
            

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(responseData.GetType());
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }


        [WebMethod]
        public byte[] FillNotExistedClosedTables(string tableId, string tableName,int tenant)
        {


            var myMehesSystemTables = new SystemTables();
            //var tenant = AuthenticationUtil.GetThreadCurrentPrincipalTenant();
            //if (!tenant.HasValue)
            //{
            //    throw new Exception("Tenant is not available");
            //}
            var entitySystemTables = myMehesSystemTables.GetTableData(tableId, tenant);
            object[] GetAllParamArray = { };

            CustomClosedTableList addedEntitiesList = new CustomClosedTableList() { ClosedTableData = new List<CustomClosedTableData>() };
            CustomClosedTableData data = null;
            foreach (var systemrecord in entitySystemTables)
            {
                bool recordExists = false;
                object existedRecord = null;
         
                foreach (object o in addedEntitiesList.ClosedTableData)
                {
                    Type objectType = o.GetType();
                    PropertyInfo codeInfo = objectType.GetProperty("Code");
                    string code = codeInfo.GetValue(o).ToString();
                    if (systemrecord.id == code)
                    {
                        recordExists = true;
                        break;
                    }
                }

                if (!recordExists)
                {
                  data = new CustomClosedTableData();
                  Type newPocoType = data.GetType();

                        PropertyInfo codeInfo = newPocoType.GetProperty("Code");
                        PropertyInfo localNameInfo = newPocoType.GetProperty("LocalName");
                        PropertyInfo searchFieldsInfo = newPocoType.GetProperty("SearchFields");

                        codeInfo.SetValue(data, systemrecord.id);
                        localNameInfo.SetValue(data, systemrecord.name);
                        searchFieldsInfo.SetValue(data, systemrecord.id + "," + systemrecord.name);


                        object[] addParams = { data };

                        addedEntitiesList.ClosedTableData.Add(data);
                 
                }
                else
                {
                    if (existedRecord != null)
                    {
                        Type exitedRecordType = existedRecord.GetType();
                        PropertyInfo localNameInfo = exitedRecordType.GetProperty("LocalName");
                        PropertyInfo searchFieldsInfo = exitedRecordType.GetProperty("SearchFields");

                        localNameInfo.SetValue(existedRecord, systemrecord.name);
                        searchFieldsInfo.SetValue(existedRecord, systemrecord.id + "," + systemrecord.name);

                    
                  
                        object[] addParams = { existedRecord };
                      
                    }

                }
            }


            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CustomClosedTableList));
            ser.Serialize(memstream, addedEntitiesList);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;


            
        }



    }

    
        

    
    }


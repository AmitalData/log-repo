using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Infrastructure.SystemTable;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.UServer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unifreight.BL.EntityPMs;
using Simplog.Server.Infrastructure;
using Unifreight.Data.AmitalModel;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Def.Messaging.Customs;
using Simplog.Data.CommonDataModel.Repositories;
namespace Logitude.Customs.BL.Messaging.Amital
{
    public class SendMessageToUServerUtil
    {
        public static string SendMessageToUServer(int tenant, string urouterRequest ,out string P_MESSAGE, out string UnifreightTester, CommunicationsParams communicationParam = null)
        {
				//implement the code to send the xml file to amital;

				string P_MOREPARAMS = "";
            string P_XML_DATA = "";
            UnifreightTester =P_MESSAGE = "";
            if (String.IsNullOrWhiteSpace(urouterRequest))
            {
                throw new ArgumentNullException("SendFileToAmitalService():xmlfile is null");
            }

	 
			var myUServerDNS = "UNIV55";// setting.UServerDNS;
            var myUServerPort = "8055";// setting.UServerPort;


            var mySetting = Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(tenant);
	 
		
			if (!mySetting.IsConnectedToUniFreight)
			{
				P_MESSAGE = OpenUnifreighTask(tenant, urouterRequest, "Urouter", mySetting.IsConnectedToUniFreight, communicationParam);	
				return null;
			}
			if (String.IsNullOrWhiteSpace(mySetting.UServerServiceAddress))
            {
                throw new Exception(
                    string.Format("SendFileToAmitalService():Tenant {0} Is Connected To UniFreight but mySetting.UServerServiceAddress is null ", tenant));
            }




            var urouter = @"http://univ55:8055/";
            urouter = mySetting.UServerServiceAddress;

            Uri uriAddress = new Uri(urouter);
            myUServerDNS = uriAddress.DnsSafeHost;
            myUServerPort = uriAddress.Port.ToString();

            var PathAndQuery = uriAddress.PathAndQuery;
            string serviceAddressWithout_gwsfinsrvexe = null;
            if (!String.IsNullOrWhiteSpace(PathAndQuery))
            {
                var list = PathAndQuery.Split(new char[] { @"/"[0] }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (list.Count > 0)
                {
                    if (list.Last().ToLower() == "gwsfinsrvexe")
                    {
                        list.RemoveAt(list.Count - 1);
                    }


                    serviceAddressWithout_gwsfinsrvexe = string.Join("/", list);
                }
            }


            var myUServerUtil = new UServerUtil(myUServerDNS, myUServerPort, serviceAddressWithout_gwsfinsrvexe, 360);
            var stopwatch = Stopwatch.StartNew();
            try
            {
                myUServerUtil.DoIt(urouterRequest, ref P_MOREPARAMS, out P_XML_DATA, out P_MESSAGE);
                UnifreightTester = myUServerUtil.UnifreightTester;
            }
            catch (Exception e)
            {
                //BAD !!!! m_service.Url ="http://10.10.10.117:8080/V591/SERVICES//gwsfinsrvexe"
                //Good !! m_service.Url = "http://10.10.10.117:8080/V591/services/gwsfinsrvexe"

                e.ChangeExceptionMessage("Send Request to UROUTER Failed,");
                throw e;
            }

            LogMessagingUtil.Instance.AppendLine("UrouterRequest:Took:" + stopwatch.Elapsed.ToString());
            return P_XML_DATA;
        }
		private static string OpenUnifreighTask(int tenant, string urouterRequest,string taskType,bool isConnectedToUniFreight, CommunicationsParams communicationParam = null)
        {
                try 
			    {
				    var loggedUserId = AuthenticationUtil.ResolveUserId(tenant, true);
				   
				    CommunicationsParams logParams = new CommunicationsParams()
				    {
				    	Tenant = tenant,
				    	CommunicationLogTypeCode = "Q",
				    	QueueName = "externaltasksqueue" + tenant + 1,
				    	Priority = 1,
				    	InOut = "O",
				    	Status = "W",
				    	LoggingUserId = loggedUserId,
				    	LoggingObjectTableId = communicationParam?.LoggingObjectTableId,
				    	LoggingEntityId = communicationParam?.LoggingEntityId,
				    	Subject = string.IsNullOrEmpty(communicationParam?.Subject)? "Send To URouter" : communicationParam?.Subject,
				    	FolderName = "ExternalTasksQueue",
                        AdditionalFields = communicationParam?.AdditionalFields,
				    };

				    List<QueueTask> queue1Tasks = new List<QueueTask>();
				    queue1Tasks.Add(new QueueTask()
				    {
				    	Action = "GWSFINSRVEXE_DOIT",
				    	Parameters = new List<Parameter>()
				    							 {
				    								new Parameter{ Name = "URouter", Order = 0, Value = urouterRequest }
				    							 }
				    });
				    
				    logParams.ByteData = LogitudeXmlSerializer.SerializeObject(queue1Tasks);
				    Communications.AddCommunicationLog(logParams);
				    return "succeeded";
				}
		        catch(Exception e)
				{
					return "failed" + e?.Message?.ToString();
				}	
			}
		

	}
}

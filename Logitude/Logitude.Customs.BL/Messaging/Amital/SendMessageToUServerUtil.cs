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
        public static string SendMessageToUServer(int tenant, string urouterRequest, out string P_MESSAGE, out string UnifreightTester)
        {
				//implement the code to send the xml file to amital;

				string P_MOREPARAMS = "";
            string P_XML_DATA = "";
            UnifreightTester =P_MESSAGE = "";
            if (String.IsNullOrWhiteSpace(urouterRequest))
            {
                throw new ArgumentNullException("SendFileToAmitalService():xmlfile is null");
            }

			//myParams.Add("GWSFBLO:UnifreightUserID", p_user);
			//var setting = UnifreightIIGCommonUtil.GetTenantSetting(tenant);
			//if (setting == null)
			//{
			//    throw new Exception("SendFileToAmitalService():no setting for tenant");
			//}
			var myUServerDNS = "UNIV55";// setting.UServerDNS;
            var myUServerPort = "8055";// setting.UServerPort;


            var mySetting = Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(tenant);
			//Removed by Yuval Chalup 13.04.2017 (Consulting with Itzik)
			//if (!mySetting.IsConnectedToUniFreight)
			//{
			//    throw new Exception(string.Format("SendFileToAmitalService():Tenant {0} Is not Connected To UniFreight", tenant));
			///}
		
			if (!mySetting.IsConnectedToUniFreight)
			{
				P_MESSAGE = OpenUnifreighTask(tenant, urouterRequest, "Urouter", mySetting.IsConnectedToUniFreight);	
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
		private static string OpenUnifreighTask(int tenant, string urouterRequest,string taskType,bool isConnectedToUniFreight)
		{			
				var sw = Stopwatch.StartNew();
				TransactionScope scope = null;
				if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
				{
					scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
				}
				try
				{
				  AmitalContext _AmitalContext = AmitalContext.GetContext(tenant);
				  
				  
				  string unifreightUser = null;
				  if (RequestSheetContext.Current != null)
				  {
					var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
					if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
					{
						UserRepository userRep = new UserRepository(tenant);
						var user = userRep.GetSingleUser(loggingUserIdFromRS, tenant, true);
						if (user != null)
						{
							if (!String.IsNullOrWhiteSpace(user.Code))
							{
								unifreightUser = user.Code;
							}
						}
					}
				  }
				  if (String.IsNullOrWhiteSpace(unifreightUser))
				  {
				  	unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(tenant);
				  }
			
				
				var myYCULTASKPM = new YCULTASKPM()
					{
						ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
						STATUS = "W",
						REQUESTDATA = urouterRequest,
						ENTNAME = "CFIFILEM",
						PRIMARYNUM = "",
						PRIORITY = YCULTASKPM.calcPriority(taskType),
						TYPE = taskType,
						USRCODE = unifreightUser,
						ARCHIVE = "F",
					};
					if (!isConnectedToUniFreight)
					{
						myYCULTASKPM.Tenant = tenant;
					}
					var myYCULTASKUpdateService = new Unifreight.BL.EntityUpdateServices.YCULTASKUpdateService(_AmitalContext);
					myYCULTASKUpdateService.DontAddTransaction = true;
					myYCULTASKUpdateService.Update(myYCULTASKPM, true);
		
					var myGGGQPM = new GGGQPM()
					{
						ChangeSetOp = ChangeSetOperation.Insert,
						ORIGINQUE = "LGT",
						STATUS = "1",
						EXPTASKTIME = 5,
						EXECDATE = DateTime.Now,
						TRY = 9,
						PRIORITY = 8,
						ENTNAME = "CFIFILEM",
						PRIMARYNUM = "",
						FORMID = "LGT_UPDATE_FCI",
						DEBUG = "F",
						DONEOPERATION = "D",
					};
					if (!isConnectedToUniFreight)
					{
						myGGGQPM.Tenant = tenant;
					}
					var myGGGQUpdateService = new Unifreight.BL.EntityUpdateServices.GGGQUpdateService(_AmitalContext);
					myGGGQUpdateService.DontAddTransaction = true;
					myGGGQUpdateService.Update(myGGGQPM, true);
		
		
					if (scope != null)
					{
						scope.Complete();
					}
					return "succeeded";
				}
		        catch(Exception e)
				{
					return "failed" + e?.Message?.ToString();
				}
				finally
				{
					if (scope != null)
					{
						scope.Dispose();
					}
				}		
			}
		

	}
}

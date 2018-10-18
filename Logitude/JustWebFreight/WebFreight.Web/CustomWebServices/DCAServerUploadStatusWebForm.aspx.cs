using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Dca;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.CustomWebServices
{
    public partial class DCAServerUploadStatusWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var log = new StringBuilder();
            try
            {
                AmitalDebuggerUtil.Break();
                ///<Unique_Environment_ID>ORACLE_1</Unique_Environment_ID>

                //<FileName>GetCD_8347_8348_Web01_02_CurrencyRateSearch_In.IL550221105.IL941079089.2016-02-15_15-01-04.8512ab10-c013-44d2-8aa6-46a243d56b21.PLT.xml</FileName>

                //<PartnerVault>GLO</PartnerVault>

                log.AppendLine("DCAServerUploadStatusWebForm").AppendLine(Request.QueryString.ToString());
                //http://localhost:62619/CustomWebServices/DCAServerUploadStatusWebForm.aspx?Unique_Environment_ID=ORACLE_1&FileName=GetCD_8347_8348_Web01_02_CurrencyRateSearch_In.IL550221105.IL941079089.2016-02-15_15-01-04.8512ab10-c013-44d2-8aa6-46a243d56b21.PLT.xml&PartnerVault=GLO
                //http://192.116.221.103/Oracle/CustomWebServices/DCAServerUploadStatusWebForm.Aspx?FileName=GetCD_8347_8348_Web01_02_CurrencyRateSearch_In.IL550221105.IL941079089.2016-02-24_19-35-21.dc130df0-e96d-4ee7-8d17-fb92bc3b1ac5.PLT.xml&Unique_Environment_ID=GLO-il550221105&PartnerVault=GLO


                var Unique_Environment_ID = Request.QueryString["Unique_Environment_ID"].ToString();
                var PartnerVault = Request.QueryString["PartnerVault"].ToString();
                var dcaFile = Request.QueryString["FileName"].ToString();

                //Unique_Environment_ID = "ORACLE_1";
                //PartnerVault = "GLO";
                //dcaFile = "GetCD_8347_8348_Web01_02_CurrencyRateSearch_In.IL550221105.IL941079089.2016-02-15_15-01-04.8512ab10-c013-44d2-8aa6-46a243d56b21.PLT.xml";
                //var part2 = Unique_Environment_ID.Substring(Unique_Environment_ID.LastIndexOf("_")+1);
                int tenant = 0;//int.Parse(part2);  ;

                var setting = CustomsSettingQueryService.GetSettingByTenant(tenant); ;
                var dcaTenantService = new DcaDownloadTenantService(setting);
                var sb = new StringBuilder();

                sb
                    .Append(@"Unique_Environment_ID=").AppendLine(Unique_Environment_ID)
                    .Append(@"FileName=").AppendLine(dcaFile)
                    .Append(@"PartnerVault=").AppendLine(PartnerVault)
                    .Append(@"RequestDCAServerUploadStatusWebFormAt=").AppendLine(DateTime.Now.ToString());


                var myDCAServerUploadStatus = new Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadStatus()
                {
                    DcaMessage = sb.ToString(),
                    TheDCAServerUploadResponse = new Logitude.CustomsMessaging.Common.DCAParams.DCAServerUploadResponse(),
                    UnifreightQueueOutStatus = "SENT"
                };
                dcaTenantService.DCAServerUploadStatus(dcaFile, myDCAServerUploadStatus);
                log.AppendLine("ok");
                
            }

            catch (Exception ee)
            {
                //log.
                
                ExceptionHandler.HandleException(ee, DateTime.Now, 0, "", "DCAServerUploadStatusWebForm", log.ToString(), null);
                log.AppendLine(ee.ToString());
            }
            finally
            {
                
                Response.Write(log.ToString());
            }
        }
    }
}
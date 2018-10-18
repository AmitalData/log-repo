using Logitude.Customs.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.DCA
{
    class DCAOut
    {
        /*
                OUT
        SaveMN_MSG1170_1171_MANIFESTRequest_In.IL512716317.IL941079089.IL513569772.2014-06-08_07-37-05.20140608073642801190900297793.PRD.xml.zip

        

        MY NUMBER IL512716317
        TO MEHES .IL941079089
        TO Ranar .IL513569772
        .2014-06-08_07-37-05
        CommID .20140608073642801190900297793.PRD.xml.zip


        In 

        Customs Push
        \\GK-UNISVC1\CyberArk_DCA\amital_shipping\Download\ranar\UDCAServerHistory\SendMN_MSG1171_SendManifestFeedBack_Message_Out.IL941079089.2014-06-15_12-46-40-871.a60c718f-3d68-4d15-9b9a-6043dabb7574.PRD.xml.zip


        Return after our Req
        "\\GK-UNISVC1\CyberArk_DCA\amital_shipping\Download\ranar\UDCAServerHistory\SaveMN_MSG1170_1171_MANIFESTRequest_Out.IL941079089.2014-06-15_09-22-58-890.20140615083441612924803021008.PRD.xml.zip"

         */


        //void Sent(IIGMessagePM messageDCA)
        //{
        //    var myDcaManager = new DcaManager(messageDCA) ;

        //    string PartnerVault;
        //    string Unique_Environment_ID;
        //    string ComAmitalID;
        //    string FileContentsBASE64;
        //    string Filename;
        //    string ServerJobID;
        //    string MoreParams = "IIGServer=" + true.ToString(); //RequestFromClientType.IIGSqlServer
        //    string MessageOut;
        //    myDcaManager.SubmitFileOutgoingQueue(PartnerVault, Unique_Environment_ID,
        //    ComAmitalID, FileContentsBASE64, Filename,
        //    out ServerJobID, ref MoreParams, out MessageOut);
        //}

        //public static string CreateBatchFile//<T1>(T1 request, 
        //    (
        //    string p_work_dir, string p_ServiceName,
        //                string p_ExternalId, bool p_taskyam, DCAParams p_EnviorentParams)
        //{
        //    string v_customer_list = "";
        //    v_customer_list = "IL" + p_EnviorentParams.consumerId;
        //    if (p_taskyam) v_customer_list += ".IL" + p_EnviorentParams.mehesCustomerId;
        //    if (p_EnviorentParams.consumerId2 != "") v_customer_list += ".IL" + p_EnviorentParams.consumerId2;
        //    if (p_EnviorentParams.consumerId3 != "") v_customer_list += ".IL" + p_EnviorentParams.consumerId3;
        //    if (p_EnviorentParams.consumerId4 != "") v_customer_list += ".IL" + p_EnviorentParams.consumerId4;
        //    string v_env = p_EnviorentParams.env_id;
        //    //if (System.Environment.UserDomainName == "NTDOMAIN")
        //    //{
        //    //    if (ConfigurationManager.AppSettings["mehesEnvId.Amital"] != null)
        //    //    {
        //    //        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["mehesEnvId.Amital"].ToString()))
        //    //            v_env = ConfigurationManager.AppSettings["mehesEnvId.Amital"].ToString();
        //    //    }
        //    //}
            
        //    string v_filename = "";

        //    v_filename = p_work_dir +
        //        string.Format("{0}{1}.{2}.{3}.{4}.{5}.xml",
        //                     p_ServiceName, "_In",
        //                     v_customer_list,
        //                     DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
        //                     p_ExternalId, v_env /*,
        //                         Guid.NewGuid()*/);


            
        //    return v_filename;
        //}


    }
}

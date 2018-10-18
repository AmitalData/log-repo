using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.BLClient;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.Utils;
using UnifreightIIG.DCA;
using UnifreightIIG.Common.Extensions;
using Logitude.Customs.BL.EntityPMs;
namespace CustomsWorkerRole.DCA
{
    class DcaManager
       : ManagerProxyBase///, IDCAService
    {

        //public DcaManager(IErrorHandler myErrorHandler = null)
        //    : base(myErrorHandler ?? ErrorHandlerUtil.CreateNew())
        //{

        //}
        IIGMessagePM _IIGMessagePM;
        public DcaManager(IIGMessagePM messageDCA = null)
            : base(ErrorHandlerUtil.CreateNew())
        {
            _IIGMessagePM = messageDCA;
        }

        public string GetState()
        {
            var state = "";
            using (var myClient = GetDCAClient()) //new UnifreightSdkDCA())
            {

                //IPhysicalCheck myPC = myUnifreightSdkGateway.Channel;
                //myUnifreightSdkGateway.Channel.PhysicalCheckChangingTime(
                state = myClient.GetChannel<IDCAService>().GetState();
            }
            return state;
        }

        public void GetOutgoingQueueStatus(
            string Unique_Environment_ID, string ComAmitalID, string PartnerVault, string Filename,
            out string UnifreightQueueOutStatus, ref string MoreParams, out string MessageOut)
        {
            UnifreightQueueOutStatus = MessageOut = "";
            string MessageLog = "";


            try
            {


                if (String.IsNullOrWhiteSpace(ComAmitalID))
                {
                    //throw new ArgumentException() 
                    throw new ArgumentNullException("ComAmitalID");
                }

                if (String.IsNullOrWhiteSpace(Unique_Environment_ID))
                {
                    //throw new ArgumentException() 
                    throw new ArgumentNullException("Unique_Environment_ID");
                }


                if (string.IsNullOrWhiteSpace(PartnerVault))
                {
                    throw new ArgumentNullException("PartnerVault");
                }

                if (string.IsNullOrWhiteSpace(Filename))
                {
                    throw new ArgumentNullException("Filename");

                }

                MoreParams = "IIGServer=1";

                using (var myClient = GetDCAClient()) //new UnifreightSdkDCA())
                {


                    myClient.GetChannel<IDCAService>().GetOutgoingQueueStatus(
                        Unique_Environment_ID, ComAmitalID,
                        PartnerVault, Filename,
                        out UnifreightQueueOutStatus,
                        ref MoreParams, out MessageLog);
                }
                Debug.WriteLine("UnifreightQueueOutStatus=" + UnifreightQueueOutStatus);

                MessageOut = MessageLog;

            }
            catch (System.Exception ex)
            {

                MessageLog = _ErrorHandler.ToFormattedMessage(ex);

            }

        }

        public void SubmitFileOutgoingQueue(
            string PartnerVault, string Unique_Environment_ID, string ComAmitalID,
            string FileContentsBASE64, string Filename,
            out string ServerJobID,
            ref string MoreParams, out string MessageOut)
        {
            ServerJobID = MessageOut = "";

            string fileContentsBASE64 = "";

            string MessageLog = "";


            try
            {


                if (String.IsNullOrWhiteSpace(ComAmitalID))
                {
                    //throw new ArgumentException() 
                    throw new ArgumentNullException("ComAmitalID");
                }

                if (String.IsNullOrWhiteSpace(Unique_Environment_ID))
                {
                    //throw new ArgumentException() 
                    throw new ArgumentNullException("Unique_Environment_ID");
                }


                if (string.IsNullOrWhiteSpace(PartnerVault))
                {
                    throw new ArgumentNullException("PartnerVault");
                }

                if (string.IsNullOrWhiteSpace(Filename))
                {
                    throw new ArgumentNullException("Filename");

                }

                MoreParams = "IIGServer=1";

                using (var myClient = GetDCAClient()) //new UnifreightSdkDCA())
                {


                    myClient.GetChannel<IDCAService>().SubmitFileOutgoingQueue(
                        PartnerVault,
                        FileContentsBASE64, Filename,
                        Unique_Environment_ID, ComAmitalID,
                        out ServerJobID,
                        ref MoreParams, out MessageLog);
                }
                Debug.WriteLine("SubmitFileOutgoingQueue():ServerJobID=" + ServerJobID);

                MessageOut = MessageLog;

            }
            catch (System.Exception ex)
            {

                //MessageLog = _ErrorHandler.ExceptionHandler<ArgumentNullException>(ex);
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);

            }

        }

        public List<string> FileListing(string PartnerVault, string Unique_Environment_ID, string SearchPattern, string appendToDownloadFolderName ,ref string MoreParams, out bool ErrorOccurred, out string MessageLog)
        {
            MessageLog = "";
            ErrorOccurred = true;
            string sErrorOccurred = "";
            string listFileInLines = "";
            try
            {

                if (String.IsNullOrWhiteSpace(Unique_Environment_ID))
                {
                    //throw new ArgumentException() 
                    throw new ArgumentNullException("Unique_Environment_ID");
                }


                if (string.IsNullOrEmpty(PartnerVault))
                {
                    throw new ArgumentNullException("PartnerVault");
                }

                if (string.IsNullOrEmpty(SearchPattern))
                {
                    throw new ArgumentNullException("SearchPattern");

                }

                if (SearchPattern.Equals("*"))
                {
                    throw new ArgumentException("SearchPattern must be different than *");

                }

                var state = "";
                using (var myClient = GetDCAClient()
                    //new UnifreightSdkDCA()

                    )
                {

                    //IPhysicalCheck myPC = myUnifreightSdkGateway.Channel;
                    //myUnifreightSdkGateway.Channel.PhysicalCheckChangingTime(
                    myClient.GetChannel<IDCAServiceLogitude>().FileListingLogitude(PartnerVault, Unique_Environment_ID, SearchPattern,
                      appendToDownloadFolderName, out listFileInLines, out sErrorOccurred, ref MoreParams, out MessageLog);
                }
                if (sErrorOccurred.ToBoolAmitalFormart())
                {
                    ErrorOccurred = true;
                    return null;
                }
                var myList = listFileInLines
                    .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
                Debug.WriteLine("listFileInLines=" + myList.Count());
                ErrorOccurred = false;
                return myList;
            }
            catch (System.Exception ex)
            {
                ErrorOccurred = true;
                //MessageLog += _ErrorHandler.ExceptionHandler<ArgumentNullException>(ex);
                MessageLog += _ErrorHandler.ToFormattedMessage(ex);
                return null;
            }



        }

        private ServiceWrapper<IDCAService> GetDCAClient()
        {

            return GenericFluentFactory<UnifreightIIG.DCA.UnifreightDCAFactory>
                                .Init(new UnifreightIIG.DCA.UnifreightDCAFactory())
                 .AddPropertyValue(x => x.UnifreightDCAServiceAddress, _IIGMessagePM.DCAServiceAddress)
                 .AddPropertyValue(x => x.UnifreightDCAServiceTimeout, 61)
                 .Get().CreateDCAClient();
        }

        public string GetContentsBASE64OfDownloadIncomeFile(
            string PartnerVault, string Unique_Environment_ID,
            //string SearchPattern,out 
            string Filename,
            //out string FileContentsBASE64, 
            string appendToDownloadFolderName ,
            ref string MoreParams,
            out bool errorOccurred, out string MessageLog)
        {

            string sErrorOccurred = "";
            string fileContentsBASE64 = "";
            string searchPattern;
            MessageLog = "";
            errorOccurred = true;

            try
            {

                if (String.IsNullOrWhiteSpace(Unique_Environment_ID))
                {
                    //throw new ArgumentException() 
                    throw new ArgumentNullException("Unique_Environment_ID");
                }


                if (string.IsNullOrWhiteSpace(PartnerVault))
                {
                    throw new ArgumentNullException("PartnerVault");
                }

                if (string.IsNullOrWhiteSpace(Filename))
                {
                    throw new ArgumentNullException("Filename");

                }
                if (Filename.IndexOf("*") >= 0)
                {
                    throw new ArgumentException("Bad file name");
                }
                searchPattern = Filename;
                using (var myClient = GetDCAClient()) // GetDCAClient()) //new UnifreightSdkDCA())
                {

                    //IPhysicalCheck myPC = myUnifreightSdkGateway.Channel;
                    //myUnifreightSdkGateway.Channel.PhysicalCheckChangingTime(
                    myClient.GetChannel<IDCAServiceLogitude>()
                        .DownloadIncomeFileLogitude(PartnerVault, Unique_Environment_ID, searchPattern, appendToDownloadFolderName,true,
                         out Filename, out fileContentsBASE64,
                        out sErrorOccurred, ref MoreParams, out MessageLog);
                }
                if (!sErrorOccurred.ToBoolAmitalFormart())
                {
                    if (!searchPattern.Equals(Filename, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception("SearchPattern <> Filename");
                    }

                }
                else
                {
                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
                }
                Debug.WriteLine("fileContentsBASE64.Length=" + fileContentsBASE64.Length);
                errorOccurred = false;
                return fileContentsBASE64;
            }
            catch (System.Exception ex)
            {
                errorOccurred = true;
                //MessageLog = _ErrorHandler.ExceptionHandler<ArgumentNullException>(ex);
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);
                return null;
            }


        }

        public void DeleteIncomeFile(
            //(string PartnerVault, string Unique_Environment_ID, string FileName, out string ErrorOccurred, ref string MoreParams, out string MessageOut)
            string PartnerVault, string Unique_Environment_ID,
            //string SearchPattern,out 
            string Filename,
            //out string FileContentsBASE64, 
            string appendToDownloadFolderName,
            ref string MoreParams,
            out bool errorOccurred, out string MessageLog)
        {

            string sErrorOccurred = "";
            string fileContentsBASE64 = "";

            MessageLog = "";
            errorOccurred = true;

            try
            {

                if (String.IsNullOrWhiteSpace(Unique_Environment_ID))
                {
                    //throw new ArgumentException() 
                    throw new ArgumentNullException("Unique_Environment_ID");
                }


                if (string.IsNullOrWhiteSpace(PartnerVault))
                {
                    throw new ArgumentNullException("PartnerVault");
                }

                if (string.IsNullOrWhiteSpace(Filename))
                {
                    throw new ArgumentNullException("Filename");

                }
                if (Filename.IndexOf("*") >= 0)
                {
                    throw new ArgumentException("Bad file name");
                }

                using (var myClient = GetDCAClient()) //new UnifreightSdkDCA())
                {

                    //IPhysicalCheck myPC = myUnifreightSdkGateway.Channel;
                    //myUnifreightSdkGateway.Channel.PhysicalCheckChangingTime(
                    myClient.GetChannel<IDCAServiceLogitude>()
                        .DeleteIncomeFileLogitude(PartnerVault, Unique_Environment_ID, Filename,appendToDownloadFolderName,
                        out sErrorOccurred,
                        ref MoreParams, out MessageLog);
                }
                Debug.WriteLine("DeleteIncomeFile=" + (!sErrorOccurred.ToBoolAmitalFormart()).ToString());
                if (sErrorOccurred.ToBoolAmitalFormart())
                {
                    throw new FaultException<UnifreightIIGFault>(UnifreightIIGFault.GetUnifreightValidation(MessageLog));
                }
                errorOccurred = false;

            }
            catch (System.Exception ex)
            {
                errorOccurred = true;
                //MessageLog = _ErrorHandler.ExceptionHandler<ArgumentNullException>(ex);
                MessageLog = _ErrorHandler.ToFormattedMessage(ex);

            }


        }

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

        public static string GetDCAOutFileName//<T1>(T1 request, 
         (
         //string p_work_dir, 
            string p_ServiceName,string p_ExternalId, bool p_taskyam, DCAParams p_EnviorentParams)
        {
            string v_customer_list = "";
            v_customer_list = "IL" + p_EnviorentParams.consumerId;
            if (p_taskyam) v_customer_list += ".IL" + p_EnviorentParams.mehesCustomerId;
            if (p_EnviorentParams.consumerId2 != "") v_customer_list += ".IL" + p_EnviorentParams.consumerId2;
            if (p_EnviorentParams.consumerId3 != "") v_customer_list += ".IL" + p_EnviorentParams.consumerId3;
            if (p_EnviorentParams.consumerId4 != "") v_customer_list += ".IL" + p_EnviorentParams.consumerId4;
            string v_env = p_EnviorentParams.env_id;
            //if (System.Environment.UserDomainName == "NTDOMAIN")
            //{
            //    if (ConfigurationManager.AppSettings["mehesEnvId.Amital"] != null)
            //    {
            //        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["mehesEnvId.Amital"].ToString()))
            //            v_env = ConfigurationManager.AppSettings["mehesEnvId.Amital"].ToString();
            //    }
            //}

            string v_filename = "";

            v_filename = //p_work_dir +
                string.Format("{0}{1}.{2}.{3}.{4}.{5}.xml",
                             p_ServiceName, "_In",
                             v_customer_list,
                             DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"),
                             p_ExternalId, v_env /*,
                                 Guid.NewGuid()*/);



            return v_filename;
        }
        void UnifaceRequest(string MessageID, string MessageXML, out string ResponseXML, out string ErrorOccurred, ref string MoreParams, out string ErrorMess)
        {
            throw new NotImplementedException();
        }
    }
}

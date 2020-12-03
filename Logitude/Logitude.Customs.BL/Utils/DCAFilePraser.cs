using Logitude.Customs.Def.ClosedTable;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Utils
{


    public static class DCAFilePraser
    {

        public static DCAFileModel GetDCAFileModel(string SelectedFileDownload)
        {
            DCAFileModel myDCAFileModel = new DCAFileModel()
            {
                ParsedSuccessfully = false,
                SelectedFileDownload = SelectedFileDownload
            };
            try
            {

                /*
            In 

            Customs Push
            \\GK-UNISVC1\CyberArk_DCA\amital_shipping\Download\ranar\UDCAServerHistory\SendMN_MSG1171_SendManifestFeedBack_Message_Out.IL941079089.2014-06-15_12-46-40-871.a60c718f-3d68-4d15-9b9a-6043dabb7574.PRD.xml.zip


            Return after our Req
            "\\GK-UNISVC1\CyberArk_DCA\amital_shipping\Download\ranar\UDCAServerHistory\SaveMN_MSG1170_1171_MANIFESTRequest_Out.IL941079089.2014-06-15_09-22-58-890.20140615083441612924803021008.PRD.xml.zip"

             */

                var extXml = Path.GetExtension(SelectedFileDownload);//
                if (!extXml.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {

                    myDCAFileModel.ErrorMessage = "Not Xml !!";
                    return myDCAFileModel;
                }

                //\\dev2008\CyberArk_DCA\dev64bit_amitestm53\Download\IIG\GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST.xml
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(SelectedFileDownload);//GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST.xml

                var prtCustomsDeploymentStage = Path.GetExtension(fileNameWithoutExtension);///
                myDCAFileModel.MyCustomsDeploymentStage = GetCustomsDeployStage(prtCustomsDeploymentStage);
                if (myDCAFileModel.MyCustomsDeploymentStage == CustomsDeploymentStage.None)
                {

                    myDCAFileModel.ErrorMessage = " MyCustomsDeploymentStage  Is not PLT Or Prod";
                    return myDCAFileModel;
                }

                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);///GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST
                                                                                                      ///GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60
                var extension = Path.GetExtension(fileNameWithoutExtension);
                //.653bc69d-31e4-4e47-b973-28bd2cd8fe60
                extension = extension.Substring(1);//remove dot 
                myDCAFileModel.IsRestored = extension.Equals(
                    Logitude.Customs.BL.Messaging.Customs.DCAUtil.SufixRestored //"MR"
                    , StringComparison.OrdinalIgnoreCase);
                myDCAFileModel.DebugCreateNew = extension.Equals(
                    Logitude.Customs.BL.Messaging.Customs.DCAUtil.SufixDebugCreateNew //"DBGN"
                    , StringComparison.OrdinalIgnoreCase);
                if (myDCAFileModel.IsRestored || myDCAFileModel.DebugCreateNew)
                {
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);
                    extension = Path.GetExtension(fileNameWithoutExtension);
                    //myDCAFileModel.IsRestored = true;
                    extension = extension.Substring(1);//remove dot 
                }
                myDCAFileModel.OurRefExtrenalId = extension;
                // remove OurRefExtrenalId 
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);


                myDCAFileModel.TimStamp = GetTimeFromFileName(Path.GetExtension(fileNameWithoutExtension));

                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);
                var parts = new List<string>(fileNameWithoutExtension.Split('.'));
                parts.RemoveAll(p => String.IsNullOrWhiteSpace(p));
                myDCAFileModel.Prefix = parts[0];
                if (myDCAFileModel.Prefix.EndsWith("_Out", StringComparison.OrdinalIgnoreCase))
                {
                    myDCAFileModel.MyInOutEnum = DCAFileInOutEnum.Out;
                    myDCAFileModel.PrefixWithout_OutOrIn = myDCAFileModel.Prefix.Substring(0, myDCAFileModel.Prefix.Length - 4);
                }
                else if (myDCAFileModel.Prefix.EndsWith("_In", StringComparison.OrdinalIgnoreCase))
                {
                    myDCAFileModel.MyInOutEnum = DCAFileInOutEnum.In;
                    myDCAFileModel.PrefixWithout_OutOrIn = myDCAFileModel.Prefix.Substring(0, myDCAFileModel.Prefix.Length - 3);
                }
                else
                {
                    myDCAFileModel.ErrorMessage = "Prefix not end with OutOrIn";
                    return myDCAFileModel;
                }
                parts.RemoveAt(0);
                myDCAFileModel.Vaults = new List<string>(parts);
                myDCAFileModel.ParsedSuccessfully = true;
            }
            catch (Exception eee)
            {
                myDCAFileModel.ErrorMessage = eee.Message;

                //throw;
            }

            return myDCAFileModel;

        }
        public static DateTime GetTimeFromFileName(string timePartStartWithDot)
        {

            //SendDEPO_MSG5111_DepositForfeitWarningInfoMsg_Out.IL941079089.2015-10-30_23-59-49-652.33bc4b3c-f6ce-4a99-8f3c-a5f2317c4abc.PLT.xml
            //SendDEPO_MSG5111_DepositForfeitWarningInfoMsg_Out.IL941079089
            //.2015-10-30_23-59-49-652
            //.33bc4b3c-f6ce-4a99-8f3c-a5f2317c4abc
            //.PLT
            //.xml

            //2015-10-30_23-59-49-652
            var timePart = timePartStartWithDot.Substring(1);//remove dot 

            var dateTimeParts = timePart.Split('_').ToList();
            var date = dateTimeParts[0].Split('-').Select(p => int.Parse(p)).ToList(); //2015-10-30_
            var time = dateTimeParts[1].Split('-').Select(p => int.Parse(p)).ToList();//23-59-49-652
            var d = new DateTime(date[0], date[1], date[2], time[0], time[1], time[2]);
            if (time.Count == 4)
            {
                d = d.AddMilliseconds(time[3]);
            }
            return d;
        }

        private static CustomsDeploymentStage GetCustomsDeployStage(string prtCustomsDeploymentStage)
        {
            var customsDeploymentStage = CustomsDeploymentStage.None;
            switch (prtCustomsDeploymentStage.ToUpper())
            {
                //case ".TST":
                //    customsDeploymentStage = CustomsDeploymentStage.Test;
                //    break;
                case ".PRE":
                    customsDeploymentStage = CustomsDeploymentStage.PREPROD;
                    break;
                case ".PLT":
                    customsDeploymentStage = CustomsDeploymentStage.Pilot;
                    break;
                case ".PRD":
                    customsDeploymentStage = CustomsDeploymentStage.Production;
                    break;
                //כל הלקוחות ששולחים מסרים לוגיסטיים בכספת תס"ק ים נדרשים לשנות את הסיומת של המסרים לPRD במקום PLT החל משעה זו.
                case ".TST":
                    customsDeploymentStage = CustomsDeploymentStage.Test;
                    break;

                default:
                    //return ".KHL.xml";
                    //    case CustomsDeploymentStage.None:
                    //    return ".KHL.xml";
                    //    break;
                    //case CustomsDeploymentStage.Test:
                    //    return ".TST.xml";
                    customsDeploymentStage = CustomsDeploymentStage.None;
                    break;

            }
            return customsDeploymentStage;
        }
    }
    public enum DCAFileInOutEnum
    {
        In, Out
    }
    public class DCAFileModel
    {
        public string SelectedFileDownload { get; set; }
        public string ErrorMessage { get; set; }



        public bool? ParsedSuccessfully { get; set; }
        public DCAFileInOutEnum MyInOutEnum { get; set; }

        public bool IsRestored { get; set; }
        public string OurRefExtrenalId { get; set; }
        public string Prefix { get; set; }
        public string PrefixWithout_OutOrIn { get; set; }
        public DateTime TimStamp { get; set; }
        public CustomsDeploymentStage MyCustomsDeploymentStage { get; set; }

        public List<String> Vaults { get; set; }



        public bool DebugCreateNew { get; set; }

        public string DownloadLog { get; set; }
    }
}
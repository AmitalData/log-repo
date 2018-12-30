using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class MamanStatusAvailabilitySplitterService : CustomAnalyzerQueueBase
    {

        public MamanStatusAvailabilitySplitterService(InterfaceDetails MyInterfaceDetails)
            :base(MyInterfaceDetails)
        {

        }

        protected override string AnalyzeData(string communicationsData)
        {
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var defInterfaceName_ECSTS_Splited = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.Code == CustomsPartnerFtpDetails.InterfaceName_ECSTS_Splited).First();
            LogMessagingUtil.Instance.AppendLine("MamanStatusAvailabilitySplitterService");
            communicationsData = communicationsData ?? "";
            var list =communicationsData.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in list)
            {
                

                var analyzeQueueUtil = new AnalyzeQueueUtil();
                analyzeQueueUtil
                    .SaveMessageToAnalyzeQueue(this._AnalyzeQueue.FileName, Encoding.UTF8.GetBytes(item), this._AnalyzeQueue.Tenant,defInterfaceName_ECSTS_Splited);

            }
            return "";
        }


 
    }
}

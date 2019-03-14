
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.Messaging.Maman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class MamanQHAWBService : CustomAnalyzerQueueBase
    {
        public MamanQHAWBService(InterfaceDetails MyInterfaceDetails)
            : base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {
                var courierGWMessageECTHRDataMamanResponseService = new CourierGWMessageECTHRDataMamanResponseService();

                var commSetting = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertDeserializeTyped<CourierWEBAPICommSettings>(_CommunicationLog.LogSettings);
                courierGWMessageECTHRDataMamanResponseService.AnalyzeQResponse(commSetting, communicationsData);
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;

            }
            catch (Exception eee)
            {

                res.ErrorMessage = eee.ToString();
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
            }
            return res;
        }
    }
}

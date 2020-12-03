using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging.ILOVS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class CourierOVSHAWBQService : CustomAnalyzerQueueBase
    {
        public CourierOVSHAWBQService(InterfaceDetails MyInterfaceDetails)
            : base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {
                var courierOVSECTHMessageResponseService = new CourierOVSECTHMessageResponseService();

                var commSetting = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertDeserializeTyped<CourierWEBAPICommSettings>(_CommunicationLog.LogSettings);
                courierOVSECTHMessageResponseService.AnalyzeQResponse(commSetting, communicationsData);
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

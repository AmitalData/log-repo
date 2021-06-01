
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging.ILSWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class ILSWSQHAWBService : CustomAnalyzerQueueBase
    {
        public ILSWSQHAWBService(InterfaceDetails MyInterfaceDetails)
            : base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            CourierSWSHAWBResponse mySWSHAWBResponse = GetSWSHAWBResponse(communicationsData);

            var res = new AnalyzeResultModel();
            try
            {
                var courierECSWSTHRMessageResponseService = new CourierECSWSTHRMessageResponseService();
                courierECSWSTHRMessageResponseService.AnalyzeQResponse(_CommunicationLog.Tenant, mySWSHAWBResponse);
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;

            }
            catch (Exception eee)
            {

                res.ErrorMessage = eee.ToString();
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
            }
            return res;
        }
        private static XElement GetXElement(XElement myXElementSTBMessage, string field)
        {

            XElement ele = myXElementSTBMessage.Element(field);
            if (ele == null)
            {
                throw new Exception($"XElement {field} not exist ");
            }

            return ele;
        }

        private static CourierSWSHAWBResponse GetSWSHAWBResponse(string communicationsData)
        {


            var mySWSHAWBResponse = new CourierSWSHAWBResponse();
            var myXElementSWSHAWBResponse = XElement.Parse(communicationsData);

            mySWSHAWBResponse.CourierCompanyVat = (string)GetXElement(myXElementSWSHAWBResponse, "CourierCompanyVat");
            mySWSHAWBResponse.CourierHawbNumber = (string)GetXElement(myXElementSWSHAWBResponse, "CourierHawbNumber");
            mySWSHAWBResponse.StatusCode = (string)GetXElement(myXElementSWSHAWBResponse, "StatusCode");
            mySWSHAWBResponse.ErrorCode = (string)GetXElement(myXElementSWSHAWBResponse, "ErrorCode");
            mySWSHAWBResponse.ErrorDescription = (string)GetXElement(myXElementSWSHAWBResponse, "ErrorDescription");


            return mySWSHAWBResponse;
        }
    }
}

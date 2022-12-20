
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.Messaging.ILSWS;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            var mySWSHAWBResponseList = GetSWSHAWBResponse(communicationsData);

            var res = new AnalyzeResultModel()
            {
                ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
            };
            try
            {
                var courierECSWSTHRMessageResponseService = new CourierECSWSTHRMessageResponseService();
                foreach (var mySWSHAWBResponse in mySWSHAWBResponseList)
                {
                    res = courierECSWSTHRMessageResponseService.AnalyzeQResponse(_CommunicationLog.Tenant, mySWSHAWBResponse, res);
                }

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

        private static List<CourierSWSHAWBResponse> GetSWSHAWBResponse(string communicationsData)
        {

            var listSWSHAWBMessage = new List<CourierSWSHAWBResponse>();
            var myXElementSWSHAWBResponse = XElement.Parse(communicationsData);
            if(myXElementSWSHAWBResponse != null)
            {
                var ArrayOfSWSHAWBResponse = myXElementSWSHAWBResponse.Elements("CourierHawbFeedback");
                if (ArrayOfSWSHAWBResponse != null)
                {
                    foreach (var item in ArrayOfSWSHAWBResponse)
                    {
                        var mySWSHAWBResponse = new CourierSWSHAWBResponse();
                        mySWSHAWBResponse.CourierCompanyVat = (string)GetXElement(item, "CourierCompanyVat");
                        mySWSHAWBResponse.CourierHawbNumber = (string)GetXElement(item, "CourierHawbNumber");
                        mySWSHAWBResponse.StatusCode = (string)GetXElement(item, "StatusCode");
                        mySWSHAWBResponse.ErrorCode = (string)GetXElement(item, "ErrorCode");
                        mySWSHAWBResponse.ErrorDescription = (string)GetXElement(item, "ErrorDescription");
                        listSWSHAWBMessage.Add(mySWSHAWBResponse);
                    }
                }
                else
                {
                    var mySWSHAWBResponse = new CourierSWSHAWBResponse();
                    mySWSHAWBResponse.CourierCompanyVat = (string)GetXElement(myXElementSWSHAWBResponse, "CourierCompanyVat");
                    mySWSHAWBResponse.CourierHawbNumber = (string)GetXElement(myXElementSWSHAWBResponse, "CourierHawbNumber");
                    mySWSHAWBResponse.StatusCode = (string)GetXElement(myXElementSWSHAWBResponse, "StatusCode");
                    mySWSHAWBResponse.ErrorCode = (string)GetXElement(myXElementSWSHAWBResponse, "ErrorCode");
                    mySWSHAWBResponse.ErrorDescription = (string)GetXElement(myXElementSWSHAWBResponse, "ErrorDescription");
                    listSWSHAWBMessage.Add(mySWSHAWBResponse);
                }
            }
            return listSWSHAWBMessage;
        }
    }
}

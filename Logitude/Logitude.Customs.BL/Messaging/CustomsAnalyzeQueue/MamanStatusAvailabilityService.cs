using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class MamanStatusAvailabilityService : CustomAnalyzerQueueBase
    {
        private DeclarationPM _DeclarationPM;

        public MamanStatusAvailabilityService(InterfaceDetails MyInterfaceDetails)
            : base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {
                
                res.ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                LogMessagingUtil.Instance.AppendLine("MamanStatusAvailabilityService");

                STBMessage mySTBMessage = GetSTBMessage(communicationsData);
                if (string.IsNullOrWhiteSpace(mySTBMessage.BaldarAwb))
                {
                    res.ErrorMessage = $"bad communicationsData  mySTBMessage.BaldarAwb is null";
                    res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                    return res;
                }

                //יש לאתר את תיק עמילות לפי מס' ש.מ. בלדר  - BaldarAwb ותאריך שטר מטען בלדר BaldarOpenDate
                string theDecId = "";
                var qs = new DeclarationQueryService(_CommunicationLog.Tenant);
                var idList = qs.GetListByCourierHAWB(mySTBMessage.BaldarAwb, _CommunicationLog.Tenant);
                LogMessagingUtil.Instance.AppendLine($"qs.GetListByCourierHAWB( {mySTBMessage.BaldarAwb} )  == {idList}");
                if (idList.Count == 0)
                {
                    res.ErrorMessage = $"idList.Count ==0  = qs.GetListByCourierHAWB( {mySTBMessage.BaldarAwb} )";
                    res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                    return res;
                }
                else if (idList.Count == 1)
                {
                    theDecId = idList.First();
                }
                else if (idList.Count > 1)
                {

                    if (string.IsNullOrWhiteSpace(mySTBMessage.BaldarOpenDate))
                    {
                        res.ErrorMessage = $"mySTBMessage.BaldarOpenDate is null  unable to choose what to do ??";
                        res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                        res.EntityID = idList.First();
                        return res;
                    }
                    var consignmentQueryService = new ConsignmentQueryService(_CommunicationLog.Tenant);
                    string myDeclarationId = consignmentQueryService.GetDeclarationIdBythirdCargoID(mySTBMessage.BaldarOpenDate, _CommunicationLog.Tenant, idList);
                    if (string.IsNullOrWhiteSpace(myDeclarationId))
                    {
                        res.ErrorMessage = $"myDeclarationId=GetDeclarationIdBythirdCargoID({mySTBMessage.BaldarOpenDate}) is null  unable to choose what to do ??";
                        res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                        res.EntityID = idList.First();

                        return res;
                    }
                    theDecId = myDeclarationId;
                }

                if (string.IsNullOrWhiteSpace(theDecId))
                {
                    res.ErrorMessage = $"theDecId is null  unable to choose what to do ??";
                    res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                    return res;
                }


                var qsDeclarationQueryService = new DeclarationQueryService(_CommunicationLog.Tenant);

                _DeclarationPM = qsDeclarationQueryService.GetSingle(theDecId, true, false);
                res.EntityID = theDecId;
                res.EntityReference = _DeclarationPM.CustomFileNo;

                ContactRepository contactRepository = new ContactRepository(_CommunicationLog.Tenant);
                var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(_CommunicationLog.Tenant), _CommunicationLog.Tenant);
                string loggedContactId = "";
                if (loggedContact != null)
                {
                    loggedContactId = loggedContact.Id;
                }
                var unifreightFUStatusTaskService = new UnifreightFUStatusTaskService();
                switch (mySTBMessage.EventCode)
                {
                    case "0001":
                        {
                            Update0001(theDecId, mySTBMessage.EventQty);
                            unifreightFUStatusTaskService.UpsertFUStatusLE2U(_CommunicationLog.Tenant, loggedContactId, new UnifreightFUStatusParam()
                            {
                                Entname = "CFIFILEM",
                                PrimaryNum = _DeclarationPM.CustomFileNo,
                                Mode = UnifreightEventMode.@new,
                                StatusCode = "SMG",
                            });
                        }
                        break;
                    case "0016":
                        {
                            unifreightFUStatusTaskService.UpsertFUStatusLE2U(_CommunicationLog.Tenant, loggedContactId, new UnifreightFUStatusParam()
                            {
                                Entname = "CFIFILEM",
                                PrimaryNum = _DeclarationPM.CustomFileNo,
                                Mode = UnifreightEventMode.@new,
                                StatusCode = "OMN",
                            });
                        }
                        break;
                    default:
                        {

                            res.ErrorMessage = $"EventCode {mySTBMessage.EventCode} not treated ";
                            LogMessagingUtil.Instance.AppendLine(res.ErrorMessage);
                            res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;
                        }
                        break;
                }


            }
            
            catch (BusinessErrorException ee)
            {
                res.ErrorMessage = ee.ToString();
                res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.D;
            }
            catch (Exception ee)
            {
                throw;
            }
            return res;
        }
        void Update0001(string theDecId, int EventQty)
        {
            string AcceptanceStatusCode = "";
            var totPackageQuantity = _DeclarationPM.Consignments.SelectMany(r => r.ConsignmentPackages).Sum(p => p.PackageQuantity);
            if (EventQty == totPackageQuantity)
            {
                AcceptanceStatusCode = "1";
            }
            else if (EventQty < totPackageQuantity)
            {
                AcceptanceStatusCode = "2";
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("EventQty > totPackageQuantity  ??? >>throw new Exception- Eitan confirm ?!?!? ");
                throw new BusinessErrorException("EventQty > totPackageQuantity  ???");
            }
            _DeclarationPM.AcceptanceStatusCode = AcceptanceStatusCode;
            var customContext = CustomContext.GetContext(_CommunicationLog.Tenant);
            var declarationUpdateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), _CommunicationLog.Tenant);
            declarationUpdateService.Update(_DeclarationPM, true);


            var declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_CommunicationLog.Tenant);
            var currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(theDecId, false, false);

            currentDeclarationCourierStatusPM.CourierPaymentStatusCode = "P";


            var declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), _CommunicationLog.Tenant);
            currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
            declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);



        }


        private static STBMessage GetSTBMessage(string communicationsData)
        {
#if false
<?xml version="1.0"?>
<MamanBaldarSTB xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <STBMessage>
    <BaldarAwb>172663172644102886</BaldarAwb>
    <BaldarHp>511247496</BaldarHp>
    <BaldarOpenDate>111118</BaldarOpenDate>
    <BaldarCode>1651</BaldarCode>
    <EventCode>1234</EventCode>
    <EventTime>2019-01-01T10:14:35.433269+02:00</EventTime>
    <AirlineCode>LY</AirlineCode>
    <Fltno>4444</Fltno>
    <FltDate>2018-11-11T00:00:00</FltDate>
    <LandTime>2018-11-11T12:12:12</LandTime>
    <EventQty>1298</EventQty>
    <Weight>1284.6</Weight>
    <DeclarationId>18041081319590</DeclarationId>
    <HataraTime>2019-01-01T10:14:35.433269+02:00</HataraTime>
    <DestLineCode>1234567890</DestLineCode>
    <DestLineName> ???</DestLineName>
    <DistributorHp>511247496</DistributorHp>
    <DistributorName> עמיטל</DistributorName>
  </STBMessage>
  <STBMessage>
    <BaldarAwb>177553172644104455</BaldarAwb>
    <BaldarHp>511247496</BaldarHp>
    <BaldarOpenDate>241118</BaldarOpenDate>
    <BaldarCode>4020</BaldarCode>
    <EventCode>1234</EventCode>
    <EventTime>2019-01-01T10:14:35.433269+02:00</EventTime>
    <AirlineCode>UA</AirlineCode>
    <Fltno>1122</Fltno>
    <FltDate>2018-11-24T00:00:00</FltDate>
    <LandTime>2018-11-24T12:12:12</LandTime>
    <EventQty>345</EventQty>
    <Weight>56.9</Weight>
    <DeclarationId>18041081319590</DeclarationId>
    <HataraTime>2019-01-01T10:14:35.433269+02:00</HataraTime>
    <DestLineCode>1234567890</DestLineCode>
    <DestLineName> ???</DestLineName>
    <DistributorHp>511247496</DistributorHp>
    <DistributorName> עמיטל</DistributorName>
  </STBMessage>
</MamanBaldarSTB>
#endif

            var mySTBMessage = new STBMessage();
            var myXElementSTBMessage = XElement.Parse(communicationsData);

            mySTBMessage.BaldarAwb = (string)GetXElement(myXElementSTBMessage, "BaldarAwb");//<BaldarAwb>177553172644104455</BaldarAwb>
            mySTBMessage.BaldarOpenDate = (string)GetXElement(myXElementSTBMessage, "BaldarOpenDate");//<BaldarOpenDate>241118</BaldarOpenDate>
            mySTBMessage.EventCode = (string)GetXElement(myXElementSTBMessage, "EventCode");//<EventCode>1234</EventCode>


            mySTBMessage.EventTime = (DateTime)GetXElement(myXElementSTBMessage, "EventTime");//<EventTime>2019-01-01T10:14:35.433269+02:00</EventTime>
            mySTBMessage.EventQty = (int)GetXElement(myXElementSTBMessage, "EventQty");//<EventQty>1298</EventQty>


            return mySTBMessage;
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
    }
    class STBMessage
    {

        public string BaldarAwb { get; internal set; }
        public string BaldarOpenDate { get; internal set; }
        public string EventCode { get; internal set; }
        public DateTime EventTime { get; internal set; }
        public int EventQty { get; internal set; }
    }
}

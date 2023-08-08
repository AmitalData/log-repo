using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
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
    public class MamanStatusAvailabilityQService : CustomAnalyzerQueueBase
    {
        private DeclarationPM _DeclarationPM;

        public MamanStatusAvailabilityQService(InterfaceDetails MyInterfaceDetails)
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
                        //leave to master res.EntityID = idList.First();
                        return res;
                    }
                    var consignmentQueryService = new ConsignmentQueryService(_CommunicationLog.Tenant);
                    string myDeclarationId = consignmentQueryService.GetDeclarationIdBythirdCargoID(mySTBMessage.BaldarOpenDate, _CommunicationLog.Tenant, idList);
                    if (string.IsNullOrWhiteSpace(myDeclarationId))
                    {
                        res.ErrorMessage = $"myDeclarationId=GetDeclarationIdBythirdCargoID({mySTBMessage.BaldarOpenDate}) is null  unable to choose what to do ?? {mySTBMessage.BaldarAwb}";
                        res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                        //leave to master res.EntityID = idList.First();

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

                Boolean updateTerminalReleaseDate = false;
                var qsDeclarationQueryService = new DeclarationQueryService(_CommunicationLog.Tenant);

                _DeclarationPM = qsDeclarationQueryService.GetSingle(theDecId, true, false);
                res.EntityID = theDecId;
                res.EntityReference = _DeclarationPM.CustomFileNo;
                var ownerUnifreightUserService = new OwnerUnifreightUserService();
                string myOwnerUnifreightUserCode = ownerUnifreightUserService.GetOwnerUnifreightUserCode(declaration: _DeclarationPM);

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
                                EventDateTime = mySTBMessage.EventTime,
                                OwnerUnifreightUserCode = myOwnerUnifreightUserCode
                            });
                        }
                        break;
                    case "0006":
                        {
                            unifreightFUStatusTaskService.UpsertFUStatusLE2U(_CommunicationLog.Tenant, loggedContactId, new UnifreightFUStatusParam()
                            {
                                Entname = "CFIFILEM",
                                PrimaryNum = _DeclarationPM.CustomFileNo,
                                Mode = UnifreightEventMode.@new,
                                StatusCode = "OMN",
                                EventDateTime = mySTBMessage.EventTime,
                                OwnerUnifreightUserCode = myOwnerUnifreightUserCode
                            });
                            updateTerminalReleaseDate = true;
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

                UpadteTerminalSuspentionNumber(mySTBMessage, theDecId,updateTerminalReleaseDate);

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

        private void UpadteTerminalSuspentionNumber(STBMessage mySTBMessage, string theDecId,Boolean updateTerminalReleaseDate)
        {
            var customContext = CustomContext.GetContext(_CommunicationLog.Tenant);
            var declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_CommunicationLog.Tenant);
            var currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(theDecId, true, false);
            var declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), _CommunicationLog.Tenant);
            currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
            if (updateTerminalReleaseDate)
            {
                currentDeclarationCourierStatusPM.TerminalReleaseDate = mySTBMessage.EventTime;
            }
            currentDeclarationCourierStatusPM.TerminalSuspentionNumber = mySTBMessage.FormNo;
            declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
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
            _DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            declarationUpdateService.Update(_DeclarationPM, true);

            var declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_CommunicationLog.Tenant);
            var currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(theDecId, true, false);
            LogMessagingUtil.Instance.AppendLine("Declaration Payment Date " + _DeclarationPM.PaymentDate + " Declaration Total Tax " + _DeclarationPM.TotalTax);
            if (_DeclarationPM.PaymentDate.HasValue && (_DeclarationPM.TotalTax == null || _DeclarationPM.TotalTax == 0))//48446//50807
            {
                currentDeclarationCourierStatusPM.CourierPaymentStatusCode = "P";
                var declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), _CommunicationLog.Tenant);
                currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                LogMessagingUtil.Instance.AppendLine("Update Declaration Courier Status CourierPaymentStatusCode=" + currentDeclarationCourierStatusPM.CourierPaymentStatusCode);
            }

        }


        private static STBMessage GetSTBMessage(string communicationsData)
        {
#if false
<STBMessage><BaldarAwb>DSV111111148N</BaldarAwb><BaldarHp>514193408</BaldarHp><OpenBaldarAwbDate>2019-03-03T00:00:00</OpenBaldarAwbDate><BaldarCode>0126</BaldarCode><EventCode>0001</EventCode><EventTime>2019-03-04T15:06:15.18</EventTime><AirlineCode>LY</AirlineCode><FltNo>145</FltNo><FltDate>2019-03-01T00:00:00</FltDate><LandTime xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" /><EventQty>1</EventQty><Weight>1.80</Weight><DeclarationId>19041091143378</DeclarationId><HataraTime xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" /><DestLineCode>9999999999</DestLineCode><DestLineDesc>כללי</DestLineDesc><DistributorHP>123456789</DistributorHP><DistributorName>כללי</DistributorName><FormNo xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" /></STBMessage>
#endif

            var mySTBMessage = new STBMessage();
            var myXElementSTBMessage = XElement.Parse(communicationsData);

            mySTBMessage.BaldarAwb = (string)GetXElement(myXElementSTBMessage, "BaldarAwb");//<BaldarAwb>177553172644104455</BaldarAwb>
            //OpenBaldarAwbDate
            mySTBMessage.BaldarOpenDate = (string)GetXElement(myXElementSTBMessage, "OpenBaldarAwbDate");//<BaldarOpenDate>241118</BaldarOpenDate>
            mySTBMessage.EventCode = (string)GetXElement(myXElementSTBMessage, "EventCode");//<EventCode>1234</EventCode>


            mySTBMessage.EventTime = (DateTime)GetXElement(myXElementSTBMessage, "EventTime");//<EventTime>2019-01-01T10:14:35.433269+02:00</EventTime>
            mySTBMessage.EventQty = (int)GetXElement(myXElementSTBMessage, "EventQty");//<EventQty>1298</EventQty>
            mySTBMessage.FormNo = (string)GetXElement(myXElementSTBMessage, "FormNo");//<EventQty>1298</EventQty>


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
        public string FormNo { get; set; }

    }
    public class MamanStatusAvailabilityTesterService
    {
        public List<KeyValuePair<string, string>> Tester(string AirlinePreFixMAWB)
        {
            var list = new List<KeyValuePair<string, string>>();
            AirlinePreFixMAWB = AirlinePreFixMAWB ?? "";
            var parts = AirlinePreFixMAWB.Split('-').ToList();
            if (parts.Count != 2)
            {
                throw new Exception("AirlineIdMAWB.Split('-').ToList() != 2");
            }
            int tenant = 1;
            var customsAirlineRepo = new CustomsAirlineRepository(tenant);
            var customsAirline = customsAirlineRepo.GetByPrefix(parts[0], tenant);
            if (customsAirline == null)
            {
                throw new Exception($"GetByPrefix({parts[0]}) not in DB");
            }
            var qs = new CourierMasterQueryService(tenant);
            var pm = qs.GetSingleByAirlineAWBs(customsAirline.Id, null, parts[1], tenant);
            if (pm == null)
            {
                throw new Exception($"{AirlinePreFixMAWB} not in DB ");
            }
            var declarationRep = new DeclarationRepository(tenant);
            var declarations = declarationRep.GetCourierConnectedDeclaratins(pm.Id, tenant).ToList();
            int indx = 0;
            foreach (var group50 in declarations
                .Select((rec, index) => new { rec, index })
                .GroupBy(x => x.index / 50))
            {
                string fileName = $"A{parts[1]}_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{indx}.STB";



                var declist= group50.Select(R => R.rec).ToList();
                var XMLData = new XDocument(
                        new XElement("MamanBaldarSTB",

                        (from item in declist
                         select
                             new XElement("STBMessage",
                                        new XElement("BaldarAwb", item.CourierHAWB),
                                    new XElement("BaldarHp", "WHoCare"),
                                    new XElement("BaldarOpenDate", "111118"),
                                    new XElement("BaldarCode", "WHoCare"),
                                    new XElement("EventCode", "0001"),
                                    new XElement("EventTime", "2019-01-01T10:14:35.433269+02:00"),
                                    new XElement("AirlineCode", "WHoCare"),
                                    new XElement("Fltno", "WHoCare"),
                                    new XElement("FltDate", "WHoCare"),
                                    new XElement("LandTime", "WHoCare"),
                                    new XElement("EventQty", "1"),
                                    new XElement("Weight", "WHoCare"),
                                    new XElement("DeclarationId", "WHoCare"),
                                    new XElement("HataraTime", "WHoCare"),
                                    new XElement("DestLineCode", "WHoCare"),
                                    new XElement("DestLineName", "WHoCare"),
                                    new XElement("DistributorHp", "WHoCare"),
                                    new XElement("DistributorName", "WHoCare")
                                    )

                 )));
                
                var xml = XMLData.ToString(SaveOptions.None);
                list.Add(new KeyValuePair<string, string>(fileName, xml));
                indx++;
            }

            return list;

        }
    }
}

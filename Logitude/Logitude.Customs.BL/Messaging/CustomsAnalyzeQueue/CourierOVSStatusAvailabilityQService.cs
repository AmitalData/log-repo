
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
    public class CourierOVSStatusAvailabilityQService : CustomAnalyzerQueueBase
    {
        private DeclarationPM _DeclarationPM;

        public CourierOVSStatusAvailabilityQService(InterfaceDetails MyInterfaceDetails)
            : base(MyInterfaceDetails)
        {

        }

        protected override AnalyzeResultModel AnalyzeData(string communicationsData)
        {
            var res = new AnalyzeResultModel();
            try
            {

                res.ObjectTableID = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                LogMessagingUtil.Instance.AppendLine("CoureirOVSStatusAvailabilityQService");

                CourierHawbStatus mySTBMessage = GetSTBMessage(communicationsData);
                if (string.IsNullOrWhiteSpace(mySTBMessage.CourierHawbNumber))
                {
                    res.ErrorMessage = $"bad communicationsData  mySTBMessage.CourierHawbNumber is null";
                    res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                    return res;
                }

                //יש לאתר את תיק עמילות לפי מס' ש.מ. בלדר  - CourierHawbNumber ותאריך שטר מטען בלדר CourierHawbDate
                string theDecId = "";
                var qs = new DeclarationQueryService(_CommunicationLog.Tenant);
                var idList = qs.GetListByCourierHAWB(mySTBMessage.CourierHawbNumber, _CommunicationLog.Tenant);
                LogMessagingUtil.Instance.AppendLine($"qs.GetListByCourierHAWB( {mySTBMessage.CourierHawbNumber} )  == {idList}");
                if (idList.Count == 0)
                {
                    res.ErrorMessage = $"idList.Count ==0  = qs.GetListByCourierHAWB( {mySTBMessage.CourierHawbNumber} )";
                    res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                    return res;
                }
                else if (idList.Count == 1)
                {
                    theDecId = idList.First();
                }
                else if (idList.Count > 1)
                {

                    if (!mySTBMessage.CourierHawbDate.HasValue)
                    {
                        res.ErrorMessage = $"mySTBMessage.CourierHawbDate is null  unable to choose what to do ??";
                        res.MyCommStatusEnum = Def.ClosedTable.CommStatusEnum.F;
                        //leave to master res.EntityID = idList.First();
                        return res;
                    }
                    var consignmentQueryService = new ConsignmentQueryService(_CommunicationLog.Tenant);
                    string myDeclarationId = consignmentQueryService.GetDeclarationIdBythirdCargoID(mySTBMessage.CourierHawbDate.GetValueOrDefault().ToString("ddMMyy"), _CommunicationLog.Tenant, idList);
                    if (string.IsNullOrWhiteSpace(myDeclarationId))
                    {
                        res.ErrorMessage = $"myDeclarationId=GetDeclarationIdBythirdCargoID({mySTBMessage.CourierHawbDate}) is null  unable to choose what to do ?? {mySTBMessage.CourierHawbNumber}";
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
                switch (mySTBMessage.StatusCode)
                {
                    case "AVA":
                        {
                            UpdateAVA(theDecId, mySTBMessage.PackageQuantity);
                            unifreightFUStatusTaskService.UpsertFUStatusLE2U(_CommunicationLog.Tenant, loggedContactId, new UnifreightFUStatusParam()
                            {
                                Entname = "CFIFILEM",
                                PrimaryNum = _DeclarationPM.CustomFileNo,
                                Mode = UnifreightEventMode.@new,
                                StatusCode = "SMG",
                                EventDateTime = mySTBMessage.StatusDate
                            });
                        }
                        break;
                    case "REL":
                        {
                            unifreightFUStatusTaskService.UpsertFUStatusLE2U(_CommunicationLog.Tenant, loggedContactId, new UnifreightFUStatusParam()
                            {
                                Entname = "CFIFILEM",
                                PrimaryNum = _DeclarationPM.CustomFileNo,
                                Mode = UnifreightEventMode.@new,
                                StatusCode = "OMN",
                                EventDateTime = mySTBMessage.StatusDate
                            });
                        }
                        break;
                    default:
                        {

                            res.ErrorMessage = $"EventCode {mySTBMessage.StatusCode} not treated ";
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
        void UpdateAVA(string theDecId, int EventQty)
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

            if(_DeclarationPM.PaymentDate != null && _DeclarationPM.TotalTax == 0)
            {
                var declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_CommunicationLog.Tenant);
                var currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(theDecId, false, false);

                currentDeclarationCourierStatusPM.CourierPaymentStatusCode = "P";

                var declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), _CommunicationLog.Tenant);
                currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
            }

        }


        private static CourierHawbStatus GetSTBMessage(string communicationsData)
        {


            var mySTBMessage = new CourierHawbStatus();
            var myXElementSTBMessage = XElement.Parse(communicationsData);

            mySTBMessage.CourierHawbNumber = (string)GetXElement(myXElementSTBMessage, "CourierHawbNumber");//<CourierHawbNumber>177553172644104455</CourierHawbNumber>
            mySTBMessage.CourierHawbDate = (DateTime)GetXElement(myXElementSTBMessage, "CourierHawbDate");//<CourierHawbDate>241118</CourierHawbDate>
            mySTBMessage.StatusCode = (string)GetXElement(myXElementSTBMessage, "StatusCode");//<EventCode>1234</EventCode>


            mySTBMessage.StatusDate = (DateTime)GetXElement(myXElementSTBMessage, "StatusDate");//<EventTime>2019-01-01T10:14:35.433269+02:00</EventTime>
            mySTBMessage.PackageQuantity = (int)GetXElement(myXElementSTBMessage, "PackageQuantity");//<EventQty>1298</EventQty>


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
    class CourierHawbStatus
    {

        public string CourierHawbNumber { get; set; }
        public DateTime? CourierHawbDate { get; set; }
        /// <summary>
        /// AVA- זמין
        //REL- יצא מהמסוף
        /// </summary>
        public string StatusCode { get; set; }
        public DateTime StatusDate { get; set; }
        public int PackageQuantity { get; set; }
    }
    
}

using Logitude.AmitalMessaging.Customs.CustomFile.CourierStatus;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.U2L.Courier
{
    public class CourierPendingReasonService : UnifreightGenericService
    {
        private LogitudeCourierStatus _LogitudeCourierXML;
        private DeclarationCourierStatusPM _MyDeclarationCourierStatusPM;
        private ICustomContext _context;
        private Stopwatch _Stopwatch;

        public CourierPendingReasonService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessGenericRequest(string DataIn, ref string MoreParams, out string MessageOut)
        {
            MessageOut = "";
            try
            {
                _Stopwatch = Stopwatch.StartNew();
                MyCommunicationsParams.Subject = "CourierPendingReason ";

                DeserilazeObject(DataIn);
                AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                MyGenericResponseObj.Stage = "GetContext";
                _context = CustomContext.GetContext(ResolvedTenant());
                AppendLogLine("GetContext:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                if (!String.IsNullOrWhiteSpace(_LogitudeCourierXML.DeclarationId))
                {
                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
                    _MyDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_LogitudeCourierXML.DeclarationId, false, false);
                    if (_MyDeclarationCourierStatusPM == null)
                    {
                        throw new BusinessErrorException("Declaration with ID " + _LogitudeCourierXML.DeclarationId + " Doesn't exist");
                    }
                }
                else if (!String.IsNullOrWhiteSpace(_LogitudeCourierXML.CustomFileNo))
                {
                    GetDeclarationPMByCustomsFile(_LogitudeCourierXML.CustomFileNo);
                    if (_MyDeclarationCourierStatusPM == null)
                    {
                        throw new BusinessErrorException("Declaration with Custom File No. " + _LogitudeCourierXML.CustomFileNo + " Doesn't exist");
                    }
                }
                else
                {
                    throw new BusinessErrorException("Declaration ID and Custom File No. is missing");
                }

                //if (_MyDeclarationCourierStatusPM != null && _MyDeclarationCourierStatusPM.CourierPendingReasonCode == "900")
                if (_MyDeclarationCourierStatusPM != null)
                {
                    DeclarationPendingQueryService myCourierPendingReasonQueryService = new DeclarationPendingQueryService(_context);
                    DeclarationPendingPM declarationPendingPM = myCourierPendingReasonQueryService.GetSingle(_MyDeclarationCourierStatusPM.DeclarationId,"900", false, false);
                    if (declarationPendingPM != null && declarationPendingPM.Status == "A")
                    {
                        declarationPendingPM.ChangeSetOp = ChangeSetOperation.Update;
                        declarationPendingPM.Status = "S";
                        DeclarationPendingUpdateService declarationPendingUpdateService = new DeclarationPendingUpdateService(_context, new Dictionary<string, IContext>(), _MyDeclarationCourierStatusPM.Tenant);
                        declarationPendingUpdateService.Update(declarationPendingPM, true);
                    }
                    
                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code 900 as Solved");
                    AppendLogLine("Set Courier Pending Reason Code 900 as Solved" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                }

                AppendLogLine("send request:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.Stage = "Done All ";
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            }
            catch (Exception eeee)
            {
                //SUCCESS = false.ToString();
                //MessageError = eeee.ToString();
            }
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        void DeserilazeObject(string xmlLOGICOURIERSTATUS)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("CourierStatusService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGICOURIERSTATUS))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGICOURIERSTATUS.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGICOURIERSTATUS.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGICOURIERSTATUS);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            LOGICOURIERSTATUS myLOGICOURIERSTATUS = XmlGenericUtil<LOGICOURIERSTATUS>.DeSerializeObject(xmlLOGICOURIERSTATUS);

            if (myLOGICOURIERSTATUS.LogitudeCourierStatus == null || myLOGICOURIERSTATUS.LogitudeCourierStatus.Length != 1)
            {
                throw new BusinessErrorException("_LOGICOURIERSTATUS.CourierStatus.Length != 1");
            }
            this._LogitudeCourierXML = myLOGICOURIERSTATUS.LogitudeCourierStatus[0];
        }

        private void GetDeclarationPMByCustomsFile(string customFileNo)
        {
            if (String.IsNullOrWhiteSpace(customFileNo))
            {
                throw new BusinessErrorException("Custom File No is missing");
            }
            var myQueryService = new DeclarationQueryService(_context);
            string existId = myQueryService.GetIdByCustomFileNo(customFileNo, ResolvedTenant());

            if (String.IsNullOrWhiteSpace(existId))
            {
                throw new BusinessErrorException("LOGITUDE FILE is missing");
            }

            MyGenericResponseObj.Stage = "GetSingle";
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
            _MyDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(existId, false, false);
            if (this._MyDeclarationCourierStatusPM == null)
            {
                throw new BusinessErrorException("LOGITUDE FILE is " + existId + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

        }
    }
}

using Logitude.AmitalMessaging.Customs.CustomFile.CourierStatus;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;


namespace Logitude.Customs.BL.Messaging.U2L.CourierStatus
{
    public class CourierStatusService : UnifreightGenericService
    {
        private LOGICOURIERSTATUS _LOGICOURIERSTATUS;
        private LogitudeCourierStatus _LogitudeCourierStatus;
        private ICustomContext _context;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.CourierStatus.CourierStatusService.Upsert()";
        private DeclarationPM _MyDeclarationPM;
        private Stopwatch _Stopwatch;
        public Boolean forcePersonalSign;
        public Boolean changeDraftDate;

        public CourierStatusService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGICOURIERSTATUS,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();  
            MyCommunicationsParams.Subject = "CourierStatusService ";

            DeserilazeObject(xmlLOGICOURIERSTATUS);
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
            
            CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString());_Stopwatch.Restart(); 
            MyGenericResponseObj.Stage = "GetContext";
            _context = CustomContext.GetContext(ResolvedTenant());
            AppendLogLine("GetContext:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            if (!String.IsNullOrWhiteSpace(_LogitudeCourierStatus.DeclarationId))
            {
                GetDeclarationPM(_LogitudeCourierStatus.DeclarationId);
                if (_MyDeclarationPM == null)
                {
                    throw new BusinessErrorException("Declaration with ID " + _LogitudeCourierStatus.DeclarationId + " Doesn't exist");
                }
            }
            else if (!String.IsNullOrWhiteSpace(_LogitudeCourierStatus.CustomFileNo))
            {
                GetDeclarationPMByCustomsFile(_LogitudeCourierStatus.CustomFileNo);
                if (_MyDeclarationPM == null)
                {
                    throw new BusinessErrorException("Declaration with Custom File No. " + _LogitudeCourierStatus.CustomFileNo + " Doesn't exist");
                }
                
            }
            else
            {
                throw new BusinessErrorException("Declaration ID and Custom File No. is missing");
            }

            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
            DeclarationCourierStatusPM newDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
            if(newDeclarationCourierStatusPM != null)
            {
                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(_context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);

                newDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                newDeclarationCourierStatusPM.LastMileStatusCode = _LogitudeCourierStatus.LastMileStatus;
                newDeclarationCourierStatusPM.LastMileStatusName = _LogitudeCourierStatus.LastMileStatusName;
                newDeclarationCourierStatusPM.LastMileStatusDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeCourierStatus.LastMileStatusDate, "_LogitudeCourierStatus.LastMileStatusDate");
                newDeclarationCourierStatusPM.LastMileStatusRemarks = _LogitudeCourierStatus.LastMileStatusRemarks;
                if (string.IsNullOrWhiteSpace(_LogitudeCourierStatus.Delivered) || (!string.IsNullOrWhiteSpace(_LogitudeCourierStatus.Delivered) && _LogitudeCourierStatus.Delivered.ToLower().Substring(0, 1) != "t"))
                {
                    //newDeclarationCourierStatusPM.Delivered = false;

                }
                else
                {
                    newDeclarationCourierStatusPM.Delivered = true;
                }
                if (string.IsNullOrWhiteSpace(_LogitudeCourierStatus.IsClosedForFollowUp) || (!string.IsNullOrWhiteSpace(_LogitudeCourierStatus.IsClosedForFollowUp) && _LogitudeCourierStatus.IsClosedForFollowUp.ToLower().Substring(0, 1) != "t"))
                {
                    //newDeclarationCourierStatusPM.IsClosedForFollowUp = false;

                }
                else
                {
                    newDeclarationCourierStatusPM.IsClosedForFollowUp = true;
                }
                declarationCourierStatusUpdateService.Update(newDeclarationCourierStatusPM, true);
            }

            AppendLogLine("send request:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "Done All ";
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;

        }

        private void GetDeclarationPM(string logitudeFile)
        {

            if (String.IsNullOrWhiteSpace(logitudeFile))
            {
                throw new BusinessErrorException("LOGITUDE FILE is missing");
            }
            var myQueryService = new DeclarationQueryService(_context);
            
            MyGenericResponseObj.Stage = "GetSingle";
            this._MyDeclarationPM = myQueryService.GetSingle(logitudeFile, true, false);
            if (this._MyDeclarationPM == null)
            {
                throw new BusinessErrorException("LOGITUDE FILE is " + logitudeFile + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            
        }

        private void GetDeclarationPMByCustomsFile(string CustomFileNo)
        {
            if (String.IsNullOrWhiteSpace(CustomFileNo))
            {
                throw new BusinessErrorException("Custom File No is missing");
            }
            var myQueryService = new DeclarationQueryService(_context);
            string existId = myQueryService.GetIdByCustomFileNo(_LogitudeCourierStatus.CustomFileNo, ResolvedTenant());
            
            if (String.IsNullOrWhiteSpace(existId))
            {
                throw new BusinessErrorException("LOGITUDE FILE is missing");
            }
            

            MyGenericResponseObj.Stage = "GetSingle";
            this._MyDeclarationPM = myQueryService.GetSingle(existId, true, false);
            
            if (this._MyDeclarationPM == null)
            {
                throw new BusinessErrorException("LOGITUDE FILE is " + existId + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

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
            this._LOGICOURIERSTATUS = XmlGenericUtil<LOGICOURIERSTATUS>.DeSerializeObject(xmlLOGICOURIERSTATUS);

            if (_LOGICOURIERSTATUS.LogitudeCourierStatus == null || _LOGICOURIERSTATUS.LogitudeCourierStatus.Length != 1)
            {
                throw new BusinessErrorException("_LOGICOURIERSTATUS.CourierStatus.Length != 1");
            }
            this._LogitudeCourierStatus = _LOGICOURIERSTATUS.LogitudeCourierStatus[0];
        }

        private void CheckIntegrity()
        {
           /* MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudeCourierStatus.))
            {
                throw new BusinessErrorException("Request Code is missing");
            }
            AppendLogLine("Request Code = " + this._LogitudeCourierStatus.);
            */
        }

        

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            return "";
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

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }


    }
}


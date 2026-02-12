using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationCourierStatusUpdateService
    {

        private void UpdateUnifreight(DeclarationCourierStatusPM dirtyDeclarationCourierStatusPM)
        {
            if (dirtyDeclarationCourierStatusPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Update &&
                dirtyDeclarationCourierStatusPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                return;
            }

            var context = CustomContext.GetContext(dirtyDeclarationCourierStatusPM.Tenant);
            DeclarationQueryService myDeclarationQueryService = new DeclarationQueryService(context);
            DeclarationPM myDeclarationPM = myDeclarationQueryService.GetSingle(dirtyDeclarationCourierStatusPM.DeclarationId, false, false);
            if (myDeclarationPM == null)
            {
                return;
            }

            if (!myDeclarationPM.IsConnectedToUnifreight)
            {
                return;
            }

            DeclarationCourierStatusPM dbOccDeclarationCourierStatusPM = GetDBEntity(dirtyDeclarationCourierStatusPM);
            CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(context);

            //if ((!string.IsNullOrEmpty(dirtyDeclarationCourierStatusPM.CourierPendingReasonCode) && dirtyDeclarationCourierStatusPM.CourierPendingReasonCode != dbOccDeclarationCourierStatusPM.CourierPendingReasonCode)
            //   || (!string.IsNullOrEmpty(dirtyDeclarationCourierStatusPM.PendingRemarks) && dirtyDeclarationCourierStatusPM.PendingRemarks != dbOccDeclarationCourierStatusPM.PendingRemarks))
            if ((!string.IsNullOrEmpty(dirtyDeclarationCourierStatusPM.CourierPendingReasonList) && dirtyDeclarationCourierStatusPM.CourierPendingReasonList != dbOccDeclarationCourierStatusPM.CourierPendingReasonList))
            {
                string[] courierPendingReasonList = dirtyDeclarationCourierStatusPM.CourierPendingReasonList.Split(',').Select(sValue => sValue.Trim()).ToArray();
                string[] prevCourierPendingReasonList = !string.IsNullOrEmpty(dbOccDeclarationCourierStatusPM.CourierPendingReasonList) ? dbOccDeclarationCourierStatusPM.CourierPendingReasonList.Split(',').Select(sValue => sValue.Trim()).ToArray() : new string[] { };
                //CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle(dirtyDeclarationCourierStatusPM.CourierPendingReasonCode, false, false);
                foreach (var courierPendingReason in courierPendingReasonList)
                {
                    if (!string.IsNullOrWhiteSpace(courierPendingReason) && !prevCourierPendingReasonList.Contains(courierPendingReason))
                    {
                        CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode(courierPendingReason, myDeclarationPM.Tenant);

                        if (courierPendingReasonPM != null && !string.IsNullOrEmpty(courierPendingReasonPM.UnifreightStatusCode))
                        {
                            DeclarationPendingPM declarationPendingPM = new DeclarationPendingPM();
                            declarationPendingPM = dirtyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.CourierPendingReasonCode == courierPendingReason).FirstOrDefault();
                            if (declarationPendingPM != null && (courierPendingReasonPM.RequiresApproval != true || (courierPendingReasonPM.RequiresApproval == true && declarationPendingPM.Approval == true && declarationPendingPM.WasApproved == false)))
                            {
                                //RaiseEventAndStatus(null, courierPendingReasonPM.UnifreightStatusCode, myDeclarationPM, declarationPendingPM.PendingRemarks, true);
                                OpenUnifreighTask(myDeclarationPM, "L2U", courierPendingReasonPM.UnifreightStatusCode, true, "", declarationPendingPM.PendingRemarks);
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(dirtyDeclarationCourierStatusPM.ApprovedCourierPendingList)){
                string[] approvedCourierPendingReasonList = dirtyDeclarationCourierStatusPM.ApprovedCourierPendingList.Split(',').Select(sValue => sValue.Trim()).ToArray();
                foreach (var courierPendingReason in approvedCourierPendingReasonList)
                {
                    if (courierPendingReason != null)
                    {
                        CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode(courierPendingReason, myDeclarationPM.Tenant);

                        if (courierPendingReasonPM != null && !string.IsNullOrEmpty(courierPendingReasonPM.UnifreightStatusCode))
                        {
                            DeclarationPendingPM declarationPendingPM = new DeclarationPendingPM();
                            declarationPendingPM = dirtyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.CourierPendingReasonCode == courierPendingReason).FirstOrDefault();
                            if (declarationPendingPM != null)
                            {
                                //RaiseEventAndStatus(null, courierPendingReasonPM.UnifreightStatusCode, myDeclarationPM, declarationPendingPM.PendingRemarks, true);
                                OpenUnifreighTask(myDeclarationPM, "L2U", courierPendingReasonPM.UnifreightStatusCode, true, "", declarationPendingPM.PendingRemarks);
                            }
                        }
                    }
                }
            }
        }

        private DeclarationCourierStatusPM GetDBEntity(DeclarationCourierStatusPM dirtyDeclarationCourierStatusPM)
        {

            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(dirtyDeclarationCourierStatusPM.Tenant);
            var myDBEntity = declarationCourierStatusQueryService.GetSingle(dirtyDeclarationCourierStatusPM.DeclarationId, true, false);
            return myDBEntity ?? new DeclarationCourierStatusPM();

        }

        public static void RaiseEventAndStatus(string statusId, string unifrieghtStatus, DeclarationPM declarationPM, string remarks, bool isRaiseUnifreightStatus)
        {
            try
            {
                string loggingUserId = "";
                loggingUserId = AuthenticationUtil.ResolveUserId(declarationPM.Tenant);

                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = declarationPM.Tenant,
                    objectTableName = "Customs.Claim",
                    EventCode = statusId,
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = declarationPM.Id.ToString(),
                    EntityId = declarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status from logitude ",
                };
                myAmitalEventTracerModel.MyFUStatus = new AmitalEventTracerModel.FUStatus()
                {
                    entname = "CFIFILEM",
                    primary_number = declarationPM.CustomFileNo,
                    status = "new",
                    xml_status = "new",
                    status_id = unifrieghtStatus,
                    status_DateTime = DateTime.Now,
                    comments = remarks,
                };
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        private AmitalContext _AmitalContext;
        private void OpenUnifreighTask(DeclarationPM dirtyDeclarationPM, string taskType, string status, bool raiseStatus, string xmlStatus, string comment)
        {
            if (dirtyDeclarationPM.Direction != "E")
            {


                var sw = Stopwatch.StartNew();
                TransactionScope scope = null;
                if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
                {
                    scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
                }
                try
                {
                    var requestData = "";
                    AmitalContext _AmitalContext = AmitalContext.GetContext(dirtyDeclarationPM.Tenant);

                    var myCCUQUELOCKQueryService = new Unifreight.BL.EntityQueryServices.CCUQUELOCKQueryService(_AmitalContext);
                    var myCCUQUELOCKUpdateService = new Unifreight.BL.EntityUpdateServices.CCUQUELOCKUpdateService(_AmitalContext);
                    myCCUQUELOCKUpdateService.DontAddTransaction = true;


                    CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", dirtyDeclarationPM.CustomFileNo,dirtyDeclarationPM.Tenant, false);
                    if (myCCUQUELOCK == null)
                    {
                        var myCCUQUELOCKPM = new CCUQUELOCKPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            ENTNAME = "CFIFILEM",
                            FILENO = dirtyDeclarationPM.CustomFileNo,
                        };
                        
                        myCCUQUELOCKPM.Tenant = EntityPM.Tenant;
                        
                        myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
                    }



                    string unifreightUser = null;
                    if (RequestSheetContext.Current != null)
                    {
                        var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                        if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                        {
                            UserRepository userRep = new UserRepository(Tenant);
                            User user = userRep.GetSingleUser(loggingUserIdFromRS, dirtyDeclarationPM.Tenant, true);
                            if (user != null)
                            {
                                if (!String.IsNullOrWhiteSpace(user.Code))
                                {
                                    unifreightUser = user.Code;
                                }
                            }
                        }
                    }
                    if (String.IsNullOrWhiteSpace(unifreightUser))
                    {
                        unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(dirtyDeclarationPM.Tenant);
                    }

                    if (raiseStatus == true)
                    {
                        string loggingUserId = null;
                        {
                            ICommonDataContext dbContext = CommonDataContext.GetContext(dirtyDeclarationPM.Tenant);
                            UserRepository userRepository = new UserRepository(dbContext);
                            var user = userRepository.GetSingleUserByCode("MEHES", dirtyDeclarationPM.Tenant, true);
                            if (user != null)
                            {
                                loggingUserId = user.Id;
                            }
                        }
                        var myDeclarationUpdateService = new UnifrightDeclarationUpdateService(dirtyDeclarationPM, null, loggingUserId);
                        requestData = myDeclarationUpdateService.GetMyFUStatusXML(status, status, comment, xmlStatus, DateTime.Now, true);
                    }

                    var myYCULTASKPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = dirtyDeclarationPM.CustomFileNo,
                        PRIORITY = YCULTASKPM.calcPriority(taskType),
                        TYPE = taskType,
                        USRCODE = unifreightUser,
                        ARCHIVE = "F",
                    };
                    myYCULTASKPM.Tenant = dirtyDeclarationPM.Tenant;
                    
                    var myYCULTASKUpdateService = new Unifreight.BL.EntityUpdateServices.YCULTASKUpdateService(_AmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;
                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                    var myGGGQPM = new GGGQPM()
                    {
                         ChangeSetOp = ChangeSetOperation.Insert,
                         ORIGINQUE = "LGT",
                         STATUS = "1",
                         EXPTASKTIME = 5,
                         EXECDATE = DateTime.Now,
                         TRY = 9,
                         PRIORITY = 8,
                         ENTNAME = "CFIFILEM",
                         PRIMARYNUM = dirtyDeclarationPM.CustomFileNo,
                         FORMID = "LGT_UPDATE_FCI",
                         DEBUG = "F",
                         DONEOPERATION = "D",
                    };
                    
                    myGGGQPM.Tenant = dirtyDeclarationPM.Tenant;
                    
                    var myGGGQUpdateService = new Unifreight.BL.EntityUpdateServices.GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;
                    myGGGQUpdateService.Update(myGGGQPM, true);


                    if (scope != null)
                    {
                        scope.Complete();
                    }
                }

                finally
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                }

                LogMessagingUtil.Instance.AppendLine("OpenUnifreighTask:Took:" + sw.ElapsedMilliseconds);
            }
        }


    }


}

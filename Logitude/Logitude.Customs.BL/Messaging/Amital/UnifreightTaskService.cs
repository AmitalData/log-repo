using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.Def.EntityQueryServicesExt;

namespace Logitude.Customs.BL.Messaging.Amital
{
    public class UnifreightTaskService
    {

        //0 references
        public void OpenUnifreighTask(DeclarationPM dirtyDeclarationPM, string taskType, string status, bool raiseStatus, string xmlStatus)
        {
            var sw = Stopwatch.StartNew();
            TransactionScope scope = null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (var myAmitalContext = AmitalContext.GetContext(dirtyDeclarationPM.Tenant))
                {
                    var myCCUQUELOCKQueryService = new CCUQUELOCKQueryService(myAmitalContext);
                    var myCCUQUELOCKUpdateService = new CCUQUELOCKUpdateService(myAmitalContext);
                    myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var myGGGQUpdateService = new GGGQUpdateService(myAmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(myAmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var requestData = "";
                    var addStatus = ""; // moran 17.9.15 - Task 15458
                    var comment = ""; // moran 20.9.15 - Task 15458
                    var addComment = ""; // moran 20.9.15 - Task 15458

                    CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", dirtyDeclarationPM.CustomFileNo, false);
                    if (myCCUQUELOCK == null)
                    {
                        var myCCUQUELOCKPM = new CCUQUELOCKPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            ENTNAME = "CFIFILEM",
                            FILENO = dirtyDeclarationPM.CustomFileNo,
                        };
                        myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
                    }
                    if (taskType == "LD2U" && dirtyDeclarationPM.IsSignedVersion) // moran 17.9.15 - Task 15458
                    {
                        if (raiseStatus != true)
                        {
                            raiseStatus = true;
                            status = "INP";
                            comment = RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName;
                        }
                        else
                        {
                            addStatus = "INP";
                            addComment = RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName;
                        }
                    }

                    //var unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(dirtyDeclarationPM.Tenant);
                    string unifreightUser = null;
                    if (RequestSheetContext.Current != null)
                    {
                        var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                        if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                        {
                            UserRepository userRep = new UserRepository(dirtyDeclarationPM.Tenant);
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
                        //if (status == "DOK") // moran 20.7.15 - Task 14521 - commented - all statuses for tasks should have user MEHES
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
                        // moran 13.7.15 - Task 14521 -->
                        //requestData = myDeclarationUpdateService.GetMyFUStatusXML(status, status, "", "new", DateTime.Now, true);
                        requestData = myDeclarationUpdateService.GetMyFUStatusXML(status, status, comment, xmlStatus, DateTime.Now, true);
                        // moran 13.7.15 - Task 14521 <--
                        if (!String.IsNullOrWhiteSpace(addStatus)) // moran 17.9.15 - Task 15458
                        {
                            var requestData2 = myDeclarationUpdateService.GetMyFUStatusXML(addStatus, addStatus, addComment, xmlStatus, DateTime.Now, true);
                            requestData = string.Concat(requestData, requestData2);
                        }
                    }
                    else if (!String.IsNullOrWhiteSpace(xmlStatus))
                    {
                        requestData = xmlStatus;
                    }
                        //eitan h 12/3/15 moved to static -->
                        //short priority = 9;
                        //switch (taskType)
                        //{
                        //    case "L2U":
                        //        priority = 1;
                        //        break;
                        //    case "LD2U":
                        //        priority = 2;
                        //        break;
                        //    case "LP2U":
                        //        priority = 3;
                        //        break;
                        //    default:
                        //        break;
                        //}
                        //<--eitan h 12/3/15 moved to static

                        var myYCULTASKPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = dirtyDeclarationPM.CustomFileNo,
                        PRIORITY = YCULTASKPM.calcPriority(taskType),//eitan h 12/3/15 new static operation
                        //PRIORITY = priority,
                        TYPE = taskType,
                        USRCODE = unifreightUser,
                        ARCHIVE = "F", // moran 28.6.16 - AMI-57170
                        //LOGTIME = (new DualQueryService(MainContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now,
                    };
                    //myYCULTASKPM.TASKID = CommCounterUtil.GetUnique30(myYCULTASKPM.LOGTIME);
                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                    var myGGGQPM = new GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", //LugitudeRequest
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
                        //GSTRING1 = myYCULTASKPM.TASKID,
                    };
                    myGGGQUpdateService.Update(myGGGQPM, true);

                    if (scope != null)
                    {
                        scope.Complete();
                    }
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


        public void OpenUnifreighTaskGen(DeclarationPM dirtyDeclarationPM, string entname, string primary, string taskType, string status, bool raiseStatus, string xmlStatus, bool toLock)
        {
            var sw = Stopwatch.StartNew();
            TransactionScope scope = null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (var myAmitalContext = AmitalContext.GetContext(dirtyDeclarationPM.Tenant))
                {

                    var myGGGQUpdateService = new GGGQUpdateService(myAmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(myAmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;
                    var requestData = "";
                    var addStatus = "";
                    var comment = "";
                    var addComment = "";

                    if (toLock)
                    {
                        var myCCUQUELOCKQueryService = new CCUQUELOCKQueryService(myAmitalContext);
                        var myCCUQUELOCKUpdateService = new CCUQUELOCKUpdateService(myAmitalContext);
                        myCCUQUELOCKUpdateService.DontAddTransaction = true;
                        CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", dirtyDeclarationPM.CustomFileNo, false);
                        if (myCCUQUELOCK == null)
                        {
                            var myCCUQUELOCKPM = new CCUQUELOCKPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                ENTNAME = "CFIFILEM",
                                FILENO = dirtyDeclarationPM.CustomFileNo,
                            };
                            myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
                        }
                    }
                    if (taskType == "LD2U" && dirtyDeclarationPM.IsSignedVersion)
                    {
                        if (raiseStatus != true)
                        {
                            raiseStatus = true;
                            status = "INP";
                            comment = RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName;
                        }
                        else
                        {
                            addStatus = "INP";
                            addComment = RequestSheetContext.Current.GetContextOrDefault().SignByX509SubjectName;
                        }
                    }

                    string unifreightUser = null;
                    if (RequestSheetContext.Current != null)
                    {
                        var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                        if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                        {
                            UserRepository userRep = new UserRepository(dirtyDeclarationPM.Tenant);
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
                        if (!String.IsNullOrWhiteSpace(addStatus))
                        {
                            var requestData2 = myDeclarationUpdateService.GetMyFUStatusXML(addStatus, addStatus, addComment, xmlStatus, DateTime.Now, true);
                            requestData = string.Concat(requestData, requestData2);
                        }
                    }

                    if (String.IsNullOrWhiteSpace(requestData) && !String.IsNullOrWhiteSpace(xmlStatus))
                    {
                        requestData = xmlStatus;
                    }

                    var myYCULTASKPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = entname,
                        PRIMARYNUM = primary,
                        PRIORITY = YCULTASKPM.calcPriority(taskType),
                        TYPE = taskType,
                        USRCODE = unifreightUser,
                        ARCHIVE = "F",
                    };
                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                    var myGGGQPM = new GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", //LugitudeRequest
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = DateTime.Now,
                        TRY = 9,
                        PRIORITY = 8,
                        ENTNAME = entname,
                        PRIMARYNUM = primary,
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "D",
                    };
                    myGGGQUpdateService.Update(myGGGQPM, true);

                    if (scope != null)
                    {
                        scope.Complete();
                    }
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

        public class DIUnifreightTaskService : IDIUnifreightTaskService
        {
            public void OpenUnifreighTaskGen(DeclarationPM dirtyDeclarationPM, string entname, string primary, string taskType, string status, bool raiseStatus, string xmlStatus, bool toLock)
            {
                var unifreightTaskService = new UnifreightTaskService();
                unifreightTaskService.OpenUnifreighTaskGen(dirtyDeclarationPM, entname, primary, taskType, status, raiseStatus, xmlStatus, toLock);
            }
        }
    }
}

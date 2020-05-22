using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using System.Data.Common;
using System.Data.SqlClient;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.AmitalMessaging.Infrastructure;

using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;

using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.Customs.BL.Messaging.Amital.CustomFile;
using Logitude.Customs.BL.Models;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Models;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
//using Unifreight.BL.EntityQueryServices;
//using Unifreight.BL.EntityUpdateServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.Customs.Def.Messaging.Customs;
using Simplog.Data.CommonDataModel;
//using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Simplog.Data.Helpers;
using Logitude.Customs.BL.BL;
using UnifreightIIG.Common.ExportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_NG_2751_MSG10000_ExportDeclarationRequestService : RequestServiceBase<DF_NG_2751_MSG10000_ExportDeclaration, GenericRequestParams>
    {
        private ICustomContext _context;
        private DeclarationPM _DeclarationPM;
        private Stopwatch _Stopwatch;
        private AmitalContext _AmitalContext;
        public override void OnRequestFail(GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }

            base.OnRequestFail(requestParams);
        }
        public override void ManipulateRequestParams(GenericRequestParams requestParams)
        {
            if (requestParams.RequestVIA == SendRequestVIA.DCABatch)
            {
                return;
            }
            int countItems = 0;
            int countSI = 0;
            int backgroundcountItems = 0;
            var fast = true;
            var sw = Stopwatch.StartNew();
            bool onlyAlwaysAccumulate = false;
            int existSupplierInvoiceItemsWithoutHash = 0;
            int existSupplierInvoiceItemsWithParent = 0;
            int SItoAccumulate = 0;
            try
            {
                var siqs = new SupplierInvoiceQueryService(requestParams.Tenant);
                SItoAccumulate = siqs.GetSupplierInvoiceToAccumulateCount(requestParams.Tenant, requestParams.AppicationId);
                var ssiqs = new SupplierInvoiceItemQueryService(requestParams.Tenant);
                bool noAccumulateForNow = true;//itzik +ihab 
                if (noAccumulateForNow)
                {
                    countSI = siqs.GetSupplierInvoiceCountForDeclaration(requestParams.AppicationId, requestParams.Tenant);
                    countItems = ssiqs.GetDeclarationCountOfSupplierInvoiceItems(requestParams.Tenant, requestParams.AppicationId, true);
                    LogMessagingUtil.Instance.AppendLine("GetDeclarationCountOfSupplierInvoiceItems: " + countItems.ToString());
                }
                else
                {
                    countItems = siqs.GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(requestParams.Tenant, requestParams.AppicationId);
                    LogMessagingUtil.Instance.AppendLine("GetDeclarationCountOfSupplierInvoiceItemsForAccumulation: " + countItems.ToString());
                }

                backgroundcountItems = countItems;
                existSupplierInvoiceItemsWithParent = ssiqs.ExistSupplierInvoiceItemsWithParent(requestParams.Tenant, requestParams.AppicationId);
                if ((countItems > 100 || SItoAccumulate > 0) && existSupplierInvoiceItemsWithParent > 0)
                {
                    //existSupplierInvoiceItemsWithoutHash = qs.ExistSupplierInvoiceItemsWithoutHash(requestParams.Tenant, requestParams.AppicationId
                    if (countItems < 999 && SItoAccumulate > 0) onlyAlwaysAccumulate = true;
                    existSupplierInvoiceItemsWithoutHash = siqs.ExistSupplierInvoiceItemsWithoutHashForAccumulation(requestParams.Tenant, requestParams.AppicationId, onlyAlwaysAccumulate);
                    if (existSupplierInvoiceItemsWithParent > 0 && existSupplierInvoiceItemsWithoutHash < 1)
                    {
                        if (countItems > 998)
                        {
                            countItems = existSupplierInvoiceItemsWithParent;
                        }
                        if (backgroundcountItems > 100)
                        {
                            backgroundcountItems = existSupplierInvoiceItemsWithParent;
                        }
                    }
                }

#if false
1>                This take All SupplierInvoiceItems  include parent !!!

2>               **maybe** if >998 and all SIItems have  accurate itemHash 
                - Then not need to ReCalcAccumulation & to send InterActive 
                - Ask Yaron 
#endif

            }
            finally
            {
                LogMessagingUtil.Instance.AppendLine("LogitudeSettings.LogitudeURL = " + LogitudeSettings.LogitudeURL);
                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                if (countItems > 998)
                {
                    if (LogitudeSettings.LogitudeURL.Contains("http://192.116.221.103/Oracle"))
                    {
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                    }
                    else
                    {
                        requestParams.RequestVIA = SendRequestVIA.DCABatch;
                    }

                    requestParams.RequestVIAChangeDue = ("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת");
                    LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                    LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת");
                }
                else
                {
                    if (backgroundcountItems >= 100)
                    {
                        if (requestParams.RequestVIA == SendRequestVIA.DCABatch)
                        {
                            LogMessagingUtil.Instance.AppendLine("***User**** Send this request VIA DCABatch-- no need to change !!!");
                            LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 100 פרטי מכס ולכן תשודר ברקע");
                        }
                        else
                        {
                            requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                            LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = SendRequestVIA.WebServiceBatch");
                            LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 100 פרטי מכס ולכן תשודר ברקע");
                            requestParams.RequestVIAChangeDue = ("הצהרה זו מכילה מעל 100 פרטי מכס ולכן תשודר ברקע");
                        }

                    }
                    if (
                        (requestParams.RequestVIA == SendRequestVIA.WebServiceInteractive
                        || requestParams.RequestVIA == SendRequestVIA.Default)
                        && countSI > 15)
                    {
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                        LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = SendRequestVIA.WebServiceBatch");
                        LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 15 חן ספק ולכן תשודר ברקע");
                        requestParams.RequestVIAChangeDue = ("הצהרה זו מכילה מעל 15 חן ספק ולכן תשודר ברקע");


                    }
                    if (SItoAccumulate > 0)
                    {
                        requestParams.RequestVIAChangeDue = ("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר");
                        LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                        LogMessagingUtil.Instance.AppendLine("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר");
                    }
                }

                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:fast=" + fast.ToString() + ":Took:" + sw.ElapsedMilliseconds);
            }
        }

        private void CreateDeclarationPM(GenericRequestParams requestParams)
        {
            if (this._context == null) this._context = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(_context);
            declarationQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
            _DeclarationPM = declarationQueryService.GetSingle(requestParams.AppicationId, true, false);
        }


        public override void PostGetRequest(DF_NG_2751_MSG10000_ExportDeclaration customRequest, GenericRequestParams requestParams)
        {
            if (this._context == null)
            {
                this._context = CustomContext.GetContext(requestParams.Tenant);
            }
            if (this._context != null && _DeclarationPM != null && _DeclarationPM.IsCourierDeclaration)
            {

                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(this._context, new Dictionary<string, IContext>(), _DeclarationPM.Tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(this._context);
                DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_DeclarationPM.Id, true, false);
                if (currentDeclarationCourierStatusPM == null)
                {
                    currentDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                    {
                        DeclarationId = _DeclarationPM.Id,
                        Tenant = _DeclarationPM.Tenant,
                        IsClosedForFollowUp = false,
                        IsCourierMissingClassification = false,
                    };
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                currentDeclarationCourierStatusPM.CourierDeclarationStatusCode = "I";
                declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
            }

            if (_DeclarationPM.IsConnectedToUnifreight)
            {
                OpenUnifreighTask(_DeclarationPM, "L2U", "INR", true, "");
                return;
            }
            ///moran please updat event "INR"
            //string loggingUserId = AuthenticationUtil.ResolveUserId(requestParams.Tenant);
            string loggingUserId = null;
            if (RequestSheetContext.Current != null) loggingUserId = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
            if (string.IsNullOrWhiteSpace(loggingUserId)) loggingUserId = AuthenticationUtil.ResolveUserId(requestParams.Tenant);
            Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.RaiseINREvent(_DeclarationPM, loggingUserId);

        }

        private void OpenUnifreighTask(DeclarationPM dirtyDeclarationPM, string taskType, string status, bool raiseStatus, string xmlStatus)
        {
            var sw = Stopwatch.StartNew();
            TransactionScope scope = null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (_AmitalContext = AmitalContext.GetContext(dirtyDeclarationPM.Tenant))
                {
                    var myCCUQUELOCKQueryService = new Unifreight.BL.EntityQueryServices.CCUQUELOCKQueryService(_AmitalContext);
                    var myCCUQUELOCKUpdateService = new Unifreight.BL.EntityUpdateServices.CCUQUELOCKUpdateService(_AmitalContext);
                    myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var myGGGQUpdateService = new Unifreight.BL.EntityUpdateServices.GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var myYCULTASKUpdateService = new Unifreight.BL.EntityUpdateServices.YCULTASKUpdateService(_AmitalContext);
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

                    var myYCULTASKPM = new Unifreight.BL.EntityPMs.YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = dirtyDeclarationPM.CustomFileNo,
                        PRIORITY = Unifreight.BL.EntityPMs.YCULTASKPM.calcPriority(taskType),
                        TYPE = taskType,
                        USRCODE = unifreightUser,
                        ARCHIVE = "F"
                    };
                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                    var myGGGQPM = new Unifreight.BL.EntityPMs.GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", //LugitudeRequest
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = (new Unifreight.BL.EntityQueryServices.DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
                        TRY = 9,
                        PRIORITY = 8,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = dirtyDeclarationPM.CustomFileNo,
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "A"
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

        public override DF_NG_2751_MSG10000_ExportDeclaration GetRequest(GenericRequestParams requestParams)
        {



            LogMessagingUtil.Instance.AppendLine("GetRequest:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
            LogMessagingUtil.Instance.AppendLine("GetRequest:requestParams.RequestVIAChangeDue = " + requestParams.RequestVIAChangeDue);
            if (((requestParams.RequestVIA == SendRequestVIA.DCABatch || requestParams.RequestVIA == SendRequestVIA.WebServiceBatch) &&
                    requestParams.RequestVIAChangeDue == ("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת")) || requestParams.RequestVIAChangeDue == ("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר"))
            {
                var mySIAccumulationUtil = new SIAccumulationUtil();
                bool onlyAlwaysAccumulate = false;
                mySIAccumulationUtil.SetParam(requestParams);
                if (requestParams.RequestVIAChangeDue == ("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר")) onlyAlwaysAccumulate = true;
                bool isAccurate = mySIAccumulationUtil.Fast_IsAllItemsHaveHash_IsAccurate(onlyAlwaysAccumulate);
                LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Is All Items Have Hash=" + isAccurate.ToString());

                if (!isAccurate)
                {
                    _Stopwatch = Stopwatch.StartNew();
                    using (var tran = TransactionFactory.GetNewTransaction())
                    {
                        mySIAccumulationUtil.FastDeleteAllParent();///maybe need in def trans (must save changes )
                        tran.Complete();
                    }
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Fast Delete All Parent Items Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    using (var tran = TransactionFactory.GetNewTransaction())
                    {
                        mySIAccumulationUtil.Run();
                        this._DeclarationPM = mySIAccumulationUtil.GetDeclarationPM();
                        tran.Complete();
                    }
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "SI Accumulation Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                }

            }
            else
            {
                int countItems = 0;
                int existSupplierInvoiceItemsWithParent = 0;
                var qs = new SupplierInvoiceItemQueryService(requestParams.Tenant);
                //countItems = qs.GetDeclarationCountOfSupplierInvoiceItems(requestParams.Tenant, requestParams.AppicationId);
                var siqs = new SupplierInvoiceQueryService(requestParams.Tenant);
                countItems = siqs.GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(requestParams.Tenant, requestParams.AppicationId);

                existSupplierInvoiceItemsWithParent = qs.ExistSupplierInvoiceItemsWithParent(requestParams.Tenant, requestParams.AppicationId);
                if (existSupplierInvoiceItemsWithParent > 0 && countItems < 999)
                {
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Less than 999 items(" + (countItems - existSupplierInvoiceItemsWithParent) + ") with accumulation - accumulation data will be cleared");
                    _Stopwatch = Stopwatch.StartNew();
                    var mySIAccumulationUtil = new SIAccumulationUtil();
                    mySIAccumulationUtil.SetParam(requestParams);
                    using (var tran = TransactionFactory.GetNewTransaction())
                    {
                        mySIAccumulationUtil.FastDeleteAllParent();
                        tran.Complete();
                    }
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Fast Delete All Parent Items Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                    using (var tran = TransactionFactory.GetNewTransaction())
                    {
                        mySIAccumulationUtil.Run();
                        this._DeclarationPM = mySIAccumulationUtil.GetDeclarationPM();
                        tran.Complete();
                    }
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "SI Accumulation clearance Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    //existSupplierInvoiceItemsWithoutHash = qs.ExistSupplierInvoiceItemsWithoutHash(requestParams.Tenant, requestParams.AppicationId);
                }

                /// due isAccurate
                //queryService.GetOnlyParent();//  work with parent only !!!!!  
            }

            //#endif
            bool fromMevaker = false;
            if (!string.IsNullOrWhiteSpace(requestParams.UnifreightListOnServerOnly))
            {
                var dic = UnifreightListsUtil.Deserialize(requestParams.UnifreightListOnServerOnly);
                fromMevaker = !String.IsNullOrWhiteSpace(UnifreightListsUtil.GetValue(ref dic, "FromMevaker"));
            }
            var req = new DF_NG_2751_MSG10000_ExportDeclaration();
            CreateDeclarationPM(requestParams);
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            if (requestParams.LoggingObjectTableId2 == objectTableIdCourierMaster || fromMevaker)
            {
                if (!fromMevaker)
                {
                  //  UCBatchCheckLock(requestParams, _DeclarationPM);
                }
               // CheckTaxationDateTime(_DeclarationPM);
            }

            LogMessagingUtil.Instance.AppendLine("declaration retrieve from db");

            req.Declaration = null;// Getdeclaration(_DeclarationPM);
            LogMessagingUtil.Instance.AppendLine("declaration build" + requestParams.AppicationId);
            _context = null;

            return req;
        }
    }
}
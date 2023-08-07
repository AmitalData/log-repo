using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
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
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class DF_MSG10000_ImportDeclarationRequestService : RequestServiceBase<DF_MSG10000_ImportDeclaration, GenericRequestParams>
    {
        private ICustomContext _context;
        private DeclarationPM _DeclarationPM;
        private Stopwatch _Stopwatch;
        private AmitalContext _AmitalContext;
        public bool IsFromOpenNewAmendment = false;
        public bool FeatureExcludeManifest = false;
        public bool IsSendWithManifest = false;

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


        public override void PostGetRequest(DF_MSG10000_ImportDeclaration customRequest, GenericRequestParams requestParams)
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
                if(_DeclarationPM.IsCourierDeclaration)
                {
                    FeatureQuery featureQuery = new FeatureQuery();
                    var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);
                    var feature = features.Features.FirstOrDefault(x => x.Code == "SendL2UFromSendDeclaration");
                    if (feature != null)
                        OpenUnifreighTask(_DeclarationPM, "L2U", null, false, "");
                }
                else
                {
                    OpenUnifreighTask(_DeclarationPM, "L2U", "INR", true, "");

                }
                return;
            }
            ///moran please updat event "INR"
            //string loggingUserId = AuthenticationUtil.ResolveUserId(requestParams.Tenant);
            string loggingUserId = null;
            if (RequestSheetContext.Current != null) loggingUserId = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
            if (string.IsNullOrWhiteSpace(loggingUserId)) loggingUserId = AuthenticationUtil.ResolveUserId(requestParams.Tenant);
            if (!_DeclarationPM.IsCourierDeclaration)
                Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.RaiseINREvent(_DeclarationPM, loggingUserId);

        }

        private void OpenUnifreighTask(DeclarationPM dirtyDeclarationPM, string taskType, string status, bool raiseStatus, string xmlStatus)
        {
            var sw = Stopwatch.StartNew();
            TransactionScope scope = null;
            bool isConnectedToUnifreight = CustomsSettingQueryService.GetSettingByTenant(dirtyDeclarationPM.Tenant).IsConnectedToUniFreight;
            AmitalContext _AmitalContext=null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                
                  
                   //we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var requestData = "";
                    var addStatus = ""; // moran 17.9.15 - Task 15458
                    var comment = ""; // moran 20.9.15 - Task 15458
                    var addComment = ""; // moran 20.9.15 - Task 15458
                if (isConnectedToUnifreight) {

                    _AmitalContext = AmitalContext.GetContext(dirtyDeclarationPM.Tenant);
                    var myCCUQUELOCKQueryService = new Unifreight.BL.EntityQueryServices.CCUQUELOCKQueryService(_AmitalContext);
                    var myCCUQUELOCKUpdateService = new Unifreight.BL.EntityUpdateServices.CCUQUELOCKUpdateService(_AmitalContext);
                    myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", dirtyDeclarationPM.CustomFileNo, false);
                    var myGGGQUpdateService = new Unifreight.BL.EntityUpdateServices.GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
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
                        DONEOPERATION = "D"
                    };
                    myGGGQUpdateService.Update(myGGGQPM, true);
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
                if (isConnectedToUnifreight)
                {
                    var myYCULTASKUpdateService = new Unifreight.BL.EntityUpdateServices.YCULTASKUpdateService(_AmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;
                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                }
                else
                {
                    var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                    {

                        Tenant = dirtyDeclarationPM.Tenant,
                        objectTableName = "Customs.Declaration",
                        EventCode = null,
                        notes = "",
                        CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                        EntityId = dirtyDeclarationPM.Id,
                        UserId = dirtyDeclarationPM.CreatedByUserId,

                        CommunicationSubject = "IIG_TASK",

                    };
                    var amitalInsertToQueueService = new AmitalInsertToQueueService< Unifreight.BL.EntityPMs.YCULTASKPM> (myYCULTASKPM);
                    amitalInsertToQueueService.InsertToQueue(myAmitalEventTracerModel, "IIG_TASK");
                }



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

        public override DF_MSG10000_ImportDeclaration GetRequest(GenericRequestParams requestParams)
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
            var req = new DF_MSG10000_ImportDeclaration();
            CreateDeclarationPM(requestParams);

            FeatureQuery featureQuery = new FeatureQuery();
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(requestParams.LoggingUserId, requestParams.Tenant);
            var feature = features.Features.FirstOrDefault(x => x.Code == "ISEXCLUDEMANIFEST");
            if (feature != null)
            {
                FeatureExcludeManifest = true;
            }
            IsSendWithManifest = (!FeatureExcludeManifest || !this._DeclarationPM.ExcludeManifest);

            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            if (requestParams.LoggingObjectTableId2 == objectTableIdCourierMaster || fromMevaker)
            {
                if (!fromMevaker)
                {
                    UCBatchCheckLock(requestParams, _DeclarationPM);
                }
                CheckTaxationDateTime(_DeclarationPM);
            }

            LogMessagingUtil.Instance.AppendLine("declaration retrieve from db");

            var sIModificationByCustomerCommissionService = new SIModificationByCustomerCommissionService();
            foreach (var currSupplierInvoices in _DeclarationPM.SupplierInvoices)
            {
                sIModificationByCustomerCommissionService.EnsureReductionByVendorCommission(_DeclarationPM, currSupplierInvoices, true && !IsFromOpenNewAmendment);

            }

            req.Declaration = Getdeclaration(_DeclarationPM);
            LogMessagingUtil.Instance.AppendLine("declaration build" + requestParams.AppicationId);
            _context = null;

            return req;
        }

        private void UCBatchCheckLock(GenericRequestParams requestParams, DeclarationPM declarationPM)
        {
            //var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            //if (requestParams.LoggingEntityId2 != objectTableIdCourierMaster)
            //{
            //    return;
            //}
            LogMessagingUtil.Instance.AppendLine("CourierMaster Send Batch===> CheckLock");

            long lCUSTOMFILENO;
            if (!long.TryParse(declarationPM.CustomFileNo, out lCUSTOMFILENO))
            {
                throw new BusinessErrorException("_DirtyDeclarationPaymentPM.DeclarationId could not convert to long ");
            }
            var myCCUFILEMRepository = new CCUFILEMRepository(declarationPM.Tenant);
            var ccufilem = myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);


            var myCCUQUELOCKRepository = new CCUQUELOCKRepository(requestParams.Tenant);
            try
            {
                var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());

            }
            catch (System.Exception)
            {

                LogMessagingUtil.Instance.AppendLine($"GetSingleGeneralLockNOWAIT(CCUFILEM, {ccufilem.ToString()}) ==> Already Lock => try later (*5) ");
                throw;
            }


        }

        private void CheckTaxationDateTime(DeclarationPM declarationPM)
        {
            //If TaxationDateTime is not Today change it before sending
            if (!_DeclarationPM.TaxationDateTime.HasValue ||
                (_DeclarationPM.TaxationDateTime.HasValue && _DeclarationPM.TaxationDateTime.Value.Date < DateTime.Now.Date))
            {
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(this._context, new Dictionary<string, IContext>(), _DeclarationPM.Tenant);
                _DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                _DeclarationPM.TaxationDateTime = TenantServerConfigration.GetCurrentDateTime(_DeclarationPM.Tenant);
                declarationUpdateService.Update(_DeclarationPM, true);
            }
        }


        public static byte[] stringToBase64ByteArray(String input)
        {
            byte[] ret = System.Text.Encoding.Unicode.GetBytes(input);
            string s = Convert.ToBase64String(ret);
            ret = System.Text.Encoding.Unicode.GetBytes(s);
            return ret;
        }

        private Attachment[] GetAddAGlobalScannedAttachmentToEntity()
        {
            byte[] myContent = stringToBase64ByteArray("moran test !!!!GetApproveChangeTimeListXML()");
            var requestMessage = new Attachment();
            var Attachments = new Attachment[] {
                new Attachment()
            {

                documentType = "380",
                //"לא התקבלו כל שדות המטה-דטא חובה הבאים: : 3,39,55,87 עבור סוג מסמך : 380"
                //"צרופה לא תקינה סוג המסמך : <NULL> שם :  נתוני שדה נוסף : 3 שגויים - הערך : IL אינו מסוג : Int"
                AdditionalData =new AttachmentAdditionalData[]
                {
                    new  AttachmentAdditionalData (){fieldID = 3,fieldData="US"  } ,//ארץ חשבון 
                    new  AttachmentAdditionalData (){fieldID = 39,fieldData="5520"} ,///מספר חשבון
                    new  AttachmentAdditionalData (){fieldID = 55,fieldData=DataTypeConvertorUtil .Convert(DateTime.Now)  },//תאריך החשבון
                    new  AttachmentAdditionalData (){fieldID = 87,fieldData=false.ToString()  } ,//האם מסמך מקורי
                },
                fileName = "mmmmsdd99000.txt",

                //documentType = "1",
               // attachmentID = "USIGN-1",
                externalAttachmentID = "EMTYC-99000",
                Remark = "mY Remark ",
                content = myContent

            } };
            return Attachments;
        }

        T SetAmountTypeValue<T>(string CurrencyCode, decimal val)
              where T : AmountType, new()
        {

            ISO3AlphaCurrencyCodeContentType isoCurrency;
            var success = Enum.TryParse<ISO3AlphaCurrencyCodeContentType>(CurrencyCode, out isoCurrency);
            ;
            var cur1 = Enum.GetNames(typeof(ISO3AlphaCurrencyCodeContentType)).ToList().FirstOrDefault(cur => cur == CurrencyCode);
            if (cur1 == null)
            {
                ///throw new System.Exception("CurrencyCode is not valid " + CurrencyCode);  
            }

            if (!success)
            {
                ///throw new System.Exception("CurrencyCode is not valid " + CurrencyCode);  
            }
            return new T()
            {
                currencyID = isoCurrency,
                currencyIDSpecified = success,
                Value = val
            };

        }


        T SetQuantityTypeValue<T>(string measurementUnit, decimal val)
              where T : QuantityType, new()
        {
            var measurementUnitRealString = "";
            if (String.IsNullOrWhiteSpace(measurementUnit)) // hard coded
            {
                //measurementUnitRealString = MeasurementUnitCommonCodeContentType.GRO.ToString();
                //measurementUnitRealString = MeasurementUnitCommonCodeContentType.EA.ToString(); // moran 5.1.16 - Task 19549 - commented - send empty
            }
            else
            {
                var list = Enum.GetNames(typeof(MeasurementUnitCommonCodeContentType)).ToList();
                if (list.Exists(unit => unit == measurementUnit))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit);
                }
                else if (list.Exists(unit => unit == measurementUnit.Reverse()))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit.Reverse());
                }
            }

            if (String.IsNullOrWhiteSpace(measurementUnitRealString))
            {
                //return null; // moran 5.1.16 - Task 19549 - commented
            }

            MeasurementUnitCommonCodeContentType measurementCommonUnit;
            var success = Enum.TryParse(measurementUnitRealString, out measurementCommonUnit);

            //var success = Enum.TryParse<MeasurementUnitCommonCodeContentType>(measurementUnit, out measurementCommonUnit);

            if (!success)
            {
                ///throw new System.Exception("measurementUnit is not valid " + measurementUnit);  
            }
            ;

            return new T()
            {
                unitCode = measurementCommonUnit,
                unitCodeSpecified = success,
                Value = val
            };

        }


        T SetMeasureTypeValue<T>(string measurementUnit, decimal val)
              where T : MeasureType, new()
        {

            var measurementUnitRealString = "";
            if (String.IsNullOrWhiteSpace(measurementUnit)) // hard coded
            {
                measurementUnitRealString = MeasurementUnitCommonCodeContentType.KGM.ToString();
            }
            else
            {
                var list = Enum.GetNames(typeof(MeasurementUnitCommonCodeContentType)).ToList();
                if (list.Exists(unit => unit == measurementUnit))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit);
                }
                else if (list.Exists(unit => unit == measurementUnit.Reverse()))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit.Reverse());
                }
            }
            if (String.IsNullOrWhiteSpace(measurementUnitRealString))
            {
                return null;
            }

            MeasurementUnitCommonCodeContentType measurementCommonUnit;
            var success = Enum.TryParse(measurementUnitRealString, out measurementCommonUnit);

            if (!success)
            {
                //measurementUnit = MeasurementUnitCommonCodeContentType.A1.ToString() ; // hard coded
                //success = Enum.TryParse<MeasurementUnitCommonCodeContentType>(measurementUnit, out measurementCommonUnit);
                ///throw new System.Exception("measurementUnit is not valid " + measurementUnit);  
            }
            ;

            return new T()
            {
                unitCode = measurementCommonUnit,
                unitCodeSpecified = success,
                Value = val
            };
        }


        T SetCodeTypeValue<T>(string val)
            where T : CodeType, new()
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return null;
            }
            return new T()
            {
                listID = "",
                listAgencyName = "",
                listName = "",
                listVersionID = "",
                name = "",
                listURI = "",
                listSchemeURI = "",
                Value = val
            };

        }

        T SetIDTypeValue<T>(string val, string schemeId = "")
            where T : IDType, new()
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return null;
            }
            return new T()
            {
                schemeID = schemeId,
                schemeName = "",
                schemeAgencyName = "",
                schemeVersionID = "",
                schemeDataURI = "",
                schemeURI = "",
                Value = val
            };
        }

        private Declaration Getdeclaration(Customs.Def.EntityPMs.DeclarationPM declarationPM)
        {
            var customDeclaration = new Declaration();
            customDeclaration.ID = new DeclarationIdentificationIDType() { Value = declarationPM.DeclarationNumber };

            customDeclaration.DeclarationOfficeID = SetIDTypeValue<DeclarationDeclarationOfficeIDType>(declarationPM.DeclarationOfficeCode);

            // customDeclaration.IssueDateTime = DataTypeConvertorUtil.Convert(DateTime.Now); // MUST  
            //  TODO: - Moran Please ReMap IssueDateTime +AgentFileReferenceNumber 
            // moran 25.5.14 - Bug 6059 - commented -->
            //#if TODO_Moran_Please_ReMap_IssueDateTime
            //          customDeclaration.AcceptanceDateTime = declarationPM.AgentFileReferenceNumber;
            //#endif

            //customDeclaration.IssueDateTime = DataTypeConvertorUtil.Convert(DateTime.Now); ;

            customDeclaration.TypeCode = SetCodeTypeValue<DeclarationTypeCodeType>("1");// MUST  hard coded

            customDeclaration.DMExtensions = GetDMExtensions(declarationPM);

            // moran 24.7.14 - Task 6817 - commented -->
            /*
            if (String.IsNullOrWhiteSpace(declarationPM.AgentId))
            {

                // moran 13.7.14 - Task 6817 -->
                var CustomsSetting = CustomsSettingQueryService.GetSettingByTenant(declarationPM.Tenant);
                // moran 13.7.14 - Task 6817 <--
                ///Debug.WriteLine("2013.07  01  agent ifd is must");
                customDeclaration.Agent = new DeclarationAgent[] //hard coded
                { 
                    new DeclarationAgent() 
                    { 
                        // moran 13.7.14 - Task 6817 change to CustomsSetting.CustomsAgentId 
                       //ID = SetIDTypeValue<AgentIdentificationIDType>("510120041"),
                       
                       ID = SetIDTypeValue<AgentIdentificationIDType>(CustomsSetting.CustomsAgentId),
                        RoleCode = new AgentRoleCodeType()
                        {
                            Value = "1"
                        }
                    } 
                };
            }
            else
            {
                */
            // moran 24.7.14 - Task 6817 - commented <--


            // moran 31.5.15 - Task 13475 -->
            //customDeclaration.Agent = new DeclarationAgent[] 
            //    { 
            //        new DeclarationAgent() 
            //        { 

            //           ID = SetIDTypeValue<AgentIdentificationIDType>(declarationPM.AgentId),
            //            RoleCode = new AgentRoleCodeType()
            //            {
            //                Value = "1" //hard coded
            //            }
            //        }

            //    };

            customDeclaration.Agent = GetDeclarationAgent(declarationPM);
            // moran 31.5.15 - Task 13475 <--        
            // }

            if (!String.IsNullOrWhiteSpace(declarationPM.ProcedureCurrentCode))
            {
                customDeclaration.GovernmentProcedure = new DeclarationGovernmentProcedure()
                {
                    CurrentCode = SetCodeTypeValue<GovernmentProcedureCurrentCodeType>(declarationPM.ProcedureCurrentCode) // new GovernmentProcedureCurrentCodeType()
                    //CurrentCode = SetCodeTypeValue < GovernmentProcedureCurrentCodeType>("4000001") 
                    //{
                    //    Value = declarationPM.ProcedureCurrentCode
                    //}
                };
            }
            // moran 13.7.14 - Task 6817 - cancel hard code --> 
            /*
            else // hard coded
            {
                customDeclaration.GovernmentProcedure = new DeclarationGovernmentProcedure()
                {
                   //CurrentCode = SetCodeTypeValue<GovernmentProcedureCurrentCodeType>(declarationPM.ProcedureCurrentCode) // new GovernmentProcedureCurrentCodeType()
                    CurrentCode = SetCodeTypeValue < GovernmentProcedureCurrentCodeType>("4000001") 
                    //{
                    //    Value = declarationPM.ProcedureCurrentCode
                    //}
                };
            }*/
            // moran 13.7.14 - Task 6817 - cancel hard code <--

            customDeclaration.Importer = GetImporter(declarationPM);
            customDeclaration.GoodsShipment = GetDeclarationGoodsShipment(declarationPM).ToArray();
            // moran 25.5.14 - Bug 6059 - commented -->
            //customDeclaration.DutyTaxFee = GetDeclarationDutyTaxFee(declarationPM).ToArray();

            return customDeclaration;
        }

        private DeclarationAgent[] GetDeclarationAgent(DeclarationPM declarationPM) // moran 31.5.15 - Task 13475
        {
            var declarationAgentList = new List<DeclarationAgent>();

            var declarationAgent = new DeclarationAgent()
            {

                ID = SetIDTypeValue<AgentIdentificationIDType>(declarationPM.AgentId),
                RoleCode = new AgentRoleCodeType()
                {
                    Value = "1" //hard coded
                }
            };

            declarationAgentList.Add(declarationAgent);

            if (declarationPM.Consignments != null && declarationPM.Consignments[0].CargoTypeCode == "17")
            {
                var declarationAgentSecond = new DeclarationAgent()
                {
                    ID = SetIDTypeValue<AgentIdentificationIDType>(declarationPM.Consignments[0].SecondCargoID),
                    RoleCode = new AgentRoleCodeType()
                    {
                        Value = "11" //hard coded
                    }
                };
                declarationAgentList.Add(declarationAgentSecond);
            }

            return declarationAgentList.ToArray();
        }


        private DeclarationDMExtensions GetDMExtensions(DeclarationPM declarationPM)
        {
            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.CustomFileNo = declarationPM.CustomFileNo;
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = declarationPM.Id;
            this.MyRequestSheetParam.RequestDescription = "הצהרת יבוא " + declarationPM.DeclarationNumber + " " + declarationPM.VersionId;

            var DMExtensions = new DeclarationDMExtensions();
            DMExtensions.AgentFileReferenceID = SetIDTypeValue<AgentFileReferenceIDType>(declarationPM.CustomFileNo); //new AgentFileReferenceIDType() { Value = declarationPM.CustomFileNo };
            // moran 25.5.14 - Bug 6059 - commented -->
            //DMExtensions.VersionID = SetIDTypeValue<DeclarationDMExtensionsVersionID>(declarationPM.VersionId); // new DeclarationDMExtensionsVersionID() { Value = declarationPM.VersionId };
            DMExtensions.ExternalDeclarationID = SetIDTypeValue<ExternalDeclarationIDType>(String.IsNullOrWhiteSpace(declarationPM.ExternalDeclarationNumber) ? declarationPM.CustomFileNo : declarationPM.ExternalDeclarationNumber); // hard coded - mandatory - takes from field other than the mapped if empty 

            // DMExtensions.ExternalDeclarationID = SetIDTypeValue<ExternalDeclarationIDType>(declarationPM.CustomFileNo); //new AgentFileReferenceIDType() { Value = declarationPM.CustomFileNo };
            //{
            //    Value = String.IsNullOrWhiteSpace(declarationPM.ExternalDeclarationNumber) ? "10008879" : declarationPM.ExternalDeclarationNumber 
            //};

            if (declarationPM.TaxationDateTime.HasValue)
            {
                DMExtensions.TaxationDateTime = DataTypeConvertorUtil.Convert(declarationPM.TaxationDateTime.Value);
            }

            DMExtensions.AutonomyRegionType = SetIDTypeValue<OriginRegionIDType>(declarationPM.AutonomyRegionTypeCode); //new OriginRegionIDType() { Value = declarationPM.AutonomyRegionTypeCode };
            if (!String.IsNullOrWhiteSpace(declarationPM.DeclarationDocumentId))
            {
                DMExtensions.PreviousDocument = GetDeclarationDMExtensionsPreviousDocument(declarationPM);
            }

            if (declarationPM.LoadingFactor.HasValue)
            {
                DMExtensions.ExpenseLoadingFactor = new DeclarationDMExtensionsExpenseLoadingFactor()
                {
                    Value = declarationPM.LoadingFactor.Value
                };
            }

            DMExtensions.AdditionalDocument = GetDeclarationDMExtensionsAdditionalDocument(declarationPM);

            return DMExtensions;
        }

        private DeclarationDMExtensionsAdditionalDocument[] GetDeclarationDMExtensionsAdditionalDocument(DeclarationPM declarationPM)
        {
            var declarationDMExtensionsAdditionalDocumentList = new List<DeclarationDMExtensionsAdditionalDocument>();
            var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = declarationPM.Id, ParentEntityCode = "Declaration" }, declarationPM.Tenant);

            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                //if (documentPointerItem.Child1EntityCode == null && documentPointerItem.Child2EntityCode == null && documentPointerItem.Child3EntityCode == null) this condition exists inside the query of get tickets for parent.
                //{
                //if (customsDocumentPM.DocumentsFilingId != null) // Only if there is a document ///mohammad.... customsdocuemntId is replaced by doucmentinid it's the same.
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId)) // Mirit 22/12/15 19136
                {
                    var declarationDMExtensionsAdditionalDocument = new DeclarationDMExtensionsAdditionalDocument();
                    declarationDMExtensionsAdditionalDocument.DMExtensions = new DeclarationDMExtensionsAdditionalDocumentDMExtensions();
                    declarationDMExtensionsAdditionalDocument.DMExtensions.ExternalAttachmentID = new ExternalAttachmentIDType();
                    declarationDMExtensionsAdditionalDocument.DMExtensions.ExternalAttachmentID.Value = customsDocumentPM.ExternalAttachmentId;

                    declarationDMExtensionsAdditionalDocumentList.Add(declarationDMExtensionsAdditionalDocument);
                }
            }
            return declarationDMExtensionsAdditionalDocumentList.ToArray();
        }


        private string ResolveFromGlobalScannedAttachmentToEntityOperation780()
        {
            return "90025241";//780

        }
        private string ResolveFromGlobalScannedAttachmentToEntityOperation707()
        {


            return "90025366"; // 707 doc type
        }
        private string ResolveFromGlobalScannedAttachmentToEntityOperation703()
        {

            return "90024967"; // 703\

        }
        private DeclarationDMExtensionsPreviousDocument GetDeclarationDMExtensionsPreviousDocument(DeclarationPM declarationPM)
        {
            return new DeclarationDMExtensionsPreviousDocument()
            {
                ID = SetIDTypeValue<DeclarationDMExtensionsPreviousDocumentID>(declarationPM.DeclarationDocumentId), // new DeclarationDMExtensionsPreviousDocumentID()
                //{
                //    Value = declarationPM.DeclarationDocumentId
                //},
                TypeCode = SetCodeTypeValue<PreviousDocumentTypeCodeType>(declarationPM.DeclarationDocumentTypeCode) // new PreviousDocumentTypeCodeType()
                //{
                //    Value = declarationPM.DeclarationDocumentTypeCode
                //}
            };
        }

        private DeclarationImporter[] GetImporter(DeclarationPM declarationPM)
        {
            var declarationImporterList = new List<DeclarationImporter>();

            // if (!String.IsNullOrWhiteSpace(declarationPM.ImporterId))
            //  {
            declarationImporterList.Add(GetDeclarationImporterRole4(declarationPM));
            //  }
            // Task 6440 - add ImporterCode fields check
            if (!String.IsNullOrWhiteSpace(declarationPM.TransferImporterId) || !String.IsNullOrWhiteSpace(declarationPM.TransferImporterCode))
            {
                declarationImporterList.Add(GetDeclarationImporterRole5(declarationPM));
            }
            if (!String.IsNullOrWhiteSpace(declarationPM.EntitleImporterId) || !String.IsNullOrWhiteSpace(declarationPM.EntitleImporterCode))
            {
                declarationImporterList.Add(GetDeclarationImporterRole6(declarationPM));
            }

            return declarationImporterList.ToArray();
        }

        private List<DeclarationDutyTaxFee> GetDeclarationDutyTaxFee(DeclarationPM declarationPM)
        {
            var declarationDutyTaxFeelist = new List<DeclarationDutyTaxFee>();
            for (int DutyTaxFeeSeq = 0; DutyTaxFeeSeq < declarationPM.DeclarationTaxes.Count(); DutyTaxFeeSeq++)
            {
                var declarationDutyTaxFee = new DeclarationDutyTaxFee();
                var declarationTaxesPM = declarationPM.DeclarationTaxes[DutyTaxFeeSeq];
                declarationDutyTaxFee.TypeCode = SetCodeTypeValue<DutyTaxFeeTypeCodeType>(declarationTaxesPM.TaxTypeCode);
                declarationDutyTaxFee.DMExtensions = GetDeclarationDutyTaxFeeDMExtension(declarationTaxesPM);

                declarationDutyTaxFeelist.Add(declarationDutyTaxFee);
            }
            return declarationDutyTaxFeelist;
        }

        private DeclarationDutyTaxFeeDMExtensions GetDeclarationDutyTaxFeeDMExtension(DeclarationTaxPM declarationTaxesPM)
        {
            var declarationDutyTaxFeeDMExtension = new DeclarationDutyTaxFeeDMExtensions();
            declarationDutyTaxFeeDMExtension.CalculatedTax = GetDeclarationDutyTaxFeeDMExtensionCalculatedTax(declarationTaxesPM);


            return declarationDutyTaxFeeDMExtension;

        }

        private DeclarationDutyTaxFeeDMExtensionsCalculatedTax GetDeclarationDutyTaxFeeDMExtensionCalculatedTax(DeclarationTaxPM declarationTaxesPM)
        {
            var calculatedTax = new DeclarationDutyTaxFeeDMExtensionsCalculatedTax();
            if (declarationTaxesPM.TotalAmount.HasValue)
            {
                calculatedTax.Amount = SetAmountTypeValue<AmountAmountType>(ISO3AlphaCurrencyCodeContentType.ILS.ToString(), declarationTaxesPM.TotalAmount.Value); // hard coded currency
            }
            if (declarationTaxesPM.DeferredTaxAmount.HasValue)
            {
                calculatedTax.DeferedTaxAmount = SetAmountTypeValue<deferedTaxAmountType>(ISO3AlphaCurrencyCodeContentType.ILS.ToString(), declarationTaxesPM.DeferredTaxAmount.Value); // hard coded currency
            }
            return calculatedTax;
        }

        private List<DeclarationGoodsShipment> GetDeclarationGoodsShipment(DeclarationPM declarationPM)
        {
            var declarationGoodsShipmentList = new List<DeclarationGoodsShipment>();
            //declarationGoodsShipmentList
            // declarationPM\



            //for (int supplierInvoiceSeq = 0; supplierInvoiceSeq < declarationPM.SupplierInvoices.Count(); supplierInvoiceSeq++)
            //{
            //var supplierInvoicePM =declarationPM.SupplierInvoices[supplierInvoiceSeq];
            //declarationGoodsShipment.SequenceNumeric = supplierInvoiceSeq + 1;

            bool IsSendConsignment = true;
            foreach (var supplierInvoicePM in declarationPM.SupplierInvoices
                ///.Where( rec => rec.SequenceNumeric !=null)
                .OrderBy(rec => rec.SequenceNumeric).ToList())
            {
                if (supplierInvoicePM.IsAccumalated == true && supplierInvoicePM.SupplierInvoiceItems != null && supplierInvoicePM.SupplierInvoiceItems.Count > 0)
                {
                    supplierInvoicePM.SupplierInvoiceItems.RemoveAll(rec => rec.IsParent != true);
                }

                var declarationGoodsShipment = new DeclarationGoodsShipment();

                declarationGoodsShipment.SequenceNumeric = supplierInvoicePM.SequenceNumeric.Value;


                declarationGoodsShipment.SequenceNumericSpecified = true;
                declarationGoodsShipment.Invoice = GetDeclarationGoodsShipmentInvoice(supplierInvoicePM);
                //CustomContext context = new CustomContext();
                //ConnectedEntity conn = new ConnectedEntity();
                //var vendorNumber = (from v in context.Vendors where v.Id == supplierInvoicePM.VendorId select v.VendorNumber);
                string vendorNumber = GetVendorNumber(supplierInvoicePM.VendorId);
                if (!String.IsNullOrWhiteSpace(vendorNumber)) // moran 13.7.14 - Task 6817 - enter into 'if'
                {
                    declarationGoodsShipment.Supplier = new DeclarationGoodsShipmentSupplier()
                    {
                        //hardcoded VendorId
                        // ID = SetIDTypeValue<SupplierIdentificationIDType>(String.IsNullOrWhiteSpace(supplierInvoicePM.VendorId) ? "2000012" : supplierInvoicePM.VendorId) // new SupplierIdentificationIDType() { Value = String.IsNullOrWhiteSpace(supplierInvoicePM.VendorId) ? "04" : supplierInvoicePM.VendorId } //HARDCODED
                        // moran 13.7.14 - Task 6817 - cancel hard code
                        //ID = SetIDTypeValue<SupplierIdentificationIDType>(String.IsNullOrWhiteSpace(vendorNumber) ? "2000012" : vendorNumber) // hard coded
                        ID = SetIDTypeValue<SupplierIdentificationIDType>(vendorNumber) // hard coded

                    };
                }
                declarationGoodsShipment.TradeTerms = new DeclarationGoodsShipmentTradeTerms() // MUST 
                {
                    //ConditionCode = SetCodeTypeValue<TradeTermsConditionCodeType>(String.IsNullOrWhiteSpace(supplierInvoicePM.IncotermCode) ? "FOB" : supplierInvoicePM.IncotermCode), 
                    ConditionCode = SetCodeTypeValue<TradeTermsConditionCodeType>(supplierInvoicePM.IncotermCode),
                    //LocationID = SetIDTypeValue <TradeTermsLocationIDType >(String.IsNullOrWhiteSpace(supplierInvoicePM.IssueCountryCode) ? "CN" : supplierInvoicePM.IssueCountryCode) 
                    LocationID = SetIDTypeValue<TradeTermsLocationIDType>(supplierInvoicePM.IssueCountryCode)
                };

                declarationGoodsShipment.CustomsValuation = GetcustomsValuation(supplierInvoicePM).ToArray();
                if (IsSendConsignment && !declarationPM.ExcludeConsignment)
                {
                    declarationGoodsShipment.Consignment = GetDeclarationConsignment(declarationPM).ToArray();
                    IsSendConsignment = false;
                }
                declarationGoodsShipment.AdditionalDocument = GetDeclarationGoodsShipmentAdditionalDocument(supplierInvoicePM);
                declarationGoodsShipment.GovernmentAgencyGoodsItem = GetDeclarationGoodsItems(supplierInvoicePM).ToArray();
                declarationGoodsShipmentList.Add(declarationGoodsShipment);

            }
            return declarationGoodsShipmentList;
        }

        private DeclarationGoodsShipmentAdditionalDocument[] GetDeclarationGoodsShipmentAdditionalDocument(SupplierInvoicePM supplierInvoicePM)
        {
            var declarationGoodsShipmentAdditionalDocumentList = new List<DeclarationGoodsShipmentAdditionalDocument>();
            //CustomsDocumentsTicketId adjustment mohammad 18.10.14
            //var customsDocumentPointerPMList = new List<CustomsDocumentPointerPM>();
            //var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_context);

            //customsDocumentPointerPMList = customsDocumentPointerQueryService.GetParentDocumentPointer(supplierInvoicePM.DeclarationId, "Declaration", supplierInvoicePM.Tenant);
            //var customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_context);
            //var customsDocumentsTicketPMList = customsDocumentsTicketQueryService.GetCustomsDocumentsTickets(new GetTicketsParams() { ParentEntityId = supplierInvoicePM.DeclarationId, ParentEntityCode = "Declaration", Child1EntityCode = "SupplierInvoice", Child1EntityId = supplierInvoicePM.InvoiceCounterKey.ToString() }, supplierInvoicePM.Tenant);


            var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = supplierInvoicePM.DeclarationId, ParentEntityCode = "Declaration", Child1EntityCode = "SupplierInvoice", Child1EntityId = supplierInvoicePM.InvoiceCounterKey.ToString() }, supplierInvoicePM.Tenant);

            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                //if (documentPointerItem.Child1EntityCode == "SupplierInvoice" && documentPointerItem.Child1EntityId == supplierInvoicePM.InvoiceCounterKey.ToString() && documentPointerItem.Child2EntityCode == null && documentPointerItem.Child3EntityCode == null)
                //{
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId)) // Mirit 22/12/15 19136
                {
                    var declarationGoodsShipmentAdditionalDocument = new DeclarationGoodsShipmentAdditionalDocument();
                    declarationGoodsShipmentAdditionalDocument.DMExtensions = new DeclarationGoodsShipmentAdditionalDocumentDMExtensions();
                    //mirit20131222 declarationGoodsShipmentAdditionalDocument.DMExtensions.AttachmentID = new AttachmentIDType();
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID = new ExternalAttachmentIDType();

                    //mirit20131222 declarationGoodsShipmentAdditionalDocument.DMExtensions.AttachmentID.Value = documentPointerItem.CustomsDocId;             // From CustomsDocuments (Ref to custom Id) 
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID.Value = customsDocumentPM.ExternalAttachmentId; //customsDocumentPM.DocumentsFilingId; // From CustomsDocumentPointers (Logitude Filling)

                    declarationGoodsShipmentAdditionalDocumentList.Add(declarationGoodsShipmentAdditionalDocument);
                }
                //}
            }
            return declarationGoodsShipmentAdditionalDocumentList.ToArray();
        }

        private string ResolveFromGlobalScannedAttachmentToEntityOperation380()
        {
            return "90019251";
        }


        private List<DeclarationGoodsShipmentCustomsValuation> GetcustomsValuation(SupplierInvoicePM supplierInvoicePM)
        {
            var customsValuationlist = new List<DeclarationGoodsShipmentCustomsValuation>();
            int supplierInvoiceFreightCounter = supplierInvoicePM.SupplierInvoiceFreightAmounts.Count();

            for (int modificationsSeq = 0; modificationsSeq < supplierInvoicePM.SupplierInvoiceModifications.Count(); modificationsSeq++)
            {
                var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications[modificationsSeq];
                if (supplierInvoiceModificationPM.TypeCode != "67" && supplierInvoiceModificationPM.TypeCode != "144" && supplierInvoiceModificationPM.TypeCode != "I02") // Mirit 25/06/15 Task 14255 - add "I02"
                {
                    var customsValuation = new DeclarationGoodsShipmentCustomsValuation();

                    customsValuation.ChargesTypeCode = new CustomsValuationChargesTypeCodeType();
                    customsValuation.ChargesTypeCode = SetCodeTypeValue<CustomsValuationChargesTypeCodeType>(supplierInvoiceModificationPM.TypeCode);
                    customsValuation.OtherChargeDeductionAmount = new CustomsValuationOtherChargeDeductionAmountType();
                    customsValuation.OtherChargeDeductionAmount = SetAmountTypeValue<CustomsValuationOtherChargeDeductionAmountType>(supplierInvoiceModificationPM.CurrencyTypeCode, supplierInvoiceModificationPM.Amount.GetValueOrDefault());
                    customsValuationlist.Add(customsValuation);
                }
            }

            if (supplierInvoicePM.InsruanceCurrencyTypeCode != null && supplierInvoicePM.InsuranceAmount != null) // Insert insurance Details (WCO: 559 = ExitToEntryChargeAmount)
            {
                var customsValuation = new DeclarationGoodsShipmentCustomsValuation();
                customsValuation.ChargesTypeCode = new CustomsValuationChargesTypeCodeType();
                customsValuation.ChargesTypeCode = SetCodeTypeValue<CustomsValuationChargesTypeCodeType>("67");
                customsValuation.ExitToEntryChargeAmount = new CustomsValuationExitToEntryChargeAmountType();
                customsValuation.ExitToEntryChargeAmount = SetAmountTypeValue<CustomsValuationExitToEntryChargeAmountType>(supplierInvoicePM.InsruanceCurrencyTypeCode, (decimal)supplierInvoicePM.InsuranceAmount);
                customsValuationlist.Add(customsValuation);
            }
            // moran 29.10.15 - call 254783 - change from InvoiceCurrencyTypeCode to FreightCurrencyTypeCode -->
            if (supplierInvoicePM.FreightCurrencyTypeCode != null && supplierInvoicePM.TotalFreightInFreightCurrency != null) // Insert Freight Details (WCO: 560 = FreightChargeAmount)
            {
                var customsValuation = new DeclarationGoodsShipmentCustomsValuation();
                customsValuation.ChargesTypeCode = new CustomsValuationChargesTypeCodeType();
                customsValuation.ChargesTypeCode = SetCodeTypeValue<CustomsValuationChargesTypeCodeType>("144");
                customsValuation.FreightChargeAmount = new CustomsValuationFreightChargeAmountType();
                customsValuation.FreightChargeAmount = SetAmountTypeValue<CustomsValuationFreightChargeAmountType>(supplierInvoicePM.FreightCurrencyTypeCode, (decimal)supplierInvoicePM.TotalFreightInFreightCurrency);
                customsValuationlist.Add(customsValuation);
            }

            return customsValuationlist;
        }


        private DeclarationGoodsShipmentInvoice GetDeclarationGoodsShipmentInvoice(SupplierInvoicePM supplierInvoicePM)
        {
            var declarationGoodsShipmentInvoice = new DeclarationGoodsShipmentInvoice();
            declarationGoodsShipmentInvoice.ID = SetIDTypeValue<InvoiceIdentificationIDType>(supplierInvoicePM.InvoiceNumber);
            // moran 18.11.15 - Task 18415 - not to send if no value -->
            //declarationGoodsShipmentInvoice.IssueDateTime = supplierInvoicePM.IssueDate.HasValue ? DataTypeConvertorUtil.Convert(supplierInvoicePM.IssueDate) : DataTypeConvertorUtil.Convert(DateTime.Now);
            if (supplierInvoicePM.IssueDate.HasValue)
            {
                declarationGoodsShipmentInvoice.IssueDateTime = DataTypeConvertorUtil.Convert(supplierInvoicePM.IssueDate);
            } // <--
            //IssueDateTimeType
            // moran 13.7.14 - Task 6817 - enter into 'if' - cancel hard code -->
            //declarationGoodsShipmentInvoice.TypeCode = SetCodeTypeValue<InvoiceTypeCodeType>(String.IsNullOrWhiteSpace(supplierInvoicePM.AccountTypeCode) ? "380" : supplierInvoicePM.AccountTypeCode); // hard coded
            if (!String.IsNullOrWhiteSpace(supplierInvoicePM.AccountTypeCode))
            {
                declarationGoodsShipmentInvoice.TypeCode = SetCodeTypeValue<InvoiceTypeCodeType>(supplierInvoicePM.AccountTypeCode);
            } // moran 13.7.14 - Task 6817 <--
            declarationGoodsShipmentInvoice.DMExtensions = GetDMExtensionsGoodsShipment(supplierInvoicePM);
            return declarationGoodsShipmentInvoice;
        }


        private DeclarationGoodsShipmentInvoiceDMExtensions GetDMExtensionsGoodsShipment(SupplierInvoicePM supplierInvoicePM)
        {

            var DMExtensions = new DeclarationGoodsShipmentInvoiceDMExtensions();
            DMExtensions.IsPrefarenceDocumentInd = new IsPrefarenceDocumentIndType() { Value = supplierInvoicePM.IsPreference };
            if (!String.IsNullOrWhiteSpace(supplierInvoicePM.PreferenceDocumentTypeCode)) //דורית שורר <PrefarenceDocumentType/>   יש לאתחל אותו כ- NULL
            {
                //not valid  <PrefarenceDocumentType/>   
                ///valid <PrefarenceDocumentType xsi:nil="true"/>
                DMExtensions.PrefarenceDocumentType = new DeclarationGoodsShipmentInvoiceDMExtensionsPrefarenceDocumentType() { Value = supplierInvoicePM.PreferenceDocumentTypeCode };
            }
            DMExtensions.PaymentType = SetCodeTypeValue<DeclarationGoodsShipmentInvoiceDMExtensionsPaymentType>(supplierInvoicePM.PaymentTypeCode); // new DeclarationGoodsShipmentInvoiceDMExtensionsPaymentType() { Value = supplierInvoicePM.PaymentTypeCode };
            //DMExtensions.InvoiceAmount = new InvoiceAmountType() { Value = supplierInvoicePM.InvoiceAmount.HasValue ? supplierInvoicePM.InvoiceAmount.Value : 0 };
            //DMExtensions.InvoiceAmount = new InvoiceAmountType() {currencyIDSpecified=true,  currencyID = entityPM.USD, Value = supplierInvoicePM.InvoiceAmount.HasValue ? supplierInvoicePM.InvoiceAmount.Value : 0 };
            if (supplierInvoicePM.InvoiceAmount.HasValue && supplierInvoicePM.InvoiceAmount != decimal.Zero)//18202
            {
                DMExtensions.InvoiceAmount = SetAmountTypeValue<InvoiceAmountType>(supplierInvoicePM.InvoiceCurrencyTypeCode, supplierInvoicePM.InvoiceAmount.Value);
            }

            //DMExtensions.InvoiceCurrency // ???

            if (supplierInvoicePM.ActualPayedAmount.HasValue)
            {
                DMExtensions.ActualPayedAmount = SetAmountTypeValue<ActualPayedAmountType>(supplierInvoicePM.ActualPayedCurrencyTypeCode, supplierInvoicePM.ActualPayedAmount.Value);
            }
            //DMExtensions.RateNumeric = supplierInvoicePM.ExchangeRate; // moran 18.11.15 - Task 18415 - not to send - commented
            //DMExtensions.RateNumericSpecified = supplierInvoicePM.ExchangeRate != null ? true : false; // moran 18.11.15 - Task 18415 - not to send - commented

            return DMExtensions;
        }

        private List<DeclarationGoodsShipmentGovernmentAgencyGoodsItem> GetDeclarationGoodsItems(SupplierInvoicePM supplierInvoicePM)
        {

            var declarationGoodsShipmentGovernmentAgencyGoodsItemList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItem>();

            for (int goodsItemSeq = 0; goodsItemSeq < supplierInvoicePM.SupplierInvoiceItems.Count(); goodsItemSeq++)
            {
                var supplierInvoiceItemPM = supplierInvoicePM.SupplierInvoiceItems[goodsItemSeq];
                var declarationGoodsShipmentGovernmentAgencyGoodsItem = new DeclarationGoodsShipmentGovernmentAgencyGoodsItem();
                declarationGoodsShipmentGovernmentAgencyGoodsItem.SequenceNumeric = supplierInvoiceItemPM.SequenceNumeric.Value; // moran 23.8.16 - changed from goodsItemSeq + 1;
                declarationGoodsShipmentGovernmentAgencyGoodsItem.SequenceNumericSpecified = true;
                declarationGoodsShipmentGovernmentAgencyGoodsItem.Origin = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemOrigin()
                {
                    CountryCode = new OriginCountryCodeType() { Value = supplierInvoiceItemPM.OriginCountryCode }
                };
                declarationGoodsShipmentGovernmentAgencyGoodsItem.Commodity = GetGoodsItemCommodity(supplierInvoiceItemPM);
                declarationGoodsShipmentGovernmentAgencyGoodsItem.GoodsMeasure = GetGoodsMeasure(supplierInvoiceItemPM); // MUST

                declarationGoodsShipmentGovernmentAgencyGoodsItem.PreviousDocument = GetPreviousDocument(supplierInvoiceItemPM);
                if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.ManufactureIdentifier))
                {
                    declarationGoodsShipmentGovernmentAgencyGoodsItem.Manufacturer = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemManufacturer()
                    {
                        ID = SetIDTypeValue<ManufacturerIdentificationIDType>(supplierInvoiceItemPM.ManufactureIdentifier) // new ManufacturerIdentificationIDType() { Value = supplierInvoiceItemPM.ManufactureIdentifier }
                    };
                }

                declarationGoodsShipmentGovernmentAgencyGoodsItem.DMExtensions = GetDMExtensionsGoodsItem(supplierInvoiceItemPM, supplierInvoicePM);
                declarationGoodsShipmentGovernmentAgencyGoodsItem.AdditionalDocument = GetGoodsItemAdditionalDocument(supplierInvoiceItemPM);
                declarationGoodsShipmentGovernmentAgencyGoodsItem.ValuationAdjustment = GetGoodsItemValuationAdjustment(supplierInvoiceItemPM);

                declarationGoodsShipmentGovernmentAgencyGoodsItemList.Add(declarationGoodsShipmentGovernmentAgencyGoodsItem);
            }
            return declarationGoodsShipmentGovernmentAgencyGoodsItemList;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment[] GetGoodsItemValuationAdjustment(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemsMods == null)
            {
                return null;
            }
            var valuationAdjustmentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment>();
            foreach (var valuationAdjustmentItem in supplierInvoiceItemPM.SupplierInvoiceItemsMods)
            {
                var valuationAdjustment = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment();
                valuationAdjustment.AdditionCode = new ValuationAdjustmentAdditionCodeType();
                valuationAdjustment.AdditionCode.Value = valuationAdjustmentItem.TypeCode;
                valuationAdjustment.AmountAmount = new ValuationAdjustmentAmountAmountType();
                valuationAdjustment.AmountAmount = SetAmountTypeValue<ValuationAdjustmentAmountAmountType>(valuationAdjustmentItem.CurrencyTypeCode, valuationAdjustmentItem.Amount > 0 ? (Decimal)valuationAdjustmentItem.Amount : 0);
                valuationAdjustmentList.Add(valuationAdjustment);
            }
            return valuationAdjustmentList.ToArray();
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument[] GetGoodsItemAdditionalDocument(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            var goodsItemAdditionalDocumentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument>();
            //CustomsDocumentsTicketId adjustment mohammad 18.10.14
            //var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_context);
            //var customsDocumentPointerPMList = new List<CustomsDocumentPointerPM>();

            //Get supplier Item Document - From CustomsDocumentPointer Table
            //customsDocumentPointerPMList = customsDocumentPointerQueryService.GetParentDocumentPointer(supplierInvoiceItemPM.DeclarationId, "Declaration", supplierInvoiceItemPM.Tenant);

            //var customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_context);
            //var customsDocumentsTicketPMList = customsDocumentsTicketQueryService.GetCustomsDocumentsTickets(new GetTicketsParams() { ParentEntityId = supplierInvoiceItemPM.DeclarationId, ParentEntityCode = "Declaration", Child1EntityCode = "SupplierInvoice", Child1EntityId = supplierInvoiceItemPM.CounterKey.ToString(), Child2EntityCode = "SupplierInvoiceItem", Child2EntityId = supplierInvoiceItemPM.LineNumber.ToString() }, supplierInvoiceItemPM.Tenant);

            var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
            var customsDocumentPMList = customsDocumentQueryService.GetCustomsDocumentPMListWithoutRequestedDoc(new GetTicketsParams() { ParentEntityId = supplierInvoiceItemPM.DeclarationId, ParentEntityCode = "Declaration", Child1EntityCode = "SupplierInvoice", Child1EntityId = supplierInvoiceItemPM.CounterKey.ToString(), Child2EntityCode = "SupplierInvoiceItem", Child2EntityId = supplierInvoiceItemPM.LineNumber.ToString() }, supplierInvoiceItemPM.Tenant);
            foreach (var customsDocumentPM in customsDocumentPMList)
            {
                //if (documentTicketItem.Child1EntityCode == "SupplierInvoice" && documentTicketItem.Child1EntityId == supplierInvoiceItemPM.CounterKey.ToString() && documentTicketItem.Child2EntityCode == "SupplierInvoiceItem" && documentTicketItem.Child2EntityId == supplierInvoiceItemPM.LineNumber.ToString())
                //{
                //if (documentTicketItem.Child3EntityId == null && documentTicketItem.Child3EntityCode == null && documentTicketItem.CustomDocumentId != null)
                //{
                if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId)) // Mirit 22/12/15 19136
                {
                    var declarationGoodsShipmentAdditionalDocument = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument();
                    declarationGoodsShipmentAdditionalDocument.TypeCode = new AdditionalDocumentTypeCodeType();
                    declarationGoodsShipmentAdditionalDocument.TypeCode.Value = "3";
                    declarationGoodsShipmentAdditionalDocument.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensions();
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID = new ExternalAttachmentIDType();
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID.Value = customsDocumentPM.ExternalAttachmentId; //customsDocumentPM.DocumentsFilingId; // From CustomsDocumentPointers (Logitude Filling)

                    goodsItemAdditionalDocumentList.Add(declarationGoodsShipmentAdditionalDocument);
                }
                //}
            }

            List<Unifreight.BL.EntityPMs.GITITEMCRPM> itemCrList = GetItemCrList(supplierInvoiceItemPM);
            //Get supplier Item Certificate - From SupplierInvioceItemsCertificates Table
            foreach (var CertificateItem in supplierInvoiceItemPM.SupplierInvioceItemCertificats.OrderBy(x => x.SequenceNumeric))
            {
                var itemCert = itemCrList.Where(r => r.REQCERT.TrimStart('0') == CertificateItem.ReqConfirmationTypeCode).FirstOrDefault();
                if (!(string.IsNullOrWhiteSpace(CertificateItem.ResConfirmationTypeCode) && string.IsNullOrWhiteSpace(CertificateItem.CertificateNumber) && string.IsNullOrWhiteSpace(CertificateItem.CertificateExemptionTypeCode) && string.IsNullOrWhiteSpace(CertificateItem.AttachmentTypeCode) && string.IsNullOrWhiteSpace(CertificateItem.CustomsAttachmentID)) || (itemCert != null && !string.IsNullOrWhiteSpace(itemCert.REQCERT)))
                { // moran 26.9.16 - Task 22961 - enter into 'if' fields are empty
                    var declarationGoodsShipmentAdditionalDocument = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument();
                    if (CertificateItem.CertificateNumber != null)
                    {
                        declarationGoodsShipmentAdditionalDocument.ID = SetIDTypeValue<AdditionalDocumentIdentificationIDType>(string.Join("", CertificateItem.CertificateNumber.ToCharArray().Where(Char.IsDigit)));
                    }
                    declarationGoodsShipmentAdditionalDocument.LPCOExemptionCode = SetCodeTypeValue<AdditionalDocumentLPCOExemptionCodeType>(CertificateItem.CertificateExemptionTypeCode);
                    declarationGoodsShipmentAdditionalDocument.TypeCode = SetCodeTypeValue<AdditionalDocumentTypeCodeType>(CertificateItem.AttachmentTypeCode);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensions();
                    //mirit20131222 declarationGoodsShipmentAdditionalDocument.DMExtensions.AttachmentID = SetIDTypeValue<AttachmentIDType>(CertificateItem.CustomsAttachmentID);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.LPCOTypeCode = SetCodeTypeValue<LpcoTypeCodeType>(CertificateItem.ResConfirmationTypeCode);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.requirementLicenseType = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensionsRequirementLicenseType>(CertificateItem.ReqConfirmationTypeCode);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.ExternalAttachmentID = SetIDTypeValue<ExternalAttachmentIDType>(CertificateItem.CustomsAttachmentID);
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.SequenceNumeric = CertificateItem.SequenceNumeric; // moran 1.8.16 - Task 21933
                    declarationGoodsShipmentAdditionalDocument.DMExtensions.SequenceNumericSpecified = true; // moran 8.8.16 - Task 21933
                    goodsItemAdditionalDocumentList.Add(declarationGoodsShipmentAdditionalDocument);
                }
            }

            return goodsItemAdditionalDocumentList.ToArray();
        }

        private static List<Unifreight.BL.EntityPMs.GITITEMCRPM> GetItemCrList(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (!CustomsSettingQueryService.GetLogitudeCustomsSettingsM(supplierInvoiceItemPM.Tenant).IsConnectedToUniFreight)
            {
                return new List<Unifreight.BL.EntityPMs.GITITEMCRPM>();
            }
            var itemCrQueryService = new Unifreight.BL.EntityQueryServices.GITITEMCRQueryService(AmitalContext.GetContext(supplierInvoiceItemPM.Tenant));
            List<Unifreight.BL.EntityPMs.GITITEMCRPM> itemCrList = itemCrQueryService.GetMulti(supplierInvoiceItemPM.ItemCode, true);
            return itemCrList;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument[] GetGoodsItemAdditionalDocument(List<SupplierInvioceItemCertificatPM> supplierInvoiceItemsCertificatesPM)
        {

            var goodsItemAdditionalDocumentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument>();
            for (int itemsCertificatesSeq = 0; itemsCertificatesSeq < supplierInvoiceItemsCertificatesPM.Count(); itemsCertificatesSeq++)
            {
                var supplierInvoiceItemsCertificates = supplierInvoiceItemsCertificatesPM[itemsCertificatesSeq];
                if (!(string.IsNullOrWhiteSpace(supplierInvoiceItemsCertificates.ResConfirmationTypeCode) && string.IsNullOrWhiteSpace(supplierInvoiceItemsCertificates.CertificateNumber) && string.IsNullOrWhiteSpace(supplierInvoiceItemsCertificates.CertificateExemptionTypeCode) && string.IsNullOrWhiteSpace(supplierInvoiceItemsCertificates.AttachmentTypeCode) && string.IsNullOrWhiteSpace(supplierInvoiceItemsCertificates.CustomsAttachmentID)))
                { // moran 26.9.16 - Task 22961 - enter into 'if' fields are empty
                    var goodsItemAdditionalDocument = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocument();
                    goodsItemAdditionalDocument.ID = new AdditionalDocumentIdentificationIDType() { Value = supplierInvoiceItemsCertificates.CertificateNumber };
                    goodsItemAdditionalDocument.LPCOExemptionCode = SetCodeTypeValue<AdditionalDocumentLPCOExemptionCodeType>(supplierInvoiceItemsCertificates.CertificateExemptionTypeCode);
                    goodsItemAdditionalDocument.TypeCode = SetCodeTypeValue<AdditionalDocumentTypeCodeType>(supplierInvoiceItemsCertificates.AttachmentTypeCode);
                    goodsItemAdditionalDocument.DMExtensions = GetGoodsItemAdditionalDocumentDMExtensions(supplierInvoiceItemsCertificates);
                    goodsItemAdditionalDocumentList.Add(goodsItemAdditionalDocument);
                }
            }
            return goodsItemAdditionalDocumentList.ToArray();
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensions GetGoodsItemAdditionalDocumentDMExtensions(SupplierInvioceItemCertificatPM supplierInvoiceItemsCertificatesPM)
        {
            var goodsItemAdditionalDocumentDMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensions();
            //mirit20131222 goodsItemAdditionalDocumentDMExtensions.AttachmentID = SetIDTypeValue<AttachmentIDType>(supplierInvoiceItemsCertificatesPM.CustomsAttachmentID);
            goodsItemAdditionalDocumentDMExtensions.requirementLicenseType = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemAdditionalDocumentDMExtensionsRequirementLicenseType>(supplierInvoiceItemsCertificatesPM.ReqConfirmationTypeCode);
            goodsItemAdditionalDocumentDMExtensions.LPCOTypeCode = SetCodeTypeValue<LpcoTypeCodeType>(supplierInvoiceItemsCertificatesPM.ResConfirmationTypeCode);
            goodsItemAdditionalDocumentDMExtensions.SequenceNumeric = supplierInvoiceItemsCertificatesPM.SequenceNumeric; // moran 1.8.16 - Task 21933
            goodsItemAdditionalDocumentDMExtensions.SequenceNumericSpecified = true; // moran 8.8.16 - Task 21933
            return goodsItemAdditionalDocumentDMExtensions;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocument[] GetPreviousDocument(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {

            var previousDocumentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocument>();
            for (int ConnectedDeclarationsSeq = 0; ConnectedDeclarationsSeq < supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars.Count(); ConnectedDeclarationsSeq++)
            {
                var supplierInvoiceItemConnectedDeclaration = supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars[ConnectedDeclarationsSeq];

                var previousDocument = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocument();
                previousDocument.ID = SetIDTypeValue<PreviousDocumentIdentificationIDType>(supplierInvoiceItemConnectedDeclaration.DeclarationNumber);
                if (supplierInvoiceItemConnectedDeclaration.ItemSequence.HasValue)
                {
                    previousDocument.SequenceNumeric = supplierInvoiceItemConnectedDeclaration.ItemSequence.Value; // ConnectedDeclarationsSeq + 1;
                    previousDocument.SequenceNumericSpecified = true;
                }
                previousDocument.TypeCode = SetCodeTypeValue<PreviousDocumentTypeCodeType>(supplierInvoiceItemConnectedDeclaration.DeclarationTypeCode);
                previousDocument.DMExtensions = GetItemPreviousDocumentDMExtensions(supplierInvoiceItemConnectedDeclaration);

                previousDocumentList.Add(previousDocument);
            }
            return previousDocumentList.ToArray();

        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocumentDMExtensions GetItemPreviousDocumentDMExtensions(SupplierInvoiceItemsConDeclarPM supplierInvoiceItemConnectedDeclaration)
        {
            var previousDocumentDMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocumentDMExtensions();
            if (supplierInvoiceItemConnectedDeclaration.Quantity.HasValue)
            {// moran 24.7.14 - Task 6817 change from KGM to EA --> // moran 22.12.15 - Task 19549 change from EA to new field
                //previousDocumentDMExtensions.QuantityQuantity = SetQuantityTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocumentDMExtensionsQuantityQuantity>(MeasurementUnitCommonCodeContentType.KGM.ToString(), supplierInvoiceItemConnectedDeclaration.Quantity.Value); // hard coded KGM
                previousDocumentDMExtensions.QuantityQuantity = SetQuantityTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemPreviousDocumentDMExtensionsQuantityQuantity>(supplierInvoiceItemConnectedDeclaration.QuantityTypeCode, supplierInvoiceItemConnectedDeclaration.Quantity.Value);
            }
            if (supplierInvoiceItemConnectedDeclaration.InvoiceNumber.HasValue)
            {
                previousDocumentDMExtensions.SequenceNumeric = supplierInvoiceItemConnectedDeclaration.InvoiceNumber.Value;
                previousDocumentDMExtensions.SequenceNumericSpecified = true;
            }
            return previousDocumentDMExtensions;
        }


        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure[] GetGoodsMeasure(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            var goodsMeasureList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure>();
            // moran 5.12.13 - Task 2296 -->
            if (supplierInvoiceItemPM.InvoiceQuantity.HasValue)
            {
                var goodsMeasure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure();
                goodsMeasure.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensions()
                {
                    MeasureQualifier = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensionsMeasureQualifier>("1")
                };
                goodsMeasure.TariffQuantity = SetQuantityTypeValue<GoodsMeasureTariffQuantityType>(supplierInvoiceItemPM.InvoiceQuantityType, supplierInvoiceItemPM.InvoiceQuantity.Value);
                goodsMeasureList.Add(goodsMeasure);
            }
            if (supplierInvoiceItemPM.StatisticQuantity.HasValue)
            {
                var goodsMeasure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure();
                goodsMeasure.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensions()
                {
                    MeasureQualifier = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensionsMeasureQualifier>("2")
                };
                goodsMeasure.TariffQuantity = SetQuantityTypeValue<GoodsMeasureTariffQuantityType>(supplierInvoiceItemPM.StatisticQuantityType, supplierInvoiceItemPM.StatisticQuantity.Value);
                goodsMeasureList.Add(goodsMeasure);
            }
            if (supplierInvoiceItemPM.AdditionalQuantity.HasValue)
            {
                var goodsMeasure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure();
                goodsMeasure.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensions()
                {
                    MeasureQualifier = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensionsMeasureQualifier>("3")
                };
                goodsMeasure.TariffQuantity = SetQuantityTypeValue<GoodsMeasureTariffQuantityType>(supplierInvoiceItemPM.AdditionalQuantityType, supplierInvoiceItemPM.AdditionalQuantity.Value);
                goodsMeasureList.Add(goodsMeasure);
            }

            return goodsMeasureList.ToArray();
            /*
            for (int itemsQuantitiesSeq = 0; itemsQuantitiesSeq < supplierInvoiceItemPM.SupplierInvoiceItemsQuantities.Count(); itemsQuantitiesSeq++)
            {
                var supplierInvoiceItemQuantity = supplierInvoiceItemPM.SupplierInvoiceItemsQuantities[itemsQuantitiesSeq];

                var goodsMeasure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasure();
                if (!String.IsNullOrWhiteSpace(supplierInvoiceItemQuantity.MeasureQualifierCode))
                {
                    goodsMeasure.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensions()
                    {
                        MeasureQualifier = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemGoodsMeasureDMExtensionsMeasureQualifier>(supplierInvoiceItemQuantity.MeasureQualifierCode)
                    };
                }
                var measurementUnit = "";
                if (supplierInvoiceItemQuantity.MeasureQualifierCode == "1")
                {
                    measurementUnit = "KGM";
                }
                else if (supplierInvoiceItemQuantity.MeasureQualifierCode == "2")
                {
                    measurementUnit = "GRO";
                }

                if (supplierInvoiceItemQuantity.Quantity.HasValue)
                {
                    goodsMeasure.TariffQuantity = SetQuantityTypeValue<GoodsMeasureTariffQuantityType>("", supplierInvoiceItemQuantity.Quantity.Value); // hard coded null for unit - no mapping
                }
                //else
                //{
                //    goodsMeasure.TariffQuantity = SetQuantityTypeValue<GoodsMeasureTariffQuantityType>(measurementUnit, 1); // hard coded null for unit + 1 for qty -> must
                //}
                goodsMeasureList.Add(goodsMeasure);
            }
            return goodsMeasureList.ToArray();
             */
            // moran 5.12.13 - Task 2296 <--
        }


        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodity GetGoodsItemCommodity(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            decimal sequenceNumeric;
            decimal.TryParse(supplierInvoiceItemPM.ActualInvoiceLines, out sequenceNumeric);
            DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification myDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification = null;

            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.DangerousClassificationCode))
            {
                if (supplierInvoiceItemPM.DangerousClassificationCode.Length == 11) // moran 1.9.14 - uncommented
                {
                    supplierInvoiceItemPM.DangerousClassificationCode = supplierInvoiceItemPM.DangerousClassificationCode.Insert(10, "/");
                }
                myDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification()
                {
                    ID = SetIDTypeValue<ClassificationIdentificationIDType>(supplierInvoiceItemPM.DangerousClassificationCode), // new ClassificationIdentificationIDType()
                    //{
                    //    Value = supplierInvoiceItemPM.DangerousClassificationCode
                    //},
                    IdentificationTypeCode = SetCodeTypeValue<ClassificationIdentificationTypeCodeType>("SSO"),

                    //{
                    //    Value = "SSO"
                    //}
                    // moran 12.4.16 - Bug 20650 -->
                    DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDMExtensions()
                    {
                        DangerousGoodsPackingRequirementsGroupCode = SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassificationDMExtensionsDangerousGoodsPackingRequirementsGroupCode>(supplierInvoiceItemPM.DangerousPackingGroupTypeCode)
                    }
                    // moran 12.4.16 - Bug 20650 <--
                };
            }
            var declarationGoodsItemCommodity = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodity();
            declarationGoodsItemCommodity.DMExtensions = GetGoodsItemCommodityDMExtensions(supplierInvoiceItemPM);
            // moran 25.5.14 - Bug 6059 - commented -->
            //declarationGoodsItemCommodity.DutyTaxFee = GetGoodsItemCommodityDutyTaxFees(supplierInvoiceItemPM);
            //declarationGoodsItemCommodity.DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensions()
            //{
            //    DutyRegimeCode = new DutyTaxFeeDutyRegimeCodeType() { Value = supplierInvoiceItemPM.TariffCode }
            //};
            if (!string.IsNullOrWhiteSpace(supplierInvoiceItemPM.ClassificationCode))
            {
                if (supplierInvoiceItemPM.ClassificationCode.Length == 11) // moran 1.9.14 - uncommented
                {
                    supplierInvoiceItemPM.ClassificationCode = supplierInvoiceItemPM.ClassificationCode.Insert(10, "/");
                }

                declarationGoodsItemCommodity.Classification = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification[]
                {
                    new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification()
                    {
                        ID = SetIDTypeValue<ClassificationIdentificationIDType>(supplierInvoiceItemPM.ClassificationCode), // new ClassificationIdentificationIDType()
                    // {
                      //   Value = supplierInvoiceItemPM.ClassificationCode
                     //},
                        IdentificationTypeCode = SetCodeTypeValue < ClassificationIdentificationTypeCodeType>("HS") /*new ClassificationIdentificationTypeCodeType
                     {
                         Value = "HS"
                     }*/
                    },
                    myDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityClassification
                };
            }
            /// 00000643	12.22010422	[7220] השדה : Declaration.GoodsShipment.GovernmentAgencyGoodsItem.Commodity.GovernmentProcedure.CurrentCode הינו שדה חובה 	
            /// please insert PROCESS_TYPE 
            /// // moran 8.3.15 - Task 11745 - commented handle with field PROCESS_TYPE and initiate with ProcessTypes table values -->
            /*
            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.PROCESS_TYPE))
            {
                declarationGoodsItemCommodity.GovernmentProcedure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityGovernmentProcedure[]
                 {
                     new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityGovernmentProcedure()
                     {
                         CurrentCode = new GovernmentProcedureCurrentCodeType()
                         {
                             Value = supplierInvoiceItemPM.PROCESS_TYPE
                         }
                     }
                 };
            }*/

            if (supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes != null && supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Count > 0)
            {
                declarationGoodsItemCommodity.GovernmentProcedure = GetGoodsItemCommodityGovernmentProcedure(supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes);
            }
            // moran 8.3.15 - Task 11745 <--
            //if (String.IsNullOrWhiteSpace(supplierInvoiceItemPM.INV_LINE_CONN))
            if (String.IsNullOrWhiteSpace(sequenceNumeric.ToString()))
            {

#if dueRefersh20140917
                declarationGoodsItemCommodity.InvoiceLine = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityInvoiceLine[]
               {
                     new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityInvoiceLine()
                     {
                         SequenceNumeric = sequenceNumeric,
                         SequenceNumericSpecified = true 
                     }
                };
#endif
            };

            return declarationGoodsItemCommodity;
        }

        // moran 8.3.15 - Task 11745 -->
        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityGovernmentProcedure[] GetGoodsItemCommodityGovernmentProcedure(List<SupplierInvoiceItemProcesTypePM> SupplierInvoiceItemsProcessTypesPM)
        {

            var goodsItemCommodityGovernmentProcedureList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityGovernmentProcedure>();
            foreach (var SupplierInvoiceItemsProcessType in SupplierInvoiceItemsProcessTypesPM)
            {
                var GoodsItemCommodityGovernmentProcedure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityGovernmentProcedure();
                GoodsItemCommodityGovernmentProcedure.CurrentCode = SetCodeTypeValue<GovernmentProcedureCurrentCodeType>(SupplierInvoiceItemsProcessType.ProcessTypeCode);
                goodsItemCommodityGovernmentProcedureList.Add(GoodsItemCommodityGovernmentProcedure);
            }
            return goodsItemCommodityGovernmentProcedureList.ToArray();
        }
        // moran 8.3.15 - Task 11745 <--


        // moran 8.3.15 - Task 11745 -->
        //private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityGovernmentProcedure[] GetGoodsItemCommodityGovernmentProcedure(List<SupplierInvoiceItemsProcessTypePM> SupplierInvoiceItemsProcessTypesPM)
        //{

        //    var goodsItemCommodityGovernmentProcedureList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityGovernmentProcedure>();
        //    foreach (var SupplierInvoiceItemsProcessType in SupplierInvoiceItemsProcessTypesPM)
        //    {
        //        var GoodsItemCommodityGovernmentProcedure = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityGovernmentProcedure();
        //        GoodsItemCommodityGovernmentProcedure.CurrentCode = SetCodeTypeValue<GovernmentProcedureCurrentCodeType>(SupplierInvoiceItemsProcessType.ProcessTypeCode);
        //        goodsItemCommodityGovernmentProcedureList.Add(GoodsItemCommodityGovernmentProcedure);
        //    }
        //    return goodsItemCommodityGovernmentProcedureList.ToArray();
        //}
        // moran 8.3.15 - Task 11745 <--

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFee[] GetGoodsItemCommodityDutyTaxFees(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            var goodsItemCommodityDutyTaxFeesList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFee>();
            for (int itemsTaxSeq = 0; itemsTaxSeq < supplierInvoiceItemPM.SupplierInvoiceItemTaxes.Count(); itemsTaxSeq++)
            {
                var supplierInvoiceItemsTax = supplierInvoiceItemPM.SupplierInvoiceItemTaxes[itemsTaxSeq];
                var goodsItemCommodityDutyTaxFees = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFee();
                goodsItemCommodityDutyTaxFees.TypeCode = SetCodeTypeValue<DutyTaxFeeTypeCodeType>(supplierInvoiceItemsTax.TaxTypeCode);
                goodsItemCommodityDutyTaxFees.DutyRegimeCode = SetCodeTypeValue<DutyTaxFeeDutyRegimeCodeType>(supplierInvoiceItemsTax.TradeAgreementTypeCode);
                if (supplierInvoiceItemsTax.TaxRate.HasValue)
                {
                    goodsItemCommodityDutyTaxFees.TaxRate = supplierInvoiceItemsTax.TaxRate.Value;
                    goodsItemCommodityDutyTaxFees.TaxRateSpecified = true;
                }
                if (supplierInvoiceItemsTax.TaxBaseAmount.HasValue)
                {
                    goodsItemCommodityDutyTaxFees.AdValoremTaxBaseAmount = SetAmountTypeValue<DutyTaxFeeAdValoremTaxBaseAmountType>(ISO3AlphaCurrencyCodeContentType.ILS.ToString(), supplierInvoiceItemsTax.TaxBaseAmount.Value);
                }
                goodsItemCommodityDutyTaxFees.DMExtensions = GetItemCommodityDutyTaxFeeDMExtensions(supplierInvoiceItemsTax);

                goodsItemCommodityDutyTaxFeesList.Add(goodsItemCommodityDutyTaxFees);
            }
            return goodsItemCommodityDutyTaxFeesList.ToArray();
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFeeDMExtensions GetItemCommodityDutyTaxFeeDMExtensions(SupplierInvoiceItemsTaxPM supplierInvoiceItemsTax)
        {
            var goodsItemCommodityDutyTaxFeeDMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFeeDMExtensions();
            goodsItemCommodityDutyTaxFeeDMExtensions.CalculatedTax = GetGoodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax(supplierInvoiceItemsTax);
            return goodsItemCommodityDutyTaxFeeDMExtensions;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax GetGoodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax(SupplierInvoiceItemsTaxPM supplierInvoiceItemsTax)
        {
            var goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax();
            if (supplierInvoiceItemsTax.TaxAmount.HasValue)
            {
                goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.Amount = SetAmountTypeValue<AmountAmountType>(ISO3AlphaCurrencyCodeContentType.ILS.ToString(), supplierInvoiceItemsTax.TaxAmount.Value);
            }
            if (supplierInvoiceItemsTax.DeferedTaxAmount.HasValue)
            {
                goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.DeferedTaxAmount = SetAmountTypeValue<deferedTaxAmountType>(ISO3AlphaCurrencyCodeContentType.ILS.ToString(), supplierInvoiceItemsTax.DeferedTaxAmount.Value);
            }
            if (supplierInvoiceItemsTax.DefinedPerUnitMeasure.HasValue)
            {
                goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.AlternateDefinedPerUnitMeasure = SetAmountTypeValue<AlternateDefinedPerUnitMeasureType>(supplierInvoiceItemsTax.MeasurementUnitCode, supplierInvoiceItemsTax.DefinedPerUnitMeasure.Value);
            }
            goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.AlternateRate = new AlternateRateType() { Value = supplierInvoiceItemsTax.AlternateRate.Value };
            if (supplierInvoiceItemsTax.AlternateDefinedPerUnitMeasure.HasValue)
            {
                goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.AlternateDefinedPerUnitMeasure = SetAmountTypeValue<AlternateDefinedPerUnitMeasureType>(supplierInvoiceItemsTax.AlternateMeasurementUnitCode, supplierInvoiceItemsTax.AlternateDefinedPerUnitMeasure.Value);
            }
            if (supplierInvoiceItemsTax.DefinedPerUnitQuantity.HasValue)
            {
                goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.DefinedPerUnitQuantity = SetQuantityTypeValue<DefinedPerUnitQuantityType>(supplierInvoiceItemsTax.MeasurementUnitCode, supplierInvoiceItemsTax.DefinedPerUnitQuantity.Value);
            }
            if (supplierInvoiceItemsTax.AlternateDefinedPerUnitQuant.HasValue)
            {
                goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.AlternateDefinedPerUnitQuantity = SetQuantityTypeValue<AlternateDefinedPerUnitQuantityType>(supplierInvoiceItemsTax.AlternateMeasurementUnitCode, supplierInvoiceItemsTax.AlternateDefinedPerUnitQuant.Value);
            }
            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemsTax.MeasurementUnitCode))
            {
                goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.MeasurementUnitCode = SetCodeTypeValue<MeasurementUnitCodeType>(supplierInvoiceItemsTax.MeasurementUnitCode);
            }
            goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.AlternateMeasurementUnit = SetCodeTypeValue<AlternateMeasurementUnitType>(supplierInvoiceItemsTax.AlternateMeasurementUnitCode);
            goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.TradeLevyNumber = SetIDTypeValue<TradeLevyNumberType>(supplierInvoiceItemsTax.TradeLevyNumber);
            if (supplierInvoiceItemsTax.TotalBtlCoverageNIS.HasValue)
            {
                goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax.TotalBtlCoverageNIS = SetAmountTypeValue<totalBtlCoverageNISType>(ISO3AlphaCurrencyCodeContentType.ILS.ToString(), supplierInvoiceItemsTax.TotalBtlCoverageNIS.Value);
            }
            return goodsItemCommodityDutyTaxFeeDMExtensionsCalculatedTax;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensions GetGoodsItemCommodityDMExtensions(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            var DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensions();
            // moran 31.5.15 - Task 13692 -->
            //DMExtensions.DutyRegimeCode = new DutyTaxFeeDutyRegimeCodeType() { Value = supplierInvoiceItemPM.TradeAgreementCode };
            DMExtensions.DutyRegimeCode = SetCodeTypeValue<DutyTaxFeeDutyRegimeCodeType>(supplierInvoiceItemPM.TradeAgreementCode);
            // moran 31.5.15 - Task 13692 <--
            //var ValuationDeductionAdjustmentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment>();
            // moran 25.5.14 - Bug 6059 - commented -->
            //DMExtensions.ValuationDeductionAdjustment = GetGoodsItemCommodityDMExtensionsValuationDeductionAdjustment(supplierInvoiceItemPM.SupplierInvoiceItemsModifications);

            DMExtensions.TradeLevyAndExampt = GetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt(supplierInvoiceItemPM); //Yuval Chalup 30.12.2014 TASK-9501
            DMExtensions.SerialNumbers = GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbers(supplierInvoiceItemPM); // Mirit 24/06/15 Task 14252
            DMExtensions.ProductName = GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductName(supplierInvoiceItemPM); // Mirit 24/06/15 Task 14252
            DMExtensions.ProductIdentification = GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentification(supplierInvoiceItemPM); // Mirit 24/06/15 Task 14252

            return DMExtensions;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentification[] GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentification(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents == null || supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentification> myDMExtensionsProductIdentification = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentification>();
            foreach (var productIdentificationItem in supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents)
            {
                var productIdentification = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentification();
                productIdentification.ID = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductIdentificationID();
                productIdentification.ID.Value = productIdentificationItem.Identification;
                productIdentification.IDTypeCode = new CommodityIDTypeCodeType();
                productIdentification.IDTypeCode.Value = productIdentificationItem.TypeCode;
                myDMExtensionsProductIdentification.Add(productIdentification);
            }

            return myDMExtensionsProductIdentification.ToArray(); ;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductName[] GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductName(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemsDescripts == null || supplierInvoiceItemPM.SupplierInvoiceItemsDescripts.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductName> myDMExtensionsProductName = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductName>();
            foreach (var productNameItem in supplierInvoiceItemPM.SupplierInvoiceItemsDescripts)
            {
                var productName = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductName();
                productName.Name = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductNameName();
                productName.Name.Value = productNameItem.Description;
                productName.NameQualifierCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsProductNameNameQualifierCode();
                productName.NameQualifierCode.Value = productNameItem.TypeCode;
                myDMExtensionsProductName.Add(productName);
            }

            return myDMExtensionsProductName.ToArray(); ;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbers[] GetGetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbers(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums == null || supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbers> myDMExtensionsSerialNumbers = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbers>();
            foreach (var serialNumbersItem in supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums)
            {
                var serialNumber = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbers();
                serialNumber.ID = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbersID();
                serialNumber.ID.Value = serialNumbersItem.SerialNumber;
                serialNumber.IdentityQualifierCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsSerialNumbersIdentityQualifierCode();
                serialNumber.IdentityQualifierCode.Value = serialNumbersItem.TypeCode;
                myDMExtensionsSerialNumbers.Add(serialNumber);
            }

            return myDMExtensionsSerialNumbers.ToArray(); ;
        }

        //<--- Yuval Chalup 30.12.2014 TASK-9501
        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt[] GetDeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            if (supplierInvoiceItemPM.SupplierInvoiceItemLevies == null)
            {
                return null;
            }
            if (supplierInvoiceItemPM.SupplierInvoiceItemLevies.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt> declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExamptList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt>();

            foreach (var supplierInvoiceItemLevies in supplierInvoiceItemPM.SupplierInvoiceItemLevies)
            {
                DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt();
                if (!string.IsNullOrWhiteSpace(supplierInvoiceItemLevies.TradeLevyExamptCode))
                {
                    declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt.tradeLevyExamptCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExamptTradeLevyExamptCode
                    {
                        Value = supplierInvoiceItemLevies.TradeLevyExamptCode
                    };
                }
                if (!string.IsNullOrEmpty(supplierInvoiceItemLevies.TradeLevyNumber)) // changed by Alaa WI:13229
                {
                    declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt.tradeLevyNumber = new TradeLevyNumberType
                    {
                        Value = supplierInvoiceItemLevies.TradeLevyNumber
                    };
                }

                declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExamptList.Add(declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExampt);

            }
            return declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsTradeLevyAndExamptList.ToArray();
        }
        //Yuval Chalup 30.12.2014 TASK-9501 --->

#if refreshWSDL20141230
        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment[] GetGoodsItemCommodityDMExtensionsValuationDeductionAdjustment(List<SupplierInvoiceItemsModificationPM> supplierInvoiceItemsModificationsPM)
         {
             var ValuationDeductionAdjustmentList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment>();
             for (int itemsModificationsSeq = 0; itemsModificationsSeq < supplierInvoiceItemsModificationsPM.Count(); itemsModificationsSeq++)
             {
                 var supplierInvoiceItemsModification = supplierInvoiceItemsModificationsPM[itemsModificationsSeq];
                 var ValuationDeductionAdjustment = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment();
                 ValuationDeductionAdjustment.ChargesTypeCode = new CustomsValuationChargesTypeCodeType() { Value = supplierInvoiceItemsModification.TypeCode };
                 if (supplierInvoiceItemsModification.Amount.HasValue)
                 {
                     ValuationDeductionAdjustment.DeductAmount = SetAmountTypeValue<DutyTaxFeeDeductAmountType>(supplierInvoiceItemsModification.CurrencyTypeCode, supplierInvoiceItemsModification.Amount.Value);
                 }
                 ValuationDeductionAdjustmentList.Add(ValuationDeductionAdjustment);
             }
             return ValuationDeductionAdjustmentList.ToArray();
         }
#endif


        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensions GetDMExtensionsGoodsItem(SupplierInvoiceItemPM supplierInvoiceItemPM, SupplierInvoicePM supplierInvoicePM)
        {
            var DMExtensions = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensions();
            var declarationGoodsItemAmountList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmount>();
            string cur = supplierInvoicePM.InvoiceCurrencyTypeCode;

            if (supplierInvoiceItemPM.ItemPrice.HasValue)
            {
                if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.ItemPriceCurrencyCode))
                {
                    declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.ItemPrice.Value, "1", supplierInvoiceItemPM.ItemPriceCurrencyCode));
                }
                else if (!String.IsNullOrWhiteSpace(cur))
                {
                    declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.ItemPrice.Value, "1", cur));
                }
            }
            if (supplierInvoiceItemPM.NonCustomsItemPrice.HasValue && supplierInvoiceItemPM.NonCustomsItemPrice != decimal.Zero && !String.IsNullOrWhiteSpace(supplierInvoiceItemPM.NonCustomsItemPriceCurCode))//17997
            {
                //if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.NonCustomsItemPriceCurCode))
                //{
                declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.NonCustomsItemPrice.Value, "11", supplierInvoiceItemPM.NonCustomsItemPriceCurCode));
                //}
                //else if (!String.IsNullOrWhiteSpace(cur))
                //{
                //declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.NonCustomsItemPrice.Value, "11", cur));
                //}
            }
            if (supplierInvoiceItemPM.WholeSaleItemPrice.HasValue && supplierInvoiceItemPM.WholeSaleItemPrice != decimal.Zero && !String.IsNullOrWhiteSpace(supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode))//17997
            {

                //if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode))
                //{
                declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.WholeSaleItemPrice.Value, "5", supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode));
                //}
                //else if (!String.IsNullOrWhiteSpace(cur))
                //{
                //declarationGoodsItemAmountList.Add(GetDeclarationGoodsItemAmount(supplierInvoiceItemPM.WholeSaleItemPrice.Value, "5", cur));
                //}
            }
            DMExtensions.GoodsItemAmount = declarationGoodsItemAmountList.ToArray();
            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.CustomsBookTypeCode))
            {
                DMExtensions.CustomsBookType = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsCustomsBookType() { Value = supplierInvoiceItemPM.CustomsBookTypeCode };
            }
            else
            {
                DMExtensions.CustomsBookType = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsCustomsBookType() { Value = "1" }; // hard coded
            }
            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.TaxExemptCode))
            {
                if (supplierInvoiceItemPM.TaxExemptCode.Length == 12)
                {
                    supplierInvoiceItemPM.TaxExemptCode = supplierInvoiceItemPM.TaxExemptCode.Insert(11, "/");
                }
                if (supplierInvoiceItemPM.TaxExemptCode.Length == 11)
                {
                    supplierInvoiceItemPM.TaxExemptCode = supplierInvoiceItemPM.TaxExemptCode.Insert(10, "/");
                }
                DMExtensions.TaxExemptCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsTaxExemptCode() { Value = supplierInvoiceItemPM.TaxExemptCode };
                //SetCodeTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsTaxExemptCode>(String.IsNullOrWhiteSpace(supplierInvoiceItemPM.TaxExemptCode) ? "1" : supplierInvoiceItemPM.TaxExemptCode)
            }
            if (supplierInvoiceItemPM.OptionalTamaPercentage.HasValue)
            {
                DMExtensions.OptionalTama = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsOptionalTama() { Value = supplierInvoiceItemPM.OptionalTamaPercentage.Value };
            }
            DMExtensions.IsUsed = false; // MUST // hard coded
            DMExtensions.IsUsedSpecified = true;
            DMExtensions.Vehicle = GetDeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification(supplierInvoiceItemPM.SupplierInvoiceItemVehicles); // Mirit 16/08/15 Task 15960
            DMExtensions.SalesTaxExemptionType = new TaxExemptCodeType() { Value = supplierInvoiceItemPM.SalesTaxExemptionTypeCode };
            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemPM.PreferenceDocumentNumber)) // moran 9.3.15 - Task 11774
            {
                DMExtensions.PreferenceDocumentNumber = SetIDTypeValue<PreferenceDocumentNumberType>(supplierInvoiceItemPM.PreferenceDocumentNumber);
            }
            DMExtensions.IsUsed = supplierInvoiceItemPM.IsUsed; // moran 15.5.16 - Task 20573

            //#if itzik20141007 + Yuval Chalup 28.12.2015 TASK-19350 (Remove remark)
            DMExtensions.InvoiceLineNumbers = supplierInvoiceItemPM.ActualInvoiceLines;
            //#endif

            //<--- Yuval Chalup 15.11.2016 TASK-23994
            if (supplierInvoiceItemPM.DeferredCustomsTax.HasValue)
            {
                DMExtensions.DeferredCustomsTax = supplierInvoiceItemPM.DeferredCustomsTax;
                DMExtensions.DeferredCustomsTaxSpecified = true;
            }
            if (supplierInvoiceItemPM.DeferredPurchaseTax.HasValue)
            {
                DMExtensions.DeferredPurchaseTax = supplierInvoiceItemPM.DeferredPurchaseTax;
                DMExtensions.DeferredPurchaseTaxSpecified = true;
            }
            //Yuval Chalup 15.11.2016 TASK-23994 --->

            return DMExtensions;
        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification[] GetDeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification(List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclesList)
        {
            if (supplierInvoiceItemVehiclesList == null || supplierInvoiceItemVehiclesList.Count < 1)
            {
                return null;
            }

            List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification> mySupplierInvoiceItemVehiclesList = new List<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification>();
            foreach (var supplierInvoiceItemVehicleItem in supplierInvoiceItemVehiclesList)
            {
                if (!supplierInvoiceItemVehicleItem.ExcludeFromInterface)
                {
                    var vehicleDetails = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification();
                    vehicleDetails.ID = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationID();
                    vehicleDetails.IDTypeCode = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationIDTypeCode();
                    if (!string.IsNullOrWhiteSpace(supplierInvoiceItemVehicleItem.RichbitFileNumber))
                    {
                        vehicleDetails.ID.Value = supplierInvoiceItemVehicleItem.RichbitFileNumber;
                        vehicleDetails.IDTypeCode.Value = supplierInvoiceItemVehicleItem.VehicleTypeCode; // "ZZZ"; // moran 28.1.16 - Bug 19966 - change to take from DB instead of constant
                    }
                    else if (!string.IsNullOrWhiteSpace(supplierInvoiceItemVehicleItem.VehicleChassisNumber))
                    {
                        vehicleDetails.ID.Value = supplierInvoiceItemVehicleItem.VehicleChassisNumber;
                        vehicleDetails.IDTypeCode.Value = supplierInvoiceItemVehicleItem.VehicleTypeCode; // "CN"; // moran 28.1.16 - Bug 19966 - change to take from DB instead of constant
                    }

                    mySupplierInvoiceItemVehiclesList.Add(vehicleDetails);
                }
            }

            return mySupplierInvoiceItemVehiclesList.ToArray(); ;


        }

        private DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmount GetDeclarationGoodsItemAmount(decimal ItemPricePM, string ItemPriceTypePM, string ItemPriceCurrencyPM)
        {
            var declarationGoodsItemAmount = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmount();
            declarationGoodsItemAmount.CustomsValueAmount = SetAmountTypeValue<DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmountCustomsValueAmount>(ItemPriceCurrencyPM, ItemPricePM);
            //declarationGoodsItemAmount.CustomsValueAmount = new DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsGoodsItemAmountCustomsValueAmount()
            //{
            //    Value =ItemPricePM,
            //    currencyIDSpecified=true,
            //    currencyID = entityPM.USD
            // };
            declarationGoodsItemAmount.AmountType = SetCodeTypeValue<CodeAmountType>(ItemPriceTypePM);

            // declarationGoodsItemAmount.AmountType = new CodeAmountType()
            // {
            //     Value = ItemPriceTypePM
            // };
            return declarationGoodsItemAmount;
        }


        private List<DeclarationGoodsShipmentConsignment> GetDeclarationConsignment(DeclarationPM declarationPM)
        {
            var declarationConsignmentList = new List<DeclarationGoodsShipmentConsignment>();
            for (int consignmentSeq = 0; consignmentSeq < declarationPM.Consignments.Count(); consignmentSeq++)
            {
                var consignmentPM = declarationPM.Consignments[consignmentSeq];
                var declarationConsignment = new DeclarationGoodsShipmentConsignment();
                declarationConsignment.SequenceNumeric = consignmentSeq + 1;
                declarationConsignment.SequenceNumericSpecified = true;

                ///if (!String.IsNullOrWhiteSpace(consignmentPM.ManifestNumber) || !String.IsNullOrWhiteSpace(consignmentPM.CargoTypeCode))
                //{
                declarationConsignment.TransportContractDocument = new DeclarationGoodsShipmentConsignmentTransportContractDocument()
                {
                    TypeCode = SetCodeTypeValue<TransportContractDocumentTypeCodeType>(consignmentPM.CargoTypeCode), //new TransportContractDocumentTypeCodeType() {Value =  "IL1"}, //hardcoded ask yaron + consignmentPM.CargoTypeCode },
                    IssueDateTime = consignmentPM.ManifestDate.HasValue ? DataTypeConvertorUtil.Convert(consignmentPM.ManifestDate.Value) : null, // DataTypeConvertorUtil.Convert(declarationPM.IssueDateTime.Value), // hard coded
                    ID = SetIDTypeValue<TransportContractDocumentIdentificationIDType>(consignmentPM.ManifestNumber), // new TransportContractDocumentIdentificationIDType() { Value = consignmentPM.ManifestNumber },
                    //ID = SetIDTypeValue<TransportContractDocumentIdentificationIDType>("123456"), // new TransportContractDocumentIdentificationIDType() { Value = consignmentPM.ManifestNumber }, HARD CODED
                    DMExtensions = new DeclarationGoodsShipmentConsignmentTransportContractDocumentDMExtensions()
                    {
                        SecondCargoID = SetIDTypeValue<SecondCargoIDType>(consignmentPM.SecondCargoID), // new SecondCargoIDType() { Value = consignmentPM.SecondCargoID },
                        ThirdCargoID = SetIDTypeValue<ThirdCargoIDType>(consignmentPM.ThirdCargoID) // new ThirdCargoIDType() { Value = consignmentPM.ThirdCargoID }
                    }
                };
                if (consignmentPM.CargoTypeCode == "17" && !string.IsNullOrWhiteSpace(consignmentPM.ThirdCargoID))
                {
                    var thirdCargoID = consignmentPM.ThirdCargoID;
                    if (consignmentPM.ThirdCargoID.Length >= 8)
                    {
                        thirdCargoID = consignmentPM.ThirdCargoID.Substring(0, 4) + consignmentPM.ThirdCargoID.Substring(6, 2);
                    }
                    declarationConsignment.TransportContractDocument.DMExtensions.ThirdCargoID = SetIDTypeValue<ThirdCargoIDType>(thirdCargoID);
                }
                //}

                /*  if (consignmentPM.ManifestDate.HasValue)
                  {
                      declarationConsignment.TransportContractDocument.IssueDateTime = DataTypeConvertorUtil.Convert(consignmentPM.ManifestDate.Value);
                  } 
                
                  if (consignmentPM.UnloadDate.HasValue)
                  {
                      declarationConsignment.UnloadingLocation.ArrivalDateTime = DataTypeConvertorUtil.Convert(consignmentPM.UnloadDate.Value);
                  }*/
                if (this.IsSendWithManifest)
                {
                    declarationConsignment.UnloadingLocation = new DeclarationGoodsShipmentConsignmentUnloadingLocation()
                    {
                        ID = SetIDTypeValue<UnloadingLocationIdentificationIDType>(consignmentPM.UnloadPortCode), // new UnloadingLocationIdentificationIDType() { Value = consignmentPM.UnloadPortCode },
                        ArrivalDateTime = consignmentPM.UnloadDate.HasValue ? DataTypeConvertorUtil.Convert(consignmentPM.UnloadDate.Value) : null,
                    };
                }
				if (this.IsSendWithManifest) {
                   declarationConsignment.LoadingLocation = new DeclarationGoodsShipmentConsignmentLoadingLocation()
                   {
                       ID = SetIDTypeValue<LoadingLocationIdentificationIDType>(consignmentPM.LoadingPortCode) // new LoadingLocationIdentificationIDType() { Value = consignmentPM.LoadingPortCode }
                   };
                }
                declarationConsignment.DMExtensions = GetDMExtensionsConsignment(consignmentPM);


                declarationConsignmentList.Add(declarationConsignment);
            }

            return declarationConsignmentList;
        }

        private DeclarationGoodsShipmentConsignmentDMExtensions GetDMExtensionsConsignment(ConsignmentPM consignmentPM)
        {
            var DMExtensions = new DeclarationGoodsShipmentConsignmentDMExtensions();
          
                DMExtensions.CargoDescription = new DeclarationGoodsShipmentConsignmentDMExtensionsCargoDescription() { Value = consignmentPM.CargoDescription };
           
                //DMExtensions.LastReleaseFromWarehousInd = new LastReleaseFromWarehousIndType() { Value = consignmentPM.IsLastReleaseFromWarehous };
            if (consignmentPM.IsLastReleaseFromWarehous == "T") // temporary treatment - Task 9683
            {
                //mohammad temp treatment due to the change of task 9684
                DMExtensions.LastReleaseFromWarehousInd = new LastReleaseFromWarehousIndType() { Value = true };//consignmentPM.IsLastReleaseFromWarehous 
            }
            else if (consignmentPM.IsLastReleaseFromWarehous == "F") // moran 9.3.15 - Task 11761 
            {
                DMExtensions.LastReleaseFromWarehousInd = new LastReleaseFromWarehousIndType() { Value = false };
            }
            if (this.IsSendWithManifest)
            {
                DMExtensions.ExportationCountryCode = new DeclarationGoodsShipmentConsignmentDMExtensionsExportationCountryCode() { Value = consignmentPM.OriginCountryCode };
            }
            var registeredFacilitylist = new List<DeclarationGoodsShipmentConsignmentDMExtensionsRegisteredFacility>();
            int seqnum = 0;

            if (!String.IsNullOrWhiteSpace(consignmentPM.StorageSiteCode))
            {
                seqnum++;
                registeredFacilitylist.Add(GetRegisteredFacility(consignmentPM.StorageSiteCode, "004", seqnum));
            }
            if (!String.IsNullOrWhiteSpace(consignmentPM.ReceiverWarehouseCode))
            {
                seqnum++;
                registeredFacilitylist.Add(GetRegisteredFacility(consignmentPM.ReceiverWarehouseCode, "003", seqnum));
            }
            if (consignmentPM.ConsignmentInternalTransitions != null)
            {
                if (consignmentPM.ConsignmentInternalTransitions.FirstOrDefault() != null)
                {
                    if (!String.IsNullOrWhiteSpace(consignmentPM.ConsignmentInternalTransitions.FirstOrDefault().SiteCode))
                    {
                        seqnum++;
                        registeredFacilitylist.Add(GetRegisteredFacility(consignmentPM.ConsignmentInternalTransitions.FirstOrDefault().SiteCode, "005", seqnum));
                    }
                }
            }
            if (seqnum > 0)
            {
                DMExtensions.RegisteredFacility = registeredFacilitylist.ToArray();
            }

            DMExtensions.PackagesMeasure = GetDeclarationConsignmentPackages(consignmentPM).ToArray();

            return DMExtensions;
        }

        private DeclarationGoodsShipmentConsignmentDMExtensionsRegisteredFacility GetRegisteredFacility(string p1, string p2, int seqnum)
        {
            var registeredFacility = new DeclarationGoodsShipmentConsignmentDMExtensionsRegisteredFacility();
            //registeredFacility.ID = SetIDTypeValue<DeclarationGoodsShipmentConsignmentDMExtensionsRegisteredFacilityID>(p1);
            registeredFacility.ID = new DeclarationGoodsShipmentConsignmentDMExtensionsRegisteredFacilityID() { Value = p1 };
            registeredFacility.FacilityType = new DeclarationGoodsShipmentConsignmentDMExtensionsRegisteredFacilityFacilityType() { Value = p2 };
            registeredFacility.SequenceNumeric = seqnum;
            registeredFacility.SequenceNumericSpecified = true;

            return registeredFacility;
        }


        private List<DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasure> GetDeclarationConsignmentPackages(ConsignmentPM consignmentPM)
        {
            /*//<--- HARD CODED
            var declarationConsignmentPackageList = new List<DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasure>();

            var declarationConsignmentPackage = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasure();
            declarationConsignmentPackage.SequenceNumeric = 1;
            declarationConsignmentPackage.SequenceNumericSpecified = true;
            declarationConsignmentPackage.PackageMeasureQualifier = new PackageMeasureQualifierType() { Value = "2"}; //HARDCODED
            declarationConsignmentPackage.TotalPackageQuantity = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureTotalPackageQuantity() { Value = 100.00M };
            declarationConsignmentPackage.GrossMassMeasure = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureGrossMassMeasure()
            {
                Value = 10.00M,
                unitCode = MeasurementUnitCommonCodeContentType.KGM,
                unitCodeSpecified = true
            };
            declarationConsignmentPackage.TypeCode = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureTypeCode() { Value = "UN" };
            declarationConsignmentPackage.MarksNumbers = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureMarksNumbers() { Value = "MarksNumbers" };




            declarationConsignmentPackageList.Add(declarationConsignmentPackage);

            return declarationConsignmentPackageList;
            //<--- HARD CODED */

            var declarationConsignmentPackageList = new List<DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasure>();

            for (int consignmentPackageSeq = 0; consignmentPackageSeq < consignmentPM.ConsignmentPackages.Count(); consignmentPackageSeq++)
            {
                var consignmentPackagePM = consignmentPM.ConsignmentPackages[consignmentPackageSeq];

                var declarationConsignmentPackage = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasure();
                declarationConsignmentPackage.SequenceNumeric = consignmentPackageSeq + 1;
                declarationConsignmentPackage.SequenceNumericSpecified = true;
                //declarationConsignmentPackage.PackageMeasureQualifier = new PackageMeasureQualifierType() { Value = consignmentPackagePM.PackageMeasureQualifierCode };
                declarationConsignmentPackage.PackageMeasureQualifier = SetCodeTypeValue<PackageMeasureQualifierType>(consignmentPackagePM.PackageMeasureQualifierCode);
                if (consignmentPackagePM.PackageQuantity.HasValue)
                {// moran 5.1.16 - Task 19549 - change to EA hard coded instead of ""
                    declarationConsignmentPackage.TotalPackageQuantity = SetQuantityTypeValue<DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureTotalPackageQuantity>(consignmentPackagePM.PackageQuantityTypeCode, consignmentPackagePM.PackageQuantity.Value); // hard coded null - mapping missing  // MeasurementUnitCommonCodeContentType.EA.ToString()
                }
                //declarationConsignmentPackage.TotalPackageQuantity = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureTotalPackageQuantity() { Value = (Decimal)consignmentPackagePM.PackageQuantity };
                if (consignmentPackagePM.GrossMassMeasure.HasValue)
                {
                    declarationConsignmentPackage.GrossMassMeasure = SetMeasureTypeValue<DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureGrossMassMeasure>(consignmentPackagePM.GrossMassMeasureTypeCode, consignmentPackagePM.GrossMassMeasure.Value);
                }

                declarationConsignmentPackage.TypeCode = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureTypeCode() { Value = consignmentPackagePM.PackageTypeCode };
                declarationConsignmentPackage.MarksNumbers = new DeclarationGoodsShipmentConsignmentDMExtensionsPackagesMeasureMarksNumbers() { Value = consignmentPackagePM.MarksNumbers };

                declarationConsignmentPackageList.Add(declarationConsignmentPackage);
            }

            return declarationConsignmentPackageList;
        }


        private DeclarationImporter GetDeclarationImporterRole6(DeclarationPM declarationPM)
        {

            //<--- Yuval Chalup 15.11.2016 TASK-24438 - CHANGED FROM:
            //var declarationImporter = new DeclarationImporter();
            // Task 6440 - add ImporterCode fields check
            //if (!String.IsNullOrWhiteSpace(declarationPM.EntitleImporterId))
            //{
            //   declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(GetImporterCode(declarationPM.EntitleImporterId), declarationPM.EntitleImporterTypeCode); // new ImporterIdentificationIDType() // moran 24.3.15 - Task 11461 - add declarationPM.EntitleImporterTypeCode
            //}
            //else
            //{
            //    declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(declarationPM.EntitleImporterCode, declarationPM.EntitleImporterTypeCode); // moran 24.3.15 - Task 11461 - add declarationPM.EntitleImporterTypeCode // Mirit 15/11/15 - Change to TransferImporterCode
            //}
            //TO:
            var ImporterId = ImportersCheck("זכאי", declarationPM.EntitleImporterCode, declarationPM.EntitleImporterId, declarationPM.EntitleImporterTypeCode, declarationPM.EntitleImporterName, declarationPM.EntitleImporterAddress, declarationPM.EntitlePassportNumber, declarationPM.EntitleImporterCountryCode);
            var declarationImporter = new DeclarationImporter();
            declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(ImporterId, declarationPM.EntitleImporterTypeCode);
            //Yuval Chalup 15.11.2016 TASK-24438 --->
            declarationImporter.DMExtensions = new DeclarationImporterDMExtensions()
            {
                Address = declarationPM.EntitleImporterAddress,
                Name = declarationPM.EntitleImporterName,
                EntitlementTypeCode = new EntitlementTypeCodeType()
                {
                    Value = declarationPM.ImporterEntitlementTypeCode
                },
                RoleCode = new DeclarationImporterDMExtensionsRoleCode()
                {
                    Value = "6"
                }
            };
            if (declarationPM.EntitleImporterTypeCode == "2" || declarationPM.EntitleImporterTypeCode == "3")
            {
                declarationImporter.DMExtensions.IssueLocation = new DeclarationImporterDMExtensionsIssueLocation() { Value = declarationPM.EntitleImporterCountryCode };
            }
            return declarationImporter;
        }

        private object GetDeclarationImporter()
        {
            throw new NotImplementedException();
        }


        private DeclarationImporter GetDeclarationImporterRole5(DeclarationPM declarationPM)
        {
            //<--- Yuval Chalup 15.11.2016 TASK-24438 - CHANGED FROM:
            //var declarationImporter = new DeclarationImporter();
            // Task 6440 - add ImporterCode fields check
            //if (!String.IsNullOrWhiteSpace(declarationPM.TransferImporterId))
            //{
            //    declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(GetImporterCode(declarationPM.TransferImporterId), declarationPM.TransferImporterTypeCode); // new ImporterIdentificationIDType()  // moran 24.3.15 - Task 11461 - add declarationPM.ImporterTypeCode
            //}
            // else
            //{
            //    declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(declarationPM.TransferImporterCode, declarationPM.TransferImporterTypeCode); // moran 24.3.15 - Task 11461 - add declarationPM.ImporterTypeCode // Mirit 15/11/15 - Change to TransferImporterCode
            //}
            //TO:
            var ImporterId = ImportersCheck("מעביר", declarationPM.TransferImporterCode, declarationPM.TransferImporterId, declarationPM.TransferImporterTypeCode, declarationPM.TransferImporterName, declarationPM.TransferImporterAddress, declarationPM.TransferPassportNumber, declarationPM.TransferImporterCountryCode);
            var declarationImporter = new DeclarationImporter();
            declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(ImporterId, declarationPM.TransferImporterTypeCode);
            //Yuval Chalup 15.11.2016 TASK-24438 --->

            declarationImporter.DMExtensions = new DeclarationImporterDMExtensions()
            {
                Address = declarationPM.TransferImporterAddress,
                Name = declarationPM.TransferImporterName,
                EntitlementTypeCode = new EntitlementTypeCodeType()
                {
                    Value = declarationPM.TransImporterEntitleTypeCode
                },
                RoleCode = new DeclarationImporterDMExtensionsRoleCode()
                {
                    Value = "5"
                }
            };
            if (declarationPM.TransferImporterTypeCode == "2" || declarationPM.TransferImporterTypeCode == "3")
            {
                declarationImporter.DMExtensions.IssueLocation = new DeclarationImporterDMExtensionsIssueLocation() { Value = declarationPM.TransferImporterCountryCode };
            }
            return declarationImporter;
        }


        private DeclarationImporter GetDeclarationImporterRole4(DeclarationPM declarationPM)
        {
            //<--- Yuval Chalup 15.11.2016 TASK-24438 - CHANGED FROM:
            //var ImporterId = GetImporterCode(declarationPM.ImporterId);
            //if (String.IsNullOrWhiteSpace(ImporterId)) // Task 6440 - add ImporterCode fields check
            //{
            //    ImporterId = GetImporterCode(declarationPM.ImporterCode);
            //}
            //if (String.IsNullOrWhiteSpace(ImporterId))
            //{
            //    throw new BusinessErrorException("Importer is empty");
            //}
            //TO:
            //ImporterId = GetImporterCode(declarationPM.ImporterId);
            var ImporterId = ImportersCheck("", declarationPM.ImporterCode, declarationPM.ImporterId, declarationPM.ImporterTypeCode, declarationPM.ImporterName, declarationPM.ImporterAddress, declarationPM.ImporterPassportNumber, declarationPM.ImporterPassCountryCode);
            //if (!string.IsNullOrWhiteSpace(errorMessage))
            //{
            //    throw new BusinessErrorException(errorMessage);
            //}
            //Yuval Chalup 15.11.2016 TASK-24438 --->

            var declarationImporter = new DeclarationImporter();
            declarationImporter.ID = SetIDTypeValue<ImporterIdentificationIDType>(ImporterId, declarationPM.ImporterTypeCode); // new ImporterIdentificationIDType() // moran 24.3.15 - Task 11461 - use declarationPM.ImporterTypeCode instead of hard coded "1"

            string importerAddress = null;
            string importerName = null;
            if (string.IsNullOrWhiteSpace(ImporterId))//task 45505
            {
                if (!string.IsNullOrWhiteSpace(declarationPM.ImporterAddress)) { importerAddress = declarationPM.ImporterAddress; }
                if (!string.IsNullOrWhiteSpace(declarationPM.ImporterName)) { importerName = declarationPM.ImporterName; }
            }
            declarationImporter.DMExtensions = new DeclarationImporterDMExtensions()
            {
                //                Address = declarationPM.ImporterAddress,
                //Name = declarationPM.ImporterName,
                Address = importerAddress,//task 45505
                Name = importerName,
                EntitlementTypeCode = new EntitlementTypeCodeType()
                {
                    Value = declarationPM.MainImporterEntitlemntTypeCode
                },
                RoleCode = new DeclarationImporterDMExtensionsRoleCode()
                {
                    Value = "4"
                }
            };
            if (declarationPM.ImporterTypeCode == "2" || declarationPM.ImporterTypeCode == "3")
            {
                declarationImporter.DMExtensions.IssueLocation = new DeclarationImporterDMExtensionsIssueLocation() { Value = declarationPM.ImporterPassCountryCode };
            }

            return declarationImporter;
        }

        public string ImportersCheck(string importerField, string importerCode, string importerId, string importerType,
    string importerName, string importerAddress, string importerPassportNumber, string importerPassCountryCode)
        {
            var errorMessage = "";
            var importer = "";
            if (_DeclarationPM != null)
            {
                switch (importerType)
                {
                    case "1":
                        if (string.IsNullOrWhiteSpace(importerId))
                        {
                            if (!string.IsNullOrWhiteSpace(importerCode))
                            {
                                importer = importerCode;
                            }
                            else
                            {
                                //Check if ImporterName & ImporterAddrress has value
                                if (string.IsNullOrWhiteSpace(importerName) && string.IsNullOrWhiteSpace(importerAddress))
                                {
                                    errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                                }
                            }
                        }
                        else
                        {
                            var queryService = new ClientQueryService(_context);
                            var importerPM = queryService.GetSingle(importerId, true, false);
                            if (importerPM == null) return "";
                            importer = importerPM.Code;
                        }
                        break;
                    case "2":
                    case "3":
                        {
                            //Check That Both PassportCountry & PassportNumber has values
                            if (string.IsNullOrWhiteSpace(importerPassportNumber) || string.IsNullOrWhiteSpace(importerPassCountryCode))
                            {
                                errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                            }
                            else
                            {
                                importer = importerPassportNumber;
                            }
                            break;
                        }
                    default:
                        {
                            if (string.IsNullOrWhiteSpace(importerId))
                            {
                                if (!string.IsNullOrWhiteSpace(importerCode))
                                {
                                    errorMessage = "יש לשלוף לקוח מהמכס עבור יבואן " + importerField + " לפני שליחה";
                                }
                                else
                                {
                                    errorMessage = "מספר יבואן " + importerField + " הוא שדה חובה";
                                }
                            }
                            break;

                        }
                }
            }
            if (_DeclarationPM.IsCourierDeclaration && IsFromOpenNewAmendment)
            {
                errorMessage = "";
                //var courierMasterPM = courierMasterQueryService.GetByDeclarationId(_DeclarationPM.Id, tenant);
                //var repository = new CardRepository(tenant);
                //var myCard = repository.GetSingleCard(courierMasterPM.IntegratorCode, tenant);
                //if (myCard != null && !String.IsNullOrWhiteSpace(myCard.Code))
                //{
                //    string defValue = GetDefault("ISRAEL", "CGO_GDPR_PRIVAC", "NON", myCard.Code, tenant);
                //    if (defValue == "Y")
                //    { }
                //}
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new BusinessErrorException(errorMessage);
            }
            return (importer);
        }

        private string GetVendorNumber(string vendorId)
        {
            var queryService = new CustomsVendorQueryService(_context);
            var vendorPM = queryService.GetSingle(vendorId, true, false);
            if (vendorPM == null) return "";
            return vendorPM.VendorNumber;

        }

        private string GetImporterCode(string importerId)
        {
            var queryService = new ClientQueryService(_context);
            var importerPM = queryService.GetSingle(importerId, true, false);
            if (importerPM == null) return "";

            if (!string.IsNullOrWhiteSpace(importerPM.PassportNumber) && !string.IsNullOrWhiteSpace(importerPM.PassportCountryCode))
            {
                return importerPM.PassportNumber;
            }
            return importerPM.Code;

        }


    }
}
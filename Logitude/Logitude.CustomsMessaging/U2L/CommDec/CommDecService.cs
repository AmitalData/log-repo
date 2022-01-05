using Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.CustomsMessaging;
using Logitude.CustomsMessaging.MessagingServices;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Server.Tools.Utils;

namespace Logitude.CustomsMessaging.U2L.CommDec
{
    public class CommDecService : UnifreightGenericService
    {
        private LOGICOMMDEC _LOGICOMMDEC;
        private LogitudeCommDecFile _LogitudeCommDecFile;
        private SupplierInvoicePM _MySupplierInvoicePM;
        private CourierMasterPM _CourierMasterPM;
        private CourierDeclarationPM _CourierDeclarationPM;
        private ICustomContext _context;
        private LOGICUSTFILE _LOGICUSTFILE;
        private LogitudeCustomsFile _AmitalCustomsFile;
        private DeclarationCourierStatusPM currentDeclarationCourierStatusPM;

        private AmitalContext amitalContext;

        public string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.CommDec.CommDecService.Upsert()";
        private Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICE _INVOICE;
        private DeclarationPM _MyDeclarationPM;



        //private DeclarationPM _MyEntryDeclarationPM;
        private Stopwatch _Stopwatch;
        private bool _IsBuildItemsUnit = false;
        private bool _IsNewDeclaration = false;
        public bool IsAutonomy = false;
        private decimal _SupplierInvoiceAmount;
        private string mode;
        private bool IsProcedureCurrentCodeChanged = false;

        public CommDecService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        int _tenant = 0;

        public override void ProccessGenericRequest(
              string xmlLOGICOMMDEC,
              ref string MoreParams,
              out string MessageOut)

        {
            string customFileNo = "";
            string decId = "";
            string courierMasterID = "";
            MessageOut = "";
            _tenant = ResolvedTenant();
            var user = AuthenticationUtil.ResolveUserId(_tenant);
            string defValue = GetDefault("ISRAEL", "CGG_OPN_DEC_MET", "NON", "NON", _tenant);

            //   if (!string.IsNullOrEmpty(defValue) && defValue == "B")
            //  {
            AppendLogLine("!string.IsNullOrEmpty(defValue) && defValue=='B'");

            var messagingService = new DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceMessagingService();
            DCAInUCUW2LRequestParams requestParams = new DCAInUCUW2LRequestParams()
            {
                LOGICOMMDEC = xmlLOGICOMMDEC,
                MoreParams = MoreParams,
                LoggingUserId = user,
                Tenant = _tenant
            };

            string message = messagingService.CreateCRS(_tenant, user, requestParams);
            AppendLogLine("message : " + message);

            if (message == "SUCCESS")
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            else
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.TecinicalFailure;


            // }
            // else
            //  {
            //      AppendLogLine("Default= WS'");

            //     ProccessGenericRequestReal(xmlLOGICOMMDEC, _tenant, user, ref MoreParams, out MessageOut, out customFileNo, out decId, out courierMasterID);
            //  }


        }
        //protected override int ResolvedTenant()
        //{
        //    return _tenant;
        //}
        public string _PBId;

      

        public static string GetGeneralLockKey(string courierMasterId)
        {
            return $"UCUDO:{courierMasterId}";
        }


        }

        private void CalcProcedureCurrentCode()
        {
            var originProcedureCurrentCode = this._MyDeclarationPM.ProcedureCurrentCode;
            if (currentDeclarationCourierStatusPM == null)
            {
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
                currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
            }

            if (this.IsAutonomy == true)
            {
                if (!String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterCode) && this._MyDeclarationPM.ImporterCode.Substring(0, 1) == "5")
                {
                    if (this.currentDeclarationCourierStatusPM != null && this.currentDeclarationCourierStatusPM.HighLowValue == "H")
                    {
                        this._MyDeclarationPM.ProcedureCurrentCode = "4000005";
                    }
                    else
                    {
                        this._MyDeclarationPM.ProcedureCurrentCode = "4000012";
                    }
                }
                else
                {
                    if (this.currentDeclarationCourierStatusPM != null && this.currentDeclarationCourierStatusPM.HighLowValue == "H")
                    {
                        this._MyDeclarationPM.ProcedureCurrentCode = "4000505";
                    }
                    else
                    {
                        this._MyDeclarationPM.ProcedureCurrentCode = "4000512";
                    }
                }
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterCode) && this._MyDeclarationPM.ImporterCode.Substring(0, 1) == "5")
                {
                    if (this.currentDeclarationCourierStatusPM != null && this.currentDeclarationCourierStatusPM.HighLowValue == "H")
                    {
                        this._MyDeclarationPM.ProcedureCurrentCode = "4000001";
                    }
                    else
                    {
                        this._MyDeclarationPM.ProcedureCurrentCode = "4000007";
                    }
                }
                else
                {
                    if (this.currentDeclarationCourierStatusPM != null && this.currentDeclarationCourierStatusPM.HighLowValue == "H")
                    {
                        this._MyDeclarationPM.ProcedureCurrentCode = "4000501";
                    }
                    else
                    {
                        this._MyDeclarationPM.ProcedureCurrentCode = "4000507";
                    }
                }
            }
            if (originProcedureCurrentCode != this._MyDeclarationPM.ProcedureCurrentCode)
            {
                this.IsProcedureCurrentCodeChanged = true;
                foreach (SupplierInvoicePM invoice in this._MyDeclarationPM.SupplierInvoices)
                {
                    if (invoice.ChangeSetOp != ChangeSetOperation.Update && invoice.ChangeSetOp != ChangeSetOperation.Insert) invoice.ChangeSetOp = ChangeSetOperation.Update;
                    foreach (SupplierInvoiceItemPM item in invoice.SupplierInvoiceItems)
                    {
                        if (item.ChangeSetOp != ChangeSetOperation.Update && item.ChangeSetOp != ChangeSetOperation.Insert) item.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
            }
        }

        private void CalcIsAutonomy()
        {
            //if (String.IsNullOrWhiteSpace(this._LogitudeCommDecFile.IsAutonomy) && !String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterCode) && this._MyDeclarationPM.ImporterCode.Substring(0, 1) == "8")
            //{
            if (!String.IsNullOrWhiteSpace(this._LogitudeCommDecFile.IsAutonomy) && this._LogitudeCommDecFile.IsAutonomy.ToLower().Substring(0, 1) == "y")
            {
                this.IsAutonomy = true;
                return;
            }

            var palestinianCode = !String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterCode) ? this._MyDeclarationPM.ImporterCode : !String.IsNullOrWhiteSpace(this._MyDeclarationPM.PalestinianCode) ? this._MyDeclarationPM.PalestinianCode : null;

            CustomsAutonomyKeywordQueryService customsAutonomyKeywordQueryService = new CustomsAutonomyKeywordQueryService(_context);

            var casualImportelTel = _AmitalCustomsFile.CasualImportelTel;

            if (!String.IsNullOrWhiteSpace(casualImportelTel)) casualImportelTel = string.Concat(casualImportelTel.Where(c => !char.IsWhiteSpace(c)));

            //if (!String.IsNullOrWhiteSpace(casualImportelTel)) casualImportelTel = _AmitalCustomsFile.CasualImportelTel.TrimStart(new Char[] { '0' });

            if (!string.IsNullOrWhiteSpace(casualImportelTel) && casualImportelTel.StartsWith("5"))//If the number start with 5 add 0 
            {
                casualImportelTel = "0" + casualImportelTel;//Task 139114: בדיקת חוקיות של הזנת מספר טלפון והעלאת PENDING 903- טלפון לא חוקי + טיפול נוסף
            }

            if (customsAutonomyKeywordQueryService.CheckIfsAutonomy(_AmitalCustomsFile.CasualImporterCity, casualImportelTel, palestinianCode, _MyDeclarationPM.Tenant))
            {
                this.IsAutonomy = true;
                return;
            }

            //if (!String.IsNullOrWhiteSpace(this._LogitudeCommDecFile.IsAutonomy) && this._LogitudeCommDecFile.IsAutonomy.ToLower().Substring(0, 1) == "y")
            //{
            //    this.IsAutonomy = true;
            //}
        }

        private void UpdateNoIdUnder150()
        {
            if (!String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterCode))
            {
                if (_CourierMasterPM != null)
                {
                    Card myCard = null;
                    var repository = new CardRepository(_tenant);
                    myCard = repository.GetSingleCard(_CourierMasterPM.IntegratorCode, _tenant);
                    if (myCard != null && !String.IsNullOrWhiteSpace(myCard.Code))
                    {
                        string defValue = GetDefault("ISRAEL", "CGO_NO_ID_150", "NON", myCard.Code, _tenant);
                        if (defValue == "Y")
                        {
                            if (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault() != null && (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().ChangeSetOp == ChangeSetOperation.Insert || (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().ChangeSetOp != ChangeSetOperation.Insert && this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().InvoiceAmount.GetValueOrDefault() != this._SupplierInvoiceAmount)))
                            {
                                ICustomContext dbContext = CustomContext.GetContext(_tenant);
                                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), _tenant);
                                declarationUpdateService.Update(this._MyDeclarationPM, true);
                                _context = CustomContext.GetContext(_tenant);
                                var myQueryService = new DeclarationQueryService(_context);
                                this._MyDeclarationPM = myQueryService.GetSingle(this._MyDeclarationPM.Id, true, false);
                                this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
                                currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
                            }
                            if (currentDeclarationCourierStatusPM != null && currentDeclarationCourierStatusPM.TotalInvoiceAmountInUSD != null && currentDeclarationCourierStatusPM.TotalInvoiceAmountInUSD < 150)
                            {
                                this._MyDeclarationPM.ImporterCode = null;
                                this._MyDeclarationPM.ImporterId = null;
                            }
                        }
                    }
                }
            }

            if (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault() != null && (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().ChangeSetOp == ChangeSetOperation.Insert || (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().ChangeSetOp != ChangeSetOperation.Insert && this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().InvoiceAmount.GetValueOrDefault() != this._SupplierInvoiceAmount)))

            {

                ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());

                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());

                declarationUpdateService.Update(this._MyDeclarationPM, true);

                _context = CustomContext.GetContext(ResolvedTenant());

                var myQueryService = new DeclarationQueryService(_context);

                this._MyDeclarationPM = myQueryService.GetSingle(this._MyDeclarationPM.Id, true, false);

                this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);

                currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);

            }


            if (currentDeclarationCourierStatusPM == null)

            {

                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);

                currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);

            }


            if (currentDeclarationCourierStatusPM != null && currentDeclarationCourierStatusPM.TotalInvoiceAmountInUSD != null && currentDeclarationCourierStatusPM.TotalInvoiceAmountInUSD <= 150)

            {

                if (!String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterCode))

                {

                    this._MyDeclarationPM.PalestinianCode = this._MyDeclarationPM.ImporterCode;

                    this._MyDeclarationPM.ImporterCode = null;

                }

                if (!String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterId))

                {

                    this._MyDeclarationPM.ImporterId = null;

                }

            }



        }


        private void CheckMasterToUpdate(string MoreParams, string Curruser)
        {
            string courier_id = null;
            if (!String.IsNullOrWhiteSpace(MoreParams))
            {
                AppendLogLine("MoreParams: " + MoreParams);
                var unifreightListsParams = UnifreightListsUtil.Deserialize(MoreParams);
                AppendLogLine("MoreParams after Deserialize: " + unifreightListsParams);
                courier_id = UnifreightListsUtil.GetValue(ref unifreightListsParams, "COURIER_ID");
                AppendLogLine("courier id param: " + courier_id);
            }

            //Get CourierMaster
            var myCourierMasterQueryService = new CourierMasterQueryService(_context);
            if (!String.IsNullOrWhiteSpace(courier_id))
            {
                _CourierMasterPM = myCourierMasterQueryService.GetSingle(courier_id, true, false);

                if (_CourierMasterPM != null)
                {
                    AppendLogLine("CourierMasterPM found for id: " + courier_id);
                }
                else
                {
                    AppendLogLine("CourierMasterPM not found for id: " + courier_id);
                }
            }
            else
            {
                AppendLogLine("CarrierPrefix: " + _LogitudeCommDecFile.CarrierPrefix);
                var airlineId = TranslateAirline(_LogitudeCommDecFile.CarrierPrefix);
                AppendLogLine("airlineId: " + airlineId);
                if (String.IsNullOrWhiteSpace(airlineId))
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "Airline Prefix " + _LogitudeCommDecFile.CarrierPrefix + " Doesn't exist";
                    AppendLogLine(MyGenericResponseObj.Message);
                }
                _CourierMasterPM = myCourierMasterQueryService.GetSingleByAirlineAWBs(airlineId, _LogitudeCommDecFile.HAWB, _LogitudeCommDecFile.MAWB, _tenant);
                if (_CourierMasterPM == null && !String.IsNullOrWhiteSpace(_LogitudeCommDecFile.HAWB))
                {
                    _CourierMasterPM = myCourierMasterQueryService.GetSingleByAirlineAWBs(airlineId, null, _LogitudeCommDecFile.MAWB, _tenant);
                }
                if (_CourierMasterPM != null)
                {
                    AppendLogLine("CourierMasterPM found for airlineId: " + airlineId + " HAWB: " + _LogitudeCommDecFile.HAWB + " MAWB: " + _LogitudeCommDecFile.MAWB);
                }
                else
                {
                    AppendLogLine("CourierMasterPM not found for airlineId: " + airlineId + " HAWB: " + _LogitudeCommDecFile.HAWB + " MAWB: " + _LogitudeCommDecFile.MAWB);
                }

            }
            if (_CourierMasterPM != null)
            {
                 var myCourierDeclarationQueryService = new CourierDeclarationQueryService(_context);
                var myCourierDeclarationUpdateService = new CourierDeclarationUpdateService(_context, new Dictionary<string, IContext>(), _tenant);
                _CourierDeclarationPM = myCourierDeclarationQueryService.GetSingle(_MyDeclarationPM.Id, _CourierMasterPM.Id, false, true);
                if (_CourierDeclarationPM == null)
                {
                    AppendLogLine("CourierDeclarationPM not found for DeclarationPM.Id: " + _MyDeclarationPM.Id + " CourierMasterPM.Id: " + _CourierMasterPM.Id);
                    if (!this._MyDeclarationPM.HatraDate.HasValue)
                    {
                        CourierDeclarationPM _CourierDeclarationPMPMDiferentMaster = myCourierDeclarationQueryService.GetCourierDeclarationByDeclarationId(_MyDeclarationPM.Id, _tenant);
                        if (_CourierDeclarationPMPMDiferentMaster != null)
                        {
                            AppendLogLine("try to delete CourierDeclaration with Diferent Master (id: " + _CourierDeclarationPMPMDiferentMaster.CourierMasterId + "  found for DeclarationPM.Id: " + _MyDeclarationPM.Id + " CourierMasterPM.Id: " + _CourierMasterPM.Id);
                            _CourierDeclarationPMPMDiferentMaster.ChangeSetOp = ChangeSetOperation.Delete;
                            try
                            {


                                myCourierDeclarationUpdateService.Update(_CourierDeclarationPMPMDiferentMaster, true);


                                if (false)
                                {


                                    CustomsRequestsSheetQueryService customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(_context);
                                    List<CustomsRequestsSheetPM> customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(_tenant, "UCUDO", "", "", null, null, _CourierDeclarationPMPMDiferentMaster.CourierMasterId, true);

                                    if (customsRequestsSheetPMList == null || customsRequestsSheetPMList.Count == 0)
                                    {
                                        AppendLogLine("open UCUDO  ??");

                                        string GeneralKey = GetGeneralLockKey(_CourierDeclarationPMPMDiferentMaster.CourierMasterId);
                                        var concurrentKiller = new ConcurrentKiller();
                                        concurrentKiller.FreeLockIfCreated15MinOld(GeneralKey, _CourierDeclarationPMPMDiferentMaster.Tenant);
                                        bool haveUCUDOInProgress = false;
                                        try
                                        {
                                            concurrentKiller.LockOrCrashOnCommitDueUnique(GeneralKey, _CourierDeclarationPMPMDiferentMaster.Tenant);
                                            haveUCUDOInProgress = false;
                                            AppendLogLine("UCUDO:concurrentKiller: Ok");
                                        }
                                        catch (Exception)
                                        {
                                            AppendLogLine("UCUDO:concurrentKiller:Have in the middle in the last 15 min- not open  UCUDO");
                                            haveUCUDOInProgress = true;
                                        }

                                        // customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(_tenant, "UCUW2L", "", "", null, null, _CourierDeclarationPMPMDiferentMaster.CourierMasterId, true);
                                        //customsRequestsSheetPMList = customsRequestsSheetPMList.Where(x => x.Id != _PBId).ToList();
                                        //  if (customsRequestsSheetPMList == null || customsRequestsSheetPMList.Count == 0)
                                        //  {
                                        if (!haveUCUDOInProgress)
                                        {
                                            var messagingService = new DCAInUCUDO_UpdateOpenDeclarationsMessagingService();
                                            UpdateOpenDeclarationsRequestParams requestParams2 = new UpdateOpenDeclarationsRequestParams()
                                            {

                                                LoggingUserId = Curruser,
                                                Tenant = _tenant,
                                                LoggingEntityId = _CourierDeclarationPMPMDiferentMaster.CourierMasterId,

                                            };

                                            string message = messagingService.CreateCRS(_tenant, Curruser, requestParams2);

                                        }
                                        //}
                                    }
                                }
                            }
                            catch (DbEntityValidationException ex)
                            {
                                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                                AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                                return;
                            }
                            catch (Exception e)
                            {
                                AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                                return;
                            }

                            string prevVal = null;
                            string currvVal = null;
                            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
                            currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
                            if (currentDeclarationCourierStatusPM != null)
                            {
                                CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(_MyDeclarationPM, _MyDeclarationPM.Id, _MyDeclarationPM.Tenant);
                                prevVal = currentDeclarationCourierStatusPM.CourierManifestStatusCode;
                                if (prevVal == "V")
                                {
                                    currvVal = "R";
                                }
                                else
                                {
                                    calculateDeclarationCourierStatus.CalcCourierManifestStatusCode(currentDeclarationCourierStatusPM);
                                    currvVal = currentDeclarationCourierStatusPM.CourierManifestStatusCode;
                                }
                                if (prevVal != currvVal)
                                {
                                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(_context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                    AppendLogLine("try to update declarationCourierStatus for DeclarationPM.Id: " + _MyDeclarationPM.Id);
                                    try
                                    {
                                        declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                                    }
                                    catch (DbEntityValidationException ex)
                                    {
                                        var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                                        AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                                        return;
                                    }
                                    catch (Exception e)
                                    {
                                        AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                                        return;
                                    }
                                }
                            }
                            this.UpsertActionConst = String.Concat(UpsertActionConst, "+CourierMasterChange");
                        }
                    }
                    _CourierDeclarationPM = new CourierDeclarationPM();
                    _CourierDeclarationPM.ChangeSetOp = ChangeSetOperation.Insert;
                    _CourierDeclarationPM.DeclarationId = _MyDeclarationPM.Id;
                    _CourierDeclarationPM.CourierMasterId = _CourierMasterPM.Id;



                }
                else
                {

                    _CourierDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;



                }
                if (_CourierDeclarationPM.SequenceNumeric == null)
                {
                    int? sequenceNumericMax = myCourierDeclarationQueryService.GetCourierMasterMaxSequenceNumeric(_CourierDeclarationPM.CourierMasterId, _tenant);
                    if (sequenceNumericMax == null)
                    {
                        sequenceNumericMax = 0;
                    }
                    _CourierDeclarationPM.SequenceNumeric = sequenceNumericMax + 1;
                }
                _CourierDeclarationPM.Tenant = _tenant;

                _context = CustomContext.GetContext(ResolvedTenant());

                CourierMasterRepository courierMasterRepository = new CourierMasterRepository(_context);

                if (courierMasterRepository != null)

                {


                    if (false) {
                    CourierMaster courierMaster = courierMasterRepository.GetSingle(new CourierMasterKeys() { Id = _CourierMasterPM.Id });

                    if (courierMaster != null)

                    {
                        if(false)
                        {

                        DeclarationCourierStatusRepository rep = new DeclarationCourierStatusRepository(_context);

                        DeclarationCourierStatus decCourier = rep.GetDeclarationsById(_CourierDeclarationPM.DeclarationId, _CourierDeclarationPM.Tenant);

                        if ((decCourier != null && !decCourier.IsClosedForFollowUp && _CourierDeclarationPM.ChangeSetOp == ChangeSetOperation.Insert) || (_IsNewDeclaration == true && courierMaster.IsOpen == false))

                        {

                            DateTime stopLogAt = new DateTime(2021, 11, 01);

                            string logData = "";

                            var loggedUser = AuthenticationUtil.ResolveUserIdentityName(_CourierDeclarationPM.Tenant);

                            logData = $"_CourierDeclarationPM.DeclarationId={_CourierDeclarationPM.DeclarationId}, ChangeSetOp={_CourierDeclarationPM.ChangeSetOp}, decCourier.IsClosedForFollowUp={decCourier.IsClosedForFollowUp},OpenDeclarations ={courierMaster.OpenDeclarations}before update1";

                            LogitudeSettings.HandleLogMe("OpenDeclarations " + logData, false, "time", stopLogAt);

                            if (_IsNewDeclaration == true && courierMaster.IsOpen == false) courierMaster.IsOpen = true;
                            courierMaster.OpenDeclarations += 1;

                            courierMasterRepository.Update(courierMaster);

                            courierMasterRepository.SubmitChanges();
                            }

                        }

                        }
                    }

                }
                AppendLogLine("try to update CourierDeclaration for DeclarationPM.Id: " + _MyDeclarationPM.Id + " CourierMasterPM.Id: " + _CourierMasterPM.Id);
                try
                {
                    myCourierDeclarationUpdateService.Update(_CourierDeclarationPM, true);
                }
                catch (DbEntityValidationException ex)
                {
                    var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                    AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                    return;
                }
                catch (Exception e)
                {
                    AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                    return;
                }
            }
        }

        public static string GetGeneralLockKey(string courierMasterId)
        {
            return $"UCUDO:{courierMasterId}";
        }


        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            AmitalContext amitalContext = AmitalContext.GetContext(tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            return
@"<?xml version=""1.0"" encoding=""windows-1255""?>
<LOGICOMMDEC>
 <LogitudeCommDecFile>
  <LoadingPortCode>USBOS</LoadingPortCode>
  <MAWB>20261079</MAWB>
  <HAWB>09878498</HAWB>
  <CarrierPrefix>114</CarrierPrefix>
  <IsAutonomy>No</IsAutonomy>
  <INVOICE>
   <CURRENCYCODE>USD</CURRENCYCODE>
   <INVOICEAMOUNT>12.00</INVOICEAMOUNT>
   <ISSUECOUNTRYCODE>CN</ISSUECOUNTRYCODE>
   <ORIGIN_COUNTRY>CN</ORIGIN_COUNTRY>
   <Amount>0</Amount>
   <CurrencyTypeCode>USD</CurrencyTypeCode>
   <INVOICEITEMS>
    <ITEMPRICE>12.0000</ITEMPRICE>
    <QUANTITY_STS>1</QUANTITY_STS>
    <ITEMORIGINCOUNTRY>CN</ITEMORIGINCOUNTRY>
   </INVOICEITEMS>
   <INCOTERM_ID>CIF</INCOTERM_ID>
   <ACCOUNTTYPE>380</ACCOUNTTYPE>
   <TRANSP_VALUE_LIST>
    <TRANSP_VALUE_L>0</TRANSP_VALUE_L>
    <TRANSP_VALUE_CURR_L>USD</TRANSP_VALUE_CURR_L>
   </TRANSP_VALUE_LIST>
  </INVOICE>
  <OriginCountryCode>US</OriginCountryCode>
  <CustomFileNo>60390074</CustomFileNo>
  <Id/>
  <DeclarationOfficeCode>4</DeclarationOfficeCode>
  <FileState>P</FileState>
  <AgentId>514193408</AgentId>
  <CustomerId>10015236</CustomerId>
  <TransportModeId>A</TransportModeId>
  <CreatedByUserId>AMITAL.COURIER</CreatedByUserId>
  <ReferentUserId/>
  <DepartmentId>MSC</DepartmentId>
  <MAWB>20261079</MAWB>
  <DealId/>
  <HAWB>09878498</HAWB>
  <ManifestNumber>99999560337</ManifestNumber>
  <LoadingPortCode/>
  <OriginCountryCode>US</OriginCountryCode>
  <CargoDescription>IBOX 233</CargoDescription>
  <PackageTypeCode>PP</PackageTypeCode>
  <PackageMeasureQualifierCode>2</PackageMeasureQualifierCode>
  <PackageQuantity>1</PackageQuantity>
  <GrossMassMeasure>0.30</GrossMassMeasure>
  <VendorId/>
  <ImporterId/>
  <Tenant>1</Tenant>
  <GrantDate/>
  <ManifestDate/>
  <ArrivalDateTime/>
  <Mode>NEW</Mode>
  <EnglishName>Kobi Cohen</EnglishName>
  <HebrewName/>
  <WarehouseId>ILMMN</WarehouseId>
  <UnloadportId/>
  <ProcedureCurrentCode>4000507</ProcedureCurrentCode>
  <ImporterAddress>Dekel 27 2nd avenu 13 ddk Tel Aviv</ImporterAddress>
  <CargoTypeCode>17</CargoTypeCode>
  <SecondCargoID>514193408</SecondCargoID>
  <ThirdCargoID>25.10.21</ThirdCargoID>
  <UnloadDate/>
  <IsCourierDeclaration>true</IsCourierDeclaration>
  <CasualSupplierName>Yaron Toys</CasualSupplierName>
  <CasualSupplierAddress>Yaron Toys-6546465 China</CasualSupplierAddress>
  <CourierHawb>99999560337</CourierHawb>
  <HAWBDATE/>
  <COUWTVAL/>
  <CasualImporterAddress1>Dekel 27 2nd avenu 13 ddk</CasualImporterAddress1>
  <CasualImporterAddress2/>
  <CasualImporterCity>Tel Aviv</CasualImporterCity>
  <CasualImporterZipCode>6546465</CasualImporterZipCode>
  <CasualImporterFax/>
  <CasualImporterEmail>ven@vendor.com</CasualImporterEmail>
  <CasualImportelTel>972089230879</CasualImportelTel>
  <CasualImporterContact/>
  <CasualImporterCountry/>
  <IsDiamondsDeclaration/>
  <EstimatedTimeOfArrival/>
  <OrderNumber>65161</OrderNumber>
  <WithPaper/>
  <NewFile>true</NewFile>
  <ImporterFile>BC32878</ImporterFile>
  <Team/>
  <FileOpenDate>20201026</FileOpenDate>
  <SiteCode>139514</SiteCode>
 </LogitudeCommDecFile>
</LOGICOMMDEC>"
                ;

            /*
"<?xml version="1.0" encoding="utf-8" ?>
<ArrayOfEntry xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
 <Entry>
  <Key>MODE</Key>
  <Value></Value>
 </Entry>
 <Entry>
  <Key>TENANT</Key>
  <Value>1</Value>
 </Entry>
 <Entry>
  <Key>UNIFREIGHT_USER_ID</Key>
  <Value>ITZIK</Value>
 </Entry>
</ArrayOfEntry>
"
             */
        }
        public string GetExampleDataIn1_()
        {

            var xml = "";
            var amitalObjExample = new LOGICOMMDEC();
            var myAmitalCommDec = new LogitudeCommDecFile();
            var myAmitalCommDecInvoice = new List<Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICE>();
            var myAmitalCommDecInvoiceItem = new List<Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICEITEMS>();

            myAmitalCommDec.Id = "1-1";
            myAmitalCommDecInvoice[1].INVOICELINENO = "1";
            myAmitalCommDecInvoice[1].INVOICELINENO = "1";
            myAmitalCommDecInvoice[1].ACCOUNTTYPE = "380";
            myAmitalCommDecInvoice[1].INVOICENUMBER = "999";
            myAmitalCommDecInvoice[1].VENDORNUMBER = "2000475";
            myAmitalCommDecInvoice[1].CURRENCYCODE = "18";
            myAmitalCommDecInvoice[1].INVOICEAMOUNT = "2";
            myAmitalCommDec.INVOICE = myAmitalCommDecInvoice.ToArray();

            myAmitalCommDecInvoiceItem[1].CLASSIFICATIONCODE = "260300009";
            myAmitalCommDecInvoiceItem[1].TRADEAGREEMENTCODE = "BGR";
            myAmitalCommDecInvoiceItem[1].QUANTITY = "1";
            myAmitalCommDecInvoiceItem[1].ITEMPRICE = "1";
            myAmitalCommDecInvoiceItem[1].ITEMORIGINCOUNTRY = "AD";
            myAmitalCommDecInvoiceItem[1].ITEMCODE = "DFDF";
            myAmitalCommDec.INVOICE[1].INVOICEITEMS = myAmitalCommDecInvoiceItem.ToArray();

            amitalObjExample.LogitudeCommDecFile = new LogitudeCommDecFile[] { myAmitalCommDec };

            xml = XmlGenericUtil<LOGICOMMDEC>.SerializeObject(amitalObjExample);

            return xml;
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


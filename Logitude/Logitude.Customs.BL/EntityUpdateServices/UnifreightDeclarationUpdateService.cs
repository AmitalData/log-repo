using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure.FuStatus;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Customs.Def.Messaging.Customs;
using Simplog.Data.CommonDataModel;
using System.Globalization;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public class UnifrightDeclarationUpdateService : IDisposable
    {
        private Def.EntityPMs.DeclarationPM _DirtyDeclarationPM;
        private Def.EntityPMs.DeclarationPM _DBOccDeclarationPM;
        private string _LoggingUserId;
        private AmitalContext _AmitalContext;
        private ICustomContext _Context;
        private CCUFILEMPM _CCUFILEMPM;
        private bool _IsSupplerInvChanged;
        private bool _IsDeclarationTaxesChanged;
        private bool _IsConsignmentChanged;
        private long lCUSTOMFILENO;
        double? _CCUFILEMPMSupplierInvoiceModificationsI01;
        double? _CCUFILEMPMSupplierInvoiceModifications527;
        private GTRTRANQueryService _GTRTRANQueryService;
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private bool _FromMessaging;
        private List<CustomsExchangeRatePM> _CustomsExchangeRates; //Yuval Chalup 31.12.2014 AMI-52371
        decimal? _loanAmount; // moran 17.1.16 - Task 19798
        //System.IO.StreamWriter file;
        private bool _IsSupplerInvUpdateCCUFILEM; //Yuval Chalup 13.02.2016 TASK-20599
        private bool _IsMainSupplerChanged; // moran 22.5.16 AMI-56276
        private string _PreviousMainSupplier; // moran 22.5.16 AMI-56276
        public bool _UpdateCCUFILEMFromSupplerInvoice;
        private CFIDATA _CFIDATA;


        private int _AccumulatedSupplierInvoice_105LastLineNo;
        private List<CCUTRANSPVALPM> _TotalCCUTRANSPVALs;
        private DateTime _EmptyDate;
        private bool _CreateCCUTAXFor105Feature;
        private CCUFILEM_4L2U _CCUFILEM4L2UPM_Before = new CCUFILEM_4L2U();
        private CCUFILEM_4L2U _CCUFILEM4L2UPM_After = new CCUFILEM_4L2U();
        private bool _IsCCUFILEM_4U2L_Changed;
        private CCUFILEMPM _CCUFILEMPMwithCCUMSHGRP = new CCUFILEMPM();
        private List<CCUTAX_4LD2U> _CCUTAX_4LD2UPM_Before = new List<CCUTAX_4LD2U>();
        private List<CCUTAX_4LD2U> _CCUTAX_4LD2UM_After = new List<CCUTAX_4LD2U>();
        private bool _IsCCUTAX_4LD2U_Changed;
        public bool _NO_LD2U;
        private bool _NoRaiseLD2ULogicFeature;


        public UnifrightDeclarationUpdateService(Def.EntityPMs.DeclarationPM dirtyDeclarationPM, Def.EntityPMs.DeclarationPM dbOccDeclarationPM, string loggingUserId)
        {
            // TODO: Complete member initialization
            this._DirtyDeclarationPM = dirtyDeclarationPM;
            this._DBOccDeclarationPM = dbOccDeclarationPM;
            this._LoggingUserId = loggingUserId;
            this._Context = CustomContext.GetContext(_DirtyDeclarationPM.Tenant);
            _EmptyDate = new DateTime(1900, 1, 1);
            //if (file==null)
            //{
            //file = new System.IO.StreamWriter(@"\\UNIV55\UnifreightV55-test2\log\CCUFILEM Save Logs.txt", true);
            //file = new System.IO.StreamWriter(@"C:\\\CCUFILEM Save Logs.txt", true);
            //file = new System.IO.StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/CCUFILEM Save Logs.txt"), true);
            //}
        }

        internal void Update(Boolean doTask)//eitan h 12/3/15 task 11788
        //internal void Update()
        {
            DateTime stopLogAt = DateTime.MinValue;
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20210427HD368109.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None);
            }
            string logData = "";

            this._CreateCCUTAXFor105Feature = true; ///ConfigurationManager.AppSettings["20180121.CreateCCUTAXFor105"] == "1";///todo
            this._NoRaiseLD2ULogicFeature = true; ///ConfigurationManager.AppSettings["20180204.NoRaiseLD2ULogicFeature"] == "1";
            var cntxt = RequestSheetContext.Current.GetContextOrDefault();
            if (cntxt.MainInterfaceCode == "2715")
            {
                LogMessagingUtil.Instance.AppendLine("While in 2715 (Batch Mode) do not update CCUFILEM !!! ");
                return;
            }

            var sw = Stopwatch.StartNew();
            try
            {
                if (_DirtyDeclarationPM.CurrentContextTag != null)
                {
                    if (string.IsNullOrWhiteSpace(_DirtyDeclarationPM.CurrentContextTag.ToString()) == false)
                    {
                        _FromMessaging = true;
                    }
                }

                //if ((!Environment.MachineName.Equals("itzik-7-new", StringComparison.OrdinalIgnoreCase)) && (!Environment.MachineName.Equals("yuval-7-new", StringComparison.OrdinalIgnoreCase))) return;
                if (_DirtyDeclarationPM.IsCancelled == true)
                {
                    logData = $"_DirtyDeclarationPM.CustomFileNo={_DirtyDeclarationPM.CustomFileNo},_DBOccDeclarationPM.CustomFileNo={_DBOccDeclarationPM.CustomFileNo},_DirtyDeclarationPM.IsCancelled={_DirtyDeclarationPM.IsCancelled}"; 
                    LogitudeSettings.HandleLogMe(logData, false, "UpdateUnifreight_" + _DirtyDeclarationPM.Id, stopLogAt);
                    //return;
                }
                //<--- Yuval Chalup 19.11.2015 TASK-17450
                if (_DirtyDeclarationPM.IsConvertedDeclaration)
                {
                    logData = $"_DirtyDeclarationPM.IsConvertedDeclaration={_DirtyDeclarationPM.IsConvertedDeclaration}";
                    LogitudeSettings.HandleLogMe(logData, false, "UpdateUnifreight_" + _DirtyDeclarationPM.Id, stopLogAt);
                    return;
                }
                //Yuval Chalup 19.11.2015 TASK-17450 --->
                if (_DirtyDeclarationPM.IsCancelled == true)
                {
                    if (String.IsNullOrWhiteSpace(_DirtyDeclarationPM.CustomFileNo) && !String.IsNullOrWhiteSpace(_DBOccDeclarationPM.CustomFileNo))
                    {
                        if (!long.TryParse(_DBOccDeclarationPM.CustomFileNo, out lCUSTOMFILENO))
                        {
                            throw new BusinessErrorException("dirtyDeclarationPM.CustomFileNo could not convert to long ");
                        }
                    }
                }
                if(lCUSTOMFILENO < 1)
                {
                    if (!long.TryParse(_DirtyDeclarationPM.CustomFileNo, out lCUSTOMFILENO))
                    {
                        throw new BusinessErrorException("dirtyDeclarationPM.CustomFileNo could not convert to long ");
                    }
                }
                // moran 22.2.16 - Task 19654 - enter into 'if', not save changes always
                if (_DirtyDeclarationPM.CurrentContextTag == Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst ||
                    _DirtyDeclarationPM.CurrentContextTag == Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.CreateUnifreightPaymentConst ||
                    HttpContextUtil.IsPath(HttpContextUtil.PathU2LUNIFREIGHTGATEWAYPage)) //Yuval Chalup 03.04.2016 TASK-20834  (Add PathU2LUNIFREIGHTGATEWAYPage)
                {
                    _IsSupplerInvChanged = IsSupplerInvChanged();
                    _IsDeclarationTaxesChanged = IsDeclarationTaxesChanged();
                    _IsConsignmentChanged = IsConsignmentChanged();
                }

                //_IsSupplerInvChanged = true;
                //var setting = CustomsSettingQueryService.GetSettingByTenant(_DirtyDeclarationPM.Tenant);
                //if (setting != null)
                //{
                //  if (!setting.IsConnectedToUnifreight)
                if (!_DirtyDeclarationPM.IsConnectedToUnifreight)
                {
                    return;
                }
                //}


                TransactionScope scope = null;//TransactionFactory.GetNewTransaction())//new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
                {
                    scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
                }
                try
                {
                    using (_AmitalContext = AmitalContext.GetContext(_DirtyDeclarationPM.Tenant))
                    {
                        //AmitalContext.SetOracleMonitor();

                        var myCCUFILEMQueryService = new CCUFILEMQueryService(_AmitalContext);
                        var myCCUFILEMUpdateService = new CCUFILEMUpdateService(_AmitalContext);
                        var myCCUMSHGRQueryService = new CCUMSHGRQueryService(_AmitalContext);
                        myCCUFILEMUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.


                        int? FILENO = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
                        if (FILENO.HasValue)
                        {
                            LogMessagingUtil.Instance.AppendLine("Update3: GetFILENOByCUSTOMFILENO, file: " + lCUSTOMFILENO);
                            int? FILENO1 = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO_forUpdateNOWAIT(lCUSTOMFILENO);

                            //if(file!=null) file.WriteLine("UnifrightDeclarationUpdateService - Updating " + FILENO + ": " + DateTime.Now.ToString());
                            //if(file!=null) file.WriteLine("Start Deleting " + FILENO + ": " + DateTime.Now.ToString());

                            //do not need the composite due we delete all down entities !!!_CCUFILEMPM = myCCUFILEMQueryService.GetSingle(FILENO.Value, true, false);
                            _CCUFILEMPM = myCCUFILEMQueryService.GetSingle(FILENO.Value, false, false);
                            if(_CCUFILEMPM == null)
                            {
                                LogMessagingUtil.Instance.AppendLine("Update4: _CCUFILEMPM GetSingle failed, file no: " + FILENO.Value);
                            }
                            else
                            {
                                LogMessagingUtil.Instance.AppendLine("Update5: _CCUFILEMPM GetSingle, file: " + _CCUFILEMPM.CUSTOMFILENO);
                            }
                            
                            _CCUFILEMPMwithCCUMSHGRP = myCCUFILEMQueryService.GetSingle(FILENO.Value, false, false);

                            //CCUFILEMKeys cCUFILEMKeys = new CCUFILEMKeys { FILENO = FILENO.GetValueOrDefault() };
                            //List<CCUMSHGRPM> myCCUMSHGRPMList = myCCUMSHGRQueryService.GetMulti(cCUFILEMKeys, false, false);
                            //_CCUFILEMPMwithCCUMSHGRP.CCUMSHGRs = myCCUMSHGRPMList;

                            if (this._UpdateCCUFILEMFromSupplerInvoice)
                            {
                                var myCCUTRANSPVALQueryService = new CCUTRANSPVALQueryService(_AmitalContext);
                                CCUFILEMKeys myCCUFILEMKeys = new CCUFILEMKeys { FILENO = FILENO.GetValueOrDefault() };
                                List<CCUTRANSPVALPM> myCCUTRANSPVALPMList = myCCUTRANSPVALQueryService.GetMulti(myCCUFILEMKeys, false, false);

                                //Delete CCUTRANSPVALs
                                if (myCCUTRANSPVALPMList != null && myCCUTRANSPVALPMList.Count() > 0)
                                {
                                    _CCUFILEMPM.CCUTRANSPVALs = myCCUTRANSPVALPMList;

                                    //Delete CCUTRANSPVALs
                                    if (_CCUFILEMPM.CCUTRANSPVALs != null && _CCUFILEMPM.CCUTRANSPVALs.Count() > 0)
                                    {
                                        foreach (var myCCUTRANSPVALPM in _CCUFILEMPM.CCUTRANSPVALs)
                                        {
                                            myCCUTRANSPVALPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                                        }

                                        _CCUFILEMPM.DeletedCCUTRANSPVALs = _CCUFILEMPM.CCUTRANSPVALs;
                                        _CCUFILEMPM.CCUTRANSPVALs = null;

                                        _CCUFILEMPM.ChangeSetOp = ChangeSetOperation.Update;
                                        // Update for the Delete
                                        myCCUFILEMUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                                        myCCUFILEMUpdateService.Update(_CCUFILEMPM, true);
                                        LogMessagingUtil.Instance.AppendLine("Update6: _CCUFILEMPM Update, file: " + _CCUFILEMPM.CUSTOMFILENO);
                                        _AmitalContext.SaveChanges();
                                        LogMessagingUtil.Instance.AppendLine("Update7: _CCUFILEMPM SaveChanges, file: " + _CCUFILEMPM.CUSTOMFILENO);
                                        //Clean up the Supplier Invoices
                                        _CCUFILEMPM.CCUTRANSPVALs = null;
                                        _CCUFILEMPM.DeletedCCUTRANSPVALs = null;
                                    }
                                }
                            }
                            else
                            {
                                if (_DirtyDeclarationPM.IsCancelled == true) // moran 5.1.16 - AMI-55274 -->
                                {
                                    logData = $"before delete ccufilem,_DirtyDeclarationPM.IsCancelled={_DirtyDeclarationPM.IsCancelled}";
                                    LogitudeSettings.HandleLogMe(logData, false, "UpdateUnifreight_" + _DirtyDeclarationPM.Id, stopLogAt);
                                    myCCUFILEMUpdateService.FastTotalDeleteComposition(_CCUFILEMPM);
                                    _AmitalContext.SaveChanges();
                                    logData = $"after delete ccufilem,_DirtyDeclarationPM.IsCancelled={_DirtyDeclarationPM.IsCancelled}";
                                    LogitudeSettings.HandleLogMe(logData, false, "UpdateUnifreight_" + _DirtyDeclarationPM.Id, stopLogAt);
                                }
                                else // moran 5.1.16 - AMI-55274 <--
                                {
                                    #region OldDeleCode
                                    if (_IsSupplerInvChanged)
                                    {
                                        if (false)
                                        {
                                            SupplierInvoiceDeleteAll(myCCUFILEMUpdateService);

                                            OtherDeleteAll(myCCUFILEMUpdateService);
                                        }

                                        #region Good2Remembre

                                        /*
     LinqConnect unites them in batches. And if you do want to delete multiple rows with a single command, or delete a row by just a primary key (to avoid getting the object from the database), you can use the ExecuteCommand method of DataContext.
    http://forums.devart.com/viewtopic.php?t=13223
                                     * --
    There is a extension provided at EntityFramework.Extended
    to delete all object
    //delete all users where FirstName matches
    context.Users.Delete(u => u.FirstName == "firstname");
                                     * 
    EntityFramework 6 has made this a bit easier with .RemoveRange().

    Example:

    db.People.RemoveRange(db.People.Where(x => State == "CA"));
                                     */


                                        /*

                                        CCUACCSUP  SupplierInvoicePM
                                            CCUSUPITEM SupplierInvoiceItem103PM 
                                                CCUCUSTITEM SupplierInvoiceItem105PM CCUCUSTITEMS
                                                CCUCRREQ CCUCRREQPM

                                        CCUMSHGR CCUMSHGRPM
                                        CCUTAX  CCUTAXPM

                                         */

                                        #endregion

                                        GetMishgors(FILENO.Value);
                                        GetCCUTAX(FILENO.Value);

                                        myCCUFILEMUpdateService.FastDeleteComposition(_CCUFILEMPM);
                                        _AmitalContext.SaveChanges();
                                    }
                                    #endregion

                                    //NOTE: Both Supplier Invoice and Declaration Taxes directed to CCUTAX, so if one of them is changed need to delete all
                                    if (_IsSupplerInvChanged || _IsDeclarationTaxesChanged)
                                    {
                                        _IsSupplerInvChanged = true;
                                        _IsConsignmentChanged = true;
                                    }

                                    if (_IsSupplerInvChanged) // moran 22.5.16 AMI-56276
                                    {
                                        if (_CCUFILEMPM != null && _CCUFILEMPM.SupplierInvoices != null && _CCUFILEMPM.SupplierInvoices.Count() > 0)
                                        {
                                            _PreviousMainSupplier = _CCUFILEMPM.SupplierInvoices.Where(d => d.MAINACCOUNT == true).FirstOrDefault().SUPPLIERID;
                                        }
                                    }

                                    _IsSupplerInvUpdateCCUFILEM = !_IsSupplerInvChanged; //Yuval Chalup 13.02.2016 TASK-20599


                                    _IsConsignmentChanged = true; //Yuval Chalup 13.02.2016 TASK-20599
                                    //In HATARA (2470) - Do not delete/update Consignment
                                    EventContextTagModel eventContextTagModel = new EventContextTagModel();
                                    eventContextTagModel = _DirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
                                    if (eventContextTagModel != null && eventContextTagModel.CallProccessID == EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate)
                                    {
                                        _IsConsignmentChanged = false;
                                    }

                                    //If required, delete ONLY what is needed
                                    if (_IsSupplerInvChanged || _IsDeclarationTaxesChanged || _IsConsignmentChanged)
                                    {
                                        if (_IsConsignmentChanged)
                                        {
                                            GetMishgors(FILENO.Value);
                                        }
                                        if (_IsDeclarationTaxesChanged)
                                        {
                                            GetCCUTAX(FILENO.Value);
                                        }
                                        myCCUFILEMUpdateService.FastDeleteComposition(_CCUFILEMPM, _IsSupplerInvChanged, _IsDeclarationTaxesChanged, _IsConsignmentChanged); // moran 14.6.16 - Task 21737 - constant true instead _IsSupplerInvChanged + Yuval Changed back from true to _IsSupplerInvChanged 13.06.2017
                                        _AmitalContext.SaveChanges();
                                    }
                                }
                            }
                        }

                        //if(file!=null) file.WriteLine("Start DoCustomFile() " + FILENO + ": " + DateTime.Now.ToString());
                        if (_DirtyDeclarationPM.IsCancelled != true) // moran 4.1.16 - AMI-55274 - enter into 'if' - do not update ccufilem for cancelled declaration/file
                        {
                            if (_CreateCCUTAXFor105Feature)
                            {
                                _AccumulatedSupplierInvoice_105LastLineNo = 0;
                            }
                            DoCustomFile();
                            _IsCCUFILEM_4U2L_Changed = CheckIfCCUFILEMChanged();
                            _NO_LD2U = (!CheckIfCCUTAXChanged() && _CCUFILEMPM.MEHESDRAFTSTATUS == 1 && _NoRaiseLD2ULogicFeature);

                            //If it is a PILOT FILE FROM PRODUCTION = The Declaration is CONNECTEDTOUNF but the environment is NOT - Set CCUFILEM as Cancelled 
                            if (_DirtyDeclarationPM.IsConnectedToUnifreight)
                            {
                                var setting = CustomsSettingQueryService.GetSettingByTenant(_DirtyDeclarationPM.Tenant);
                                if (setting != null)
                                {
                                    if (!setting.IsConnectedToUniFreight)
                                    {
                                        _CCUFILEMPM.CANCELLED = "T";
                                    }
                                }
                            }

                            //if(file!=null) file.WriteLine("Start myCCUFILEMUpdateService.Update " + FILENO + ": " + DateTime.Now.ToString());
                            myCCUFILEMUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.

                            using (var logger = (_AmitalContext as DbContextBase).CreateLogger())
                            {
                                try
                                {
                                    LogMessagingUtil.Instance.AppendLine("Update1: _CCUFILEMPM Update, file: " + _CCUFILEMPM.CUSTOMFILENO);
                                    myCCUFILEMUpdateService.Update(_CCUFILEMPM, true);
                                }
                                catch (Exception eUpdate)
                                {


                                    LogMessagingUtil.Instance.Append("CCUFILEMUpdateService.Update:");

                                    LogMessagingUtil.Instance.AppendLine(logger.ToString(2040));
                                    throw;
                                }

                            }
                            if (doTask)//eitan h 12/3/15 task 11788
                            {
                                //if(file!=null) file.WriteLine("Start OpenUnifreighTask " + FILENO + ": " + DateTime.Now.ToString());
                                OpenUnifreighTask(lCUSTOMFILENO, _CCUFILEMPM);
                            }
                            //if(file!=null) file.WriteLine("End Updating " + FILENO + ": " + DateTime.Now.ToString());
                        }
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
            }
            catch (Exception ex)
            {

                ex.ChangeExceptionMessage("UnifrightDeclarationUpdateService Exception");
                LogMessagingUtil.Instance.AppendLine("Update2: Exception, file: " + _CCUFILEMPM.CUSTOMFILENO + "\n" + ex.Message); 
                throw;
                //throw new BusinessErrorException("") ;
            }
            finally
            {
                //if(file!=null) file.Close();
                LogMessagingUtil.Instance.AppendLine("UnifrightDeclarationUpdateService:took:" + sw.Elapsed.ToString());
            }

        }

        private void GetMishgors(int? fileno)
        {
            if (_CCUFILEMPMwithCCUMSHGRP.CCUMSHGRs != null && _CCUFILEMPMwithCCUMSHGRP.CCUMSHGRs.Count > 1)
            {
                return;
            }

            TransactionScope tempscope = null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                tempscope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (AmitalContext tempAmitalContext = AmitalContext.GetContext(_DirtyDeclarationPM.Tenant))
                {
                    var myCCUFILEMQueryService = new CCUFILEMQueryService(tempAmitalContext);
                    var myCCUMSHGRQueryService = new CCUMSHGRQueryService(tempAmitalContext);

                    if (fileno.HasValue)
                    {
                        _CCUFILEMPMwithCCUMSHGRP = myCCUFILEMQueryService.GetSingle(fileno.Value, false, false);
                        CCUFILEMKeys cCUFILEMKeys = new CCUFILEMKeys { FILENO = fileno.GetValueOrDefault() };
                        List<CCUMSHGRPM> myCCUMSHGRPMList = myCCUMSHGRQueryService.GetMulti(cCUFILEMKeys, false, true);
                        _CCUFILEMPMwithCCUMSHGRP.CCUMSHGRs = myCCUMSHGRPMList;
                    }
                }
                if (tempscope != null)
                {
                    tempscope.Complete();
                }
            }
            finally
            {
                if (tempscope != null)
                {
                    tempscope.Dispose();
                }
            }
        }

        private void GetCCUTAX(int? fileno)
        {

            if (_CCUTAX_4LD2UPM_Before != null && _CCUTAX_4LD2UPM_Before.Count > 1)
            {
                return;
            }

            TransactionScope tempscope = null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                tempscope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (AmitalContext tempAmitalContext = AmitalContext.GetContext(_DirtyDeclarationPM.Tenant))
                {
                    var myCCUTAXQueryService = new CCUTAXQueryService(tempAmitalContext);

                    if (fileno.HasValue)
                    {
                        CCUFILEMKeys cCUFILEMKeys = new CCUFILEMKeys { FILENO = fileno.GetValueOrDefault() };
                        List<CCUTAXPM> myCCUTAXPMList = myCCUTAXQueryService.GetMulti(cCUFILEMKeys, false, false);
                        _CCUTAX_4LD2UPM_Before = CCUTAX_4LD2U_Mapping(myCCUTAXPMList);
                    }
                }
                if (tempscope != null)
                {
                    tempscope.Complete();
                }
            }
            finally
            {
                if (tempscope != null)
                {
                    tempscope.Dispose();
                }
            }
        }

        private bool CheckIfCCUFILEMChanged()
        {
            var myXMLBefore = XmlGenericUtil<CCUFILEM_4L2U>.SerializeObject(_CCUFILEM4L2UPM_Before);
            var myXMLAfter = XmlGenericUtil<CCUFILEM_4L2U>.SerializeObject(_CCUFILEM4L2UPM_After);

            return (myXMLBefore != myXMLAfter);
        }

        private bool CheckIfCCUTAXChanged()
        {
            var myXMLBefore = XmlGenericUtil<List<CCUTAX_4LD2U>>.SerializeObject(_CCUTAX_4LD2UPM_Before);
            var myXMLAfter = XmlGenericUtil<List<CCUTAX_4LD2U>>.SerializeObject(_CCUTAX_4LD2UM_After);

            return (myXMLBefore != myXMLAfter);
        }

        private void OtherDeleteAll(CCUFILEMUpdateService myCCUFILEMUpdateService)
        {
            foreach (var cons in _CCUFILEMPM.CCUMSHGRs)
            {
                cons.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            }

            foreach (var tax in _CCUFILEMPM.CCUTAXPM)
            {
                tax.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            }

            //<--- Yuval Chalup 14.06.2015 TASK-13951
            foreach (var tax in _CCUFILEMPM.CCUTRANSPVALs)
            {
                tax.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            }
            //Yuval Chalup 14.06.2015 TASK-13951 --->

            _CCUFILEMPM.DeletedCCUTAXPM = _CCUFILEMPM.CCUTAXPM;
            _CCUFILEMPM.DeletedCCUMSHGRs = _CCUFILEMPM.CCUMSHGRs;
            _CCUFILEMPM.DeletedCCUTRANSPVALs = _CCUFILEMPM.CCUTRANSPVALs; //Yuval Chalup 14.06.2015 TASK-13951

            _CCUFILEMPM.CCUTAXPM = null;
            _CCUFILEMPM.CCUMSHGRs = null;
            _CCUFILEMPM.CCUTRANSPVALs = null; //Yuval Chalup 14.06.2015 TASK-13951

            _CCUFILEMPM.ChangeSetOp = ChangeSetOperation.Update;
            // Update for the Delete
            myCCUFILEMUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
            myCCUFILEMUpdateService.Update(_CCUFILEMPM, true);

            //Clean up the Supplier Invoices
            _CCUFILEMPM.CCUTAXPM = null;
            _CCUFILEMPM.CCUMSHGRs = null;
            _CCUFILEMPM.CCUTRANSPVALs = null; //Yuval Chalup 14.06.2015 TASK-13951

            _CCUFILEMPM.DeletedCCUTAXPM = null;
            _CCUFILEMPM.DeletedCCUMSHGRs = null;
            _CCUFILEMPM.DeletedCCUTRANSPVALs = null; //Yuval Chalup 14.06.2015 TASK-13951
        }

        private void SupplierInvoiceDeleteAll(CCUFILEMUpdateService myCCUFILEMUpdateService)
        {
            foreach (var si in _CCUFILEMPM.SupplierInvoices)
            {
                si.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

                foreach (var si103 in si.SupplierInvoiceItems103s)
                {
                    si103.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

                    foreach (var si105 in si103.SupplierInvoiceItems105)
                    {
                        si105.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        // moran 12.1.16 - Task 17425 -->
                        foreach (var sicarl in si105.CCUCARLs)
                        {
                            sicarl.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            si105.DeletedCCUCARLPM.Add(sicarl);
                        }
                        si105.DeletedCCUCARLPM = si105.CCUCARLs;
                        si105.CCUCARLs = null;

                        foreach (var sicarl in si105.CCUCARs)
                        {
                            sicarl.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            si105.DeletedCCUCARPM.Add(sicarl);
                        }
                        si105.DeletedCCUCARPM = si105.CCUCARs;
                        si105.CCUCARs = null;
                        // moran 12.1.16 - Task 17425 <--
                        si103.DeletedSupplierInvoiceItems105.Add(si105);
                    }
                    si103.DeletedSupplierInvoiceItems105 = si103.SupplierInvoiceItems105;
                    si103.SupplierInvoiceItems105 = null;

                    foreach (var sireq in si103.CCUCRREQPM)
                    {
                        sireq.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        si103.DeletedCCUCRREQPM.Add(sireq);
                    }
                    si103.DeletedCCUCRREQPM = si103.CCUCRREQPM;
                    si103.CCUCRREQPM = null;
                }
                si.DeletedSupplierInvoiceItems103s = si.SupplierInvoiceItems103s;
                si.SupplierInvoiceItems103s = null;
            }

            _CCUFILEMPM.DeletedSupplierInvoices = _CCUFILEMPM.SupplierInvoices;

            _CCUFILEMPM.SupplierInvoices = null;

            _CCUFILEMPM.ChangeSetOp = ChangeSetOperation.Update;
            // Update for the Delete
            myCCUFILEMUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
            myCCUFILEMUpdateService.Update(_CCUFILEMPM, true);

            //Clean up the Supplier Invoices
            _CCUFILEMPM.SupplierInvoices = null;
            _CCUFILEMPM.DeletedSupplierInvoices = null;
        }

        private void OpenUnifreighTask(long customFile, CCUFILEMPM _CCUFILEMPM)
        {
            var myCCUQUELOCKQueryService = new CCUQUELOCKQueryService(_AmitalContext);
            var myCCUQUELOCKUpdateService = new CCUQUELOCKUpdateService(_AmitalContext);
            var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
            var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);
            var requestData = "";

            var unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(_DirtyDeclarationPM.Tenant);

            CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", customFile.ToString(), false);
            if (myCCUQUELOCK == null)
            {
                var myCCUQUELOCKPM = new CCUQUELOCKPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ENTNAME = "CFIFILEM",
                    FILENO = customFile.ToString(),
                };
                myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
            }

            #region remarkedCode
            //if (_FromMessaging == true)
            //{
            //    requestData = GetMyFUStatusXML("CFIFILEM", customFile.ToString(), "INR", "INR", "");
            //}

            //var myYCULTASKPM = new YCULTASKPM()
            //{
            //    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            //    STATUS = "W",
            //    REQUESTDATA = requestData,
            //    ENTNAME = "CFIFILEM",
            //    PRIMARYNUM = customFile.ToString(),
            //    PRIORITY = true,
            //    TYPE = "L2U",
            //    USRCODE = unifreightUser
            //};
            //myYCULTASKUpdateService.Update(myYCULTASKPM, true);

            //var myGGGQPM = new GGGQPM()
            //{
            //    ChangeSetOp = ChangeSetOperation.Insert,
            //    ORIGINQUE = "LGT", //LugitudeRequest
            //    STATUS = "1",
            //    EXPTASKTIME = 5,
            //    EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
            //    TRY = 3,
            //    PRIORITY = 8,
            //    ENTNAME = "CFIFILEM",
            //    PRIMARYNUM = customFile.ToString(),
            //    FORMID = "LGT_UPDATE_FCI",
            //    DEBUG = "F",
            //    DONEOPERATION = "A"
            //};
            //myGGGQUpdateService.Update(myGGGQPM, true);
            #endregion
            //<--- Yuval Chalup 23.10.2014 TASK-6711
            //Create another task to update CFIPACKS
            //if (this._DirtyDeclarationPM.CurrentContextTag is CFIPACKS)
            if (this._DirtyDeclarationPM.CurrentContextTag is CargoQueryContext)
            {
                var myCargoQueryContext = this._DirtyDeclarationPM.CurrentContextTag as CargoQueryContext;
                //var myCFIPACKS = this._DirtyDeclarationPM.CurrentContextTag as CFIPACKS;
                var myCFIPACKS = myCargoQueryContext.ResponseCFIPACKS as CFIPACKS;
                string myCustomFileNo = "";
                if (myCFIPACKS != null)
                {
                    myCustomFileNo = myCFIPACKS.CFIPACKS_DATA[0].FILE_NO;
                    var xmlCFIPACKS = XmlGenericUtil<CFIPACKS>.SerializeObject(myCFIPACKS, true);
                    CCUQUELOCKPM myCCUQUELOCK_Packs = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", myCFIPACKS.CFIPACKS_DATA[0].FILE_NO, false);
                    if (myCCUQUELOCK_Packs == null)
                    {
                        var myCCUQUELOCKPM = new CCUQUELOCKPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            ENTNAME = "CFIFILEM",
                            FILENO = myCFIPACKS.CFIPACKS_DATA[0].FILE_NO,
                        };
                        myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                        myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
                    }

                    //transmission mytransmission = GetTransmission<CFIPACKS>(myCFIPACKS, "AMITAL", "Customs packs from logitude");
                    transmission mytransmission = GetTransmission(myCFIPACKS, "AMITAL", "Customs packs from logitude");
                    var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
                    requestData = xmltransmission;
                }
                if (!string.IsNullOrWhiteSpace(myCargoQueryContext.RaiseStatus)) // moran 22.5.17 - Task 27973
                {
                    var sts = myCargoQueryContext.RaiseStatus;
                    DateTime statusDate = DateTime.Now;
                    if (myCargoQueryContext.StatusDate.HasValue) statusDate = myCargoQueryContext.StatusDate.Value;
                    var requestData2 = GetMyFUStatusXML(sts, sts, "", "new", statusDate, false);
                    if (string.IsNullOrWhiteSpace(requestData))
                    {
                        GFUSTS myGFUSTS = XmlGenericUtil<GFUSTS>.DeSerializeObject(requestData2);
                        transmission mytransmission = GetTransmission(myGFUSTS, "AMITAL", "FU Status from logitude");
                        var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
                        requestData = xmltransmission;
                        requestData = requestData.Replace("</transmission>", string.Concat("<CARGOQUERYMODE>CANCELSEND</CARGOQUERYMODE>", "</transmission>"));
                    }
                    else
                    {
                        requestData = requestData.Replace("</transmission>", string.Concat(requestData2, "<CARGOQUERYMODE>CANCELSEND</CARGOQUERYMODE>", "</transmission>"));
                    }

                }
                else if (myCargoQueryContext.RequestAutoSend)
                {
                    requestData = requestData.Replace("</transmission>", string.Concat("<CARGOQUERYMODE>AUTOSEND</CARGOQUERYMODE>", "</transmission>"));
                }
                /*
                if (_DBOccDeclarationPM.IsValueForCustomsOnly != _DirtyDeclarationPM.IsValueForCustomsOnly)
                {
                    string xml_status = "new";
                    if (_DirtyDeclarationPM.IsValueForCustomsOnly != true) xml_status = "del";
                    var addStatusData = GetMyFUStatusXML("DFC", "DFC", "", xml_status, DateTime.Now, false); ;
                    requestData.Replace("</transmission>", string.Concat(addStatusData, "</transmission>"));
                }
                */
                SetCFIDATA();
                if (_CFIDATA != null)
                {
                    var xmlCFIDATA = XmlGenericUtil<CFIDATA>.SerializeObject(_CFIDATA, true);
                    if (!string.IsNullOrWhiteSpace(xmlCFIDATA))
                    {
                        requestData = requestData.Replace("</transmission>", string.Concat(xmlCFIDATA, "</transmission>"));
                    }
                }
                var myFileAdditionalData = myCargoQueryContext.FileAdditionalData as FileAdditionalData;
                if (myFileAdditionalData != null)
                {
                    var xmlFileAdditionalData = XmlGenericUtil<FileAdditionalData>.SerializeObject(myFileAdditionalData, true);
                    if (!string.IsNullOrWhiteSpace(xmlFileAdditionalData))
                    {
                        requestData = requestData.Replace("</transmission>", string.Concat(xmlFileAdditionalData, "</transmission>"));
                    }
                }
                if (string.IsNullOrWhiteSpace(myCustomFileNo)) myCustomFileNo = this._DirtyDeclarationPM.CustomFileNo;

                if (!string.IsNullOrWhiteSpace(requestData))
                {
                    var myYCULTASKPM_Packs = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = myCustomFileNo,
                        PRIORITY = YCULTASKPM.calcPriority("L2U"),//eitan h 12/3/15 new static operation
                        //PRIORITY = 1,
                        TYPE = "L2U",
                        USRCODE = unifreightUser,
                        ARCHIVE = "F", // moran 28.6.16 - AMI-57170
                        //LOGTIME = (new DualQueryService(MainContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now,
                    };
                    //myYCULTASKPM.TASKID = CommCounterUtil.GetUnique30(myYCULTASKPM.LOGTIME);
                    myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    myYCULTASKUpdateService.Update(myYCULTASKPM_Packs, true);

                    var myGGGQPM_Packs = new GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", //LugitudeRequest
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
                        TRY = 9,
                        PRIORITY = 8,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = myCustomFileNo,
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "D",
                        //GSTRING1 = myYCULTASKPM.TASKID,
                    };
                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    myGGGQUpdateService.Update(myGGGQPM_Packs, true);
                }
            }
            else
            {
                //if (_FromMessaging == true)
                //{
                //    var myEventContextTagModel = new EventContextTagModel();
                //    myEventContextTagModel = this._DirtyDeclarationPM.CurrentContextTag as EventContextTagModel;

                //    if (myEventContextTagModel.EventCode.ToString() == "INR" || string.IsNullOrWhiteSpace(myEventContextTagModel.EventCode.ToString()))
                //    {
                //        //requestData = GetMyFUStatusXML("CFIFILEM", customFile.ToString(), "INR", "INR", "", DateTime.Now);
                //        requestData = GetMyFUStatusXML("INR", "INR", "", "new", DateTime.Now, false);
                //    }
                //    if (myEventContextTagModel.CallProccessID == EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate)
                //    {
                //        //requestData = GetMyFUStatusXML("CFIFILEM", customFile.ToString(), myEventContextTagModel.EventCode, "", "", myEventContextTagModel.StatusDateTime);
                //        requestData = GetMyFUStatusXML(myEventContextTagModel.EventCode, myEventContextTagModel.EventCode, "", "new", myEventContextTagModel.StatusDateTime, false);
                //    }
                //}

                SetCFIDATA();
                if (_CFIDATA != null)
                {
                    transmission mytransmission = GetTransmission(_CFIDATA, "AMITAL", "Customs additional data from logitude");
                    var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
                    requestData = xmltransmission;
                }
                if (_FromMessaging == true )
                {
                    var requestData2 = "";
                    bool isAmendmentRelease = false;
                    var myEventContextTagModel = new EventContextTagModel();
                    myEventContextTagModel = this._DirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
                    if (myEventContextTagModel != null)
                    {
                        if (!this._DirtyDeclarationPM.IsCourierDeclaration &&(  myEventContextTagModel.EventCode.ToString() == "INR" || string.IsNullOrWhiteSpace(myEventContextTagModel.EventCode.ToString())))
                        {
                            requestData2 = GetMyFUStatusXML("INR", "INR", "", "new", DateTime.Now, false);
                        }
                        if (myEventContextTagModel.CallProccessID == EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate)
                        {
                            requestData2 = GetMyFUStatusXML(myEventContextTagModel.EventCode, myEventContextTagModel.EventCode, "", "new", myEventContextTagModel.StatusDateTime, false);
                            if (!(string.IsNullOrEmpty(this._DirtyDeclarationPM.AmendmentOriginalDeclartation) && this._DirtyDeclarationPM.AmendmentDontDisplayInList == false))
                            {
                                isAmendmentRelease = true;
                            }
                        }
                    }
                    //if (myEventContextTagModel.CallProccessID == EventContextTagModel.ProccessEnum.MN_MSG4_SendManifestFeedBack_MessageResponseService)
                    //{
                    //    requestData2 = GetMyFUStatusXML(myEventContextTagModel.EventCode, myEventContextTagModel.EventRemarks, "", "new", myEventContextTagModel.StatusDateTime, false);
                    //}
                    if (!string.IsNullOrWhiteSpace(requestData2))
                    {
                        if (!string.IsNullOrWhiteSpace(requestData))
                        {
                            requestData = requestData.Replace("</transmission>", string.Concat(requestData2, "</transmission>"));

                        }
                        else
                        {
                            requestData = requestData2;
                        }
                    }
                    if (isAmendmentRelease) 
                    {
                        requestData = requestData.Replace("</transmission>", string.Concat("<GENERALQUERYMODE>AMENDMENTRELEASE</GENERALQUERYMODE>", "</transmission>"));
                    }
                }
                /*
                if (_DBOccDeclarationPM.IsValueForCustomsOnly != _DirtyDeclarationPM.IsValueForCustomsOnly)
                {
                    string xml_status = "new";
                    if (_DirtyDeclarationPM.IsValueForCustomsOnly != true) xml_status = "del";
                    var addStatusData = GetMyFUStatusXML("DFC", "DFC", "", xml_status, DateTime.Now, false); ;
                    requestData = string.Concat(requestData, addStatusData);
                }
                */
                if (!_IsCCUFILEM_4U2L_Changed && string.IsNullOrWhiteSpace(requestData))
                {
                    return;
                }

                var myYCULTASKPM = new YCULTASKPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    STATUS = "W",
                    REQUESTDATA = requestData,
                    ENTNAME = "CFIFILEM",
                    PRIMARYNUM = customFile.ToString(),
                    PRIORITY = YCULTASKPM.calcPriority("L2U"),//eitan h 12/3/15 new static operation
                    //PRIORITY = 1,
                    //TYPE = _IsMainSupplerChanged ? "L2UMAIN" : "L2U", // moran 22.5.16 AMI-56276 - undo for now - waiting for reply
                    TYPE = "L2U",
                    USRCODE = unifreightUser,
                    ARCHIVE = "F", // moran 28.6.16 - AMI-57170
                    //LOGTIME = (new DualQueryService(MainContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now,
                };

                //myYCULTASKPM.TASKID = CommCounterUtil.GetUnique30(myYCULTASKPM.LOGTIME);
                myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                var myGGGQPM = new GGGQPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ORIGINQUE = "LGT", //LugitudeRequest
                    STATUS = "1",
                    EXPTASKTIME = 5,
                    EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
                    TRY = 9,
                    PRIORITY = 8,
                    ENTNAME = "CFIFILEM",
                    PRIMARYNUM = customFile.ToString(),
                    FORMID = "LGT_UPDATE_FCI",
                    DEBUG = "F",
                    DONEOPERATION = "D",
                    //GSTRING1 = myYCULTASKPM.TASKID,
                };
                myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.

                //AmitalContext.DisableQuoting(false);

                myGGGQUpdateService.Update(myGGGQPM, true);
                //AmitalContext.DisableQuoting(true);

            }
            //Yuval Chalup 23.10.2014 TASK-6711 --->
        }

        private void SetCFIDATA()
        {
            if (_DirtyDeclarationPM.IsCourierDeclaration)
            {
                //Get CourierDeclaration
                CourierDeclarationQueryService myCourierDeclarationQueryService = new CourierDeclarationQueryService(_Context);
                CourierMasterPM courierMasterPM = null;
                CourierDeclarationPM courierDeclarationPM = myCourierDeclarationQueryService.GetCourierDeclarationByDeclarationId(_DirtyDeclarationPM.Id, _DirtyDeclarationPM.Tenant);
                if (courierDeclarationPM != null)
                {
                    //Get CourierMaster
                    CourierMasterQueryService myCourierMasterQueryService = new CourierMasterQueryService(_Context);
                    courierMasterPM = myCourierMasterQueryService.GetSingle(courierDeclarationPM.CourierMasterId, false, true);
                }
                _CFIDATA = new CFIDATA();
                List<CFIDATA_DATA> myCFIDATA_DATAList = new List<CFIDATA_DATA>();
                CFIDATA_DATA myCFIDATA_DATA = new CFIDATA_DATA();
                myCFIDATA_DATA.ImporterName = _DirtyDeclarationPM.ImporterName;
                myCFIDATA_DATA.ImporterAddress = _DirtyDeclarationPM.ImporterAddress;
                myCFIDATA_DATA.ImporterId = _DirtyDeclarationPM.ImporterCode;
                myCFIDATA_DATA.CasualSupplierName = _DirtyDeclarationPM.CasualSupplierName;
                myCFIDATA_DATA.CasualSupplierAddress = _DirtyDeclarationPM.CasualSupplierAddress;
                myCFIDATA_DATA.COUWTVAL = _DirtyDeclarationPM.WeightValue;
                myCFIDATA_DATA.CasualImporterAddress1 = _DirtyDeclarationPM.CasualImporterAddress1;
                myCFIDATA_DATA.CasualImporterAddress2 = _DirtyDeclarationPM.CasualImporterAddress2;
                myCFIDATA_DATA.CasualImporterCity = _DirtyDeclarationPM.CasualImporterCity;
                myCFIDATA_DATA.CasualImporterZipCode = _DirtyDeclarationPM.CasualImporterZipCode;
                myCFIDATA_DATA.CasualImporterFax = _DirtyDeclarationPM.CasualImporterFax;
                myCFIDATA_DATA.CasualImporterEmail = _DirtyDeclarationPM.CasualImporterEmail;
                myCFIDATA_DATA.CasualImportelTel = _DirtyDeclarationPM.CasualImporterTel;
                myCFIDATA_DATA.CasualImporterContact = _DirtyDeclarationPM.CasualImporterContact;
                if(_DirtyDeclarationPM.TotalInvoiceAmountInUSD != null)
                {
                    myCFIDATA_DATA.VALUE_IN_USD = _DirtyDeclarationPM.TotalInvoiceAmountInUSD.ToString();
                }

                if (_DirtyDeclarationPM.Consignments != null && _DirtyDeclarationPM.Consignments.Count() > 0)
                {
                    myCFIDATA_DATA.ManifestNumber = _DirtyDeclarationPM.Consignments[0].ManifestNumber;
                    myCFIDATA_DATA.ThirdCargoID = _DirtyDeclarationPM.Consignments[0].ThirdCargoID;
                    if (_DirtyDeclarationPM.Consignments[0].UnloadDate != null)
                    {
                        myCFIDATA_DATA.UNLOADDATE = _DirtyDeclarationPM.Consignments[0].UnloadDate.ToString();
                    }
                    else
                    {
                    //    myCFIDATA_DATA.ThirdCargoID = "";
                    }
                }
                if (courierMasterPM != null)
                {
                    if (!string.IsNullOrWhiteSpace(courierMasterPM.AirlineId))
                    {
                        //AirlineRepository airlineRepository = new AirlineRepository(_DirtyDeclarationPM.Tenant);
                        //Airline airline = airlineRepository.GetSingleAirline(courierMasterPM.AirlineId, _DirtyDeclarationPM.Tenant);
                        CustomsAirlineRepository airlineRepository = new CustomsAirlineRepository(_DirtyDeclarationPM.Tenant);
                        CustomsAirline airline = airlineRepository.GetSingle(courierMasterPM.AirlineId, _DirtyDeclarationPM.Tenant);
                        if (airline != null)
                        {
                            myCFIDATA_DATA.AirlineId = airline.AirlinePrefix;
                        }
                    }
                }
                if (courierMasterPM != null)
                {
                    myCFIDATA_DATA.MAWB = courierMasterPM.MAWB;
                    myCFIDATA_DATA.HAWB = courierMasterPM.HAWB;
                }
                
                myCFIDATA_DATAList.Add(myCFIDATA_DATA);
                _CFIDATA.CFIDATA_DATA = myCFIDATA_DATAList.ToArray();
            }
        }

        /*private string GetMyFUStatusXML(string entname, string primary_number, string status_id, string status_place, string comments, DateTime status_dateTime)
        {
            if (status_dateTime == null)
            {
                status_dateTime = DateTime.Now;
            }
            var myFUStatus = new AmitalEventTracerModel.FUStatus()
            {
                entname = entname,
                primary_number = primary_number,
                status = "new",
                xml_status = "new",
                status_id = status_id,
                status_DateTime = status_dateTime,
                //status_place = status_place,
                status_save = "no_fail",
                comments = comments,
            };

            var myAmitalEventTracerModel = new AmitalEventTracerModel();
            myAmitalEventTracerModel.MyFUStatus = myFUStatus;
            GFUSTS myGFUSTS = AmitalEventTracer.GetFUStatus(myAmitalEventTracerModel);
            //transmission mytransmission = GetTransmission(myGFUSTS, "AMITAL", "FU Status from logitude ");
            //var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
            var xmltransmission = XmlGenericUtil<GFUSTS>.SerializeObject(myGFUSTS, true);
            return xmltransmission;
        }*/

        private void RaiseNotificationEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, string eventCode, string statusCode, bool doNotSendStatus = false)
        {
            try
            {
                var eventContextTagModel = dirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
                if (eventContextTagModel != null)
                {
                    var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                    {

                        Tenant = dirtyDeclarationPM.Tenant,
                        objectTableName = "Customs.Declaration",
                        EventCode = eventCode,
                        notes = "",
                        CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                        EntityId = dirtyDeclarationPM.Id,
                        UserId = loggingUserId,
                        CommunicationSubject = "FU Status" + statusCode + " from logitude",
                        MyFUStatus = new AmitalEventTracerModel.FUStatus()
                        {
                            entname = "CFIFILEM",
                            primary_number = dirtyDeclarationPM.CustomFileNo,
                            status = "new",
                            xml_status = "new",
                            status_id = statusCode,
                            status_DateTime = DateTime.Now,
                            //status_save = "no_fail",
                            comments = "",
                        }
                    };

                    LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: EventCode= " + eventCode + "CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "  ");
                    AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, doNotSendStatus);
                }
            }
            catch (Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

        public  string GetMyFUStatusXML(string event_id, string status_id, string comments, string xmlStatus, DateTime statusDateTime, bool isRaiseEvent = false) // Mirit 25/05/15 Task 13521
        {
            string loggingUserId = this._LoggingUserId;
            if (String.IsNullOrWhiteSpace(xmlStatus))
            {
                xmlStatus = "new";
            }

            if (string.IsNullOrWhiteSpace(loggingUserId))
            {
                loggingUserId = AuthenticationUtil.ResolveUserId(this._DirtyDeclarationPM.Tenant);
            }

            if (isRaiseEvent)
            {
                RaiseNotificationEvent(this._DirtyDeclarationPM, loggingUserId, event_id, status_id, true);
            }

            var myFUStatus = new AmitalEventTracerModel.FUStatus()
            {
                entname = "CFIFILEM",
                primary_number = this._DirtyDeclarationPM.CustomFileNo,
                status = "new",
                xml_status = xmlStatus,
                status_id = status_id,
                status_DateTime = statusDateTime,
                //status_save = "no_fail",
                comments = comments,
            };

            var myAmitalStatusTracerModel = new AmitalEventTracerModel();
            myAmitalStatusTracerModel.Tenant = this._DirtyDeclarationPM.Tenant;
            myAmitalStatusTracerModel.UserId = loggingUserId;
            myAmitalStatusTracerModel.MyFUStatus = myFUStatus;
            GFUSTS myGFUSTS = AmitalEventTracer.GetFUStatus(myAmitalStatusTracerModel);
            var xml = XmlGenericUtil<GFUSTS>.SerializeObject(myGFUSTS, true);
            return xml;
        }


        transmission GetTransmission<T>(T mySerilazeObject, string from, string Subject)
            where T : class
            //transmission GetTransmission(CFIPACKS mySerilazeObject, string from, string Subject)
        {
            var CommunicationsParamsSubject = Subject;
            var mytransmission = new transmission();
            var mytransmission_details = new List<transmission_details>();
            var mytransmission_detail1 = new transmission_details()
            {
                sender = new sender() { Value = from },
                subject = new subject() { Value = CommunicationsParamsSubject }
            };

            var xml = XmlGenericUtil<T>.SerializeObject(mySerilazeObject, true);

            var myListdata = new List<data>() { new data() { entity = xml } };

            mytransmission.data = myListdata.ToArray();// GetDataList().ToArray();
            if (mytransmission.data.Count() < 1)
            {
                throw new Exception("(mytransmission.data.Count < 1)");
            }

            mytransmission_details.Add(mytransmission_detail1);
            mytransmission.transmission_details = mytransmission_details.ToArray();
            return mytransmission;
        }

        private void DoCustomFile()
        {
            var clientRepository = new ClientRepository(_DirtyDeclarationPM.Tenant);
            var cardRepository = new CardRepository(_DirtyDeclarationPM.Tenant);
            var userRepository = new UserRepository(_DirtyDeclarationPM.Tenant);
            var DepartmentRepository = new DepartmentRepository(_DirtyDeclarationPM.Tenant);
            string userCode = "";
            _loanAmount = 0; // moran 17.1.16 - Task 19798
            if (_CCUFILEMPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("DoCustomFile1: _CCUFILEMPM new record");
                userCode = GetUserCodeByID(_DirtyDeclarationPM.CreatedByUserId);
                _CCUFILEMPM = new CCUFILEMPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,

                    DeclarationId = _DirtyDeclarationPM.Id,
                    OPENDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now,
                    FILECLOSE = 0,
                    OPENBYUSER = userCode,
                    FROMIIG = "T",
                };
                LogMessagingUtil.Instance.AppendLine("DoCustomFile2: _CCUFILEMPM new record created, OPENBYUSER: " + userCode);
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("DoCustomFile3: _CCUFILEMPM record exist, file: " + _CCUFILEMPM.CUSTOMFILENO);
                _CCUFILEMPM.ChangeSetOp = ChangeSetOperation.Update;
            }

            _CCUFILEM4L2UPM_Before = CCUFILEM_4L2U_Mapping(_CCUFILEMPMwithCCUMSHGRP);

            //במסר התרה - יש לעדכן רק את שדות תאריך התרה + סטטוס הצהרה
            if (_CCUFILEMPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                EventContextTagModel eventContextTagModel = new EventContextTagModel();
                eventContextTagModel = _DirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
                if (eventContextTagModel != null && eventContextTagModel.CallProccessID == EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate)
                {
                    //_CCUFILEMPM.MEHESDRAFTSTATUS = _DirtyDeclarationPM.DeclarationStatusTypeCode.ToNullableInt("_DirtyDeclarationPM.DeclarationStatusTypeCode");
                    _CCUFILEMPM.MEHESDRAFTSTATUS = TranslateDeclarationStatusTypeCodeToUNF(_DirtyDeclarationPM.DeclarationStatusTypeCode);
                    _CCUFILEMPM.GRANTDATE = null;
                    _CCUFILEMPM.GRANTTIME = null;
                    if (_DirtyDeclarationPM.HatraDate.HasValue)
                    {
                        _CCUFILEMPM.GRANTDATE = _DirtyDeclarationPM.HatraDate.Value.Date;
                        //_CCUFILEMPM.GRANTTIME = new DateTime(_DirtyDeclarationPM.HatraDate.Value.TimeOfDay.Ticks);
                        _CCUFILEMPM.GRANTTIME = new DateTime(_EmptyDate.Year, _EmptyDate.Month, _EmptyDate.Day, _DirtyDeclarationPM.HatraDate.Value.Hour, _DirtyDeclarationPM.HatraDate.Value.Minute, 0, _DirtyDeclarationPM.HatraDate.Value.Kind);

                        _CCUFILEMPM.INDICATORS = "";
                    }
                    return;
                }
            }

            //Writing RESHIMON DATE and there is no HATARA DATE  ==> INDICATORS="G"
            if (_DirtyDeclarationPM.PaymentDate.HasValue && !_DirtyDeclarationPM.HatraDate.HasValue)
            {
                _CCUFILEMPM.INDICATORS = "G";
            }
            //Deleting RESHIMON DATE ==> INDICATORS=""
            if (!_DirtyDeclarationPM.PaymentDate.HasValue && _CCUFILEMPM.RESHMDATE.HasValue)
            {
                _CCUFILEMPM.INDICATORS = "";
            }
            //Writing HATARA DATE ==> INDICATORS=""
            if (_DirtyDeclarationPM.HatraDate.HasValue)
            {
                _CCUFILEMPM.INDICATORS = "";
            }

            //if(file!=null) file.WriteLine("Start Updating CCUFILEMPM " + _CCUFILEMPM.CUSTOMFILENO + ": " + DateTime.Now.ToString());
            _CCUFILEMPM.DeclarationId = _DirtyDeclarationPM.Id;
            _CCUFILEMPM.CUSTOMERID = null;
            if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPM.CustomerId))
            {
                Card myCard = cardRepository.GetSingleCardCache(_DirtyDeclarationPM.CustomerId, _DirtyDeclarationPM.Tenant);
                if (myCard != null)
                {
                    _CCUFILEMPM.CUSTOMERID = myCard.Code;
                }
            }

            _CCUFILEMPM.CUSTOMFILENO = lCUSTOMFILENO;

            _CCUFILEMPM.DRAWNO = null;
            _CCUFILEMPM.DRAWNON = null;
            if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPM.DeclarationNumber))
            {
                //_CCUFILEMPM.DRAWNO = _DirtyDeclarationPM.DeclarationNumber.Substring(0, Math.Min(9, _DirtyDeclarationPM.DeclarationNumber.Length)); //First 9 chars
                _CCUFILEMPM.DRAWNO = _DirtyDeclarationPM.DeclarationNumber.GetLast(9); //Last 9 chars
                _CCUFILEMPM.DRAWNON = _DirtyDeclarationPM.DeclarationNumber;
            }

            _CCUFILEMPM.RESHIMONTYPE = GetTranslationP2L("IIGC", "CTBRESHTYPE", _DirtyDeclarationPM.ProcedureCurrentCode);
            if (String.IsNullOrWhiteSpace(_CCUFILEMPM.RESHIMONTYPE))
            {
                _CCUFILEMPM.RESHIMONTYPE = "1";
            }
            _CCUFILEMPM.RESHIMONTYPEN = _DirtyDeclarationPM.ProcedureCurrentCode;

            _CCUFILEMPM.RESHIMONNO = null;
            _CCUFILEMPM.RESHIMONNON = null;
            if (_DirtyDeclarationPM.PaymentDate.HasValue)
            {
                if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPM.DeclarationNumber))
                {
                    //_CCUFILEMPM.RESHIMONNO = _DirtyDeclarationPM.DeclarationNumber.Substring(0, Math.Min(9, _DirtyDeclarationPM.DeclarationNumber.Length)); //First 9 chars
                    //_CCUFILEMPM.RESHIMONNO = _DirtyDeclarationPM.DeclarationNumber.GetLast(9);
                    //Last 9 digits after removing last digit
                    var DeclarationNumber = _DirtyDeclarationPM.DeclarationNumber.Remove(_DirtyDeclarationPM.DeclarationNumber.Length - 1, 1);
                    _CCUFILEMPM.RESHIMONNO = DeclarationNumber.GetLast(9);
                }
                _CCUFILEMPM.RESHIMONNON = _DirtyDeclarationPM.DeclarationNumber;
            }

            _CCUFILEMPM.DRAFTDATE = null;
            if (_DirtyDeclarationPM.TaxationDateTime.HasValue)
            {
                _CCUFILEMPM.DRAFTDATE = _DirtyDeclarationPM.TaxationDateTime.Value.Date;
            }

            _CCUFILEMPM.AUTONOMY = null;
            switch (_DirtyDeclarationPM.AutonomyRegionTypeCode)
            {
                case "70":
                    _CCUFILEMPM.AUTONOMY = "3";
                    break;
                case "80":
                    _CCUFILEMPM.AUTONOMY = "1";
                    break;
                default:
                    break;
            }

            _CCUFILEMPM.CUSTOMAGENT = GetDefault("ISRAEL", "GGG_CUSTOM_AGT", "NON", "NON");
            _CCUFILEMPM.CUSTOMSBRANCH = _DirtyDeclarationPM.DeclarationOfficeCode;

            _CCUFILEMPM.IMPORTERID = null;
            _CCUFILEMPM.TRANSIMPORTID = null;

            ClientQueryService myClientQueryService = new ClientQueryService(_DirtyDeclarationPM.Tenant);
            var imporetr = "";
            var transferImporter = "";
            var entitleImporter = "";
            //Find Importer
            if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.ImporterId))
            {
                ClientPM myClientPM = myClientQueryService.GetSingle(_DirtyDeclarationPM.ImporterId, false, false);
                if (myClientPM != null)
                {
                    imporetr = myClientPM.Code;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.ImporterCode))
                    {
                        imporetr = _DirtyDeclarationPM.ImporterCode;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.ImporterCode))
                {
                    imporetr = _DirtyDeclarationPM.ImporterCode;
                }
            }

            //Find TransferImporter
            if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.TransferImporterId))
            {
                ClientPM myClientPM = myClientQueryService.GetSingle(_DirtyDeclarationPM.TransferImporterId, false, false);
                if (myClientPM != null)
                {
                    transferImporter = myClientPM.Code;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.TransferImporterCode))
                    {
                        transferImporter = _DirtyDeclarationPM.TransferImporterCode;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.TransferImporterCode))
                {
                    transferImporter = _DirtyDeclarationPM.TransferImporterCode;
                }
            }

            //Find EntitleImporter
            if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.EntitleImporterId))
            {
                ClientPM myClientPM = myClientQueryService.GetSingle(_DirtyDeclarationPM.EntitleImporterId, false, false);
                if (myClientPM != null)
                {
                    entitleImporter = myClientPM.Code;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.EntitleImporterCode))
                    {
                        entitleImporter = _DirtyDeclarationPM.EntitleImporterCode;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.EntitleImporterCode))
                {
                    entitleImporter = _DirtyDeclarationPM.EntitleImporterCode;
                }
            }
            //If there is Entitle Importer: Importer ==> Transfer Importer AND Entitle Importer ==> Importer
            if (!string.IsNullOrWhiteSpace(entitleImporter))
            {
                entitleImporter = new String(entitleImporter.Where(Char.IsDigit).ToArray());
                _CCUFILEMPM.IMPORTERID = entitleImporter.GetLast(9);
                if (!string.IsNullOrWhiteSpace(imporetr))
                {
                    imporetr = new String(imporetr.Where(Char.IsDigit).ToArray());
                    _CCUFILEMPM.TRANSIMPORTID = imporetr.GetLast(9);
                }
            }
            //If there is NO Entitle Importer: Importer ==> Importer AND Transfer Importer ==> Transfer Importer
            else
            {
                if (!string.IsNullOrWhiteSpace(imporetr))
                {
                    imporetr = new String(imporetr.Where(Char.IsDigit).ToArray());
                    _CCUFILEMPM.IMPORTERID = imporetr.GetLast(9);
                }
                if (!string.IsNullOrWhiteSpace(transferImporter))
                {
                    transferImporter = new String(transferImporter.Where(Char.IsDigit).ToArray());
                    _CCUFILEMPM.TRANSIMPORTID = transferImporter.GetLast(9);
                }
            }

            _CCUFILEMPM.RIGHTOWNID = null;
            if(!string.IsNullOrWhiteSpace(_DirtyDeclarationPM.ImporterEntitlementTypeCode))
            {
                short importerEntitlementTypeCode = 0;
                var rightownid = GetTranslationP2L("IIGC", "CTBRGOWN", _DirtyDeclarationPM.ImporterEntitlementTypeCode);
                if (short.TryParse(rightownid, out importerEntitlementTypeCode))
                {
                    _CCUFILEMPM.RIGHTOWNID = importerEntitlementTypeCode;
                }
            }

            if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPM.DepartmentId))
            {
                Department myDepartment = DepartmentRepository.GetSingleDepartment(_DirtyDeclarationPM.DepartmentId, _DirtyDeclarationPM.Tenant);
                if (myDepartment != null)
                {
                    _CCUFILEMPM.DEPARTID = myDepartment.Code;
                }
            }

            _CCUFILEMPM.OPENDATE = _DirtyDeclarationPM.CreateDateTime;
            _CCUFILEMPM.OPENBYUSER = null;
            if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPM.CreatedByUserId))
            {
                User myUser = userRepository.GetSingleUser(_DirtyDeclarationPM.CreatedByUserId, _DirtyDeclarationPM.Tenant, true);
                if (myUser != null)
                {
                    _CCUFILEMPM.OPENBYUSER = myUser.Code;
                    if(!String.IsNullOrWhiteSpace(myUser.BranchId))
                    {
                        BranchRepository branchRepository = new BranchRepository(_DirtyDeclarationPM.Tenant);
                        Branch myBranch = branchRepository.GetSingleBranch(myUser.BranchId, _DirtyDeclarationPM.Tenant);
                        if(myBranch != null && myBranch.Code != null)
                        {
                            _CCUFILEMPM.BRANCHID = myBranch.Code;
                        }
                    }
                }
            }
            _CCUFILEMPM.CHANGE = (_DirtyDeclarationPM.IsChanged) ? "T" : "F";

            _CCUFILEMPM.RESHMDATE = null;
            if (_DirtyDeclarationPM.PaymentDate.HasValue)
            {
                _CCUFILEMPM.RESHMDATE = _DirtyDeclarationPM.PaymentDate.Value.Date;
            }

            //_CCUFILEMPM.PRICEINDEX = _DirtyDeclarationPM.LoadingFactor.ToNullableDouble("_DirtyDeclarationPM.LoadingFactor"); //Yuval Chalup 20.09.2015 TASK-16472 (Comment)

            _CCUFILEMPM.GRANTDATE = null;
            _CCUFILEMPM.GRANTTIME = null;
            if (_DirtyDeclarationPM.HatraDate.HasValue)
            {
                _CCUFILEMPM.GRANTDATE = _DirtyDeclarationPM.HatraDate.Value.Date;
                //_CCUFILEMPM.GRANTTIME = new DateTime(_DirtyDeclarationPM.HatraDate.Value.TimeOfDay.Ticks);
                _CCUFILEMPM.GRANTTIME = new DateTime(_EmptyDate.Year, _EmptyDate.Month, _EmptyDate.Day, _DirtyDeclarationPM.HatraDate.Value.Hour, _DirtyDeclarationPM.HatraDate.Value.Minute, 0, _DirtyDeclarationPM.HatraDate.Value.Kind);
            }

            _CCUFILEMPM.CIFVALUE = _DirtyDeclarationPM.CIFValue.ToNullableDouble("_DirtyDeclarationPM.CIFValue");
            _CCUFILEMPM.ACCEPTEDPRICE = _DirtyDeclarationPM.DealValue.ToNullableDouble("_DirtyDeclarationPM.DealValue");
            _CCUFILEMPM.TOTALTAX = _DirtyDeclarationPM.TotalTax.ToNullableDouble("_DirtyDeclarationPM.TotalTax");
            _CCUFILEMPM.GOODSVALUE = _DirtyDeclarationPM.DealValueWithoutFactor.ToNullableDouble("_DirtyDeclarationPM.DealValueWithoutFactor");
            //_CCUFILEMPM.MEHESDRAFTSTATUS = _DirtyDeclarationPM.DeclarationStatusTypeCode.ToNullableInt("_DirtyDeclarationPM.DeclarationStatusTypeCode"); // moran 9.2.15 - Task 1613
            _CCUFILEMPM.MEHESDRAFTSTATUS = TranslateDeclarationStatusTypeCodeToUNF(_DirtyDeclarationPM.DeclarationStatusTypeCode);

            _CCUFILEMPM.PAYDATE = null;
            _CCUFILEMPM.PAYTIME = null;
            if (_DirtyDeclarationPM.PaymentDate.HasValue)
            {
                _CCUFILEMPM.PAYDATE = _DirtyDeclarationPM.PaymentDate.Value.Date;
                //_CCUFILEMPM.PAYTIME = new DateTime(_DirtyDeclarationPM.PaymentDate.Value.TimeOfDay.Ticks);
                _CCUFILEMPM.PAYTIME = new DateTime(_EmptyDate.Year, _EmptyDate.Month, _EmptyDate.Day, _DirtyDeclarationPM.PaymentDate.Value.Hour, _DirtyDeclarationPM.PaymentDate.Value.Minute, 0, _DirtyDeclarationPM.PaymentDate.Value.Kind);

            }

            //if (_IsSupplerInvChanged || _IsSupplerInvUpdateCCUFILEM) //Yuval Chalup 13.02.2016 TASK-20599 (Add || _IsSupplerInvUpdateCCUFILEM)
            //if (_IsSupplerInvChanged) // moran 1.8.16 - Task 21737 - commented - do always
            {
                //_CCUFILEMPM.FEEPLATFORM = 0; //This is to be done in a full saving mode ONLY (Moved to DoSupplierInvoices())
                //_CCUFILEMPM.EXPENSEVALUE = 0; //This is to be done in a full saving mode ONLY (Moved to DoSupplierInvoices())
                _CCUFILEMPM.REGIONVALUE = 0;
                //_CCUFILEMPMSupplierInvoiceModificationsI01 = 0; //This is to be done in a full saving mode ONLY (Moved to DoSupplierInvoices())
                //_CCUFILEMPMSupplierInvoiceModifications527 = 0; //This is to be done in a full saving mode ONLY (Moved to DoSupplierInvoices())
                //_CCUFILEMPM.CHANGINGVALUE = 0; //This is to be done in a full saving mode ONLY (Moved to DoSupplierInvoices())
                //_CCUFILEMPM.SERVICEVALUE = 0; //This is to be done in a full saving mode ONLY (Moved to DoSupplierInvoices())
                //if(file!=null) file.WriteLine("Start DoSupplierInvoices() " + _CCUFILEMPM.CUSTOMFILENO + ": " + DateTime.Now.ToString());
                DoSupplierInvoices();
                //if Supplier Invoice Modifications of type I02 is empty - Take Type I01
                //if (_CCUFILEMPM.FEEPLATFORM == 0)  //This is to be done in a full saving mode ONLY (Moved to DoSupplierInvoices())
                //{
                //   _CCUFILEMPM.FEEPLATFORM = _CCUFILEMPMSupplierInvoiceModificationsI01;
                //}

                /*Double? amountDouble1 = Transfer(_CCUFILEMPM.EXPENSEVALUE.ToNullableDecimal("_CCUFILEMPM.EXPENSEVALUE"), "ILS", _CCUFILEMPM.COINIDN, null); // Mirit 02/05/16 Task 20995 - Remove lines
                Double? amountDouble2 = Transfer(_CCUFILEMPMSupplierInvoiceModifications527.ToNullableDecimal("_CCUFILEMPMSupplierInvoiceModifications527"), "ILS", _CCUFILEMPM.COINIDN, null);

                _CCUFILEMPM.CHANGINGVALUE = _CCUFILEMPM.INDEXVALUE.ToNullableDouble("_CCUFILEMPM.INDEXVALUE") + amountDouble1 - amountDouble2;*/
            }
            if (_IsConsignmentChanged)
            {
                //if(file!=null) file.WriteLine("Start DoConsignments() " + _CCUFILEMPM.CUSTOMFILENO + ": " + DateTime.Now.ToString());
                DoConsignments();
            }
            if (_IsDeclarationTaxesChanged)
            {
                //if(file!=null) file.WriteLine("Start DoDeclarationTaxes() " + _CCUFILEMPM.CUSTOMFILENO + ": " + DateTime.Now.ToString());
                DoDeclarationTaxes();
            }
            _CCUFILEMPM.LOANAMOUNT = _loanAmount; // moran 17.1.16 - Task 19798

            //Calculate Total Transport Value & Currency // Mirit 20/11/17 Task 33909
            CalculateTotalTransport();

            _CCUFILEM4L2UPM_After = CCUFILEM_4L2U_Mapping(_CCUFILEMPM);
            _CCUTAX_4LD2UM_After = CCUTAX_4LD2U_Mapping(_CCUFILEMPM.CCUTAXPM);
        }

        private CCUFILEM_4L2U CCUFILEM_4L2U_Mapping(CCUFILEMPM myCCUFILEMPM)
        {
            if (myCCUFILEMPM == null)
            {
                return null;
            }

            CCUFILEM_4L2U myCCUFILEM4L2UPM = new CCUFILEM_4L2U()
            {
                FILENO = myCCUFILEMPM.FILENO,
                CIFVALUE = myCCUFILEMPM.CIFVALUE,
                CUSTOMSBRANCH = (string.IsNullOrWhiteSpace(myCCUFILEMPM.CUSTOMSBRANCH)) ? null : myCCUFILEMPM.CUSTOMSBRANCH,
                GRANTDATE = myCCUFILEMPM.GRANTDATE,
                MEHESDRAFTSTATUS = myCCUFILEMPM.MEHESDRAFTSTATUS,
                RESHIMONNON = (string.IsNullOrWhiteSpace(myCCUFILEMPM.RESHIMONNON)) ? null : myCCUFILEMPM.RESHIMONNON,
                RESHIMONTYPEN = (string.IsNullOrWhiteSpace(myCCUFILEMPM.RESHIMONTYPEN)) ? null : myCCUFILEMPM.RESHIMONTYPEN,
                RESHMDATE = myCCUFILEMPM.RESHMDATE,
                TOTALTAX = myCCUFILEMPM.TOTALTAX,
                PAYDATE = myCCUFILEMPM.PAYDATE,
                PAYTIME = myCCUFILEMPM.PAYTIME,
            };

            if (myCCUFILEMPM.CCUMSHGRs != null && myCCUFILEMPM.CCUMSHGRs.Count > 0)
            {
                foreach (var myCCUMSHGR in myCCUFILEMPM.CCUMSHGRs)
                {
                    CCUMSHGR_4L2U myCCUMSHGR_4L2U = new CCUMSHGR_4L2U()
                    {
                        DESCOFGOODS1 = (string.IsNullOrWhiteSpace(myCCUMSHGR.DESCOFGOODS1)) ? null : myCCUMSHGR.DESCOFGOODS1,
                        DESCOFGOODS2 = (string.IsNullOrWhiteSpace(myCCUMSHGR.DESCOFGOODS2)) ? null : myCCUMSHGR.DESCOFGOODS2,
                        DESCOFGOODS3 = (string.IsNullOrWhiteSpace(myCCUMSHGR.DESCOFGOODS3)) ? null : myCCUMSHGR.DESCOFGOODS3,
                        EXPORTLANDN = (string.IsNullOrWhiteSpace(myCCUMSHGR.EXPORTLANDN)) ? null : myCCUMSHGR.EXPORTLANDN,
                        FIRSTCARGOID = (string.IsNullOrWhiteSpace(myCCUMSHGR.FIRSTCARGOID)) ? null : myCCUMSHGR.FIRSTCARGOID,
                        HAWB = (string.IsNullOrWhiteSpace(myCCUMSHGR.HAWB)) ? null : myCCUMSHGR.HAWB,
                        HAWBDATE = myCCUMSHGR.HAWBDATE,
                        HAWBN = (string.IsNullOrWhiteSpace(myCCUMSHGR.HAWBN)) ? null : myCCUMSHGR.HAWBN,
                        IDENTIFIERNO = (string.IsNullOrWhiteSpace(myCCUMSHGR.IDENTIFIERNO)) ? null : myCCUMSHGR.IDENTIFIERNO,
                        LOADPORTID = (string.IsNullOrWhiteSpace(myCCUMSHGR.LOADPORTID)) ? null : myCCUMSHGR.LOADPORTID,
                        PACKTYPEIDN = (string.IsNullOrWhiteSpace(myCCUMSHGR.PACKTYPEIDN)) ? null : myCCUMSHGR.PACKTYPEIDN,
                        QUANTITY = myCCUMSHGR.QUANTITY,
                        SECONDCARGOID = (string.IsNullOrWhiteSpace(myCCUMSHGR.SECONDCARGOID)) ? null : myCCUMSHGR.SECONDCARGOID,
                        UNLOADDATE = myCCUMSHGR.UNLOADDATE,
                        UNLOADPORTID = (string.IsNullOrWhiteSpace(myCCUMSHGR.UNLOADPORTID)) ? null : myCCUMSHGR.UNLOADPORTID,
                        WAREHOUSEIDN = (string.IsNullOrWhiteSpace(myCCUMSHGR.WAREHOUSEIDN)) ? null : myCCUMSHGR.WAREHOUSEIDN,
                        WEIGHT = myCCUMSHGR.WEIGHT,
                    };
                    myCCUFILEM4L2UPM.CCUMSHGRs.Add(myCCUMSHGR_4L2U);
                }
            }

            return myCCUFILEM4L2UPM;
        }

        private List<CCUTAX_4LD2U> CCUTAX_4LD2U_Mapping(List<CCUTAXPM> myCCUTAXPMList)
        {
            List<CCUTAX_4LD2U> myCCUTAX_4LD2U_List = new List<Unifreight.BL.EntityPMs.CCUTAX_4LD2U>();
            if (myCCUTAXPMList != null && myCCUTAXPMList.Count > 0)
            {
                foreach (var myCCUTAXPM in myCCUTAXPMList)
                {
                    CCUTAX_4LD2U myCCUTAX_4LD2U = new CCUTAX_4LD2U()
                    {
                        ADDEFINEDTAX = myCCUTAXPM.ADDEFINEDTAX,
                        ADDIMPORT = myCCUTAXPM.ADDIMPORT,
                        ADDTAXRATE = myCCUTAXPM.ADDTAXRATE,
                        DEFINEDTAX = myCCUTAXPM.DEFINEDTAX,
                        GOODSNO = myCCUTAXPM.GOODSNO,
                        POSTPONEDTAX = myCCUTAXPM.POSTPONEDTAX,
                        PRATMEHES = (string.IsNullOrWhiteSpace(myCCUTAXPM.PRATMEHES)) ? null : myCCUTAXPM.PRATMEHES,
                        PRATMEHESN = (string.IsNullOrWhiteSpace(myCCUTAXPM.PRATMEHESN)) ? null : myCCUTAXPM.PRATMEHESN,
                        TAXAMOUNT = myCCUTAXPM.TAXAMOUNT,
                        TAXBASIS = myCCUTAXPM.TAXBASIS,
                        TAXCALCCODE = myCCUTAXPM.TAXCALCCODE,
                        TAXRATE = myCCUTAXPM.TAXRATE,
                        TAXTOPAY = myCCUTAXPM.TAXTOPAY,
                        TAXTYPE = (string.IsNullOrWhiteSpace(myCCUTAXPM.TAXTYPE)) ? null : myCCUTAXPM.TAXTYPE,
                        TAXTYPEN = (string.IsNullOrWhiteSpace(myCCUTAXPM.PRATMEHESN)) ? null : myCCUTAXPM.PRATMEHESN,
                    };
                    myCCUTAX_4LD2U_List.Add(myCCUTAX_4LD2U);
                }
            }
            return myCCUTAX_4LD2U_List;
        }

        private int? TranslateDeclarationStatusTypeCodeToUNF(string declarationStatusTypeCode)
        {
            int? draftstatus = null;
            if (string.IsNullOrWhiteSpace(declarationStatusTypeCode))
            {
                return draftstatus;
            }

            switch (declarationStatusTypeCode)
            {
                case "0":
                case "1":
                case "9":
                case "17":
                    draftstatus = 0;
                    break;
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "10":
                case "22":
                case "23":
                case "24":
                case "25":
                case "29":
                case "30":
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                    draftstatus = 4;
                    break;
                case "11":
                    draftstatus = 6;
                    break;
                case "12":
                    draftstatus = 1;
                    break;
                case "13":
                case "14":
                case "21":
                    draftstatus = 2;
                    break;
                case "15":
                case "16":
                case "18":
                    draftstatus = null;
                    break;
                case "7":
                case "8":
                case "19":
                case "20":
                    draftstatus = 8;
                    break;
                case "26":
                case "27":
                case "28":
                    draftstatus = 12;
                    break;
                default:
                    break;
            }

            return draftstatus;
        }

        private void CalculateTotalTransport()
        {
            _CCUFILEMPM.TRANSPVALFC = 0;
            _CCUFILEMPM.TRANCURRENCYN = null;

            //Calculate Total Transport Value & Currency
            List<string> freightCurrencyList = (from a in _DirtyDeclarationPM.SupplierInvoices
                                                where (a.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete)
                                                select a.FreightCurrencyTypeCode).Distinct().ToList();

            if (freightCurrencyList.Count == 1)
            {
                decimal totalFreightList = 0;
                foreach (var decSupplierInvoice in _DirtyDeclarationPM.SupplierInvoices)
                {
                    if (decSupplierInvoice.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete
                        && decSupplierInvoice.TotalFreightInFreightCurrency != null)
                    {
                        totalFreightList = totalFreightList + (decimal)decSupplierInvoice.TotalFreightInFreightCurrency;
                    }
                }
                _CCUFILEMPM.TRANSPVALFC = (double)totalFreightList;
                _CCUFILEMPM.TRANCURRENCYN = freightCurrencyList[0];
            }
        }

        private string GetUserCodeByID(string userID)
        {
            var userRepository = new UserRepository(_DirtyDeclarationPM.Tenant);
            string userCode = "";

            if (_CCUFILEMPM == null)
            {
                User myUser = userRepository.GetSingleUser(userID, _DirtyDeclarationPM.Tenant, true);
                if (myUser != null)
                {
                    userCode = myUser.Code;
                }
            }
            return (userCode);
        }

        private void DoDeclarationTaxes()
        {
            if (_DirtyDeclarationPM.DeclarationTaxes == null)
            {
                return;
            }
            if (_DirtyDeclarationPM.DeclarationTaxes.Count < 1)
            {
                return;
            }

            foreach (var decDeclarationTaxes in _DirtyDeclarationPM.DeclarationTaxes)
            {
                switch (decDeclarationTaxes.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        _CCUFILEMPM.CCUTAXPM.Add(SetDeclarationTaxes(decDeclarationTaxes));
                        break;
                }
            }
        }

        private CCUTAXPM SetDeclarationTaxes(DeclarationTaxPM decDeclarationTaxes)
        {
            CCUTAXPM cCUTAXPM = new CCUTAXPM();
            cCUTAXPM.ChangeSetOp = ChangeSetOperation.Insert;

            cCUTAXPM.FILENO = _CCUFILEMPM.FILENO;
            cCUTAXPM.TAXTYPE = GetTranslationP2L("IIGC", "CTBTAXTYPE", decDeclarationTaxes.TaxTypeCode);
            cCUTAXPM.TAXTYPEN = decDeclarationTaxes.TaxTypeCode;
            //cCUTAXPM.TAXAMOUNT = decDeclarationTaxes.TotalAmount.ToNullableDouble("decDeclarationTaxes.TotalAmount");
            cCUTAXPM.TAXAMOUNT = decDeclarationTaxes.TotalAmount.ToNullableDouble("decDeclarationTaxes.TotalAmount") + decDeclarationTaxes.DeferredTaxAmount.ToNullableDouble("decDeclarationTaxes.DeferredTaxAmount");
            //cCUTAXPM.TAXTOPAY = cCUTAXPM.TAXAMOUNT - decDeclarationTaxes.DeferredTaxAmount.ToNullableDouble("decDeclarationTaxes.DeferredTaxAmount");
            cCUTAXPM.TAXTOPAY = decDeclarationTaxes.TotalAmount.ToNullableDouble("decDeclarationTaxes.TotalAmount");
            cCUTAXPM.POSTPONEDTAX = decDeclarationTaxes.DeferredTaxAmount.ToNullableDouble("decDeclarationTaxes.DeferredTaxAmount");
            cCUTAXPM.TAXBASIS = decDeclarationTaxes.TaxBaseAmount.ToNullableDouble("decDeclarationTaxes.TaxBaseAmount");

            return cCUTAXPM;
        }

        private void DoConsignments()
        {
            if (_DirtyDeclarationPM.Consignments == null)
            {
                return;
            }
            if (_DirtyDeclarationPM.Consignments.Count < 1)
            {
                return;
            }

            foreach (var decConsignment in _DirtyDeclarationPM.Consignments)
            {
                switch (decConsignment.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        _CCUFILEMPM.CCUMSHGRs.Add(SetMishgurim(decConsignment));
                        break;
                }
            }
        }

        private CCUMSHGRPM SetMishgurim(ConsignmentPM decConsignment)
        {
            Unifreight.BL.EntityPMs.CCUMSHGRPM cCUMSHGRPM = new Unifreight.BL.EntityPMs.CCUMSHGRPM();
            cCUMSHGRPM.ChangeSetOp = ChangeSetOperation.Insert;

            if (decConsignment.SequenceNumeric.HasValue)
            {
                cCUMSHGRPM.MISHGORNO = decConsignment.SequenceNumeric.ToString();
            }

            if (!string.IsNullOrWhiteSpace(decConsignment.ManifestNumber))
            {
                //if (_DirtyDeclarationPM.TransportModeId != "A") //Yuval Chalup 27.04.2014 TASK-12799 (Add IF)
                //{
                //    cCUMSHGRPM.MANIFESTNO = decConsignment.ManifestNumber.Substring(decConsignment.ManifestNumber.Length - Math.Min(6, decConsignment.ManifestNumber.Length), Math.Min(6, decConsignment.ManifestNumber.Length)); //Last 6 chars
                //}
                //else // moran 9.9.15 - Task 16436
                //{
                //    cCUMSHGRPM.MANIFESTNO = decConsignment.ManifestNumber;
                //}


                //cCUMSHGRPM.MANIFESTNO = decConsignment.ManifestNumber.Substring(decConsignment.ManifestNumber.Length - Math.Min(6, decConsignment.ManifestNumber.Length), Math.Min(6, decConsignment.ManifestNumber.Length)); //Last 6 chars
                //<--- New logic: TASK-34997
                cCUMSHGRPM.MANIFESTNO = "";
                if (_DirtyDeclarationPM.TransportModeId == "A") // From SecondCargoID: 3 digits before "-"
                {
                    if (!string.IsNullOrWhiteSpace(decConsignment.SecondCargoID))
                    {
                        int index = decConsignment.SecondCargoID.IndexOf("-");
                        if (index > 0)
                        {
                            cCUMSHGRPM.MANIFESTNO = decConsignment.SecondCargoID.Substring(0, Math.Min(3, index));
                        }
                    }
                }
                else if (_DirtyDeclarationPM.TransportModeId == "O") // ManifestNumber
                {
                    if (!string.IsNullOrWhiteSpace(decConsignment.ManifestNumber))
                    {
                        int length = decConsignment.ManifestNumber.Length - 1;
                        cCUMSHGRPM.MANIFESTNO = decConsignment.ManifestNumber.Substring(0, Math.Min(6, decConsignment.ManifestNumber.Length));
                    }
                }
                else if (_DirtyDeclarationPM.TransportModeId == "L") // From ManifestNumber: digits 2-7 (total 6 digits)
                {
                    if (!string.IsNullOrWhiteSpace(decConsignment.ManifestNumber) && decConsignment.ManifestNumber.Length > 1)
                    {
                        int length = decConsignment.ManifestNumber.Length - 1;
                        cCUMSHGRPM.MANIFESTNO = decConsignment.ManifestNumber.Substring(1, Math.Min(6, length));
                    }
                }
                //New logic: TASK-34997 --->


                cCUMSHGRPM.FIRSTCARGOID = decConsignment.ManifestNumber; // moran 30.5.16 - Task 21602
            }

            cCUMSHGRPM.IDENTIFIERTYPE = GetTranslationP2L("IIGC", "CTBIDNTP", decConsignment.CargoTypeCode);
            cCUMSHGRPM.IDENTIFIERTYPEN = decConsignment.CargoTypeCode; //Yuval Chalup 17.04.2016 AMI-56338

            cCUMSHGRPM.HAWB = "";
            cCUMSHGRPM.HAWBN = "";
            if (_DirtyDeclarationPM.TransportModeId == "A")
            {
                if (!String.IsNullOrWhiteSpace(decConsignment.ThirdCargoID))
                {
                    if (decConsignment.CargoTypeCode == "17")
                    {
                        cCUMSHGRPM.HAWB = "";
                        cCUMSHGRPM.HAWBN = "";
                    }
                    else
                    {
                        //cCUMSHGRPM.HAWB = decConsignments.ThirdCargoID.Substring(0, Math.Min(8, decConsignments.ThirdCargoID.Length)); //First 8 chars;
                        cCUMSHGRPM.HAWB = decConsignment.ThirdCargoID.GetLast(8);
                        cCUMSHGRPM.HAWB = Regex.Replace(cCUMSHGRPM.HAWB, "[^0-9]", "");
                        cCUMSHGRPM.HAWBN = decConsignment.ThirdCargoID;
                    }
                }
            }

            if (_DirtyDeclarationPM.TransportModeId == "O")
            {
                if (!String.IsNullOrWhiteSpace(decConsignment.ThirdCargoID))
                {
                    if (decConsignment.CargoTypeCode == "11" || decConsignment.CargoTypeCode == "20")
                    {
                        cCUMSHGRPM.HAWB = decConsignment.ThirdCargoID.GetLast(8);
                        cCUMSHGRPM.HAWB = Regex.Replace(cCUMSHGRPM.HAWB, "[^0-9]", "");
                        cCUMSHGRPM.HAWBN = decConsignment.ThirdCargoID;
                    }
                }
            }
            cCUMSHGRPM.HAWBDATE = null;
            if (decConsignment.ManifestDate.HasValue && decConsignment.CargoTypeCode != "17")
            {
                cCUMSHGRPM.HAWBDATE = decConsignment.ManifestDate.Value.Date;
            }
            if (!string.IsNullOrWhiteSpace(decConsignment.ThirdCargoID) && decConsignment.CargoTypeCode == "17")
            {
                cCUMSHGRPM.HAWBDATE = AmitalConvertUtil.GetUnifreightFormatedDate(decConsignment.ThirdCargoID, "decConsignment.ThirdCargoID");
            }
            if (!_DirtyDeclarationPM.IsCourierDeclaration)
            {
                if (_DirtyDeclarationPM.TransportModeId == "A")
                {
                    if (!string.IsNullOrWhiteSpace(cCUMSHGRPM.HAWB))
                    {
                        //cCUMSHGRPM.HAWBDATE = decConsignment.
                    }
                    else
                    {

                    }
                }
            }

            cCUMSHGRPM.CARNETNUMBER = "";
            cCUMSHGRPM.IDENTIFIERNO = "";
            if (!String.IsNullOrWhiteSpace(decConsignment.SecondCargoID))
            {
                if (_DirtyDeclarationPM.TransportModeId == "A")
                {
                    if (decConsignment.CargoTypeCode == "17")
                    {
                        //cCUMSHGRPM.IDENTIFIERNO = "";
                    }
                    else
                    {
                        //cCUMSHGRPM.IDENTIFIERNO = decConsignments.SecondCargoID.GetLast(9); //Yuval Chalup 02.04.2015 TASK-12348
                        //<--- Yuval Chalup 20.04.2015 TASK-12632 - Take the last 9 digit AFTER the '-' (if tehre is)
                        int index = decConsignment.SecondCargoID.IndexOf("-") + 1;
                        string secondCargoID = decConsignment.SecondCargoID.Substring(index);
                        cCUMSHGRPM.IDENTIFIERNO = secondCargoID.GetLast(9);
                        //Yuval Chalup 20.04.2015 TASK-12632 --->
                    }
                    cCUMSHGRPM.TRANSPTYPE = "04";
                }
                if (_DirtyDeclarationPM.TransportModeId == "O" || _DirtyDeclarationPM.TransportModeId == "L")
                {
                    var secondCargoID = decConsignment.SecondCargoID;
                    if (secondCargoID.StartsWith("I"))
                    {
                        secondCargoID = secondCargoID.Substring(1);
                    }
                    cCUMSHGRPM.CARNETNUMBER = secondCargoID.Substring(0, Math.Min(9, secondCargoID.Length));
                    cCUMSHGRPM.TRANSPTYPE = "01";
                    if (decConsignment.CargoTypeCode == "11" || decConsignment.CargoTypeCode == "20")
                    {
                        //cCUMSHGRPM.IDENTIFIERNO = decConsignment.;
                    }
                }

                if (decConsignment.CargoTypeCode != "8" && decConsignment.CargoTypeCode != "20")
                {
                    cCUMSHGRPM.SECONDCARGOID = decConsignment.SecondCargoID; // moran 30.5.16 - Task 21602
                }
            }

            if (decConsignment.CargoTypeCode == "8")
            {
                if (!string.IsNullOrWhiteSpace(decConsignment.ManifestNumber))
                {
                    //If ManifestNumber is a converted Reshimon 
                    if (decConsignment.ManifestNumber.Length >= 4 && decConsignment.ManifestNumber.Substring(2, 2) == "99")
                    {
                        //Convert ManifestNumber into Reshimon number
                        var reshimonNumber = LuhnAlgorithm.ConvertDeclartionToReshimon(decConsignment.ManifestNumber);
                        if (!string.IsNullOrWhiteSpace(reshimonNumber))
                        {
                            cCUMSHGRPM.IDENTIFIERNO = reshimonNumber;
                        }
                    }
                    else
                    {
                        //cCUMSHGRPM.IDENTIFIERNO = decConsignment.ManifestNumber.GetLast(9);
                        var manifestNumber = decConsignment.ManifestNumber.Remove(decConsignment.ManifestNumber.Length - 1, 1);
                        cCUMSHGRPM.IDENTIFIERNO = manifestNumber.GetLast(9);
                    }
                }

            }

            if (!string.IsNullOrWhiteSpace(decConsignment.ManifestNumber))
            {
                if (_DirtyDeclarationPM.TransportModeId == "A" && decConsignment.CargoTypeCode == "17")
                {
                    //int length = decConsignment.ManifestNumber.Length - 1;
                    //cCUMSHGRPM.CARNETNUMBER = decConsignment.ManifestNumber.Substring(0, Math.Min(9, length));
                    cCUMSHGRPM.IDENTIFIERNO = decConsignment.ManifestNumber.GetLast(9);
                }
                else if (decConsignment.CargoTypeCode == "20")
                {
                    cCUMSHGRPM.CARNETNUMBER = decConsignment.ManifestNumber.GetLast(9);
                }
            }

            if (decConsignment.CargoTypeCode == "8")
            {
                if (!string.IsNullOrWhiteSpace(decConsignment.ManifestNumber))
                {
                    //If ManifestNumber is a converted Reshimon 
                    if (decConsignment.ManifestNumber.Length >= 4 && decConsignment.ManifestNumber.Substring(2, 2) == "99")
                    {
                        //Convert ManifestNumber into Reshimon number
                        var reshimonNumber = LuhnAlgorithm.ConvertDeclartionToReshimon(decConsignment.ManifestNumber);
                        if (!string.IsNullOrWhiteSpace(reshimonNumber))
                        {
                            cCUMSHGRPM.IDENTIFIERNO = reshimonNumber;
                        }
                    }
                    else
                    {
                        //cCUMSHGRPM.IDENTIFIERNO = decConsignment.ManifestNumber.GetLast(9);
                        var manifestNumber = decConsignment.ManifestNumber.Remove(decConsignment.ManifestNumber.Length - 1, 1);
                        cCUMSHGRPM.IDENTIFIERNO = manifestNumber.GetLast(9);
                    }
                }

            }

            if (!string.IsNullOrWhiteSpace(decConsignment.ManifestNumber))
            {
                if (_DirtyDeclarationPM.TransportModeId == "A" && decConsignment.CargoTypeCode == "17")
                {
                    //int length = decConsignment.ManifestNumber.Length - 1;
                    //cCUMSHGRPM.CARNETNUMBER = decConsignment.ManifestNumber.Substring(0, Math.Min(9, length));
                    cCUMSHGRPM.IDENTIFIERNO = decConsignment.ManifestNumber.GetLast(9);
                }
                else if (decConsignment.CargoTypeCode == "20")
                {
                    cCUMSHGRPM.CARNETNUMBER = decConsignment.ManifestNumber.GetLast(9);
                }
            }

            if (_DirtyDeclarationPM.TransportModeId == "A")
            {
                cCUMSHGRPM.TRANSPTYPE = "04";
            }
            if (_DirtyDeclarationPM.TransportModeId == "O")
            {
                cCUMSHGRPM.TRANSPTYPE = "01";
            }

            cCUMSHGRPM.WAREHOUSEID = GetTranslationP2L("IIGC", "CTBBONDED", decConsignment.StorageSiteCode);
            cCUMSHGRPM.WAREHOUSEIDN = decConsignment.StorageSiteCode;

            cCUMSHGRPM.WAREHOUSEREC = GetTranslationP2L("IIGC", "CTBBONDED", decConsignment.ReceiverWarehouseCode);
            cCUMSHGRPM.WAREHOUSERECN = decConsignment.ReceiverWarehouseCode;

            cCUMSHGRPM.EXPORTLAND = GetTranslationP2L("IIGC", "CTBCOUNTRY", decConsignment.OriginCountryCode);
            cCUMSHGRPM.EXPORTLANDN = decConsignment.OriginCountryCode;

            cCUMSHGRPM.LOADPORTID = decConsignment.LoadingPortCode;
            if (!string.IsNullOrWhiteSpace(decConsignment.CargoDescription))
            {
                cCUMSHGRPM.DESCOFGOODS1 = decConsignment.CargoDescription.Substring(0, Math.Min(30, decConsignment.CargoDescription.Length)); //First 30 chars
                if (decConsignment.CargoDescription.Length > 30)
                {
                    cCUMSHGRPM.DESCOFGOODS2 = decConsignment.CargoDescription.Substring(30, Math.Min(30, decConsignment.CargoDescription.Length - 30)); //Second 30 chars
                }
                if (decConsignment.CargoDescription.Length > 60)
                {
                    cCUMSHGRPM.DESCOFGOODS3 = decConsignment.CargoDescription.Substring(60, Math.Min(30, decConsignment.CargoDescription.Length - 60)); //Third 30 chars
                }
            }
            if (!String.IsNullOrWhiteSpace(decConsignment.UnloadPortCode))
            {
                cCUMSHGRPM.UNLOADPORTID = decConsignment.UnloadPortCode.Substring(2);
            }

            switch (decConsignment.IsLastReleaseFromWarehous)
            {
                case "F":
                    cCUMSHGRPM.PARTIALITYID = "1";
                    break;
                case "T":
                    cCUMSHGRPM.PARTIALITYID = "3";// "2";task 44331
                    break;
                case "N":
                    cCUMSHGRPM.PARTIALITYID = "2";// 3";task 44331            
                    break;
                default:
                    cCUMSHGRPM.PARTIALITYID = "";
                    break;
            }

            //if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPM.DeclarationOfficeCode))
            //{
            //    this._Context = CustomContext.GetContext(_DirtyDeclarationPM.Tenant);
            //    var myCustomsBranchesQueryService = new CustomsBranchQueryService(_Context);
            //    CustomsBranchPM myCustomsBranchPM = myCustomsBranchesQueryService.GetSingle(_DirtyDeclarationPM.DeclarationOfficeCode,true, false);
            //    if (!String.IsNullOrWhiteSpace(myCustomsBranchPM.UnloadPortCode))
            //    {
            //        cCUMSHGRPM.UNLOADPORTID = myCustomsBranchPM.UnloadPortCode;
            //    }
            //}

            //if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPM.DeclarationOfficeCode))
            //{
            //    this._Context = CustomContext.GetContext(_DirtyDeclarationPM.Tenant);
            //    var myTanentCustomsBranchesQueryService = new TanentCustomsBranchesQueryService(_Context);
            //    TanentCustomsBranchPM myTanentCustomsBranchPM = myTanentCustomsBranchesQueryService.GetSingle(_DirtyDeclarationPM.DeclarationOfficeCode, true, false);
            //    if (!String.IsNullOrWhiteSpace(myTanentCustomsBranchPM.UnloadPortCode))
            //    {
            //        cCUMSHGRPM.UNLOADPORTID = myTanentCustomsBranchPM.UnloadPortCode;
            //    }
            //}

            //if (!String.IsNullOrWhiteSpace(_DirtyDeclarationPM.DeclarationOfficeCode))
            //{
            //    this._Context = CustomContext.GetContext(_DirtyDeclarationPM.Tenant);
            //    var myCustomsHouseTypeQueryService = new CustomsHouseTypeQueryService(_Context);
            //    TanentCustomsBranchPM myTanentCustomsBranchPM = myTanentCustomsBranchesQueryService.GetSingle(_DirtyDeclarationPM.DeclarationOfficeCode, true, false);
            //    if (!String.IsNullOrWhiteSpace(myTanentCustomsBranchPM.UnloadPortCode))
            //    {
            //        cCUMSHGRPM.UNLOADPORTID = myTanentCustomsBranchPM.UnloadPortCode;
            //    }
            //}



            if (decConsignment.UnloadDate.HasValue)
            {
                cCUMSHGRPM.UNLOADDATE = decConsignment.UnloadDate.Value.Date;
            }
            if (cCUMSHGRPM.QUANTITY == null)
            {
                cCUMSHGRPM.QUANTITY = 0;
            }
            if (cCUMSHGRPM.WEIGHT == null)
            {
                cCUMSHGRPM.WEIGHT = 0;
            }
            if (decConsignment.ConsignmentPackages != null)
            {
                foreach (var decConsignmentPackage in decConsignment.ConsignmentPackages)
                {
                    if (decConsignmentPackage.PackageMeasureQualifierCode == "2") // Mirit 19/11/15 Task 17030
                    {
                        cCUMSHGRPM.QUANTITY = cCUMSHGRPM.QUANTITY.GetValueOrDefault() + decConsignmentPackage.PackageQuantity.GetValueOrDefault();

                        if (decConsignmentPackage.GrossMassMeasure.HasValue)
                        {
                            int myint = 0;
                            var stringDecimal = decConsignmentPackage.GrossMassMeasure.Value.ToString("0");
                            if (int.TryParse(stringDecimal, out myint))
                            {
                                //marked code moved to Uniface...
                                //if(decConsignmentPackage.GrossMassMeasureTypeCode == "TNE")
                                //{
                                    //myint = myint * 1000;
                                //}
                                cCUMSHGRPM.WEIGHT = cCUMSHGRPM.WEIGHT.GetValueOrDefault() + myint;
                            }
                        }

                        //Take from first line
                        if (String.IsNullOrWhiteSpace(cCUMSHGRPM.PACKTYPEID))
                        {
                            cCUMSHGRPM.PACKTYPEID = GetTranslationP2L("IIGC", "CTBPACKTYPE", decConsignmentPackage.PackageTypeCode);
                            if (String.IsNullOrWhiteSpace(cCUMSHGRPM.PACKTYPEID))
                            {
                                cCUMSHGRPM.PACKTYPEID = "05";
                            }
                        }
                        if (String.IsNullOrWhiteSpace(cCUMSHGRPM.PACKTYPEIDN))
                        {
                            cCUMSHGRPM.PACKTYPEIDN = decConsignmentPackage.PackageTypeCode;
                        }
                    }
                }
                cCUMSHGRPM.CCUSIGNUMPMs = SetCCUSIGNUMPM(cCUMSHGRPM, decConsignment);
            }

            return cCUMSHGRPM;
        }

        private List<CCUSIGNUMPM> SetCCUSIGNUMPM(CCUMSHGRPM myCCUMSHGRPM, ConsignmentPM decConsignment)
        {
            CCUSIGNUMPM myCCUSIGNUMPM = new CCUSIGNUMPM();
            List<CCUSIGNUMPM> myCCUSIGNUMPMList = new List<CCUSIGNUMPM>();

            if (decConsignment.ConsignmentPackages != null && decConsignment.ConsignmentPackages.Count() > 0)
            {
                foreach (var decConsignmentPackage in decConsignment.ConsignmentPackages)
                {
                    if (decConsignmentPackage.LineNumber < 1000 && !string.IsNullOrWhiteSpace(decConsignmentPackage.MarksNumbers))
                    {
                        switch (decConsignmentPackage.ChangeSetOp)
                        {
                            case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                                break;
                            default:
                                int maxlength = 30;
                                for (int i = 0; i < decConsignmentPackage.MarksNumbers.Length; i += maxlength)
                                {
                                    if (i + maxlength > decConsignmentPackage.MarksNumbers.Length)
                                    {
                                        maxlength = decConsignmentPackage.MarksNumbers.Length - i;
                                    }

                                    myCCUSIGNUMPM = new CCUSIGNUMPM();
                                    myCCUSIGNUMPM.ChangeSetOp = ChangeSetOperation.Insert;
                                    myCCUSIGNUMPM.SIGNNUM = decConsignmentPackage.MarksNumbers.Substring(i, maxlength);

                                    myCCUSIGNUMPMList.Add(myCCUSIGNUMPM);
                                }
                                break;
                        }
                    }
                }
            }
            return myCCUSIGNUMPMList;
        }

        private int GetCounter(DeclarationPM dirtyDeclarationPM)
        {
            int i = Convert.ToInt32(dirtyDeclarationPM.Id.Replace("-", ""));
            return 50000000 + i;
        }

        private void DoSupplierInvoices()
        {
            EntityQueryServices.SupplierInvoiceQueryService mySupplierInvoiceQueryService = new EntityQueryServices.SupplierInvoiceQueryService(_Context);
            List<Def.EntityPMs.SupplierInvoicePM> mySupplierInvoicePMList = new List<Def.EntityPMs.SupplierInvoicePM>();

            if (!_IsSupplerInvChanged && !_UpdateCCUFILEMFromSupplerInvoice)
            {
                return;
            }
            if (_DirtyDeclarationPM.SupplierInvoices == null)
            {
                return;
            }
            if (_DirtyDeclarationPM.SupplierInvoices.Count < 1)
            {
                return;
            }

            LoadCurrenciesExchangeRates(); //Yuval Chalup 31.12.2014 AMI-52371

            //<--- This is to be done in a full saving mode ONLY (Moved from befor the call to DoSupplierInvoices())
            _CCUFILEMPM.FEEPLATFORM = 0;
            _CCUFILEMPM.EXPENSEVALUE = 0;
            _CCUFILEMPMSupplierInvoiceModificationsI01 = 0;
            _CCUFILEMPMSupplierInvoiceModifications527 = 0;
            _CCUFILEMPM.CHANGINGVALUE = 0;
            _CCUFILEMPM.SERVICEVALUE = 0;
            //This is to be done in a full saving mode ONLY  --->

            _CCUFILEMPM.TRANSPVALUE = 0;
            _CCUFILEMPM.INDEXVALUE = 0;

            _CCUFILEMPM.INSURANCEVALUE = 0;
            _CCUFILEMPM.INSURANCEAMNT = 0;
            _CCUFILEMPM.INSURANCEPERCENT = 0;
            _CCUFILEMPM.INSURANCECURR = "";
            _CCUFILEMPM.INSURANCECURRN = "";


    _TotalCCUTRANSPVALs = new List<CCUTRANSPVALPM>();

            //In case need to save fields from SupplierInvoices to CCUFILEM without saving SupplierInvoices
            if (_UpdateCCUFILEMFromSupplerInvoice)
            {
                foreach (var decSupplierInvoice in _DirtyDeclarationPM.SupplierInvoices)
                {
                    switch (decSupplierInvoice.ChangeSetOp)
                    {
                        case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                            break;
                        default: // Insert & Update
                            _CCUFILEMPM.SupplierInvoices.Add(SetSupplierInvoice(decSupplierInvoice));
                            break;
                    }
                }

                _CCUFILEMPM.SupplierInvoices.Clear();
            }
            else
            {
                foreach (var decSupplierInvoice in _DirtyDeclarationPM.SupplierInvoices)
                {
                    switch (decSupplierInvoice.ChangeSetOp)
                    {
                        case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                            break;
                        default: // Insert & Update
                            _CCUFILEMPM.SupplierInvoices.Add(SetSupplierInvoice(decSupplierInvoice));
                            break;
                    }
                }
            }


            _CCUFILEMPM.NOOFINVOICES = _DirtyDeclarationPM.SupplierInvoices.Count();
            _CCUFILEMPM.TOTALINVOICELINESNO = GetCountSupplierInvoicesItems();
            _CCUFILEMPM.PRATMEHESLIST = GetAllPratMehesList(3);
            _CCUFILEMPM.ALLPRATMEHESLIST = GetAllPratMehesList();
            if(_CCUFILEMPM.ALLPRATMEHESLIST.Length > 1024) _CCUFILEMPM.ALLPRATMEHESLIST = _CCUFILEMPM.ALLPRATMEHESLIST.Substring(0, 1024);


            CreateCCUTRANSPVAL();

            //<--- This is to be done in a full saving mode ONLY (Moved from befor the call to DoSupplierInvoices())
            //if Supplier Invoice Modifications of type I02 is empty - Take Type I01
            if (_CCUFILEMPM.FEEPLATFORM == 0)
            {
                _CCUFILEMPM.FEEPLATFORM = _CCUFILEMPMSupplierInvoiceModificationsI01;
            }
            //This is to be done in a full saving mode ONLY  --->
        }

        private int GetCountSupplierInvoicesItems()
        {
            int countInvoiceItems=0;
            foreach (var invoice in _DirtyDeclarationPM.SupplierInvoices )
            {
                countInvoiceItems += invoice.SupplierInvoiceItems.Count();
            }

            return countInvoiceItems;
        }

        private string GetAllPratMehesList(int top = 0)
        {
            List<string> list = new List<string>();
            foreach (var invoice in _DirtyDeclarationPM.SupplierInvoices)
            {
                //list.AddRange(invoice.SupplierInvoiceItems.Where(r=>r.ClassificationCode != null).Select(x=>x.ClassificationCode.Substring(0, Math.Min(8, x.ClassificationCode.Length)) + x.ClassificationCode.Substring(Math.Min(11, x.ClassificationCode.Length - 1), 1)));

                var range = invoice.SupplierInvoiceItems
                    .Where(r => !string.IsNullOrWhiteSpace(r.ClassificationCode))
                    .Select(x =>
                    x.ClassificationCode.Substring(0, Math.Min(8, x.ClassificationCode.Length))
                    + x.ClassificationCode.Substring(Math.Min(11, x.ClassificationCode.Length - 1)
                    , 1));
                if (range.Count() > 0)
                {
                    list.AddRange(range);
                }
            }
            list = list.Where(x => x != null).OrderBy(x => x).Distinct().ToList();
            if (top != 0 && top < list.Count())
            {
                list = list.Take(top).ToList();
            }//
            return string.Join(",", list).TrimEnd(',');
        }

        private Unifreight.BL.EntityPMs.SupplierInvoicePM SetSupplierInvoice(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice)
        {
            CustomsExchangeRatePM rate = new CustomsExchangeRatePM();
            Unifreight.BL.EntityPMs.SupplierInvoicePM supplierInvoicePM = new Unifreight.BL.EntityPMs.SupplierInvoicePM();
            supplierInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;
            //<--- Yuval Chalup 13.02.2016 TASK-20599
            if (_IsSupplerInvUpdateCCUFILEM)
            {
                //    supplierInvoicePM.ChangeSetOp = ChangeSetOperation.None; // moran 14.6.16 - Task 21737 - commented
            }
            //Yuval Chalup 13.02.2016 TASK-20599 --->

            var myCustomsVendorQueryService = new CustomsVendorQueryService(_Context);

            supplierInvoicePM.MAINACCOUNT = false;

            //if (!_MainAccountSet)
            //if (decSupplierInvoice.InvoiceCounterKey.Equals(_DirtyDeclarationPM.PrimaryInvoiceCounterKey)) //Yuval Chalup 20.09.2015 TASK-16392
            //if (decSupplierInvoice.InvoiceCounterKey.ToString() == _DirtyDeclarationPM.PrimaryInvoiceCounterKey) //Yuval Chalup 20.09.2015 TASK-16392
            if (decSupplierInvoice.InvoiceCounterKey.ToString() == _DirtyDeclarationPM.PrimaryInvoiceCounterKey || _DirtyDeclarationPM.SupplierInvoices.Count() == 1) //Yuval Chalup 09.12.2015 Replace line above
            {
                supplierInvoicePM.MAINACCOUNT = true;

                //Set Customs file fields
                _CCUFILEMPM.SELLCONDITIONID = GetTranslationP2L("IIGC", "CTBINCOTERMS", decSupplierInvoice.IncotermCode);
                _CCUFILEMPM.COINID = GetTranslationP2L("IIGC", "CTBCURRENCY", decSupplierInvoice.InvoiceCurrencyTypeCode);
                _CCUFILEMPM.COINIDN = decSupplierInvoice.InvoiceCurrencyTypeCode;

                //Take the Exchange rate from the Main Account, if it does not exist calculate it according the InvoiceCurrencyTypeCode
                if (decSupplierInvoice.ExchangeRate.HasValue && decSupplierInvoice.ExchangeRate > 0)
                {
                    _CCUFILEMPM.CURRENCYRATE = decSupplierInvoice.ExchangeRate.ToNullableDouble("decSupplierInvoice.ExchangeRate");
                    _CCUFILEMPM.CURRENCYRATENEW = decSupplierInvoice.ExchangeRate.Value; // moran 12.1.16 - Task 17425
                }
                else
                {
                    rate = _CustomsExchangeRates.FirstOrDefault(obj => obj.CurrencyTypeCode == decSupplierInvoice.InvoiceCurrencyTypeCode);
                    if (rate != null)
                    {
                        if (!string.IsNullOrWhiteSpace(rate.ExchangeRate.ToString()))
                        {
                            _CCUFILEMPM.CURRENCYRATE = rate.ExchangeRate.ToNullableDouble("rate.ExchangeRate");
                            _CCUFILEMPM.CURRENCYRATENEW = rate.ExchangeRate.Value; // moran 12.1.16 - Task 17425
                        }
                    }
                }

                //Set Supplier Invoice fields
                _MainAccountSet = true;
            }

            _CCUFILEMPM.TRANSPVALUE = _CCUFILEMPM.TRANSPVALUE.GetValueOrDefault() + decSupplierInvoice.TotalFreightInNIS.ToNullableDouble("decSupplierInvoice.TotalFreightInNIS").GetValueOrDefault();

            double? amountDouble = 0;
            double amountDouble2 = 0;

            amountDouble = Transfer(decSupplierInvoice.InsuranceAmount, decSupplierInvoice.InsruanceCurrencyTypeCode, "ILS", decSupplierInvoice.ExchangeRate);
            if (amountDouble != null)
            {
                _CCUFILEMPM.INSURANCEVALUE = _CCUFILEMPM.INSURANCEVALUE.GetValueOrDefault() + amountDouble;
            }

            ////<--- Yuval Chalup 02.08.2015 // Mirit 10/07/16 Task 20996
            //if (double.TryParse(decSupplierInvoice.InsuranceAmount.ToString(), out amountDouble2))
            //{
            //    _CCUFILEMPM.INSURANCEAMNT = _CCUFILEMPM.INSURANCEAMNT.GetValueOrDefault() + amountDouble2;
            //}
            ////_CCUFILEMPM.INSURANCECURR = GetTranslationP2L("IIGC", "CTBCURRENCY", decSupplierInvoice.InsruanceCurrencyTypeCode);
            ////_CCUFILEMPM.INSURANCECURRN = decSupplierInvoice.InvoiceCurrencyTypeCode;
            //if (double.TryParse(decSupplierInvoice.InsruancePercentage.ToString(), out amountDouble2))
            //{
            //    _CCUFILEMPM.INSURANCEPERCENT = amountDouble2;
            //}
            ////Yuval Chalup 02.08.2015 --->

            if (double.TryParse(decSupplierInvoice.InsruancePercentage.ToString(), out amountDouble2))
            {
                _CCUFILEMPM.INSURANCEPERCENT = amountDouble2;
            }

            //<--- Yuval Chalup 31.12.2014 AMI-52371
            //_CCUFILEMPM.INDEXVALUE = _CCUFILEMPM.INDEXVALUE.GetValueOrDefault() + decSupplierInvoice.InvoiceAmount;
            if (decSupplierInvoice.InvoiceCurrencyTypeCode != _CCUFILEMPM.COINIDN)
            {
                Decimal? firstInvoiceExchangeRate = (_CCUFILEMPM.CURRENCYRATENEW.HasValue && _CCUFILEMPM.CURRENCYRATENEW > 0) ? (Decimal?)_CCUFILEMPM.CURRENCYRATENEW : 1;
                Decimal? invoiceExchangeRate = decSupplierInvoice.ExchangeRate;

                //If invoice Exchange Rate does not exist, calculate the Exchange Rate
                if (!invoiceExchangeRate.HasValue)
                {
                    rate = _CustomsExchangeRates.FirstOrDefault(obj => obj.CurrencyTypeCode == decSupplierInvoice.InvoiceCurrencyTypeCode);
                    if (rate != null)
                    {
                        if (!string.IsNullOrWhiteSpace(rate.ExchangeRate.ToString()))
                        {
                            invoiceExchangeRate = rate.ExchangeRate;
                        }
                    }
                }

                _CCUFILEMPM.INDEXVALUE = _CCUFILEMPM.INDEXVALUE.GetValueOrDefault() + ((decSupplierInvoice.InvoiceAmount * invoiceExchangeRate) / firstInvoiceExchangeRate);
            }
            else
            {
                _CCUFILEMPM.INDEXVALUE = _CCUFILEMPM.INDEXVALUE.GetValueOrDefault() + decSupplierInvoice.InvoiceAmount;
            }
            supplierInvoicePM.VALUE = decSupplierInvoice.InvoiceAmount.ToNullableDouble("decSupplierInvoice.InvoiceAmount");
            //Yuval Chalup 31.12.2014 AMI-52371 --->

            //supplierInvoicePM.DECLARATIONNO = _DirtyDeclarationPM.Id;

            supplierInvoicePM.ACCOUNTTYPE = GetTranslationP2L("IIGC", "CTBACCTYPE", decSupplierInvoice.AccountTypeCode);

            if (!String.IsNullOrWhiteSpace(decSupplierInvoice.InvoiceNumber))
            {
                var invoiceNumber = Regex.Replace(decSupplierInvoice.InvoiceNumber, @"[a-z]|[A-Z]|\s", "");
                //supplierInvoicePM.SUPPLIERACCOUNT = decSupplierInvoice.InvoiceNumber.Substring(0, Math.Min(9, invoiceNumber.Length)); //First 9 chars
                supplierInvoicePM.SUPPLIERACCOUNT = decSupplierInvoice.InvoiceNumber.GetLast(9);
                supplierInvoicePM.SUPPLIERACCOUNT = Regex.Replace(supplierInvoicePM.SUPPLIERACCOUNT, "[^0-9]", "");
            }
            supplierInvoicePM.SUPPLIERACCOUNTN = decSupplierInvoice.InvoiceNumber;
            if (!String.IsNullOrWhiteSpace(decSupplierInvoice.VendorId))
            {
                CustomsVendorPM myCustomsVendorPM = myCustomsVendorQueryService.GetSingle(decSupplierInvoice.VendorId, false, true);
                supplierInvoicePM.SUPPLIERID = myCustomsVendorPM.VendorNumber;
            }
            supplierInvoicePM.COUNTRYID = GetTranslationP2L("IIGC", "CTBCOUNTRY", decSupplierInvoice.IssueCountryCode);
            supplierInvoicePM.INCOTERMID = GetTranslationP2L("IIGC", "CTBINCOTERMS", decSupplierInvoice.IncotermCode);
            supplierInvoicePM.CURRENCYID = GetTranslationP2L("IIGC", "CTBCURRENCY", decSupplierInvoice.InvoiceCurrencyTypeCode);
             CalculateSupplierInvoiceModifications(_CCUFILEMPM, supplierInvoicePM, decSupplierInvoice);

            supplierInvoicePM.CHANGINGVALUE = supplierInvoicePM.CHANGINGVALUE.GetValueOrDefault() + supplierInvoicePM.VALUE;
            _CCUFILEMPM.CHANGINGVALUE = _CCUFILEMPM.INDEXVALUE.ToNullableDouble("_CCUFILEMPM.INDEXVALUE");  // += supplierInvoicePM.CHANGINGVALUE; // moran 23.11.16 - Bug 21746 - change handle to get the same value as index

            if (!_UpdateCCUFILEMFromSupplerInvoice)
            {
                supplierInvoicePM.SupplierInvoiceItems103s = DoSupplierInvoicesItems(decSupplierInvoice);
            }

            // (_IsSupplerInvChanged)  //Yuval Chalup 13.02.2016 TASK-20599 (Add IF only)
            if (_IsSupplerInvChanged || _UpdateCCUFILEMFromSupplerInvoice)
            {
                //_CCUFILEMPM.CCUTRANSPVALs = DoCCUTRANSPVAL(decSupplierInvoice); //Yuval Chalup 14.06.2015 TASK-13951
                List<CCUTRANSPVALPM> siCCUTRANSPVALs = DoCCUTRANSPVAL(decSupplierInvoice);
                if (siCCUTRANSPVALs != null && siCCUTRANSPVALs.Count > 0)
                {
                    _TotalCCUTRANSPVALs.AddRange(siCCUTRANSPVALs);
                }
            }
            if (supplierInvoicePM.MAINACCOUNT == true) // moran 22.5.16 AMI-56276
            {
                if (supplierInvoicePM.SUPPLIERID != _PreviousMainSupplier)
                {
                    _IsMainSupplerChanged = true;
                }
            }

            return supplierInvoicePM;
        }

        private void CalculateSupplierInvoiceModifications(CCUFILEMPM _CCUFILEMPM, Unifreight.BL.EntityPMs.SupplierInvoicePM supplierInvoicePM, Def.EntityPMs.SupplierInvoicePM decSupplierInvoice)
        {
            double? amountDouble = 0;
            if (decSupplierInvoice.SupplierInvoiceModifications == null)
            {
                return;
            }
            if (decSupplierInvoice.SupplierInvoiceModifications.Count < 1)
            {
                return;
            }
            if (_CCUFILEMPM.REGIONVALUE == null)
            {
                _CCUFILEMPM.REGIONVALUE = 0;
            }
            if (_CCUFILEMPM.FEEPLATFORM == null)
            {
                _CCUFILEMPM.FEEPLATFORM = 0;
            }
            if (_CCUFILEMPM.EXPENSEVALUE == null)
            {
                _CCUFILEMPM.EXPENSEVALUE = 0;
            }
            if (_CCUFILEMPM.SERVICEVALUE == null)
            {
                _CCUFILEMPM.SERVICEVALUE = 0;
            }

            string isCancelUpdateExpenses = GetDefault("ISRAEL", "CGO_CUST_EXPENS", "NON", "NON");

            foreach (var decSupplierInvoiceModifications in decSupplierInvoice.SupplierInvoiceModifications)
            {
                switch (decSupplierInvoiceModifications.TypeCode)
                {
                    case "09":
                    case "9":
                        amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, "ILS", null);
                        _CCUFILEMPM.REGIONVALUE = _CCUFILEMPM.REGIONVALUE.GetValueOrDefault() + amountDouble;
                        break;
                    case "I01":
                        amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, "ILS", null);
                        _CCUFILEMPMSupplierInvoiceModificationsI01 = _CCUFILEMPMSupplierInvoiceModificationsI01 + amountDouble;
                        break;
                    case "I02":
                        amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, "ILS", null);
                        _CCUFILEMPM.FEEPLATFORM = _CCUFILEMPM.FEEPLATFORM.GetValueOrDefault() + amountDouble;
                        break;
                    case "160":
                        if(isCancelUpdateExpenses != "Y")
                        {
                            amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, "ILS", null);
                            _CCUFILEMPM.EXPENSEVALUE = _CCUFILEMPM.EXPENSEVALUE.GetValueOrDefault() + amountDouble;
                        }
                        amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, decSupplierInvoice.InvoiceCurrencyTypeCode, null);
                        supplierInvoicePM.CHANGINGVALUE = supplierInvoicePM.CHANGINGVALUE.GetValueOrDefault() + amountDouble;
                        break;
                    //case "527": // Task 20995
                    case "I07":
                    case "I08":
                        amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, "ILS", null);
                        _CCUFILEMPMSupplierInvoiceModifications527 = _CCUFILEMPMSupplierInvoiceModifications527 + amountDouble;
                        amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, decSupplierInvoice.InvoiceCurrencyTypeCode, null);
                        supplierInvoicePM.CHANGINGVALUE = supplierInvoicePM.CHANGINGVALUE.GetValueOrDefault() - amountDouble;
                        break;
                    case "16":
                        //amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, _CCUFILEMPM.COINIDN, null);
                        //supplierInvoicePM.COMMISSION = supplierInvoicePM.COMMISSION.GetValueOrDefault() + amountDouble;
                        break;
                    case "265":
                        //amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, "ILS", null);
                        //_CCUFILEMPM.SERVICEVALUE = _CCUFILEMPM.SERVICEVALUE.GetValueOrDefault() + amountDouble;
                        break;
                    //<---Yuval Chalup 16.02.2016 TASK-19998 (Also remark case "265" and "16" above)
                    case "I10":
                        amountDouble = Transfer(decSupplierInvoiceModifications.Amount, decSupplierInvoiceModifications.CurrencyTypeCode, "ILS", null);
                        _CCUFILEMPM.SERVICEVALUE = _CCUFILEMPM.SERVICEVALUE.GetValueOrDefault() + amountDouble;
                        supplierInvoicePM.COMMISSION = supplierInvoicePM.COMMISSION.GetValueOrDefault() + amountDouble;
                        break;
                    //Yuval Chalup 16.02.2016 TASK-19998 --->
                    default:
                        break;
                }
            }
        }

        private double? Transfer(decimal? amountFrom, string currenceyFrom, string currenceyTo, decimal? exchangeRate)
        {
            Decimal? ExchangeRateFrom = 1;
            Decimal? ExchangeRateTo = 1;
            Decimal? amountTo = 1;
            CustomsExchangeRatePM rate = new CustomsExchangeRatePM();

            if (currenceyFrom == currenceyTo) return (amountFrom.ToNullableDouble("amountFrom")); ;

            // moran 17.3.16 -->
            if (_CustomsExchangeRates == null || _CustomsExchangeRates.FirstOrDefault(obj => obj.CurrencyTypeCode == currenceyFrom) == null || _CustomsExchangeRates.FirstOrDefault(obj => obj.CurrencyTypeCode == currenceyTo) == null)
            {
                List<string> codesList = new List<string>();
                if (_CustomsExchangeRates == null)
                {
                    if (!string.IsNullOrWhiteSpace(currenceyFrom)) codesList.Add(currenceyFrom);
                    if (!string.IsNullOrWhiteSpace(currenceyTo)) codesList.Add(currenceyTo);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(currenceyFrom))
                    {
                        if (_CustomsExchangeRates.FirstOrDefault(obj => obj.CurrencyTypeCode == currenceyFrom) == null) codesList.Add(currenceyFrom);
                    }
                    if (!string.IsNullOrWhiteSpace(currenceyTo))
                    {
                        if (_CustomsExchangeRates.FirstOrDefault(obj => obj.CurrencyTypeCode == currenceyTo) == null) codesList.Add(currenceyTo);
                    }
                }

                if (codesList != null) LoadCurrenciesExchangeRates(codesList);

            } // moran 17.3.16 <--

            //Get the Exchange Rate FROM
            if (exchangeRate.HasValue)
            {
                ExchangeRateFrom = exchangeRate;
            }
            else
            {
                rate = _CustomsExchangeRates.FirstOrDefault(obj => obj.CurrencyTypeCode == currenceyFrom);
                if (rate != null)
                {
                    if (!string.IsNullOrWhiteSpace(rate.ExchangeRate.ToString()))
                    {
                        ExchangeRateFrom = rate.ExchangeRate;
                    }
                }
            }
            //Get the Exchange Rate TO
            rate = _CustomsExchangeRates.FirstOrDefault(obj => obj.CurrencyTypeCode == currenceyTo);
            if (rate != null)
            {
                if (!string.IsNullOrWhiteSpace(rate.ExchangeRate.ToString()))
                {
                    ExchangeRateTo = rate.ExchangeRate;
                }
            }
            //Calculate amount OUT
            amountTo = (amountFrom * ExchangeRateFrom) / ExchangeRateTo;

            return (amountTo.ToNullableDouble("amountTo"));
        }

        private List<SupplierInvoiceItem103PM> DoSupplierInvoicesItems(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice)
        {
            if (decSupplierInvoice.SupplierInvoiceItems == null)
            {
                return null;
            }
            if (decSupplierInvoice.SupplierInvoiceItems.Count < 1)
            {
                return null;
            }

            List<SupplierInvoiceItem103PM> supplierInvoiceItem103PMList = new List<SupplierInvoiceItem103PM>();

            //The logic in accumulative SI is that it holds ONLY the Parent (105) SII.
            //We want to save also the childs (103) so we call AddSupplierInvoiceAccumalated103 that adds 103 to the SI.
            if (decSupplierInvoice.IsAccumalated)
            ////if(false)
            {
                //_AccumulatedSupplierInvoice_105LastLineNo = 0;
                decSupplierInvoice = AddSupplierInvoiceAccumalated103(decSupplierInvoice);
            }

            foreach (var decSupplierInvoiceItem in decSupplierInvoice.SupplierInvoiceItems)
            {
                switch (decSupplierInvoiceItem.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:

                        //The structure of 103 and 105 is the SI holds a list of 103 and each 103 holds a list of 105.
                        //In a NON Accumulative SI we want to for every 103 also a 105.
                        //In an Accumulative SI we want to save only 1 105 for all his 103 children.
                        if (decSupplierInvoice.IsAccumalated)
                        ////if (false)
                        {
                            //If it is a CHILD (103) invoice item line ONLY
                            if (!decSupplierInvoiceItem.IsParent)
                            {
                                //Get a CHILD (103) with the same PARENT (105) according to ParentLineNumber of this CHILD
                                //SupplierInvoiceItem103PM supplierInvoiceItem103PM = supplierInvoiceItem103PMList.FirstOrDefault(rec => rec.ITEMLINENO == decSupplierInvoiceItem.ParentLineNumber);
                                SupplierInvoiceItem103PM supplierInvoiceItem103PM = supplierInvoiceItem103PMList.FirstOrDefault(rec => rec.ParentLineNumberInAccumulatedSI == decSupplierInvoiceItem.ParentLineNumber);
                                //If it exists send it's PARENT otherwise send NULL
                                if (supplierInvoiceItem103PM != null)
                                {
                                    supplierInvoiceItem103PMList.Add(getSupplierInvoicesItem103(decSupplierInvoice, decSupplierInvoiceItem, supplierInvoiceItem103PM.SupplierInvoiceItems105.FirstOrDefault(), null, supplierInvoiceItem103PM.ParentLineNumberInAccumulatedSI));
                                }
                                else
                                {
                                    //Get the PARENT from items to create the 105 from it
                                    SupplierInvoiceItemPM supplierInvoiceItemParentPM = null;
                                    supplierInvoiceItemParentPM = decSupplierInvoice.SupplierInvoiceItems.FirstOrDefault(rec => rec.IsParent == true && rec.LineNumber == decSupplierInvoiceItem.ParentLineNumber);

                                    supplierInvoiceItem103PMList.Add(getSupplierInvoicesItem103(decSupplierInvoice, decSupplierInvoiceItem, null, supplierInvoiceItemParentPM, supplierInvoiceItemParentPM.LineNumber));
                                }
                            }
                        }
                        else
                        {
                            supplierInvoiceItem103PMList.Add(getSupplierInvoicesItem103(decSupplierInvoice, decSupplierInvoiceItem, null, null, null));
                        }
                        break;
                }
            }
            return supplierInvoiceItem103PMList;
        }

        private Def.EntityPMs.SupplierInvoicePM AddSupplierInvoiceAccumalated103(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice)
        {
            //Check if there are Children - If there are return
            SupplierInvoiceItemPM supplierInvoiceItemPM = decSupplierInvoice.SupplierInvoiceItems.FirstOrDefault(rec => rec.IsParent == false);
            if (supplierInvoiceItemPM != null)
            {
                return decSupplierInvoice;
            }

            //For every Parent - Add it's Children
            List<SupplierInvoiceItemPM> supplierInvoiceItemsPMChildList = new List<SupplierInvoiceItemPM>();
            foreach (var decSupplierInvoiceItem in decSupplierInvoice.SupplierInvoiceItems)
            {
                var qs = new SupplierInvoiceItemQueryService(this._DirtyDeclarationPM.Tenant);
                List<SupplierInvoiceItemPM> supplierInvoiceItemsPMChildTempList = qs.GetSupplierInvoiceItemsByParentFullPM(decSupplierInvoiceItem.DeclarationId,
decSupplierInvoiceItem.CounterKey, decSupplierInvoiceItem.LineNumber, this._DirtyDeclarationPM.Tenant);

                supplierInvoiceItemsPMChildList.InsertRange(0, supplierInvoiceItemsPMChildTempList);
            }
            decSupplierInvoice.SupplierInvoiceItems.InsertRange(0, supplierInvoiceItemsPMChildList);

            return decSupplierInvoice;
        }

        private SupplierInvoiceItem103PM getSupplierInvoicesItem103(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItemPM decSupplierInvoiceItem, SupplierInvoiceItem105PM supplierInvoiceItem105PM, SupplierInvoiceItemPM supplierInvoiceItemParentPM, int? parentLineNumber)
        {
            SupplierInvoiceItem103PM supplierInvoiceItem103PM = new SupplierInvoiceItem103PM();
            supplierInvoiceItem103PM.ChangeSetOp = ChangeSetOperation.Insert;
            if (_IsSupplerInvUpdateCCUFILEM) // moran 14.6.16 - Task 21737
            {
                supplierInvoiceItem103PM.ChangeSetOp = ChangeSetOperation.None;
            }
            if (!String.IsNullOrWhiteSpace(decSupplierInvoiceItem.ClassificationCode))
            {
                supplierInvoiceItem103PM.PRATMEHES = decSupplierInvoiceItem.ClassificationCode.Substring(0, Math.Min(8, decSupplierInvoiceItem.ClassificationCode.Length)) + decSupplierInvoiceItem.ClassificationCode.Substring(Math.Min(11, decSupplierInvoiceItem.ClassificationCode.Length - 1), 1);  //First 8 + Last char
                supplierInvoiceItem103PM.PRATMEHESN = decSupplierInvoiceItem.ClassificationCode;
            }
            /*
            if (!String.IsNullOrWhiteSpace(decSupplierInvoiceItem.TaxExemptCode))
            {
                supplierInvoiceItem103PM.PRATMEHES = decSupplierInvoiceItem.TaxExemptCode;
                supplierInvoiceItem103PM.ESSENTIALITEM = decSupplierInvoiceItem.ClassificationCode.Substring(0, Math.Min(8, decSupplierInvoiceItem.ClassificationCode.Length)) + decSupplierInvoiceItem.ClassificationCode.Substring(Math.Min(11, decSupplierInvoiceItem.ClassificationCode.Length - 1), 1);  //First 8 + Last char
            }
            else if (!String.IsNullOrWhiteSpace(decSupplierInvoiceItem.ClassificationCode))
            {
                supplierInvoiceItem103PM.PRATMEHES = decSupplierInvoiceItem.ClassificationCode.Substring(0, Math.Min(8, decSupplierInvoiceItem.ClassificationCode.Length)) + decSupplierInvoiceItem.ClassificationCode.Substring(Math.Min(11, decSupplierInvoiceItem.ClassificationCode.Length - 1), 1);  //First 8 + Last char
            }
            */
            //supplierInvoiceItem103PM.TARIFFCODE = GetTranslationP2L("IIGC", "CTBTARIFF", decSupplierInvoiceItem.TradeAgreementCode);
            //supplierInvoiceItem103PM.TARIFFCODEN = decSupplierInvoiceItem.TradeAgreementCode;
            supplierInvoiceItem103PM.TARIFFCODE = decSupplierInvoiceItem.TradeAgreementCode;
            if (!string.IsNullOrWhiteSpace(decSupplierInvoiceItem.TradeAgreementCode))
            {
                if (decSupplierInvoiceItem.TradeAgreementCode.Length > 2)
                {
                    supplierInvoiceItem103PM.TARIFFCODE = null;
                }
            }
            supplierInvoiceItem103PM.ORIGINCOUNTRY = GetTranslationP2L("IIGC", "CTBCOUNTRY", decSupplierInvoiceItem.OriginCountryCode);
            supplierInvoiceItem103PM.ORIGINCOUNTRYN = decSupplierInvoiceItem.OriginCountryCode;
            supplierInvoiceItem103PM.ITEMNO = decSupplierInvoiceItem.ItemCode;
            //supplierInvoiceItem103PM.ESSENTIALITEM = supplierInvoiceItem103PM.PRATMEHES; 
            //supplierInvoiceItem103PM.BITHATAXITEM = supplierInvoiceItem103PM.PRATMEHES;
            supplierInvoiceItem103PM.PURCHCOUNTRY = GetTranslationP2L("IIGC", "CTBCOUNTRY", decSupplierInvoice.IssueCountryCode);
            supplierInvoiceItem103PM.PURCHCOUNTRYN = decSupplierInvoice.IssueCountryCode;
            supplierInvoiceItem103PM.WHOLESALEPRICE = decSupplierInvoiceItem.WholeSaleItemPrice;
            /////supplierInvoiceItem103PM.DISCOUNTCODE = GetTranslationP2L("IIGC", "CTBDISCOUNT", decSupplierInvoiceItem.TaxExemptCode); ;Yuval Chalup 30.11.2016 TASK-24754 (Removed)
            supplierInvoiceItem103PM.QUANTITY = decSupplierInvoiceItem.InvoiceQuantity.ToNullableDouble("decSupplierInvoiceItem.InvoiceQuantity");
            supplierInvoiceItem103PM.EXTRAQNTY = decSupplierInvoiceItem.AdditionalQuantity.ToNullableDouble("decSupplierInvoiceItem.AdditionalQuantity");
            supplierInvoiceItem103PM.STSQNTY = decSupplierInvoiceItem.StatisticQuantity.ToNullableDouble("decSupplierInvoiceItem.StatisticQuantity");
            supplierInvoiceItem103PM.TSVIRA = true;
            if (decSupplierInvoice.IsAccumalated)
            {
                supplierInvoiceItem103PM.TSVIRA = false;
            }
            supplierInvoiceItem103PM.ORIGINVALUE = decSupplierInvoiceItem.ItemPrice;
            supplierInvoiceItem103PM.NIDHEMEHESPCNT = decSupplierInvoiceItem.DeferredCustomsTax.ToNullableDouble("decSupplierInvoiceItem.DeferredCustomsTax");
            supplierInvoiceItem103PM.NIDHEMASPCNT = decSupplierInvoiceItem.DeferredPurchaseTax.ToNullableDouble("decSupplierInvoiceItem.DeferredPurchaseTax");
            double? FOREIGNCURRVAL_AfterExchangeRate = 0;
            string currencyCode = decSupplierInvoiceItem.ItemPriceCurrencyCode;
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                currencyCode = decSupplierInvoice.InvoiceCurrencyTypeCode;
            }
            FOREIGNCURRVAL_AfterExchangeRate = Transfer(decSupplierInvoiceItem.ItemPrice, currencyCode, _CCUFILEMPM.COINIDN, null);
            supplierInvoiceItem103PM.FOREIGNCURRVAL = FOREIGNCURRVAL_AfterExchangeRate.ToNullableDecimal("FOREIGNCURRVAL_AfterExchangeRate");

            if (!_CreateCCUTAXFor105Feature)
            {
                //List<CCUTAXPM> moreCCUTAXPM = DoSupplierInvoiceItemTaxes(decSupplierInvoice, decSupplierInvoiceItem, supplierInvoiceItem103PM);
                //if (moreCCUTAXPM != null)
                if (_CCUFILEMPM.CCUTAXPM == null)
                {
                    _CCUFILEMPM.CCUTAXPM = DoSupplierInvoiceItemTaxes(decSupplierInvoice, decSupplierInvoiceItem, supplierInvoiceItem103PM);
                }
                else
                {
                    List<CCUTAXPM> CCUTAXPMList = DoSupplierInvoiceItemTaxes(decSupplierInvoice, decSupplierInvoiceItem, supplierInvoiceItem103PM);
                    if (CCUTAXPMList != null && CCUTAXPMList.Count > 0)
                    {
                        _CCUFILEMPM.CCUTAXPM.AddRange(CCUTAXPMList);
                    }
                }
            }
            supplierInvoiceItem103PM.CCUCRREQPM = DoSupplierInvioceItemsCertificate(decSupplierInvoice, decSupplierInvoiceItem);
            CalculateSupplierInvoiceItemsModifications(supplierInvoiceItem103PM, decSupplierInvoice, decSupplierInvoiceItem);

            //supplierInvoiceItem103PM.RAISEVALUE = 0;


            //SupplierInvoiceItem105PM supplierInvoiceItem105PM = getSupplierInvoicesItem105(decSupplierInvoice, supplierInvoiceItem103PM);
            //If the PARENT (105) of one of the CHILDS (103) does NOT exist - Create the PARENT and update it's line in the child. if it exists only update it's line in the child.
            if (supplierInvoiceItem105PM == null)
            {
                //Create the 105 from 103
                supplierInvoiceItem105PM = getSupplierInvoicesItem105(decSupplierInvoice, supplierInvoiceItem103PM, decSupplierInvoiceItem);
                //Update 105 accumulated fields from PARENT item 
                if (supplierInvoiceItemParentPM != null)
                {
                    supplierInvoiceItem105PM.QUANTITY = supplierInvoiceItemParentPM.InvoiceQuantity.ToNullableDouble("decSupplierInvoiceItem.InvoiceQuantity");
                    supplierInvoiceItem105PM.STSQNTY = supplierInvoiceItemParentPM.StatisticQuantity.ToNullableDouble("decSupplierInvoiceItem.StatisticQuantity");
                    currencyCode = supplierInvoiceItemParentPM.ItemPriceCurrencyCode;
                    if (string.IsNullOrWhiteSpace(currencyCode))
                    {
                        currencyCode = decSupplierInvoice.InvoiceCurrencyTypeCode;
                    }
                    FOREIGNCURRVAL_AfterExchangeRate = Transfer(supplierInvoiceItemParentPM.ItemPrice, currencyCode, _CCUFILEMPM.COINIDN, null);
                    supplierInvoiceItem105PM.FOREIGNCURRVAL = FOREIGNCURRVAL_AfterExchangeRate.ToNullableDecimal("FOREIGNCURRVAL_AfterExchangeRate");
                }

                supplierInvoiceItem103PM.SupplierInvoiceItems105.Add(supplierInvoiceItem105PM);
            }
            if (parentLineNumber != null)
            {
                supplierInvoiceItem103PM.ParentLineNumberInAccumulatedSI = parentLineNumber;
            }
            supplierInvoiceItem103PM.ITEMLINENO = supplierInvoiceItem105PM.LINENO;

            supplierInvoiceItem103PM.CCUSUPITEMSIPM = DoCCUSUPITEMSI(decSupplierInvoice, decSupplierInvoiceItem, supplierInvoiceItem103PM);


            return supplierInvoiceItem103PM;
        }

        private CCUSUPITEMSIPM DoCCUSUPITEMSI(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItemPM decSupplierInvoiceItem, SupplierInvoiceItem103PM supplierInvoiceItem103PM)
        {
            if (decSupplierInvoice.UnfInvoiceCounterKey != null || decSupplierInvoiceItem.UnfInvoiceLine != null)
            {
                CCUSUPITEMSIPM myCCUSUPITEMSIPM = new CCUSUPITEMSIPM();
                myCCUSUPITEMSIPM.FILENO = supplierInvoiceItem103PM.FILENO;
                myCCUSUPITEMSIPM.LINENO = supplierInvoiceItem103PM.LINENO;
                myCCUSUPITEMSIPM.ACCLINENO = supplierInvoiceItem103PM.ACCLINENO;

                myCCUSUPITEMSIPM.ChangeSetOp = ChangeSetOperation.Insert;

                int unfInvoiceCounterKey;
                if (int.TryParse(decSupplierInvoice.UnfInvoiceCounterKey, out unfInvoiceCounterKey))
                {
                    myCCUSUPITEMSIPM.SICOUNTER = unfInvoiceCounterKey;
                }
                myCCUSUPITEMSIPM.LINEID = decSupplierInvoiceItem.UnfInvoiceLine.GetValueOrDefault();

                return myCCUSUPITEMSIPM;
            }

            return null;
        }

        //<--- Yuval Chalup 14.06.2015 TASK-13951
        private List<CCUTRANSPVALPM> DoCCUTRANSPVAL(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice)
        {
            if (decSupplierInvoice.SupplierInvoiceFreightAmounts == null)
            {
                return null;
            }
            if (decSupplierInvoice.SupplierInvoiceFreightAmounts.Count < 1)
            {
                return null;
            }

            List<CCUTRANSPVALPM> cCUTRANSPVALPMList = new List<CCUTRANSPVALPM>();

            foreach (var supplierInvoiceFreightAmounts in decSupplierInvoice.SupplierInvoiceFreightAmounts)
            {
                switch (supplierInvoiceFreightAmounts.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        cCUTRANSPVALPMList.Add(getCCUTRANSPVAL(decSupplierInvoice, supplierInvoiceFreightAmounts));
                        break;
                }
            }
            return cCUTRANSPVALPMList;
        }

        private CCUTRANSPVALPM getCCUTRANSPVAL(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceFreightAmountPM supplierInvoiceFreightAmounts)
        {
            CCUTRANSPVALPM cCUTRANSPVALPM = new CCUTRANSPVALPM();
            cCUTRANSPVALPM.ChangeSetOp = ChangeSetOperation.Insert;

            cCUTRANSPVALPM.TRANSPVALFC = supplierInvoiceFreightAmounts.Amount.ToNullableDouble("supplierInvoiceFreightAmounts.Amount");

            cCUTRANSPVALPM.CURRID = GetTranslationP2L("IIGC", "CTBCURRENCY", supplierInvoiceFreightAmounts.CurrencyTypeCode);
            cCUTRANSPVALPM.CURRIDN = supplierInvoiceFreightAmounts.CurrencyTypeCode;

            double? CURRID_AfterExchangeRate = 0;
            string currencyCode = supplierInvoiceFreightAmounts.CurrencyTypeCode;
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                currencyCode = decSupplierInvoice.InvoiceCurrencyTypeCode;
            }
            CURRID_AfterExchangeRate = Transfer(supplierInvoiceFreightAmounts.Amount, currencyCode, "ILS", null);
            cCUTRANSPVALPM.TRANSPVAL = CURRID_AfterExchangeRate;

            return cCUTRANSPVALPM;
        }
        //Yuval Chalup 14.06.2015 TASK-13951 --->

        private void CreateCCUTRANSPVAL()
        {
            if (_TotalCCUTRANSPVALs != null && _TotalCCUTRANSPVALs.Count > 0)
            {
                foreach (var myCCUTRANSPVALPM in _TotalCCUTRANSPVALs)
                {
                    CCUTRANSPVALPM existingCCUTRANSPVALPM = _CCUFILEMPM.CCUTRANSPVALs.FirstOrDefault(rec => rec.CURRID == myCCUTRANSPVALPM.CURRID);
                    if (existingCCUTRANSPVALPM != null)
                    {
                        existingCCUTRANSPVALPM.TRANSPVAL = existingCCUTRANSPVALPM.TRANSPVAL.GetValueOrDefault() + myCCUTRANSPVALPM.TRANSPVAL;
                        existingCCUTRANSPVALPM.TRANSPVALFC = existingCCUTRANSPVALPM.TRANSPVALFC.GetValueOrDefault() + myCCUTRANSPVALPM.TRANSPVALFC;
                    }
                    else
                    {
                        _CCUFILEMPM.CCUTRANSPVALs.Add(myCCUTRANSPVALPM);
                    }
                }
            }
        }

        private void CalculateSupplierInvoiceItemsModifications(SupplierInvoiceItem103PM supplierInvoiceItem103PM, Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItemPM decSupplierInvoiceItem)
        {
            double? amountDouble = 0;
            if (decSupplierInvoiceItem.SupplierInvoiceItemsMods == null)
            {
                return;
            }
            if (decSupplierInvoiceItem.SupplierInvoiceItemsMods.Count < 1)
            {
                return;
            }
            if (supplierInvoiceItem103PM.RAISEVALUE == null)
            {
                supplierInvoiceItem103PM.RAISEVALUE = 0;
            }
            foreach (var decSupplierInvoiceItemsModifications in decSupplierInvoiceItem.SupplierInvoiceItemsMods)
            {
                switch (decSupplierInvoiceItemsModifications.TypeCode)
                {
                    case "09":
                    case "9":
                        amountDouble = Transfer(decSupplierInvoiceItemsModifications.Amount, decSupplierInvoiceItemsModifications.CurrencyTypeCode, "ILS", decSupplierInvoice.ExchangeRate);
                        supplierInvoiceItem103PM.RAISEVALUE = supplierInvoiceItem103PM.RAISEVALUE.GetValueOrDefault() + amountDouble;
                        break;
                    default:
                        break;
                }
            }
        }

        private List<CCUCRREQPM> DoSupplierInvioceItemsCertificate(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItemPM decSupplierInvoiceItem)
        {
            if (decSupplierInvoiceItem.SupplierInvioceItemCertificats == null)
            {
                return null;
            }
            if (decSupplierInvoiceItem.SupplierInvioceItemCertificats.Count < 1)
            {
                return null;
            }

            List<CCUCRREQPM> CCUCRREQPMList = new List<CCUCRREQPM>();

            foreach (var decSupplierInvioceItemsCertificates in decSupplierInvoiceItem.SupplierInvioceItemCertificats)
            {
                switch (decSupplierInvioceItemsCertificates.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:

                        CCUCRREQPMList.Add(SetSupplierInvioceItemsCertificates(decSupplierInvoice, decSupplierInvioceItemsCertificates));
                        break;
                }
            }

            return CCUCRREQPMList;
        }

        private CCUCRREQPM SetSupplierInvioceItemsCertificates(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvioceItemCertificatPM decSupplierInvioceItemsCertificates)
        {
            CCUCRREQPM cCUCRREQPM = new CCUCRREQPM();
            cCUCRREQPM.ChangeSetOp = ChangeSetOperation.Insert;

            cCUCRREQPM.CERTIFICATENO = decSupplierInvioceItemsCertificates.CertificateNumber;
            if (!String.IsNullOrWhiteSpace(decSupplierInvioceItemsCertificates.ReqConfirmationTypeCode))
            {
                //cCUCRREQPM.APPROVTYPE = reqConfirmationTypeCode.Substring(0, Math.Min(2, reqConfirmationTypeCode.Length)); //First 2 chars
                //cCUCRREQPM.APPROVCODE = reqConfirmationTypeCode.Substring(Math.Max(0, reqConfirmationTypeCode.Length - 2), Math.Min(2, reqConfirmationTypeCode.Length)); //Last 2 chars

                string reqConfirmationTypeCode = decSupplierInvioceItemsCertificates.ReqConfirmationTypeCode;
                while (reqConfirmationTypeCode.Length < 4)
                {
                    reqConfirmationTypeCode = "0" + reqConfirmationTypeCode;
                }
                cCUCRREQPM.APPROVTYPE = reqConfirmationTypeCode.Substring(0, 2); //First 2 chars
                cCUCRREQPM.APPROVCODE = reqConfirmationTypeCode.Substring(2, 2); //Last 2 chars
            }
            if (!string.IsNullOrWhiteSpace(decSupplierInvioceItemsCertificates.ExternalRequestTypeCode))
            {
                cCUCRREQPM.REQCERTID = decSupplierInvioceItemsCertificates.ExternalRequestTypeCode.Substring(0, Math.Min(10, decSupplierInvioceItemsCertificates.ExternalRequestTypeCode.Length));
            }
            if (!string.IsNullOrWhiteSpace(decSupplierInvioceItemsCertificates.ApprovalRequestNumber))
            {
                cCUCRREQPM.REQUESTNO = decSupplierInvioceItemsCertificates.ApprovalRequestNumber.Substring(0, Math.Min(50, decSupplierInvioceItemsCertificates.ApprovalRequestNumber.Length));
            }

            return cCUCRREQPM;
        }

        private List<CCUTAXPM> DoSupplierInvoiceItemTaxes(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItemPM decSupplierInvoiceItem, SupplierInvoiceItem103PM mySupplierInvoiceItem103PM)
        {
            if (decSupplierInvoiceItem.SupplierInvoiceItemTaxes == null)
            {
                return null;
            }
            if (decSupplierInvoiceItem.SupplierInvoiceItemTaxes.Count < 1)
            {
                return null;
            }

            List<CCUTAXPM> CCUTAXPMList = new List<CCUTAXPM>();

            foreach (var decSupplierInvoiceItemTaxes in decSupplierInvoiceItem.SupplierInvoiceItemTaxes)
            {
                switch (decSupplierInvoiceItemTaxes.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:

                        CCUTAXPMList.Add(SetSupplierInvoiceItemTaxes(decSupplierInvoiceItem, decSupplierInvoiceItemTaxes, mySupplierInvoiceItem103PM));
                        break;
                }
            }

            return CCUTAXPMList;
        }

        private CCUTAXPM SetSupplierInvoiceItemTaxes(Def.EntityPMs.SupplierInvoiceItemPM decSupplierInvoiceItem, SupplierInvoiceItemsTaxPM decSupplierInvoiceItemTaxes, SupplierInvoiceItem103PM mySupplierInvoiceItem103PM)
        {
            int? nullableInt = null;
            CCUTAXPM cCUTAXPM = new CCUTAXPM();
            cCUTAXPM.ChangeSetOp = ChangeSetOperation.Insert;
            //<--- Yuval Chalup 03.04.2016 TASK-20599 + TASK-20834
            if (!_IsSupplerInvChanged)
            {
                cCUTAXPM.ChangeSetOp = ChangeSetOperation.None;
            }
            //Yuval Chalup 03.04.2016 TASK-20599 + TASK-20834 --->
            cCUTAXPM.TAXTYPE = GetTranslationP2L("IIGC", "CTBTAXTYPE", decSupplierInvoiceItemTaxes.TaxTypeCode);
            cCUTAXPM.TAXTYPEN = decSupplierInvoiceItemTaxes.TaxTypeCode;
            cCUTAXPM.PRATMEHES = (mySupplierInvoiceItem103PM == null) ? "" : mySupplierInvoiceItem103PM.PRATMEHES;
            cCUTAXPM.PRATMEHESN = (mySupplierInvoiceItem103PM == null) ? "" : mySupplierInvoiceItem103PM.PRATMEHESN;
            //cCUTAXPM.GOODSNO = (mySupplierInvoiceItem103PM == null) ? null : mySupplierInvoiceItem103PM.ITEMLINENO;
            //cCUTAXPM.GOODSNO = mySupplierInvoiceItem103PM.ITEMLINENO;
            nullableInt = decSupplierInvoiceItem.LineNumber;
            cCUTAXPM.GOODSNO = (decSupplierInvoiceItem == null) ? null : nullableInt;
            cCUTAXPM.TAXBASIS = decSupplierInvoiceItemTaxes.TaxBaseAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxBaseAmount");
            //cCUTAXPM.TAXAMOUNT = decSupplierInvoiceItemTaxes.TaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxAmount");
            cCUTAXPM.TAXAMOUNT = decSupplierInvoiceItemTaxes.TaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxAmount") + decSupplierInvoiceItemTaxes.DeferedTaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.DeferedTaxAmount");
            cCUTAXPM.POSTPONEDTAX = decSupplierInvoiceItemTaxes.DeferedTaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.DeferedTaxAmount");
            //cCUTAXPM.TAXTOPAY = cCUTAXPM.TAXAMOUNT - cCUTAXPM.POSTPONEDTAX;
            cCUTAXPM.TAXTOPAY = decSupplierInvoiceItemTaxes.TaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxAmount");
            cCUTAXPM.TAXRATE = decSupplierInvoiceItemTaxes.TaxRate.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxRate");
            //cCUTAXPM.DEFINEDTAX = cCUTAXPM.POSTPONEDTAX; //Remarked by Yuval Chalup TASK-21875 05.07.2016
            cCUTAXPM.ADDTAXRATE = decSupplierInvoiceItemTaxes.AlternateRate.ToNullableDouble("decSupplierInvoiceItemTaxes.AlternateRate");
            if (cCUTAXPM.PRATMEHES != null) // moran 17.1.16 - Task 19798
            {
                _loanAmount += decSupplierInvoiceItemTaxes.TotalBtlCoverageNIS;
            }
            return cCUTAXPM;
        }

        private SupplierInvoiceItem105PM getSupplierInvoicesItem105(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItem103PM supplierInvoiceItem103PM, SupplierInvoiceItemPM decSupplierInvoiceItem)
        {
            SupplierInvoiceItem105PM supplierInvoiceItem105PM = new SupplierInvoiceItem105PM();
            supplierInvoiceItem105PM.ChangeSetOp = ChangeSetOperation.Insert;
            //<--- Yuval Chalup 03.04.2016 TASK-20599 + TASK-20834
            if (!_IsSupplerInvChanged)
            {
                supplierInvoiceItem105PM.ChangeSetOp = ChangeSetOperation.None;
            }
            //Yuval Chalup 03.04.2016 TASK-20599 + TASK-20834 --->

            //double? FOREIGNCURRVAL_AfterExchangeRate = 0;

            supplierInvoiceItem105PM.PRATMEHES = supplierInvoiceItem103PM.PRATMEHES;
            supplierInvoiceItem105PM.PRATMEHESN = supplierInvoiceItem103PM.PRATMEHESN;
            supplierInvoiceItem105PM.TARIFFCODE = supplierInvoiceItem103PM.TARIFFCODE;

            //supplierInvoiceItem105PM.TARIFFCODEN = supplierInvoiceItem103PM.TARIFFCODEN;
            supplierInvoiceItem105PM.ORIGINCOUNTRY = supplierInvoiceItem103PM.ORIGINCOUNTRY;
            supplierInvoiceItem105PM.ORIGINCOUNTRYN = supplierInvoiceItem103PM.ORIGINCOUNTRYN;
            supplierInvoiceItem105PM.PURCHCOUNTRY = supplierInvoiceItem103PM.PURCHCOUNTRY;
            supplierInvoiceItem105PM.PURCHCOUNTRYN = supplierInvoiceItem103PM.PURCHCOUNTRYN;
            //FOREIGNCURRVAL_AfterExchangeRate = Transfer(supplierInvoiceItem103PM.ORIGINVALUE, decSupplierInvoice.InvoiceCurrencyTypeCode, _CCUFILEMPM.COINIDN, null);
            // supplierInvoiceItem105PM.FOREIGNCURRVAL = FOREIGNCURRVAL_AfterExchangeRate.ToNullableDecimal("FOREIGNCURRVAL_AfterExchangeRate");
            supplierInvoiceItem105PM.FOREIGNCURRVAL = supplierInvoiceItem103PM.FOREIGNCURRVAL;
            supplierInvoiceItem105PM.RAISEPERCENT = supplierInvoiceItem103PM.RAISEPERCENT;
            supplierInvoiceItem105PM.RAISEVALUE = supplierInvoiceItem103PM.RAISEVALUE;
            supplierInvoiceItem105PM.QUANTITY = supplierInvoiceItem103PM.QUANTITY;
            supplierInvoiceItem105PM.EXTRAQNTY = supplierInvoiceItem103PM.EXTRAQNTY;
            supplierInvoiceItem105PM.STSQNTY = supplierInvoiceItem103PM.STSQNTY;
            supplierInvoiceItem105PM.WHOLESALEPRICE = supplierInvoiceItem103PM.WHOLESALEPRICE;
            supplierInvoiceItem105PM.IMPORTADDITION = supplierInvoiceItem103PM.IMPORTADDITION;
            supplierInvoiceItem105PM.DISCOUNTCODE = supplierInvoiceItem103PM.DISCOUNTCODE;
            supplierInvoiceItem105PM.AGNTPAYCUST = supplierInvoiceItem103PM.AGNTPAYCUST;
            supplierInvoiceItem105PM.AGNTPAYTAX = supplierInvoiceItem103PM.AGNTPAYTAX;
            supplierInvoiceItem105PM.AGNTPAYBITHA = supplierInvoiceItem103PM.AGNTPAYBITHA;
            supplierInvoiceItem105PM.NIDHEMEHESPCNT = supplierInvoiceItem103PM.NIDHEMEHESPCNT;
            supplierInvoiceItem105PM.NIDHEMASPCNT = supplierInvoiceItem103PM.NIDHEMASPCNT;
            supplierInvoiceItem105PM.VEHICLECODE = supplierInvoiceItem103PM.VEHICLECODE;
            supplierInvoiceItem105PM.ABSAMOUNT = supplierInvoiceItem103PM.ABSAMOUNT;
            supplierInvoiceItem105PM.AIRBAGSAMOUNT = supplierInvoiceItem103PM.AIRBAGSAMOUNT;
            supplierInvoiceItem105PM.ACAMOUNT = supplierInvoiceItem103PM.ACAMOUNT;
            supplierInvoiceItem105PM.GUARANTEENO = supplierInvoiceItem103PM.GUARANTEENO;
            supplierInvoiceItem105PM.GUARANPERCENT = supplierInvoiceItem103PM.GUARANPERCENT;
            supplierInvoiceItem105PM.GUARANTEETYPE = supplierInvoiceItem103PM.GUARANTEETYPE;
            supplierInvoiceItem105PM.EXEMPTIONCODE = supplierInvoiceItem103PM.EXEMPTIONCODE;
            supplierInvoiceItem105PM.GOODSDESC = supplierInvoiceItem103PM.GOODSDESC;
            supplierInvoiceItem105PM.PRATMEHESCAN = supplierInvoiceItem103PM.PRATMEHESCAN;
            supplierInvoiceItem105PM.AUTONOMYBOOK = supplierInvoiceItem103PM.AUTONOMYBOOK;
            supplierInvoiceItem105PM.PRIVATEIMPCURR = supplierInvoiceItem103PM.PRIVATEIMPCURR;
            supplierInvoiceItem105PM.TSVIRA = supplierInvoiceItem103PM.TSVIRA;
            supplierInvoiceItem105PM.KATALOGNO = supplierInvoiceItem103PM.KATALOGNO;
            supplierInvoiceItem105PM.UNITID = supplierInvoiceItem103PM.UNITID;
            supplierInvoiceItem105PM.NIDHEMEHESPCNT = supplierInvoiceItem103PM.NIDHEMEHESPCNT;
            supplierInvoiceItem105PM.NIDHEMASPCNT = supplierInvoiceItem103PM.NIDHEMASPCNT;
            //supplierInvoiceItem105PM.ESSENTIALITEM = supplierInvoiceItem103PM.ESSENTIALITEM;
            //supplierInvoiceItem105PM.BITHATAXITEM = supplierInvoiceItem103PM.BITHATAXITEM;
            if (!String.IsNullOrWhiteSpace(decSupplierInvoice.InvoiceNumber))
            {
                var invoiceNumber = Regex.Replace(decSupplierInvoice.InvoiceNumber, @"[a-z]|[A-Z]|\s", "");
                supplierInvoiceItem105PM.SUPPLIERACCOUNT = decSupplierInvoice.InvoiceNumber.Substring(0, Math.Min(9, invoiceNumber.Length)); //First 9 chars
            }

            supplierInvoiceItem105PM.CCUCARs = DoCCUCAR(decSupplierInvoice, supplierInvoiceItem105PM, decSupplierInvoiceItem); // moran 12.1.16 - Task 17425
            supplierInvoiceItem105PM.CCUCARLs = DoCCUCARL(decSupplierInvoice, supplierInvoiceItem105PM, decSupplierInvoiceItem); // moran 11.1.16 - Task 17425
            supplierInvoiceItem105PM.CCUCARSCs = DoCCUCARSC(decSupplierInvoice, supplierInvoiceItem105PM, decSupplierInvoiceItem);// moran 9.3.16 - AMI-55747

            if (decSupplierInvoice.IsAccumalated)
            {
                _AccumulatedSupplierInvoice_105LastLineNo = _AccumulatedSupplierInvoice_105LastLineNo + 1;
                supplierInvoiceItem105PM.LINENO = _AccumulatedSupplierInvoice_105LastLineNo;
            }
            else
            {
                if (_CreateCCUTAXFor105Feature)
                {
                    _AccumulatedSupplierInvoice_105LastLineNo = _AccumulatedSupplierInvoice_105LastLineNo + 1;
                    supplierInvoiceItem105PM.LINENO = _AccumulatedSupplierInvoice_105LastLineNo;
                }
            }

            if (_CreateCCUTAXFor105Feature)
            {
                if (_CCUFILEMPM.CCUTAXPM == null)
                {
                    _CCUFILEMPM.CCUTAXPM = DoSupplierInvoiceItemTaxes105(decSupplierInvoice, decSupplierInvoiceItem, supplierInvoiceItem103PM, supplierInvoiceItem105PM);
                }
                else
                {
                    List<CCUTAXPM> CCUTAXPMList = DoSupplierInvoiceItemTaxes105(decSupplierInvoice, decSupplierInvoiceItem, supplierInvoiceItem103PM, supplierInvoiceItem105PM);
                    if (CCUTAXPMList != null && CCUTAXPMList.Count > 0)
                    {
                        _CCUFILEMPM.CCUTAXPM.AddRange(CCUTAXPMList);
                    }
                }
            }
            return supplierInvoiceItem105PM;
        }

        private List<CCUTAXPM> DoSupplierInvoiceItemTaxes105(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItemPM decSupplierInvoiceItem, SupplierInvoiceItem103PM mySupplierInvoiceItem103PM, SupplierInvoiceItem105PM supplierInvoiceItem105PM)
        {
            if (!_CreateCCUTAXFor105Feature) return null;
            if (decSupplierInvoiceItem.SupplierInvoiceItemTaxes == null)
            {
                return null;
            }
            if (decSupplierInvoiceItem.SupplierInvoiceItemTaxes.Count < 1)
            {
                return null;
            }

            List<CCUTAXPM> CCUTAXPMList = new List<CCUTAXPM>();

            foreach (var decSupplierInvoiceItemTaxes in decSupplierInvoiceItem.SupplierInvoiceItemTaxes)
            {
                switch (decSupplierInvoiceItemTaxes.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                        //           case Simplog.Server.Infrastructure.ChangeSetOperation.Update:

                        CCUTAXPMList.Add(SetSupplierInvoiceItemTaxes105(decSupplierInvoiceItem, decSupplierInvoiceItemTaxes, mySupplierInvoiceItem103PM, supplierInvoiceItem105PM));
                        break;
                }
            }

            return CCUTAXPMList;
        }

        private CCUTAXPM SetSupplierInvoiceItemTaxes105(Def.EntityPMs.SupplierInvoiceItemPM decSupplierInvoiceItem, SupplierInvoiceItemsTaxPM decSupplierInvoiceItemTaxes, SupplierInvoiceItem103PM mySupplierInvoiceItem103PM, SupplierInvoiceItem105PM supplierInvoiceItem105PM)
        {
            if (!_CreateCCUTAXFor105Feature) return null;
            int? nullableInt = null;
            CCUTAXPM cCUTAXPM = new CCUTAXPM();
            cCUTAXPM.ChangeSetOp = ChangeSetOperation.Insert;
            if (!_IsSupplerInvChanged)
            {
                cCUTAXPM.ChangeSetOp = ChangeSetOperation.None;
            }
            cCUTAXPM.TAXTYPE = GetTranslationP2L("IIGC", "CTBTAXTYPE", decSupplierInvoiceItemTaxes.TaxTypeCode);
            cCUTAXPM.TAXTYPEN = decSupplierInvoiceItemTaxes.TaxTypeCode;
            cCUTAXPM.PRATMEHES = (supplierInvoiceItem105PM == null) ? "" : supplierInvoiceItem105PM.PRATMEHES;
            cCUTAXPM.PRATMEHESN = (supplierInvoiceItem105PM == null) ? "" : supplierInvoiceItem105PM.PRATMEHESN;
            nullableInt = supplierInvoiceItem105PM.LINENO;
            cCUTAXPM.GOODSNO = nullableInt;
            cCUTAXPM.TAXBASIS = decSupplierInvoiceItemTaxes.TaxBaseAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxBaseAmount");
            cCUTAXPM.TAXAMOUNT = decSupplierInvoiceItemTaxes.TaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxAmount") + decSupplierInvoiceItemTaxes.DeferedTaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.DeferedTaxAmount");
            cCUTAXPM.POSTPONEDTAX = decSupplierInvoiceItemTaxes.DeferedTaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.DeferedTaxAmount");
            cCUTAXPM.TAXTOPAY = decSupplierInvoiceItemTaxes.TaxAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxAmount");
            cCUTAXPM.TAXRATE = decSupplierInvoiceItemTaxes.TaxRate.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxRate");
            cCUTAXPM.ADDTAXRATE = decSupplierInvoiceItemTaxes.AlternateRate.ToNullableDouble("decSupplierInvoiceItemTaxes.AlternateRate");
            cCUTAXPM.TAXBASIS = decSupplierInvoiceItemTaxes.TaxBaseAmount.ToNullableDouble("decSupplierInvoiceItemTaxes.TaxBaseAmount");
            if (cCUTAXPM.PRATMEHES != null)
            {
                _loanAmount += decSupplierInvoiceItemTaxes.TotalBtlCoverageNIS;
            }
            return cCUTAXPM;
        }

        private List<CCUCARPM> DoCCUCAR(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItem105PM supplierInvoiceItem105PM, SupplierInvoiceItemPM decSupplierInvoiceItem) // moran 12.1.16 - Task 17425
        {
            if (decSupplierInvoiceItem.SupplierInvoiceItemModVehicles == null)
            {
                return null;
            }
            if (decSupplierInvoiceItem.SupplierInvoiceItemModVehicles.Count < 1)
            {
                return null;
            }

            List<CCUCARPM> CCUCARPMList = new List<CCUCARPM>();
            // moran 25.1.16 - Bug 19966 - adjustments -->
            switch (decSupplierInvoiceItem.SupplierInvoiceItemModVehicles.FirstOrDefault().ChangeSetOp)
            {
                case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                    break;
                default:

                    CCUCARPMList.Add(SetSupplierInvioceItemsCars(decSupplierInvoice, decSupplierInvoiceItem.SupplierInvoiceItemModVehicles));
                    break;
            }

            return CCUCARPMList;
        }


        private CCUCARPM SetSupplierInvioceItemsCars(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, List<SupplierInvoiceItemModVehiclePM> supplierInvoiceItemModVehicles) // moran 12.1.16 - Task 17425
        {

            CCUCARPM cCUCARPM = new CCUCARPM();
            cCUCARPM.ChangeSetOp = ChangeSetOperation.Insert;

            foreach (var decSupplierInvioceItemsCar in supplierInvoiceItemModVehicles)
            {
                int amount;
                switch (decSupplierInvioceItemsCar.AdjustmentTypeCode)
                {
                    case "10":
                        if (int.TryParse(decSupplierInvioceItemsCar.DeductAmount.Value.ToString("0"), out amount))
                        {
                            cCUCARPM.MEMIRDEDUCT = amount;
                        }
                        break;
                    case "9":
                        if (int.TryParse(decSupplierInvioceItemsCar.DeductAmount.Value.ToString("0"), out amount))
                        {
                            cCUCARPM.MADADDEDUCT = amount;
                        }
                        break;
                    default:
                        break;
                }
            }
            return cCUCARPM;
        }


        private List<CCUCARLPM> DoCCUCARL(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItem105PM supplierInvoiceItem105PM, SupplierInvoiceItemPM decSupplierInvoiceItem) // moran 11.1.16 - Task 17425
        {
            if (decSupplierInvoiceItem.SupplierInvoiceItemVehicles == null)
            {
                return null;
            }
            if (decSupplierInvoiceItem.SupplierInvoiceItemVehicles.Count < 1)
            {
                return null;
            }

            List<CCUCARLPM> CCUCARLPMList = new List<CCUCARLPM>();

            foreach (var decSupplierInvioceItemsCars in decSupplierInvoiceItem.SupplierInvoiceItemVehicles)
            {
                switch (decSupplierInvioceItemsCars.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:

                        CCUCARLPMList.Add(SetSupplierInvioceItemsCarls(decSupplierInvoice, decSupplierInvioceItemsCars));
                        break;
                }
            }

            return CCUCARLPMList;
        }

        private CCUCARLPM SetSupplierInvioceItemsCarls(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItemVehiclePM decSupplierInvioceItemsCars) // moran 11.1.16 - Task 17425
        {

            CCUCARLPM cCUCARLPM = new CCUCARLPM();
            cCUCARLPM.ChangeSetOp = ChangeSetOperation.Insert;

            cCUCARLPM.COUNTER = decSupplierInvioceItemsCars.LineNumber;
            cCUCARLPM.RIHBIT = decSupplierInvioceItemsCars.RichbitFileNumber;
            cCUCARLPM.SHEILDNO = decSupplierInvioceItemsCars.VehicleChassisNumber;

            foreach (var decSupplierInvioceItemsCarMods in decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleMods)
            {
                int amount;
                switch (decSupplierInvioceItemsCarMods.AdjustmentTypeCode)
                {
                    case "10":
                        if (int.TryParse(decSupplierInvioceItemsCarMods.DeductAmount.Value.ToString("0"), out amount))
                        {
                            cCUCARLPM.MEMIRDEDUCT = amount;
                        }
                        break;
                    case "9":
                        if (int.TryParse(decSupplierInvioceItemsCarMods.DeductAmount.Value.ToString("0"), out amount))
                        {
                            cCUCARLPM.MADADDEDUCT = amount;
                        }
                        break;
                    default:
                        break;
                }
            }

            return cCUCARLPM;
        }


        private List<CCUCARSCPM> DoCCUCARSC(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItem105PM supplierInvoiceItem105PM, SupplierInvoiceItemPM decSupplierInvoiceItem) // moran 9.3.16 - AMI-55747
        {
            if (decSupplierInvoiceItem.SupplierInvoiceItemVehicles == null)
            {
                return null;
            }
            if (decSupplierInvoiceItem.SupplierInvoiceItemVehicles.Count < 1)
            {
                return null;
            }

            List<CCUCARSCPM> CCUCARSCPMList = new List<CCUCARSCPM>();

            foreach (var decSupplierInvioceItemsCars in decSupplierInvoiceItem.SupplierInvoiceItemVehicles)
            {
                switch (decSupplierInvioceItemsCars.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        break;
                    default:
                        if (decSupplierInvioceItemsCars != null && decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds != null && decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.Count > 0)
                        {// moran 1.9.16 - call 271087 - enter into 'if'
                            CCUCARSCPMList.Add(SetSupplierInvioceItemsCarscs(decSupplierInvoice, decSupplierInvioceItemsCars));
                        }
                        break;
                }
            }

            return CCUCARSCPMList;
        }

        private CCUCARSCPM SetSupplierInvioceItemsCarscs(Def.EntityPMs.SupplierInvoicePM decSupplierInvoice, SupplierInvoiceItemVehiclePM decSupplierInvioceItemsCars) // moran 9.3.16 - AMI-55747
        {
            if (decSupplierInvioceItemsCars == null)
            {
                return null;
            }
            if (decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds == null)
            {
                return null;
            }
            if (decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.Count < 1)
            {
                return null;
            }
            CCUCARSCPM cCUCARSCPM = new CCUCARSCPM();
            cCUCARSCPM.ChangeSetOp = ChangeSetOperation.Insert;
            //cCUCARSCPM.LINENO = decSupplierInvioceItemsCars.InvoiceItemLineNumber;
            cCUCARSCPM.COUNTER = decSupplierInvioceItemsCars.LineNumber;
            cCUCARSCPM.VEHICLEFILE = decSupplierInvioceItemsCars.RichbitFileNumber;
            cCUCARSCPM.CHASSISNO = decSupplierInvioceItemsCars.VehicleChassisNumber;
            cCUCARSCPM.CARMODEL = decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.FirstOrDefault().VehicleModel;
            cCUCARSCPM.ENGINENO = decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.FirstOrDefault().EngineNumber;
            cCUCARSCPM.FOB = decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.FirstOrDefault().VehicleValue.ToNullableDouble("VehicleValue");
            cCUCARSCPM.EXEMPTTYPE = decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.FirstOrDefault().Exempt_type;
            cCUCARSCPM.WINDOWNO = decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.FirstOrDefault().WindowNumber;
            cCUCARSCPM.BUYTAX = decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.FirstOrDefault().ChassisPurchaseTax.ToNullableDouble("ChassisPurchaseTax");
            cCUCARSCPM.GENERALTAX = decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.FirstOrDefault().ChassisTax.ToNullableDouble("ChassisTax");
            cCUCARSCPM.VATRESHIMON = decSupplierInvioceItemsCars.SupplierInvoiceItemVehicleAdds.FirstOrDefault().ChassisVat.ToNullableDouble("ChassisVat");

            return cCUCARSCPM;
        }

        //private bool IsSupplerInvChanged()
        //{
        //    bool existChange = this._DirtyDeclarationPM.SupplierInvoices
        //        .Exists(si =>
        //            si.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None
        //            || si.DeletedSupplierInvoiceItems.Exists(sii => sii.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
        //            || si.SupplierInvoiceItems.Exists(sii =>
        //                sii.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None ||
        //                sii.SupplierInvioceItemsCertificates.Exists(siic => siic.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
        //            )
        //            || si.SupplierInvoiceModifications.Exists(siim => siim.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)

        //            );
        //    return existChange;


        //}

        private bool IsSupplerInvChanged()
        {
            bool existChange = this._DirtyDeclarationPM.SupplierInvoices
                .Exists(si =>
                    si.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None
                    || si.DeletedSupplierInvoiceItems.Exists(sii => sii.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
                    || si.SupplierInvoiceItems.Exists(sii =>
                        sii.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None
                         || sii.SupplierInvioceItemCertificats.Exists(siic => siic.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
                         || sii.SupplierInvoiceItemTaxes.Exists(siit => siit.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
                    )
                    || si.SupplierInvoiceModifications.Exists(sim => sim.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
                    || si.SupplierInvoiceFreightAmounts.Exists(sif => sif.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
                    );
            return existChange;
        }

        private bool IsDeclarationTaxesChanged()
        {
            bool existChange = this._DirtyDeclarationPM.DeclarationTaxes
                .Exists(tax => tax.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None);
            return existChange;
        }

        private bool IsConsignmentChanged()
        {
            bool existChange = this._DirtyDeclarationPM.Consignments
                .Exists(cons =>
                    cons.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None
                    || cons.DeletedConsignmentPackages.Exists(consp => consp.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
                    || cons.ConsignmentPackages.Exists(consp =>
                        consp.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
                        );
            return existChange;
        }

        //static HashSet<string> _HashSet = new HashSet<string>();
        //static Dictionary<string,int>  _HashSet1 = new Dictionary<string,int>();
        //int i;
        private string GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {
            //i++;
            //_HashSet.Add(partnerID + "," + tableID + "," + partnerCode);
            //if (!_HashSet1.ContainsKey(this.GetHashCode().ToString() + tableID))
            //{
            //    _HashSet1[this.GetHashCode().ToString() + tableID] = 0;
            //}
            //_HashSet1[this.GetHashCode().ToString() + tableID] = ++_HashSet1[this.GetHashCode().ToString() + tableID];
            //return "";
            if (partnerID == null || tableID == null || partnerCode == null)
            {
                return ("");
            }

            if (_GTRTRANQueryService == null)
            {
                _GTRTRANQueryService = new GTRTRANQueryService(_AmitalContext);
            }
            if (tableID == "CTBCURRENCY" || tableID == "CTBTARIFF" || tableID == "CTBCOUNTRY")
            {
                return GetTranslationP2LFromCache(partnerID, tableID, partnerCode);
            }
            var myGTRTRANPM = _GTRTRANQueryService.GetSingle(partnerID, tableID, partnerCode, null, true);
            //GTRTRAN myGTRTRANPM = myGTRTRANQueryService.GetTranslationP2L(partnerID, tableID, partnerCode);

            if (myGTRTRANPM == null)
            {
                if (tableID == "CTBBONDED") // moran 15.5.16 - Task 20709
                {
                    CTBBONDEDQueryService myCTBBONDEDQueryService = new CTBBONDEDQueryService(_AmitalContext);

                    var myCTBBONDED = myCTBBONDEDQueryService.GetSingle(partnerCode, true);
                    if (myCTBBONDED != null)
                    {
                        if (!string.IsNullOrWhiteSpace(myCTBBONDED.WAREHOUSEID) && myCTBBONDED.WAREHOUSEID.Length == 4) // moran 24.8.16 - Task 22652 - enter into 'if'
                        {
                            return (myCTBBONDED.WAREHOUSEID);
                        }
                    }
                }
                return ("");
            }

            return (myGTRTRANPM.LOCALCODE);
        }

        private string GetTranslationP2LFromCache(string partnerID, string tableID, string partnerCode)
        {
            var key = partnerID + "'," + tableID;
            if (!_MyLocalCache.ContainsKey(key))
            {
                _MyLocalCache[key] = _GTRTRANQueryService.GetMulti(partnerID, tableID) ?? new List<GTRTRANPM>();
            }
            var myList = _MyLocalCache[key] as List<GTRTRANPM>;
            var recordTR = myList.FirstOrDefault(rec => rec.PARTNERID == partnerID && rec.TABLEID == tableID && rec.PARTNERCODE == partnerCode);
            if (recordTR == null)
            {
                return "";
            }
            return recordTR.LOCALCODE;
        }

        //<--- Yuval Chalup 31.12.2014 AMI-52371
        public void LoadCurrenciesExchangeRates()
        {
            var myCustomsExchangeRateQueryService = new CustomsExchangeRateQueryService(_Context);
            List<string> codesList = new List<string>();
            //Get all Currency types from Invoices, Freights and Modifications
            if (_DirtyDeclarationPM.SupplierInvoices != null)
            {
                if (_DirtyDeclarationPM.SupplierInvoices.Count > 0)
                {
                    foreach (var supplierInvoice in _DirtyDeclarationPM.SupplierInvoices)
                    {
                        if (!string.IsNullOrWhiteSpace(supplierInvoice.InvoiceCurrencyTypeCode))
                        {
                            codesList.Add(supplierInvoice.InvoiceCurrencyTypeCode);
                        }
                        if (supplierInvoice.SupplierInvoiceFreightAmounts != null)
                        {
                            if (supplierInvoice.SupplierInvoiceFreightAmounts.Count > 0)
                            {
                                foreach (var supplierInvoiceFreightAmounts in supplierInvoice.SupplierInvoiceFreightAmounts)
                                {
                                    if (!string.IsNullOrWhiteSpace(supplierInvoiceFreightAmounts.CurrencyTypeCode))
                                    {
                                        codesList.Add(supplierInvoiceFreightAmounts.CurrencyTypeCode);
                                    }
                                }
                            }
                        }
                        if (supplierInvoice.SupplierInvoiceModifications != null)
                        {
                            if (supplierInvoice.SupplierInvoiceModifications.Count > 0)
                            {
                                foreach (var supplierInvoiceModifications in supplierInvoice.SupplierInvoiceModifications)
                                {
                                    if (!string.IsNullOrWhiteSpace(supplierInvoiceModifications.CurrencyTypeCode))
                                    {
                                        codesList.Add(supplierInvoiceModifications.CurrencyTypeCode);
                                    }
                                }
                            }
                        }
                    }

                    //Get Exchange rates for all Currencey types
                    codesList = codesList.Distinct<string>().ToList();
                    string codes = string.Join(",", codesList.ToArray());
                    _CustomsExchangeRates = myCustomsExchangeRateQueryService.GetExchangeRateByCurrencyAndDate(codes, _DirtyDeclarationPM.TaxationDateTime, _DirtyDeclarationPM.Tenant);
                }
            }
        }
        //Yuval Chalup 31.12.2014 AMI-52371 --->


        private void LoadCurrenciesExchangeRates(List<string> codesList) // moran 17.3.16
        {
            if (codesList == null) return;
            var myCustomsExchangeRateQueryService = new CustomsExchangeRateQueryService(_Context);
            List<string> lcodesList = codesList;
            lcodesList = lcodesList.Distinct<string>().ToList();
            string codes = string.Join(",", lcodesList.ToArray());
            if (_CustomsExchangeRates == null)
            {
                _CustomsExchangeRates = myCustomsExchangeRateQueryService.GetExchangeRateByCurrencyAndDate(codes, _DirtyDeclarationPM.TaxationDateTime, _DirtyDeclarationPM.Tenant);
            }
            else
            {
                _CustomsExchangeRates.AddRange(myCustomsExchangeRateQueryService.GetExchangeRateByCurrencyAndDate(codes, _DirtyDeclarationPM.TaxationDateTime, _DirtyDeclarationPM.Tenant));
            }
        }

        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID)
        {
            var myGDFDATAQueryService = new GDFDATAQueryService(_AmitalContext);

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

        public bool _MainAccountSet { get; set; }

        public void Dispose()
        {
            _GTRTRANQueryService = null;
            _DirtyDeclarationPM = null;
            _DBOccDeclarationPM = null;

            _AmitalContext = null;
            _Context = null;
            _CCUFILEMPM = null;

            _GTRTRANQueryService = null;
            _MyLocalCache = null;
        }
    }
}
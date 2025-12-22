using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.SqlClient;
using Devart.Data.Oracle;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Helpers;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using System.Diagnostics;

using Logitude.Customs.BL.Utils;
using Logitude.Customs.BL.Messaging.Customs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.BL;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.Security;
using Unifreight.BL.EntityUpdateServices;
using System.Data.Entity.Core;
using System.Globalization;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceUpdateService : EntityUpdateService<SupplierInvoice, SupplierInvoicePM, EntityPM>
    {
        private bool _FromDec;
        private AmitalContext _AmitalContext;
        private ICustomContext _Context;
        private DeclarationPM _DeclarationPM;
        private bool openTaskForUnifreight;
        private CourierMasterPM _CourierMasterPM;
        private DeclarationCourierStatusPM currentDeclarationCourierStatusPM;
        private DeclarationPM _DeclarationPMAncestor;

        public bool IsProcedureCurrentCodeChanged { get; set; }

        public bool Multi_LastSIWillUpdateCCU { get; set; }//שינוי בלוגיקה לבניית CCU בעקבות משוב להצהרה/הגשה - פניה 303319  אבל במצב הראשון - אין צורך לשמור ולבנות CCU אחרי כל שמירה של כל חשבון ספק. מספיק לבנות את CCU פעם אחת בסיום כל השמירות.
        public bool UpdateFromDeclaration { get; set; }
        protected override void OnCreating(SupplierInvoicePM entityPM, EntityPM entityParentPM)
        {

            _DeclarationPMAncestor = (entityParentPM as DeclarationPM);
            entityPM.DeclarationId = entityPM.DeclarationId ?? _DeclarationPMAncestor.Id;
            var a = 44;
            a = 444;
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug("323");
            ICustomContext _Context = MainContext as CustomContext;

            SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(_Context);
            var declarationPM = entityParentPM as DeclarationPM;
            if (String.IsNullOrWhiteSpace(entityPM.DeclarationId) && declarationPM?.Direction == "E")
            {
                entityPM.DeclarationId = declarationPM.Id;
            }
            int? maxSequenceNumeric = supplierInvoiceQueryService.GetMaxSequenceNumeric(entityPM.DeclarationId, entityPM.Tenant);
            if (maxSequenceNumeric != null)
            {
                entityPM.InvoiceCounterKey = maxSequenceNumeric.Value + 1;
                if (declarationPM == null)
                {


                    DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                    declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false, false);
                    if  ( declarationPM?.IsAmendment == true && entityPM.SequenceNumeric.GetValueOrDefault() == 0)
                        entityPM.SequenceNumeric = entityPM.InvoiceCounterKey;
                }
            }


            //var accumulationFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCUMULATION") && f.ObjectTableId == "Customs.SupplierInvoice Customs.Declaration")[0];

            //FeaturePM feature = TenantContext.Current.Features.Where(d => d.Code == "SENDTESTCASES").FirstOrDefault();
            bool notToCheckFeature = false;
            if (notToCheckFeature)
            {
                if (string.IsNullOrEmpty(entityPM.AccumalationStateCode)) entityPM.AccumalationStateCode = "3";
            }
            else
            {
                try
                {
                    int tenant = entityPM.Tenant;
                    string email = AuthenticationUtil.ResolveUserIdentityName(tenant);
                    string id = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        ContactRepository contactrep = new ContactRepository(tenant);
                        var contact = contactrep.GetSingleContact(id, tenant);
                        email = contact.Email;

                    }
                    InjectionUtil.Instance.CheckContactFeature("Customs.Declaration", "ACCUMULATION", tenant, email);
                    if (string.IsNullOrEmpty(entityPM.AccumalationStateCode)) entityPM.AccumalationStateCode = "1";
                }
                catch (SecurityException ex)
                {
                    LogMessagingUtil.Instance.AppendLine("Check for ACCUMULATION Feature Failed, Message: " + ex.Message);
                    if (string.IsNullOrEmpty(entityPM.AccumalationStateCode)) entityPM.AccumalationStateCode = "3";
                }
                catch (InvalidOperationException ex)
                {
                    LogMessagingUtil.Instance.AppendLine("Invalid operation, possibly due to transaction isolation level: " + ex.Message);
                    // Additional handling for InvalidOperationException if needed
                }
                catch (EntityException ex)
                {
                    LogMessagingUtil.Instance.AppendLine("EntityException occurred, possible database connection issue: " + ex.Message);
                    // Additional handling for EntityException if needed
                }
                catch (Exception ex)
                {
                    LogMessagingUtil.Instance.AppendLine("An unexpected error occurred: " + ex.Message);
                    // Additional handling for other exceptions
                }

            }
            //base.OnCreating(entityPM, entityParentPM);
        }

        public void DeclarationSupplierInvoiceItemsParentsFastDelete(SupplierInvoicePM entityPM, ICustomContext dbContext,bool isParent = true )
        {



            var mySupplierInvoiceItemUpdateService = new SupplierInvoiceItemUpdateService(dbContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            mySupplierInvoiceItemUpdateService.DeclarationSupplierInvoiceItemsParentsFastDeleteComposition(new Data.EntityKeys.SupplierInvoiceKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.InvoiceCounterKey }, dbContext, entityPM.Tenant, isParent);

          
        }

        
        protected override void OnUpdating(SupplierInvoicePM entityPM, SupplierInvoice entityPOCO)
        {
            var sIModificationByCustomerCommissionService = new SIModificationByCustomerCommissionService();
            var curDeclarationPm = _DeclarationPM ?? _DeclarationPMAncestor;
            sIModificationByCustomerCommissionService.EnsureReductionByVendorCommission(curDeclarationPm/*_DeclarationPM*/, entityPM,false);
            if (/*_DeclarationPM*/curDeclarationPm != null && /*_DeclarationPM*/curDeclarationPm.IsCourierDeclaration)
            {
                decimal InvoiceAmountInUSD_round2 = (System.Math.Truncate((decimal)entityPM.InvoiceAmountInUSD.GetValueOrDefault() * 100) / 100);
                bool pHaveChange = //entityPM.InvoiceAmountInUSD 
                    InvoiceAmountInUSD_round2
                    != entityPOCO.InvoiceAmountInUSD;
                if (pHaveChange)
                {
                    DeclarationPM myDBDeclarationPM = null;
                    object AncestorEntityUpdateService = null;
                    this.GetAncestorEntityUpdateService(out AncestorEntityUpdateService);
                    var AncestorDeclarationUpdateService = AncestorEntityUpdateService as DeclarationUpdateService;
                    if (AncestorDeclarationUpdateService != null)
                    {

                        //myDBDeclarationPM = AncestorDeclarationUpdateService.GetDBEntity(entityPM.DeclarationId, entityPM.Tenant);
                    }
                    if (myDBDeclarationPM == null)
                    {
                        DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                        _DeclarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, true, false);

                    }
                    else
                    {
                        _DeclarationPM = myDBDeclarationPM;
                    }
                    if ( _DeclarationPMAncestor?.ChangeSetOp!=  ChangeSetOperation.Insert)// AVOID - DUE INSERT ALREADY SENT IN dECLARATIONuPDATE.oNuPDATE
                    {
                        var mySend2MasofIfNeededService = new Send2MasofIfNeededService();
                        mySend2MasofIfNeededService.Send2Masof(_DeclarationPM ?? curDeclarationPm, pHaveChange, _DeclarationPM);

                    }
                    this.openTaskForUnifreight = true;
                }
            }
            if (_DeclarationPM != null && _DeclarationPM.Direction != "E")
            {
                string remarksClass = ""; string remarksChas = "";

                foreach (SupplierInvoiceItemPM itemPM in entityPM.SupplierInvoiceItems)
                {
                    if (itemPM.ClassificationCode != itemPM.ClassificationCodeSource && itemPM.ClassificationCodeSource != null)
                    {
                        if (remarksClass != "")
                        {
                            remarksClass += "\r";
                        }
                        remarksClass += "מס' חשבון ספק: " + entityPM.InvoiceNumber;
                        remarksClass += " שורת פרט מכס: " + itemPM.LineNumber;
                        remarksClass += " פרט מכס ישן: " + itemPM.ClassificationCodeSource;
                        remarksClass += " פרט מכס חדש: " + itemPM.ClassificationCode;
                    }
                    foreach (SupplierInvoiceItemVehiclePM vehicle in itemPM.SupplierInvoiceItemVehicles)
                    {
                        if ((vehicle.RichbitFileNumberSource != null || vehicle.VehicleChassisNumberSource != null)
                            && ((vehicle.RichbitFileNumber != vehicle.RichbitFileNumberSource && vehicle.RichbitFileNumberSource != null)
                            || (vehicle.VehicleChassisNumber != vehicle.VehicleChassisNumberSource && vehicle.VehicleChassisNumberSource != null)))
                        {
                            if (remarksChas != "")
                            {
                                remarksChas += "\r";
                            }
                            remarksChas += "מס' חשבון ספק: " + entityPM.InvoiceNumber;
                            remarksChas += " שורת פרט מכס: " + itemPM.LineNumber;
                            remarksChas += " שורת שלדה: " + vehicle.LineNumber;
                            if (vehicle.RichbitFileNumberSource != null)
                            {
                                remarksChas += " מס' ריכבית ישן: " + vehicle.RichbitFileNumberSource;
                            }
                            else
                            {
                                remarksChas += " מס' שלדה ישן: " + vehicle.VehicleChassisNumberSource;
                            }
                            if (vehicle.RichbitFileNumber != null)
                            {
                                remarksChas += " מס' ריכבית חדש: " + vehicle.RichbitFileNumber;
                            }
                            else
                            {
                                remarksChas += " מס' שלדה חדש: " + vehicle.VehicleChassisNumber;
                            }
                        }
                    }
                }
                if (remarksClass != "" )
                {
                    SendClass(entityPM.Tenant, _DeclarationPM.CustomFileNo, AuthenticationUtil.ResolveUserId(entityPM.Tenant), remarksClass);
                }
                if (remarksChas != "")
                {
                    SendCHAS(entityPM.Tenant, _DeclarationPM.CustomFileNo, AuthenticationUtil.ResolveUserId(entityPM.Tenant), remarksChas);
                }
            }
            base.OnUpdating(entityPM, entityPOCO);
        }
        protected override void OnUpdating(SupplierInvoicePM entityPM)
        {



            _Context = CustomContext.GetContext(entityPM.Tenant);

 
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            _DeclarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false, false);

            if (_DeclarationPM != null && _DeclarationPM.IsConnectedToUnifreight)
            {
                base.OnUpdating(entityPM);
                SupplierInvoicePM dbOccSupplierInvoicePM = GetDBEntity(entityPM);
                var unifreightFUStatusTaskService = new UnifreightFUStatusTaskService();
                unifreightFUStatusTaskService.DeleteINAFUStatus(_DeclarationPM.Tenant, _DeclarationPM.CustomFileNo);

                if (dbOccSupplierInvoicePM != null && dbOccSupplierInvoicePM.IsValueForCustomsOnly != entityPM.IsValueForCustomsOnly)
                {
                    string xml_status = "new";
                    if (entityPM.IsValueForCustomsOnly != true) xml_status = "del";
                    RaiseStatus(_DeclarationPM, "", "DFC", xml_status);
                }
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            {
                ICustomContext context = this.MainContext as CustomContext;
                CustomsDocumentsTicketQueryService ticketsQueryService = new CustomsDocumentsTicketQueryService(context);
                CustomsDocumentsTicketUpdateService ticketsUpdateService = new CustomsDocumentsTicketUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                List<CustomsDocumentsTicketPM> tickets = ticketsQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(entityPM.DeclarationId, entityPM.InvoiceCounterKey.ToString(), null, null, entityPM.Tenant, "Declaration");
                foreach (CustomsDocumentsTicketPM ticket in tickets)
                {

                    List<CustomsDocumentPointerPM> deletedPointers = (from a in ticket.CustomsDocumentPointers
                                                                      where a.Child1EntityId == entityPM.InvoiceCounterKey.ToString()
                                                                      select a).ToList();



                    foreach (CustomsDocumentPointerPM pointer in deletedPointers)
                    {
                        CustomsDocumentPointerPM deletedPointer = new CustomsDocumentPointerPM() { Id = pointer.Id, ChangeSetOp = ChangeSetOperation.Delete, Tenant = pointer.Tenant, CustomsDocumentsTicketId = ticket.Id,/*DocumentTypeCode=ticket.DocumentTypeCode */};
                        ticket.DeletedCustomsDocumentPointers.Add(deletedPointer);

                    }
                    if (ticket.CustomsDocumentPointers.Count == ticket.DeletedCustomsDocumentPointers.Count)
                    {
                        ticket.ChangeSetOp = ChangeSetOperation.Delete;
                    }
                    else
                    {
                        ticket.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    ticketsUpdateService.Update(ticket, true);
                }
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                if (string.IsNullOrEmpty(entityPM.ChangeInSupplierInvoice)) // Task 49995
                {
                    entityPM.ChangeInSupplierInvoice = "1";
                }
            }
            //CONFLICT HELL !!!
            //if (_DeclarationPM != null)
            //{

            //    //calculate frieghts total
            //    //this.CalculateFrieghtTotals(entityPM, declarationPM);
            //    InsuranceFreightUtil util = new Utils.InsuranceFreightUtil();
            //    util.CalculateFreightForInvoice(entityPM, _DeclarationPM.TaxationDateTime);
            //    entityPM.InvoiceAmountInUSD = InsuranceFreightUtil.CalcInvoiceAmountInUSD(_DeclarationPM.TaxationDateTime, entityPM.InvoiceCurrencyTypeCode, entityPM.InvoiceAmount.GetValueOrDefault(), entityPM.Tenant);
            //}



            object entityPOCO; object entityPM1; object entityParentPM;
            this.GetAncestor(out entityPOCO, out entityPM1, out entityParentPM);
            _DeclarationPMAncestor = (entityPM1 as DeclarationPM);
            if (_DeclarationPMAncestor != null)
            {
                this._FromDec = true;

            }

            DateTime? myTaxationDateTime = _DeclarationPM != null ? _DeclarationPM.TaxationDateTime : _DeclarationPMAncestor?.TaxationDateTime;
            //calculate frieghts total
            //this.CalculateFrieghtTotals(entityPM, declarationPM);
            InsuranceFreightUtil util = new Utils.InsuranceFreightUtil();
            util.CalculateFreightForInvoice(entityPM, myTaxationDateTime/* _DeclarationPM.TaxationDateTime*/);
            entityPM.InvoiceAmountInUSD = InsuranceFreightUtil.CalcInvoiceAmountInUSD(myTaxationDateTime/*_DeclarationPM.TaxationDateTime*/, entityPM.InvoiceCurrencyTypeCode, entityPM.InvoiceAmount.GetValueOrDefault(), entityPM.Tenant);



            this.SetDeclarationChanged(entityPM);
            this.UpdateDeclarationPlatformFeeAndPrimaryInvoice(entityPM);
                  }



        private SupplierInvoicePM GetDBEntity(SupplierInvoicePM dirtySupplierInvoicePM)
        {

            var supplierInvoiceQueryService = new SupplierInvoiceQueryService(dirtySupplierInvoicePM.Tenant);
            var myDBEntity = supplierInvoiceQueryService.GetSingle(dirtySupplierInvoicePM.DeclarationId, dirtySupplierInvoicePM.InvoiceCounterKey, true, false);
            return myDBEntity ?? new SupplierInvoicePM();

        }

        public void SendClass(int Tenant, string CustomFileNo, string loggedContactId, string remarks)
        {
         
                if (string.IsNullOrWhiteSpace(loggedContactId))
                {
                    ContactRepository contactRepository = new ContactRepository(Tenant);
                    var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
                    if (loggedContact != null)
                    {
                        loggedContactId = loggedContact.Id;
                    }
                }
                string unifrieghtEvent = "CLASS";
                string eventRemarks = remarks;
                var MyUnifreightEventParam = new UnifreightEventParam()
                {
                    Code = unifrieghtEvent,
                    Mode = UnifreightEventMode.@new,
                    EventDateTime = DateTime.Now,
                    Entname = "CFIFILEM",
                    PrimaryNum = CustomFileNo,
                    EventRemarks = eventRemarks,
                };
                LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                var myOpenUnifreighTask = new UnifreightEventTaskService();
                myOpenUnifreighTask.UpsertEventLE2U(
                    Tenant,
                    loggedContactId,
                    MyUnifreightEventParam);
           
        }
        public void SendCHAS(int Tenant, string CustomFileNo, string loggedContactId, string remarks)
        {
        
                if (string.IsNullOrWhiteSpace(loggedContactId))
                {
                    ContactRepository contactRepository = new ContactRepository(Tenant);
                    var loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.ResolveLoggingUserId(Tenant), Tenant);
                    if (loggedContact != null)
                    {
                        loggedContactId = loggedContact.Id;
                    }
                }
                string unifrieghtEvent = "CHAS";
                string eventRemarks = remarks;
                var MyUnifreightEventParam = new UnifreightEventParam()
                {
                    Code = unifrieghtEvent,
                    Mode = UnifreightEventMode.@new,
                    EventDateTime = DateTime.Now,
                    Entname = "CFIFILEM",
                    PrimaryNum = CustomFileNo,
                    EventRemarks = eventRemarks,
                };
                LogMessagingUtil.Instance.AppendLine("MyUnifreightEventParam = " + MyUnifreightEventParam ?? "NULL");
                var myOpenUnifreighTask = new UnifreightEventTaskService();
                myOpenUnifreighTask.UpsertEventLE2U(
                    Tenant,
                    loggedContactId,
                    MyUnifreightEventParam);
           
        }

        private void SetDeclarationChanged(SupplierInvoicePM entityPM)
        {



            if (this.UpdateFromDeclaration) return;
            RealSetDeclarationChanged(entityPM.Tenant, entityPM.DeclarationId);
        }

        public static void RealSetDeclarationChanged(int tenant, string declarationId)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(tenant);
            if (dbms == "oracle")
            {

                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update Declarations set IsChanged = 1";
                    //cmd = cmd + " where Id=" + "'" + entityPM.DeclarationId + "'";
                    cmd = cmd + " where Id=:p1 ";

                    OracleCommand sqlCommand = new OracleCommand(cmd, con);
                    sqlCommand.Parameters.Add(new OracleParameter("p1", declarationId));
                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = "Update Customs.Declarations set IsChanged = 1";
                    cmd = cmd + " where Id=" + "'" + declarationId + "'";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        private void UpdateDeclarationPlatformFeeAndPrimaryInvoice(SupplierInvoicePM entityPM)
        {
            ICustomContext _context = MainContext as CustomContext;
            SupplierInvoiceModificationQueryService supplierInvoiceModQueryService = new SupplierInvoiceModificationQueryService(_context);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(_context);
            // declarationQueryService.LoadSupplierInvoices = false;
            if (this._FromDec) return;
            List<SupplierInvoiceModificationPM> modifications = entityPM.SupplierInvoiceModifications;//supplierInvoiceModQueryService.GetSupplierInvoiceModificationsForDeclaration(entityPM.DeclarationId);

            decimal? platformFee = 0;
            platformFee += modifications.Where(a => a.TypeCode == "I02").Sum(d => d.Amount);
            platformFee += modifications.Where(a => a.TypeCode == "I01").Sum(d => d.Amount);
            int count = 0;

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(_context);
                count = supplierInvoiceQueryService.GetSupplierInvoiceCountForDeclaration(entityPM.DeclarationId, entityPM.Tenant);


                //if (count == 0)
                //{
                //    //declarationPM.PrimaryInvoiceCounterKey = entityPM.InvoiceCounterKey.ToString();
                //}
            }

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(entityPM.Tenant);
            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update Declarations set PlatformFee=" + platformFee;
                    cmd = cmd + ",IsValueForCustomsOnly=" + (entityPM.IsValueForCustomsOnly == false ? "0" : "1");
                    if (entityPM.ChangeSetOp == ChangeSetOperation.Insert && count == 0)
                    {
                        cmd = cmd + ",PrimaryInvoiceCounterKey=" + entityPM.InvoiceCounterKey;
                    }
                    cmd = cmd + " where Id=" + "'" + entityPM.DeclarationId + "'";
                    OracleCommand sqlCommand = new OracleCommand(cmd, con);

                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = "Update Customs.Declarations set PlatformFee=" + platformFee;
                    cmd = cmd + ",IsValueForCustomsOnly=" + (entityPM.IsValueForCustomsOnly == false ? "0" : "1");
                    if (entityPM.ChangeSetOp == ChangeSetOperation.Insert && count == 0)
                    {
                        cmd = cmd + ",PrimaryInvoiceCounterKey=" + entityPM.InvoiceCounterKey;
                    }
                    cmd = cmd + " where Id=" + "'" + entityPM.DeclarationId + "'";
                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
            //DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(_context, new Dictionary<string, IContext>(), entityPM.Tenant);
            //DeclarationPM declarationPM = declarationQueryService.GetSingleDeclarationById(entityPM.DeclarationId, entityPM.Tenant);

            //if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            //{

            //    SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(_context);
            //    int count = supplierInvoiceQueryService.GetSupplierInvoiceCountForDeclaration(entityPM.DeclarationId, entityPM.Tenant);
            //    int? maxCounterKey = supplierInvoiceQueryService.GetMaxCounterKey(entityPM.DeclarationId, entityPM.Tenant);
            //    if (maxCounterKey != null)
            //    {
            //        entityPM.InvoiceCounterKey = maxCounterKey.Value + 1;
            //    }
            //    if (count == 0)
            //    {
            //        declarationPM.PrimaryInvoiceCounterKey = entityPM.InvoiceCounterKey.ToString();
            //    }
            //}
            //if (this._FromDec) return;
            //List<SupplierInvoiceModificationPM> modifications = supplierInvoiceModQueryService.GetSupplierInvoiceModificationsForDeclaration(entityPM.DeclarationId);

            //decimal? platformFee = 0;
            //platformFee += modifications.Where(a => a.TypeCode == "I02").Sum(d => d.Amount);
            //platformFee += modifications.Where(a => a.TypeCode == "I01").Sum(d => d.Amount);

            //declarationPM.PlatformFee = platformFee;

            //declarationPM.ChangeSetOp = ChangeSetOperation.Update;
            //declarationUpdateService.Update(declarationPM, false);

        }

        protected override void UpdateComposition(SupplierInvoicePM entityPM)
        {

            if (entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            {

            }
            entityPM.SupplierInvoiceItems.ForEach(x => x.Direction = _DeclarationPM?.Direction);

            SupplierInvoiceItemUpdateService supplierInvoiceUpdateService = new SupplierInvoiceItemUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            supplierInvoiceUpdateService.UpdateMulti(entityPM.SupplierInvoiceItems, entityPM.DeletedSupplierInvoiceItems, entityPM, false);

            SupplierInvoiceModificationUpdateService supplierInvoiceModificationUpdateService = new SupplierInvoiceModificationUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            supplierInvoiceModificationUpdateService.UpdateMulti(entityPM.SupplierInvoiceModifications, entityPM.DeletedSupplierInvoiceModifications, entityPM, false);

            SupplierInvoiceFreightAmountUpdateService supplierInvoiceFreightAmountUpdateService = new SupplierInvoiceFreightAmountUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            supplierInvoiceFreightAmountUpdateService.UpdateMulti(entityPM.SupplierInvoiceFreightAmounts, entityPM.DeletedSupplierInvoiceFreightAmounts, entityPM, false);

            SupplierInvoicePaymentUpdateService supplierInvoicePaymentUpdateService = new SupplierInvoicePaymentUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            supplierInvoicePaymentUpdateService.UpdateMulti(entityPM.SupplierInvoicePayments, entityPM.DeletedSupplierInvoicePayments, entityPM, false);

            SupplierInvoiceUCRUpdateService supplierInvoiceUCRUpdateService = new SupplierInvoiceUCRUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            supplierInvoiceUCRUpdateService.UpdateMulti(entityPM.SupplierInvoiceUCRs, entityPM.DeletedSupplierInvoiceUCRs, entityPM, false);


            base.UpdateComposition(entityPM);
        }


        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceRepository).FastDeleteMulti(entityKeyFields);
        }

        protected override void AfterUpdating(SupplierInvoicePM entityPM, EntityPM entityParentPM)
        {
            //--------calculating sequence numerics for invoices.


            bool isSubmitChanges = false;
            //bool isInvoiceInsert = (from a in entityPM.SupplierInvoices
            //                        where a.ChangeSetOp == ChangeSetOperation.Insert
            //                        select a).Any();

            //bool isInvoiceDelete = (from a in entityPM.DeletedSupplierInvoices
            //                        select a).Any();
            ICustomContext context = MainContext as CustomContext;
            SupplierInvoiceRepository invoiceRepository = new SupplierInvoiceRepository(context);
            List<SupplierInvoice> supplierInvoices = null; // invoiceRepository.GetMulti(new DeclarationKeys() { Id = entityPM.DeclarationId });
            //supplierInvoices = supplierInvoices.OrderBy(d => d.InvoiceCounterKey).ToList();
            string shopId = null;
            if (currentDeclarationCourierStatusPM == null)
            {
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPM.DeclarationId, true, false);
            }
            if (currentDeclarationCourierStatusPM != null && !String.IsNullOrWhiteSpace(currentDeclarationCourierStatusPM.ShopId))
            {
                Card myCard = null;
                var repository = new CardRepository(entityPM.Tenant);
                myCard = repository.GetSingleCard(currentDeclarationCourierStatusPM.ShopId, entityPM.Tenant);
                if (myCard != null && !String.IsNullOrWhiteSpace(myCard.Code)) shopId = myCard.Code;
            }

            bool dirty = false;
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingle(entityPM.DeclarationId, false, false)?? _DeclarationPMAncestor;
            DeclarationPM defaultDeclarationPM = (DeclarationPM)entityParentPM;
            if (defaultDeclarationPM == null) defaultDeclarationPM = declarationPM;
            declarationQueryService.GetSingle(entityPM.DeclarationId, false, false);
            bool toUpdateClassification = false;
            string defaultClassificationCode = null;
            string defaultClassificationCodeUnit = null;
            bool isInvoiceItemInsertNullClassification = (from a in entityPM.SupplierInvoiceItems
                                                          where a.ChangeSetOp == ChangeSetOperation.Insert && a.ClassificationCode is null
                                                          select a).Any();
            //if ((entityPM.ChangeSetOp == ChangeSetOperation.Insert || (entityPM.ChangeSetOp == ChangeSetOperation.Update && IsProcedureCurrentCodeChanged)) && declarationPM.IsCourierDeclaration && entityPM.InvoiceAmountInUSD <= 1000 && (declarationPM.ProcedureCurrentCode == "4000512" || declarationPM.ProcedureCurrentCode == "4000507"))
            LogMessagingUtil.Instance.AppendLine($"ChangeSetOp{entityPM.ChangeSetOp} IsProcedureCurrentCodeChanged{IsProcedureCurrentCodeChanged} ProcedureCurrentCode{defaultDeclarationPM.ProcedureCurrentCode}");
            if ((entityPM.ChangeSetOp == ChangeSetOperation.Insert || (entityPM.ChangeSetOp == ChangeSetOperation.Update && isInvoiceItemInsertNullClassification || IsProcedureCurrentCodeChanged)) && defaultDeclarationPM.IsCourierDeclaration && entityPM.InvoiceAmountInUSD <= 1000 && (defaultDeclarationPM.ProcedureCurrentCode == "4000512" || defaultDeclarationPM.ProcedureCurrentCode == "4000507"))
             {
                try
                {
                    using (_AmitalContext = AmitalContext.GetContext(entityPM.Tenant))
                    {
                        
                        if (isInvoiceItemInsertNullClassification || IsProcedureCurrentCodeChanged)
                        {
                            string IntegratorCode = null;
                            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(entityPM.Tenant);


                            if (_CourierMasterPM == null)
                            {
                                var myCourierMasterQueryService = new CourierMasterQueryService(context);
                                _CourierMasterPM = myCourierMasterQueryService.GetByDeclarationId(declarationPM.Id, entityPM.Tenant)?? _DeclarationPMAncestor?.MyEcomInsert?.MyCourierMasterPM;
                            }

                            if (_CourierMasterPM != null)
                            {
                                Card myCard = null;
                                var repository = new CardRepository(entityPM.Tenant);
                                myCard = repository.GetSingleCard(_CourierMasterPM.IntegratorCode, entityPM.Tenant);
                                if (myCard != null && !String.IsNullOrWhiteSpace(myCard.Code)) IntegratorCode = myCard.Code;
                            }
                            LogMessagingUtil.Instance.AppendLine($"InvoiceAmountInUSD{entityPM.InvoiceAmountInUSD} shopId{shopId} IntegratorCode{IntegratorCode}");

                            decimal minValPay = 75m;
                            var minValPayStr = defaultValueQueryService.GetDefault("ISRAEL", "CGO_MINVAL_PAY", "NON", "NON", entityPM.Tenant);

                            if (!string.IsNullOrWhiteSpace(minValPayStr))
                            {
                                var normalized = minValPayStr.Trim().Replace(",", ".");
                                if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed) && parsed > 0)
                                {
                                    minValPay = parsed;
                                }
                            }
                            if (entityPM.InvoiceAmountInUSD <= minValPay)
                            {
                                if (!String.IsNullOrWhiteSpace(shopId)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_LOWVAL_ITM", "NON", shopId, entityPM.Tenant);
                                if (String.IsNullOrWhiteSpace(defaultClassificationCode) && !String.IsNullOrWhiteSpace(IntegratorCode)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_LOWVAL_ITM", "NON", IntegratorCode, entityPM.Tenant);
                                if (String.IsNullOrWhiteSpace(defaultClassificationCode)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_LOWVAL_ITEM", "NON", "NON", entityPM.Tenant);
                            }
                            else if (entityPM.InvoiceAmountInUSD > minValPay && entityPM.InvoiceAmountInUSD <= 500)
                            {
                                if (!String.IsNullOrWhiteSpace(shopId)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_VAL2_ITM", "NON", shopId, entityPM.Tenant);
                                if (String.IsNullOrWhiteSpace(defaultClassificationCode) && !String.IsNullOrWhiteSpace(IntegratorCode)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_VAL2_ITM", "NON", IntegratorCode, entityPM.Tenant);
                                if (String.IsNullOrWhiteSpace(defaultClassificationCode)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_VAL2_ITEM", "NON", "NON", entityPM.Tenant);
                            }
                            else if (entityPM.InvoiceAmountInUSD > 500 && entityPM.InvoiceAmountInUSD <= 1000)
                            {
                                if (!String.IsNullOrWhiteSpace(shopId)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_VAL3_ITM", "NON", shopId, entityPM.Tenant);
                                if (String.IsNullOrWhiteSpace(defaultClassificationCode) && !String.IsNullOrWhiteSpace(IntegratorCode)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_VAL3_ITM", "NON", IntegratorCode, entityPM.Tenant);
                                if (String.IsNullOrWhiteSpace(defaultClassificationCode)) defaultClassificationCode = defaultValueQueryService.GetDefault("ISRAEL", "CGO_VAL3_ITEM", "NON", "NON", entityPM.Tenant);
                            }
                            if (!string.IsNullOrWhiteSpace(defaultClassificationCode))
                            {
                                CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(entityPM.Tenant);
                                foreach (SupplierInvoiceItemPM item in entityPM.SupplierInvoiceItems)
                                {
                                    if (string.IsNullOrWhiteSpace(item.ClassificationCode))
                                    {
                                        item.ClassificationCode = defaultClassificationCode;
                                        if (!string.IsNullOrWhiteSpace(defaultClassificationCodeUnit))
                                        {
                                            item.InvoiceQuantityType = defaultClassificationCodeUnit;
                                        }
                                        else
                                        {
                                            item.InvoiceQuantityType = customsItemQueryService.GetQuantityTypeByClassificationCode(item.ClassificationCode, entityPM.Tenant);
                                            defaultClassificationCodeUnit = item.InvoiceQuantityType;
                                        }
                                    }
                                }
                                toUpdateClassification = true;
                            }
                        }
                        LogMessagingUtil.Instance.AppendLine($"defaultClassificationCode{defaultClassificationCode} toUpdateClassification{toUpdateClassification} ");

                    }
                }
                catch (Exception ex)
                {
                    ex.ChangeExceptionMessage("AfterUpdating update ClassificationCode default Exception");
                    throw;
                }
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Delete || entityPM.ChangeSetOp == ChangeSetOperation.Insert || toUpdateClassification)
            {
                SubmitChanges();
                //if(supplierInvoices == null || supplierInvoices.Count() < 1)
                {
                    supplierInvoices = invoiceRepository.GetMulti(new DeclarationKeys() { Id = entityPM.DeclarationId });
                    supplierInvoices = supplierInvoices.OrderBy(d => d.InvoiceCounterKey).ToList();
                }

                isSubmitChanges = true;
                int index = 0;
                var my = new SupplierInvoiceKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.InvoiceCounterKey };

                if (defaultDeclarationPM.IsAmendment != true )
                {


                    foreach (SupplierInvoice item in supplierInvoices)
                    {

                        index += 1;
                        if (item.SequenceNumeric == index && !toUpdateClassification) continue;
                        dirty = true;
                        item.SequenceNumeric = index;
                        if (my.GetFullKey() == item.DeclarationId + '_' + item.InvoiceCounterKey)
                        {
                            //update SequenceNumeric Soo the Client will have Accurate PM !!!!!!
                            entityPM.SequenceNumeric = item.SequenceNumeric;
                            if (toUpdateClassification)
                            {
                                SupplierInvoiceItemRepository invoiceItemRepository = new SupplierInvoiceItemRepository(context);
                                List<SupplierInvoiceItem> supplierInvoiceItems = invoiceItemRepository.GetMulti(new SupplierInvoiceKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.InvoiceCounterKey });
                                foreach (SupplierInvoiceItem SIitem in supplierInvoiceItems)
                                {
                                    if (string.IsNullOrWhiteSpace(SIitem.ClassificationCode))
                                    {
                                        SIitem.ClassificationCode = defaultClassificationCode;
                                        if (string.IsNullOrWhiteSpace(SIitem.InvoiceQuantityType)) SIitem.InvoiceQuantityType = defaultClassificationCodeUnit;
                                        invoiceItemRepository.Update(SIitem);
                                        LogMessagingUtil.Instance.AppendLine($"upsdate SIitem.ClassificationCode{SIitem.ClassificationCode} ");

                                    }
                                }
                            }
                        }

                        invoiceRepository.Update(item);
                    }
                }



            }
            invoiceRepository.SubmitChanges();
            InsuranceFreightUtil insuranceFreightUtil = new Utils.InsuranceFreightUtil();
            insuranceFreightUtil.CalculateInsurance(declarationPM ?? defaultDeclarationPM);

            #region insurance old calculations 
            //SupplierInvoice invoice = supplierInvoices.Where(d => d.SequenceNumeric == 1).FirstOrDefault();
            //CustomsExchangeRatePM firstExchangeRate = null;
            //decimal? firstInvoiceRate = null;

            //if (invoice != null && invoice.InsruancePercentage != null)
            //{
            //    CustomsExchangeRateQueryService CustomsExchangeRateQueryService = new CustomsExchangeRateQueryService(_Context);
            //    //List<CustomsExchangeRatePM> customsExchangeRates = CustomsExchangeRateQueryService.GetCustomsExchangeRateForDate(declarationPM.TaxationDateTime, entityPM.Tenant);
            //    List<CustomsExchangeRatePM> customsExchangeRates = new List<CustomsExchangeRatePM>();
            //    CustomsExchangeRatePM customsExchangeRate = new CustomsExchangeRatePM();
            //    firstExchangeRate = CustomsExchangeRateQueryService.GetCustomsExchangeRateForDateAndCurrencyTypeCode(invoice.InvoiceCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //    if (firstExchangeRate != null)
            //    {
            //        customsExchangeRates.Add(firstExchangeRate);
            //        firstInvoiceRate = firstExchangeRate.ExchangeRate;
            //    }
            //    if (!string.IsNullOrWhiteSpace(entityPM.InvoiceCurrencyTypeCode))
            //    {
            //        customsExchangeRate = CustomsExchangeRateQueryService.GetCustomsExchangeRateForDateAndCurrencyTypeCode(entityPM.InvoiceCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //        if (customsExchangeRate != null) customsExchangeRates.Add(customsExchangeRate);
            //    }
            //    if (!string.IsNullOrWhiteSpace(entityPM.FreightCurrencyTypeCode) && entityPM.FreightCurrencyTypeCode != entityPM.InvoiceCurrencyTypeCode)
            //    {
            //        customsExchangeRate = CustomsExchangeRateQueryService.GetCustomsExchangeRateForDateAndCurrencyTypeCode(entityPM.FreightCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //        if (customsExchangeRate != null) customsExchangeRates.Add(customsExchangeRate);
            //    }

            //    decimal? InvocieCurrencyRate = 0;
            //    decimal? rate = 0;
            //    decimal? totalFreight = 0;
            //    decimal? totalAmount = 0;
            //    decimal? total = 0;
            //    decimal? amount = 0;

            //    if (entityPM.ChangeSetOp != ChangeSetOperation.Delete)
            //    {
            //        var value = customsExchangeRates.Where(d => d.CurrencyTypeCode == entityPM.InvoiceCurrencyTypeCode).FirstOrDefault();
            //        if (value != null)
            //        {

            //            InvocieCurrencyRate = value.ExchangeRate;

            //        }




            //        amount = entityPM.InvoiceAmount == null ? 0 : entityPM.InvoiceAmount;
            //        //if (invoice != null && entityPM.InvoiceCurrencyTypeCode != invoice.InvoiceCurrencyTypeCode)
            //        //{
            //        //    if (amount != null && InvocieCurrencyRate != null)
            //        //    {
            //        //        amount = amount * InvocieCurrencyRate;
            //        //    }
            //        //}
            //        total = entityPM.TotalFreightInFreightCurrency == null ? 0 : entityPM.TotalFreightInFreightCurrency;
            //        var FreightCurrencyRate = customsExchangeRates.Where(d => d.CurrencyTypeCode == entityPM.FreightCurrencyTypeCode).FirstOrDefault();
            //        if (FreightCurrencyRate != null)
            //        {
            //            rate = FreightCurrencyRate.ExchangeRate;
            //            if (InvocieCurrencyRate > 0)
            //            {
            //                total = total * (rate / InvocieCurrencyRate);
            //            }
            //            else
            //            {
            //                total = total * rate;
            //            }
            //        }
            //    }

            //    else
            //    {
            //        //foreach (SupplierInvoice item in supplierInvoices)
            //        //{

            //        //    dirty = true;


            //        //    if (invoice != null && item.FreightCurrencyTypeCode != invoice.InvoiceCurrencyTypeCode)
            //        //    {
            //        //        if (item.TotalFreightInFreightCurrency != null)
            //        //        {
            //        //            var result = customsExchangeRates.Where(d => d.CurrencyTypeCode == item.FreightCurrencyTypeCode).FirstOrDefault();
            //        //            if (result != null && InvocieCurrencyRate != 0)
            //        //            {
            //        //                rate = result.ExchangeRate;

            //        //                totalFreight = totalFreight + (item.TotalFreightInFreightCurrency * (rate / InvocieCurrencyRate));
            //        //            }
            //        //        }
            //        //    }
            //        //    else
            //        //    {
            //        //        totalFreight = totalFreight + item.TotalFreightInFreightCurrency;
            //        //    }
            //        //    if (invoice != null && item.InvoiceCurrencyTypeCode != invoice.InvoiceCurrencyTypeCode)
            //        //    {
            //        //        if (!string.IsNullOrEmpty(item.InvoiceCurrencyTypeCode))
            //        //        {
            //        //            var x = customsExchangeRates.Where(d => d.CurrencyTypeCode == item.InvoiceCurrencyTypeCode).FirstOrDefault();
            //        //            if (x != null && InvocieCurrencyRate != 0)
            //        //            {
            //        //                rate = x.ExchangeRate;


            //        //                totalAmount = totalAmount + (item.InvoiceAmount == null ? item.InvoiceAmount = 0 : item.InvoiceAmount * (rate / InvocieCurrencyRate));
            //        //            }
            //        //        }
            //        //    }
            //        //    else
            //        //    {
            //        //        totalAmount = totalAmount + (item.InvoiceAmount == null ? item.InvoiceAmount = 0 : item.InvoiceAmount);
            //        //    }


            //        //    //  invoiceRepository.Update(item);

            //        //}

            //    }


            //    SupplierInvoice currenctInvoice = supplierInvoices.Where(d => d.DeclarationId == entityPM.DeclarationId && d.InvoiceCounterKey == entityPM.InvoiceCounterKey).FirstOrDefault();

            //    if (currenctInvoice != null)
            //    {
            //        foreach (SupplierInvoice item in supplierInvoices.Where(d => d.SequenceNumeric != currenctInvoice.SequenceNumeric))
            //        {

            //            dirty = true;

            //            if (item.SequenceNumeric != currenctInvoice.SequenceNumeric)
            //            {

            //                if (!string.IsNullOrWhiteSpace(item.InvoiceCurrencyTypeCode) && customsExchangeRates.Where(r => r.CurrencyTypeCode == item.InvoiceCurrencyTypeCode).FirstOrDefault() == null)
            //                {
            //                    customsExchangeRate = CustomsExchangeRateQueryService.GetCustomsExchangeRateForDateAndCurrencyTypeCode(item.InvoiceCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //                    if (customsExchangeRate != null) customsExchangeRates.Add(customsExchangeRate);
            //                }
            //                if (!string.IsNullOrWhiteSpace(item.FreightCurrencyTypeCode) && customsExchangeRates.Where(r => r.CurrencyTypeCode == item.FreightCurrencyTypeCode).FirstOrDefault() == null)
            //                {
            //                    customsExchangeRate = CustomsExchangeRateQueryService.GetCustomsExchangeRateForDateAndCurrencyTypeCode(item.FreightCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //                    if (customsExchangeRate != null) customsExchangeRates.Add(customsExchangeRate);
            //                }
            //                if (item.FreightCurrencyTypeCode != currenctInvoice.InvoiceCurrencyTypeCode)
            //                {
            //                    if (item.TotalFreightInFreightCurrency != null)
            //                    {
            //                        var result = customsExchangeRates.Where(d => d.CurrencyTypeCode == item.FreightCurrencyTypeCode).FirstOrDefault();
            //                        if (result != null && InvocieCurrencyRate != 0)
            //                        {
            //                            rate = result.ExchangeRate;

            //                            totalFreight = totalFreight + (item.TotalFreightInFreightCurrency * (rate / InvocieCurrencyRate));
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    totalFreight = totalFreight + item.TotalFreightInFreightCurrency;
            //                }
            //                if (item.InvoiceCurrencyTypeCode != currenctInvoice.InvoiceCurrencyTypeCode)
            //                {
            //                    if (!string.IsNullOrEmpty(item.InvoiceCurrencyTypeCode))
            //                    {
            //                        var x = customsExchangeRates.Where(d => d.CurrencyTypeCode == item.InvoiceCurrencyTypeCode).FirstOrDefault();
            //                        if (x != null && InvocieCurrencyRate != 0)
            //                        {
            //                            rate = x.ExchangeRate;


            //                            totalAmount = totalAmount + (item.InvoiceAmount == null ? item.InvoiceAmount = 0 : item.InvoiceAmount * (rate / InvocieCurrencyRate));
            //                        }
            //                        else
            //                        {
            //                            totalAmount = totalAmount + (item.InvoiceAmount == null ? item.InvoiceAmount = 0 : item.InvoiceAmount);
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    totalAmount = totalAmount + (item.InvoiceAmount == null ? item.InvoiceAmount = 0 : item.InvoiceAmount);
            //                }
            //            }

            //            //  invoiceRepository.Update(item);

            //        }

            //    }
            //    else if (invoice != null)
            //    {
            //        amount = invoice.InvoiceAmount == null ? 0 : invoice.InvoiceAmount;
            //        total = invoice.TotalFreightInFreightCurrency == null ? 0 : invoice.TotalFreightInFreightCurrency;
            //        var FreightCurrencyRate = customsExchangeRates.Where(d => d.CurrencyTypeCode == invoice.FreightCurrencyTypeCode).FirstOrDefault();
            //        if (FreightCurrencyRate != null)
            //        {
            //            rate = FreightCurrencyRate.ExchangeRate;
            //            if (firstInvoiceRate > 0)
            //            {
            //                total = total * (rate / firstInvoiceRate);
            //            }
            //            else
            //            {
            //                total = total * rate;
            //            }
            //        }

            //        foreach (SupplierInvoice item in supplierInvoices.Where(d => d.SequenceNumeric != invoice.SequenceNumeric))
            //        {

            //            dirty = true;

            //            if (item.SequenceNumeric != invoice.SequenceNumeric)
            //            {

            //                if (!string.IsNullOrWhiteSpace(item.InvoiceCurrencyTypeCode) && customsExchangeRates.Where(r => r.CurrencyTypeCode == item.InvoiceCurrencyTypeCode).FirstOrDefault() == null)
            //                {
            //                    customsExchangeRate = CustomsExchangeRateQueryService.GetCustomsExchangeRateForDateAndCurrencyTypeCode(item.InvoiceCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //                    if (customsExchangeRate != null) customsExchangeRates.Add(customsExchangeRate);
            //                }
            //                if (!string.IsNullOrWhiteSpace(item.FreightCurrencyTypeCode) && customsExchangeRates.Where(r => r.CurrencyTypeCode == item.FreightCurrencyTypeCode).FirstOrDefault() == null)
            //                {
            //                    customsExchangeRate = CustomsExchangeRateQueryService.GetCustomsExchangeRateForDateAndCurrencyTypeCode(item.FreightCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //                    if (customsExchangeRate != null) customsExchangeRates.Add(customsExchangeRate);
            //                }
            //                if (invoice != null && item.FreightCurrencyTypeCode != invoice.InvoiceCurrencyTypeCode)
            //                {
            //                    if (item.TotalFreightInFreightCurrency != null)
            //                    {
            //                        var result = customsExchangeRates.Where(d => d.CurrencyTypeCode == item.FreightCurrencyTypeCode).FirstOrDefault();
            //                        if (result != null && firstInvoiceRate != 0)
            //                        {
            //                            rate = result.ExchangeRate;

            //                            totalFreight = totalFreight + (item.TotalFreightInFreightCurrency * (rate / firstInvoiceRate));
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    totalFreight = totalFreight + item.TotalFreightInFreightCurrency;
            //                }
            //                if (invoice != null && item.InvoiceCurrencyTypeCode != invoice.InvoiceCurrencyTypeCode)
            //                {
            //                    if (!string.IsNullOrEmpty(item.InvoiceCurrencyTypeCode))
            //                    {
            //                        var x = customsExchangeRates.Where(d => d.CurrencyTypeCode == item.InvoiceCurrencyTypeCode).FirstOrDefault();
            //                        if (x != null && firstInvoiceRate != 0)
            //                        {
            //                            rate = x.ExchangeRate;


            //                            totalAmount = totalAmount + (item.InvoiceAmount == null ? item.InvoiceAmount = 0 : item.InvoiceAmount * (rate / firstInvoiceRate));
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    totalAmount = totalAmount + (item.InvoiceAmount == null ? item.InvoiceAmount = 0 : item.InvoiceAmount);
            //                }
            //            }

            //            //  invoiceRepository.Update(item);

            //        }
            //    }

            //    if (invoice != null && invoice.InsruancePercentage > 0)
            //    {
            //        if (total == null) total = 0;
            //        if (totalAmount == null) totalAmount = 0;
            //        if (totalFreight == null) totalFreight = 0;
            //        if (amount == null) amount = 0;
            //        decimal? insuranceAmountInCurrentInvoiceCurrency = (amount + total + totalAmount + totalFreight) * (invoice.InsruancePercentage / 100);

            //        decimal? insuranceAmountInFirstInvoiceCurrency = insuranceAmountInCurrentInvoiceCurrency;
            //        if (currenctInvoice != null && currenctInvoice.InvoiceCounterKey != invoice.InvoiceCounterKey)
            //        {
            //            if (!string.IsNullOrWhiteSpace(invoice.InvoiceCurrencyTypeCode) && !string.IsNullOrWhiteSpace(currenctInvoice.InvoiceCurrencyTypeCode))
            //            {


            //                insuranceAmountInFirstInvoiceCurrency = insuranceAmountInCurrentInvoiceCurrency * InvocieCurrencyRate / firstInvoiceRate;
            //            }
            //        }


            //        invoice.InsuranceAmount = insuranceAmountInFirstInvoiceCurrency;
            //        invoice.InsuranceAmount = Math.Round(invoice.InsuranceAmount.Value, 2, MidpointRounding.AwayFromZero);
            //        invoiceRepository.Update(invoice);
            //        dirty = true;
            //    }
            //}
            //if (dirty)
            //{
            //    invoiceRepository.SubmitChanges();
            //}
            #endregion


            //--------calculating sequence numerics for invoiceitems.
            //foreach (SupplierInvoicePM invoicePM in entityPM.SupplierInvoices.Where(d => d.ChangeSetOp == ChangeSetOperation.Update || d.ChangeSetOp == ChangeSetOperation.Insert))
            //{

            bool isInvoiceItemInsert = (from a in entityPM.SupplierInvoiceItems
                                        where a.ChangeSetOp == ChangeSetOperation.Insert
                                        select a).Any();

            bool isInvoiceItemDelete = (from a in entityPM.DeletedSupplierInvoiceItems
                                        select a).Any();
            if (!isInvoiceItemDelete)
            {
                isInvoiceItemDelete = (from a in entityPM.SupplierInvoiceItems
                                       where a.ChangeSetOp == ChangeSetOperation.Delete
                                       select a).Any();
            }


            if (isInvoiceItemDelete || isInvoiceItemInsert)
            {
                if (!isSubmitChanges)
                {
                    SubmitChanges();
                    isSubmitChanges = true;
                }

                bool isAmendOrConverted = this._DeclarationPM?.IsAmendment == true || this._DeclarationPM?.IsConvertedDeclaration == true;

                if (!isAmendOrConverted)
                {
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {


                        CustomsStoredProcedures.UpdateSupplierInvoiceItemsSequenceOracle
                            (entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.Tenant);

                        CustomsStoredProcedures.UpdateParentSupplierInvoiceItemsSequenceOracle
                            (entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.Tenant);
                    }
                    else
                    {
                        CustomsStoredProcedures.UpdateSupplierInvoiceItemsSequence(entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.Tenant);
                        CustomsStoredProcedures.UpdateParentSupplierInvoiceItemsSequence(entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.Tenant);
                    }
                }


                //ICustomContext context = MainContext as CustomContext;


                //SupplierInvoiceItemRepository invoiceItemRepository = new SupplierInvoiceItemRepository(context);
                //List<SupplierInvoiceItem> supplierInvoiceItems = invoiceItemRepository.GetMulti(new SupplierInvoiceKeys() { DeclarationId = entityPM.DeclarationId, InvoiceCounterKey = entityPM.InvoiceCounterKey });


                //bool dirty = false;
                //int index = 0;
                //int count = 0;
                //foreach (SupplierInvoiceItem item in supplierInvoiceItems)
                //{
                //    count++;
                //    index += 1;
                //    if (item.SequenceNumeric == index) continue;
                //    dirty = true;
                //    item.SequenceNumeric = index;
                //    invoiceItemRepository.Update(item);
                //    //SupplierInvoiceItemPM itemPM = (from a in entityPM.SupplierInvoiceItems
                //    //                                where a.DeclarationId == item.DeclarationId && a.CounterKey == item.CounterKey && a.LineNumber == item.LineNumber
                //    //                                select a).FirstOrDefault();
                //    //itemPM.SequenceNumeric = item.SequenceNumeric;
                //    if (count == 500)
                //    {
                //        count = 0;
                //        if (dirty)
                //        {
                //            invoiceItemRepository.SubmitChanges();
                //        }
                //    }
                //}
                //if (dirty)
                //{
                //    invoiceItemRepository.SubmitChanges();
                //}
            }
            ICustomContext context1 = MainContext as CustomContext;
            bool dirty1 = false;
            bool dirtyVehicle = false;
            SupplierInvioceItemCertificatRepository supplierInvioceItemCertificatRepository = new SupplierInvioceItemCertificatRepository(context1);
            SupplierInvoiceItemVehicleRepository supplierInvoiceItemVehicleRepository = new SupplierInvoiceItemVehicleRepository(context1);

            // invoice items
            try
            {
                foreach (SupplierInvoiceItemPM invoiceItem in entityPM.SupplierInvoiceItems.Where(d => d.ChangeSetOp == ChangeSetOperation.Update || d.ChangeSetOp == ChangeSetOperation.Insert))
                {

                    bool isSupplierInvioceItemCertificatInsert = (from a in invoiceItem.SupplierInvioceItemCertificats
                                                                  where a.ChangeSetOp == ChangeSetOperation.Insert
                                                                  select a).Any();

                    bool isSupplierInvioceItemCertificatDelete = (from a in invoiceItem.DeletedSupplierInvioceItemCertificats
                                                                  select a).Any();

                    if (!isSupplierInvioceItemCertificatDelete)
                    {

                        isSupplierInvioceItemCertificatDelete = (from a in invoiceItem.SupplierInvioceItemCertificats
                                                                 where a.ChangeSetOp == ChangeSetOperation.Delete
                                                                 select a).Any();

                    }
                    bool isSupplierInvioceItemVehicleInsert = (from a in invoiceItem.SupplierInvoiceItemVehicles
                                                               where a.ChangeSetOp == ChangeSetOperation.Insert
                                                               select a).Any();

                    bool isSupplierInvioceItemVehicleDelete = (from a in invoiceItem.DeletedSupplierInvoiceItemVehicles
                                                               select a).Any();

                    if (!isSupplierInvioceItemVehicleDelete)
                    {
                        isSupplierInvioceItemVehicleDelete = (from a in invoiceItem.SupplierInvoiceItemVehicles
                                                              where a.ChangeSetOp == ChangeSetOperation.Delete
                                                              select a).Any();

                    }

                    if (isSupplierInvioceItemCertificatDelete || isSupplierInvioceItemCertificatInsert)
                    {
                        if (!isSubmitChanges)
                        {
                            SubmitChanges();
                            isSubmitChanges = true;
                        }

                        bool isAmendOrConverted = declarationPM.IsAmendment == true || declarationPM.IsConvertedDeclaration == true;

                        if (!isAmendOrConverted)
                        {

                            List<SupplierInvioceItemCertificat> supplierInvioceItemCertificats = supplierInvioceItemCertificatRepository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = invoiceItem.DeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber });


                            int index = 0;
                            foreach (SupplierInvioceItemCertificat item in
                                //supplierInvioceItemCertificats)  // itzik :why not to sort it ???
                                supplierInvioceItemCertificats.OrderBy(rec => rec.ItemCertificateCounterKey))
                            {
                                index += 1;
                                if (item.SequenceNumeric == index) continue;
                                dirty1 = true;
                                item.SequenceNumeric = index;
                                supplierInvioceItemCertificatRepository.Update(item);
                                SupplierInvioceItemCertificatPM itemPM = (from a in invoiceItem.SupplierInvioceItemCertificats
                                                                          where a.DeclarationId == item.DeclarationId && a.InvoiceCounterKey == item.InvoiceCounterKey && a.LineNumber == item.LineNumber
                                                                          select a).FirstOrDefault();
                                if (itemPM != null) itemPM.SequenceNumeric = item.SequenceNumeric;
                            }
                        }
                    }

                    if (isSupplierInvioceItemVehicleDelete || isSupplierInvioceItemVehicleInsert)
                    {
                        if (!isSubmitChanges)
                        {
                            SubmitChanges();
                            isSubmitChanges = true;
                        }

                        List<SupplierInvoiceItemVehicle> supplierInvioceItemVehicles = supplierInvoiceItemVehicleRepository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = invoiceItem.DeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber });
                        int index = 0;
                        foreach (SupplierInvoiceItemVehicle item in supplierInvioceItemVehicles.OrderBy(rec => rec.SequenceNumeric))
                        {
                            index += 1;
                            if (item.SequenceNumeric == index) continue;
                            dirtyVehicle = true;
                            item.SequenceNumeric = index;
                            supplierInvoiceItemVehicleRepository.Update(item);
                            SupplierInvoiceItemVehiclePM itemPM = (from a in invoiceItem.SupplierInvoiceItemVehicles
                                                                   where a.DeclarationId == item.DeclarationId && a.InvoiceCounterKey == item.InvoiceCounterKey && a.LineNumber == item.LineNumber
                                                                   select a).FirstOrDefault();
                            if (itemPM != null) itemPM.SequenceNumeric = item.SequenceNumeric;
                        }
                    }
                }

                //<--- Yuval Chalup 04.12.2016 TASK-24655
                if (entityPM.GTBITEMsToUpdate != null && entityPM.GTBITEMsToUpdate.Count() > 0 && defaultDeclarationPM?.Direction!="E")
                {
                    OpenUnifreighTask(entityPM);
                }
                //Yuval Chalup 04.12.2016 TASK-24655 --->
                if (declarationPM.Direction != "E" || (declarationPM.Direction == "E" && entityPM.ChangeSetOp != ChangeSetOperation.Insert) )
                {
                    UpdateDeclarationFields(entityPM);
                }

            }
            finally
            {
                if (dirty1)
                {
                    supplierInvioceItemCertificatRepository.SubmitChanges();
                }
                if (dirtyVehicle)
                {
                    supplierInvoiceItemVehicleRepository.SubmitChanges();
                }
            }

            if (declarationPM != null && declarationPM.IsCourierDeclaration)
            {
                DeclarationPM myDBDeclarationPM = null;
                object AncestorEntityUpdateService = null;
                this.GetAncestorEntityUpdateService(out AncestorEntityUpdateService);
                var AncestorDeclarationUpdateService = AncestorEntityUpdateService as DeclarationUpdateService;
                if (AncestorDeclarationUpdateService == null)//AncestorDeclarationUpdateService.afterUpdate do CalculateDeclarationCourierStatus
                {
                    DeclarationQueryService cDeclarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                    DeclarationPM fullDeclarationPM = cDeclarationQueryService.GetSingle(entityPM.DeclarationId, true, false);
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(_Context, new Dictionary<string, IContext>(), entityPM.Tenant);
                    DeclarationCourierStatusPM newDeclarationCourierStatusPM = declarationCourierStatusUpdateService.CalculateDeclarationCourierStatus(fullDeclarationPM);
                    declarationCourierStatusUpdateService.Update(newDeclarationCourierStatusPM, true);
                }
                /*
                 * getSingle moved to CalculateDeclarationCourierStatus
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_Context);
                DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPM.DeclarationId, false, false);
                if (currentDeclarationCourierStatusPM == null)
                {
                    newDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    newDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                */

            }

            //  -------- Declaration Referant Data 
            UpdateReferantData(declarationPM?? defaultDeclarationPM);
            
        }


        private void UpdateReferantData(DeclarationPM entityPM)
        {

            DeclarationReferantDataQueryService declarationReferantDataQueryService = new DeclarationReferantDataQueryService(entityPM.Tenant);
            DeclarationReferantDataPM referant = declarationReferantDataQueryService.GetSingle(entityPM.Id, false, false);
            if (referant != null)
            {
                if (referant.NewFile != false)
                {
                    if (HttpContextUtil.IsCustomDomainService())
                    {
                        referant.NewFile = false;
                        referant.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
                if (referant.ChangeSetOp == ChangeSetOperation.Update)
                {
                    ICustomContext context = MainContext as CustomContext;
                    DeclarationReferantDataUpdateService service = new DeclarationReferantDataUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                    service.Update(referant, true);
                }
            }
        }

        private void UpdateDeclarationFields(SupplierInvoicePM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var mySupplierInvoiceQueryService = new SupplierInvoiceQueryService(context);
            var supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(context);
            var supplierInvoiceModificationQueryService = new SupplierInvoiceModificationQueryService(context);
            var supplierInvoiceItemssQueryService = new SupplierInvoiceItemQueryService(context);

            //var unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(entityPM.Tenant);
            string unifreightUser = null;
            if (RequestSheetContext.Current != null)
            {
                var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                {
                    UserRepository userRep = new UserRepository(Tenant);
                    User user = userRep.GetSingleUser(loggingUserIdFromRS, entityPM.Tenant, true);
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
                unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(entityPM.Tenant);
            }
            var supplierInvoiceKeys = new SupplierInvoiceKeys();

            //Get Declaration ONLY
            DeclarationPM myDeclarationPM = myDeclarationQueryService.GetSingleDeclarationById(entityPM.DeclarationId, entityPM.Tenant);
            if (myDeclarationPM == null || string.IsNullOrWhiteSpace(myDeclarationPM.CustomFileNo))
            {
                return;
            }

            //Get Declaration's Supplier Invoices ONLY
            myDeclarationPM.SupplierInvoices = mySupplierInvoiceQueryService.GetSupplierInvoicesForDeclaration(entityPM.DeclarationId, entityPM.Tenant, false);
            if (myDeclarationPM.SupplierInvoices == null || myDeclarationPM.SupplierInvoices.Count() == 0)
            {
                return;
            }

            //For each Supplier Invoice - Get FreightAmount and Modification ONLY
            foreach (var supplierInvoicePM in myDeclarationPM.SupplierInvoices)
            {
                supplierInvoiceKeys.DeclarationId = supplierInvoicePM.DeclarationId;
                supplierInvoiceKeys.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;
                supplierInvoicePM.SupplierInvoiceFreightAmounts = supplierInvoiceFreightAmountQueryService.GetMulti(supplierInvoiceKeys, false);
                supplierInvoicePM.SupplierInvoiceModifications = supplierInvoiceModificationQueryService.GetMulti(supplierInvoiceKeys, false);
                supplierInvoicePM.SupplierInvoiceItems = supplierInvoiceItemssQueryService.GetMulti(supplierInvoiceKeys, false);

            }
            if (this.Multi_LastSIWillUpdateCCU)
            {
                if (myDeclarationPM.SupplierInvoices.LastOrDefault().SequenceNumeric != entityPM.SequenceNumeric)
                {
                    LogMessagingUtil.Instance.AppendLine($"Not last SupplierInvoice{entityPM.SequenceNumeric} Suppress UnifrightDeclarationUpdateService");
                    return;
                }
                LogMessagingUtil.Instance.AppendLine($"Last!!!! SupplierInvoice{entityPM.SequenceNumeric} Call UnifrightDeclarationUpdateService");
            }
            bool dotask = false;
            if (!this.Multi_LastSIWillUpdateCCU && this.openTaskForUnifreight)
            {
                dotask = true;
                myDeclarationPM.TotalInvoiceAmountInUSD = 0;
                myDeclarationPM.TotalInvoiceAmountInUSD = myDeclarationPM.SupplierInvoices.Sum(r => r.InvoiceAmountInUSD);
                this.openTaskForUnifreight = false;
            }
            UnifrightDeclarationUpdateService UnifrightDeclarationUpdateService = new UnifrightDeclarationUpdateService(myDeclarationPM, null, unifreightUser);
            UnifrightDeclarationUpdateService._UpdateCCUFILEMFromSupplerInvoice = true;

            UnifrightDeclarationUpdateService.Update(dotask);
        }

        //<--- Yuval Chalup 04.12.2016 TASK-24655
        private void OpenUnifreighTask(SupplierInvoicePM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var requestData = "";
            //var unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(entityPM.Tenant);
            string unifreightUser = null;
            if (RequestSheetContext.Current != null)
            {
                var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                {
                    UserRepository userRep = new UserRepository(Tenant);
                    User user = userRep.GetSingleUser(loggingUserIdFromRS, entityPM.Tenant, true);
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
                unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(entityPM.Tenant);
            }

            DeclarationPM myDeclarationPM = myDeclarationQueryService.GetSingleDeclarationById(entityPM.DeclarationId, entityPM.Tenant);
            if (myDeclarationPM == null || string.IsNullOrWhiteSpace(myDeclarationPM.CustomFileNo))
            {
                return;
            }

            TransactionScope scope = null;
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
               
                bool isConnectedToUnifreight = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
             
                     _AmitalContext = AmitalContext.GetContext(entityPM.Tenant);
                    var myCCUQUELOCKQueryService = new Unifreight.BL.EntityQueryServices.CCUQUELOCKQueryService(_AmitalContext);
                    var myCCUQUELOCKUpdateService = new Unifreight.BL.EntityUpdateServices.CCUQUELOCKUpdateService(_AmitalContext);
                    var myGGGQUpdateService = new Unifreight.BL.EntityUpdateServices.GGGQUpdateService(_AmitalContext);

                    Unifreight.BL.EntityPMs.UGenerated.CCUQUELOCKPM myCCUQUELOCK = myCCUQUELOCKQueryService.GetSingle("CFIFILEM", myDeclarationPM.CustomFileNo, false);
                    if (myCCUQUELOCK == null)
                    {
                        var myCCUQUELOCKPM = new Unifreight.BL.EntityPMs.UGenerated.CCUQUELOCKPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            ENTNAME = "CFIFILEM",
                            FILENO = myDeclarationPM.CustomFileNo,
                        };

                        if (!isConnectedToUnifreight)
                        {
                            myCCUQUELOCK.Tenant = EntityPM.Tenant;
                        }

                        myCCUQUELOCKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                        myCCUQUELOCKUpdateService.Update(myCCUQUELOCKPM, true);
                    }
                
                    transmission mytransmission = GetTransmission(entityPM.GTBITEMsToUpdate, "AMITAL", "GTBITEMs from logitude");
                    var xmltransmission = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
                    requestData = xmltransmission;


                    var myGGGQPM = new Unifreight.BL.EntityPMs.GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", //LugitudeRequest
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = DateTime.Now,
                        TRY = 9,
                        PRIORITY = 8,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = myDeclarationPM.CustomFileNo,
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "D"
                    };

                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    if (!isConnectedToUnifreight)
                    {
                        myGGGQPM.Tenant = myDeclarationPM.Tenant;
                    }
                    myGGGQUpdateService.Update(myGGGQPM, true);
                

                    var myYCULTASKPM = new Unifreight.BL.EntityPMs.YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = myDeclarationPM.CustomFileNo,
                        PRIORITY = Unifreight.BL.EntityPMs.YCULTASKPM.calcPriority("L2U"),
                        TYPE = "LI2U",
                        USRCODE = unifreightUser,
                        ARCHIVE = "F"
                    };
                    if (!isConnectedToUnifreight)
                    {
                        myYCULTASKPM.Tenant = myDeclarationPM.Tenant;
                    }
                    var myYCULTASKUpdateService = new Unifreight.BL.EntityUpdateServices.YCULTASKUpdateService(_AmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);
                    
                

                if (scope != null)
                {
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                ex.ChangeExceptionMessage("SupplierInvoiceUpdateService Exception");
                throw;
            }
            finally
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
            }
        }

        transmission GetTransmission<T>(T mySerilazeObject, string from, string Subject)
    where T : class
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
        //Yuval Chalup 04.12.2016 TASK-24655 --->

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }

        private void CalculateFrieghtTotals(SupplierInvoicePM entityPM, DeclarationPM declarationPM)
        {
            if (entityPM.FreightCurrencyTypeCode != null && entityPM.TotalFreightInFreightCurrency != null && entityPM.TotalFreightInFreightCurrency > 0)
            {
                CustomsExchangeRateQueryService rateQuery = new CustomsExchangeRateQueryService(entityPM.Tenant);
                CustomsExchangeRatePM rate;
                rate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(entityPM.FreightCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
                if (rate != null)
                {
                    entityPM.TotalFreightInNIS = entityPM.TotalFreightInFreightCurrency * rate.ExchangeRate;
                }
            }
            //return;
            //decimal? totalFreightInNIS = 0;
            //CustomsExchangeRateQueryService rateQuery = new CustomsExchangeRateQueryService(entityPM.Tenant);

            //foreach (SupplierInvoiceFreightAmountPM item in entityPM.SupplierInvoiceFreightAmounts)
            //{
            //    if (item.ChangeSetOp != ChangeSetOperation.Delete)
            //    {
            //        decimal? amountInNIS;

            //        if (item.CurrencyTypeCode == "ILS")
            //        {
            //            amountInNIS = item.Amount;
            //            totalFreightInNIS += item.Amount;
            //        }
            //        else
            //        {
            //            CustomsExchangeRatePM rate;
            //            rate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(item.CurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //            if (rate != null)
            //            {
            //                amountInNIS = item.Amount * rate.ExchangeRate;
            //                totalFreightInNIS += amountInNIS;
            //            }
            //        }
            //    }
            //}


            //decimal? totalFreightInInvoice = 0;
            //if (entityPM.FreightCurrencyTypeCode == "ILS")
            //{
            //    totalFreightInInvoice = totalFreightInNIS;
            //}
            //else
            //{
            //    CustomsExchangeRatePM invoiceRate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(entityPM.FreightCurrencyTypeCode, declarationPM.TaxationDateTime, entityPM.Tenant);
            //    if (invoiceRate != null)
            //    {
            //        totalFreightInInvoice = totalFreightInNIS / invoiceRate.ExchangeRate;
            //    }
            //}


            //if (entityPM.TotalFreightInFreightCurrency != totalFreightInInvoice)
            //    entityPM.TotalFreightInFreightCurrency = totalFreightInInvoice;

            //if (entityPM.TotalFreightInNIS != totalFreightInNIS)
            //    entityPM.TotalFreightInNIS = totalFreightInNIS;

        }

        public static void RaiseStatus(DeclarationPM dirtyDeclarationPM, string loggingUserId, string statusId, string xmlStatus)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(loggingUserId)) loggingUserId = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = statusId,
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + statusId + " from Logitude",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = xmlStatus,
                        status_id = statusId,
                        status_DateTime = DateTime.Now,
                        //status_place = "",
                        //status_save = "no_fail",
                        comments = "",
                    }
                };
                if (!dirtyDeclarationPM.IsConnectedToUnifreight) myAmitalEventTracerModel.NotConnectedToUniface = true;

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent Status " + statusId + "  CustomFileNo = " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

      


      
    }
}

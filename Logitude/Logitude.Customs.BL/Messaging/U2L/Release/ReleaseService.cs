using Logitude.AmitalMessaging.Customs.CustomFile.ReleaseFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
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
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;

namespace Logitude.Customs.BL.Messaging.U2L.Release
{
    public class ReleaseService : UnifreightGenericService
    {
        private LOGIBONDREL _LOGIBONDREL;
        private LogitudeReleaseFile _LogitudeReleaseFile;
        private SupplierInvoicePM _MySupplierInvoicePM;
        //private EntityPMs.DeclarationPM _MyDeclarationPM;
        private ICustomContext _context;

        private AmitalContext amitalContext;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Release.ReleaseService.Upsert()";
        private Logitude.AmitalMessaging.Customs.CustomFile.ReleaseFile.INVOICE _INVOICE;
        private DeclarationPM _MyDeclarationPM;
        private DeclarationPM _MyEntryDeclarationPM;
        private Stopwatch _Stopwatch;
        private string mode;
        private string exemptTypesForEntitlement;
        private bool isExemptTypeInDefault;

        public ReleaseService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGIBONDREL,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "ReleaseService ";

            DeserilazeObject(xmlLOGIBONDREL);
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            if (!String.IsNullOrWhiteSpace(MoreParams))
            {
                AppendLogLine("MoreParams: " + MoreParams);
                var unifreightListsParams = UnifreightListsUtil.Deserialize(MoreParams);
                AppendLogLine("MoreParams after Deserialize: " + unifreightListsParams);
                mode = UnifreightListsUtil.GetValue(ref unifreightListsParams, "MODE");
                AppendLogLine("mode: " + mode);
            }

            //CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";
            _context = CustomContext.GetContext(ResolvedTenant());
            amitalContext = AmitalContext.GetContext(ResolvedTenant());
            var myQueryService = new DeclarationQueryService(_context);

            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            MyGenericResponseObj.Stage = "DeclarationUpsert";
            string xmlLOGICUSTFILE = xmlLOGIBONDREL;
            xmlLOGICUSTFILE = xmlLOGICUSTFILE.Replace("LOGIBONDREL", "LOGICUSTFILE");
            xmlLOGICUSTFILE = xmlLOGICUSTFILE.Replace("LogitudeReleaseFile", "LogitudeCustomsFile");
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Clear();
            DeclarationUpsertService myDeclarationUpsertService = new DeclarationUpsertService();
            try
            {
                myDeclarationUpsertService.ProccessGenericRequest(xmlLOGICUSTFILE, ref MoreParams, out MessageOut);
            }
            catch (DbEntityValidationException ex)
            {
                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                AppendLogLine("Declaration Upsert Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(2000));
                AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                return;
            }
            catch (Exception e)
            {
                AppendLogLine("Declaration Upsert Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(2000));
                AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                return;
            }
            //Delete Supplier Invoice
            AppendLogLine("Declaration Upsert Log " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(2000));

            if (String.IsNullOrWhiteSpace(_LogitudeReleaseFile.Id))
            {
                string existId = myQueryService.GetIdByCustomFileNo(_LogitudeReleaseFile.CustomFileNo, ResolvedTenant());
                _LogitudeReleaseFile.Id = existId;
            }

            MyGenericResponseObj.Stage = "GetSingle"; // - To delete";
            this._MyDeclarationPM = myQueryService.GetSingle(this._LogitudeReleaseFile.Id, true, false);
            if (this._MyDeclarationPM == null)
            {
                AppendLogLine("Declaration Get Single Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(2000));
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                throw new BusinessErrorException("LOGITUDE FILE is " + this._LogitudeReleaseFile.Id + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            exemptTypesForEntitlement = GetDefault("ISRAEL", "CGG_ENTI_EXEMPT", "NON", "NON", ResolvedTenant());
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            if (mode == "SecondaryEntry")
            {
                this._MyDeclarationPM.ProcedureCurrentCode = "7070001";
            }
            else
            {
                this._MyDeclarationPM.ProcedureCurrentCode = "4070001";
            }
            this._MyDeclarationPM.IsReleaseFile = true; // moran 14.2.16 - Task 19876
            this._MyDeclarationPM.TaxationDateTime = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeReleaseFile.TaxationDateTime, "LogitudeReleaseFile.TaxationDateTime"); // moran 22.1.17 - AMI-58777
            if (this._MyEntryDeclarationPM != null)
            {
                if (!string.IsNullOrWhiteSpace(this._MyEntryDeclarationPM.AutonomyRegionTypeCode)) this._MyDeclarationPM.AutonomyRegionTypeCode = this._MyEntryDeclarationPM.AutonomyRegionTypeCode;
            }
            if (!string.IsNullOrWhiteSpace(_LogitudeReleaseFile.EntryFile))
            {
                this._MyEntryDeclarationPM = myQueryService.GetSingle(this._LogitudeReleaseFile.EntryFile, true, false);
                if (this._MyEntryDeclarationPM != null)
                {
                    if (!string.IsNullOrWhiteSpace(this._MyEntryDeclarationPM.AutonomyRegionTypeCode)) this._MyDeclarationPM.AutonomyRegionTypeCode = this._MyEntryDeclarationPM.AutonomyRegionTypeCode;
                }
            }
            if (!string.IsNullOrWhiteSpace(this._LogitudeReleaseFile.TransferImporterId)) // moran 22.3.17 - AMI-59830
            {
                if (_LogitudeReleaseFile.TransferImporterId.Length > 9)
                {
                    this._MyDeclarationPM.EntitleImporterId = TranslateClient(_LogitudeReleaseFile.TransferImporterId.Substring(0, 9));
                    this._MyDeclarationPM.EntitleImporterCode = _LogitudeReleaseFile.TransferImporterId.Substring(0, 9);
                }
                else
                {
                    this._MyDeclarationPM.EntitleImporterId = TranslateClient(_LogitudeReleaseFile.TransferImporterId);
                    this._MyDeclarationPM.EntitleImporterCode = _LogitudeReleaseFile.TransferImporterId;
                }
                if (!string.IsNullOrWhiteSpace(this._LogitudeReleaseFile.ImporterEntitlementTypeCode))
                {
                    EntitlementTypeQueryService entitlementTypeQueryService = new EntitlementTypeQueryService(this._MyDeclarationPM.Tenant);
                    EntitlementTypePM entitlementType = entitlementTypeQueryService.GetSingle(this._LogitudeReleaseFile.ImporterEntitlementTypeCode, false, false);
                    if (entitlementType != null)
                    {
                        this._MyDeclarationPM.ImporterEntitlementTypeCode = entitlementType.Code;
                        this._MyDeclarationPM.ImporterEntitlementTypeName = entitlementType.LocalName;
                    }
                }
            }
            //            DeclarationUpdateService.MarkToDeleteSupplierInvoice(_MyDeclarationPM);
            //           AppendLogLine("MarkToDeleteSupplierInvoice:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
            //           DeclarationUpdateService.Update(this._MyDeclarationPM, true);


            if (this._MyDeclarationPM.Consignments.Count == 1) // moran 11.1.17 - AMI-59265 - moved before creating invoices
            {
                if (this._MyDeclarationPM.Consignments[0].ChangeSetOp != ChangeSetOperation.Insert)
                {
                    this._MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update;
                }

                if (this._MyDeclarationPM.TransportModeId == "A")
                {
                    //this._MyDeclarationPM.Consignments[0].SecondCargoID = _LogitudeReleaseFile.MAWB; // moran 24.3.16 - AMI-56039 - commented
                    //this._MyDeclarationPM.Consignments[0].ThirdCargoID = _LogitudeReleaseFile.HAWB; // moran 24.3.16 - AMI-56039 - commented
                }
                else
                {
                    //this._MyDeclarationPM.Consignments[0].SecondCargoID = _LogitudeReleaseFile.DealId; // moran 24.3.16 - AMI-56039 - commented
                }
                if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.ManifestNumber))
                {
                    this._MyDeclarationPM.Consignments[0].ManifestNumber = _LogitudeReleaseFile.ManifestNumber;
                }
                else
                {
                    if (this._MyEntryDeclarationPM != null) this._MyDeclarationPM.Consignments[0].ManifestNumber = this._MyEntryDeclarationPM.DeclarationNumber; // moran 24.3.16 - AMI-56039 - change to Declaration Number
                }
                //this._MyDeclarationPM.Consignments[0].LoadingPortCode = _LogitudeReleaseFile.LoadingPortCode;
                string loadingPortCode = null;
                if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.LoadingPortCode))
                {
                    loadingPortCode = TranslateloadPort(_LogitudeReleaseFile.LoadingPortCode);
                }
                this._MyDeclarationPM.Consignments[0].LoadingPortCode = loadingPortCode;

                if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.OriginCountryCode))
                {
                    string countryCode = "";
                    if (_LogitudeReleaseFile.OriginCountryCode.Length > 2)
                    {
                        countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", _LogitudeReleaseFile.OriginCountryCode);
                    }
                    else
                    {
                        countryCode = _LogitudeReleaseFile.OriginCountryCode;
                    }
                    if (!string.IsNullOrWhiteSpace(countryCode)) this._MyDeclarationPM.Consignments[0].OriginCountryCode = countryCode;
                }
                else
                {
                    this._MyDeclarationPM.Consignments[0].OriginCountryCode = null;
                }

                this._MyDeclarationPM.Consignments[0].CargoDescription = _LogitudeReleaseFile.CargoDescription;
                //this._MyDeclarationPM.Consignments[0].ManifestDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeReleaseFile.ManifestDate, "LogitudeReleaseFile.ManifestDate");
                this._MyDeclarationPM.Consignments[0].UnloadDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeReleaseFile.ArrivalDateTime, "LogitudeReleaseFile.ArrivalDateTime");

                this._MyDeclarationPM.Consignments[0].CargoTypeCode = "8";
                if (this._MyEntryDeclarationPM != null && this._MyEntryDeclarationPM.Consignments != null && this._MyEntryDeclarationPM.Consignments.Count > 0)
                {
                    //this._MyDeclarationPM.Consignments[0].ManifestNumber = this._MyEntryDeclarationPM.DeclarationNumber; //Consignments[0].ManifestNumber; // moran 3.3.16 - AMI-56039 - change to Declaration Number // moran 24.3.16 - AMI-56039 - commented
                    if (this._MyEntryDeclarationPM.Consignments[0].UnloadDate.HasValue) this._MyDeclarationPM.Consignments[0].UnloadDate = this._MyEntryDeclarationPM.Consignments[0].UnloadDate;
                    if (!string.IsNullOrWhiteSpace(this._MyEntryDeclarationPM.Consignments[0].UnloadPortCode)) this._MyDeclarationPM.Consignments[0].UnloadPortCode = this._MyEntryDeclarationPM.Consignments[0].UnloadPortCode;
                    if (!string.IsNullOrWhiteSpace(this._MyEntryDeclarationPM.Consignments[0].LoadingPortCode)) this._MyDeclarationPM.Consignments[0].LoadingPortCode = this._MyEntryDeclarationPM.Consignments[0].LoadingPortCode;
                }
                this._MyDeclarationPM.Consignments[0].IsLastReleaseFromWarehous = _LogitudeReleaseFile.ISLASTRELEASEFROMWAREHOUS;
                if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.OriginCountryId))
                {
                    //this._MyDeclarationPM.Consignments[0].OriginCountryCode = _LogitudeReleaseFile.OriginCountryId;
                    string countryCode = "";
                    if (_LogitudeReleaseFile.OriginCountryId.Length > 2)
                    {
                        countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", _LogitudeReleaseFile.OriginCountryId);
                    }
                    else
                    {
                        countryCode = _LogitudeReleaseFile.OriginCountryId;
                    }
                    if (!string.IsNullOrWhiteSpace(countryCode)) this._MyDeclarationPM.Consignments[0].OriginCountryCode = countryCode;
                }
                if (mode == "SecondaryEntry")
                {
                    this._MyDeclarationPM.Consignments[0].ReceiverWarehouseCode = TranslateReceiverWarehouse(_LogitudeReleaseFile.WarehouseId);
                    this._MyDeclarationPM.Consignments[0].StorageSiteCode = TranslateReceiverWarehouse(_LogitudeReleaseFile.StorageSiteCode);
                }
                else
                {
                    this._MyDeclarationPM.Consignments[0].StorageSiteCode = TranslateDeliverySite(_LogitudeReleaseFile.WarehouseId);
                }

                if (this._LogitudeReleaseFile.PACKAGES != null && this._LogitudeReleaseFile.PACKAGES.Count() > 0)
                {
                    var myConsignmentPackageUpdateService = new ConsignmentPackageUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());
                    DeleteConsignmentPackages(myConsignmentPackageUpdateService);
                    this._MyDeclarationPM.Consignments[0].ConsignmentPackages = GetConsignmentPackages(this._LogitudeReleaseFile.PACKAGES);
                }

            }
            else
            {
                MyGenericResponseObj.Message = "Declaration has multiple Consignments(" + this._MyDeclarationPM.Consignments.Count.ToString() + ") and Consignment details didn't update";
                AppendLogLine("Declaration has multiple Consignments(" + this._MyDeclarationPM.Consignments.Count.ToString() + ") and Consignment details didn't update");
            }


            if (this._LogitudeReleaseFile.INVOICE != null)
            {
                if (this._LogitudeReleaseFile.INVOICE.Count() > 0)
                {
                    //                   AppendLogLine("Update:MarkToDeleteSupplierInvoice:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                    this._MyDeclarationPM.SupplierInvoices = new List<SupplierInvoicePM>();
                    foreach (var itemINVOICE in this._LogitudeReleaseFile.INVOICE)
                    {
                        this._INVOICE = itemINVOICE;
                        MyCommunicationsParams.Tenant = ResolvedTenant();
                        MyGenericResponseObj.Stage = "Upsert " + this._INVOICE.INVOICENUMBER;
                        InvoiceInsert();

                        MyGenericResponseObj.Stage = "Done " + this._INVOICE.INVOICENUMBER;
                    }
                    AppendLogLine("InvoiceInsert:All:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    if (this._MyDeclarationPM.SupplierInvoices != null && this._MyDeclarationPM.SupplierInvoices.Count() > 0) // moran 8.10.15 - Task 16452
                    {
                        this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().IsPrimarySupplierInvoice = true;
                        //this._MyDeclarationPM.PrimaryInvoiceCounterKey = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().InvoiceCounterKey.ToString();
                    }
                    //DeclarationUpdateService.Update(this._MyDeclarationPM, true);
                    AppendLogLine("Update:InvoiceInsert:All:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    //MyGenericResponseObj.Stage = "Done All ";

                }
            }

            if (this._LogitudeReleaseFile.CustomsDocuments != null && this._LogitudeReleaseFile.CustomsDocuments.Where(d => d.Blocked != "1").Count() > 0) // moran 2.6.16 - AMI-56624
            {
                var myCustomsDocumentQueryService = new CustomsDocumentQueryService(dbContext);
                //var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(dbContext);
                var myCustomsDocumentPointerUpdateService = new CustomsDocumentPointerUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                //var myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(dbContext);
                var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);

                //CustomsDocumentPointerPM customsDocumentPointerPM = new CustomsDocumentPointerPM();
                //CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();


                foreach (var customsDocument in this._LogitudeReleaseFile.CustomsDocuments)
                {
                    if (customsDocument.Blocked != "1")
                    {
                        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(_MyDeclarationPM.Tenant);
                        DocumentsFilingPM documentIn = documentsFilingQuery.GetSinglePM(customsDocument.COM_ID, _MyDeclarationPM.Tenant);
                        if (documentIn != null)
                        {
                            CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();
                            customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Insert;
                            customsDocumentsTicketPM.Tenant = _MyDeclarationPM.Tenant;
                            customsDocumentsTicketPM.DocumentsFilingId = customsDocument.COM_ID;
                            customsDocumentsTicketPM.DocumentTypeCode = customsDocument.DocumentTypeCode;
                            myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);

                            CustomsDocumentPointerPM customsDocumentPointerPM = new CustomsDocumentPointerPM();
                            customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Insert;

                            // Create Document Pointer (every document pointer has a ticket)
                            customsDocumentPointerPM.Tenant = _MyDeclarationPM.Tenant;
                            customsDocumentPointerPM.CustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                            customsDocumentPointerPM.ParentEntityCode = "Declaration";
                            customsDocumentPointerPM.ParentEntityId = _MyDeclarationPM.Id;
                            if (customsDocument.Entname == "SI")
                            {
                                customsDocumentPointerPM.Child1EntityCode = "SupplierInvoice";
                                customsDocumentPointerPM.Child1EntityId = "1"; // customsDocument.Key_2;
                            }
                            if (customsDocument.Entname == "CI")
                            {

                                customsDocumentPointerPM.Child2EntityCode = "SupplierInvoiceItem";
                                customsDocumentPointerPM.Child2EntityId = "1"; //customsDocument.Key_3;
                            }
                            //customsDocumentPointerPM.DocumentStatusCode = "3";
                            customsDocumentPointerPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                            myCustomsDocumentPointerUpdateService.Update(customsDocumentPointerPM, true);

                            var myDocumentId = myCustomsDocumentQueryService.GetSingle(customsDocument.COM_ID, true, false);
                            if (myDocumentId == null)
                            {
                                var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                                CustomsDocumentPM customsDocumentPM = new CustomsDocumentPM();
                                customsDocumentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                                customsDocumentPM.DocumentsFilingId = customsDocument.COM_ID;
                                customsDocumentPM.DocumentTypeCode = customsDocument.DocumentTypeCode;
                                customsDocumentPM.CurrentCustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                                customsDocumentPM.Tenant = _MyDeclarationPM.Tenant;
                                foreach (CustomsDocumentMetaDataValuePM value in customsDocumentPM.CustomsDocumentMetaDataValues)
                                {
                                    value.ChangeSetOp = ChangeSetOperation.Insert;
                                }

                                myCustomsDocumentUpdateService.Update(customsDocumentPM, true);
                            }
                        }
                    }
                }
            }
            /*
            if (this._MyDeclarationPM.Consignments.Count == 1)
            {
                if (this._MyDeclarationPM.Consignments[0].ChangeSetOp != ChangeSetOperation.Insert)
                {
                    this._MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update;
                }

                if (this._MyDeclarationPM.TransportModeId == "A")
                {
                    //this._MyDeclarationPM.Consignments[0].SecondCargoID = _LogitudeReleaseFile.MAWB; // moran 24.3.16 - AMI-56039 - commented
                    //this._MyDeclarationPM.Consignments[0].ThirdCargoID = _LogitudeReleaseFile.HAWB; // moran 24.3.16 - AMI-56039 - commented
                }
                else
                {
                    //this._MyDeclarationPM.Consignments[0].SecondCargoID = _LogitudeReleaseFile.DealId; // moran 24.3.16 - AMI-56039 - commented
                }
                if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.ManifestNumber)) 
                {
                this._MyDeclarationPM.Consignments[0].ManifestNumber = _LogitudeReleaseFile.ManifestNumber;
                }
                else
                {
                    if (this._MyEntryDeclarationPM != null) this._MyDeclarationPM.Consignments[0].ManifestNumber = this._MyEntryDeclarationPM.DeclarationNumber; // moran 24.3.16 - AMI-56039 - change to Declaration Number
                }
                this._MyDeclarationPM.Consignments[0].LoadingPortCode = _LogitudeReleaseFile.LoadingPortCode;
                if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.OriginCountryCode))
                {
                    string countryCode = "";
                    if(_LogitudeReleaseFile.OriginCountryCode.Length > 2)
                    {
                        countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", _LogitudeReleaseFile.OriginCountryCode);
                    }
                    else
                    {
                        countryCode = _LogitudeReleaseFile.OriginCountryCode;
                    }
                    if (!string.IsNullOrWhiteSpace(countryCode)) this._MyDeclarationPM.Consignments[0].OriginCountryCode = countryCode;
                }
                else
                {
                    this._MyDeclarationPM.Consignments[0].OriginCountryCode = null;
                }

                this._MyDeclarationPM.Consignments[0].CargoDescription = _LogitudeReleaseFile.CargoDescription;
                this._MyDeclarationPM.Consignments[0].ManifestDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeReleaseFile.ManifestDate, "LogitudeReleaseFile.ManifestDate");
                this._MyDeclarationPM.Consignments[0].UnloadDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeReleaseFile.ArrivalDateTime, "LogitudeReleaseFile.ArrivalDateTime");

                this._MyDeclarationPM.Consignments[0].CargoTypeCode = "8";
                if (this._MyEntryDeclarationPM != null && this._MyEntryDeclarationPM.Consignments != null && this._MyEntryDeclarationPM.Consignments.Count > 0)
                {
                    //this._MyDeclarationPM.Consignments[0].ManifestNumber = this._MyEntryDeclarationPM.DeclarationNumber; //Consignments[0].ManifestNumber; // moran 3.3.16 - AMI-56039 - change to Declaration Number // moran 24.3.16 - AMI-56039 - commented
                    if (this._MyEntryDeclarationPM.Consignments[0].UnloadDate != null) this._MyDeclarationPM.Consignments[0].UnloadDate = this._MyEntryDeclarationPM.Consignments[0].UnloadDate;
                    if (this._MyEntryDeclarationPM.Consignments[0].UnloadPortCode != null) this._MyDeclarationPM.Consignments[0].UnloadPortCode = this._MyEntryDeclarationPM.Consignments[0].UnloadPortCode;
                    if (this._MyEntryDeclarationPM.Consignments[0].LoadingPortCode != null) this._MyDeclarationPM.Consignments[0].LoadingPortCode = this._MyEntryDeclarationPM.Consignments[0].LoadingPortCode;
                }
                this._MyDeclarationPM.Consignments[0].IsLastReleaseFromWarehous = _LogitudeReleaseFile.ISLASTRELEASEFROMWAREHOUS;
                if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.OriginCountryId))
                {
                    //this._MyDeclarationPM.Consignments[0].OriginCountryCode = _LogitudeReleaseFile.OriginCountryId;
                    string countryCode = "";
                    if (_LogitudeReleaseFile.OriginCountryId.Length > 2)
                    {
                        countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", _LogitudeReleaseFile.OriginCountryId);
                    }
                    else
                    {
                        countryCode = _LogitudeReleaseFile.OriginCountryId;
                    }
                    if (!string.IsNullOrWhiteSpace(countryCode)) this._MyDeclarationPM.Consignments[0].OriginCountryCode = countryCode;
                }
                this._MyDeclarationPM.Consignments[0].StorageSiteCode = TranslateDeliverySite(_LogitudeReleaseFile.WarehouseId);


                if (this._LogitudeReleaseFile.PACKAGES != null && this._LogitudeReleaseFile.PACKAGES.Count() > 0) 
                {
                    var myConsignmentPackageUpdateService = new ConsignmentPackageUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());
                    DeleteConsignmentPackages(myConsignmentPackageUpdateService);
                    this._MyDeclarationPM.Consignments[0].ConsignmentPackages = GetConsignmentPackages(this._LogitudeReleaseFile.PACKAGES);
                }
                
            }
            else 
            {
                MyGenericResponseObj.Message = "Declaration has multiple Consignments(" + this._MyDeclarationPM.Consignments.Count.ToString() + ") and Consignment details didn't update";
                AppendLogLine("Declaration has multiple Consignments(" + this._MyDeclarationPM.Consignments.Count.ToString() + ") and Consignment details didn't update");
            }
             */
            _MyDeclarationPM.CurrentContextTag = UpsertActionConst; // moran 28.7.16 - Task 22249
            declarationUpdateService.Update(this._MyDeclarationPM, true);
            AppendLogLine("declarationUpdat:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            // moran 3.3.16 - AMI-56069 - commented Send Declaration Request 
            string val = "";
            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AvoidCreateCustomsRequestSheet"]))
            {
                val = ConfigurationManager.AppSettings["AvoidCreateCustomsRequestSheet"].ToString();
            }
            if (val != "1")
            {
                if (this._MyDeclarationPM.Consignments != null && this._MyDeclarationPM.Consignments[0].ConsignmentPackages != null && this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageTypeCode == "VN") // moran 8.3.16 - AMI-55751 - if vehicle
                {
                    MyGenericResponseObj.Stage = "Send Declaration Request";
                    string user = _MyDeclarationPM.CreatedByUserId;
                    if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(_MyDeclarationPM.Tenant);

                    GenericRequestParams requestParams = new GenericRequestParams()
                    {
                        Tenant = _MyDeclarationPM.Tenant,
                        LoggingUserId = user,
                        AppicationId = _MyDeclarationPM.Id,
                        InterfaceTypeCode = "2750",
                        LoggingEntityId = _MyDeclarationPM.Id,
                        LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                        LoggingEntityReference = _MyDeclarationPM.DeclarationNumber,

                        RequestName = "Declaration Request",
                        ResponseName = "Declaration Response",
                        RequestVIA = SendRequestVIA.WebServiceBatch,
                    };

                    SBQMessageService.
                        CreateSheetSBQMessage<GenericRequestParams>(requestParams, false);

                    AppendLogLine("Declaration Request:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                }
            }
            else
            {
                AppendLogLine("Declaration Request will not be Sent due to permission issues");
            }
            MyGenericResponseObj.Stage = "Done All ";
            MyGenericResponseObj.ApplicationId = this._MyDeclarationPM.Id;
            MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;

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

        private string TranslateClient(string importerId)
        {
            if (String.IsNullOrWhiteSpace(importerId))
            {
                AppendLogLine("importerId is null");
                return null;
            }
            ClientQueryService clientQueryService = new ClientQueryService(ResolvedTenant());

            var clientId = clientQueryService.GetIdByCode(importerId, ResolvedTenant());

            if (clientId == null)
            {
                AppendLogLine("importerId = " + importerId + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("importerId = " + importerId + " Translated to " + clientId);
            return clientId;
        }



        private void DeleteConsignmentPackages(ConsignmentPackageUpdateService myConsignmentPackageUpdateService)
        {
            foreach (var consignmentPackage in this._MyDeclarationPM.Consignments[0].ConsignmentPackages)
            {
                consignmentPackage.ChangeSetOp = ChangeSetOperation.Delete;
                myConsignmentPackageUpdateService.Update(consignmentPackage, true);
            }
        }

        private string TranslateDeliverySite(string amitalDeliverySiteCode)
        {
            if (String.IsNullOrWhiteSpace(amitalDeliverySiteCode))
            {
                AppendLogLine("amitalDeliverySiteCode is null");
                return null;
            }
            var deliverySiteType = new DeliverySiteTypeRepository(ResolvedTenant());
            var myDeliverySite = deliverySiteType.GetSingle(amitalDeliverySiteCode);
            if (myDeliverySite == null)
            {
                AppendLogLine("amitalDeliverySiteCode = " + amitalDeliverySiteCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalDeliverySiteCode = " + amitalDeliverySiteCode + " Translated to " + myDeliverySite.Code);
            return myDeliverySite.Code;
        }


        private string TranslateReceiverWarehouse(string amitalReceiverWarehouseCode)
        {
            if (String.IsNullOrWhiteSpace(amitalReceiverWarehouseCode))
            {
                AppendLogLine("amitalReceiverWarehouseCode is null");
                return null;
            }
            var registeredWarehouseSiteType = new RegisteredWarehouseSiteTypeRepository(ResolvedTenant());
            var myRegisteredWarehouseSite = registeredWarehouseSiteType.GetSingle(amitalReceiverWarehouseCode);
            if (myRegisteredWarehouseSite == null)
            {
                AppendLogLine("amitalReceiverWarehouseCode = " + amitalReceiverWarehouseCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalReceiverWarehouseCode = " + amitalReceiverWarehouseCode + " Translated to " + myRegisteredWarehouseSite.Code);
            return myRegisteredWarehouseSite.Code;
        }

        private string TranslateMeasurmentUnit(string amitalMeasurmentUnitCode)
        {
            if (String.IsNullOrWhiteSpace(amitalMeasurmentUnitCode))
            {
                AppendLogLine("amitalDepartmentCode is null");
                return null;
            }
            var measurmentUnit = new MeasurmentUnitRepository(ResolvedTenant());
            var myMeasurmentUnit = measurmentUnit.GetSingle(amitalMeasurmentUnitCode);
            if (myMeasurmentUnit == null)
            {
                AppendLogLine("amitalMeasurmentUnitCode = " + amitalMeasurmentUnitCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalMeasurmentUnitCode = " + amitalMeasurmentUnitCode + " Translated to " + myMeasurmentUnit.Code);
            return myMeasurmentUnit.Code;
        }

        private string TranslatePackingType(string amitalPackingTypeCode)
        {
            if (String.IsNullOrWhiteSpace(amitalPackingTypeCode))
            {
                AppendLogLine("amitalDepartmentCode is null");
                return null;
            }
            var packingType = new PackingTypeRepository(ResolvedTenant());
            var myPackingType = packingType.GetSingle(amitalPackingTypeCode);
            string PackageTypeCode = "";
            if (myPackingType == null)
            {
                PackageTypeCode = GetTranslationL2P("IIGC", "CTBPACKTYPE", amitalPackingTypeCode);
            }
            else
            {
                PackageTypeCode = myPackingType.Code;
            }

            if (string.IsNullOrWhiteSpace(PackageTypeCode))
            {
                AppendLogLine("amitalPackingTypeCode = " + amitalPackingTypeCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalPackingTypeCode = " + amitalPackingTypeCode + " Translated to " + PackageTypeCode);
            return PackageTypeCode;
        }

        private string TranslateloadPort(string loadportId)
        {
            if (String.IsNullOrWhiteSpace(loadportId))
            {
                AppendLogLine("loadportId is null");
                return null;
            }
            string portId = null;
            InternationalSiteQueryService InternationalSiteQuery = new InternationalSiteQueryService(ResolvedTenant());
            InternationalSitePM InternationalSite = InternationalSiteQuery.GetSingle(loadportId, true, false);
            if (InternationalSite != null && !InternationalSite.Inactive)
            {
                portId = InternationalSite.Code;
            }
            else
            {
                AppendLogLine("loadportId = " + loadportId + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("loadportId = " + loadportId + " Translated to " + portId);
            return portId;
        }

        private List<ConsignmentPackagePM> GetConsignmentPackages(PACKAGES[] pACKAGES)
        {
            var ConsignmentPackagePMList = new List<ConsignmentPackagePM>();
            foreach (var package in pACKAGES)
            {
                int int1 = 0;
                decimal decimal1 = 0;
                var ConsignmentPackagePM = new ConsignmentPackagePM();

                ConsignmentPackagePM.DeclarationId = this._MyDeclarationPM.Id;
                ConsignmentPackagePM.PackageMeasureQualifierCode = "2";
                //ConsignmentPackagePM.PackageQuantity = package.PackQuantity;
                if (!string.IsNullOrWhiteSpace(package.PackQuantity) && package.PackQuantity != "0")
                {
                    if (int.TryParse(package.PackQuantity, out int1))
                    {
                        ConsignmentPackagePM.PackageQuantity = int1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing Pack Quantity (" + package.PackQuantity + ") into integer");
                    }
                }
                //ConsignmentPackagePM.GrossMassMeasure = package.PackWeight;
                if (!string.IsNullOrWhiteSpace(package.PackWeight) && package.PackWeight != "0")
                {
                    if (decimal.TryParse(package.PackWeight, out decimal1))
                    {
                        ConsignmentPackagePM.GrossMassMeasure = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing Pack Weight (" + package.PackWeight + ") into decimal");
                    }
                }
                ConsignmentPackagePM.PackageTypeCode = TranslatePackingType(package.PackTypeId);
                ConsignmentPackagePM.MarksNumbers = package.SignNum;

                ConsignmentPackagePM.Tenant = ResolvedTenant();
                ConsignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;

                ConsignmentPackagePMList.Add(ConsignmentPackagePM);
            }

            return ConsignmentPackagePMList;
        }


        void DeserilazeObject(string xmlLOGIBONDREL)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("ReleaseService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGIBONDREL))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGIBONDREL.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGIBONDREL.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGIBONDREL);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGIBONDREL = XmlGenericUtil<LOGIBONDREL>.DeSerializeObject(xmlLOGIBONDREL);

            if (_LOGIBONDREL.LogitudeReleaseFile == null || _LOGIBONDREL.LogitudeReleaseFile.Length != 1)
            {
                throw new BusinessErrorException("_LOGIBONDREL.Release.Length != 1");
            }
            this._LogitudeReleaseFile = _LOGIBONDREL.LogitudeReleaseFile[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudeReleaseFile.Id))
            {
                throw new BusinessErrorException("LOGITUDEFILE is missing");
            }
            AppendLogLine("LOGITUDE FILE = " + this._LogitudeReleaseFile.Id);
        }



        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGIBONDREL();
            var myAmitalRelease = new LogitudeReleaseFile();
            var myAmitalReleaseInvoice = new List<Logitude.AmitalMessaging.Customs.CustomFile.ReleaseFile.INVOICE>();
            var myAmitalReleaseInvoiceItem = new List<Logitude.AmitalMessaging.Customs.CustomFile.ReleaseFile.INVOICEITEMS>();

            myAmitalRelease.Id = "1-1";
            //myAmitalRelease.TENANT = "1";
            myAmitalReleaseInvoice[1].INVOICELINENO = "1";
            myAmitalReleaseInvoice[1].INVOICELINENO = "1";
            myAmitalReleaseInvoice[1].ACCOUNTTYPE = "380";
            myAmitalReleaseInvoice[1].INVOICENUMBER = "999";
            myAmitalReleaseInvoice[1].VENDORNUMBER = "2000475";
            myAmitalReleaseInvoice[1].CURRENCYCODE = "18";
            myAmitalReleaseInvoice[1].INVOICEAMOUNT = "2";
            myAmitalRelease.INVOICE = myAmitalReleaseInvoice.ToArray();

            myAmitalReleaseInvoiceItem[1].CLASSIFICATIONCODE = "260300009";
            myAmitalReleaseInvoiceItem[1].TRADEAGREEMENTCODE = "BGR";
            myAmitalReleaseInvoiceItem[1].QUANTITY = "1";
            myAmitalReleaseInvoiceItem[1].ITEMPRICE = "1";
            myAmitalReleaseInvoiceItem[1].ITEMORIGINCOUNTRY = "AD";
            myAmitalReleaseInvoiceItem[1].ITEMCODE = "DFDF";
            myAmitalRelease.INVOICE[1].INVOICEITEMS = myAmitalReleaseInvoiceItem.ToArray();

            amitalObjExample.LogitudeReleaseFile = new LogitudeReleaseFile[] { myAmitalRelease };

            xml = XmlGenericUtil<LOGIBONDREL>.SerializeObject(amitalObjExample);

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

        private void InvoiceInsert()
        {
            //CheckExist();

            var myQueryService = new CustomsVendorQueryService(this._context);

            int int1 = 0;
            decimal decimal1 = 0;

            AppendLogLine("Insert Invoice..");

            MyGenericResponseObj.Stage = "GetContext";

            this._MySupplierInvoicePM = new Def.EntityPMs.SupplierInvoicePM();
            this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;

            MyGenericResponseObj.Stage = "Mapping";

            if (this._MySupplierInvoicePM.SupplierInvoiceItems == null)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
            }
            if (this._MySupplierInvoicePM.SupplierInvoiceItems.Count == 0)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems.Add(new Def.EntityPMs.SupplierInvoiceItemPM() { ChangeSetOp = ChangeSetOperation.Insert });
            }

            this._MySupplierInvoicePM.DeclarationId = this._MyDeclarationPM.Id;
            if (!string.IsNullOrWhiteSpace(this._INVOICE.INVOICELINENO) && this._INVOICE.INVOICELINENO != "0")
            {
                if (int.TryParse(this._INVOICE.INVOICELINENO, out int1))
                {
                    this._MySupplierInvoicePM.SequenceNumeric = int1;
                    //this._MySupplierInvoicePM.InvoiceCounterKey = int1; // moran 8.10.15 - Task 16452 - commented - initiated automatically on creating
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing INVOICELINENO (" + this._INVOICE.INVOICELINENO + ") into integer");
                }
            }
            this._MySupplierInvoicePM.AccountTypeCode = this._INVOICE.ACCOUNTTYPE;
            this._MySupplierInvoicePM.InvoiceNumber = this._INVOICE.INVOICENUMBER;
            if (!string.IsNullOrWhiteSpace(this._INVOICE.VENDORNUMBER))
            {
                this._MySupplierInvoicePM.VendorId = myQueryService.GetIdByVendorNumber(this._INVOICE.VENDORNUMBER, ResolvedTenant());
                if (!string.IsNullOrWhiteSpace(this._MySupplierInvoicePM.VendorId))
                {
                    CustomsVendorPM customsVendorPM = myQueryService.GetSingle(this._MySupplierInvoicePM.VendorId, true, false);
                    if (customsVendorPM == null)
                    {
                        throw new BusinessErrorException("customsVendorPM '" + this._MySupplierInvoicePM.VendorId + "' is not found");
                    }
                    this._MySupplierInvoicePM.IssueCountryCode = customsVendorPM.CountryCode;
                }
            }
            this._MySupplierInvoicePM.InvoiceCurrencyTypeCode = this._INVOICE.CURRENCYCODE;
            if (!String.IsNullOrWhiteSpace(this._INVOICE.CURRENCYCODE))
            {


                var freightCurrency = new CurrencyTypeRepository(ResolvedTenant());
                var myfreightCurrency = freightCurrency.GetSingle(this._INVOICE.CURRENCYCODE);
                if (myfreightCurrency == null)
                {
                    string freightCurrencyCode = "";
                    freightCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", this._INVOICE.CURRENCYCODE);

                    if (!string.IsNullOrWhiteSpace(freightCurrencyCode)) this._MySupplierInvoicePM.InvoiceCurrencyTypeCode = freightCurrencyCode;
                }
                else
                {
                    this._MySupplierInvoicePM.InvoiceCurrencyTypeCode = myfreightCurrency.Code.ToString();
                }

            }

            if (!string.IsNullOrWhiteSpace(this._INVOICE.INVOICEAMOUNT) && this._INVOICE.INVOICEAMOUNT != "0")
            {
                if (decimal.TryParse(this._INVOICE.INVOICEAMOUNT, out decimal1))
                {
                    this._MySupplierInvoicePM.InvoiceAmount = decimal1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing INVOICEAMOUNT (" + this._INVOICE.INVOICEAMOUNT + ") into integer");
                }
            }
            if (!string.IsNullOrWhiteSpace(this._INVOICE.ISSUEDATE))
            {
                this._MySupplierInvoicePM.IssueDate = AmitalConvertUtil.GetUnifreightFormatedDate(this._INVOICE.ISSUEDATE, "INVOICE.ISSUEDATE");
            }
            if (string.IsNullOrWhiteSpace(this._MySupplierInvoicePM.IssueCountryCode) && !string.IsNullOrWhiteSpace(this._INVOICE.ISSUECOUNTRYCODE))
            {
                string countryCode = "";
                if (this._INVOICE.ISSUECOUNTRYCODE.Length > 2)
                {
                    countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", this._INVOICE.ISSUECOUNTRYCODE);
                }
                else
                {
                    countryCode = this._INVOICE.ISSUECOUNTRYCODE;
                }
                if (!string.IsNullOrWhiteSpace(countryCode)) this._MySupplierInvoicePM.IssueCountryCode = countryCode;
            }
            if (!string.IsNullOrWhiteSpace(this._INVOICE.INCOTERM_ID))
            {
                this._MySupplierInvoicePM.IncotermCode = this._INVOICE.INCOTERM_ID;
            }
            if (this._INVOICE.TRANSP_VALUE_LIST != null) // moran 7.8.17 - AMI-61197
            {
                this._MySupplierInvoicePM.SupplierInvoiceFreightAmounts = GetSupplierInvoiceFreightAmount(this._INVOICE.TRANSP_VALUE_LIST, this._MySupplierInvoicePM);
            }
            else if (!string.IsNullOrWhiteSpace(this._INVOICE.TRANSP_VALUE) && this._INVOICE.TRANSP_VALUE != "0")
            {
                if (decimal.TryParse(this._INVOICE.TRANSP_VALUE, out decimal1))
                {
                    this._MySupplierInvoicePM.TotalFreightInFreightCurrency = decimal1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing TRANSP_VALUE (" + this._INVOICE.TRANSP_VALUE + ") into integer");
                }
                if (!String.IsNullOrWhiteSpace(this._INVOICE.TRANSP_VALUE_CURR))
                {
                    var freightCurrency = new CurrencyTypeRepository(ResolvedTenant());
                    var myfreightCurrency = freightCurrency.GetSingle(this._INVOICE.TRANSP_VALUE_CURR);
                    if (myfreightCurrency == null)
                    {
                        string freightCurrencyCode = "";
                        freightCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", this._INVOICE.TRANSP_VALUE_CURR);

                        if (!string.IsNullOrWhiteSpace(freightCurrencyCode)) this._MySupplierInvoicePM.FreightCurrencyTypeCode = freightCurrencyCode;
                    }
                    else
                    {
                        this._MySupplierInvoicePM.FreightCurrencyTypeCode = myfreightCurrency.Code.ToString();
                    }
                }
                this._MySupplierInvoicePM.SupplierInvoiceFreightAmounts = GetSupplierInvoiceFreightAmountPM(this._MySupplierInvoicePM);
            }

            if (!string.IsNullOrWhiteSpace(this._INVOICE.INSURANCE_VALUE))
            {
                if (!string.IsNullOrWhiteSpace(this._INVOICE.INSURANCE_AMNT) && this._INVOICE.INSURANCE_AMNT != "0")
                {
                    if (decimal.TryParse(this._INVOICE.INSURANCE_AMNT, out decimal1))
                    {
                        this._MySupplierInvoicePM.InsuranceAmount = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing INSURANCE_AMNT (" + this._INVOICE.INSURANCE_AMNT + ") into integer");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(this._INVOICE.INSURANCE_PERCENT) && this._INVOICE.INSURANCE_PERCENT != "0")
                {
                    if (decimal.TryParse(this._INVOICE.INSURANCE_PERCENT, out decimal1))
                    {
                        this._MySupplierInvoicePM.InsruancePercentage = decimal1;
                        if (this._MySupplierInvoicePM.InsruancePercentage.HasValue)
                        {
                            CalculateInsuranceAmount(this._MySupplierInvoicePM.InsruancePercentage);
                        }
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing INSURANCE_PERCENT (" + this._INVOICE.INSURANCE_PERCENT + ") into integer");
                    }
                }
                if (!String.IsNullOrWhiteSpace(this._INVOICE.INSURANCE_CURR))
                {
                    var insuranceCurrency = new CurrencyTypeRepository(ResolvedTenant());
                    var myinsuranceCurrency = insuranceCurrency.GetSingle(this._INVOICE.INSURANCE_CURR);
                    if (myinsuranceCurrency == null)
                    {
                        string insuranceCurrencyCode = "";
                        insuranceCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", this._INVOICE.INSURANCE_CURR);

                        if (!string.IsNullOrWhiteSpace(insuranceCurrencyCode)) this._MySupplierInvoicePM.InsruanceCurrencyTypeCode = insuranceCurrencyCode;
                    }
                    else
                    {
                        this._MySupplierInvoicePM.InsruanceCurrencyTypeCode = myinsuranceCurrency.Code.ToString();
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(this._INVOICE.IS_PREFERENCE) && this._INVOICE.IS_PREFERENCE == "true" || this._INVOICE.IS_PREFERENCE == "1") this._MySupplierInvoicePM.IsPreference = true;
            if (!String.IsNullOrWhiteSpace(this._INVOICE.TRADE_AGREEMENT))
            {

                var tradeAgreement = new TradeAgreementRepository(ResolvedTenant());
                var myTradeAgreement = tradeAgreement.GetSingle(this._INVOICE.TRADE_AGREEMENT);
                if (myTradeAgreement == null)
                {
                    string TradeAgreementCode = "";
                    TradeAgreementCode = GetTranslationL2P("IIGC", "CTBTARIFF", this._INVOICE.TRADE_AGREEMENT);

                    if (!string.IsNullOrWhiteSpace(TradeAgreementCode)) this._MySupplierInvoicePM.PreferenceDocumentTypeCode = TradeAgreementCode;
                }
                else
                {
                    this._MySupplierInvoicePM.PreferenceDocumentTypeCode = myTradeAgreement.Code.ToString();
                }
            }

            this._MySupplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModificationsPM(this._INVOICE); // moran 26.5.16 - AMI-56711

            this._MySupplierInvoicePM.Tenant = ResolvedTenant();
            this._MySupplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItemPM(this._INVOICE);
            this._MyDeclarationPM.SupplierInvoices.Add(this._MySupplierInvoicePM);

            return;

        }

        private void CalculateInsuranceAmount(decimal? insruancePercentage)
        {
            if (insruancePercentage == null) return;
            decimal? value = null;
            if (this._MySupplierInvoicePM.InvoiceAmount.HasValue) value = this._MySupplierInvoicePM.InvoiceAmount;
            if (value == null)
            {
                value = this._MySupplierInvoicePM.TotalFreightInFreightCurrency;
            }
            else
            {
                if (this._MySupplierInvoicePM.TotalFreightInFreightCurrency.HasValue) value = value + this._MySupplierInvoicePM.TotalFreightInFreightCurrency;
            }
            value = value * (insruancePercentage / 100);
            if (value != null)
            {
                this._MySupplierInvoicePM.InsuranceAmount = Math.Round(value.Value, 2);
            }
            else
            {
                this._MySupplierInvoicePM.InsuranceAmount = value;
            }
            this._MySupplierInvoicePM.InsruanceCurrencyTypeCode = this._MySupplierInvoicePM.InvoiceCurrencyTypeCode;
        }

        private List<SupplierInvoiceFreightAmountPM> GetSupplierInvoiceFreightAmount(TRANSP_VALUE_LIST[] tRANSP_VALUE_LIST, SupplierInvoicePM supplierInvoicePM) // moran 7.8.17 - AMI-61197
        {
            var SupplierInvoiceFreightAmountPMList = new List<SupplierInvoiceFreightAmountPM>();
            var customsExchangeRates = new List<CustomsExchangeRatePM>();
            var CustomsExchangeRatequery = new CustomsExchangeRateQueryService(ResolvedTenant());

            foreach (var transpValItem in tRANSP_VALUE_LIST)
            {
                if (!string.IsNullOrWhiteSpace(transpValItem.TRANSP_VALUE_L) && transpValItem.TRANSP_VALUE_L != "0")
                {
                    var SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM();
                    decimal decimal1;
                    if (decimal.TryParse(transpValItem.TRANSP_VALUE_L, out decimal1))
                    {
                        SupplierInvoiceFreightAmountPM.Amount = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing TRANSP_VALUE_L (" + transpValItem.TRANSP_VALUE_L + ") into integer");
                    }
                    if (!String.IsNullOrWhiteSpace(transpValItem.TRANSP_VALUE_CURR_L))
                    {
                        var freightCurrency = new CurrencyTypeRepository(ResolvedTenant());
                        var myfreightCurrency = freightCurrency.GetSingle(transpValItem.TRANSP_VALUE_CURR_L);
                        if (myfreightCurrency == null)
                        {
                            string freightCurrencyCode = "";
                            freightCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", transpValItem.TRANSP_VALUE_CURR_L);

                            if (!string.IsNullOrWhiteSpace(freightCurrencyCode)) SupplierInvoiceFreightAmountPM.CurrencyTypeCode = freightCurrencyCode;
                        }
                        else
                        {
                            SupplierInvoiceFreightAmountPM.CurrencyTypeCode = myfreightCurrency.Code.ToString();
                        }
                        if (String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.FreightCurrencyTypeCode)) this._MySupplierInvoicePM.FreightCurrencyTypeCode = SupplierInvoiceFreightAmountPM.CurrencyTypeCode;

                    }
                    SupplierInvoiceFreightAmountPM.DeclarationId = supplierInvoicePM.DeclarationId;
                    if (supplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceFreightAmountPM.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;
                    SupplierInvoiceFreightAmountPM.Tenant = ResolvedTenant();
                    SupplierInvoiceFreightAmountPM.ChangeSetOp = ChangeSetOperation.Insert;

                    SupplierInvoiceFreightAmountPMList.Add(SupplierInvoiceFreightAmountPM);

                    decimal? amountInNIS = 0;

                    if (!String.IsNullOrWhiteSpace(SupplierInvoiceFreightAmountPM.CurrencyTypeCode) && SupplierInvoiceFreightAmountPM.CurrencyTypeCode != "ILS")
                    {
                        customsExchangeRates = CustomsExchangeRatequery.GetExchangeRateByCurrencyAndDate(SupplierInvoiceFreightAmountPM.CurrencyTypeCode, System.DateTime.Now, ResolvedTenant());
                        CustomsExchangeRatePM rate = customsExchangeRates.Where(d => d.CurrencyTypeCode == SupplierInvoiceFreightAmountPM.CurrencyTypeCode).FirstOrDefault();
                        if (rate != null)
                        {
                            amountInNIS = SupplierInvoiceFreightAmountPM.Amount * rate.ExchangeRate;
                        }
                    }
                    else
                    {
                        amountInNIS = SupplierInvoiceFreightAmountPM.Amount;
                    }
                    if (this._MySupplierInvoicePM.TotalFreightInNIS == null && amountInNIS > 0) this._MySupplierInvoicePM.TotalFreightInNIS = 0;
                    this._MySupplierInvoicePM.TotalFreightInNIS = this._MySupplierInvoicePM.TotalFreightInNIS + amountInNIS;
                }
            }
            if (this._MySupplierInvoicePM.TotalFreightInNIS.HasValue) this._MySupplierInvoicePM.TotalFreightInNIS = Math.Round(this._MySupplierInvoicePM.TotalFreightInNIS.Value, 2);

            if (this._MySupplierInvoicePM.FreightCurrencyTypeCode == "ILS")
            {
                this._MySupplierInvoicePM.TotalFreightInFreightCurrency = this._MySupplierInvoicePM.TotalFreightInNIS;
            }
            else if (!String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.FreightCurrencyTypeCode) && this._MySupplierInvoicePM.TotalFreightInNIS.HasValue)
            {
                customsExchangeRates = CustomsExchangeRatequery.GetExchangeRateByCurrencyAndDate(this._MySupplierInvoicePM.FreightCurrencyTypeCode, System.DateTime.Now, ResolvedTenant());
                CustomsExchangeRatePM freightRate = customsExchangeRates.Where(d => d.CurrencyTypeCode == this._MySupplierInvoicePM.FreightCurrencyTypeCode).FirstOrDefault();
                if (freightRate != null)
                {
                    if (this._MySupplierInvoicePM.TotalFreightInFreightCurrency == null) this._MySupplierInvoicePM.TotalFreightInFreightCurrency = 0;
                    this._MySupplierInvoicePM.TotalFreightInFreightCurrency = this._MySupplierInvoicePM.TotalFreightInNIS / freightRate.ExchangeRate;
                }
            }
            if (this._MySupplierInvoicePM.TotalFreightInFreightCurrency.HasValue) this._MySupplierInvoicePM.TotalFreightInFreightCurrency = Math.Round(this._MySupplierInvoicePM.TotalFreightInFreightCurrency.Value, 2);

            return SupplierInvoiceFreightAmountPMList;
        }

        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModificationsPM(INVOICE iNVOICE) // moran 26.5.16 - AMI-56711
        {
            var SupplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>();
            decimal decimal1 = 0;

            //if (this._MyDeclarationPM.SupplierInvoices.Count() < 1)
            {

                if (iNVOICE.EXPENSES != null)
                {

                    foreach (var expense in iNVOICE.EXPENSES)
                    {
                        var SupplierInvoiceModificationPM = new SupplierInvoiceModificationPM();
                        if (decimal.TryParse(expense.Amount, out decimal1))
                        {
                            SupplierInvoiceModificationPM.Amount = decimal1;
                        }
                        else
                        {
                            throw new BusinessErrorException("Error in parsing expense.Amount (" + expense.Amount + ") into decimal");
                        }
                        if (!String.IsNullOrWhiteSpace(expense.CurrencyTypeCode))
                        {
                            var expenseCurrency = new CurrencyTypeRepository(resolvedTenant());
                            var myexpenseCurrency = expenseCurrency.GetSingle(expense.CurrencyTypeCode);
                            if (myexpenseCurrency == null)
                            {
                                string expenseCurrencyCode = "";
                                expenseCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", this._INVOICE.TRANSP_VALUE_CURR);

                                if (!string.IsNullOrWhiteSpace(expenseCurrencyCode)) SupplierInvoiceModificationPM.CurrencyTypeCode = expenseCurrencyCode;
                            }
                            else
                            {
                                SupplierInvoiceModificationPM.CurrencyTypeCode = myexpenseCurrency.Code.ToString();
                            }
                        }
                        if (!String.IsNullOrWhiteSpace(expense.TypeCode))
                        {
                            var modificationAndDiscountType = new ModificationAndDiscountTypeRepository(resolvedTenant());
                            var mymodificationAndDiscountType = modificationAndDiscountType.GetSingle(expense.TypeCode);
                            if (mymodificationAndDiscountType != null && !String.IsNullOrWhiteSpace(mymodificationAndDiscountType.Code))
                            {
                                SupplierInvoiceModificationPM.TypeCode = mymodificationAndDiscountType.Code;
                            }
                        }
                        SupplierInvoiceModificationPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                        if (this._MySupplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceModificationPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                        SupplierInvoiceModificationPM.Tenant = (this._MyDeclarationPM.Tenant > 0) ? this._MyDeclarationPM.Tenant : ResolvedTenant();
                        SupplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Insert;

                        SupplierInvoiceModificationPMList.Add(SupplierInvoiceModificationPM);
                    }
                }
                if (SupplierInvoiceModificationPMList == null || SupplierInvoiceModificationPMList.Count() < 1 || SupplierInvoiceModificationPMList.Where(r => r.TypeCode == "160").FirstOrDefault() == null)
                {
                    if (!string.IsNullOrWhiteSpace(_LogitudeReleaseFile.agent_fee) && _LogitudeReleaseFile.agent_fee != "0")
                    {
                        var SupplierInvoiceModificationPM = new SupplierInvoiceModificationPM();
                        if (decimal.TryParse(_LogitudeReleaseFile.agent_fee, out decimal1))
                        {
                            SupplierInvoiceModificationPM.Amount = decimal1;
                        }
                        else
                        {
                            throw new BusinessErrorException("Error in parsing agent_fee (" + _LogitudeReleaseFile.agent_fee + ") into integer");
                        }
                        if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.agent_fee_currency))
                        {
                            var agentFeeCurrency = new CurrencyTypeRepository(ResolvedTenant());
                            var myagentFeeCurrency = agentFeeCurrency.GetSingle(_LogitudeReleaseFile.agent_fee_currency);
                            if (myagentFeeCurrency == null)
                            {
                                string agentFeeCurrencyCode = "";
                                agentFeeCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", this._INVOICE.TRANSP_VALUE_CURR);
                                if (!string.IsNullOrWhiteSpace(agentFeeCurrencyCode)) SupplierInvoiceModificationPM.CurrencyTypeCode = agentFeeCurrencyCode;
                            }
                            else
                            {
                                SupplierInvoiceModificationPM.CurrencyTypeCode = myagentFeeCurrency.Code.ToString();
                            }
                        }
                        SupplierInvoiceModificationPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                        if (this._MySupplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceModificationPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                        SupplierInvoiceModificationPM.TypeCode = "160";
                        SupplierInvoiceModificationPM.Tenant = (this._MyDeclarationPM.Tenant > 0) ? this._MyDeclarationPM.Tenant : ResolvedTenant();
                        SupplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Insert;

                        SupplierInvoiceModificationPMList.Add(SupplierInvoiceModificationPM);
                    }
                }

                return SupplierInvoiceModificationPMList;
            }
        }

        private int resolvedTenant()
        {
            if (this._MyDeclarationPM.Tenant > 0) return this._MyDeclarationPM.Tenant;
            return ResolvedTenant();
        }

        private List<SupplierInvoiceFreightAmountPM> GetSupplierInvoiceFreightAmountPM(SupplierInvoicePM supplierInvoicePM) // moran 11.4.16 - AMI-56512
        {
            var SupplierInvoiceFreightAmountPMList = new List<SupplierInvoiceFreightAmountPM>();
            var SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM();

            SupplierInvoiceFreightAmountPM.DeclarationId = supplierInvoicePM.DeclarationId;
            if (supplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceFreightAmountPM.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;

            SupplierInvoiceFreightAmountPM.Amount = supplierInvoicePM.TotalFreightInFreightCurrency;
            SupplierInvoiceFreightAmountPM.CurrencyTypeCode = supplierInvoicePM.FreightCurrencyTypeCode;

            SupplierInvoiceFreightAmountPM.Tenant = ResolvedTenant();
            SupplierInvoiceFreightAmountPM.ChangeSetOp = ChangeSetOperation.Insert;

            SupplierInvoiceFreightAmountPMList.Add(SupplierInvoiceFreightAmountPM);

            return SupplierInvoiceFreightAmountPMList;
        }



        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItemPM(Logitude.AmitalMessaging.Customs.CustomFile.ReleaseFile.INVOICE invoice)
        {
            var SupplierInvoiceItemPMList = new List<SupplierInvoiceItemPM>();
            foreach (var invoiceItem in invoice.INVOICEITEMS)
            {
                int int1 = 0;
                decimal decimal1 = 0;
                var SupplierInvoiceItemPM = new SupplierInvoiceItemPM();

                SupplierInvoiceItemPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                if (this._MySupplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceItemPM.CounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                if (!string.IsNullOrWhiteSpace(invoiceItem.ITEMLINENO) && invoiceItem.ITEMLINENO != "0")
                {
                    if (int.TryParse(invoiceItem.ITEMLINENO, out int1))
                    {
                        SupplierInvoiceItemPM.SequenceNumeric = int1;
                        SupplierInvoiceItemPM.LineNumber = int1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing ITEMLINENO (" + invoiceItem.ITEMLINENO + ") into integer");
                    }
                }
                SupplierInvoiceItemPM.ClassificationCode = invoiceItem.CLASSIFICATIONCODE;
                // moran 18.3.15 - Task 11540 -->
                //SupplierInvoiceItemPM.TradeAgreementCode = invoiceItem.TRADEAGREEMENTCODE;
                if (!String.IsNullOrWhiteSpace(invoiceItem.TRADEAGREEMENTCODE))
                {

                    var tradeAgreement = new TradeAgreementRepository(ResolvedTenant());
                    var myTradeAgreement = tradeAgreement.GetSingle(invoiceItem.TRADEAGREEMENTCODE);
                    if (myTradeAgreement == null)
                    {
                        string TradeAgreementCode = "";
                        TradeAgreementCode = GetTranslationL2P("IIGC", "CTBTARIFF", invoiceItem.TRADEAGREEMENTCODE);

                        if (!string.IsNullOrWhiteSpace(TradeAgreementCode)) SupplierInvoiceItemPM.TradeAgreementCode = TradeAgreementCode;
                    }
                    else
                    {
                        SupplierInvoiceItemPM.TradeAgreementCode = myTradeAgreement.Code.ToString();
                    }
                }

                // moran 18.3.15 - Task 11540 <--
                // moran 2.5.16 - AMI-56720  - change QUANTITY to QUANTITY_STS and UNIT to QUANTITY_TYPE -->
                if (!string.IsNullOrWhiteSpace(invoiceItem.QUANTITY_STS) && invoiceItem.QUANTITY_STS != "0")
                {
                    if (decimal.TryParse(invoiceItem.QUANTITY_STS, out decimal1))
                    {
                        SupplierInvoiceItemPM.InvoiceQuantity = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing QUANTITY (" + invoiceItem.QUANTITY_STS + ") into decimal");
                    }
                }
                SupplierInvoiceItemPM.InvoiceQuantityType = TranslateMeasurmentUnit(invoiceItem.QUANTITY_TYPE);
                // moran 2.5.16 - AMI-56720  - change QUANTITY to QUANTITY_STS and UNIT to QUANTITY_TYPE <--
                // moran 2.5.16 - AMI-56720  - commented -->
                /*
                if (!string.IsNullOrWhiteSpace(invoiceItem.QUANTITY_STS) && invoiceItem.QUANTITY_STS != "0")
                {
                    if (decimal.TryParse(invoiceItem.QUANTITY_STS, out decimal1))
                    {
                        SupplierInvoiceItemPM.StatisticQuantity = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing Statistic QUANTITY (" + invoiceItem.QUANTITY_STS + ") into decimal");
                    }
                }
                SupplierInvoiceItemPM.StatisticQuantityType = TranslateMeasurmentUnit(invoiceItem.QUANTITY_TYPE);
                 * */
                // moran 2.5.16 - AMI-56720  - commented <--
                if (!string.IsNullOrWhiteSpace(invoiceItem.ITEMPRICE) && invoiceItem.ITEMPRICE != "0")
                {
                    if (decimal.TryParse(invoiceItem.ITEMPRICE, out decimal1))
                    {
                        SupplierInvoiceItemPM.ItemPrice = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing ITEMPRICE (" + invoiceItem.ITEMPRICE + ") into decimal");
                    }
                }
                if (!string.IsNullOrWhiteSpace(invoiceItem.ITEMORIGINCOUNTRY))
                {
                    //SupplierInvoiceItemPM.OriginCountryCode = invoiceItem.ITEMORIGINCOUNTRY;
                    string countryCode = "";
                    if (invoiceItem.ITEMORIGINCOUNTRY.Length > 2)
                    {
                        countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", invoiceItem.ITEMORIGINCOUNTRY);
                    }
                    else
                    {
                        countryCode = invoiceItem.ITEMORIGINCOUNTRY;
                    }
                    if (!string.IsNullOrWhiteSpace(countryCode)) SupplierInvoiceItemPM.OriginCountryCode = countryCode;
                }
                SupplierInvoiceItemPM.PreferenceDocumentNumber = invoiceItem.PREFERENCEDOCUMENTNUMBER;

                if (!string.IsNullOrWhiteSpace(invoiceItem.COMMERCE_PRICE) && invoiceItem.COMMERCE_PRICE != "0")
                {
                    if (decimal.TryParse(invoiceItem.COMMERCE_PRICE, out decimal1))
                    {
                        SupplierInvoiceItemPM.WholeSaleItemPrice = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing COMMERCE PRICE (" + invoiceItem.COMMERCE_PRICE + ") into decimal");
                    }

                    if (String.IsNullOrWhiteSpace(invoiceItem.COMMERCE_PRICE_CURRENCY)) invoiceItem.COMMERCE_PRICE_CURRENCY = "ILS";
                    if (!String.IsNullOrWhiteSpace(invoiceItem.COMMERCE_PRICE_CURRENCY))
                    {
                        var wholesaleCurrency = new CurrencyTypeRepository(ResolvedTenant());
                        var mywholesaleCurrency = wholesaleCurrency.GetSingle(invoiceItem.COMMERCE_PRICE_CURRENCY);
                        if (mywholesaleCurrency == null)
                        {
                            string wholesaleCurrencyCode = "";
                            wholesaleCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", invoiceItem.COMMERCE_PRICE_CURRENCY);

                            if (!string.IsNullOrWhiteSpace(wholesaleCurrencyCode)) SupplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode = wholesaleCurrencyCode;
                        }
                        else
                        {
                            SupplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode = mywholesaleCurrency.Code.ToString();
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(invoiceItem.ADDITIONAL_QUANTITY) && invoiceItem.ADDITIONAL_QUANTITY != "0")
                {
                    if (decimal.TryParse(invoiceItem.ADDITIONAL_QUANTITY, out decimal1))
                    {
                        SupplierInvoiceItemPM.AdditionalQuantity = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing ADDITIONAL QUANTITY (" + invoiceItem.ADDITIONAL_QUANTITY + ") into decimal");
                    }
                }
                SupplierInvoiceItemPM.AdditionalQuantityType = TranslateMeasurmentUnit(invoiceItem.ADDITIONAL_QUANTITY_TYPE);
                if (!string.IsNullOrWhiteSpace(invoiceItem.STATISTICAL_QUANTITY) && invoiceItem.STATISTICAL_QUANTITY != "0")
                {
                    if (decimal.TryParse(invoiceItem.STATISTICAL_QUANTITY, out decimal1))
                    {
                        SupplierInvoiceItemPM.StatisticQuantity = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing STATISTICAL QUANTITY (" + invoiceItem.STATISTICAL_QUANTITY + ") into decimal");
                    }
                }
                SupplierInvoiceItemPM.StatisticQuantityType = TranslateMeasurmentUnit(invoiceItem.STATISTICAL_QUANTITY_TYPE);
                if(SupplierInvoiceItemPM.WholeSaleItemPrice.HasValue || SupplierInvoiceItemPM.AdditionalQuantity.HasValue || SupplierInvoiceItemPM.StatisticQuantity.HasValue)
                {
                    SupplierInvoiceItemPM.ItemAdditionalStatus = true;
                }

                if (invoiceItem.CERTIFICATES != null && invoiceItem.CERTIFICATES.Count() > 0)
                {
                    SupplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvoiceItemCertificatePM(invoiceItem, SupplierInvoiceItemPM);
                }
                if (invoiceItem.POINTERS != null && invoiceItem.POINTERS.Count() > 0)
                {
                    SupplierInvoiceItemPM.SupplierInvoiceItemsConDeclars = GetSupplierInvoiceItemConDeclarsPM(invoiceItem, SupplierInvoiceItemPM);
                }
                if (invoiceItem.CARS != null && invoiceItem.CARS.Count() > 0) // moran 14.3.16 - AMI-55746
                {
                    SupplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemVehiclesPM(invoiceItem, SupplierInvoiceItemPM);
                    SupplierInvoiceItemPM.VehicleStatus = true;
                }
                SupplierInvoiceItemPM.ItemCode = invoiceItem.ITEMCODE;
                SupplierInvoiceItemPM.Tenant = ResolvedTenant();
                SupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                if(isExemptTypeInDefault)
                {
                    SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesType = new SupplierInvoiceItemProcesTypePM()
                    {
                        DeclarationId = SupplierInvoiceItemPM.DeclarationId,
                        InvoiceItemLineNumber = SupplierInvoiceItemPM.LineNumber,
                        ProcessTypeCode = "4100105",
                        InvoiceCounterKey = SupplierInvoiceItemPM.CounterKey,
                        Tenant = SupplierInvoiceItemPM.Tenant,
                        ChangeSetOp = ChangeSetOperation.Insert,
                    };
                    SupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesType);
                    isExemptTypeInDefault = false;
                }
                
                if (this._MyDeclarationPM.Consignments != null && this._MyDeclarationPM.Consignments[0].ConsignmentPackages != null && this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageTypeCode == "VN") // moran 11.4.16 - AMI-56512 - if vehicle
                {
                    SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesType = new SupplierInvoiceItemProcesTypePM()
                    {
                        DeclarationId = SupplierInvoiceItemPM.DeclarationId,
                        InvoiceItemLineNumber = SupplierInvoiceItemPM.LineNumber,
                        ProcessTypeCode = "4100103",
                        InvoiceCounterKey = SupplierInvoiceItemPM.CounterKey,
                        Tenant = SupplierInvoiceItemPM.Tenant,
                        ChangeSetOp = ChangeSetOperation.Insert,
                    };
                    SupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesType);
                }
                SupplierInvoiceItemPMList.Add(SupplierInvoiceItemPM);
            }

            return SupplierInvoiceItemPMList;
        }

        private List<SupplierInvoiceItemsConDeclarPM> GetSupplierInvoiceItemConDeclarsPM(AmitalMessaging.Customs.CustomFile.ReleaseFile.INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {

            var SupplierInvoiceItemConDeclarPMList = new List<SupplierInvoiceItemsConDeclarPM>();
            foreach (var invoiceItemConDeclar in invoiceItem.POINTERS)
            {
                int int1 = 0;
                decimal decimal1 = 0;
                var SupplierInvoiceItemConDeclarPM = new SupplierInvoiceItemsConDeclarPM();
                
                SupplierInvoiceItemConDeclarPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                if(this._MySupplierInvoicePM.InvoiceCounterKey > 0)SupplierInvoiceItemConDeclarPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                SupplierInvoiceItemConDeclarPM.InvoiceItemLineNumber = supplierInvoiceItemPM.LineNumber;
                // moran 20.7.16 - AMI-57596 -->
                // SupplierInvoiceItemConDeclarPM.ItemSequence = supplierInvoiceItemPM.SequenceNumeric
                if (int.TryParse(invoiceItemConDeclar.LINENUMBER, out int1)) // moran 31.3.16 - AMI-56462
                {
                    SupplierInvoiceItemConDeclarPM.ItemSequence = int1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing COUNTERKEY (" + invoiceItemConDeclar.LINENUMBER + ") into integer");
                }
                // moran 20.7.16 - AMI-57596 <--
                SupplierInvoiceItemConDeclarPM.DeclarationTypeCode = "1";

                //if (int.TryParse(this._MySupplierInvoicePM.InvoiceNumber, out int1))
                if (!string.IsNullOrWhiteSpace(invoiceItemConDeclar.COUNTERKEY) && invoiceItemConDeclar.COUNTERKEY != "0")
                {
                    if (int.TryParse(invoiceItemConDeclar.COUNTERKEY, out int1)) // moran 31.3.16 - AMI-56462
                    {
                        SupplierInvoiceItemConDeclarPM.InvoiceNumber = int1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing COUNTERKEY (" + invoiceItemConDeclar.COUNTERKEY + ") into integer");
                    }
                }
                // moran 3.3.16 - AMI-56039 -->
                //SupplierInvoiceItemConDeclarPM.DeclarationNumber = this._MyDeclarationPM.DeclarationNumber;
                if (!String.IsNullOrWhiteSpace(_LogitudeReleaseFile.ManifestNumber)) // moran 17.7.16 - AMI-57487
                {
                    SupplierInvoiceItemConDeclarPM.DeclarationNumber = _LogitudeReleaseFile.ManifestNumber;
                }
                else
                {
                    if (this._MyEntryDeclarationPM != null && invoiceItemConDeclar.DECLARATIONID == this._MyEntryDeclarationPM.Id)
                    {
                        SupplierInvoiceItemConDeclarPM.DeclarationNumber = this._MyEntryDeclarationPM.DeclarationNumber;
                    }
                    else if (!string.IsNullOrWhiteSpace(invoiceItemConDeclar.DECLARATIONID))
                    {
                        DeclarationPM EntryDeclarationPM;
                        var myQueryService = new DeclarationQueryService(_context);
                        EntryDeclarationPM = myQueryService.GetSingle(this._LogitudeReleaseFile.EntryFile, true, false);
                        if (EntryDeclarationPM != null)
                        {
                            SupplierInvoiceItemConDeclarPM.DeclarationNumber = EntryDeclarationPM.DeclarationNumber;
                        }
                    }
                }
                // moran 3.3.16 - AMI-56039 <--
                /*
                if (!string.IsNullOrWhiteSpace(invoiceItemConDeclar.STATISTICQUANTITY) && invoiceItemConDeclar.STATISTICQUANTITY != "0")
                {
                if (int.TryParse(invoiceItemConDeclar.STATISTICQUANTITY, out int1))
                {
                    SupplierInvoiceItemConDeclarPM.Quantity = int1;
                }
                else
                {
                    if (decimal.TryParse(invoiceItemConDeclar.STATISTICQUANTITY, out decimal1))
                    {
                        var tempVar = (int?)Math.Truncate(decimal1);
                        SupplierInvoiceItemConDeclarPM.Quantity = tempVar;
                    }
                        else
                        {
                            throw new BusinessErrorException("Error in parsing STATISTICQUANTITY (" + invoiceItemConDeclar.STATISTICQUANTITY + ") into decimal");
                        }
                //    throw new BusinessErrorException("Error in parsing QUANTITY (" + invoiceItemConDeclar.STATISTICQUANTITY + ") into integer");
                }
                }*/
                SupplierInvoiceItemConDeclarPM.Quantity = supplierInvoiceItemPM.InvoiceQuantity; // moran 27.10.16 - AMI-58519
                SupplierInvoiceItemConDeclarPM.QuantityTypeCode = TranslateMeasurmentUnit(invoiceItemConDeclar.QTY_UNIT); // moran 13.1.16 - AMI-55589


                SupplierInvoiceItemConDeclarPM.Tenant = ResolvedTenant();
                SupplierInvoiceItemConDeclarPM.ChangeSetOp = ChangeSetOperation.Insert;

                SupplierInvoiceItemConDeclarPMList.Add(SupplierInvoiceItemConDeclarPM);
            }

            return SupplierInvoiceItemConDeclarPMList;
        }

        private List<SupplierInvioceItemCertificatPM> GetSupplierInvoiceItemCertificatePM(AmitalMessaging.Customs.CustomFile.ReleaseFile.INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {

            var SupplierInvoiceItemCertificatePMList = new List<SupplierInvioceItemCertificatPM>();
            foreach (var invoiceItemCert in invoiceItem.CERTIFICATES)
            {

                var SupplierInvoiceItemCertificatePM = new SupplierInvioceItemCertificatPM();

                SupplierInvoiceItemCertificatePM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;

                if (this._MySupplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceItemCertificatePM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                SupplierInvoiceItemCertificatePM.LineNumber = supplierInvoiceItemPM.LineNumber;
                //SupplierInvoiceItemCertificatePM.ResConfirmationTypeCode = invoiceItemCert.RESPONSECONFIRMATIONTYPECODE;
                if (!String.IsNullOrWhiteSpace(invoiceItemCert.RESPONSECONFIRMATIONTYPECODE)) // moran 11.7.16 - AMI-57272
                {
                    var confirmationType = new ConfirmationTypeRepository(ResolvedTenant());
                    var myConfirmationType = confirmationType.GetSingle(invoiceItemCert.RESPONSECONFIRMATIONTYPECODE);
                    if (myConfirmationType != null)
                    {
                        SupplierInvoiceItemCertificatePM.ResConfirmationTypeCode = myConfirmationType.Code;
                    }
                }

                //SupplierInvoiceItemCertificatePM.ReqConfirmationTypeCode = invoiceItemCert.REQUESTCONFIRMATIONTYPECODE;
                if (!String.IsNullOrWhiteSpace(invoiceItemCert.REQUESTCONFIRMATIONTYPECODE)) // moran 11.7.16 - AMI-57272
                {
                    var confirmationType = new ConfirmationTypeRepository(ResolvedTenant());
                    var myConfirmationType = confirmationType.GetSingle(invoiceItemCert.REQUESTCONFIRMATIONTYPECODE);
                    if (myConfirmationType != null)
                    {
                        SupplierInvoiceItemCertificatePM.ReqConfirmationTypeCode = myConfirmationType.Code;
                    }
                }
                SupplierInvoiceItemCertificatePM.CustomsAttachmentID = invoiceItemCert.CUSTOMSATTACHMENTID;
                SupplierInvoiceItemCertificatePM.CertificateNumber = invoiceItemCert.CERTIFICATENUMBER;
                //SupplierInvoiceItemCertificatePM.CertificateExemptionTypeCode = invoiceItemCert.CERTIFICATEEXEMPTIONTYPE;
                if (!String.IsNullOrWhiteSpace(invoiceItemCert.CERTIFICATEEXEMPTIONTYPE)) // moran 11.7.16 - AMI-57272
                {
                    var certificateExemptionType = new CertificateExemptionTypeRepository(ResolvedTenant());
                    var myCertificateExemptionType = certificateExemptionType.GetSingle(invoiceItemCert.CERTIFICATEEXEMPTIONTYPE);
                    if (myCertificateExemptionType != null)
                    {
                        SupplierInvoiceItemCertificatePM.CertificateExemptionTypeCode = myCertificateExemptionType.Code;
                    }
                }
                //SupplierInvoiceItemCertificatePM.AttachmentTypeCode = invoiceItemCert.ATTACHMENTTYPECODE;
                if (!String.IsNullOrWhiteSpace(invoiceItemCert.ATTACHMENTTYPECODE)) // moran 11.7.16 - AMI-57272
                {
                    var attachmentType = new AttachmentTypeRepository(ResolvedTenant());
                    var myAttachmentType = attachmentType.GetSingle(invoiceItemCert.ATTACHMENTTYPECODE);
                    if (myAttachmentType != null)
                    {
                        SupplierInvoiceItemCertificatePM.AttachmentTypeCode = myAttachmentType.Code;
                    }
                }
                SupplierInvoiceItemCertificatePM.Tenant = ResolvedTenant();
                SupplierInvoiceItemCertificatePM.ChangeSetOp = ChangeSetOperation.Insert;

                SupplierInvoiceItemCertificatePMList.Add(SupplierInvoiceItemCertificatePM);
            }

            return SupplierInvoiceItemCertificatePMList;
        }


        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehiclesPM(AmitalMessaging.Customs.CustomFile.ReleaseFile.INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        { // moran 14.3.16 - AMI-55746

            var SupplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();
            foreach (var invoiceItemCar in invoiceItem.CARS)
            {
                int int1 = 0;
                var SupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM();

                SupplierInvoiceItemVehiclePM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;

                if (this._MySupplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceItemVehiclePM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                //SupplierInvoiceItemVehiclePM.LineNumber = supplierInvoiceItemPM.LineNumber;
                SupplierInvoiceItemVehiclePM.InvoiceItemLineNumber = supplierInvoiceItemPM.LineNumber;
                SupplierInvoiceItemVehiclePM.RichbitFileNumber = invoiceItemCar.VEHICLE_FILE;
                SupplierInvoiceItemVehiclePM.VehicleChassisNumber = invoiceItemCar.CHASSIS;
                if (!string.IsNullOrWhiteSpace(invoiceItemCar.COUNTER) && invoiceItemCar.COUNTER != "0")
                {
                    if (int.TryParse(invoiceItemCar.COUNTER, out int1))
                    {
                        SupplierInvoiceItemVehiclePM.LineNumber = int1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing COUNTER (" + invoiceItemCar.COUNTER + ") into integer");
                    }
                }
                SupplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds = GetSupplierInvoiceItemVehiclesAddsPM(SupplierInvoiceItemVehiclePM, invoiceItemCar);
                if (!string.IsNullOrWhiteSpace(invoiceItemCar.CHASSIS) && string.IsNullOrWhiteSpace(invoiceItemCar.VEHICLE_FILE))
                {
                    SupplierInvoiceItemVehiclePM.VehicleTypeCode = "CN";
                }
                else if (!string.IsNullOrWhiteSpace(invoiceItemCar.VEHICLE_FILE))
                {
                    SupplierInvoiceItemVehiclePM.VehicleTypeCode = "ZZZ";
                }
                SupplierInvoiceItemVehiclePM.Tenant = ResolvedTenant();
                SupplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Insert;

                SupplierInvoiceItemVehiclePMList.Add(SupplierInvoiceItemVehiclePM);
            }

            return SupplierInvoiceItemVehiclePMList;
        }

        private List<SupplierInvoiceItemVehicleAddPM> GetSupplierInvoiceItemVehiclesAddsPM(SupplierInvoiceItemVehiclePM SupplierInvoiceItemVehiclePM, CARS invoiceItemCar)
        { // moran 15.3.16 - AMI-55746

            var SupplierInvoiceItemVehicleAddPMList = new List<SupplierInvoiceItemVehicleAddPM>();

            int int1 = 0;
            decimal decimal1 = 0;

            var SupplierInvoiceItemVehicleAddPM = new SupplierInvoiceItemVehicleAddPM();

            SupplierInvoiceItemVehicleAddPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;

            if (this._MySupplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceItemVehicleAddPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
            SupplierInvoiceItemVehicleAddPM.LineNumber = SupplierInvoiceItemVehiclePM.LineNumber;
            if (!string.IsNullOrWhiteSpace(invoiceItemCar.COUNTER) && invoiceItemCar.COUNTER != "0")
            {
                if (int.TryParse(invoiceItemCar.COUNTER, out int1))
                {
                    SupplierInvoiceItemVehicleAddPM.InvoiceCounterKey = int1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing COUNTER (" + invoiceItemCar.COUNTER + ") into integer");
                }
            }
            SupplierInvoiceItemVehicleAddPM.ChassisNumber = invoiceItemCar.CHASSIS;
            SupplierInvoiceItemVehicleAddPM.EngineNumber = invoiceItemCar.ENGINE;
            SupplierInvoiceItemVehicleAddPM.VehicleModel = invoiceItemCar.MODEL;
            SupplierInvoiceItemVehicleAddPM.RichbitNumber = invoiceItemCar.VEHICLE_FILE;
            SupplierInvoiceItemVehicleAddPM.WindowNumber = invoiceItemCar.WINDOW; // moran 19.11.17 - AMI-62348
            if (!string.IsNullOrWhiteSpace(invoiceItemCar.FOB) && invoiceItemCar.FOB != "0")
            {
                if (decimal.TryParse(invoiceItemCar.FOB, out decimal1))
                {
                    var tempVar = (decimal?)Math.Round(decimal1,2);
                    SupplierInvoiceItemVehicleAddPM.VehicleValue = tempVar;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing FOB (" + invoiceItemCar.FOB + ") into decimal");
                }
            }
            SupplierInvoiceItemVehicleAddPM.Exempt_type = invoiceItemCar.EXEMPT_TYPE;
            if(!string.IsNullOrWhiteSpace(exemptTypesForEntitlement) && !isExemptTypeInDefault && !string.IsNullOrWhiteSpace(SupplierInvoiceItemVehicleAddPM.Exempt_type))
            {
                isExemptTypeInDefault = exemptTypesForEntitlement.Contains(SupplierInvoiceItemVehicleAddPM.Exempt_type);
            }
            SupplierInvoiceItemVehicleAddPM.ChangeSetOp = ChangeSetOperation.Insert;
            SupplierInvoiceItemVehicleAddPM.Tenant = _MyDeclarationPM.Tenant;
            SupplierInvoiceItemVehicleAddPMList.Add(SupplierInvoiceItemVehicleAddPM);


            return SupplierInvoiceItemVehicleAddPMList;
        }


        public string GetTranslationL2P(string partnerID, string tableID, string localCode)
        {
            var rec = (from a in amitalContext.GTRTRANs
                       where a.PARTNERID == partnerID && a.TABLEID == tableID && a.LOCALCODE == localCode
                       select a).FirstOrDefault();
            if (rec == null)
            {
                return null;
            }
            return rec.PARTNERCODE;
        }

    }
}


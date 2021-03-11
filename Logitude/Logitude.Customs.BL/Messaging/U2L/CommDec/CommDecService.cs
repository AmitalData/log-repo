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

namespace Logitude.Customs.BL.Messaging.U2L.CommDec
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

        public bool IsAutonomy = false;
        private decimal _SupplierInvoiceAmount;

        public CommDecService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }
        public override void ProccessGenericRequest(
              string xmlLOGICOMMDEC,
              ref string MoreParams,
              out string MessageOut)

        {
            MessageOut = "";
            int tenant = 0;
            tenant = ResolvedTenant();

            string defValue = GDFDATAQueryService.GetDefault(tenant, "ISRAEL", "CGG_OPN_DEC_MET", "NON", "NON");

            if(!string.IsNullOrEmpty(defValue) && defValue=="B")
            {
                 var messagingService = new DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceMessagingService();

                messagingService.CreateCRS(tenant, null, null);
 
             
            }
            else
            {
                ProccessGenericRequestReal(xmlLOGICOMMDEC,ref MoreParams,out MessageOut);
            }


        }


        public void ProccessGenericRequestReal(
              string xmlLOGICOMMDEC,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "CommDecService ";
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Clear();
            DeserilazeObject(xmlLOGICOMMDEC);
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            //CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            _context = CustomContext.GetContext(ResolvedTenant());
            amitalContext = AmitalContext.GetContext(ResolvedTenant());
            var myQueryService = new DeclarationQueryService(_context);

            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());

            MyGenericResponseObj.Stage = "GetSingleB4Upsert";
            if (!String.IsNullOrWhiteSpace(_LogitudeCommDecFile.CustomFileNo))
            {
                string existId = myQueryService.GetIdByCustomFileNo(_LogitudeCommDecFile.CustomFileNo, ResolvedTenant());
                if (!String.IsNullOrWhiteSpace(existId))
                {
                    this._MyDeclarationPM = myQueryService.GetSingle(existId, true, false);
                    AppendLogLine("GetSingleB4Upsert:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    if (this._MyDeclarationPM != null)
                    {
                        if (!declarationUpdateService.CheckIfUpdatingAllowed(this._MyDeclarationPM))
                        {
                            CheckMasterToUpdate(MoreParams);
                            AppendLogLine("Updating Not Allowed For Declaration " + this._MyDeclarationPM.CustomFileNo + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                            return;
                        }
                    }
                }
            }

            MyGenericResponseObj.Stage = "DeclarationUpsert";
            string xmlLOGICUSTFILE = xmlLOGICOMMDEC;
            xmlLOGICUSTFILE = xmlLOGICUSTFILE.Replace("LOGICOMMDEC", "LOGICUSTFILE");
            xmlLOGICUSTFILE = xmlLOGICUSTFILE.Replace("LogitudeCommDecFile", "LogitudeCustomsFile");
            DeclarationUpsertService myDeclarationUpsertService = new DeclarationUpsertService();
            try
            {
                var upsertParam = "CommDecService";
                ///myDeclarationUpsertService.suppressNewTrans = true;
                myDeclarationUpsertService.ProccessGenericRequest(xmlLOGICUSTFILE, ref upsertParam, out MessageOut);
            }
            catch (DbEntityValidationException ex)
            {
                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                AppendLogLine("Declaration Upsert Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                return;
            }
            catch (Exception e)
            {
                AppendLogLine("Declaration Upsert Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                return;
            }
            if(!String.IsNullOrWhiteSpace(MyGenericResponseObj.StatusType.ToString()) && MyGenericResponseObj.StatusType != GenericResponseObj.StatusEnum.Success)
            {
                AppendLogLine("Declaration Upsert Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                return;
            }
            //Delete Supplier Invoice

            if (String.IsNullOrWhiteSpace(_LogitudeCommDecFile.Id))
            {
                string existId = myQueryService.GetIdByCustomFileNo(_LogitudeCommDecFile.CustomFileNo, ResolvedTenant());
                _LogitudeCommDecFile.Id = existId;
            }

            /////////////////////////////////////////////////////////////
            _context = CustomContext.GetContext(ResolvedTenant());
            MyGenericResponseObj.Stage = "GetSingle";
            myQueryService = new DeclarationQueryService(_context);
            this._MyDeclarationPM = myQueryService.GetSingle(this._LogitudeCommDecFile.Id, true, false);
            if (this._MyDeclarationPM == null)
            {
                AppendLogLine("Declaration Get Single Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                throw new BusinessErrorException("LOGITUDE FILE is " + this._LogitudeCommDecFile.Id + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            CheckMasterToUpdate(MoreParams);
            if (!String.IsNullOrWhiteSpace(_LogitudeCommDecFile.TaxationDateTime))
            {
                this._MyDeclarationPM.TaxationDateTime = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeCommDecFile.TaxationDateTime, "LogitudeCommDecFile.TaxationDateTime"); // moran 22.1.17 - AMI-58777
            }

            if (!string.IsNullOrWhiteSpace(this._LogitudeCommDecFile.TransferImporterId)) // moran 22.3.17 - AMI-59830
            {
                if (_LogitudeCommDecFile.TransferImporterId.Length > 9)
                {
                    this._MyDeclarationPM.EntitleImporterId = TranslateClient(_LogitudeCommDecFile.TransferImporterId.Substring(0, 9));
                    this._MyDeclarationPM.EntitleImporterCode = _LogitudeCommDecFile.TransferImporterId.Substring(0, 9);
                }
                else
                {
                    this._MyDeclarationPM.EntitleImporterId = TranslateClient(_LogitudeCommDecFile.TransferImporterId);
                    this._MyDeclarationPM.EntitleImporterCode = _LogitudeCommDecFile.TransferImporterId;
                }
                if (!string.IsNullOrWhiteSpace(this._LogitudeCommDecFile.ImporterEntitlementTypeCode))
                {
                    EntitlementTypeQueryService entitlementTypeQueryService = new EntitlementTypeQueryService(this._MyDeclarationPM.Tenant);
                    EntitlementTypePM entitlementType = entitlementTypeQueryService.GetSingle(this._LogitudeCommDecFile.ImporterEntitlementTypeCode, false, false);
                    if (entitlementType != null)
                    {
                        this._MyDeclarationPM.ImporterEntitlementTypeCode = entitlementType.Code;
                        this._MyDeclarationPM.ImporterEntitlementTypeName = entitlementType.LocalName;
                    }
                }
            }

            if (this._MyDeclarationPM.Consignments.Count == 1) // moran 11.1.17 - AMI-59265 - moved before creating invoices
            {
                if (this._MyDeclarationPM.Consignments[0].ChangeSetOp != ChangeSetOperation.Insert)
                {
                    this._MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update;
                }

                if (!String.IsNullOrWhiteSpace(_LogitudeCommDecFile.ManifestNumber))
                {
                    this._MyDeclarationPM.Consignments[0].ManifestNumber = _LogitudeCommDecFile.ManifestNumber;
                }

                string loadingPortCode = null;
                if (!String.IsNullOrWhiteSpace(_LogitudeCommDecFile.LoadingPortCode))
                {
                    loadingPortCode = TranslateloadPort(_LogitudeCommDecFile.LoadingPortCode);
                }
                if (!String.IsNullOrWhiteSpace(loadingPortCode)) this._MyDeclarationPM.Consignments[0].LoadingPortCode = loadingPortCode;

                if (!String.IsNullOrWhiteSpace(_LogitudeCommDecFile.OriginCountryCode))
                {
                    string countryCode = "";
                    if (_LogitudeCommDecFile.OriginCountryCode.Length > 2)
                    {
                        countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", _LogitudeCommDecFile.OriginCountryCode);
                    }
                    else
                    {
                        countryCode = _LogitudeCommDecFile.OriginCountryCode;
                    }
                    if (!string.IsNullOrWhiteSpace(countryCode)) this._MyDeclarationPM.Consignments[0].OriginCountryCode = countryCode;
                }
                else
                {
                    //                    this._MyDeclarationPM.Consignments[0].OriginCountryCode = null;
                }

                if (!string.IsNullOrWhiteSpace(_LogitudeCommDecFile.CargoDescription)) this._MyDeclarationPM.Consignments[0].CargoDescription = _LogitudeCommDecFile.CargoDescription;
                //this._MyDeclarationPM.Consignments[0].ManifestDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeCommDecFile.ManifestDate, "LogitudeCommDecFile.ManifestDate");
                if (!string.IsNullOrWhiteSpace(_LogitudeCommDecFile.ArrivalDateTime)) this._MyDeclarationPM.Consignments[0].UnloadDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeCommDecFile.ArrivalDateTime, "LogitudeCommDecFile.ArrivalDateTime");

                if (!String.IsNullOrWhiteSpace(_LogitudeCommDecFile.OriginCountryId))
                {
                    string countryCode = "";
                    if (_LogitudeCommDecFile.OriginCountryId.Length > 2)
                    {
                        countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", _LogitudeCommDecFile.OriginCountryId);
                    }
                    else
                    {
                        countryCode = _LogitudeCommDecFile.OriginCountryId;
                    }
                    if (!string.IsNullOrWhiteSpace(countryCode)) this._MyDeclarationPM.Consignments[0].OriginCountryCode = countryCode;
                }
                if (!string.IsNullOrWhiteSpace(_LogitudeCommDecFile.WarehouseId)) this._MyDeclarationPM.Consignments[0].StorageSiteCode = TranslateDeliverySite(_LogitudeCommDecFile.WarehouseId);


                if (this._LogitudeCommDecFile.PACKAGES != null && this._LogitudeCommDecFile.PACKAGES.Count() > 0)
                {
                    var myConsignmentPackageUpdateService = new ConsignmentPackageUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());
                    DeleteConsignmentPackages(myConsignmentPackageUpdateService);
                    this._MyDeclarationPM.Consignments[0].ConsignmentPackages = GetConsignmentPackages(this._LogitudeCommDecFile.PACKAGES);
                }
            }
            else
            {
                MyGenericResponseObj.Message = "Declaration has multiple Consignments(" + this._MyDeclarationPM.Consignments.Count.ToString() + ") and Consignment details didn't update";
                AppendLogLine("Declaration has multiple Consignments(" + this._MyDeclarationPM.Consignments.Count.ToString() + ") and Consignment details didn't update");
            }

            if (this._LogitudeCommDecFile.INVOICE != null)
            {
                if (this._LogitudeCommDecFile.INVOICE.Count() > 0)
                {

                    string defValue = GetDefault("ISRAEL", "CGG_BUILD_UNIT", "NON", "NON", ResolvedTenant());
                    if (defValue == "Y")
                    {
                        _IsBuildItemsUnit = true;
                    }
                    if (this._MyDeclarationPM.SupplierInvoices == null || this._MyDeclarationPM.SupplierInvoices.Count() == 0)
                    {
                        this._MyDeclarationPM.SupplierInvoices = new List<SupplierInvoicePM>();
                    }
                    /*
                    else if (this._MyDeclarationPM.SupplierInvoices.Count() > 1)
                    {
                        AppendLogLine("Declaration has More than one Invoice, Invoice will not be Updated..");
                        return;
                    }
                    else
                    {
                    
                        if (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().SupplierInvoiceItems != null && this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().SupplierInvoiceItems.Count() > 1)
                        {
                            AppendLogLine("Declaration has More than one Invoice item, Invoice will not be Updated..");
                            return;
                        }
                        if (this._LogitudeCommDecFile.INVOICE != null)
                        {
                            if(this._LogitudeCommDecFile.INVOICE.Count() > 1)
                            {
                                AppendLogLine("More than one Invoice received in the message, Invoice will not be Updated..");
                                return;
                            }
                            if (this._LogitudeCommDecFile.INVOICE.FirstOrDefault().INVOICEITEMS != null && this._LogitudeCommDecFile.INVOICE.FirstOrDefault().INVOICEITEMS.Count() > 1)
                            {
                                AppendLogLine("More than one Invoice item received in the message, Invoice will not be Updated..");
                                return;
                            }
                        }
                    }
                    */
                    foreach (var itemINVOICE in this._LogitudeCommDecFile.INVOICE)
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
                    }
                    AppendLogLine("Update:InvoiceInsert:All:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                }
            }

            AppendLogLine("Importer Code: " + this._MyDeclarationPM.ImporterCode);

            if (this._LogitudeCommDecFile.CustomsDocuments != null && this._LogitudeCommDecFile.CustomsDocuments.Where(d => d.Blocked != "1").Count() > 0) // moran 2.6.16 - AMI-56624
            {
                var myCustomsDocumentQueryService = new CustomsDocumentQueryService(dbContext);
                var myCustomsDocumentPointerUpdateService = new CustomsDocumentPointerUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);

                foreach (var customsDocument in this._LogitudeCommDecFile.CustomsDocuments)
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
            if(!string.IsNullOrWhiteSpace(_LogitudeCommDecFile.SiteCode) && this._MyDeclarationPM.Consignments != null && this._MyDeclarationPM.Consignments.Count() > 0 && (this._MyDeclarationPM.Consignments[0].ConsignmentInternalTransitions == null || (this._MyDeclarationPM.Consignments[0].ConsignmentInternalTransitions != null && this._MyDeclarationPM.Consignments[0].ConsignmentInternalTransitions.Count() < 1)))
            {
                var internalBorderSiteType = new InternalBorderSiteTypeRepository(ResolvedTenant());
                var myinternalBorderSiteType = internalBorderSiteType.GetSingle(_LogitudeCommDecFile.SiteCode);
                string PackageTypeCode = "";
                if (myinternalBorderSiteType == null)
                {
                    PackageTypeCode = GetTranslationL2P("IIGC", "CTBBONDED", _LogitudeCommDecFile.SiteCode);
                }
                else
                {
                    PackageTypeCode = myinternalBorderSiteType.Code.ToString();
                }
                if (!string.IsNullOrWhiteSpace(PackageTypeCode))
                {

                    ConsignmentInternalTransitionPM transitionPM = new ConsignmentInternalTransitionPM()
                    {
                        ConsignmentNumber = this._MyDeclarationPM.Consignments[0].ConsignmentNumber,
                        DeclarationId = this._MyDeclarationPM.Consignments[0].DeclarationId,
                        LineNumber = 1,
                        Tenant = this._MyDeclarationPM.Consignments[0].Tenant,
                        SiteCode = PackageTypeCode,

                        ChangeSetOp = ChangeSetOperation.Insert,

                    };
                    this._MyDeclarationPM.Consignments[0].ConsignmentInternalTransitions.Add(transitionPM);
                }
            }

            this._LOGICUSTFILE = XmlGenericUtil<LOGICUSTFILE>.DeSerializeObject(xmlLOGICUSTFILE);
            if (_LOGICUSTFILE.LogitudeCustomsFile == null || _LOGICUSTFILE.LogitudeCustomsFile.Length != 1)
            {
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                MyGenericResponseObj.Message = "customFile.LogitudeCustomsFile.Length !=1 !!!";
            }
            else
            {
                this._AmitalCustomsFile = _LOGICUSTFILE.LogitudeCustomsFile[0];
                if (_MyDeclarationPM.IsCourierDeclaration == true)
                {
                    CalcIsAutonomy();
                    CalcProcedureCurrentCode();
                    if (this.IsAutonomy)
                    {
                        UpdateDeclarationPending("901");
                    }
                    UpdateNoIdUnder150();
                }
            }

            _MyDeclarationPM.CurrentContextTag = UpsertActionConst; // moran 28.7.16 - Task 22249

            if(this._MyDeclarationPM.ProcedureCurrentCode == "4000020")
            {
                this._MyDeclarationPM.ExcludeConsignment = true;
            }

            declarationUpdateService = new DeclarationUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());

            declarationUpdateService.Update(this._MyDeclarationPM, true);
            AppendLogLine("declarationUpdat:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
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

        private void UpdateDeclarationPending(string declarationPendingCode)
        {
            if (currentDeclarationCourierStatusPM != null)
            {
                CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(_context);
                CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle(declarationPendingCode, false, false);
                if (courierPendingReasonPM == null)
                {
                    LogMessagingUtil.Instance.AppendLine("לא קיים קוד Pending = " + declarationPendingCode + " בטבלת סיבות Pending");
                    return;
                }
                LogMessagingUtil.Instance.AppendLine("Pending - " + declarationPendingCode);
                DeclarationPendingPM _declarationPendingPM = null;
                if (currentDeclarationCourierStatusPM.DeclarationPendings != null && currentDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
                {
                    _declarationPendingPM = currentDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == currentDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == declarationPendingCode).FirstOrDefault();
                }
                if (_declarationPendingPM == null)
                {
                    _declarationPendingPM = new DeclarationPendingPM();
                    _declarationPendingPM.CourierPendingReasonCode = declarationPendingCode;
                    _declarationPendingPM.Status = "A";
                    _declarationPendingPM.ChangeSetOp = ChangeSetOperation.Insert;
                    currentDeclarationCourierStatusPM.DeclarationPendings.Add(_declarationPendingPM);
                }
                else if (_declarationPendingPM.Status != "A")
                {
                    _declarationPendingPM.ChangeSetOp = ChangeSetOperation.Update;
                    _declarationPendingPM.Status = "A";
                }
                if (_declarationPendingPM.ChangeSetOp != ChangeSetOperation.None)
                {
                    LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code To " + declarationPendingCode);
                    if (currentDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                if (currentDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(_context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                    declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                }
            }
        }

        private void CalcProcedureCurrentCode()
        {
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
        }

        private void CalcIsAutonomy()
        {
            if (String.IsNullOrWhiteSpace(this._LogitudeCommDecFile.IsAutonomy) && !String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterCode) && this._MyDeclarationPM.ImporterCode.Substring(0, 1) == "8")
            {
                this.IsAutonomy = true;
                return;
            }

            CustomsAutonomyKeywordQueryService customsAutonomyKeywordQueryService = new CustomsAutonomyKeywordQueryService(_context);
            var casualImportelTel = _AmitalCustomsFile.CasualImportelTel;
            if (!String.IsNullOrWhiteSpace(casualImportelTel)) casualImportelTel = _AmitalCustomsFile.CasualImportelTel.TrimStart(new Char[] { '0' });
            if (customsAutonomyKeywordQueryService.CheckIfsAutonomy(_AmitalCustomsFile.CasualImporterCity, casualImportelTel, ResolvedTenant()))
            {
                this.IsAutonomy = true;
                return;
            }

            if (!String.IsNullOrWhiteSpace(this._LogitudeCommDecFile.IsAutonomy) && this._LogitudeCommDecFile.IsAutonomy.ToLower().Substring(0, 1) == "y")
            {
                this.IsAutonomy = true;
            }
        }

        private void UpdateNoIdUnder150()
        {
            if (!String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterCode))
            {
                if (_CourierMasterPM != null)
                {
                    Card myCard = null;
                    var repository = new CardRepository(ResolvedTenant());
                    myCard = repository.GetSingleCard(_CourierMasterPM.IntegratorCode, ResolvedTenant());
                    if (myCard != null && !String.IsNullOrWhiteSpace(myCard.Code))
                    {
                        string defValue = GetDefault("ISRAEL", "CGO_NO_ID_150", "NON", myCard.Code, ResolvedTenant());
                        if (defValue == "Y")
                        {
                            if(this._MyDeclarationPM.SupplierInvoices.FirstOrDefault() != null && (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().ChangeSetOp == ChangeSetOperation.Insert || (this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().ChangeSetOp != ChangeSetOperation.Insert && this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().InvoiceAmount.GetValueOrDefault() != this._SupplierInvoiceAmount)))
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
                            if (currentDeclarationCourierStatusPM != null && currentDeclarationCourierStatusPM.TotalInvoiceAmountInUSD != null && currentDeclarationCourierStatusPM.TotalInvoiceAmountInUSD < 150)
                            {
                                this._MyDeclarationPM.ImporterCode = null;
                                this._MyDeclarationPM.ImporterId = null;
                            }
                        }
                    }
                }
            }
        }


        private void CheckMasterToUpdate(string MoreParams)
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
                _CourierMasterPM = myCourierMasterQueryService.GetSingleByAirlineAWBs(airlineId, _LogitudeCommDecFile.HAWB, _LogitudeCommDecFile.MAWB, ResolvedTenant());
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
                var myCourierDeclarationUpdateService = new CourierDeclarationUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());
                _CourierDeclarationPM = myCourierDeclarationQueryService.GetSingle(_MyDeclarationPM.Id, _CourierMasterPM.Id, false, true);
                if (_CourierDeclarationPM == null)
                {
                    AppendLogLine("CourierDeclarationPM not found for DeclarationPM.Id: " + _MyDeclarationPM.Id + " CourierMasterPM.Id: " + _CourierMasterPM.Id);
                    if (!this._MyDeclarationPM.HatraDate.HasValue)
                    {
                        CourierDeclarationPM _CourierDeclarationPMPMDiferentMaster = myCourierDeclarationQueryService.GetCourierDeclarationByDeclarationId(_MyDeclarationPM.Id, ResolvedTenant());
                        if (_CourierDeclarationPMPMDiferentMaster != null)
                        {
                            AppendLogLine("try to delete CourierDeclaration with Diferent Master (id: " + _CourierDeclarationPMPMDiferentMaster.CourierMasterId + "  found for DeclarationPM.Id: " + _MyDeclarationPM.Id + " CourierMasterPM.Id: " + _CourierMasterPM.Id);
                            _CourierDeclarationPMPMDiferentMaster.ChangeSetOp = ChangeSetOperation.Delete;
                            try
                            {
                                myCourierDeclarationUpdateService.Update(_CourierDeclarationPMPMDiferentMaster, true);
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
                                    AppendLogLine("try to update declarationCourierStatus for DeclarationPM.Id: " + _MyDeclarationPM.Id );
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
                    int? sequenceNumericMax = myCourierDeclarationQueryService.GetCourierMasterMaxSequenceNumeric(_CourierDeclarationPM.CourierMasterId, ResolvedTenant());
                    if (sequenceNumericMax == null)
                    {
                        sequenceNumericMax = 0;
                    }
                    _CourierDeclarationPM.SequenceNumeric = sequenceNumericMax + 1;
                }
                _CourierDeclarationPM.Tenant = ResolvedTenant();
                
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

        private string TranslateAirline(string airlineId)
        {
            if (String.IsNullOrWhiteSpace(airlineId))
            {
                AppendLogLine("airlineId is null");
                return null;
            }

            //GET Airline.Id BY PREFIX
            int index = airlineId.IndexOf('-');
            if (index > 0)
            {
                string code = airlineId.Substring(0, index);
                string prefix = airlineId.Substring(index + 1);
                CustomsAirlineRepository airlineRepository = new CustomsAirlineRepository(ResolvedTenant());
                CustomsAirline airline = airlineRepository.GetByAirlineAndPrefix(code, prefix, ResolvedTenant());
                if (airline != null) return airline.Id;
            }
            else
            {
                CustomsAirlineRepository airlineRepository = new CustomsAirlineRepository(ResolvedTenant());
                CustomsAirline airline = airlineRepository.GetByPrefix(airlineId, ResolvedTenant());
                if (airline != null) return airline.Id;
            }

            AppendLogLine("No Airline found for airlineId " + airlineId);
            return null;
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
                AppendLogLine("amitalDepartmentCode is null");
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
                if (string.IsNullOrWhiteSpace(package.PackTypeId))
                {
                    //package.PackTypeId = "PP";
                }
                ConsignmentPackagePM.MarksNumbers = package.SignNum;
                ConsignmentPackagePM.Tenant = ResolvedTenant();
                ConsignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;

                ConsignmentPackagePMList.Add(ConsignmentPackagePM);
            }

            return ConsignmentPackagePMList;
        }


        void DeserilazeObject(string xmlLOGICOMMDEC)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("CommDecService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");

            if (string.IsNullOrWhiteSpace(xmlLOGICOMMDEC))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGICOMMDEC.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGICOMMDEC.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGICOMMDEC);
            }

            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGICOMMDEC = XmlGenericUtil<LOGICOMMDEC>.DeSerializeObject(xmlLOGICOMMDEC);

            if (_LOGICOMMDEC.LogitudeCommDecFile == null || _LOGICOMMDEC.LogitudeCommDecFile.Length != 1)
            {
                throw new BusinessErrorException("_LOGICOMMDEC.CommDec.Length != 1");
            }
            this._LogitudeCommDecFile = _LOGICOMMDEC.LogitudeCommDecFile[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudeCommDecFile.Id))
            {
                throw new BusinessErrorException("LOGITUDEFILE is missing");
            }
            AppendLogLine("LOGITUDE FILE = " + this._LogitudeCommDecFile.Id);
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
        public  string GetExampleDataIn1_()
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

        private void InvoiceInsert()
        {
            //CheckExist();

            if (this._MyDeclarationPM.SupplierInvoices == null || this._MyDeclarationPM.SupplierInvoices.Count() == 0 || this._MyDeclarationPM.SupplierInvoices.Where(r => r.ChangeSetOp != ChangeSetOperation.Insert).Count() == 0)
            {
                AppendLogLine("Insert Invoice..");
                this._MySupplierInvoicePM = new SupplierInvoicePM();
                this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;
            }
            else if (this._MyDeclarationPM.SupplierInvoices.Count() == 1 && this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().ChangeSetOp != ChangeSetOperation.Insert)
            {
                AppendLogLine("Update Invoice..");
                this._MySupplierInvoicePM = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault();
                this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Update;
                this._SupplierInvoiceAmount = this._MySupplierInvoicePM.InvoiceAmount.GetValueOrDefault();
            }
            else
            {
                this._SupplierInvoiceAmount = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().InvoiceAmount.GetValueOrDefault();
                AppendLogLine("Unknown state of Invoice update, Invoice will not be Updated..");
                return;
            }

            var myQueryService = new CustomsVendorQueryService(this._context);

            int int1 = 0;
            decimal decimal1 = 0;

            MyGenericResponseObj.Stage = "GetContext";

            if (this._MySupplierInvoicePM.SupplierInvoiceItems == null)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
            }
            if (this._MySupplierInvoicePM.SupplierInvoiceItems.Count == 0 || this._MySupplierInvoicePM.SupplierInvoiceItems.Where(r => r.ChangeSetOp != ChangeSetOperation.Insert).Count() == 0)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems.Add(new SupplierInvoiceItemPM() { ChangeSetOp = ChangeSetOperation.Insert });
            }
            else if (this._MySupplierInvoicePM.SupplierInvoiceItems.Count == 1 && this._MySupplierInvoicePM.SupplierInvoiceItems.FirstOrDefault().ChangeSetOp != ChangeSetOperation.Insert)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems.FirstOrDefault().ChangeSetOp = ChangeSetOperation.Update;
            }

            MyGenericResponseObj.Stage = "Mapping";

            this._MySupplierInvoicePM.DeclarationId = this._MyDeclarationPM.Id;
            if (!string.IsNullOrWhiteSpace(this._INVOICE.INVOICELINENO) && this._INVOICE.INVOICELINENO != "0")
            {
                if (int.TryParse(this._INVOICE.INVOICELINENO, out int1))
                {
                    this._MySupplierInvoicePM.SequenceNumeric = int1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing INVOICELINENO (" + this._INVOICE.INVOICELINENO + ") into integer");
                }
            }
            if (!string.IsNullOrWhiteSpace(this._INVOICE.ACCOUNTTYPE))
            {
                this._MySupplierInvoicePM.AccountTypeCode = this._INVOICE.ACCOUNTTYPE;
            }
            else
            {
                this._MySupplierInvoicePM.AccountTypeCode = "380";
            }

            if (!string.IsNullOrWhiteSpace(this._INVOICE.INVOICENUMBER)) this._MySupplierInvoicePM.InvoiceNumber = this._INVOICE.INVOICENUMBER;
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
                    if (!string.IsNullOrWhiteSpace(customsVendorPM.CountryCode)) this._MySupplierInvoicePM.IssueCountryCode = customsVendorPM.CountryCode;
                }
            }
            //this._MySupplierInvoicePM.InvoiceCurrencyTypeCode = this._INVOICE.CURRENCYCODE;
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
            if (this._INVOICE.ISSUEDATE != null)
            {
                this._MySupplierInvoicePM.IssueDate = AmitalConvertUtil.GetUnifreightFormatedDate(this._INVOICE.ISSUEDATE, "INVOICE.ISSUEDATE");
            }
            if (this._MySupplierInvoicePM.IssueCountryCode == null && this._INVOICE.ISSUECOUNTRYCODE != null)
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
            if (!_MyDeclarationPM.IsCourierDeclaration)
            {
                this._MySupplierInvoicePM.IssueCountryCode = this._INVOICE.ISSUECOUNTRYCODE;
            }
            if (this._INVOICE.INCOTERM_ID != null)
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
                        throw new BusinessErrorException("Error in parsing INSURANCE_AMNT (" + this._INVOICE.INSURANCE_AMNT + ") into decimal");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(this._INVOICE.INSURANCE_PERCENT) && this._INVOICE.INSURANCE_PERCENT != "0")
                {
                    if (decimal.TryParse(this._INVOICE.INSURANCE_PERCENT, out decimal1))
                    {
                        this._MySupplierInvoicePM.InsruancePercentage = decimal1;
                        if (this._MySupplierInvoicePM.InsruancePercentage != null)
                        {
                            CalculateInsuranceAmount(this._MySupplierInvoicePM.InsruancePercentage);
                        }
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing INSURANCE_PERCENT (" + this._INVOICE.INSURANCE_PERCENT + ") into decimal");
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
            //this._MyDeclarationPM.SupplierInvoices.Add(this._MySupplierInvoicePM);
            if (this._MySupplierInvoicePM.ChangeSetOp != ChangeSetOperation.Update)
            {
                this._MySupplierInvoicePM.Tenant = ResolvedTenant();
                this._MyDeclarationPM.SupplierInvoices.Add(this._MySupplierInvoicePM);
            }
            return;

        }

        private void CalculateInsuranceAmount(decimal? insruancePercentage)
        {
             if (insruancePercentage == null) return;
            decimal? value = null;
            if (this._MySupplierInvoicePM.InvoiceAmount != null) value = this._MySupplierInvoicePM.InvoiceAmount;
            if (value == null)
            {
                value = this._MySupplierInvoicePM.TotalFreightInFreightCurrency;
            }
            else
            {
                if (this._MySupplierInvoicePM.TotalFreightInFreightCurrency != null) value = value + this._MySupplierInvoicePM.TotalFreightInFreightCurrency;
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
                    string tempFreightCurrencyCode = SupplierInvoiceFreightAmountPM.CurrencyTypeCode;
                    var myQueryService = new SupplierInvoiceFreightAmountQueryService(_context);
                    SupplierInvoiceFreightAmountPM = myQueryService.GetSingle(supplierInvoicePM.DeclarationId, supplierInvoicePM.InvoiceCounterKey, tempFreightCurrencyCode, true, false);
                    if(SupplierInvoiceFreightAmountPM != null && SupplierInvoiceFreightAmountPM.InvoiceCounterKey == supplierInvoicePM.InvoiceCounterKey)
                    {
                        SupplierInvoiceFreightAmountPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    else
                    {
                        SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM();
                        SupplierInvoiceFreightAmountPM.ChangeSetOp = ChangeSetOperation.Insert;
                        SupplierInvoiceFreightAmountPM.DeclarationId = supplierInvoicePM.DeclarationId;
                        SupplierInvoiceFreightAmountPM.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;
                        SupplierInvoiceFreightAmountPM.Tenant = ResolvedTenant();
                    }
                    SupplierInvoiceFreightAmountPM.CurrencyTypeCode = tempFreightCurrencyCode;
                    decimal decimal1;
                    if (decimal.TryParse(transpValItem.TRANSP_VALUE_L, out decimal1))
                    {
                        SupplierInvoiceFreightAmountPM.Amount = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing TRANSP_VALUE_L (" + transpValItem.TRANSP_VALUE_L + ") into integer");
                    }

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
            if (this._MySupplierInvoicePM.TotalFreightInNIS != null) this._MySupplierInvoicePM.TotalFreightInNIS = Math.Round(this._MySupplierInvoicePM.TotalFreightInNIS.Value, 2);

            if (this._MySupplierInvoicePM.FreightCurrencyTypeCode == "ILS")
            {
                this._MySupplierInvoicePM.TotalFreightInFreightCurrency = this._MySupplierInvoicePM.TotalFreightInNIS;
            }
            else if (!String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.FreightCurrencyTypeCode) && this._MySupplierInvoicePM.TotalFreightInNIS != null)
            {
                customsExchangeRates = CustomsExchangeRatequery.GetExchangeRateByCurrencyAndDate(this._MySupplierInvoicePM.FreightCurrencyTypeCode, System.DateTime.Now, ResolvedTenant());
                CustomsExchangeRatePM freightRate = customsExchangeRates.Where(d => d.CurrencyTypeCode == this._MySupplierInvoicePM.FreightCurrencyTypeCode).FirstOrDefault();
                if (freightRate != null)
                {
                    if (this._MySupplierInvoicePM.TotalFreightInFreightCurrency == null) this._MySupplierInvoicePM.TotalFreightInFreightCurrency = 0;
                    this._MySupplierInvoicePM.TotalFreightInFreightCurrency = this._MySupplierInvoicePM.TotalFreightInNIS / freightRate.ExchangeRate;
                }
            }
            if (this._MySupplierInvoicePM.TotalFreightInFreightCurrency != null) this._MySupplierInvoicePM.TotalFreightInFreightCurrency = Math.Round(this._MySupplierInvoicePM.TotalFreightInFreightCurrency.Value, 2);

            return SupplierInvoiceFreightAmountPMList;
        }

        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModificationsPM(INVOICE iNVOICE) // moran 26.5.16 - AMI-56711
        {
            var SupplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>();
            decimal decimal1 = 0;

            if (this._MyDeclarationPM.SupplierInvoices.Count() < 1)
            {
                var SupplierInvoiceModificationPM = new SupplierInvoiceModificationPM();
                if (!string.IsNullOrWhiteSpace(this._LOGICOMMDEC.LogitudeCommDecFile[0].agent_fee) && this._LOGICOMMDEC.LogitudeCommDecFile[0].agent_fee != "0")
                {
                    if (decimal.TryParse(this._LOGICOMMDEC.LogitudeCommDecFile[0].agent_fee, out decimal1))
                    {
                        SupplierInvoiceModificationPM.Amount = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing agent_fee (" + this._LOGICOMMDEC.LogitudeCommDecFile[0].agent_fee + ") into integer");
                    }
                    if (!String.IsNullOrWhiteSpace(this._LOGICOMMDEC.LogitudeCommDecFile[0].agent_fee_currency))
                    {


                        var agentFeeCurrency = new CurrencyTypeRepository(ResolvedTenant());
                        var myagentFeeCurrency = agentFeeCurrency.GetSingle(this._LOGICOMMDEC.LogitudeCommDecFile[0].agent_fee_currency);
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
                    SupplierInvoiceModificationPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                    SupplierInvoiceModificationPM.TypeCode = "160";
                    SupplierInvoiceModificationPM.Tenant = ResolvedTenant();
                    SupplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Insert;

                    SupplierInvoiceModificationPMList.Add(SupplierInvoiceModificationPM);
                }
            }
            return SupplierInvoiceModificationPMList;
        }


        private List<SupplierInvoiceFreightAmountPM> GetSupplierInvoiceFreightAmountPM(SupplierInvoicePM supplierInvoicePM) // moran 11.4.16 - AMI-56512
        {
            var SupplierInvoiceFreightAmountPMList = new List<SupplierInvoiceFreightAmountPM>();
            var SupplierInvoiceFreightAmountPM = new SupplierInvoiceFreightAmountPM();

            SupplierInvoiceFreightAmountPM.DeclarationId = supplierInvoicePM.DeclarationId;
            SupplierInvoiceFreightAmountPM.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;

            SupplierInvoiceFreightAmountPM.Amount = supplierInvoicePM.TotalFreightInFreightCurrency;
            SupplierInvoiceFreightAmountPM.CurrencyTypeCode = supplierInvoicePM.FreightCurrencyTypeCode;

            SupplierInvoiceFreightAmountPM.Tenant = ResolvedTenant();
            SupplierInvoiceFreightAmountPM.ChangeSetOp = ChangeSetOperation.Insert;

            SupplierInvoiceFreightAmountPMList.Add(SupplierInvoiceFreightAmountPM);

            return SupplierInvoiceFreightAmountPMList;
        }


        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItemPM(Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICE invoice)
        {
            Dictionary<string, string> ClasificationQtyTypes = new Dictionary<string, string>() { };
            CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(ResolvedTenant());
            var SupplierInvoiceItemPMList = new List<SupplierInvoiceItemPM>();
            
            foreach (var invoiceItem in invoice.INVOICEITEMS)
            {
                int int1 = 0;
                decimal decimal1 = 0;
                var SupplierInvoiceItemPM = new SupplierInvoiceItemPM();
                if(this._MySupplierInvoicePM.SupplierInvoiceItems != null && this._MySupplierInvoicePM.SupplierInvoiceItems.Count() == 1 && invoice.INVOICEITEMS.Count() == 1)
                {
                    SupplierInvoiceItemPM = this._MySupplierInvoicePM.SupplierInvoiceItems.FirstOrDefault();
                }
                SupplierInvoiceItemPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                SupplierInvoiceItemPM.CounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
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
                if (String.IsNullOrWhiteSpace(SupplierInvoiceItemPM.ClassificationCode)) SupplierInvoiceItemPM.ClassificationCode = invoiceItem.CLASSIFICATIONCODE;
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
                
                string invoiceQuantityType = null;
                if (_IsBuildItemsUnit && !string.IsNullOrEmpty(SupplierInvoiceItemPM.ClassificationCode))
                {
                    if (ClasificationQtyTypes.Keys.Contains(SupplierInvoiceItemPM.ClassificationCode))
                    {
                        invoiceQuantityType = ClasificationQtyTypes[SupplierInvoiceItemPM.ClassificationCode];
                    }
                    else
                    {
                        invoiceQuantityType = customsItemQueryService.GetQuantityTypeByClassificationCode(SupplierInvoiceItemPM.ClassificationCode, ResolvedTenant());
                        ClasificationQtyTypes.Add(SupplierInvoiceItemPM.ClassificationCode, invoiceQuantityType);
                    }
                }
                if (string.IsNullOrWhiteSpace(invoiceQuantityType) && !string.IsNullOrWhiteSpace(invoiceItem.QUANTITY_TYPE))
                {
                    invoiceQuantityType = TranslateMeasurmentUnit(invoiceItem.QUANTITY_TYPE);
                }
                if (!string.IsNullOrWhiteSpace(invoiceQuantityType))
                {
                    SupplierInvoiceItemPM.InvoiceQuantityType = invoiceQuantityType;
                }

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
                }
                if (!string.IsNullOrWhiteSpace(invoiceItem.ITEMCODE)) SupplierInvoiceItemPM.ItemCode = invoiceItem.ITEMCODE;
                SupplierInvoiceItemPM.Tenant = ResolvedTenant();
                if(SupplierInvoiceItemPM.ChangeSetOp != ChangeSetOperation.Update) SupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
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
                if (!string.IsNullOrWhiteSpace(invoiceItem.PROCESSTYPE))
                {
                    SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesType = new SupplierInvoiceItemProcesTypePM()
                    {
                        DeclarationId = SupplierInvoiceItemPM.DeclarationId,
                        InvoiceItemLineNumber = SupplierInvoiceItemPM.LineNumber,
                        ProcessTypeCode = invoiceItem.PROCESSTYPE,
                        InvoiceCounterKey = SupplierInvoiceItemPM.CounterKey,
                        Tenant = SupplierInvoiceItemPM.Tenant,
                        ChangeSetOp = ChangeSetOperation.Insert,
                    };
                    SupplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesType);
                }
                if (!string.IsNullOrWhiteSpace(invoiceItem.TAXEXEMPTCODE))
                {
                    SupplierInvoiceItemPM.TaxExemptCode = invoiceItem.TAXEXEMPTCODE; //TranslateTaxExemptCode(invoiceItem.TAXEXEMPTCODE);
                }
                if (string.IsNullOrWhiteSpace(SupplierInvoiceItemPM.StatisticQuantityType) && !string.IsNullOrWhiteSpace(invoiceItem.StatisticQuantityType))
                {
                    SupplierInvoiceItemPM.StatisticQuantityType = TranslateMeasurmentUnit(invoiceItem.StatisticQuantityType);
                }
                SupplierInvoiceItemPMList.Add(SupplierInvoiceItemPM);
            }

            return SupplierInvoiceItemPMList;
        }

        private string TranslateTaxExemptCode(string amitalTaxExemptCode)
        {
            if (String.IsNullOrWhiteSpace(amitalTaxExemptCode))
            {
                AppendLogLine("amitalTaxExemptCode is null");
                return null;
            }
            var taxExemptCode = new ValidCustomsItemQueryService(ResolvedTenant());
            var myTaxExemptCode = taxExemptCode.GetSingle(amitalTaxExemptCode, false, true);
            if (myTaxExemptCode == null)
            {
                AppendLogLine("amitalTaxExemptCode = " + amitalTaxExemptCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalTaxExemptCode = " + amitalTaxExemptCode + " Translated to " + myTaxExemptCode.Code);
            return myTaxExemptCode.Code;
        }

        private List<SupplierInvoiceItemsConDeclarPM> GetSupplierInvoiceItemConDeclarsPM(AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {

            var SupplierInvoiceItemConDeclarPMList = new List<SupplierInvoiceItemsConDeclarPM>();
            foreach (var invoiceItemConDeclar in invoiceItem.POINTERS)
            {
                int int1 = 0;
                decimal decimal1 = 0;
                var SupplierInvoiceItemConDeclarPM = new SupplierInvoiceItemsConDeclarPM();

                SupplierInvoiceItemConDeclarPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                SupplierInvoiceItemConDeclarPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                SupplierInvoiceItemConDeclarPM.InvoiceItemLineNumber = supplierInvoiceItemPM.LineNumber;
                if (int.TryParse(invoiceItemConDeclar.LINENUMBER, out int1)) // moran 31.3.16 - AMI-56462
                {
                    SupplierInvoiceItemConDeclarPM.ItemSequence = int1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing COUNTERKEY (" + invoiceItemConDeclar.LINENUMBER + ") into integer");
                }
                SupplierInvoiceItemConDeclarPM.DeclarationTypeCode = "1";

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
                if (!String.IsNullOrWhiteSpace(_LogitudeCommDecFile.ManifestNumber)) // moran 17.7.16 - AMI-57487
                {
                    SupplierInvoiceItemConDeclarPM.DeclarationNumber = _LogitudeCommDecFile.ManifestNumber;
                }
                else
                {
                    if (invoiceItemConDeclar.DECLARATIONID != null)
                    {
                        DeclarationPM EntryDeclarationPM;
                        var myQueryService = new DeclarationQueryService(_context);
                        EntryDeclarationPM = myQueryService.GetSingle(this._LogitudeCommDecFile.EntryFile, true, false);
                        if (EntryDeclarationPM != null)
                        {
                            SupplierInvoiceItemConDeclarPM.DeclarationNumber = EntryDeclarationPM.DeclarationNumber;
                        }
                    }
                }

                SupplierInvoiceItemConDeclarPM.Quantity = supplierInvoiceItemPM.InvoiceQuantity; // moran 27.10.16 - AMI-58519
                SupplierInvoiceItemConDeclarPM.QuantityTypeCode = TranslateMeasurmentUnit(invoiceItemConDeclar.QTY_UNIT); // moran 13.1.16 - AMI-55589

                SupplierInvoiceItemConDeclarPM.Tenant = ResolvedTenant();
                SupplierInvoiceItemConDeclarPM.ChangeSetOp = ChangeSetOperation.Insert;

                SupplierInvoiceItemConDeclarPMList.Add(SupplierInvoiceItemConDeclarPM);
            }
            return SupplierInvoiceItemConDeclarPMList;
        }

        private List<SupplierInvioceItemCertificatPM> GetSupplierInvoiceItemCertificatePM(AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            var SupplierInvoiceItemCertificatePMList = new List<SupplierInvioceItemCertificatPM>();
            foreach (var invoiceItemCert in invoiceItem.CERTIFICATES)
            {
                var SupplierInvoiceItemCertificatePM = new SupplierInvioceItemCertificatPM();

                SupplierInvoiceItemCertificatePM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                SupplierInvoiceItemCertificatePM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                SupplierInvoiceItemCertificatePM.LineNumber = supplierInvoiceItemPM.LineNumber;
                if (!String.IsNullOrWhiteSpace(invoiceItemCert.RESPONSECONFIRMATIONTYPECODE)) // moran 11.7.16 - AMI-57272
                {
                    var confirmationType = new ConfirmationTypeRepository(ResolvedTenant());
                    var myConfirmationType = confirmationType.GetSingle(invoiceItemCert.RESPONSECONFIRMATIONTYPECODE);
                    if (myConfirmationType != null)
                    {
                        SupplierInvoiceItemCertificatePM.ResConfirmationTypeCode = myConfirmationType.Code;
                    }
                }

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
                if (!String.IsNullOrWhiteSpace(invoiceItemCert.CERTIFICATEEXEMPTIONTYPE)) // moran 11.7.16 - AMI-57272
                {
                    var certificateExemptionType = new CertificateExemptionTypeRepository(ResolvedTenant());
                    var myCertificateExemptionType = certificateExemptionType.GetSingle(invoiceItemCert.CERTIFICATEEXEMPTIONTYPE);
                    if (myCertificateExemptionType != null)
                    {
                        SupplierInvoiceItemCertificatePM.CertificateExemptionTypeCode = myCertificateExemptionType.Code;
                    }
                }
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


        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehiclesPM(AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        { // moran 14.3.16 - AMI-55746

            var SupplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();
            foreach (var invoiceItemCar in invoiceItem.CARS)
            {
                int int1 = 0;
                var SupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM();

                SupplierInvoiceItemVehiclePM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;

                SupplierInvoiceItemVehiclePM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
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
                if (invoiceItemCar.CHASSIS != null && invoiceItemCar.VEHICLE_FILE == null)
                {
                    SupplierInvoiceItemVehiclePM.VehicleTypeCode = "CN";
                }
                else if (invoiceItemCar.VEHICLE_FILE != null)
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
            SupplierInvoiceItemVehicleAddPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
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
                    var tempVar = (decimal?)Math.Round(decimal1, 2);
                    SupplierInvoiceItemVehicleAddPM.VehicleValue = tempVar;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing FOB (" + invoiceItemCar.FOB + ") into decimal");
                }
            }
            SupplierInvoiceItemVehicleAddPM.Exempt_type = invoiceItemCar.EXEMPT_TYPE;
            SupplierInvoiceItemVehicleAddPM.ChangeSetOp = ChangeSetOperation.Insert;
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


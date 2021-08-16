using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using UnifreightIIG.Common.ExportDeclarationServiceReference;
using Logitude.Customs.BL.BL;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2757_MSG10004_ExportAmendmentDeclarationResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_2757_MSG10004_ExportDeclarationResponse, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        
        
        private bool isFromImporter;
        string decIdOrg;
        bool isFromAmendment = false;
        SupplierInvoicePM _OrgSupplierInvoicePM;
        bool _isUpdateAfterAccept = false;
        public override void OnRequestFail(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        #region Helpers
        private string GetValueIDType<T>(T codeType) where T : IDType
        {
            if (codeType != null)
                return codeType.Value;
            return null;
        }

        private string GetValueCodeType<T>(T codeType) where T : CodeType
        {
            if (codeType != null)
                return codeType.Value;
            return null;
        }

        private string GetValueTextType<T>(T codeType) where T : TextType
        {
            if (codeType != null)
                return codeType.Value;
            return null;
        }


        private decimal GetValueAmountType<T>(T codeType) where T : AmountType
        {
            if (codeType != null)
                return codeType.Value;
            return 0;
        }

        #endregion

        public DeclarationPM MapResponseToDeclaration(Declaration declaration, int tenant, bool FromImporter, string idOrg, out string error, bool isUpdate = false, string user = null, bool isUpdateAfterAccept=false, bool isCopy =false)
        {
            error = "";
            try
            {
                var context = CustomContext.GetContext(tenant);
                DeclarationRepository declarationRepository = new DeclarationRepository(context);
                var myQueryService = new DeclarationQueryService(context);
                DeclarationPM declarationOrg;
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);
                _isUpdateAfterAccept = isUpdateAfterAccept;

                if (isUpdate)
                {
                    declarationOrg = null;
                }
                else
                {
                    string idDec = myQueryService.GetIdByDeclarationNumber(GetValueIDType(declaration.ID), tenant);
                    declarationOrg = myQueryService.GetDeclarationsByIds(new List<string> { idDec }, tenant).FirstOrDefault();
                    if (declarationOrg == null)
                    {
                        if(isCopy)
                            declarationOrg = myQueryService.GetDeclarationsByIds(new List<string> { idOrg }, tenant).FirstOrDefault();
                        if (declarationOrg == null)
                            declarationOrg = myQueryService.GetAcceptDeclarationAmendment(  idOrg , tenant);
                        if(declarationOrg == null)
                            declarationOrg = myQueryService.GetDeclarationsByIds(new List<string> { idOrg }, tenant).FirstOrDefault();
                        else
                            declarationOrg = myQueryService.GetDeclarationsByIds(new List<string> { declarationOrg.Id }, tenant).FirstOrDefault();
                    }
                    
                    if (declarationOrg.IsAmendment == true && declarationOrg.AmendmentDontDisplayInList == true)
                    {
                        isFromAmendment = true;
                    }
                    decIdOrg = declarationOrg.Id;
                }
            
                isFromImporter = FromImporter;

                List<SupplierInvoicePM> invoicePMs = new List<SupplierInvoicePM>(); ;
                DeclarationPM declarationPM;

                if (declarationOrg != null && !_isUpdateAfterAccept)
                {
                    declarationPM = new DeclarationPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        DeclarationOfficeCode = GetValueIDType(declaration.DeclarationOfficeID),
                        Tenant = tenant,
                        Direction = "E",
                        IsConnectedToUnifreight = false,
                        AmendmentDontDisplayInList = true,
                        IsAmendment = true,
                        ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID),
                        DeclarationTypeCode = GetValueCodeType(declaration.TypeCode),
                        Consignments = GetConsignments(declaration, tenant, null, context),
                    };
                    declarationPM.ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID);
                    declarationPM.ExportFile = declarationOrg.ExportFile;
                    declarationPM.DeclarationTypeCode = GetValueCodeType(declaration.TypeCode);
                    declarationPM.AgentRoleCode = declarationOrg.AgentRoleCode;
                    declarationPM.LoadingFactor = declarationOrg.LoadingFactor;
                    declarationPM.DealValue = declarationOrg.DealValue;
                    declarationPM.CIFValue = declarationOrg.CIFValue;
                    declarationPM.TotalTax = declarationOrg.TotalTax;
                    declarationPM.DealValueWithFactor = declarationOrg.DealValueWithFactor;
                    declarationPM.TaxationDateTime = declarationOrg.TaxationDateTime;
                    declarationPM.DealValueWithoutFactor = declarationOrg.DealValueWithoutFactor;
                    declarationPM.CustomerId = declarationOrg.CustomerId;
                    declarationPM.DestinationCountryCode = declarationOrg.DestinationCountryCode;
                    declarationPM.DepartmentId = declarationOrg.DepartmentId;
                    declarationPM.TransportModeId = declarationOrg.TransportModeId;
                    declarationPM.ReferentUserId = declarationOrg.ReferentUserId;
                    declarationPM.FileState = declarationOrg.FileState;
                    declarationPM.PrimaryInvoiceCounterKey = declarationOrg.PrimaryInvoiceCounterKey;
                    declarationPM.ExcludeConsignment = declarationOrg.ExcludeConsignment;
                    declarationPM.IsDiamondDeclaration = declarationOrg.IsDiamondDeclaration;
                    if (declarationOrg.IsCourierDeclaration)
                    {
                        declarationPM.IsCourierDeclaration = true;
                        declarationPM.CasualSupplierName = declarationOrg.CasualSupplierName;
                        declarationPM.CasualSupplierAddress = declarationOrg.CasualSupplierAddress;
                        declarationPM.ManifestCargoStatusCode = declarationOrg.ManifestCargoStatusCode;
                        declarationPM.ManifestErrorXml = declarationOrg.ManifestErrorXml;
                        declarationPM.CourierHAWB = declarationOrg.CourierHAWB;
                        declarationPM.CourierCustomStatusCode = declarationOrg.CourierCustomStatusCode;
                        declarationPM.CourierSuspentionReasonCode = declarationOrg.CourierSuspentionReasonCode;
                        declarationPM.DealValueWithFactor = declarationOrg.DealValueWithFactor;
                        declarationPM.WeightValue = declarationOrg.WeightValue;
                        declarationPM.CasualImporterAddress1 = declarationOrg.CasualImporterAddress1;
                        declarationPM.CasualImporterAddress2 = declarationOrg.CasualImporterAddress2;
                        declarationPM.CasualImporterCity = declarationOrg.CasualImporterCity;
                        declarationPM.CasualImporterZipCode = declarationOrg.CasualImporterZipCode;
                        declarationPM.CasualImporterFax = declarationOrg.CasualImporterFax;
                        declarationPM.CasualImporterEmail = declarationOrg.CasualImporterEmail;
                        declarationPM.CasualImporterTel = declarationOrg.CasualImporterTel;
                        declarationPM.CasualImporterContact = declarationOrg.CasualImporterContact;
                        declarationPM.PalestinianCode = declarationOrg.PalestinianCode;
                        declarationPM.CourierSuspentionCode = declarationOrg.CourierSuspentionCode;
                    }

                    if (isFromAmendment)
                    {
                        if(!isCopy)
                        {
                            declarationPM.ReplacingRepairRequest = declarationOrg.AmendmentRequestNumber;
                        }
                        declarationPM.AmendmentOriginalDeclartation = declarationOrg.AmendmentOriginalDeclartation;
                        //declarationPM.AmendmentRequestNumber = declarationOrg.AmendmentRequestNumber;
                    }
                    else
                    {
                        if (declarationOrg.IsAmendment == true)
                            declarationPM.AmendmentOriginalDeclartation = declarationOrg.AmendmentOriginalDeclartation;
                        else
                            declarationPM.AmendmentOriginalDeclartation = declarationOrg.Id;
                    }
                }
                else
                {
                    declarationPM = myQueryService.GetSingle(idOrg, true,false);
                    var declarationId2 = declarationRepository.GetLastDeclarationByDeclarationId(declarationPM.AmendmentOriginalDeclartation, tenant).Id;
                    invoicePMs = GetSupplierInvoices(declaration, tenant, context, declarationId2, declarationOrg);
                    DeleteSomeObjects(declarationPM,tenant,context);
                    declarationPM.DeclarationOfficeCode = GetValueIDType(declaration.DeclarationOfficeID);
                    declarationPM.ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID);
                    declarationPM.DeclarationTypeCode = GetValueCodeType(declaration.TypeCode);
                    declarationPM.Tenant = tenant;
                    declarationPM.IsConnectedToUnifreight = false;
                    declarationPM.AmendmentDontDisplayInList = false;
                    declarationPM.IsAmendment = true;
                    declarationPM.Consignments = GetConsignments(declaration, tenant,declarationPM, context);

                    declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                }

                if (isFromImporter)
                {
                    declarationPM.AmendmentCorrectedByUserId = user;
                }
                GetAgent(declaration, ref declarationPM);

                if (declaration.GovernmentProcedure != null)
                {
                    declarationPM.ProcedureCurrentCode = declaration.GovernmentProcedure.CurrentCode.Value;
                }
                if (declaration.DMExtensions != null)
                {
                    declarationPM.DeclarationExportRecipients = GetRecipients(declaration, tenant, declarationPM, context);
                    //declarationPM. = GetClosingDetails(declaration, tenant, declarationPM, context);
                    declarationPM.CustomFileNo = GetValueIDType(declaration.DMExtensions.AgentFileReferenceID);
                    //declarationPM.ExternalDeclarationNumber = GetValueIDType(declaration.DMExtensions.AgentFileReferenceID) + DateTime.Now.Year;
                    declarationPM.ExternalDeclarationNumber = GetValueIDType(declaration.DMExtensions.ExternalDeclarationID);
                    declarationPM.DestinationCountryCode = GetValueCodeType(declaration.DMExtensions.DestinationCountry);
                    //declarationPM.AutonomyRegionTypeCode = GetValueIDType(declaration.DMExtensions.AutonomyRegionType);
                    declarationPM.ExportAutonomyRegionTypeCode = GetValueIDType(declaration.DMExtensions.AutonomyRegionType);
                    if (declaration.DMExtensions.TransferDeclarationToDestinationCountry != null)
                        declarationPM.IsExporterConfirmation = declaration.DMExtensions.TransferDeclarationToDestinationCountry.Value;
                    if (declaration.DMExtensions.ReferenceDateTime != null) 
                        declarationPM.TaxationDateTime = Convert.ToDateTime(declaration.DMExtensions.ReferenceDateTime);

                    if (declaration.PreviousDocument != null)
                    {
                        declarationPM.DeclarationDocumentId = GetValueIDType(declaration.PreviousDocument.ID);
                        declarationPM.DeclarationDocumentTypeCode = GetValueCodeType(declaration.PreviousDocument.TypeCode);
                    }
                    if (declaration.DMExtensions.ExpenseLoadingFactorDetails != null)
                        declarationPM.LoadingFactor = declaration.DMExtensions.ExpenseLoadingFactorDetails.FirstOrDefault()?.ExpenseLoadingFactor.Value;
                 }

                if (declaration.Exporter != null)
                {
                    SetImporters(ref declarationPM, declaration, tenant, context);
                }

                context = CustomContext.GetContext(tenant);
                declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);

                declarationUpdateService.Update(declarationPM, true);

                if (declarationPM.IsCourierDeclaration)
                {
                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);

                    DeclarationCourierStatusPM declarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationOrg.Id, false, false);
                    DeclarationCourierStatusPM declarationCourierStatusPMNew = null;

                    if (!isUpdateAfterAccept)
                    {
                        declarationCourierStatusPMNew = declarationCourierStatusPM;
                        declarationCourierStatusPMNew.DeclarationId = declarationPM.Id;
                    }
                    else
                    {
                        declarationCourierStatusPMNew = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
                        declarationCourierStatusPMNew.IsClosedForFollowUp = declarationCourierStatusPM.IsClosedForFollowUp;
                        declarationCourierStatusPMNew.SpecialActionStatus = declarationCourierStatusPM.SpecialActionStatus;
                        declarationCourierStatusPMNew.FastIndividualProcessCode = declarationCourierStatusPM.FastIndividualProcessCode;
                        declarationCourierStatusPMNew.ManualProcessCode = declarationCourierStatusPM.ManualProcessCode;
                        declarationCourierStatusPMNew.TerminalSuspentionNumber = declarationCourierStatusPM.TerminalSuspentionNumber;
                        declarationCourierStatusPMNew.StorageSiteStatusCode = declarationCourierStatusPM.StorageSiteStatusCode;
                        declarationCourierStatusPMNew.StorageSiteErrorText = declarationCourierStatusPM.StorageSiteErrorText;
                        declarationCourierStatusPMNew.LastMileStatusName = declarationCourierStatusPM.LastMileStatusName;
                        declarationCourierStatusPMNew.LastMileStatusCode = declarationCourierStatusPM.LastMileStatusCode;
                        declarationCourierStatusPMNew.LastMileStatusDate = declarationCourierStatusPM.LastMileStatusDate;
                        declarationCourierStatusPMNew.LastMileStatusRemarks = declarationCourierStatusPM.LastMileStatusRemarks;
                        declarationCourierStatusPMNew.CourierPendingReasonList = declarationCourierStatusPM.CourierPendingReasonList;
                        declarationCourierStatusPMNew.Delivered = declarationCourierStatusPM.Delivered;
                        declarationCourierStatusPMNew.TruckerId = declarationCourierStatusPM.TruckerId;
                        declarationCourierStatusPMNew.DistributionArea = declarationCourierStatusPM.DistributionArea;

                        DeclarationPendingQueryService declarationPendingQueryService = new DeclarationPendingQueryService(context);

                        var declarationPendings = declarationPendingQueryService.GetDeclarationPendingsByDeclarationId(declarationOrg.Id, tenant);

                        foreach (var item in declarationCourierStatusPMNew.DeclarationPendings)
                        {
                            item.ChangeSetOp = ChangeSetOperation.Delete;
                        }

                        foreach (var pending in declarationPendings)
                        {
                            DeclarationPendingPM declarationPendingPM = new DeclarationPendingPM();
                            declarationPendingPM.DeclarationID = declarationCourierStatusPMNew.DeclarationId;
                            declarationPendingPM.ChangeSetOp = ChangeSetOperation.Insert;
                            declarationPendingPM.CourierPendingReasonCode = pending.CourierPendingReasonCode;
                            declarationPendingPM.Tenant = tenant;
                            declarationPendingPM.PendingRemarks = pending.PendingRemarks;
                            declarationPendingPM.Status = pending.Status;
                            declarationCourierStatusPMNew.DeclarationPendings.Add(declarationPendingPM);
                        }

                        DeclarationMamanSpecialActionQueryService declarationMamanSpecialActionQueryService = new DeclarationMamanSpecialActionQueryService(context);

                        DeclarationMamanSpecialActionRepository declarationMamanSpecialActionRepository = new DeclarationMamanSpecialActionRepository(tenant);
                        var declarationMamanSpecialActionList = declarationMamanSpecialActionRepository.GetDeclarationMamanSpecialActionByDeclarationId(declarationCourierStatusPM.DeclarationId, declarationCourierStatusPMNew.Tenant);

                        var declarationMamanSpecialActionListOld = declarationMamanSpecialActionRepository.GetDeclarationMamanSpecialActionByDeclarationId(declarationCourierStatusPMNew.DeclarationId, declarationCourierStatusPMNew.Tenant);

                        foreach (var action in declarationMamanSpecialActionList)
                        {
                            DeclarationMamanSpecialActionPM declarationMamanSpecialActionPM = new DeclarationMamanSpecialActionPM();
                            declarationMamanSpecialActionPM.DeclarationId = declarationCourierStatusPMNew.DeclarationId;
                            declarationMamanSpecialActionPM.MamanLabelText1 = action.MamanLabelText1;
                            declarationMamanSpecialActionPM.MamanLabelText2 = action.MamanLabelText2;
                            declarationMamanSpecialActionPM.MamanLabelText3 = action.MamanLabelText3;
                            declarationMamanSpecialActionPM.MamanLabelText4 = action.MamanLabelText4;
                            declarationMamanSpecialActionPM.MamanLabelText5 = action.MamanLabelText5;
                            declarationMamanSpecialActionPM.MamanSpecialActionCode = action.MamanSpecialActionCode;
                            declarationMamanSpecialActionPM.MamanSpecialActionsErrorXml = action.MamanSpecialActionsErrorXml;
                            declarationMamanSpecialActionPM.MamanSpecialActionStatusCode = action.MamanSpecialActionStatusCode;
                            declarationMamanSpecialActionPM.Tenant = action.Tenant;
                            declarationMamanSpecialActionPM.ChangeSetOp = ChangeSetOperation.Insert;

                            if (declarationMamanSpecialActionListOld.FirstOrDefault(x => x.MamanSpecialActionCode == action.MamanSpecialActionCode) != null)
                                declarationMamanSpecialActionPM.ChangeSetOp = ChangeSetOperation.Update;

                            DeclarationMamanSpecialActionUpdateService declarationMamanSpecialActionUpdateService = new DeclarationMamanSpecialActionUpdateService(context, new Dictionary<string, IContext>(), tenant);

                            declarationMamanSpecialActionUpdateService.Update(declarationMamanSpecialActionPM, true);
                        }
                    }

                    declarationCourierStatusPMNew.ChangeSetOp = ChangeSetOperation.Update;
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), tenant);
                    declarationCourierStatusUpdateService.Update(declarationCourierStatusPMNew, true);
                    CourierDeclarationQueryService courierDeclarationQueryService = new CourierDeclarationQueryService(context);
                    CourierDeclarationUpdateService courierDeclarationUpdateService = new CourierDeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);
                    if(isUpdateAfterAccept)
                    {
                        var courierdeclaration = courierDeclarationQueryService.GetCourierDeclarationByDeclarationId(declarationOrg.Id, tenant);
                        if (courierdeclaration != null)
                            courierdeclaration.ChangeSetOp = ChangeSetOperation.Delete;
                        courierDeclarationUpdateService.Update(courierdeclaration, true);
                        CourierDeclarationPM courierDeclarationPM = new CourierDeclarationPM();
                        courierDeclarationPM.CourierMasterId = courierdeclaration.CourierMasterId;
                        courierDeclarationPM.DeclarationId = declarationCourierStatusPMNew.DeclarationId;
                        courierDeclarationPM.ChangeSetOp = ChangeSetOperation.Insert;
                        courierDeclarationUpdateService.Update(courierDeclarationPM, true);
                    }
                }

                string declarationId;
                if (declarationOrg != null)
                    declarationId = declarationRepository.GetLastDeclarationByDeclarationId(declarationPM.AmendmentOriginalDeclartation, tenant).Id;
                else
                    declarationId = declarationPM.Id;
            
                declarationPM.DeclarationTaxes = GetDeclarationTaxesPM(declaration, declarationOrg, declarationId, tenant);

                if(isUpdateAfterAccept)
                {
                    declarationPM.SupplierInvoices = invoicePMs;
                }
                else
                {
                    declarationPM.SupplierInvoices = GetSupplierInvoices(declaration, tenant, context, declarationId, declarationOrg);
                }
                declarationPM.ChangeSetOp = ChangeSetOperation.Update;
                declarationPM.Consignments.ForEach(x => x.ChangeSetOp = ChangeSetOperation.None);
                declarationPM.DeclarationExportRecipients.ForEach(x => x.ChangeSetOp = ChangeSetOperation.None);
                declarationUpdateService.Update(declarationPM, true);
                CustomsDocumentsTicketQueryService customsDocumentsTicketQuery = new CustomsDocumentsTicketQueryService(tenant);
                if(declarationOrg!=null && !isUpdateAfterAccept)
                {
                    List<CustomsDocumentsTicketPM> customsDocumentsTicketPMs = customsDocumentsTicketQuery.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationOrg.Id, "", "", "", tenant, "Declaration");
                    CustomsDocumentsTicketUpdateService customsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(context, new Dictionary<string, IContext>(), declarationPM.Tenant);

                    foreach (var customsDocumentsTicketPM in customsDocumentsTicketPMs)
                    {
                        customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Insert;
                        foreach (var CustomsDocumentPointer in customsDocumentsTicketPM.CustomsDocumentPointers)
                        {
                            CustomsDocumentPointer.ChangeSetOp = ChangeSetOperation.Insert;
                            CustomsDocumentPointer.ParentEntityId = declarationId;
                            CustomsDocumentPointer.CustomsDocumentsTicketId = null;
                        }
                        customsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);
                    }
                }
                return myQueryService.GetSingleDeclarationById(declarationPM.Id, declarationPM.Tenant);
            }
            catch (System.Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        private void DeleteSomeObjects(DeclarationPM declarationPM, int tenant, ICustomContext context)
        {
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(tenant);
            declarationUpdateService.DeclarationConsignmentsFastDelete(declarationPM, context);
            declarationUpdateService.DeclarationSupplierInvoicesFastDelete(declarationPM, context);
            
            var mySupplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(context, new Dictionary<string, IContext>(), tenant);
            var mySupplierInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 20.10.15 - Task 17209 
            var mySupplierInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424 
                                                                                                                                                                    //var mySupplierInvoiceItemsTaxesModificationUpdateService = new SupplierInvoiceItemsTaxesModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(context, new Dictionary<string, IContext>(), tenant);

            var myDeclarationKeys = new DeclarationKeys { Id = declarationPM.Id };
            myDeclarationTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemVehicleModUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemModVehicleUpdateService.FastDeleteComposition(myDeclarationKeys);
            
            (context as DbContextBase).SaveChanges();
        }

        private void SetImporters(ref DeclarationPM declarationPM, Declaration declaration, int tenant, ICustomContext context)
        {
            foreach (var importer in declaration.Exporter)
            {
                if (importer.ID != null)
                {
                    switch (GetValueCodeType(importer.DMExtensions.RoleCode))
                    {
                        case "7":
                            {
                                declarationPM.ImporterTypeCode = importer.ID.schemeID;
                                declarationPM.ImporterAddress = importer.DMExtensions.Address;
                                declarationPM.ImporterName = importer.DMExtensions.Name;
                                //declarationPM.MainImporterEntitlemntTypeCode = GetValueCodeType(importer.DMExtensions.RoleCode.EntitlementTypeCode);
                                if (importer.ID.schemeID == "2" || importer.ID.schemeID == "3") declarationPM.ImporterPassCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);

                                switch (importer.ID.schemeID)
                                {
                                    case "1":
                                        {
                                            var queryService = new ClientQueryService(context);
                                            var importerPM = queryService.GetClientByCode(importer.ID.Value, tenant);
                                            if (importerPM == null)
                                                declarationPM.ImporterCode = importer.ID.Value;
                                            else
                                            {
                                                declarationPM.ImporterCode = importerPM.Code;
                                                declarationPM.ImporterId = importerPM.Id;
                                            }
                                            break;
                                        }
                                    case "3":
                                    case "2":
                                        {
                                            declarationPM.ImporterPassportNumber = importer.ID.Value;
                                            break;
                                        }
                                }
                                break;
                            }
                        case "12":
                            {
                                switch (importer.ID.schemeID)
                                {
                                    case "1":
                                        {
                                            var queryService = new ClientQueryService(context);
                                            var importerPM = queryService.GetClientByCode(importer.ID.Value, tenant);
                                            if (importerPM == null)
                                                declarationPM.TransferImporterCode = importer.ID.Value;
                                            else
                                            {
                                                declarationPM.TransferImporterCode = importerPM.Code;
                                                declarationPM.TransferImporterId = importerPM.Id;
                                            }
                                            break;
                                        }
                                    case "3":
                                    case "2":
                                        {
                                            declarationPM.TransferPassportNumber = importer.ID.Value;
                                            break;
                                        }
                                }
                                declarationPM.TransferImporterTypeCode = importer.ID.schemeID;
                                declarationPM.TransferImporterAddress = importer.DMExtensions.Address;
                                declarationPM.TransferImporterName = importer.DMExtensions.Name;
                                //declarationPM.TransImporterEntitleTypeCode = GetValueCodeType(importer.DMExtensions.EntitlementTypeCode);
                                if (importer.ID.schemeID == "2" || importer.ID.schemeID == "3") declarationPM.TransferImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);
                                break;
                            }
                        case "6":
                            {
                                switch (importer.ID.schemeID)
                                {
                                    case "1":
                                        {
                                            var queryService = new ClientQueryService(context);
                                            var importerPM = queryService.GetClientByCode(importer.ID.Value, tenant);
                                            if (importerPM == null)
                                                declarationPM.EntitleImporterCode = importer.ID.Value;
                                            else
                                            {
                                                declarationPM.EntitleImporterCode = importerPM.Code;
                                                declarationPM.EntitleImporterId = importerPM.Id;
                                            }
                                             break;
                                        }
                                    case "3":
                                    case "2":
                                        {
                                            declarationPM.EntitlePassportNumber = importer.ID.Value;
                                            break;
                                        }
                                }
                                declarationPM.EntitleImporterTypeCode = importer.ID.schemeID;
                                declarationPM.EntitleImporterAddress = importer.DMExtensions.Address;
                                declarationPM.EntitleImporterName = importer.DMExtensions.Name;
                                //declarationPM.ImporterEntitlementTypeCode = GetValueCodeType(importer.DMExtensions.EntitlementTypeCode);
                                if (importer.ID.schemeID == "2" || importer.ID.schemeID == "3") declarationPM.EntitleImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);
                                break;
                            }
                    }
                }
                else
                {
                    if (importer.DMExtensions != null)
                    {
                        switch (importer.DMExtensions.RoleCode.Value)
                        {
                            case "7":
                                {
                                    declarationPM.ImporterAddress = importer.DMExtensions.Address;
                                    declarationPM.ImporterName = importer.DMExtensions.Name;
                                    //   declarationPM.impo = GetValueTextType(importer.DMExtensions.IssueLocation);
                                    break;
                                }
                            case "12":
                                {
                                    declarationPM.TransferImporterAddress = importer.DMExtensions.Address;
                                    declarationPM.TransferImporterName = importer.DMExtensions.Name;
                                    declarationPM.TransferImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);
                                    break;
                                }
                            case "6":
                                {  //  declarationPM.EntitleImporterTypeCode = "4";
                                    declarationPM.EntitleImporterAddress = importer.DMExtensions.Address;
                                    declarationPM.EntitleImporterName = importer.DMExtensions.Name;
                                    declarationPM.EntitleImporterCountryCode = GetValueTextType(importer.DMExtensions.IssueLocation);
                                    break;
                                }
                        }
                    }
                }
            }

        }

        private ExportDeclarationClosingDataPM GetClosingDetails(Declaration declaration, int tenant, DeclarationPM declarationPM, ICustomContext context)
        {
            var closingDetails = new ExportDeclarationClosingDataPM();
            if (declaration.DMExtensions.DeclarationClosingDetails != null)
            {
                closingDetails.FinalShipCode = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalShipID);
                closingDetails.FinalLoadingSite = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalLoadingSite);
                if (declaration.DMExtensions.DeclarationClosingDetails.DepartureDateTime != null)
                {
                    closingDetails.LoadingDateTime = declaration.DMExtensions.DeclarationClosingDetails.DepartureDateTime.Value;
                }
                closingDetails.FinalManifestNumber = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.FirstCargoID);
                closingDetails.FinalCargoTypeCode = GetValueCodeType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.TypeCode);
                closingDetails.FinalSecondCargoId = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.SecondCargoID);
                closingDetails.FinalThirdCargoId = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.ThirdCargoID);
            }
            return closingDetails;
        }
        private List<DeclarationExportRecipientPM> GetRecipients(Declaration declaration, int tenant, DeclarationPM declarationPM, ICustomContext context)
        {
            List<DeclarationExportRecipientPM> recipientPMs = new List<DeclarationExportRecipientPM>();
            if (declaration.DMExtensions.RecipientDetails != null && declaration.DMExtensions.RecipientDetails.Count() > 0)
            {
                foreach (var declarationExportRecipient in declaration.DMExtensions.RecipientDetails)
                {
                    DeclarationExportRecipientPM recipientPM = new DeclarationExportRecipientPM();
                    recipientPM.Tenant = tenant;
                    recipientPM.RecipientName = declarationExportRecipient.Name;
                    recipientPM.RecipientAddress = declarationExportRecipient.Address;
                    recipientPM.RecipientIssueCountryCode = GetValueCodeType(declarationExportRecipient.IssueLocation);
                    recipientPM.ChangeSetOp = ChangeSetOperation.Insert;
                    recipientPMs.Add(recipientPM);
                }
            }
            return recipientPMs;
        }

        private List<ConsignmentPM> GetConsignments(Declaration declaration, int tenant , DeclarationPM declarationPM, ICustomContext context)
        {
            if (declaration.GoodsShipment == null || declaration.GoodsShipment.Count() == 0) return null;
            if ((declaration.GoodsShipment[0].ExportConsignment == null && declaration.GoodsShipment[0].ImportConsignment == null) || 
                (declaration.GoodsShipment[0].ExportConsignment.Count() == 0 && declaration.GoodsShipment[0].ImportConsignment.Count() == 0)) return null;

            List<ConsignmentPM> consignmentPMs = new List<ConsignmentPM>();
            foreach (var consignment in declaration.GoodsShipment[0].ExportConsignment)
            {
                ConsignmentPM consignmentPM = new ConsignmentPM();
                consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPM.Tenant = tenant;
                consignmentPM.ConsignmentType = "E";
                var consignmentQueryService = new ConsignmentQueryService(context);
                
                if(declarationPM!= null)
                {
                    var maxCounter = consignmentQueryService.GetMaxCounterKey(declarationPM.Id, tenant) ?? 0;
                    consignmentPM.SequenceNumeric = maxCounter + 1;
                }
                if (consignment.TransportContractDocument != null)
                {
                    consignmentPM.CargoTypeCode = GetValueCodeType(consignment.TransportContractDocument.TypeCode);
                    //if (consignment.TransportContractDocument.IssueDateTime != null) consignmentPM.ManifestDate = Convert.ToDateTime(consignment.TransportContractDocument.IssueDateTime);
                    consignmentPM.ManifestNumber = GetValueIDType(consignment.TransportContractDocument.ID);
                    if (consignment.TransportContractDocument.DMExtensions != null)
                    {
                        consignmentPM.SecondCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.SecondCargoID);
                        consignmentPM.ThirdCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.ThirdCargoID);
                    }
                }
                if (consignment.UnloadingLocation != null)
                {
                    consignmentPM.ExportUnloadingPortCode = GetValueIDType(consignment.UnloadingLocation.ID);
                    //if (consignment.UnloadingLocation.ArrivalDateTime != null)
                    //{
                    //    consignmentPM.UnloadDate = Convert.ToDateTime(consignment.UnloadingLocation.ArrivalDateTime);
                    //}
                }
                if (consignment.LoadingLocation != null)
                {
                    consignmentPM.ExportLoadingPortCode = GetValueIDType(consignment.LoadingLocation.ID);
                }
                if (consignment.DMExtensions != null)
                {
                    consignmentPM.CargoDescription = GetValueTextType(consignment.DMExtensions.CargoDescription);
                    //if (consignment.DMExtensions.LastReleaseFromWarehousInd != null)
                    //{
                    //    if (consignment.DMExtensions.LastReleaseFromWarehousInd.Value == true)
                    //        consignmentPM.IsLastReleaseFromWarehous = "T";
                    //    else
                    //        consignmentPM.IsLastReleaseFromWarehous = "F";
                    //}
                    //else
                    //{
                    //    consignmentPM.IsLastReleaseFromWarehous = "N";
                    //}
                    //consignmentPM.OriginCountryCode = GetValueCodeType(consignment.DMExtensions.ExportationCountryCode);

                    if (consignment.DMExtensions.RegisteredFacility != null && consignment.DMExtensions.RegisteredFacility.Count() > 0)
                    {
                        foreach (var registeredFacility in consignment.DMExtensions.RegisteredFacility)
                        {
                            switch (GetValueCodeType(registeredFacility.FacilityType))
                            {
                                case "004":
                                    {
                                        consignmentPM.StorageSiteCode = GetValueIDType(registeredFacility.ID);
                                        break;
                                    }
                                case "003":
                                    {
                                        consignmentPM.ReceiverWarehouseCode = GetValueIDType(registeredFacility.ID);
                                        break;
                                    }
                                case "005":
                                    {
                                        consignmentPM.ConsignmentInternalTransitions = new List<ConsignmentInternalTransitionPM>()
                                        {
                                            new ConsignmentInternalTransitionPM (){
                                       ///   DeclarationId = GetValueIDType(declaration.ID),
                                          ChangeSetOp = ChangeSetOperation.Insert,
                                          SiteCode=GetValueIDType(registeredFacility.ID)
                                        }
                                        };
                                        break;
                                    }
                            }
                        }
                    }
                }

                if (consignment.DMExtensions.PackagesMeasure != null)
                {
                    consignmentPM.ConsignmentPackages = GetConsignmentPackages(consignment.DMExtensions.PackagesMeasure, declaration, tenant);
                }

                consignmentPMs.Add(consignmentPM);
            }
            if (declaration.GoodsShipment[0].ImportConsignment != null)
            {
                foreach (var consignment in declaration.GoodsShipment[0].ImportConsignment)
                {
                    ConsignmentPM consignmentPM = new ConsignmentPM();
                    consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                    consignmentPM.Tenant = tenant;
                    consignmentPM.ConsignmentType = "I";
                    var consignmentQueryService = new ConsignmentQueryService(context);

                    if (declarationPM != null)
                    {
                        var maxCounter = consignmentQueryService.GetMaxCounterKey(declarationPM.Id, tenant) ?? 0;
                        consignmentPM.SequenceNumeric = maxCounter + 1;
                    }
                    if (consignment.TransportContractDocument != null)
                    {
                        consignmentPM.CargoTypeCode = GetValueCodeType(consignment.TransportContractDocument.TypeCode);
                        //if (consignment.TransportContractDocument.IssueDateTime != null) consignmentPM.ManifestDate = Convert.ToDateTime(consignment.TransportContractDocument.IssueDateTime);
                        consignmentPM.ManifestNumber = GetValueIDType(consignment.TransportContractDocument.ID);
                        if (consignment.TransportContractDocument.DMExtensions != null)
                        {
                            consignmentPM.SecondCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.SecondCargoID);
                            consignmentPM.ThirdCargoID = GetValueIDType(consignment.TransportContractDocument.DMExtensions.ThirdCargoID);
                        }
                    }
                    if (consignment.UnloadingLocation != null)
                    {
                        consignmentPM.UnloadPortCode = GetValueIDType(consignment.UnloadingLocation.ID);
                        
                    }
                    if (consignment.LoadingLocation != null)
                    {
                        consignmentPM.LoadingPortCode = GetValueIDType(consignment.LoadingLocation.ID);
                    }
                    if (consignment.DMExtensions != null)
                    {
                        consignmentPM.CargoDescription = GetValueTextType(consignment.DMExtensions.CargoDescription);
                        if (consignment.DMExtensions.LastReleaseFromWarehousInd != null)
                        {
                            if (consignment.DMExtensions.LastReleaseFromWarehousInd.Value == true)
                                consignmentPM.IsLastReleaseFromWarehous = "T";
                            else
                                consignmentPM.IsLastReleaseFromWarehous = "F";
                        }
                        else
                        {
                            consignmentPM.IsLastReleaseFromWarehous = "N";
                        }
                        consignmentPM.OriginCountryCode = GetValueCodeType(consignment.DMExtensions.ExportationCountryCode);
                        if (consignment.DMExtensions.RegisteredFacility != null && consignment.DMExtensions.RegisteredFacility.Count() > 0)
                        {
                            foreach (var registeredFacility in consignment.DMExtensions.RegisteredFacility)
                            {
                                switch (GetValueCodeType(registeredFacility.FacilityType))
                                {
                                    case "004":
                                        {
                                            consignmentPM.StorageSiteCode = GetValueIDType(registeredFacility.ID);
                                            break;
                                        }
                                    case "003":
                                        {
                                            consignmentPM.ReceiverWarehouseCode = GetValueIDType(registeredFacility.ID);
                                            break;
                                        }
                                    case "005":
                                        {
                                            consignmentPM.ConsignmentInternalTransitions = new List<ConsignmentInternalTransitionPM>()
                                        {
                                            new ConsignmentInternalTransitionPM (){
                                       ///   DeclarationId = GetValueIDType(declaration.ID),
                                          ChangeSetOp = ChangeSetOperation.Insert,
                                          SiteCode=GetValueIDType(registeredFacility.ID)
                                        }
                                        };
                                            break;
                                        }
                                }
                            }
                        }
                    }
                    //if (consignment.DMExtensions.PackagesMeasure != null)
                    //{
                    //    consignmentPM.ConsignmentPackages = GetConsignmentPackages(consignment.DMExtensions.PackagesMeasure, declaration, tenant);
                    //}

                    consignmentPMs.Add(consignmentPM);
                }
            }
            return consignmentPMs;
        }

        private List<ConsignmentPackagePM> GetConsignmentPackages(DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasure[] packagesMeasures, Declaration declaration, int tenant)
        {
            List<ConsignmentPackagePM> consignmentPackagePMs = new List<ConsignmentPackagePM>();
            foreach (var packagesMeasure in packagesMeasures)
            {
                ConsignmentPackagePM consignmentPackagePM = new ConsignmentPackagePM();
                //  consignmentPackagePM.DeclarationId = GetValueIDType(declaration.ID);
                consignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPackagePM.PackageMeasureQualifierCode = GetValueCodeType(packagesMeasure.PackageMeasureQualifier);
                consignmentPackagePM.PackageQuantityTypeCode = packagesMeasure.TotalPackageQuantity.unitCode.ToString();
                consignmentPackagePM.PackageQuantity = Convert.ToInt32(packagesMeasure.TotalPackageQuantity.Value);
                consignmentPackagePM.GrossMassMeasureTypeCode = packagesMeasure.GrossMassMeasure.unitCode.ToString();
                if (packagesMeasure.GrossMassMeasure != null) consignmentPackagePM.GrossMassMeasure = packagesMeasure.GrossMassMeasure.Value;
                consignmentPackagePM.PackageTypeCode = GetValueCodeType(packagesMeasure.TypeCode);
                consignmentPackagePM.MarksNumbers = GetValueTextType(packagesMeasure.MarksNumbers);
                consignmentPackagePM.Tenant = tenant;
                consignmentPackagePMs.Add(consignmentPackagePM);
            }
            return consignmentPackagePMs;
        }

        private string GetAgent(Declaration declaration, ref DeclarationPM declarationPM)
        {
            if (declaration.Agent != null && declaration.Agent.Count() > 0)
            {
                var agent = declaration.Agent.FirstOrDefault(x => GetValueCodeType(x.RoleCode) != "1");
                if (agent != null)
                {
                    declarationPM.AgentId = GetValueIDType(agent.ID);
                }
            }
            return null;
        }

        private List<SupplierInvoicePM> GetSupplierInvoices(Declaration declaration, int tenant, ICustomContext context, string declarationId, DeclarationPM declarationPMOrg)
        {
            List<SupplierInvoicePM> supplierInvoicePMs = new List<SupplierInvoicePM>();

            if(_isUpdateAfterAccept)
            {
                //var declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424 
                //declarationUpdateService.DeclarationSupplierInvoicesFastDelete(_MyDeclarationPM, context);
            }

            foreach (var item in declaration.GoodsShipment.OrderBy(x => x.SequenceNumeric))
            {
                SupplierInvoicePM supplierInvoicePM = new SupplierInvoicePM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    SequenceNumeric = (int)item.SequenceNumeric,
                    InvoiceNumber = GetValueIDType(item.Invoice.ID),
                    AccountTypeCode = GetValueCodeType(item.Invoice.TypeCode),
                    DeclarationId = declarationId,
                    Tenant = tenant,
                };
                SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(tenant);
                _OrgSupplierInvoicePM = supplierInvoiceQueryService.GetSupplierInvoiceBySequenceNumber(decIdOrg, (int)supplierInvoicePM.SequenceNumeric, 0, 0);

                if (item.Invoice.IssueDateTime != null) supplierInvoicePM.IssueDate = Convert.ToDateTime(item.Invoice.IssueDateTime);

                if (item.Invoice.DMExtensions != null)
                {
                    if (item.Invoice.DMExtensions.IsPreferenceDocumentInd != null) supplierInvoicePM.IsPreference = item.Invoice.DMExtensions.IsPreferenceDocumentInd.Value;
                    supplierInvoicePM.PreferenceDocumentTypeCode = GetValueCodeType(item.Invoice.DMExtensions.PreferenceDocumentType);
                    //supplierInvoicePM.PaymentTypeCode = GetValueCodeType(item.Invoice.DMExtensions.PaymentDetails.FirstOrDefault().PaymentType);
                    supplierInvoicePM.InvoiceAmount = GetValueAmountType(item.Invoice.DMExtensions.InvoiceAmount);
                    supplierInvoicePM.PartyRelationshipCode = GetValueCodeType(item.Invoice.DMExtensions.PartyRelationshipCode);
                    if (item.Invoice.DMExtensions.InvoiceAmount != null) supplierInvoicePM.InvoiceCurrencyTypeCode = item.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                    if (item.Invoice.DMExtensions.PaymentDetails != null && item.Invoice.DMExtensions.PaymentDetails.Count() > 0)
                    {
                        List<SupplierInvoicePaymentPM> paymentPMs = new List<SupplierInvoicePaymentPM>();
                        foreach (var payment in item.Invoice.DMExtensions.PaymentDetails)
                        {
                            var paymentPM = new SupplierInvoicePaymentPM
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                Tenant = tenant,
                                DeclarationId = declarationId,
                                SequenceNumeric = (int)payment.SequenceNumeric,
                                PaymentTypeCode = GetValueCodeType(payment.PaymentType),
                                PaymentAmount = payment.PaymentAmount.Value,
                            };
                            paymentPMs.Add(paymentPM);
                        }
                        supplierInvoicePM.SupplierInvoicePayments = paymentPMs;
                    }
                }
                if (item.Invoice.DMExtensions.BuyerDetails != null)
                {
                    supplierInvoicePM.BuyerAddress = item.Invoice.DMExtensions.BuyerDetails.Address;
                    supplierInvoicePM.BuyerName = item.Invoice.DMExtensions.BuyerDetails.Name;
                    supplierInvoicePM.BuyerRoleCode = GetValueCodeType(item.Invoice.DMExtensions.BuyerDetails.RoleCode);
                    supplierInvoicePM.BuyerCountryCode = GetValueCodeType(item.Invoice.DMExtensions.BuyerDetails.IssueLocation);
                }
                if (item.TradeTerms != null)
                {
                    supplierInvoicePM.IncotermCode = GetValueCodeType(item.TradeTerms.ConditionCode);
                    //supplierInvoicePM.IssueCountryCode = GetValueIDType(item...LocationID);
                }

                supplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModifications(item, ref supplierInvoicePM, declaration, declarationId, tenant);
                //supplierInvoicePM.SupplierInvoiceFreightAmounts = GetSupplierInvoiceFreightAmounts(ref supplierInvoicePM, item, tenant);
                SupplierInvoicePM SupplierInvoicePMOrg;
                if(declarationPMOrg!=null)
                {
                    SupplierInvoicePMOrg = declarationPMOrg.SupplierInvoices.FirstOrDefault(x => x.SequenceNumeric == supplierInvoicePM.SequenceNumeric);
                }
                else
                {
                    SupplierInvoicePMOrg = null;
                }
                if (SupplierInvoicePMOrg != null)
                    supplierInvoicePM.InvoiceCounterKey = SupplierInvoicePMOrg.InvoiceCounterKey;

                supplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItems(item, declaration, declarationId, tenant, supplierInvoicePM, context, SupplierInvoicePMOrg);
                supplierInvoicePMs.Add(supplierInvoicePM);
            }
            return supplierInvoicePMs;
        }


        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItems(DeclarationGoodsShipment item, Declaration declaration, string declarationId, int tenant, SupplierInvoicePM supplierInvoicePM, ICustomContext context, SupplierInvoicePM supplierInvoicePMPMOrg)
        {
            List<SupplierInvoiceItemPM> supplierInvoiceItemPMs = new List<SupplierInvoiceItemPM>();
            if(item.GovernmentAgencyGoodsItem!=null)
            {
                foreach (var governmentAgencyGoodsItem in item.GovernmentAgencyGoodsItem)
                {
                    SupplierInvoiceItemPM supplierInvoiceItemPM = new SupplierInvoiceItemPM();
                    //SupplierInvoiceItemPM supplierInvoiceItemPMOrg = new SupplierInvoiceItemPM();

                    //supplierInvoiceItemPMOrg = declarationPMOrg.SupplierInvoices.FirstOrDefault(x=>x.DeclarationId == )

                    supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemPM.DeclarationId = declarationId;
                    supplierInvoiceItemPM.SequenceNumeric = (int)governmentAgencyGoodsItem.SequenceNumeric;
                    supplierInvoiceItemPM.OriginCountryCode = GetValueCodeType(governmentAgencyGoodsItem.Origin.CountryCode);
                    if (item.Invoice.DMExtensions.InvoiceAmount != null) supplierInvoiceItemPM.ItemPriceCurrencyCode = item.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                      
                    supplierInvoiceItemPM.Tenant = tenant;
                    //if (governmentAgencyGoodsItem.Commodity.DMExtensions != null)
                    //{
                    //    supplierInvoiceItemPM.TradeAgreementCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.DMExtensions.DutyRegimeCode);

                    //}
                    if (governmentAgencyGoodsItem.Commodity.Classification != null || governmentAgencyGoodsItem.Commodity.Classification.Count() > 0)
                    {
                        var classification = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x != null && GetValueCodeType(x.IdentificationTypeCode) == "SSO");

                        if (classification != null)
                        {
                            supplierInvoiceItemPM.DangerousClassificationCode = GetValueIDType(classification.ID);
                            //supplierInvoiceItemPM.DangerousPackingGroupTypeCode = GetValueCodeType(classification.DMExtensions..DangerousGoodsPackingRequirementsGroupCode);
                        }
                        var classification2 = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x != null && GetValueCodeType(x.IdentificationTypeCode) == "HS");

                        if (classification2 != null)
                        {
                            supplierInvoiceItemPM.ClassificationCode = GetValueIDType(classification2.ID).Replace("/", "");
                        }
                    }

                    //if (governmentAgencyGoodsItem.Commodity.GovernmentProcedure != null && governmentAgencyGoodsItem.Commodity.GovernmentProcedure.Count() > 0)
                    //{
                    //    supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes = new List<SupplierInvoiceItemProcesTypePM>();


                    //    foreach (var governmentProcedure in governmentAgencyGoodsItem.Commodity.GovernmentProcedure)
                    //    {
                    //        SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM();
                    //        supplierInvoiceItemProcesTypePM.ChangeSetOp = ChangeSetOperation.Insert;
                    //        supplierInvoiceItemProcesTypePM.DeclarationId = declarationId;
                    //        supplierInvoiceItemProcesTypePM.Tenant = tenant;
                    //        supplierInvoiceItemProcesTypePM.ProcessTypeCode = GetValueCodeType(governmentProcedure.CurrentCode);

                    //        supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesTypePM);
                    //    }




                    //}
                    foreach (var goodsMeasure in governmentAgencyGoodsItem.GoodsMeasure)
                    {
                        if (goodsMeasure.DMExtensions != null && goodsMeasure.TariffQuantity != null)
                        {
                            switch (goodsMeasure.DMExtensions.MeasureQualifier.Value)
                            {
                                case "1":
                                    {
                                        supplierInvoiceItemPM.InvoiceQuantityType = goodsMeasure.TariffQuantity.unitCode.ToString();
                                        supplierInvoiceItemPM.InvoiceQuantity = goodsMeasure.TariffQuantity.Value;
                                        break;
                                    }
                                case "2":
                                    {
                                        supplierInvoiceItemPM.StatisticQuantityType = goodsMeasure.TariffQuantity.unitCode.ToString();
                                        supplierInvoiceItemPM.StatisticQuantity = goodsMeasure.TariffQuantity.Value;
                                        break;
                                    }
                                case "3":
                                    {
                                        supplierInvoiceItemPM.AdditionalQuantityType = goodsMeasure.TariffQuantity.unitCode.ToString();
                                        supplierInvoiceItemPM.AdditionalQuantity = goodsMeasure.TariffQuantity.Value;
                                        break;
                                    }
                            }
                        }
                    }

                    supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars = new List<SupplierInvoiceItemsConDeclarPM>();

                    if (governmentAgencyGoodsItem.PreviousDocument != null && governmentAgencyGoodsItem.PreviousDocument.Count() > 0)
                    {
                        foreach (var previousDocument in governmentAgencyGoodsItem.PreviousDocument)
                        {
                            SupplierInvoiceItemsConDeclarPM supplierInvoiceItemsConDeclarPM = new SupplierInvoiceItemsConDeclarPM();
                            supplierInvoiceItemsConDeclarPM.ChangeSetOp = ChangeSetOperation.Insert;
                            supplierInvoiceItemsConDeclarPM.DeclarationNumber = GetValueIDType(previousDocument.ID);
                            supplierInvoiceItemsConDeclarPM.ItemSequence = (int)previousDocument.SequenceNumeric;
                            supplierInvoiceItemsConDeclarPM.DeclarationTypeCode = GetValueCodeType(previousDocument.TypeCode);
                            supplierInvoiceItemsConDeclarPM.Tenant = tenant;
                            if (previousDocument.DMExtensions != null)
                            {
                                supplierInvoiceItemsConDeclarPM.QuantityTypeCode = previousDocument.DMExtensions.QuantityQuantity.unitCode.ToString();
                                supplierInvoiceItemsConDeclarPM.Quantity = previousDocument.DMExtensions.QuantityQuantity.Value;
                                supplierInvoiceItemsConDeclarPM.InvoiceNumber = ((int?)previousDocument.DMExtensions.SequenceNumeric);
                            }
                            supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars.Add(supplierInvoiceItemsConDeclarPM);
                        }
                    }
                    //if (governmentAgencyGoodsItem.Manufacturer != null)
                    //    supplierInvoiceItemPM.ManufactureIdentifier = GetValueIDType(governmentAgencyGoodsItem.Manufacturer.ID);

                    if (governmentAgencyGoodsItem.DMExtensions != null)
                    {
                        if (governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null && governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount.Count() > 0)
                        {
                            foreach (var goodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                            {
                                //    var success = Enum.TryParse<ISO3AlphaCurrencyCodeContentType>(CurrencyCode, out isoCurrency);
                                if (goodsItemAmount.CustomsValueAmount != null)
                                    switch (GetValueCodeType(goodsItemAmount.AmountType))
                                    {
                                        //case "3":
                                        //    {
                                        //        if (!isFromImporter)
                                        //        {
                                        //            if (item.Invoice != null && item.Invoice.DMExtensions != null && item.Invoice.DMExtensions.InvoiceAmount != null)
                                        //            {
                                        //                if (goodsItemAmount.CustomsValueAmount.currencyID.ToString() == item.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString())
                                        //                {
                                        //                    supplierInvoiceItemPM.ItemPrice = GetValueAmountType(goodsItemAmount.CustomsValueAmount);
                                        //                    supplierInvoiceItemPM.ItemPriceCurrencyCode = goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                        //                }
                                        //            }
                                        //        }
                                        //        break;

                                        //    }
                                        case "1":
                                            {
                                                    if (item.Invoice != null && item.Invoice.DMExtensions != null && item.Invoice.DMExtensions.InvoiceAmount != null)
                                                    {
                                                        if (goodsItemAmount.CustomsValueAmount.currencyID.ToString() == item.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString())
                                                        {
                                                            supplierInvoiceItemPM.ItemPrice = GetValueAmountType(goodsItemAmount.CustomsValueAmount);
                                                            supplierInvoiceItemPM.ItemPriceCurrencyCode = goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                                        }
                                                    }
                                                break;
                                            }
                                        case "11":
                                            {
                                                supplierInvoiceItemPM.NonCustomsItemPrice = GetValueAmountType(goodsItemAmount.CustomsValueAmount);
                                                supplierInvoiceItemPM.NonCustomsItemPriceCurCode = goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                                break;
                                            }
                                        case "5":
                                            {
                                                supplierInvoiceItemPM.WholeSaleItemPrice = GetValueAmountType(goodsItemAmount.CustomsValueAmount);
                                                supplierInvoiceItemPM.WholeSaleItemPriceCurrencyCode = goodsItemAmount.CustomsValueAmount.currencyID.ToString();
                                                break;
                                            }
                                    }
                            }
                        }
                        //supplierInvoiceItemPM.CustomsBookTypeCode = GetValueCodeType(governmentAgencyGoodsItem.DMExtensions..CustomsBookType);
                        //supplierInvoiceItemPM.TaxExemptCode = GetValueCodeType(governmentAgencyGoodsItem.DMExtensions.TaxExemptCode);
                        //if (governmentAgencyGoodsItem.DMExtensions.OptionalTama != null) supplierInvoiceItemPM.OptionalTamaPercentage = governmentAgencyGoodsItem.DMExtensions.OptionalTama.Value;
                        if (isFromImporter)
                        {
                            SupplierInvoiceItemVehicleQueryService supplierInvoiceItemVehicleQueryService = new SupplierInvoiceItemVehicleQueryService(context);

                            SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);

                            var invoiceItem = supplierInvoiceItemQueryService.GetSingleSupplierInvoicePMBySequence(decIdOrg, Convert.ToInt32(supplierInvoicePM.InvoiceCounterKey), Convert.ToInt32(supplierInvoiceItemPM.SequenceNumeric));
                         if(invoiceItem!= null)
                            {
                                //  supplierInvoiceItemPM.SupplierInvoiceItemVehicles = invoiceItem.SupplierInvoiceItemVehicles;

                                supplierInvoiceItemPM.SupplierInvoiceItemVehicles = supplierInvoiceItemVehicleQueryService.GetSupplierInvoiceItemVehiclesForSupplierInvoiceItem(invoiceItem.DeclarationId, Convert.ToInt32(invoiceItem.CounterKey), invoiceItem.LineNumber, tenant);

                                // supplierInvoiceItemVehicleQueryService.GetSupplierInvoiceItemVehiclesForSupplierInvoiceItem(invoiceItem.DeclarationId, Convert.ToInt32(invoiceItem.CounterKey), invoiceItem.LineNumber, tenant);

                                foreach (var supplierInvoiceItemVehicle in supplierInvoiceItemPM.SupplierInvoiceItemVehicles)
                                {
                                    supplierInvoiceItemVehicle.ChangeSetOp = ChangeSetOperation.Insert;
                                }

                                supplierInvoiceItemPM.CatalogNumber = invoiceItem.CatalogNumber;
                                supplierInvoiceItemPM.ItemCode = invoiceItem.ItemCode;
                                supplierInvoiceItemPM.ItemDescription = invoiceItem.ItemDescription;
                                supplierInvoiceItemPM.ItemAdditionalStatus = invoiceItem.ItemAdditionalStatus;
                                supplierInvoiceItemPM.CertificatesStatusCode = invoiceItem.CertificatesStatusCode;
                            }
                        }
                        else
                        {
                            supplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemVehicles(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                        }
                    }
                    //supplierInvoiceItemPM.SalesTaxExemptionTypeCode = GetValueCodeType(governmentAgencyGoodsItem.DMExtensions.SalesTaxExemptionType);
                    supplierInvoiceItemPM.PreferenceDocumentNumber = GetValueIDType(governmentAgencyGoodsItem.DMExtensions.PreferenceDocumentNumber);
                    //if (governmentAgencyGoodsItem.DMExtensions.IsUsed != null) supplierInvoiceItemPM.IsUsed = governmentAgencyGoodsItem.DMExtensions.IsUsed.Value;
                    supplierInvoiceItemPM.ActualInvoiceLines = governmentAgencyGoodsItem.DMExtensions.InvoiceLineNumbers;
                    //if (governmentAgencyGoodsItem.DMExtensions.DeferredCustomsTax != null) supplierInvoiceItemPM.DeferredCustomsTax = governmentAgencyGoodsItem.DMExtensions.DeferredCustomsTax.Value;
                    //if (governmentAgencyGoodsItem.DMExtensions.DeferredPurchaseTax != null) supplierInvoiceItemPM.DeferredPurchaseTax = governmentAgencyGoodsItem.DMExtensions.DeferredPurchaseTax.Value;
                    //AdditionalDocument**********
                    supplierInvoiceItemPM.SupplierInvoiceItemsMods = GetSupplierInvoiceItemsMods(governmentAgencyGoodsItem, declaration, declarationId, tenant);

                    // supplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvioceItemCertificats(supplierInvoicePM, supplierInvoiceItemPM);

                    supplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvioceItemCertificats(governmentAgencyGoodsItem, declaration, declarationId, tenant);

                    if (supplierInvoicePMPMOrg != null)
                    {
                        SupplierInvoiceItemPM supplierInvoiceItemPMOrg = supplierInvoicePMPMOrg.SupplierInvoiceItems.FirstOrDefault(x => x.SequenceNumeric == supplierInvoiceItemPM.SequenceNumeric);
                        if (supplierInvoiceItemPMOrg != null)
                        {
                            supplierInvoiceItemPM.SupplierInvoiceItemTaxes = supplierInvoiceItemPMOrg.SupplierInvoiceItemTaxes;

                            foreach (var supplierInvoiceItemTax in supplierInvoiceItemPM.SupplierInvoiceItemTaxes)
                            {
                                supplierInvoiceItemTax.ChangeSetOp = ChangeSetOperation.Insert;
                                supplierInvoiceItemTax.DeclarationId = supplierInvoiceItemPM.DeclarationId;
                            }
                        }
                    }
                    supplierInvoiceItemPMs.Add(supplierInvoiceItemPM);
                }
            }
            return supplierInvoiceItemPMs;
        }

   

        private List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsMods(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsModPM> supplierInvoiceItemsModPMs = new List<SupplierInvoiceItemsModPM>();

            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.DMExtensions.ValuationAdjustment != null)
            {
                foreach (var valuationAdjustment in governmentAgencyGoodsItem.DMExtensions.ValuationAdjustment)
                {
                    SupplierInvoiceItemsModPM supplierInvoiceItemsMod = new SupplierInvoiceItemsModPM();
                    supplierInvoiceItemsMod.DeclarationId = declarationId;
                    supplierInvoiceItemsMod.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsMod.TypeCode = GetValueCodeType(valuationAdjustment.AdditionCode);
                    supplierInvoiceItemsMod.CurrencyTypeCode = valuationAdjustment.AmountAmount.currencyID.ToString();
                    supplierInvoiceItemsMod.Amount = GetValueAmountType(valuationAdjustment.AmountAmount);
                    supplierInvoiceItemsMod.Tenant = tenant;
                    supplierInvoiceItemsModPMs.Add(supplierInvoiceItemsMod);
                }
            }
            return supplierInvoiceItemsModPMs;
        }

        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemVehiclePM> SupplierInvoiceItemVehiclePMs = new List<SupplierInvoiceItemVehiclePM>();
            if (governmentAgencyGoodsItem.DMExtensions.Vehicle != null)
                foreach (var vehicle in governmentAgencyGoodsItem.DMExtensions.Vehicle)
                {
                    SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM();
                    supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemVehiclePM.DeclarationId = declarationId;
                    supplierInvoiceItemVehiclePM.Tenant = tenant;
                    if (GetValueCodeType(vehicle.IDTypeCode) == "ZZZ")
                    {
                        supplierInvoiceItemVehiclePM.RichbitFileNumber = GetValueIDType(vehicle.ID);
                    }
                    if (GetValueCodeType(vehicle.IDTypeCode) == "CN")
                    {
                        supplierInvoiceItemVehiclePM.VehicleChassisNumber = GetValueIDType(vehicle.ID);
                    }
                    supplierInvoiceItemVehiclePM.VehicleTypeCode = GetValueCodeType(vehicle.IDTypeCode);
                    //vehicleDetails.ID.Value = supplierInvoiceItemVehicleItem.VehicleChassisNumber;
                    //vehicleDetails.IDTypeCode.Value = supplierInvoiceItemVehicleItem.VehicleTypeCode
                    SupplierInvoiceItemVehiclePMs.Add(supplierInvoiceItemVehiclePM);
                }
            return SupplierInvoiceItemVehiclePMs;
        }

        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModifications(DeclarationGoodsShipment declarationGoodsShipment,
            ref SupplierInvoicePM supplierInvoicePM, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceModificationPM> supplierInvoiceModificationPMs = new List<SupplierInvoiceModificationPM>();

            SupplierInvoiceModificationQueryService supplierInvoiceModificationQueryService = new SupplierInvoiceModificationQueryService(tenant);
            if (decIdOrg != null && isFromImporter)
            {
                supplierInvoiceModificationPMs = supplierInvoiceModificationQueryService.GetSupplierInvoiceModificationsForInvoice(decIdOrg, _OrgSupplierInvoicePM.InvoiceCounterKey);

                if (supplierInvoiceModificationPMs != null)
                    supplierInvoiceModificationPMs.ForEach(invoice =>
                    {
                        invoice.ChangeSetOp = ChangeSetOperation.Insert;
                    });

                supplierInvoicePM.InsruanceCurrencyTypeCode = _OrgSupplierInvoicePM.InsruanceCurrencyTypeCode;
                supplierInvoicePM.InsuranceAmount = _OrgSupplierInvoicePM.InsuranceAmount;
                supplierInvoicePM.InsruancePercentage = _OrgSupplierInvoicePM.InsruancePercentage;
                supplierInvoicePM.FreightCurrencyTypeCode = _OrgSupplierInvoicePM.FreightCurrencyTypeCode;
                supplierInvoicePM.TotalFreightInFreightCurrency = _OrgSupplierInvoicePM.TotalFreightInFreightCurrency;
            }
            else
            {
                if(declarationGoodsShipment.Invoice.DMExtensions.CustomsValuation != null)
                {
                    foreach (var customsValuation in declarationGoodsShipment.Invoice.DMExtensions.CustomsValuation)
                    {
                        string[] ChargesTypeCode = new string[] { "67", "144", "I02" };
                        if (!ChargesTypeCode.Contains(GetValueCodeType(customsValuation.ChargesTypeCode)))
                        {
                            if (GetValueAmountType(customsValuation.OtherChargeDeductionAmount) == 0) continue;

                            SupplierInvoiceModificationPM supplierInvoiceModificationPM = new SupplierInvoiceModificationPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                DeclarationId = declarationId,
                                TypeCode = GetValueCodeType(customsValuation.ChargesTypeCode),
                                //  CurrencyTypeCode = customsValuation.OtherChargeDeductionAmount.currencyID,
                                Amount = GetValueAmountType(customsValuation.OtherChargeDeductionAmount),
                                Tenant = tenant
                            }; 
                            supplierInvoiceModificationPMs.Add(supplierInvoiceModificationPM);
                        }

                        if (customsValuation.OtherChargeDeductionAmount != null)
                        {
                            if (customsValuation.ChargesTypeCode.Value == "67")
                            {
                                supplierInvoicePM.InsruanceCurrencyTypeCode = customsValuation.OtherChargeDeductionAmount.currencyID.ToString();
                                supplierInvoicePM.InsuranceAmount = GetValueAmountType(customsValuation.OtherChargeDeductionAmount);
                            }
                            if (customsValuation.ChargesTypeCode.Value == "144")
                            {
                                supplierInvoicePM.FreightCurrencyTypeCode = customsValuation.OtherChargeDeductionAmount.currencyID.ToString();
                                supplierInvoicePM.TotalFreightInFreightCurrency = GetValueAmountType(customsValuation.OtherChargeDeductionAmount);
                            }
                        }
                    }
                }
            }
            return supplierInvoiceModificationPMs;
        }

        public override void Update(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {

        }

 
        private List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificats(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            if (governmentAgencyGoodsItem == null || governmentAgencyGoodsItem.AdditionalDocument == null) return null;
            List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificatPMs = new List<SupplierInvioceItemCertificatPM>();
            foreach (var additionalDocument in governmentAgencyGoodsItem.AdditionalDocument)
            {
                SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                supplierInvioceItemCertificatPM.CertificateNumber = GetValueIDType(additionalDocument.ID);
                supplierInvioceItemCertificatPM.CertificateExemptionTypeCode = GetValueCodeType(additionalDocument.LPCOExemptionCode);
                supplierInvioceItemCertificatPM.AttachmentTypeCode = GetValueCodeType(additionalDocument.TypeCode);
                if(additionalDocument.DMExtensions!= null)
                {
                    supplierInvioceItemCertificatPM.ResConfirmationTypeCode= GetValueCodeType(additionalDocument.DMExtensions.LPCOTypeCode);
                    supplierInvioceItemCertificatPM.ReqConfirmationTypeCode = GetValueCodeType(additionalDocument.DMExtensions.RequirementLicenseType);
                    supplierInvioceItemCertificatPM.CustomsAttachmentID = GetValueIDType(additionalDocument.DMExtensions.ExternalAttachmentID);
                    supplierInvioceItemCertificatPM.SequenceNumeric = Convert.ToInt32( additionalDocument.DMExtensions.SequenceNumeric) ;

                }
                supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Insert;
                supplierInvioceItemCertificatPM.Tenant = tenant;
               supplierInvioceItemCertificatPMs.Add(supplierInvioceItemCertificatPM);
            }
            return supplierInvioceItemCertificatPMs;
         }

       
         private List<DeclarationTaxPM> GetDeclarationTaxesPM(Declaration declaration, DeclarationPM declarationPMOrg, string declarationId,int tenant)
        {
            var declarationTaxPMList = new List<DeclarationTaxPM>();

            if (declaration.DutyTaxFee == null && declarationPMOrg != null)
            {
                foreach (var item in declarationPMOrg.DeclarationTaxes)
                {
                    item.ChangeSetOp = ChangeSetOperation.Insert;
                    item.DeclarationId = declarationId;
                }
                return declarationPMOrg.DeclarationTaxes;
            }

            foreach (var dutyTaxFee in declaration.DutyTaxFee)
            {
                var declarationTaxPM = new DeclarationTaxPM();
                declarationTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
                declarationTaxPM.DeclarationId = declarationId;
                declarationTaxPM.Tenant = tenant;
                declarationTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
                declarationTaxPM.TotalAmount = dutyTaxFee.DMExtensions.CalculatedTax.Amount.Value;
                //declarationTaxPM.DeferredTaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.DeferedTaxAmount.Value;
                declarationTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;
                declarationTaxPMList.Add(declarationTaxPM);
            }
            return declarationTaxPMList;
        }
 
    }
}

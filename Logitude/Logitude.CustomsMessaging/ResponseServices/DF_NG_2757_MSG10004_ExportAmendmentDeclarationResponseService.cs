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
using System.Web.UI.WebControls;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2757_MSG10004_ExportAmendmentDeclarationResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_2757_MSG10004_ExportDeclarationResponse, GenericRequestParams>
    {


        DeclarationPM _MyDeclarationPM;
        public bool _IsSubmitDeclarationResponse { get; set; }
        decimal? vat = 0;
        DeclarationError _MyDeclarationError;
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

        public DeclarationPM MapResponseToDeclaration(Declaration declaration, int tenant, bool FromImporter, string idOrg, out string error, bool isUpdate = false, string user = null, bool isUpdateAfterAccept = false, bool isCopy = false, bool isClose = false, bool? isAmendApprove = null)
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
                        if (isCopy)
                            declarationOrg = myQueryService.GetDeclarationsByIds(new List<string> { idOrg }, tenant).FirstOrDefault();
                        if (declarationOrg == null)
                            declarationOrg = myQueryService.GetAcceptDeclarationAmendment(idOrg, tenant);
                        if (declarationOrg == null)
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
                        Consignments = GetConsignments(declaration, tenant, null, context, declarationOrg?.Consignments, isAmendApprove),
                    };
                   // declarationPM.IsSubmitDeclaration = declarationOrg.IsSubmitDeclaration;
                    declarationPM.IsExportClosed = declarationOrg.IsExportClosed;
                    declarationPM.ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID);
                    declarationPM.ExportFile = declarationOrg.ExportFile;
                    declarationPM.CustomFileNo = declarationOrg.CustomFileNo;
                    declarationPM.DeclarationTypeCode = GetValueCodeType(declaration.TypeCode);
                    declarationPM.AgentRoleCode = declarationOrg.AgentRoleCode;
                    declarationPM.LoadingFactor = declarationOrg.LoadingFactor;
                    declarationPM.DealValue = declarationOrg.DealValue;
                    declarationPM.IsConvertedDeclaration = declarationOrg.IsConvertedDeclaration;
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
                        if (!isCopy)
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
                    declarationPM = myQueryService.GetSingle(idOrg, true, false);
                    var declarationId2 = declarationRepository.GetLastDeclarationByDeclarationId(declarationPM.AmendmentOriginalDeclartation, tenant, true)?.Id;
                    invoicePMs = GetSupplierInvoices(declaration, tenant, context, declarationId2, declarationOrg);
                    DeleteSomeObjects(declarationPM, tenant, context);
                    declarationPM.DeclarationOfficeCode = GetValueIDType(declaration.DeclarationOfficeID);
                    declarationPM.ExportDeclarationOfficeCode = GetValueIDType(declaration.ExportDeclarationOfficeID);
                    declarationPM.DeclarationTypeCode = GetValueCodeType(declaration.TypeCode);
                    declarationPM.Tenant = tenant;
                    declarationPM.IsConnectedToUnifreight = false;
                    declarationPM.AmendmentDontDisplayInList = false;
                    declarationPM.IsAmendment = true;
                    declarationPM.Consignments = GetConsignments(declaration, tenant, declarationPM, context, declarationOrg?.Consignments, isAmendApprove);

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
                    declarationPM.AmedmentType = isClose ? "2" : "1";
                    //declarationPM.ExportDeclarationClosingDatas = GetClosingDetails(declaration, tenant, declarationPM, context);
                    //declarationPM.CustomFileNo = GetValueIDType(declaration.DMExtensions.AgentFileReferenceID);
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

                var exportDeclarationClosingData = GetClosingDetails(declaration, tenant, declarationPM, context);
                if (exportDeclarationClosingData != null)
                {
                    exportDeclarationClosingData.DeclarationId = declarationPM.Id;
                    exportDeclarationClosingData.Tenant = tenant;
                    ExportDeclarationClosingDataUpdateService exportDeclarationClosingDataUpdateService = new ExportDeclarationClosingDataUpdateService(context, new Dictionary<string, IContext>(), tenant);
                    exportDeclarationClosingDataUpdateService.Update(exportDeclarationClosingData, true);
                }

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
                    if (isUpdateAfterAccept)
                    {
                        var courierdeclaration = courierDeclarationQueryService.GetCourierDeclarationByDeclarationId(declarationOrg.Id, tenant);
                        if (courierdeclaration != null)
                            courierdeclaration.ChangeSetOp = ChangeSetOperation.Delete;
                        courierDeclarationUpdateService.Update(courierdeclaration, true);
                        CourierDeclarationPM courierDeclarationPM = new CourierDeclarationPM();
                        courierDeclarationPM.Tenant = courierdeclaration.Tenant;
                        courierDeclarationPM.SequenceNumeric = courierdeclaration.SequenceNumeric;
                        courierDeclarationPM.CourierMasterId = courierdeclaration.CourierMasterId;
                        courierDeclarationPM.DeclarationId = declarationCourierStatusPMNew.DeclarationId;
                        courierDeclarationPM.ChangeSetOp = ChangeSetOperation.Insert;
                        courierDeclarationUpdateService.Update(courierDeclarationPM, true);
                    }
                }

                string declarationId;
                if (declarationOrg != null)
                    declarationId = declarationRepository.GetLastDeclarationByDeclarationId(declarationPM.AmendmentOriginalDeclartation, tenant, true)?.Id;
                else
                    declarationId = declarationPM.Id;

                declarationPM.DeclarationTaxes = GetDeclarationTaxesPM(declaration, declarationOrg, declarationId, tenant);
                if (declarationOrg != null && !_isUpdateAfterAccept)
                {
                    declarationPM.ReferentUserId = declarationOrg.ReferentUserId;
                    LogMessagingUtil.Instance.AppendLine("set ReferentUserId to new declaration after save");
                }

                if (isUpdateAfterAccept)
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
                if (declarationOrg != null && !isUpdateAfterAccept)
                {
                    List<CustomsDocumentsTicketPM> customsDocumentsTicketPMs = customsDocumentsTicketQuery.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationOrg.Id, "", "", "", tenant, "Declaration");
                    customsDocumentsTicketPMs.AddRange(customsDocumentsTicketQuery.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationOrg.Id, "", "", "", tenant, parentEntityCode :"ExportDeclarationClosingData"));


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
            declarationUpdateService.DeclarationRecipientFastDelete(declarationPM, context);
            declarationUpdateService.DeclarationClosingDataFastDelete(declarationPM, context);

            var mySupplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(context, new Dictionary<string, IContext>(), tenant);
            var mySupplierInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 20.10.15 - Task 17209 
            var mySupplierInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424 
            var mySupplierInvoiceItemsPriceUpdateService = new SupplierInvoiceItemsPriceUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySupplierInvoiceItemsLevyUpdateService = new SupplierInvoiceItemsLevyUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySupplierInvoiceItemsDescriptUpdateService = new SupplierInvoiceItemsDescriptUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySupplierInvoiceItemsProdIdentUpdateService = new SupplierInvoiceItemsProdIdentUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySupplierInvoiceItemsSerialNumUpdateService = new SupplierInvoiceItemsSerialNumUpdateService(context, new Dictionary<string, IContext>(), tenant); // moran 24.11.15 - Task 17424
            var mySuppInvoiceItemsAbachStatementUpdateService = new SuppInvoiceItemsAbachStatementUpdateService(context, new Dictionary<string, IContext>(), tenant);                                                                                                       //var mySupplierInvoiceItemsTaxesModificationUpdateService = new SupplierInvoiceItemsTaxesModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(context, new Dictionary<string, IContext>(), tenant);


           


            var myDeclarationKeys = new DeclarationKeys { Id = declarationPM.Id };
            myDeclarationTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemVehicleModUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsTaxUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemModVehicleUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsPriceUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsLevyUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsDescriptUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsProdIdentUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySupplierInvoiceItemsSerialNumUpdateService.FastDeleteComposition(myDeclarationKeys);
            mySuppInvoiceItemsAbachStatementUpdateService.FastDeleteComposition(myDeclarationKeys);
            declarationUpdateService.DeclarationSupplierInvoicesFastDelete(declarationPM, context);

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
            if (declaration.DMExtensions.DeclarationClosingDetails != null)
            {
                var closingDetails = new ExportDeclarationClosingDataPM();
                closingDetails.FinalShipCode = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalShipID);
                closingDetails.FinalLoadingSite = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalLoadingSite);
                if (declaration.DMExtensions.DeclarationClosingDetails.DepartureDateTime != null)
                {
                    closingDetails.LoadingDateTime = declaration.DMExtensions.DeclarationClosingDetails.DepartureDateTime.Value;
                }
                if (declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument != null)//420095 orn
                {
                    closingDetails.FinalManifestNumber = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.FirstCargoID);
                    closingDetails.FinalCargoTypeCode = GetValueCodeType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.TypeCode);
                    closingDetails.FinalSecondCargoId = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.SecondCargoID);
                    closingDetails.FinalThirdCargoId = GetValueIDType(declaration.DMExtensions.DeclarationClosingDetails.FinalTransportContractDocument.ThirdCargoID);
                }
                closingDetails.ChangeSetOp = ChangeSetOperation.Insert;
                
                return closingDetails;
            }
            return null;
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

        private List<ConsignmentPM> GetConsignments(Declaration declaration, int tenant, DeclarationPM declarationPM, ICustomContext context, List<ConsignmentPM> consignments, bool? isAmendApprove = null)
        {
            List<ConsignmentPM> consignmentPMs = new List<ConsignmentPM>();

            if (declaration.GoodsShipment == null || declaration.GoodsShipment.Count() == 0)
            {
                ConsignmentPM consignmentPM = new ConsignmentPM();
                consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPMs.Add(consignmentPM);
                return consignmentPMs;
            }
            if ((declaration.GoodsShipment[0].ExportConsignment == null && declaration.GoodsShipment[0].ImportConsignment == null) ||
                (declaration.GoodsShipment[0].ExportConsignment?.Count() == 0 && declaration.GoodsShipment[0].ImportConsignment?.Count() == 0))
            {
                ConsignmentPM consignmentPM = new ConsignmentPM();
                consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPMs.Add(consignmentPM);
                return consignmentPMs;
            }

            if (declaration.GoodsShipment[0].ExportConsignment != null) { 

                foreach (var consignment in declaration.GoodsShipment[0].ExportConsignment)
                {
                    ConsignmentPM consignmentPM = new ConsignmentPM();
                    consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                    consignmentPM.Tenant = tenant;
                    consignmentPM.ConsignmentType = "E";

                    var consignmentQueryService = new ConsignmentQueryService(context);


                    consignmentPM.SequenceNumeric = Convert.ToInt32(consignment.SequenceNumeric);

                    
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


                        if (isAmendApprove.GetValueOrDefault())
                        {
                            if (declarationPM != null)
                            {
                                //העתקת אחסנה שקושרה במסגרת התיקון
                                consignmentPM.ExportStoragesId = declarationPM.Consignments.Where(x => x.CargoTypeCode == consignmentPM.CargoTypeCode && x.ManifestNumber == consignmentPM.ManifestNumber && x.SecondCargoID == consignmentPM.SecondCargoID && x.ThirdCargoID == consignmentPM.ThirdCargoID).Select(y => y.ExportStoragesId).FirstOrDefault();
                            }
                            if (consignmentPM.ExportStoragesId == null)
                            {
                                //העתקת אחסנה מהמשגור בהצהרה ראשית
                                consignmentPM.ExportStoragesId = consignments.Where(x => x.CargoTypeCode == consignmentPM.CargoTypeCode && x.ManifestNumber == consignmentPM.ManifestNumber && x.SecondCargoID == consignmentPM.SecondCargoID && x.ThirdCargoID == consignmentPM.ThirdCargoID).Select(y => y.ExportStoragesId).FirstOrDefault();
                            }
                        }



                        if (declarationPM == null)
                        {
                            consignmentPM.ExportContainerizationID = consignments.Where(x => x.CargoTypeCode == consignmentPM.CargoTypeCode && x.ManifestNumber == consignmentPM.ManifestNumber && x.SecondCargoID == consignmentPM.SecondCargoID && x.ThirdCargoID == consignmentPM.ThirdCargoID).Select(y => y.ExportContainerizationID).FirstOrDefault();
                        }
                        else
                        {
                            consignmentPM.ExportContainerizationID = declarationPM.Consignments.Where(x => x.CargoTypeCode == consignmentPM.CargoTypeCode && x.ManifestNumber == consignmentPM.ManifestNumber && x.SecondCargoID == consignmentPM.SecondCargoID && x.ThirdCargoID == consignmentPM.ThirdCargoID).Select(y => y.ExportContainerizationID).FirstOrDefault();
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
                        consignmentPM.FinalDestinationPortCode = consignment.DMExtensions.FinalDestinationPort?.Value;
                        consignmentPM.ShipCode = consignment.DMExtensions.ShipID?.Value;
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
                        consignmentPM.ConsignmentPackages = GetConsignmentPackagesExport(consignment.DMExtensions.PackagesMeasure, declaration, tenant);
                    }

                    consignmentPMs.Add(consignmentPM);
                }
            }
            if (declaration.GoodsShipment[0].ImportConsignment != null)
            {
                foreach (var consignment in declaration.GoodsShipment[0].ImportConsignment)
                {
                    ConsignmentPM consignmentPM = new ConsignmentPM();
                    consignmentPM.ChangeSetOp = ChangeSetOperation.Insert;
                    consignmentPM.Tenant = tenant;
                    consignmentPM.ConsignmentType = "I";
                    consignmentPM.SequenceNumeric = Convert.ToInt32(consignment.SequenceNumeric);

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
                        if (declarationPM == null)
                        {
                            consignmentPM.ExportContainerizationID = consignments.Where(x => x.CargoTypeCode == consignmentPM.CargoTypeCode && x.ManifestNumber == consignmentPM.ManifestNumber && x.SecondCargoID == consignmentPM.SecondCargoID && x.ThirdCargoID == consignmentPM.ThirdCargoID).Select(y => y.ExportContainerizationID).FirstOrDefault();
                        }
                        else
                        {
                            consignmentPM.ExportContainerizationID = declarationPM.Consignments.Where(x => x.CargoTypeCode == consignmentPM.CargoTypeCode && x.ManifestNumber == consignmentPM.ManifestNumber && x.SecondCargoID == consignmentPM.SecondCargoID && x.ThirdCargoID == consignmentPM.ThirdCargoID).Select(y => y.ExportContainerizationID).FirstOrDefault();
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
                                    case "008":
                                        {
                                            consignmentPM.ExportRecieverWareHouseCode = GetValueIDType(registeredFacility.ID);
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
                    if (consignment?.DMExtensions?.RegisteredFacility != null &&
                        consignment.DMExtensions.RegisteredFacility.Length > 0 &&
                        consignment.DMExtensions.RegisteredFacility[0].DMExtensions?.PackagesMeasure != null
                        )
                    {
                        consignmentPM.ConsignmentPackages = GetConsignmentPackagesImport(consignment.DMExtensions.RegisteredFacility[0].DMExtensions.PackagesMeasure, declaration, tenant);
                    }

                    consignmentPMs.Add(consignmentPM);
                }
            }
            return consignmentPMs;
        }

        private List<ConsignmentPackagePM> GetConsignmentPackagesImport(DeclarationGoodsShipmentImportConsignmentDMExtensionsRegisteredFacilityDMExtensionsPackagesMeasure[] packagesMeasures, Declaration declaration, int tenant)
        {
            List<ConsignmentPackagePM> consignmentPackagePMs = new List<ConsignmentPackagePM>();
            
            foreach (var packagesMeasure in packagesMeasures)
            {
                ConsignmentPackagePM consignmentPackagePM = new ConsignmentPackagePM();

                consignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPackagePM.PackageMeasureQualifierCode = "1";
                consignmentPackagePM.PackageQuantityTypeCode = packagesMeasure.TotalPackageQuantity.unitCode.ToString();
                consignmentPackagePM.PackageQuantity = Convert.ToInt32(packagesMeasure.TotalPackageQuantity.Value);
                if (packagesMeasure.GrossMassMeasure != null)
                {
                    consignmentPackagePM.GrossMassMeasureTypeCode = packagesMeasure.GrossMassMeasure.unitCode.ToString();
                    consignmentPackagePM.GrossMassMeasure = packagesMeasure.GrossMassMeasure.Value;
                }
                consignmentPackagePM.PackageTypeCode = GetValueCodeType(packagesMeasure.TypeCode);
                consignmentPackagePM.MarksNumbers = GetValueTextType(packagesMeasure.MarksNumbers);
                consignmentPackagePM.Tenant = tenant;
                consignmentPackagePMs.Add(consignmentPackagePM);
            }
            return consignmentPackagePMs;
        }

        private List<ConsignmentPackagePM> GetConsignmentPackagesExport(DeclarationGoodsShipmentExportConsignmentDMExtensionsPackagesMeasure[] packagesMeasures, Declaration declaration, int tenant)
        {
            List<ConsignmentPackagePM> consignmentPackagePMs = new List<ConsignmentPackagePM>();
            foreach (var packagesMeasure in packagesMeasures)
            {
                ConsignmentPackagePM consignmentPackagePM = new ConsignmentPackagePM();
                consignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;
                consignmentPackagePM.PackageMeasureQualifierCode = GetValueCodeType(packagesMeasure.PackageMeasureQualifier);
                if (packagesMeasure.TotalPackageQuantity != null)
                {
                    consignmentPackagePM.PackageQuantityTypeCode = packagesMeasure.TotalPackageQuantity.unitCode.ToString();
                    consignmentPackagePM.PackageQuantity = Convert.ToInt32(packagesMeasure.TotalPackageQuantity.Value);
                }
                if(packagesMeasure.GrossMassMeasure != null)
                {
                    consignmentPackagePM.GrossMassMeasureTypeCode = packagesMeasure.GrossMassMeasure.unitCode.ToString();
                    consignmentPackagePM.GrossMassMeasure = packagesMeasure.GrossMassMeasure.Value;
                }
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

            if (_isUpdateAfterAccept)
            {
               
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
                    supplierInvoicePM.DutyRegimeProtocolCode= GetValueCodeType(item.Invoice.DMExtensions.DutyRegimeProtocolCode);
                    supplierInvoicePM.PreferenceDocumentTypeCode = GetValueCodeType(item.Invoice.DMExtensions.PreferenceDocumentType);
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
                }

                supplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModifications(item, ref supplierInvoicePM, declaration, declarationId, tenant);
                SupplierInvoicePM SupplierInvoicePMOrg;
                if (declarationPMOrg != null)
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

        //private List<SupplierInvoiceFreightAmountPM> GetSupplierInvoiceFreightAmounts(ref SupplierInvoicePM supplierInvoicePM, DeclarationGoodsShipment declarationGoodsShipment, int tenant)
        //{
        //    SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(tenant);
        //    List<SupplierInvoiceFreightAmountPM> supplierInvoiceFreightAmountPM = new List<SupplierInvoiceFreightAmountPM>();
        //    if (decIdOrg != null && isFromImporter)
        //    {
        //        supplierInvoiceFreightAmountPM = supplierInvoiceFreightAmountQueryService.GetSupplierInvoiceFreightAmountsByInvoice(decIdOrg, _OrgSupplierInvoicePM.InvoiceCounterKey);

        //        if (supplierInvoiceFreightAmountPM != null)
        //        {
        //            supplierInvoiceFreightAmountPM.ForEach(x => { x.ChangeSetOp = ChangeSetOperation.Insert; });
        //        }
        //    }
        //    //else if (declarationGoodsShipment..CustomsValuation != null)
        //    //{
        //    //    foreach (var item in declarationGoodsShipment.CustomsValuation)
        //    //    {
        //    //        if (item.ChargesTypeCode.Value == "144")
        //    //        {
        //    //            SupplierInvoiceFreightAmountPM supplierInvoiceFreightAmountPM1 = new SupplierInvoiceFreightAmountPM();
        //    //            supplierInvoiceFreightAmountPM1.Amount = GetValueAmountType(item.FreightChargeAmount);
        //    //            supplierInvoiceFreightAmountPM1.CurrencyTypeCode = item.FreightChargeAmount.currencyID.ToString();
        //    //            supplierInvoiceFreightAmountPM1.ChangeSetOp = ChangeSetOperation.Insert;
        //    //            supplierInvoiceFreightAmountPM1.DeclarationId = supplierInvoicePM.DeclarationId;
        //    //            supplierInvoiceFreightAmountPM1.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;
        //    //            supplierInvoiceFreightAmountPM1.Tenant = tenant;
        //    //             supplierInvoiceFreightAmountPM.Add(supplierInvoiceFreightAmountPM1);

        //    //        }
        //    //    }

        //    //}
        //    return supplierInvoiceFreightAmountPM;
        //}

        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItems(DeclarationGoodsShipment item, Declaration declaration, string declarationId, int tenant, SupplierInvoicePM supplierInvoicePM, ICustomContext context, SupplierInvoicePM supplierInvoicePMPMOrg)
        {
            List<SupplierInvoiceItemPM> supplierInvoiceItemPMs = new List<SupplierInvoiceItemPM>();
            if (item.GovernmentAgencyGoodsItem != null)
            {
                foreach (var governmentAgencyGoodsItem in item.GovernmentAgencyGoodsItem)
                {
                    SupplierInvoiceItemPM supplierInvoiceItemPM = new SupplierInvoiceItemPM();
                    
                    supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemPM.DeclarationId = declarationId;
                    supplierInvoiceItemPM.SequenceNumeric = (int)governmentAgencyGoodsItem.SequenceNumeric;
                    
                    if(governmentAgencyGoodsItem.Origin != null)
                        supplierInvoiceItemPM.OriginCountryCode = GetValueCodeType(governmentAgencyGoodsItem.Origin.CountryCode);
                    
                    supplierInvoiceItemPM.ClaimReasonCode = GetValueCodeType(governmentAgencyGoodsItem.DMExtensions.ClaimReasonCode);
                    supplierInvoiceItemPM.Tenant = tenant;
                    
                    if (governmentAgencyGoodsItem.Commodity.Classification != null || governmentAgencyGoodsItem.Commodity.Classification.Count() > 0)
                    {
                        var classification = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x != null && GetValueCodeType(x.IdentificationTypeCode) == "SSO");

                        if (classification != null)
                        {
                            supplierInvoiceItemPM.DangerousClassificationCode = GetValueIDType(classification.ID);
                        }
                        var classification2 = governmentAgencyGoodsItem.Commodity.Classification.FirstOrDefault(x => x != null && GetValueCodeType(x.IdentificationTypeCode) == "HS");

                        if (classification2 != null)
                        {
                            supplierInvoiceItemPM.ClassificationCode = GetValueIDType(classification2.ID).Replace("/", "");
                        }
                        supplierInvoiceItemPM.DutyRegimeProtocolCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.Classification[0].DMExtensions.DutyRegimeProtocolCode);
                        supplierInvoiceItemPM.TradeAgreementCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.Classification[0].DMExtensions.DutyRegimeCode);
                        supplierInvoiceItemPM.ClassificationTypeCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.Classification[0].IdentificationTypeCode);

                        supplierInvoiceItemPM.TaxExemptCode = GetValueCodeType(governmentAgencyGoodsItem.Commodity.Classification[0].DMExtensions?.TaxExemptCode)?.Replace("/", "");




                    }

                    if (governmentAgencyGoodsItem.GovernmentProcedure != null && governmentAgencyGoodsItem.GovernmentProcedure.Count() > 0)
                    {
                        supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes = new List<SupplierInvoiceItemProcesTypePM>();
                        foreach (var governmentProcedure in governmentAgencyGoodsItem.GovernmentProcedure)
                        {
                            SupplierInvoiceItemProcesTypePM supplierInvoiceItemProcesTypePM = new SupplierInvoiceItemProcesTypePM();
                            supplierInvoiceItemProcesTypePM.ChangeSetOp = ChangeSetOperation.Insert;
                            supplierInvoiceItemProcesTypePM.DeclarationId = declarationId;
                            supplierInvoiceItemProcesTypePM.Tenant = tenant;
                            supplierInvoiceItemProcesTypePM.ProcessTypeCode = GetValueCodeType(governmentProcedure.CurrentCode);
                            supplierInvoiceItemPM.SupplierInvoiceItemProcesTypes.Add(supplierInvoiceItemProcesTypePM);
                        }
                    }
                    
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
                                supplierInvoiceItemsConDeclarPM.QuantityTypeCode = previousDocument.DMExtensions.QuantityQuantity?.unitCode.ToString();
                                supplierInvoiceItemsConDeclarPM.Quantity = previousDocument.DMExtensions.QuantityQuantity?.Value;
                                supplierInvoiceItemsConDeclarPM.InvoiceNumber = ((int?)previousDocument.DMExtensions.SequenceNumeric);
                            }
                            supplierInvoiceItemPM.SupplierInvoiceItemsConDeclars.Add(supplierInvoiceItemsConDeclarPM);
                        }
                    }
                    

                    if (governmentAgencyGoodsItem.DMExtensions != null)
                    {
                        if (governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null && governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount.Count() > 0)
                        {
                            foreach (var goodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                            {
                               
                                if (goodsItemAmount.CustomsValueAmount != null)
                                    switch (GetValueCodeType(goodsItemAmount.AmountType))
                                    {
                                        
                                        case "1":
                                            {
                                                if (item.Invoice != null && item.Invoice.DMExtensions != null && item.Invoice.DMExtensions.InvoiceAmount != null)
                                                {
                                                    if (goodsItemAmount.CustomsValueAmount.currencyID.ToString() == item.Invoice.DMExtensions.InvoiceAmount.currencyID.ToString())
                                                    {
                                                        supplierInvoiceItemPM.ItemPrice = GetValueAmountType(goodsItemAmount.CustomsValueAmount);
                                                      
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


                        supplierInvoiceItemPM.TransactionNatureCode = GetValueCodeType(governmentAgencyGoodsItem.DMExtensions.TransactionNatureCode);
                        if (isFromImporter)
                        {
                            SupplierInvoiceItemVehicleQueryService supplierInvoiceItemVehicleQueryService = new SupplierInvoiceItemVehicleQueryService(context);

                            SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);

                            var invoiceItem = supplierInvoiceItemQueryService.GetSingleSupplierInvoicePMBySequence(decIdOrg, Convert.ToInt32(supplierInvoicePM.InvoiceCounterKey), Convert.ToInt32(supplierInvoiceItemPM.SequenceNumeric));
                            if (invoiceItem != null)
                            {
                                
                                supplierInvoiceItemPM.SupplierInvoiceItemVehicles = supplierInvoiceItemVehicleQueryService.GetSupplierInvoiceItemVehiclesForSupplierInvoiceItem(invoiceItem.DeclarationId, Convert.ToInt32(invoiceItem.CounterKey), invoiceItem.LineNumber, tenant);

                               
                                foreach (var supplierInvoiceItemVehicle in supplierInvoiceItemPM.SupplierInvoiceItemVehicles)
                                {
                                    supplierInvoiceItemVehicle.ChangeSetOp = ChangeSetOperation.Insert;
                                }

                                supplierInvoiceItemPM.CatalogNumber = invoiceItem.CatalogNumber;
                                supplierInvoiceItemPM.ItemCode = invoiceItem.ItemCode;
                                supplierInvoiceItemPM.ItemDescription = invoiceItem.ItemDescription;
                                supplierInvoiceItemPM.ItemAdditionalStatus = invoiceItem.ItemAdditionalStatus;
                                supplierInvoiceItemPM.CertificatesStatusCode = invoiceItem.CertificatesStatusCode;
                                supplierInvoiceItemPM.TaxExemptCode = invoiceItem.TaxExemptCode;
                            }
                        }
                        else
                        {
                            supplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemVehicles(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                        }
                    }                   
                    supplierInvoiceItemPM.PreferenceDocumentNumber = GetValueIDType(governmentAgencyGoodsItem.DMExtensions.PreferenceDocumentNumber);
                    supplierInvoiceItemPM.ActualInvoiceLines = governmentAgencyGoodsItem.DMExtensions.InvoiceLineNumbers;
                    supplierInvoiceItemPM.SupplierInvoiceItemsMods = GetSupplierInvoiceItemsMods(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemsPrices = GetSupplierInvoiceItemsPrices(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SuppInvoiceItemsAbachStatements = GetSupplierInvoiceItemsAbachStatements(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemsSerialNums = GetSupplierInvoiceItemsSerialNums(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemsProdIdents = GetSupplierInvoiceItemsProdIdents(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemsDescripts = GetSupplierInvoiceItemsDescript(governmentAgencyGoodsItem, declaration, declarationId, tenant);
                    supplierInvoiceItemPM.SupplierInvoiceItemLevies = GetSupplierInvoiceItemLevy(governmentAgencyGoodsItem, declaration, declarationId, tenant);
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

        private List<SupplierInvoiceItemsPricePM> GetSupplierInvoiceItemsPrices(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsPricePM> SupplierInvoiceItemsPricePM = new List<SupplierInvoiceItemsPricePM>();
            
            SupplierInvoiceItemsPriceQueryService supplierInvoiceItemsPriceQueryService = new SupplierInvoiceItemsPriceQueryService(tenant);
            var AdditionalPriceTypeCodes = supplierInvoiceItemsPriceQueryService.GetSupplierInvoiceItemsPriceByDeclarationId(declarationId, tenant);
            
            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount != null)
            { 
                var cur = declaration.GoodsShipment[0].Invoice.DMExtensions.InvoiceAmount.currencyID.ToString();
                foreach (var GoodsItemAmount in governmentAgencyGoodsItem.DMExtensions.GoodsItemAmount)
                {
                    var AdditionalPriceTypeCode = GetValueCodeType(GoodsItemAmount.AmountType);
                    bool IsExist = SupplierInvoiceItemsPricePM.Any(x => x.AdditionalPriceTypeCode == GetValueCodeType(GoodsItemAmount.AmountType));
                    
                    if (!IsExist&&GetValueCodeType(GoodsItemAmount.AmountType) != "1" && GoodsItemAmount.CustomsValueAmount.currencyID.ToString()== cur)
                    {
                        SupplierInvoiceItemsPricePM supplierInvoiceItemsPrice = new SupplierInvoiceItemsPricePM();


                        if (isFromImporter)//פתיחת בקשה לתיקון
                        {
                            supplierInvoiceItemsPrice.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        else //משוב לתיקון
                        {
                            if ((new string[]{"5","11","16" }).Contains(GetValueCodeType(GoodsItemAmount.AmountType)))
                                supplierInvoiceItemsPrice.ChangeSetOp = ChangeSetOperation.Insert;
                            else
                                continue;
                        }
                       

                        /*if (!isFromImporter && AdditionalPriceTypeCodes.Contains(AdditionalPriceTypeCode))

                        {
                            //supplierInvoiceItemsPrice.ChangeSetOp = ChangeSetOperation.Update;
                        }
                        else if (isFromImporter)
                        {
                            supplierInvoiceItemsPrice.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        else
                        {
                            continue;
                        }*/

                        supplierInvoiceItemsPrice.DeclarationId = declarationId;
                        supplierInvoiceItemsPrice.AdditionalPrice = GetValueAmountType(GoodsItemAmount.CustomsValueAmount);
                       
                        supplierInvoiceItemsPrice.AdditionalPriceTypeCode = GetValueCodeType(GoodsItemAmount.AmountType);
                        supplierInvoiceItemsPrice.Tenant = tenant;
                        SupplierInvoiceItemsPricePM.Add(supplierInvoiceItemsPrice);
                        
                    } 
                    
                }
            }
            return SupplierInvoiceItemsPricePM;
        }
        

        private List<SuppInvoiceItemsAbachStatementPM> GetSupplierInvoiceItemsAbachStatements(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SuppInvoiceItemsAbachStatementPM> supplierInvoiceItemsAbachStatementPM = new List<SuppInvoiceItemsAbachStatementPM>();
           

            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].DangerousGoodsStatement != null)
            {
                foreach (var GoodsItemAbachStatement in governmentAgencyGoodsItem.Commodity.Classification[0].DangerousGoodsStatement)
                {
                    SuppInvoiceItemsAbachStatementPM supplierInvoiceItemsAbachStatement = new SuppInvoiceItemsAbachStatementPM();
                    supplierInvoiceItemsAbachStatement.DeclarationId = declarationId;
                    supplierInvoiceItemsAbachStatement.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsAbachStatement.StatementTypeCode = GetValueIDType(GoodsItemAbachStatement.StatementType);
                    supplierInvoiceItemsAbachStatement.IsStatementInd = GoodsItemAbachStatement.DangerousGoodsStatementInd.Value;
                    supplierInvoiceItemsAbachStatement.SequenceNumeric = GoodsItemAbachStatement.SequenceNumeric;
                    supplierInvoiceItemsAbachStatement.Tenant = tenant;
                    supplierInvoiceItemsAbachStatementPM.Add(supplierInvoiceItemsAbachStatement);
                }
            }
            return supplierInvoiceItemsAbachStatementPM;
        }

        private List<SupplierInvoiceItemsSerialNumPM> GetSupplierInvoiceItemsSerialNums(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsSerialNumPM> supplierInvoiceItemsSerialNumPM = new List<SupplierInvoiceItemsSerialNumPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].SerialNumbers != null)
            {
                foreach (var GoodsItemSerialNumbers in governmentAgencyGoodsItem.Commodity.Classification[0].SerialNumbers)
                {
                    SupplierInvoiceItemsSerialNumPM supplierInvoiceItemsSerialNum = new SupplierInvoiceItemsSerialNumPM();
                    supplierInvoiceItemsSerialNum.DeclarationId = declarationId;
                    supplierInvoiceItemsSerialNum.ChangeSetOp = ChangeSetOperation.Insert;
                  supplierInvoiceItemsSerialNum.TypeCode = GetValueCodeType(GoodsItemSerialNumbers.IdentityQualifierCode);
                   supplierInvoiceItemsSerialNum.SerialNumber = GetValueIDType(GoodsItemSerialNumbers.ID);
                    supplierInvoiceItemsSerialNum.Tenant = tenant;
                    supplierInvoiceItemsSerialNumPM.Add(supplierInvoiceItemsSerialNum);
                }
            }
            return supplierInvoiceItemsSerialNumPM;
        }
        private List<SupplierInvoiceItemsProdIdentPM> GetSupplierInvoiceItemsProdIdents(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsProdIdentPM> supplierInvoiceItemsProdIdentPM = new List<SupplierInvoiceItemsProdIdentPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].ProductIdentification != null)
            {
                foreach (var GoodsItemProdIdent in governmentAgencyGoodsItem.Commodity.Classification[0].ProductIdentification)
                {
                    SupplierInvoiceItemsProdIdentPM supplierInvoiceItemsProdIdent = new SupplierInvoiceItemsProdIdentPM();
                    supplierInvoiceItemsProdIdent.DeclarationId = declarationId;
                    supplierInvoiceItemsProdIdent.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsProdIdent.TypeCode = GetValueCodeType(GoodsItemProdIdent.IDTypeCode);
                    supplierInvoiceItemsProdIdent.Identification = GetValueIDType(GoodsItemProdIdent.ID);
                    supplierInvoiceItemsProdIdent.Tenant = tenant;
                    supplierInvoiceItemsProdIdentPM.Add(supplierInvoiceItemsProdIdent);
                }
            }
            return supplierInvoiceItemsProdIdentPM;
        }

        private List<SupplierInvoiceItemsDescriptPM> GetSupplierInvoiceItemsDescript(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsDescriptPM> supplierInvoiceItemsDescriptPM = new List<SupplierInvoiceItemsDescriptPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].ProductName != null)
            {
                foreach (var GoodsItemDescript in governmentAgencyGoodsItem.Commodity.Classification[0].ProductName)
                {
                    SupplierInvoiceItemsDescriptPM supplierInvoiceItemsDescript = new SupplierInvoiceItemsDescriptPM();
                    supplierInvoiceItemsDescript.DeclarationId = declarationId;
                    supplierInvoiceItemsDescript.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsDescript.TypeCode = GetValueCodeType(GoodsItemDescript.NameQualifierCode);
                    supplierInvoiceItemsDescript.Description = GetValueTextType(GoodsItemDescript.Name);
                    supplierInvoiceItemsDescript.Tenant = tenant;
                    supplierInvoiceItemsDescriptPM.Add(supplierInvoiceItemsDescript);
                }
            }
            return supplierInvoiceItemsDescriptPM;
        }

        private List<SupplierInvoiceItemsLevyPM> GetSupplierInvoiceItemLevy(DeclarationGoodsShipmentGovernmentAgencyGoodsItem governmentAgencyGoodsItem, Declaration declaration, string declarationId, int tenant)
        {
            List<SupplierInvoiceItemsLevyPM> supplierInvoiceItemsLevyPM = new List<SupplierInvoiceItemsLevyPM>();


            if (governmentAgencyGoodsItem != null && governmentAgencyGoodsItem.Commodity.Classification[0].TradeLevyAndExampt != null)
            {
                foreach (var GoodsItemLevy in governmentAgencyGoodsItem.Commodity.Classification[0].TradeLevyAndExampt)
                {
                    SupplierInvoiceItemsLevyPM supplierInvoiceItemsLevy = new SupplierInvoiceItemsLevyPM();
                    supplierInvoiceItemsLevy.DeclarationId = declarationId;
                    supplierInvoiceItemsLevy.ChangeSetOp = ChangeSetOperation.Insert;
                    supplierInvoiceItemsLevy.TradeLevyExamptCode = GetValueCodeType(GoodsItemLevy.TradeLevyExamptCode);
                    supplierInvoiceItemsLevy.TradeLevyNumber = GetValueIDType(GoodsItemLevy.TradeLevyNumber);
                    supplierInvoiceItemsLevy.Tenant = tenant;
                    supplierInvoiceItemsLevyPM.Add(supplierInvoiceItemsLevy);
                }
            }
            return supplierInvoiceItemsLevyPM;
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
                    supplierInvoiceItemVehiclePM.IdentifierID = GetValueIDType(vehicle.ID);
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
                if (declarationGoodsShipment.Invoice.DMExtensions.CustomsValuation != null)
                {
                    foreach (var customsValuation in declarationGoodsShipment.Invoice.DMExtensions.CustomsValuation)
                    {
                        //string[] ChargesTypeCode = new string[] { "67", "144", "I02" };
                        //if (!ChargesTypeCode.Contains(GetValueCodeType(customsValuation.ChargesTypeCode)))
                        //{
                            if (GetValueAmountType(customsValuation.OtherChargeDeductionAmount) == 0) continue;

                            SupplierInvoiceModificationPM supplierInvoiceModificationPM = new SupplierInvoiceModificationPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                DeclarationId = declarationId,
                                TypeCode = GetValueCodeType(customsValuation.ChargesTypeCode),
                                CurrencyTypeCode = customsValuation.OtherChargeDeductionAmount.currencyID.ToString(),
                                Amount = GetValueAmountType(customsValuation.OtherChargeDeductionAmount),
                                Tenant = tenant
                            };
                            supplierInvoiceModificationPMs.Add(supplierInvoiceModificationPM);
                        //}

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
                if (additionalDocument.DMExtensions != null)
                {
                    supplierInvioceItemCertificatPM.ResConfirmationTypeCode = GetValueCodeType(additionalDocument.DMExtensions.LPCOTypeCode);
                    supplierInvioceItemCertificatPM.ReqConfirmationTypeCode = GetValueCodeType(additionalDocument.DMExtensions.RequirementLicenseType);
                    var entity = CustomsAttachmentID(declarationId, Convert.ToInt32(additionalDocument.DMExtensions.SequenceNumeric), tenant);
                    supplierInvioceItemCertificatPM.CustomsAttachmentID = GetValueIDType(additionalDocument.DMExtensions.ExternalAttachmentID) ?? entity;
                    supplierInvioceItemCertificatPM.SequenceNumeric = Convert.ToInt32(additionalDocument.DMExtensions.SequenceNumeric);

                }
                supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Insert;
                supplierInvioceItemCertificatPM.Tenant = tenant;
                supplierInvioceItemCertificatPMs.Add(supplierInvioceItemCertificatPM);
            }
            return supplierInvioceItemCertificatPMs;
        }
          private string CustomsAttachmentID(string declarationId, int SequenceNumeric,int tenant)
          {
          var   context = CustomContext.GetContext(tenant);

            string entity = (from a in context.SupplierInvioceItemCertificats
                             where (a.DeclarationId == declarationId && a.SequenceNumeric == SequenceNumeric)
                             select a.CustomsAttachmentID).FirstOrDefault();
            return entity;
        }
        private List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificats(SupplierInvoicePM supplierInvoicePM, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            //if (supplierInvoiceItemPM.SupplierInvioceItemCertificats != null && supplierInvoiceItemPM.SupplierInvioceItemCertificats.Count() > 0)
            //{
            //    return supplierInvoiceItemPM.SupplierInvioceItemCertificats;
            //}

            //var supplierInvioceItemCertificatPMList = new List<SupplierInvioceItemCertificatPM>();
            var supplierInvioceItemCertificatPMList = supplierInvoiceItemPM.SupplierInvioceItemCertificats;

            List<string> certificateCodeListFromErrosXml = GetCertificateCodeListFromErrosXml("SupplierInvoice", supplierInvoicePM.SequenceNumeric.ToString(), "SupplierInvoiceItem", supplierInvoiceItemPM.SequenceNumeric.ToString());

            if (certificateCodeListFromErrosXml == null)
            {
                return supplierInvioceItemCertificatPMList;
                return null;
            }
            foreach (var certificateCodeFromErrosXml in certificateCodeListFromErrosXml)
            {
                //Check if the code exists current SupplierInvioceItemCertificats
                List<string> entityList = (from a in supplierInvoiceItemPM.SupplierInvioceItemCertificats
                                           where (a.ReqConfirmationTypeCode == certificateCodeFromErrosXml)
                                           select a.ReqConfirmationTypeCode).ToList();

                //If it does NOT exist - Add it to SupplierInvioceItemCertificat
                if (entityList.Count == 0)
                {
                    SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                    supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Insert;

                    supplierInvioceItemCertificatPM.Tenant = this._MyDeclarationPM.Tenant;
                    supplierInvioceItemCertificatPM.ReqConfirmationTypeCode = certificateCodeFromErrosXml;

                    supplierInvioceItemCertificatPMList.Add(supplierInvioceItemCertificatPM);
                }
            }
            return supplierInvioceItemCertificatPMList;
        }

        private List<string> GetCertificateCodeListFromErrosXml(string myChild1Type, string myChild1Sequence, string myChild2Type, string myChild2Sequence)
        {
            if (_MyDeclarationError == null)
            {
                return null;
            }
            if (_MyDeclarationError.Entitites.Count == 0)
            {
                return null;
            }

            List<string> certificateCodeListFromErrosXml = new List<string>();

            //Get all 'Entity' for the SupplierInvoiceItem
            List<Entity> entityList = (from a in _MyDeclarationError.Entitites
                                       where (a.Child1Type == myChild1Type && a.Child1Sequence == myChild1Sequence
                                       && a.Child2Type == myChild2Type && a.Child2Sequence == myChild2Sequence)
                                       select a).ToList();

            if (entityList.Count > 0) //Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
            //if (entityList.Count != null)
            {
                //Go over all the 'Entity'
                foreach (var entity in entityList)
                {
                    if (entity.FieldErrors != null)
                    {
                        //Go over all the 'FieldErrors'
                        foreach (var fieldErrors in entity.FieldErrors)
                        {
                            //Get all 'FieldErrors' for the 'FieldError'
                            List<field> fieldList = (from a in entity.FieldErrors
                                                     where (a.Code == "2592" && a.Fieldcode == "ClassificationCode")
                                                     select a).ToList();
                            //if (fieldList.Count != null)
                            if (fieldList.Count > 0)// Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                            {
                                //Go over all the 'FieldErrors'
                                foreach (var field in fieldList)
                                {
                                    var messageError = field.MessageError;
                                    messageError = messageError.Substring(messageError.IndexOf("#") + 1, messageError.LastIndexOf("#") - messageError.IndexOf("#") - 1);

                                    string[] codes = messageError.Split(new string[] { ";" }, StringSplitOptions.None);
                                    for (int i = 0; i < codes.Length; i++)
                                    {
                                        var code = codes[i];
                                        var charList = new List<char>(); // moran 19.10.16 - Bug 23715 - update handle -->
                                        charList.Add(' ');
                                        charList.Add(',');
                                        code = code.Trim(charList.ToArray());
                                        if (!string.IsNullOrWhiteSpace(code))
                                        {
                                            if (code.Length <= 4)//&& code.Length==3 Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                                            {
                                                var confirmationType = new ConfirmationTypeRepository(_MyDeclarationPM.Tenant);
                                                var myConfirmationType = confirmationType.GetSingle(code);
                                                if (myConfirmationType != null)
                                                {
                                                    certificateCodeListFromErrosXml.Add(code);
                                                }
                                                else
                                                {
                                                    LogMessagingUtil.Instance.AppendLine("Certificate " + code + " does not exist in DB.");
                                                }
                                            }
                                            else
                                            {
                                                LogMessagingUtil.Instance.AppendLine("Certificate " + code + " is too large.");
                                            }
                                        } // moran 19.10.16 - Bug 23715 - update handle <--
                                    }
                                }
                            }
                        }
                    }
                }
                return certificateCodeListFromErrosXml;
            }
            else
            {
                return null;
            }
        }
        private List<DeclarationTaxPM> GetDeclarationTaxesPM(Declaration declaration, DeclarationPM declarationPMOrg, string declarationId, int tenant)
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

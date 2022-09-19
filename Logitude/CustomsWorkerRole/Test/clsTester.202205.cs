using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.Test
{
    public partial class clsTester
    {



        public static void ReAnalyze2470_CustomsWithheld(int tenant, int maxretry)//מעוכב מכס	
        {

            var objectTableDecId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var qs = new DeclarationCourierStatusQueryService(tenant);
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
            var q = qs.GetQCustomsWithheld(tenant);
            var decs = q.ToList();
            Debug.WriteLine($"GetQCustomsWithheld({decs.Count})");
            var crsList = new List<string>();
            foreach (var decId in decs)
            {
                Debug.WriteLine($"currentdecId={decId}");
                var list = customsRequestsSheetQueryService.GetRequestByInterfaceTypeCode(tenant, "2470", objectTableDecId, decId, null);
                if (list?.Count > 0)
                {
                    Debug.WriteLine($"GetRequestByInterfaceTypeCode(2470).count={list?.Count}");
                    var customsRequestsSheetPM = list.FirstOrDefault(x => x.RequestStatusCode == "30");

                    if (customsRequestsSheetPM != null)
                    {

                        Debug.WriteLine($"customsRequestsSheetPM.id={customsRequestsSheetPM.Id}");
                        string mess = null;
                        try
                        {
                            if (maxretry > 0)
                            {
                                maxretry--;

                                ChangeAnalyzeFailAndReQueue(tenant, customsRequestsSheetPM);
                            }
                            mess = null;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.ToString());

                        }
                    }


                }

            }

        }
        public static void RequeByID(int tenant, string customsRequestsSheetId)
        {


            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
            var customsRequestsSheetPM = customsRequestsSheetQueryService.GetSingle(customsRequestsSheetId, false, false);


            ChangeAnalyzeFailAndReQueue(tenant, customsRequestsSheetPM);

        }

        public static void ChangeAnalyzeFailAndReQueue(int tenant, Logitude.Customs.Def.EntityPMs.CustomsRequestsSheetPM customsRequestsSheetPM)
        {

            if (customsRequestsSheetPM?.RequestStatusEnum != Logitude.CustomsMessaging.Common.ResponseData.SheetStatusEnum.Analyzed)
            {
                Debug.WriteLine($"status not Analyzed-abort {customsRequestsSheetPM?.Id}");
                return;
            }

            using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
            {

                var customContext = CustomContext.GetContext(tenant);
                var customsRequestsSheetUpdateService = new CustomsRequestsSheetUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

                //customsRequestsSheetPM.RequestStatusCode = "25";
                customsRequestsSheetPM.RequestStatusEnum = Logitude.CustomsMessaging.Common.ResponseData.SheetStatusEnum.AnalyzeFailed;
                customsRequestsSheetPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                customsRequestsSheetUpdateService.Update(customsRequestsSheetPM, true);

                MessagingServiceFactoryHelper
                .ResolveAndReQueue(customsRequestsSheetPM.InterfaceTypeCode, customsRequestsSheetPM.Tenant, customsRequestsSheetPM.Id);
                Debug.WriteLine($"customsRequestsSheetPM.id={customsRequestsSheetPM.Id}:done");
                scopeNewCRS.Complete();

            }
        }

        public static void CheckCustomsContext()
        {
            var customContext = CustomContext.GetContext(6) as ICustomContext;
            customContext.DecisionTypes.FirstOrDefault();
            var properties = customContext.GetType().GetProperties();
            foreach (var prop in properties)
            {
                if (prop.PropertyType.AssemblyQualifiedName.Contains("DbSet"))
                {
                    dynamic d = prop.GetValue(customContext);
                    try
                    {
                        d.FirstOrDefault();
                    }
                    catch (Exception)
                    {


                    }
                }
            }


        }

        public static void TestSP()
        {

            var CustomFileNo = TableCounter.GetNumber(6, "DECL", "DC", null);

            var Id = IdCounter.GetNumber("Customs.Tapag", 6).ToString();

            var TapagNumber = CodeCounter.GetNumber("Customs.Tapag", 6).ToString();


        }
        public void CheckCustomContext()
        {
            var customContext = CustomContext.GetContext(0);
            customContext.AcceptanceStatuses.FirstOrDefault();
            customContext.AccumalationStates.FirstOrDefault();
            customContext.ActionCodes.FirstOrDefault();
            customContext.AddressContactStates.FirstOrDefault();
            customContext.AddressPurposes.FirstOrDefault();
            customContext.AgentTalkBackTypes.FirstOrDefault();
            customContext.AmedmentTypes.FirstOrDefault();
            customContext.AmendCancellRequestInitiators.FirstOrDefault();
            customContext.AmendmentFieldReasonTypes.FirstOrDefault();
            customContext.AmendmentFieldStatusTypes.FirstOrDefault();
            customContext.AmendmentRequestStatuses.FirstOrDefault();
            customContext.AmendmentStatuses.FirstOrDefault();
            customContext.AmendmentTypes.FirstOrDefault();
            customContext.AmendRequestRejectReasonTypes.FirstOrDefault();
            customContext.AmountTypes.FirstOrDefault();
            customContext.ApprovedProfessions.FirstOrDefault();
            customContext.AssigneeNotificationTypes.FirstOrDefault();
            customContext.AttachmentTypes.FirstOrDefault();
            customContext.Authorities.FirstOrDefault();
            customContext.AuthorizedSignerPermits.FirstOrDefault();
            customContext.AutonomyRegionTypes.FirstOrDefault();
            customContext.AutonomyTypes.FirstOrDefault();
            customContext.Banks.FirstOrDefault();
            customContext.BuyerRoleTypes.FirstOrDefault();
            customContext.CancellationReasonRequestTypes.FirstOrDefault();
            customContext.CancellationRequestStatuses.FirstOrDefault();
            customContext.CancelRequestRejectReasonTypes.FirstOrDefault();
            customContext.CargoIdentifireTypes.FirstOrDefault();
            customContext.CargoIdentityQualifiers.FirstOrDefault();
            customContext.CargoSeals.FirstOrDefault();
            customContext.CargoSealIdentifiers.FirstOrDefault();
            customContext.CargoSplitRequestStatuses.FirstOrDefault();
            customContext.CargoStatuses.FirstOrDefault();
            customContext.CargoTypes.FirstOrDefault();
            customContext.CertificateExemptionTypes.FirstOrDefault();
            customContext.CertificatesStatuses.FirstOrDefault();
            customContext.CheckEntityTypes.FirstOrDefault();
            customContext.CheckEssenceLookups.FirstOrDefault();
            customContext.CheckQueueTypes.FirstOrDefault();
            customContext.CheckRepresentativeTypes.FirstOrDefault();
            customContext.CheckTypeLookups.FirstOrDefault();
            customContext.Cities.FirstOrDefault();
            customContext.Claims.FirstOrDefault();
            customContext.ClaimEntities.FirstOrDefault();
            customContext.ClaimExplanationCodes.FirstOrDefault();
            customContext.ClaimImporterDeclarsP3Lois.FirstOrDefault();
            customContext.ClaimImporterDeclarsPage3s.FirstOrDefault();
            customContext.ClaimImporterDeclarsPage3As.FirstOrDefault();
            customContext.ClaimImporterDeclarsPage3Bs.FirstOrDefault();
            customContext.ClaimReasonTypes.FirstOrDefault();
            customContext.ClaimsRelatedEntitiesAmounts.FirstOrDefault();
            customContext.ClaimsRelatedEntitiesReasons.FirstOrDefault();
            customContext.ClaimsRelatedEntitiesRefunds.FirstOrDefault();
            customContext.ClaimsRelatedEntitiesSeizures.FirstOrDefault();
            customContext.ClaimsRelatedEntities.FirstOrDefault();
            customContext.ClaimsRelatedEntsExpDeclars.FirstOrDefault();
            customContext.ClaimsRelatedEntsReasonsExps.FirstOrDefault();
            customContext.ClassificationTypes.FirstOrDefault();
            customContext.Clients.FirstOrDefault();
            customContext.ClientAddresses.FirstOrDefault();
            customContext.ClientDrivingLicenses.FirstOrDefault();
            customContext.ClientDrivingLicenseTypes.FirstOrDefault();
            customContext.ClientsAddressCommTypes.FirstOrDefault();
            customContext.ClientsPoas.FirstOrDefault();
            customContext.ClosedTableStatus.FirstOrDefault();
            customContext.CollateralAnswerStatus.FirstOrDefault();
            customContext.CollateralAnswerTypes.FirstOrDefault();
            customContext.CollateralRequestStatus.FirstOrDefault();
            customContext.CollateralsRequestFileConds.FirstOrDefault();
            customContext.CollateralTypes.FirstOrDefault();
            customContext.CommercialSales.FirstOrDefault();
            customContext.CommunicationTypes.FirstOrDefault();
            customContext.ConfirmationTypes.FirstOrDefault();
            customContext.Consignments.FirstOrDefault();
            customContext.ConsignmentInternalTransitions.FirstOrDefault();
            customContext.ConsignmentPackages.FirstOrDefault();
            customContext.ConsignmentPackDangers.FirstOrDefault();
            customContext.ConstraintApprovalDecisions.FirstOrDefault();
            customContext.ConstraintProcessTypes.FirstOrDefault();
            customContext.ConstraintStatuses.FirstOrDefault();
            customContext.ConstraintTypes.FirstOrDefault();
            customContext.ContactRoleTypes.FirstOrDefault();
            customContext.Containerizations.FirstOrDefault();
            customContext.ContainerizationHataraStatuses.FirstOrDefault();
            customContext.ContainerizationStatusCodes.FirstOrDefault();
            customContext.ContainerTypes.FirstOrDefault();
            customContext.ContinuousMessagesTypeCodes.FirstOrDefault();
            customContext.ContinuousRequestTypes.FirstOrDefault();
            customContext.ConverterTypes.FirstOrDefault();
            customContext.CoolingReportingMethods.FirstOrDefault();
            customContext.CountryGroups.FirstOrDefault();
            customContext.CourierCustomStatuses.FirstOrDefault();
            customContext.CourierDeclarations.FirstOrDefault();
            customContext.CourierDeclarationStatuses.FirstOrDefault();
            customContext.CourierManifestStatuses.FirstOrDefault();
            customContext.CourierMasters.FirstOrDefault();
            customContext.CourierPaymentStatuses.FirstOrDefault();
            customContext.CourierPendingReasons.FirstOrDefault();
            customContext.CourierStatuses.FirstOrDefault();
            customContext.CouriersVats.FirstOrDefault();
            customContext.CourtInstances.FirstOrDefault();
            customContext.CurrencyTypes.FirstOrDefault();
            customContext.CurrencyTypeTenants.FirstOrDefault();
            customContext.CustomBanks.FirstOrDefault();
            customContext.CustomBanksCards.FirstOrDefault();
            customContext.CustomDocumentTypes.FirstOrDefault();
            customContext.CustomDocumentTypeMetaData.FirstOrDefault();
            customContext.CustomerActivityTypes.FirstOrDefault();
            customContext.CustomerClassificationTypes.FirstOrDefault();
            customContext.CustomerIdentificationTypes.FirstOrDefault();
            customContext.CustomerIdentifyTypes.FirstOrDefault();
            customContext.CustomerIndicationTypes.FirstOrDefault();
            customContext.CustomerRoleTypes.FirstOrDefault();
            customContext.CustomerTypeGenerals.FirstOrDefault();
            customContext.CustomMetaDataTypes.FirstOrDefault();
            customContext.CustomsAddressTypes.FirstOrDefault();
            customContext.CustomsAirlines.FirstOrDefault();
            customContext.CustomsAutonomyKeywords.FirstOrDefault();
            customContext.CustomsBooks.FirstOrDefault();
            customContext.CustomsBookTypes.FirstOrDefault();
            customContext.CustomsBranches.FirstOrDefault();
            customContext.CustomsClosedTables.FirstOrDefault();
            customContext.CustomsCollaterals.FirstOrDefault();
            customContext.CustomsCollateralsAnswers.FirstOrDefault();
            customContext.CustomsCollateralsConditions.FirstOrDefault();
            customContext.CustomsCountries.FirstOrDefault();
            customContext.CustomsDocuments.FirstOrDefault();
            customContext.CustomsDocumentMetaDataValues.FirstOrDefault();
            customContext.CustomsDocumentPointers.FirstOrDefault();
            customContext.CustomsDocumentsDefinitions.FirstOrDefault();
            customContext.CustomsDocumentStatusTypes.FirstOrDefault();
            customContext.CustomsDocumentsTickets.FirstOrDefault();
            customContext.CustomsDocumentUploads.FirstOrDefault();
            customContext.CustomsEnvironmentSettings.FirstOrDefault();
            customContext.CustomsEnvoirmentTypes.FirstOrDefault();
            customContext.CustomsExchangeRates.FirstOrDefault();
            customContext.CustomsGenerals.FirstOrDefault();
            customContext.CustomsHouseTypes.FirstOrDefault();
            customContext.CustomsHouseTypeAdditionals.FirstOrDefault();
            customContext.CustomsInsuranceCompanies.FirstOrDefault();
            customContext.CustomsItems.FirstOrDefault();
            customContext.CustomsItemDetailsHistorys.FirstOrDefault();
            customContext.CustomsPartnerFtps.FirstOrDefault();
            customContext.CustomsPartnersItems.FirstOrDefault();
            customContext.CustomsPaymentTerms.FirstOrDefault();
            customContext.CustomsRequestsSheets.FirstOrDefault();
            customContext.CustomsRequestsSheetStatuses.FirstOrDefault();
            customContext.CustomsRequiredFields.FirstOrDefault();
            customContext.CustomsSettings.FirstOrDefault();
            customContext.CustomsShips.FirstOrDefault();
            customContext.CustomsTransportModes.FirstOrDefault();
            customContext.CustomsVendors.FirstOrDefault();
            customContext.CustomsVerificationStatusTypes.FirstOrDefault();
            customContext.DangerousGoodsPackingReqs.FirstOrDefault();
            customContext.DBMigrations.FirstOrDefault();
            customContext.DBMigrationLines.FirstOrDefault();
            customContext.DebtNotificationTypes.FirstOrDefault();
            customContext.DecCargoSplitCargoIdentifiers.FirstOrDefault();
            customContext.DecCargoSplitCons.FirstOrDefault();
            customContext.DecCargoSplitConsItems.FirstOrDefault();
            customContext.DecCargoSplitConsPackDets.FirstOrDefault();
            customContext.DecDangersContacts.FirstOrDefault();
            customContext.DecisionTypes.FirstOrDefault();
            customContext.Declarations.FirstOrDefault();
            customContext.DeclarationCargoSplits.FirstOrDefault();
            customContext.DeclarationCasualDetailses.FirstOrDefault();
            customContext.DeclarationConsAcceptances.FirstOrDefault();
            customContext.DeclarationConstraints.FirstOrDefault();
            customContext.DeclarationCourierStatuses.FirstOrDefault();
            customContext.DeclarationErrorMappings.FirstOrDefault();
            customContext.DeclarationExportRecipients.FirstOrDefault();
            customContext.DeclarationMamanSpecialActions.FirstOrDefault();
            customContext.DeclarationPayments.FirstOrDefault();
            customContext.DeclarationPaymentMethods.FirstOrDefault();
            customContext.DeclarationPaymentProtests.FirstOrDefault();
            customContext.DeclarationPendings.FirstOrDefault();
            customContext.DeclarationReferantDatas.FirstOrDefault();
            customContext.DeclarationStatementTypes.FirstOrDefault();
            customContext.DeclarationStatuses.FirstOrDefault();
            customContext.DeclarationStatusTypes.FirstOrDefault();
            customContext.DeclarationTaxes.FirstOrDefault();
            customContext.Deficits.FirstOrDefault();
            customContext.DeficitConnFileParagraphTypes.FirstOrDefault();
            customContext.DeficitDecisions.FirstOrDefault();
            customContext.DeliverySiteTypes.FirstOrDefault();
            customContext.DeliveryTypes.FirstOrDefault();
            customContext.DemanderTypes.FirstOrDefault();
            customContext.Deposits.FirstOrDefault();
            customContext.DepositConditions.FirstOrDefault();
            customContext.DepositCustomerActivities.FirstOrDefault();
            customContext.DepositEssenceTypes.FirstOrDefault();
            customContext.DepositFileTypes.FirstOrDefault();
            customContext.DocumentRejectTypes.FirstOrDefault();
            customContext.DocumentTypeCustomsData.FirstOrDefault();
            customContext.EntitlementTypes.FirstOrDefault();
            customContext.EntityTypeLookups.FirstOrDefault();
            customContext.ExceptionReasons.FirstOrDefault();
            customContext.ExportDeclarationClosingDatas.FirstOrDefault();
            customContext.ExportDeliveryDocumentMessages.FirstOrDefault();
            customContext.ExporterRoleTypes.FirstOrDefault();
            customContext.ExportLogisticPermitActions.FirstOrDefault();
            customContext.ExportReferences.FirstOrDefault();
            customContext.ExportStorages.FirstOrDefault();
            customContext.ExternalFieldMappings.FirstOrDefault();
            customContext.FacilitationTypes.FirstOrDefault();
            customContext.FaultInspectionTypes.FirstOrDefault();
            customContext.FclLclCodes.FirstOrDefault();
            customContext.FreightPaymentMethods.FirstOrDefault();
            customContext.FuelTypes.FirstOrDefault();
            customContext.FullnessCodes.FirstOrDefault();
            customContext.GatepassRequests.FirstOrDefault();
            customContext.GatepassReturnCodes.FirstOrDefault();
            customContext.Genders.FirstOrDefault();
            customContext.GovernmentProcedureTypes.FirstOrDefault();
            customContext.Guarantees.FirstOrDefault();
            customContext.GuaranteeCertificateTypes.FirstOrDefault();
            customContext.GuaranteeConditions.FirstOrDefault();
            customContext.GuaranteeCustomerActivities.FirstOrDefault();
            customContext.HandingCodes.FirstOrDefault();
            customContext.HazardousSubstances.FirstOrDefault();
            customContext.ImporterDeclarationTypes.FirstOrDefault();
            customContext.ImporterDespositions.FirstOrDefault();
            customContext.ImporterPeriodicDeclarStatuses.FirstOrDefault();
            customContext.ImporterTypeForClaims.FirstOrDefault();
            customContext.IncotemrsFileValidations.FirstOrDefault();
            customContext.InterfaceManagements.FirstOrDefault();
            customContext.InterfaceSendOptions.FirstOrDefault();
            customContext.InterfaceTenantDefinitions.FirstOrDefault();
            customContext.InternalBorderSiteTypes.FirstOrDefault();
            customContext.InternationalSites.FirstOrDefault();
            customContext.InvoiceTypes.FirstOrDefault();
            customContext.ItemGovernmentProcedureTypes.FirstOrDefault();
            customContext.LastReleaseFromWarehouses.FirstOrDefault();
            customContext.LeadDocumentExceptionTypes.FirstOrDefault();
            customContext.LeadDocumentTypes.FirstOrDefault();
            customContext.LoadingSiteTypes.FirstOrDefault();
            customContext.LogisticActionRequests.FirstOrDefault();
            customContext.LogisticActionRequestTypes.FirstOrDefault();
            customContext.LogisticActionResponseReqSes.FirstOrDefault();
            customContext.LogisticPermits.FirstOrDefault();
            customContext.LogisticsReferenceTypes.FirstOrDefault();
            customContext.MamanSpecialActions.FirstOrDefault();
            customContext.MamanSpecialActionStatuses.FirstOrDefault();
            customContext.MamanStatuses.FirstOrDefault();
            customContext.ManifestCargoStatuses.FirstOrDefault();
            customContext.MAWBTypes.FirstOrDefault();
            customContext.MeasureQualifier.FirstOrDefault();
            customContext.MeasurmentUnits.FirstOrDefault();
            customContext.ModificationAndDiscountTypes.FirstOrDefault();
            customContext.MorningMessageTypes.FirstOrDefault();
            customContext.NbcDeclarationTypes.FirstOrDefault();
            customContext.NDMessageActionCodes.FirstOrDefault();
            customContext.Notifications.FirstOrDefault();
            customContext.NotificationDefinitions.FirstOrDefault();
            customContext.NotificationReplies.FirstOrDefault();
            customContext.NotificationTenantDefinition.FirstOrDefault();
            customContext.NotificationTypes.FirstOrDefault();
            customContext.OrganizationUnitTypes.FirstOrDefault();
            customContext.PackageMeasureQualifiers.FirstOrDefault();
            customContext.PackingTypes.FirstOrDefault();
            customContext.ParagraphTypes.FirstOrDefault();
            customContext.PartyRelationshipTypes.FirstOrDefault();
            customContext.PassportTypes.FirstOrDefault();
            customContext.PayerActivityTypes.FirstOrDefault();
            customContext.PayerTypes.FirstOrDefault();
            customContext.PaymentMethodStatus.FirstOrDefault();
            customContext.PaymentMethodTypes.FirstOrDefault();
            customContext.PaymentOrders.FirstOrDefault();
            customContext.PaymentOrderConnectionTables.FirstOrDefault();
            customContext.PaymentOrderLines.FirstOrDefault();
            customContext.PaymentOrderMethods.FirstOrDefault();
            customContext.PaymentOrderProtestReasons.FirstOrDefault();
            customContext.PaymentOrderStatus.FirstOrDefault();
            customContext.PaymentOrderTypes.FirstOrDefault();
            customContext.PaymentProcesses.FirstOrDefault();
            customContext.PaymentProtestTypes.FirstOrDefault();
            customContext.PaymentTypes.FirstOrDefault();
            customContext.PendingByKeywords.FirstOrDefault();
            customContext.PendingErrorPlaces.FirstOrDefault();
            customContext.PhysicalChecks.FirstOrDefault();
            customContext.PhysicalCheckOperations.FirstOrDefault();
            customContext.PhysicalCheckSearchResultTypes.FirstOrDefault();
            customContext.PhysicalCheckStatusMessages.FirstOrDefault();
            customContext.PoaAuthorizationTypeLookups.FirstOrDefault();
            customContext.PoaStatusTypeLookUps.FirstOrDefault();
            customContext.PointerLevels.FirstOrDefault();
            //customContext.ProceduralFaults.FirstOrDefault();
            customContext.ProceduralFaultInProcessTypes.FirstOrDefault();
            customContext.ProceduralFaultInSourceTypes.FirstOrDefault();
            customContext.ProceduralFaultsConnEntities.FirstOrDefault();
            customContext.ProceduralFaultStatuses.FirstOrDefault();
            customContext.ProceduralFaultTypes.FirstOrDefault();
            customContext.ProcessingReasons.FirstOrDefault();
            customContext.ProductIdentificationTypes.FirstOrDefault();
            customContext.ProductNameTypes.FirstOrDefault();
            customContext.PropertiesDetailsHistorys.FirstOrDefault();
            customContext.RansomViolationTypes.FirstOrDefault();
            customContext.ReferantExceptions.FirstOrDefault();
            customContext.ReferantTeams.FirstOrDefault();
            customContext.ReferenceInputTypes.FirstOrDefault();
            customContext.ReferenceStatuses.FirstOrDefault();
            customContext.RefundCustomerActivityTypes.FirstOrDefault();
            customContext.RegisteredWarehouseSiteTypes.FirstOrDefault();
            customContext.ReleaseMessageTypes.FirstOrDefault();
            customContext.RequestStatuses.FirstOrDefault();
            customContext.RequestTypes.FirstOrDefault();
            customContext.RequiredGuaranteeTypes.FirstOrDefault();
            customContext.ReturnConditions.FirstOrDefault();
            customContext.SalesTaxExemptionTypes.FirstOrDefault();
            customContext.SealCompleteness.FirstOrDefault();
            customContext.SealTypes.FirstOrDefault();
            customContext.SealUpdateReasonTypes.FirstOrDefault();
            customContext.SecurityClearenceTypeCodes.FirstOrDefault();
            customContext.SeizureFactorTypes.FirstOrDefault();
            customContext.SeizureMethodTypes.FirstOrDefault();
            customContext.SignatureTypes.FirstOrDefault();
            customContext.SiteLookups.FirstOrDefault();
            customContext.SiteTypes.FirstOrDefault();
            customContext.SpecialActionDescriptionTypes.FirstOrDefault();
            customContext.SpecializationTypes.FirstOrDefault();
            customContext.SplitOrMergeReasons.FirstOrDefault();
            customContext.StatusFieldTypes.FirstOrDefault();
            customContext.StorageMessageTypes.FirstOrDefault();
            customContext.StorageStatuses.FirstOrDefault();
            customContext.StorageStatusTables.FirstOrDefault();
            customContext.StuffingSiteTypes.FirstOrDefault();
            customContext.SubCountries.FirstOrDefault();
            customContext.SuppInvoiceItemsAbachStatement.FirstOrDefault();
            customContext.SupplierInvioceItemCertificats.FirstOrDefault();
            customContext.SupplierInvoices.FirstOrDefault();
            customContext.SupplierInvoiceFreightAmounts.FirstOrDefault();
            customContext.SupplierInvoiceItems.FirstOrDefault();
            customContext.SupplierInvoiceItemModVehicles.FirstOrDefault();
            customContext.SupplierInvoiceItemProcesTypes.FirstOrDefault();
            customContext.SupplierInvoiceItemsConDeclars.FirstOrDefault();
            customContext.SupplierInvoiceItemsDescripts.FirstOrDefault();
            customContext.SupplierInvoiceItemsLevies.FirstOrDefault();
            customContext.SupplierInvoiceItemsMods.FirstOrDefault();
            customContext.SupplierInvoiceItemsPrices.FirstOrDefault();
            customContext.SupplierInvoiceItemsProdIdents.FirstOrDefault();
            customContext.SupplierInvoiceItemsSerialNums.FirstOrDefault();
            customContext.SupplierInvoiceItemsTaxes.FirstOrDefault();
            customContext.SupplierInvoiceItemVehicles.FirstOrDefault();
            customContext.SupplierInvoiceItemVehicleAdds.FirstOrDefault();
            customContext.SupplierInvoiceItemVehicleMods.FirstOrDefault();
            customContext.SupplierInvoiceModifications.FirstOrDefault();
            customContext.SupplierInvoicePayments.FirstOrDefault();
            customContext.SupplierInvoiceUCRs.FirstOrDefault();
            customContext.SupplierPartyTypes.FirstOrDefault();
            customContext.Tapags.FirstOrDefault();
            customContext.TapagConnectionTables.FirstOrDefault();
            customContext.TapagTypes.FirstOrDefault();
            customContext.TermsOfSaleTypes.FirstOrDefault();
            customContext.TPGFileTypes.FirstOrDefault();
            customContext.TradeAgreements.FirstOrDefault();
            customContext.TradeAgreementProtocols.FirstOrDefault();
            customContext.TradeLevyExamptTypes.FirstOrDefault();
            customContext.TransactionNatureTypes.FirstOrDefault();
            customContext.TransferCargoMethodTypes.FirstOrDefault();
            customContext.TransportMeansTypes.FirstOrDefault();
            customContext.TreatmentWays.FirstOrDefault();
            customContext.UIMessages.FirstOrDefault();
            customContext.UIMessageAdditionals.FirstOrDefault();
            customContext.UnloadingSiteType.FirstOrDefault();
            customContext.UpdateCodes.FirstOrDefault();
            customContext.ValidCustomsItems.FirstOrDefault();
            customContext.Vehicles.FirstOrDefault();
            customContext.VehicleManufacturers.FirstOrDefault();
            customContext.VehicleOwners.FirstOrDefault();
            customContext.VehiclePoolTypes.FirstOrDefault();
            customContext.VehiclePriceListType.FirstOrDefault();
            customContext.VehicleReductionTypes.FirstOrDefault();
            customContext.VehicleSafeAccessoryInstlTypes.FirstOrDefault();
            customContext.VehicleSafetyAccessories.FirstOrDefault();
            customContext.VehicleSafetyAccessoryTypes.FirstOrDefault();
            customContext.VehicleStatuses.FirstOrDefault();
            customContext.VehicleTecnologyTypes.FirstOrDefault();
            customContext.VehicleTypes.FirstOrDefault();
            customContext.VendorCommissions.FirstOrDefault();
            customContext.VendorCommunications.FirstOrDefault();
            customContext.VendorStatuses.FirstOrDefault();
            customContext.VendorTransactionTypes.FirstOrDefault();
            customContext.VendorTypes.FirstOrDefault();
        }
    }
}

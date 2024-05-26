using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs.SignQueueBL;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Dca;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.WcfApi;

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
            var customContext = CustomContext.GetContext(1) as ICustomContext;

            //var A = (from a in customContext.ExportStorages
            //             where a.Id == "1-100"
            //         select a).Take(100).ToList();

            var test163 = customContext.Declarations.Take(100).ToList();

            var test0 = customContext.AcceptanceStatuses.Take(100).ToList();
            var test1 = customContext.AccumalationStates.Take(100).ToList();
            var test2 = customContext.ActionCodes.Take(100).ToList();
            var test3 = customContext.AddressContactStates.Take(100).ToList();
            var test4 = customContext.AddressPurposes.Take(100).ToList();
            var test5 = customContext.AgentTalkBackTypes.Take(100).ToList();
            var test6 = customContext.AmedmentTypes.Take(100).ToList();
            var test7 = customContext.AmendCancellRequestInitiators.Take(100).ToList();
            var test8 = customContext.AmendmentFieldReasonTypes.Take(100).ToList();
            var test9 = customContext.AmendmentFieldStatusTypes.Take(100).ToList();
            var test10 = customContext.AmendmentRequestStatuses.Take(100).ToList();
            var test11 = customContext.AmendmentStatuses.Take(100).ToList();
            var test12 = customContext.AmendmentTypes.Take(100).ToList();
            var test13 = customContext.AmendRequestRejectReasonTypes.Take(100).ToList();
            var test14 = customContext.AmountTypes.Take(100).ToList();
            var test15 = customContext.ApprovedProfessions.Take(100).ToList();
            var test16 = customContext.AssigneeNotificationTypes.Take(100).ToList();
            var test17 = customContext.AttachmentTypes.Take(100).ToList();
            var test18 = customContext.Authorities.Take(100).ToList();
            var test19 = customContext.AuthorizedSignerPermits.Take(100).ToList();
            var test20 = customContext.AutonomyRegionTypes.Take(100).ToList();
            var test21 = customContext.AutonomyTypes.Take(100).ToList();
            var test22 = customContext.Banks.Take(100).ToList();
            var test23 = customContext.BuyerRoleTypes.Take(100).ToList();
            var test24 = customContext.CancellationReasonRequestTypes.Take(100).ToList();
            var test25 = customContext.CancellationRequestStatuses.Take(100).ToList();
            var test26 = customContext.CancelRequestRejectReasonTypes.Take(100).ToList();
            var test27 = customContext.CargoIdentifireTypes.Take(100).ToList();
            var test28 = customContext.CargoIdentityQualifiers.Take(100).ToList();
            var test29 = customContext.CargoSeals.Take(100).ToList();
            var test30 = customContext.CargoSealIdentifiers.Take(100).ToList();
            var test31 = customContext.CargoSplitRequestStatuses.Take(100).ToList();
            var test32 = customContext.CargoStatuses.Take(100).ToList();
            var test33 = customContext.CargoTypes.Take(100).ToList();
            var test34 = customContext.CertificateExemptionTypes.Take(100).ToList();
            var test35 = customContext.CertificatesStatuses.Take(100).ToList();
            var test36 = customContext.CheckEntityTypes.Take(100).ToList();
            var test37 = customContext.CheckEssenceLookups.Take(100).ToList();
            var test38 = customContext.CheckQueueTypes.Take(100).ToList();
            var test39 = customContext.CheckRepresentativeTypes.Take(100).ToList();
            var test40 = customContext.CheckTypeLookups.Take(100).ToList();
            var test41 = customContext.Cities.Take(100).ToList();
            var test42 = customContext.Claims.Take(100).ToList();
            var test43 = customContext.ClaimEntities.Take(100).ToList();
            var test44 = customContext.ClaimExplanationCodes.Take(100).ToList();
            var test45 = customContext.ClaimImporterDeclarsP3Lois.Take(100).ToList();
            var test46 = customContext.ClaimImporterDeclarsPage3s.Take(100).ToList();
            var test47 = customContext.ClaimImporterDeclarsPage3As.Take(100).ToList();
            var test48 = customContext.ClaimImporterDeclarsPage3Bs.Take(100).ToList();
            var test49 = customContext.ClaimReasonTypes.Take(100).ToList();
            var test50 = customContext.ClaimsRelatedEntitiesAmounts.Take(100).ToList();
            var test51 = customContext.ClaimsRelatedEntitiesReasons.Take(100).ToList();
            var test52 = customContext.ClaimsRelatedEntitiesRefunds.Take(100).ToList();
            var test53 = customContext.ClaimsRelatedEntitiesSeizures.Take(100).ToList();
            var test54 = customContext.ClaimsRelatedEntities.Take(100).ToList();
            var test55 = customContext.ClaimsRelatedEntsExpDeclars.Take(100).ToList();
            var test56 = customContext.ClaimsRelatedEntsReasonsExps.Take(100).ToList();
            var test57 = customContext.ClassificationTypes.Take(100).ToList();
            var test58 = customContext.Clients.Take(100).ToList();
            var test59 = customContext.ClientAddresses.Take(100).ToList();
            var test60 = customContext.ClientDrivingLicenses.Take(100).ToList();
            var test61 = customContext.ClientDrivingLicenseTypes.Take(100).ToList();
            var test62 = customContext.ClientsAddressCommTypes.Take(100).ToList();
            var test63 = customContext.ClientsPoas.Take(100).ToList();
            var test64 = customContext.ClosedTableStatus.Take(100).ToList();
            var test65 = customContext.CollateralAnswerStatus.Take(100).ToList();
            var test66 = customContext.CollateralAnswerTypes.Take(100).ToList();
            var test67 = customContext.CollateralRequestStatus.Take(100).ToList();
            var test68 = customContext.CollateralsRequestFileConds.Take(100).ToList();
            var test69 = customContext.CollateralTypes.Take(100).ToList();
            var test70 = customContext.CommercialSales.Take(100).ToList();
            var test71 = customContext.CommunicationTypes.Take(100).ToList();
            var test72 = customContext.ConfirmationTypes.Take(100).ToList();
            var test73 = customContext.Consignments.Take(100).ToList();
            var test74 = customContext.ConsignmentInternalTransitions.Take(100).ToList();
            var test75 = customContext.ConsignmentPackages.Take(100).ToList();
            var test76 = customContext.ConsignmentPackDangers.Take(100).ToList();
            var test77 = customContext.ConstraintApprovalDecisions.Take(100).ToList();
            var test78 = customContext.ConstraintProcessTypes.Take(100).ToList();
            var test79 = customContext.ConstraintStatuses.Take(100).ToList();
            var test80 = customContext.ConstraintTypes.Take(100).ToList();
            var test81 = customContext.ContactRoleTypes.Take(100).ToList();
            var test82 = customContext.Containerizations.Take(100).ToList();
            var test83 = customContext.ContainerizationHataraStatuses.Take(100).ToList();
            var test84 = customContext.ContainerizationStatusCodes.Take(100).ToList();
            var test85 = customContext.ContainerTypes.Take(100).ToList();
            var test86 = customContext.ContinuousMessagesTypeCodes.Take(100).ToList();
            var test87 = customContext.ContinuousRequestTypes.Take(100).ToList();
            var test88 = customContext.ConverterTypes.Take(100).ToList();
            var test89 = customContext.CoolingReportingMethods.Take(100).ToList();
            var test90 = customContext.CountryGroups.Take(100).ToList();
            var test91 = customContext.CourierCustomStatuses.Take(100).ToList();
            var test92 = customContext.CourierDeclarations.Take(100).ToList();
            var test93 = customContext.CourierDeclarationStatuses.Take(100).ToList();
            var test94 = customContext.CourierManifestStatuses.Take(100).ToList();
            var test95 = customContext.CourierMasters.Take(100).ToList();
            var test96 = customContext.CourierPaymentStatuses.Take(100).ToList();
            var test97 = customContext.CourierPendingReasons.Take(100).ToList();
            var test98 = customContext.CourierStatuses.Take(100).ToList();
            var test99 = customContext.CouriersVats.Take(100).ToList();
            var test100 = customContext.CourtInstances.Take(100).ToList();
            var test101 = customContext.CurrencyTypes.Take(100).ToList();
            var test102 = customContext.CurrencyTypeTenants.Take(100).ToList();
            var test103 = customContext.CustomBanks.Take(100).ToList();
            var test104 = customContext.CustomBanksCards.Take(100).ToList();
            var test105 = customContext.CustomDocumentTypes.Take(100).ToList();
            var test106 = customContext.CustomDocumentTypeMetaData.Take(100).ToList();
            var test107 = customContext.CustomerActivityTypes.Take(100).ToList();
            var test108 = customContext.CustomerClassificationTypes.Take(100).ToList();
            var test109 = customContext.CustomerIdentificationTypes.Take(100).ToList();
            var test110 = customContext.CustomerIdentifyTypes.Take(100).ToList();
            var test111 = customContext.CustomerIndicationTypes.Take(100).ToList();
            var test112 = customContext.CustomerRoleTypes.Take(100).ToList();
            var test113 = customContext.CustomerTypeGenerals.Take(100).ToList();
            var test114 = customContext.CustomMetaDataTypes.Take(100).ToList();
            var test115 = customContext.CustomsAddressTypes.Take(100).ToList();
            var test116 = customContext.CustomsAirlines.Take(100).ToList();
            var test117 = customContext.CustomsAutonomyKeywords.Take(100).ToList();
            var test118 = customContext.CustomsBooks.Take(100).ToList();
            var test119 = customContext.CustomsBookTypes.Take(100).ToList();
            var test120 = customContext.CustomsBranches.Take(100).ToList();
            var test121 = customContext.CustomsClosedTables.Take(100).ToList();
            var test122 = customContext.CustomsCollaterals.Take(100).ToList();
            var test123 = customContext.CustomsCollateralsAnswers.Take(100).ToList();
            var test124 = customContext.CustomsCollateralsConditions.Take(100).ToList();
            var test125 = customContext.CustomsCountries.Take(100).ToList();
            var test126 = customContext.CustomsDocuments.Take(100).ToList();
            var test127 = customContext.CustomsDocumentMetaDataValues.Take(100).ToList();
            var test128 = customContext.CustomsDocumentPointers.Take(100).ToList();
            var test129 = customContext.CustomsDocumentsDefinitions.Take(100).ToList();
            var test130 = customContext.CustomsDocumentStatusTypes.Take(100).ToList();
            var test131 = customContext.CustomsDocumentsTickets.Take(100).ToList();
            var test132 = customContext.CustomsDocumentUploads.Take(100).ToList();
            var test133 = customContext.CustomsEnvironmentSettings.Take(100).ToList();
            var test134 = customContext.CustomsEnvoirmentTypes.Take(100).ToList();
            var test135 = customContext.CustomsExchangeRates.Take(100).ToList();
            var test136 = customContext.CustomsGenerals.Take(100).ToList();
            var test137 = customContext.CustomsHouseTypes.Take(100).ToList();
            var test138 = customContext.CustomsHouseTypeAdditionals.Take(100).ToList();
            var test139 = customContext.CustomsInsuranceCompanies.Take(100).ToList();
            var test140 = customContext.CustomsItems.Take(100).ToList();
            var test141 = customContext.CustomsItemDetailsHistorys.Take(100).ToList();
            var test142 = customContext.CustomsPartnerFtps.Take(100).ToList();
            var test143 = customContext.CustomsPartnersItems.Take(100).ToList();
            var test144 = customContext.CustomsPaymentTerms.Take(100).ToList();
            var test145 = customContext.CustomsRequestsSheets.Take(100).ToList();
            var test146 = customContext.CustomsRequestsSheetStatuses.Take(100).ToList();
            var test147 = customContext.CustomsRequiredFields.Take(100).ToList();
            var test148 = customContext.CustomsSettings.Take(100).ToList();
            var test149 = customContext.CustomsShips.Take(100).ToList();
            var test150 = customContext.CustomsTransportModes.Take(100).ToList();
            var test151 = customContext.CustomsVendors.Take(100).ToList();
            var test152 = customContext.CustomsVerificationStatusTypes.Take(100).ToList();
            var test153 = customContext.DangerousGoodsPackingReqs.Take(100).ToList();
            var test154 = customContext.DBMigrations.Take(100).ToList();
            var test155 = customContext.DBMigrationLines.Take(100).ToList();
            var test156 = customContext.DebtNotificationTypes.Take(100).ToList();
            var test157 = customContext.DecCargoSplitCargoIdentifiers.Take(100).ToList();
            var test158 = customContext.DecCargoSplitCons.Take(100).ToList();
            var test159 = customContext.DecCargoSplitConsItems.Take(100).ToList();
            var test160 = customContext.DecCargoSplitConsPackDets.Take(100).ToList();
            var test161 = customContext.DecDangersContacts.Take(100).ToList();
            var test162 = customContext.DecisionTypes.Take(100).ToList();
            
            var test164 = customContext.DeclarationCargoSplits.Take(100).ToList();
            var test165 = customContext.DeclarationCasualDetailses.Take(100).ToList();
            var test166 = customContext.DeclarationConsAcceptances.Take(100).ToList();
            var test167 = customContext.DeclarationConstraints.Take(100).ToList();
            var test168 = customContext.DeclarationCourierStatuses.Take(100).ToList();
            var test169 = customContext.DeclarationErrorMappings.Take(100).ToList();
            var test170 = customContext.DeclarationExportRecipients.Take(100).ToList();
            var test171 = customContext.DeclarationMamanSpecialActions.Take(100).ToList();
            var test172 = customContext.DeclarationPayments.Take(100).ToList();
            var test173 = customContext.DeclarationPaymentMethods.Take(100).ToList();
            var test174 = customContext.DeclarationPaymentProtests.Take(100).ToList();
            var test175 = customContext.DeclarationPendings.Take(100).ToList();
            var test176 = customContext.DeclarationReferantDatas.Take(100).ToList();
            var test177 = customContext.DeclarationStatementTypes.Take(100).ToList();
            var test178 = customContext.DeclarationStatuses.Take(100).ToList();
            var test179 = customContext.DeclarationStatusTypes.Take(100).ToList();
            var test180 = customContext.DeclarationTaxes.Take(100).ToList();
            var test181 = customContext.Deficits.Take(100).ToList();
            var test182 = customContext.DeficitConnFileParagraphTypes.Take(100).ToList();
            var test183 = customContext.DeficitDecisions.Take(100).ToList();
            var test184 = customContext.DeliverySiteTypes.Take(100).ToList();
            var test185 = customContext.DeliveryTypes.Take(100).ToList();
            var test186 = customContext.DemanderTypes.Take(100).ToList();
            var test187 = customContext.Deposits.Take(100).ToList();
            var test188 = customContext.DepositConditions.Take(100).ToList();
            var test189 = customContext.DepositCustomerActivities.Take(100).ToList();
            var test190 = customContext.DepositEssenceTypes.Take(100).ToList();
            var test191 = customContext.DepositFileTypes.Take(100).ToList();
            var test192 = customContext.DocumentRejectTypes.Take(100).ToList();
            var test193 = customContext.DocumentTypeCustomsData.Take(100).ToList();
            var test194 = customContext.EntitlementTypes.Take(100).ToList();
            var test195 = customContext.EntityTypeLookups.Take(100).ToList();
            var test196 = customContext.ExceptionReasons.Take(100).ToList();
            var test197 = customContext.ExportDeclarationClosingDatas.Take(100).ToList();
            var test198 = customContext.ExportDeliveryDocumentMessages.Take(100).ToList();
            var test199 = customContext.ExporterRoleTypes.Take(100).ToList();
            var test200 = customContext.ExportLogisticPermitActions.Take(100).ToList();
            var test201 = customContext.ExportReferences.Take(100).ToList();
            var test202 = customContext.ExportStorages.Take(100).ToList();
            var test203 = customContext.ExternalFieldMappings.Take(100).ToList();
            var test204 = customContext.FacilitationTypes.Take(100).ToList();
            var test205 = customContext.FaultInspectionTypes.Take(100).ToList();
            var test206 = customContext.FclLclCodes.Take(100).ToList();
            var test207 = customContext.FreightPaymentMethods.Take(100).ToList();
            var test208 = customContext.FuelTypes.Take(100).ToList();
            var test209 = customContext.FullnessCodes.Take(100).ToList();
            var test210 = customContext.GatepassRequests.Take(100).ToList();
            var test211 = customContext.GatepassReturnCodes.Take(100).ToList();
            var test212 = customContext.Genders.Take(100).ToList();
            var test213 = customContext.GovernmentProcedureTypes.Take(100).ToList();
            var test214 = customContext.Guarantees.Take(100).ToList();
            var test215 = customContext.GuaranteeCertificateTypes.Take(100).ToList();
            var test216 = customContext.GuaranteeConditions.Take(100).ToList();
            var test217 = customContext.GuaranteeCustomerActivities.Take(100).ToList();
            var test218 = customContext.HandingCodes.Take(100).ToList();
            var test219 = customContext.HazardousSubstances.Take(100).ToList();
            var test220 = customContext.ImporterDeclarationTypes.Take(100).ToList();
            var test221 = customContext.ImporterDespositions.Take(100).ToList();
            var test222 = customContext.ImporterPeriodicDeclarStatuses.Take(100).ToList();
            var test223 = customContext.ImporterTypeForClaims.Take(100).ToList();
            var test224 = customContext.IncotemrsFileValidations.Take(100).ToList();
            var test225 = customContext.InterfaceManagements.Take(100).ToList();
            var test226 = customContext.InterfaceSendOptions.Take(100).ToList();
            var test227 = customContext.InterfaceTenantDefinitions.Take(100).ToList();
            var test228 = customContext.InternalBorderSiteTypes.Take(100).ToList();
            var test229 = customContext.InternationalSites.Take(100).ToList();
            var test230 = customContext.InvoiceTypes.Take(100).ToList();
            var test231 = customContext.ItemGovernmentProcedureTypes.Take(100).ToList();
            var test232 = customContext.LastReleaseFromWarehouses.Take(100).ToList();
            var test233 = customContext.LeadDocumentExceptionTypes.Take(100).ToList();
            var test234 = customContext.LeadDocumentTypes.Take(100).ToList();
            var test235 = customContext.LoadingSiteTypes.Take(100).ToList();
            var test236 = customContext.LogisticActionRequests.Take(100).ToList();
            var test237 = customContext.LogisticActionRequestTypes.Take(100).ToList();
            var test238 = customContext.LogisticActionResponseReqSes.Take(100).ToList();
            var test239 = customContext.LogisticPermits.Take(100).ToList();
            var test240 = customContext.LogisticsReferenceTypes.Take(100).ToList();
            var test241 = customContext.MamanSpecialActions.Take(100).ToList();
            var test242 = customContext.MamanSpecialActionStatuses.Take(100).ToList();
            var test243 = customContext.MamanStatuses.Take(100).ToList();
            var test244 = customContext.ManifestCargoStatuses.Take(100).ToList();
            var test245 = customContext.MAWBTypes.Take(100).ToList();
            var test246 = customContext.MeasureQualifier.Take(100).ToList();
            var test247 = customContext.MeasurmentUnits.Take(100).ToList();
            var test248 = customContext.ModificationAndDiscountTypes.Take(100).ToList();
            var test249 = customContext.MorningMessageTypes.Take(100).ToList();
            var test250 = customContext.NbcDeclarationTypes.Take(100).ToList();
            var test251 = customContext.NDMessageActionCodes.Take(100).ToList();
            var test252 = customContext.Notifications.Take(100).ToList();
            var test253 = customContext.NotificationDefinitions.Take(100).ToList();
            var test254 = customContext.NotificationReplies.Take(100).ToList();
            var test255 = customContext.NotificationTenantDefinition.Take(100).ToList();
            var test256 = customContext.NotificationTypes.Take(100).ToList();
            var test257 = customContext.OrganizationUnitTypes.Take(100).ToList();
            var test258 = customContext.PackageMeasureQualifiers.Take(100).ToList();
            var test259 = customContext.PackingTypes.Take(100).ToList();
            var test260 = customContext.ParagraphTypes.Take(100).ToList();
            var test261 = customContext.PartyRelationshipTypes.Take(100).ToList();
            var test262 = customContext.PassportTypes.Take(100).ToList();
            var test263 = customContext.PayerActivityTypes.Take(100).ToList();
            var test264 = customContext.PayerTypes.Take(100).ToList();
            var test265 = customContext.PaymentMethodStatus.Take(100).ToList();
            var test266 = customContext.PaymentMethodTypes.Take(100).ToList();
            var test267 = customContext.PaymentOrders.Take(100).ToList();
            var test268 = customContext.PaymentOrderConnectionTables.Take(100).ToList();
            var test269 = customContext.PaymentOrderLines.Take(100).ToList();
            var test270 = customContext.PaymentOrderMethods.Take(100).ToList();
            var test271 = customContext.PaymentOrderProtestReasons.Take(100).ToList();
            var test272 = customContext.PaymentOrderStatus.Take(100).ToList();
            var test273 = customContext.PaymentOrderTypes.Take(100).ToList();
            var test274 = customContext.PaymentProcesses.Take(100).ToList();
            var test275 = customContext.PaymentProtestTypes.Take(100).ToList();
            var test276 = customContext.PaymentTypes.Take(100).ToList();
            var test277 = customContext.PendingByKeywords.Take(100).ToList();
            var test278 = customContext.PendingErrorPlaces.Take(100).ToList();
            var test279 = customContext.PhysicalChecks.Take(100).ToList();
            var test280 = customContext.PhysicalCheckOperations.Take(100).ToList();
            var test281 = customContext.PhysicalCheckSearchResultTypes.Take(100).ToList();
            var test282 = customContext.PhysicalCheckStatusMessages.Take(100).ToList();
            var test283 = customContext.PoaAuthorizationTypeLookups.Take(100).ToList();
            var test284 = customContext.PoaStatusTypeLookUps.Take(100).ToList();
            var test285 = customContext.PointerLevels.Take(100).ToList();
            var test286 = customContext.ProceduralFaults.Take(100).ToList();
            var test287 = customContext.ProceduralFaultInProcessTypes.Take(100).ToList();
            var test288 = customContext.ProceduralFaultInSourceTypes.Take(100).ToList();
            var test289 = customContext.ProceduralFaultsConnEntities.Take(100).ToList();
            var test290 = customContext.ProceduralFaultStatuses.Take(100).ToList();
            var test291 = customContext.ProceduralFaultTypes.Take(100).ToList();
            var test292 = customContext.ProcessingReasons.Take(100).ToList();
            var test293 = customContext.ProductIdentificationTypes.Take(100).ToList();
            var test294 = customContext.ProductNameTypes.Take(100).ToList();
            var test295 = customContext.PropertiesDetailsHistorys.Take(100).ToList();
            var test296 = customContext.RansomViolationTypes.Take(100).ToList();
            var test297 = customContext.ReferantExceptions.Take(100).ToList();
            var test298 = customContext.ReferantTeams.Take(100).ToList();
            var test299 = customContext.ReferenceInputTypes.Take(100).ToList();
            var test300 = customContext.ReferenceStatuses.Take(100).ToList();
            var test301 = customContext.RefundCustomerActivityTypes.Take(100).ToList();
            var test302 = customContext.RegisteredWarehouseSiteTypes.Take(100).ToList();
            var test303 = customContext.ReleaseMessageTypes.Take(100).ToList();
            var test304 = customContext.RequestStatuses.Take(100).ToList();
            var test305 = customContext.RequestTypes.Take(100).ToList();
            var test306 = customContext.RequiredGuaranteeTypes.Take(100).ToList();
            var test307 = customContext.ReturnConditions.Take(100).ToList();
            var test308 = customContext.SalesTaxExemptionTypes.Take(100).ToList();
            var test309 = customContext.SealCompleteness.Take(100).ToList();
            var test310 = customContext.SealTypes.Take(100).ToList();
            var test311 = customContext.SealUpdateReasonTypes.Take(100).ToList();
            var test312 = customContext.SecurityClearenceTypeCodes.Take(100).ToList();
            var test313 = customContext.SeizureFactorTypes.Take(100).ToList();
            var test314 = customContext.SeizureMethodTypes.Take(100).ToList();
            var test315 = customContext.SignatureTypes.Take(100).ToList();
            var test316 = customContext.SiteLookups.Take(100).ToList();
            var test317 = customContext.SiteTypes.Take(100).ToList();
            var test318 = customContext.SpecialActionDescriptionTypes.Take(100).ToList();
            var test319 = customContext.SpecializationTypes.Take(100).ToList();
            var test320 = customContext.SplitOrMergeReasons.Take(100).ToList();
            var test321 = customContext.StatusFieldTypes.Take(100).ToList();
            var test322 = customContext.StorageMessageTypes.Take(100).ToList();
            var test323 = customContext.StorageStatuses.Take(100).ToList();
            var test324 = customContext.StorageStatusTables.Take(100).ToList();
            var test325 = customContext.StuffingSiteTypes.Take(100).ToList();
            var test326 = customContext.SubCountries.Take(100).ToList();
            var test327 = customContext.SuppInvoiceItemsAbachStatement.Take(100).ToList();
            var test328 = customContext.SupplierInvioceItemCertificats.Take(100).ToList();
            var test329 = customContext.SupplierInvoices.Take(100).ToList();
            var test330 = customContext.SupplierInvoiceFreightAmounts.Take(100).ToList();
            var test331 = customContext.SupplierInvoiceItems.Take(100).ToList();
            var test332 = customContext.SupplierInvoiceItemModVehicles.Take(100).ToList();
            var test333 = customContext.SupplierInvoiceItemProcesTypes.Take(100).ToList();
            var test334 = customContext.SupplierInvoiceItemsConDeclars.Take(100).ToList();
            var test335 = customContext.SupplierInvoiceItemsDescripts.Take(100).ToList();
            var test336 = customContext.SupplierInvoiceItemsLevies.Take(100).ToList();
            var test337 = customContext.SupplierInvoiceItemsMods.Take(100).ToList();
            var test338 = customContext.SupplierInvoiceItemsPrices.Take(100).ToList();
            var test339 = customContext.SupplierInvoiceItemsProdIdents.Take(100).ToList();
            var test340 = customContext.SupplierInvoiceItemsSerialNums.Take(100).ToList();
            var test341 = customContext.SupplierInvoiceItemsTaxes.Take(100).ToList();
            var test342 = customContext.SupplierInvoiceItemVehicles.Take(100).ToList();
            var test343 = customContext.SupplierInvoiceItemVehicleAdds.Take(100).ToList();
            var test344 = customContext.SupplierInvoiceItemVehicleMods.Take(100).ToList();
            var test345 = customContext.SupplierInvoiceModifications.Take(100).ToList();
            var test346 = customContext.SupplierInvoicePayments.Take(100).ToList();
            var test347 = customContext.SupplierInvoiceUCRs.Take(100).ToList();
            var test348 = customContext.SupplierPartyTypes.Take(100).ToList();
            var test349 = customContext.Tapags.Take(100).ToList();
            var test350 = customContext.TapagConnectionTables.Take(100).ToList();
            var test351 = customContext.TapagTypes.Take(100).ToList();
            var test352 = customContext.TermsOfSaleTypes.Take(100).ToList();
            var test353 = customContext.TPGFileTypes.Take(100).ToList();
            var test354 = customContext.TradeAgreements.Take(100).ToList();
            var test355 = customContext.TradeAgreementProtocols.Take(100).ToList();
            var test356 = customContext.TradeLevyExamptTypes.Take(100).ToList();
            var test357 = customContext.TransactionNatureTypes.Take(100).ToList();
            var test358 = customContext.TransferCargoMethodTypes.Take(100).ToList();
            var test359 = customContext.TransportMeansTypes.Take(100).ToList();
            var test360 = customContext.TreatmentWays.Take(100).ToList();
            var test361 = customContext.UIMessages.Take(100).ToList();
            var test362 = customContext.UIMessageAdditionals.Take(100).ToList();
            var test363 = customContext.UnloadingSiteType.Take(100).ToList();
            var test364 = customContext.UpdateCodes.Take(100).ToList();
            var test365 = customContext.ValidCustomsItems.Take(100).ToList();
            var test366 = customContext.Vehicles.Take(100).ToList();
            var test367 = customContext.VehicleManufacturers.Take(100).ToList();
            var test368 = customContext.VehicleOwners.Take(100).ToList();
            var test369 = customContext.VehiclePoolTypes.Take(100).ToList();
            var test370 = customContext.VehiclePriceListType.Take(100).ToList();
            var test371 = customContext.VehicleReductionTypes.Take(100).ToList();
            var test372 = customContext.VehicleSafeAccessoryInstlTypes.Take(100).ToList();
            var test373 = customContext.VehicleSafetyAccessories.Take(100).ToList();
            var test374 = customContext.VehicleSafetyAccessoryTypes.Take(100).ToList();
            var test375 = customContext.VehicleStatuses.Take(100).ToList();
            var test376 = customContext.VehicleTecnologyTypes.Take(100).ToList();
            var test377 = customContext.VehicleTypes.Take(100).ToList();
            var test378 = customContext.VendorCommissions.Take(100).ToList();
            var test379 = customContext.VendorCommunications.Take(100).ToList();
            var test380 = customContext.VendorStatuses.Take(100).ToList();
            var test381 = customContext.VendorTransactionTypes.Take(100).ToList();
            var test382 = customContext.VendorTypes.Take(100).ToList();



        }

        public static void DcaPerEnv()
        {
            DcaFilterByEnvironmentService.UnitTest();
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
            customContext.AcceptanceStatuses.Take(100).ToList();
            customContext.AccumalationStates.Take(100).ToList();
            customContext.ActionCodes.Take(100).ToList();
            customContext.AddressContactStates.Take(100).ToList();
            customContext.AddressPurposes.Take(100).ToList();
            customContext.AgentTalkBackTypes.Take(100).ToList();
            customContext.AmedmentTypes.Take(100).ToList();
            customContext.AmendCancellRequestInitiators.Take(100).ToList();
            customContext.AmendmentFieldReasonTypes.Take(100).ToList();
            customContext.AmendmentFieldStatusTypes.Take(100).ToList();
            customContext.AmendmentRequestStatuses.Take(100).ToList();
            customContext.AmendmentStatuses.Take(100).ToList();
            customContext.AmendmentTypes.Take(100).ToList();
            customContext.AmendRequestRejectReasonTypes.Take(100).ToList();
            customContext.AmountTypes.Take(100).ToList();
            customContext.ApprovedProfessions.Take(100).ToList();
            customContext.AssigneeNotificationTypes.Take(100).ToList();
            customContext.AttachmentTypes.Take(100).ToList();
            customContext.Authorities.Take(100).ToList();
            customContext.AuthorizedSignerPermits.Take(100).ToList();
            customContext.AutonomyRegionTypes.Take(100).ToList();
            customContext.AutonomyTypes.Take(100).ToList();
            customContext.Banks.Take(100).ToList();
            customContext.BuyerRoleTypes.Take(100).ToList();
            customContext.CancellationReasonRequestTypes.Take(100).ToList();
            customContext.CancellationRequestStatuses.Take(100).ToList();
            customContext.CancelRequestRejectReasonTypes.Take(100).ToList();
            customContext.CargoIdentifireTypes.Take(100).ToList();
            customContext.CargoIdentityQualifiers.Take(100).ToList();
            customContext.CargoSeals.Take(100).ToList();
            customContext.CargoSealIdentifiers.Take(100).ToList();
            customContext.CargoSplitRequestStatuses.Take(100).ToList();
            customContext.CargoStatuses.Take(100).ToList();
            customContext.CargoTypes.Take(100).ToList();
            customContext.CertificateExemptionTypes.Take(100).ToList();
            customContext.CertificatesStatuses.Take(100).ToList();
            customContext.CheckEntityTypes.Take(100).ToList();
            customContext.CheckEssenceLookups.Take(100).ToList();
            customContext.CheckQueueTypes.Take(100).ToList();
            customContext.CheckRepresentativeTypes.Take(100).ToList();
            customContext.CheckTypeLookups.Take(100).ToList();
            customContext.Cities.Take(100).ToList();
            customContext.Claims.Take(100).ToList();
            customContext.ClaimEntities.Take(100).ToList();
            customContext.ClaimExplanationCodes.Take(100).ToList();
            customContext.ClaimImporterDeclarsP3Lois.Take(100).ToList();
            customContext.ClaimImporterDeclarsPage3s.Take(100).ToList();
            customContext.ClaimImporterDeclarsPage3As.Take(100).ToList();
            customContext.ClaimImporterDeclarsPage3Bs.Take(100).ToList();
            customContext.ClaimReasonTypes.Take(100).ToList();
            customContext.ClaimsRelatedEntitiesAmounts.Take(100).ToList();
            customContext.ClaimsRelatedEntitiesReasons.Take(100).ToList();
            customContext.ClaimsRelatedEntitiesRefunds.Take(100).ToList();
            customContext.ClaimsRelatedEntitiesSeizures.Take(100).ToList();
            customContext.ClaimsRelatedEntities.Take(100).ToList();
            customContext.ClaimsRelatedEntsExpDeclars.Take(100).ToList();
            customContext.ClaimsRelatedEntsReasonsExps.Take(100).ToList();
            customContext.ClassificationTypes.Take(100).ToList();
            customContext.Clients.Take(100).ToList();
            customContext.ClientAddresses.Take(100).ToList();
            customContext.ClientDrivingLicenses.Take(100).ToList();
            customContext.ClientDrivingLicenseTypes.Take(100).ToList();
            customContext.ClientsAddressCommTypes.Take(100).ToList();
            customContext.ClientsPoas.Take(100).ToList();
            customContext.ClosedTableStatus.Take(100).ToList();
            customContext.CollateralAnswerStatus.Take(100).ToList();
            customContext.CollateralAnswerTypes.Take(100).ToList();
            customContext.CollateralRequestStatus.Take(100).ToList();
            customContext.CollateralsRequestFileConds.Take(100).ToList();
            customContext.CollateralTypes.Take(100).ToList();
            customContext.CommercialSales.Take(100).ToList();
            customContext.CommunicationTypes.Take(100).ToList();
            customContext.ConfirmationTypes.Take(100).ToList();
            customContext.Consignments.Take(100).ToList();
            customContext.ConsignmentInternalTransitions.Take(100).ToList();
            customContext.ConsignmentPackages.Take(100).ToList();
            customContext.ConsignmentPackDangers.Take(100).ToList();
            customContext.ConstraintApprovalDecisions.Take(100).ToList();
            customContext.ConstraintProcessTypes.Take(100).ToList();
            customContext.ConstraintStatuses.Take(100).ToList();
            customContext.ConstraintTypes.Take(100).ToList();
            customContext.ContactRoleTypes.Take(100).ToList();
            customContext.Containerizations.Take(100).ToList();
            customContext.ContainerizationHataraStatuses.Take(100).ToList();
            customContext.ContainerizationStatusCodes.Take(100).ToList();
            customContext.ContainerTypes.Take(100).ToList();
            customContext.ContinuousMessagesTypeCodes.Take(100).ToList();
            customContext.ContinuousRequestTypes.Take(100).ToList();
            customContext.ConverterTypes.Take(100).ToList();
            customContext.CoolingReportingMethods.Take(100).ToList();
            customContext.CountryGroups.Take(100).ToList();
            customContext.CourierCustomStatuses.Take(100).ToList();
            customContext.CourierDeclarations.Take(100).ToList();
            customContext.CourierDeclarationStatuses.Take(100).ToList();
            customContext.CourierManifestStatuses.Take(100).ToList();
            customContext.CourierMasters.Take(100).ToList();
            customContext.CourierPaymentStatuses.Take(100).ToList();
            customContext.CourierPendingReasons.Take(100).ToList();
            customContext.CourierStatuses.Take(100).ToList();
            customContext.CouriersVats.Take(100).ToList();
            customContext.CourtInstances.Take(100).ToList();
            customContext.CurrencyTypes.Take(100).ToList();
            customContext.CurrencyTypeTenants.Take(100).ToList();
            customContext.CustomBanks.Take(100).ToList();
            customContext.CustomBanksCards.Take(100).ToList();
            customContext.CustomDocumentTypes.Take(100).ToList();
            customContext.CustomDocumentTypeMetaData.Take(100).ToList();
            customContext.CustomerActivityTypes.Take(100).ToList();
            customContext.CustomerClassificationTypes.Take(100).ToList();
            customContext.CustomerIdentificationTypes.Take(100).ToList();
            customContext.CustomerIdentifyTypes.Take(100).ToList();
            customContext.CustomerIndicationTypes.Take(100).ToList();
            customContext.CustomerRoleTypes.Take(100).ToList();
            customContext.CustomerTypeGenerals.Take(100).ToList();
            customContext.CustomMetaDataTypes.Take(100).ToList();
            customContext.CustomsAddressTypes.Take(100).ToList();
            customContext.CustomsAirlines.Take(100).ToList();
            customContext.CustomsAutonomyKeywords.Take(100).ToList();
            customContext.CustomsBooks.Take(100).ToList();
            customContext.CustomsBookTypes.Take(100).ToList();
            customContext.CustomsBranches.Take(100).ToList();
            customContext.CustomsClosedTables.Take(100).ToList();
            customContext.CustomsCollaterals.Take(100).ToList();
            customContext.CustomsCollateralsAnswers.Take(100).ToList();
            customContext.CustomsCollateralsConditions.Take(100).ToList();
            customContext.CustomsCountries.Take(100).ToList();
            customContext.CustomsDocuments.Take(100).ToList();
            customContext.CustomsDocumentMetaDataValues.Take(100).ToList();
            customContext.CustomsDocumentPointers.Take(100).ToList();
            customContext.CustomsDocumentsDefinitions.Take(100).ToList();
            customContext.CustomsDocumentStatusTypes.Take(100).ToList();
            customContext.CustomsDocumentsTickets.Take(100).ToList();
            customContext.CustomsDocumentUploads.Take(100).ToList();
            customContext.CustomsEnvironmentSettings.Take(100).ToList();
            customContext.CustomsEnvoirmentTypes.Take(100).ToList();
            customContext.CustomsExchangeRates.Take(100).ToList();
            customContext.CustomsGenerals.Take(100).ToList();
            customContext.CustomsHouseTypes.Take(100).ToList();
            customContext.CustomsHouseTypeAdditionals.Take(100).ToList();
            customContext.CustomsInsuranceCompanies.Take(100).ToList();
            customContext.CustomsItems.Take(100).ToList();
            customContext.CustomsItemDetailsHistorys.Take(100).ToList();
            customContext.CustomsPartnerFtps.Take(100).ToList();
            customContext.CustomsPartnersItems.Take(100).ToList();
            customContext.CustomsPaymentTerms.Take(100).ToList();
            customContext.CustomsRequestsSheets.Take(100).ToList();
            customContext.CustomsRequestsSheetStatuses.Take(100).ToList();
            customContext.CustomsRequiredFields.Take(100).ToList();
            customContext.CustomsSettings.Take(100).ToList();
            customContext.CustomsShips.Take(100).ToList();
            customContext.CustomsTransportModes.Take(100).ToList();
            customContext.CustomsVendors.Take(100).ToList();
            customContext.CustomsVerificationStatusTypes.Take(100).ToList();
            customContext.DangerousGoodsPackingReqs.Take(100).ToList();
            customContext.DBMigrations.Take(100).ToList();
            customContext.DBMigrationLines.Take(100).ToList();
            customContext.DebtNotificationTypes.Take(100).ToList();
            customContext.DecCargoSplitCargoIdentifiers.Take(100).ToList();
            customContext.DecCargoSplitCons.Take(100).ToList();
            customContext.DecCargoSplitConsItems.Take(100).ToList();
            customContext.DecCargoSplitConsPackDets.Take(100).ToList();
            customContext.DecDangersContacts.Take(100).ToList();
            customContext.DecisionTypes.Take(100).ToList();
            customContext.Declarations.Take(100).ToList();
            customContext.DeclarationCargoSplits.Take(100).ToList();
            customContext.DeclarationCasualDetailses.Take(100).ToList();
            customContext.DeclarationConsAcceptances.Take(100).ToList();
            customContext.DeclarationConstraints.Take(100).ToList();
            customContext.DeclarationCourierStatuses.Take(100).ToList();
            customContext.DeclarationErrorMappings.Take(100).ToList();
            customContext.DeclarationExportRecipients.Take(100).ToList();
            customContext.DeclarationMamanSpecialActions.Take(100).ToList();
            customContext.DeclarationPayments.Take(100).ToList();
            customContext.DeclarationPaymentMethods.Take(100).ToList();
            customContext.DeclarationPaymentProtests.Take(100).ToList();
            customContext.DeclarationPendings.Take(100).ToList();
            customContext.DeclarationReferantDatas.Take(100).ToList();
            customContext.DeclarationStatementTypes.Take(100).ToList();
            customContext.DeclarationStatuses.Take(100).ToList();
            customContext.DeclarationStatusTypes.Take(100).ToList();
            customContext.DeclarationTaxes.Take(100).ToList();
            customContext.Deficits.Take(100).ToList();
            customContext.DeficitConnFileParagraphTypes.Take(100).ToList();
            customContext.DeficitDecisions.Take(100).ToList();
            customContext.DeliverySiteTypes.Take(100).ToList();
            customContext.DeliveryTypes.Take(100).ToList();
            customContext.DemanderTypes.Take(100).ToList();
            customContext.Deposits.Take(100).ToList();
            customContext.DepositConditions.Take(100).ToList();
            customContext.DepositCustomerActivities.Take(100).ToList();
            customContext.DepositEssenceTypes.Take(100).ToList();
            customContext.DepositFileTypes.Take(100).ToList();
            customContext.DocumentRejectTypes.Take(100).ToList();
            customContext.DocumentTypeCustomsData.Take(100).ToList();
            customContext.EntitlementTypes.Take(100).ToList();
            customContext.EntityTypeLookups.Take(100).ToList();
            customContext.ExceptionReasons.Take(100).ToList();
            customContext.ExportDeclarationClosingDatas.Take(100).ToList();
            customContext.ExportDeliveryDocumentMessages.Take(100).ToList();
            customContext.ExporterRoleTypes.Take(100).ToList();
            customContext.ExportLogisticPermitActions.Take(100).ToList();
            customContext.ExportReferences.Take(100).ToList();
            customContext.ExportStorages.Take(100).ToList();
            customContext.ExternalFieldMappings.Take(100).ToList();
            customContext.FacilitationTypes.Take(100).ToList();
            customContext.FaultInspectionTypes.Take(100).ToList();
            customContext.FclLclCodes.Take(100).ToList();
            customContext.FreightPaymentMethods.Take(100).ToList();
            customContext.FuelTypes.Take(100).ToList();
            customContext.FullnessCodes.Take(100).ToList();
            customContext.GatepassRequests.Take(100).ToList();
            customContext.GatepassReturnCodes.Take(100).ToList();
            customContext.Genders.Take(100).ToList();
            customContext.GovernmentProcedureTypes.Take(100).ToList();
            customContext.Guarantees.Take(100).ToList();
            customContext.GuaranteeCertificateTypes.Take(100).ToList();
            customContext.GuaranteeConditions.Take(100).ToList();
            customContext.GuaranteeCustomerActivities.Take(100).ToList();
            customContext.HandingCodes.Take(100).ToList();
            customContext.HazardousSubstances.Take(100).ToList();
            customContext.ImporterDeclarationTypes.Take(100).ToList();
            customContext.ImporterDespositions.Take(100).ToList();
            customContext.ImporterPeriodicDeclarStatuses.Take(100).ToList();
            customContext.ImporterTypeForClaims.Take(100).ToList();
            customContext.IncotemrsFileValidations.Take(100).ToList();
            customContext.InterfaceManagements.Take(100).ToList();
            customContext.InterfaceSendOptions.Take(100).ToList();
            customContext.InterfaceTenantDefinitions.Take(100).ToList();
            customContext.InternalBorderSiteTypes.Take(100).ToList();
            customContext.InternationalSites.Take(100).ToList();
            customContext.InvoiceTypes.Take(100).ToList();
            customContext.ItemGovernmentProcedureTypes.Take(100).ToList();
            customContext.LastReleaseFromWarehouses.Take(100).ToList();
            customContext.LeadDocumentExceptionTypes.Take(100).ToList();
            customContext.LeadDocumentTypes.Take(100).ToList();
            customContext.LoadingSiteTypes.Take(100).ToList();
            customContext.LogisticActionRequests.Take(100).ToList();
            customContext.LogisticActionRequestTypes.Take(100).ToList();
            customContext.LogisticActionResponseReqSes.Take(100).ToList();
            customContext.LogisticPermits.Take(100).ToList();
            customContext.LogisticsReferenceTypes.Take(100).ToList();
            customContext.MamanSpecialActions.Take(100).ToList();
            customContext.MamanSpecialActionStatuses.Take(100).ToList();
            customContext.MamanStatuses.Take(100).ToList();
            customContext.ManifestCargoStatuses.Take(100).ToList();
            customContext.MAWBTypes.Take(100).ToList();
            customContext.MeasureQualifier.Take(100).ToList();
            customContext.MeasurmentUnits.Take(100).ToList();
            customContext.ModificationAndDiscountTypes.Take(100).ToList();
            customContext.MorningMessageTypes.Take(100).ToList();
            customContext.NbcDeclarationTypes.Take(100).ToList();
            customContext.NDMessageActionCodes.Take(100).ToList();
            customContext.Notifications.Take(100).ToList();
            customContext.NotificationDefinitions.Take(100).ToList();
            customContext.NotificationReplies.Take(100).ToList();
            customContext.NotificationTenantDefinition.Take(100).ToList();
            customContext.NotificationTypes.Take(100).ToList();
            customContext.OrganizationUnitTypes.Take(100).ToList();
            customContext.PackageMeasureQualifiers.Take(100).ToList();
            customContext.PackingTypes.Take(100).ToList();
            customContext.ParagraphTypes.Take(100).ToList();
            customContext.PartyRelationshipTypes.Take(100).ToList();
            customContext.PassportTypes.Take(100).ToList();
            customContext.PayerActivityTypes.Take(100).ToList();
            customContext.PayerTypes.Take(100).ToList();
            customContext.PaymentMethodStatus.Take(100).ToList();
            customContext.PaymentMethodTypes.Take(100).ToList();
            customContext.PaymentOrders.Take(100).ToList();
            customContext.PaymentOrderConnectionTables.Take(100).ToList();
            customContext.PaymentOrderLines.Take(100).ToList();
            customContext.PaymentOrderMethods.Take(100).ToList();
            customContext.PaymentOrderProtestReasons.Take(100).ToList();
            customContext.PaymentOrderStatus.Take(100).ToList();
            customContext.PaymentOrderTypes.Take(100).ToList();
            customContext.PaymentProcesses.Take(100).ToList();
            customContext.PaymentProtestTypes.Take(100).ToList();
            customContext.PaymentTypes.Take(100).ToList();
            customContext.PendingByKeywords.Take(100).ToList();
            customContext.PendingErrorPlaces.Take(100).ToList();
            customContext.PhysicalChecks.Take(100).ToList();
            customContext.PhysicalCheckOperations.Take(100).ToList();
            customContext.PhysicalCheckSearchResultTypes.Take(100).ToList();
            customContext.PhysicalCheckStatusMessages.Take(100).ToList();
            customContext.PoaAuthorizationTypeLookups.Take(100).ToList();
            customContext.PoaStatusTypeLookUps.Take(100).ToList();
            customContext.PointerLevels.Take(100).ToList();
            //customContext.ProceduralFaults.Take(100).ToList();
            customContext.ProceduralFaultInProcessTypes.Take(100).ToList();
            customContext.ProceduralFaultInSourceTypes.Take(100).ToList();
            customContext.ProceduralFaultsConnEntities.Take(100).ToList();
            customContext.ProceduralFaultStatuses.Take(100).ToList();
            customContext.ProceduralFaultTypes.Take(100).ToList();
            customContext.ProcessingReasons.Take(100).ToList();
            customContext.ProductIdentificationTypes.Take(100).ToList();
            customContext.ProductNameTypes.Take(100).ToList();
            customContext.PropertiesDetailsHistorys.Take(100).ToList();
            customContext.RansomViolationTypes.Take(100).ToList();
            customContext.ReferantExceptions.Take(100).ToList();
            customContext.ReferantTeams.Take(100).ToList();
            customContext.ReferenceInputTypes.Take(100).ToList();
            customContext.ReferenceStatuses.Take(100).ToList();
            customContext.RefundCustomerActivityTypes.Take(100).ToList();
            customContext.RegisteredWarehouseSiteTypes.Take(100).ToList();
            customContext.ReleaseMessageTypes.Take(100).ToList();
            customContext.RequestStatuses.Take(100).ToList();
            customContext.RequestTypes.Take(100).ToList();
            customContext.RequiredGuaranteeTypes.Take(100).ToList();
            customContext.ReturnConditions.Take(100).ToList();
            customContext.SalesTaxExemptionTypes.Take(100).ToList();
            customContext.SealCompleteness.Take(100).ToList();
            customContext.SealTypes.Take(100).ToList();
            customContext.SealUpdateReasonTypes.Take(100).ToList();
            customContext.SecurityClearenceTypeCodes.Take(100).ToList();
            customContext.SeizureFactorTypes.Take(100).ToList();
            customContext.SeizureMethodTypes.Take(100).ToList();
            customContext.SignatureTypes.Take(100).ToList();
            customContext.SiteLookups.Take(100).ToList();
            customContext.SiteTypes.Take(100).ToList();
            customContext.SpecialActionDescriptionTypes.Take(100).ToList();
            customContext.SpecializationTypes.Take(100).ToList();
            customContext.SplitOrMergeReasons.Take(100).ToList();
            customContext.StatusFieldTypes.Take(100).ToList();
            customContext.StorageMessageTypes.Take(100).ToList();
            customContext.StorageStatuses.Take(100).ToList();
            customContext.StorageStatusTables.Take(100).ToList();
            customContext.StuffingSiteTypes.Take(100).ToList();
            customContext.SubCountries.Take(100).ToList();
            customContext.SuppInvoiceItemsAbachStatement.Take(100).ToList();
            customContext.SupplierInvioceItemCertificats.Take(100).ToList();
            customContext.SupplierInvoices.Take(100).ToList();
            customContext.SupplierInvoiceFreightAmounts.Take(100).ToList();
            customContext.SupplierInvoiceItems.Take(100).ToList();
            customContext.SupplierInvoiceItemModVehicles.Take(100).ToList();
            customContext.SupplierInvoiceItemProcesTypes.Take(100).ToList();
            customContext.SupplierInvoiceItemsConDeclars.Take(100).ToList();
            customContext.SupplierInvoiceItemsDescripts.Take(100).ToList();
            customContext.SupplierInvoiceItemsLevies.Take(100).ToList();
            customContext.SupplierInvoiceItemsMods.Take(100).ToList();
            customContext.SupplierInvoiceItemsPrices.Take(100).ToList();
            customContext.SupplierInvoiceItemsProdIdents.Take(100).ToList();
            customContext.SupplierInvoiceItemsSerialNums.Take(100).ToList();
            customContext.SupplierInvoiceItemsTaxes.Take(100).ToList();
            customContext.SupplierInvoiceItemVehicles.Take(100).ToList();
            customContext.SupplierInvoiceItemVehicleAdds.Take(100).ToList();
            customContext.SupplierInvoiceItemVehicleMods.Take(100).ToList();
            customContext.SupplierInvoiceModifications.Take(100).ToList();
            customContext.SupplierInvoicePayments.Take(100).ToList();
            customContext.SupplierInvoiceUCRs.Take(100).ToList();
            customContext.SupplierPartyTypes.Take(100).ToList();
            customContext.Tapags.Take(100).ToList();
            customContext.TapagConnectionTables.Take(100).ToList();
            customContext.TapagTypes.Take(100).ToList();
            customContext.TermsOfSaleTypes.Take(100).ToList();
            customContext.TPGFileTypes.Take(100).ToList();
            customContext.TradeAgreements.Take(100).ToList();
            customContext.TradeAgreementProtocols.Take(100).ToList();
            customContext.TradeLevyExamptTypes.Take(100).ToList();
            customContext.TransactionNatureTypes.Take(100).ToList();
            customContext.TransferCargoMethodTypes.Take(100).ToList();
            customContext.TransportMeansTypes.Take(100).ToList();
            customContext.TreatmentWays.Take(100).ToList();
            customContext.UIMessages.Take(100).ToList();
            customContext.UIMessageAdditionals.Take(100).ToList();
            customContext.UnloadingSiteType.Take(100).ToList();
            customContext.UpdateCodes.Take(100).ToList();
            customContext.ValidCustomsItems.Take(100).ToList();
            customContext.Vehicles.Take(100).ToList();
            customContext.VehicleManufacturers.Take(100).ToList();
            customContext.VehicleOwners.Take(100).ToList();
            customContext.VehiclePoolTypes.Take(100).ToList();
            customContext.VehiclePriceListType.Take(100).ToList();
            customContext.VehicleReductionTypes.Take(100).ToList();
            customContext.VehicleSafeAccessoryInstlTypes.Take(100).ToList();
            customContext.VehicleSafetyAccessories.Take(100).ToList();
            customContext.VehicleSafetyAccessoryTypes.Take(100).ToList();
            customContext.VehicleStatuses.Take(100).ToList();
            customContext.VehicleTecnologyTypes.Take(100).ToList();
            customContext.VehicleTypes.Take(100).ToList();
            customContext.VendorCommissions.Take(100).ToList();
            customContext.VendorCommunications.Take(100).ToList();
            customContext.VendorStatuses.Take(100).ToList();
            customContext.VendorTransactionTypes.Take(100).ToList();
            customContext.VendorTypes.Take(100).ToList();
        }
        public static void HSMSignTests()
        {
            var signQueueHybridDbService = new SignQueueHybridDbService();
            var res1=signQueueHybridDbService.GetAvailableSignServer(6, Logitude.Server.Tools.ExternalServices.SignQueueByType.SignQueueByPersonId, "031561053");

            var hSMActiveSignCardService = new HSMActiveSignCardService();
            var res=hSMActiveSignCardService.GetActiveCertificates(
                6,
                @"https://customs.amital.co.il/api/SignHSMGetActiveCertificates",
                 @"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA==",
                  new HSMActiveSignCardParams()
                  {
                      companyid = "101",
                      token = "c6f85591-6e4e-4203-95ef-628b826577b8",
                      signprocess = "MehesExport",
                      companyBN = "550221105"
                  }
                );
            var hSMSignFileService = new HSMSignFileService();
            string xml = "<r></r>";
            hSMSignFileService.SignFile(
                 @"https://customs.amital.co.il/api/SignHSM",
                 @"9edYig7zg_b2mBV-72DaOKVMlqtJp-xovFY0k5uBNSRtAzFuY2xcGA==",
                 new HSMSignFileService.HSMSignFileParams
                 {
                     companyid = "101",
                     token = "c6f85591-6e4e-4203-95ef-628b826577b8",
                     id = "308623615",
                     signprocess = "MehesExport",
                     companypersonal = "PC",
                     filename = "AAI-222552-1.xml",
                     companyBN = "550221105"
                 },
                 UTF8Encoding.UTF8.GetBytes(xml)
                );
        }
        public static void Check_UserWcfService(int tenant)
        {
            var xmlstring = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><UserPM xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><ExtensionData /><ActiveModified>0</ActiveModified><Anniversary xsi:nil=\"true\" /><Birthday xsi:nil=\"true\" /><BranchId>TLV</BranchId><BranchName>Tel Aviv</BranchName><BusinessPhone>03333332</BusinessPhone><BusinessUnitId>1</BusinessUnitId><Code>BELLA</Code><ComputedLocalName>בלה</ComputedLocalName><Contact><Anniversary xsi:nil=\"true\" /><AnniversaryReminder>false</AnniversaryReminder><BirthDayOfYear xsi:nil=\"true\" /><Birthday xsi:nil=\"true\" /><BirthdayReminder>false</BirthdayReminder><DisconectFromCard>false</DisconectFromCard><DisplayGettingStarted>false</DisplayGettingStarted><DoneDate xsi:nil=\"true\" /><DontShowLocal>false</DontShowLocal><Email>TESTTT@AMITAL.CO.IL</Email><EnglishName>BELLA</EnglishName><FieldsChanged>false</FieldsChanged><HasCardContact>false</HasCardContact><HasPassword>false</HasPassword><Id>BELLA</Id><InActive>false</InActive><IndexColor xsi:nil=\"true\" /><IsAirExport>false</IsAirExport><IsAirImport>false</IsAirImport><IsAll>false</IsAll><IsCreatedWithPartner>false</IsCreatedWithPartner><IsCustomsImport>false</IsCustomsImport><IsHybrid>true</IsHybrid><IsInlandDomestic>false</IsInlandDomestic><IsInlandExport>false</IsInlandExport><IsInlandImport>false</IsInlandImport><IsLocked>false</IsLocked><IsOceanExport>false</IsOceanExport><IsOceanImport>false</IsOceanImport><IsSendNotificationForMobile>false</IsSendNotificationForMobile><IsUser>false</IsUser><LocalName>בלה</LocalName><MustChangePassword>false</MustChangePassword><NumberOfRetries>0</NumberOfRetries><SetAsPrimaryForCard>false</SetAsPrimaryForCard><SharedMobileAppAlertonExceptions>false</SharedMobileAppAlertonExceptions><SharedMobileAppAlertsforFollowedShipment>false</SharedMobileAppAlertsforFollowedShipment><SignupRole>false</SignupRole><Tenant>1</Tenant></Contact><CreateDate>2021-06-30T12:13:24.4114336</CreateDate><DepartmentId>---</DepartmentId><DepartmentName>---</DepartmentName><Email>TESTTT@AMITAL.CO.IL</Email><EnglishName>BELLA</EnglishName><EntityChanged>false</EntityChanged><ExpirationDate xsi:nil=\"true\" /><ExpirationDaysLeft>0</ExpirationDaysLeft><Id>1-6859</Id><InActive>false</InActive><InternetAccess>false</InternetAccess><IsBranchRestricted>false</IsBranchRestricted><IsCustomerCare>false</IsCustomerCare><IsDistributor>false</IsDistributor><IsFreelancer>true</IsFreelancer><IsHybrid>true</IsHybrid><IsProductRestricted>false</IsProductRestricted><IsSalesman>false</IsSalesman><IsShowContactDetailsInTheMobileApp>false</IsShowContactDetailsInTheMobileApp><LicencedUser>false</LicencedUser><LocalName>בלה</LocalName><Mobile>0533333322</Mobile><Roles><UserRolesPM><Added>true</Added><Exists>false</Exists><Id>FRL1</Id><Removed>false</Removed><Tenant>1</Tenant></UserRolesPM></Roles><SearchFields>testtt@amital.co.il,BELLA,בלה,BELLA</SearchFields><SignupRole>false</SignupRole><Tenant>1</Tenant><UserLastLogin><ExtensionData /><ComputerId>3d12b27d-f422-404b-94c6-bcbceadbb7f1</ComputerId><Id>1-6859</Id><LoginDateTime>2021-07-04T13:19:30.8321699</LoginDateTime><Tenant>1</Tenant></UserLastLogin><UserPermittedBranches /><UserPermittedProducts /><UserType>R</UserType></UserPM></root>";
            //UserPM entityPM = XmlGenericUtil<UserPM>.DeSerializeObject(xmlstring);
            UserQuery userQuery = new UserQuery(tenant);
            UserPM entityPM = userQuery.GetSinglePMByCode("BELLA",tenant);
            var userWcfService = new UserWcfService();
            userWcfService.Upsert(entityPM,false);
            
        }
    }
}

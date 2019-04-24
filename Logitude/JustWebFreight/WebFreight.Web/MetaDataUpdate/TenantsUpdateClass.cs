using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel;
using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.MetaDataUpdate.UpdateClasses;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.InvoiceModel.DomainServices;
using WebFreight.Web.ShipmentsModel.DomainServices;
using WebFreight.Web.QuoteModel.DomainServices;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.CommonDataModel.DomainServices;
using System.Reflection;
using Simplog.Data.ShipmentsModel;
using ICSharpCode.SharpZipLib.Core;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.ShipmentsModel;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.QuoteModel;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InvoiceModel;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InfrastructureModel;
using WebFreight.Web.MetaDataUpdate.GeneratedUpdate.GlobalModel;

namespace WebFreight.Web.MetaDataUpdate
{
    public class TenantsUpdateClass
    {
        public static void UpdateDataForTenant(int tenant, string message)
        {
            tenant = 1;
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);

            #region Repositories definitions

            TranslationHeaderRepository translationHeadersRepository = new TranslationHeaderRepository(context);
            MeasurementRepository measurementsRepository = new MeasurementRepository(commonContext);
            EntityStatusRepository entityStatusRepository = new EntityStatusRepository(context);
            EventTypeRepository eventTypeRepository = new EventTypeRepository(context);
            RankRepository rankRepository = new RankRepository(commonContext);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);
            DocumentTypeCopyRepository documentTypeCopyRepository = new DocumentTypeCopyRepository(commonContext);
            DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(commonContext);
            DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(commonContext);
            CreditCardTypeRepository creditCardTypeRepositoryRepository = new CreditCardTypeRepository(invoiceContext);
            MoveTypeRepository moveTypeRepository = new MoveTypeRepository(context);
            EmailAlertSettingRepository emailAlertSettingRepository = new EmailAlertSettingRepository(context);
            FullAccountingSettingRepository fullAccSettingRepository = new FullAccountingSettingRepository(accountingContext);
            JournalActionTypeRepository journalActionTypeRepository = new JournalActionTypeRepository(accountingContext);
            ChargesGroupRepository chargesGroupRepository = new ChargesGroupRepository(context);
            TaxWithholdingAssessOfficeRepository taxWithholdingAssessOfficeRepository = new TaxWithholdingAssessOfficeRepository(accountingContext);
            AccountingCompanyTypeRepository accountingCompanyTypeRepository = new AccountingCompanyTypeRepository(accountingContext);
            WithholdingTaxDeductionTypeRepository withholdingTaxDeductionTypeRepository = new WithholdingTaxDeductionTypeRepository(accountingContext);

            #endregion

            #region Dictionaries and lists

            Dictionary<string, TranslationHeader> tenantZeroTranslationHeaders = translationHeadersRepository.GetTranslationHeadersByTenant(0).ToDictionary(d => d.Description, a => a);
            Dictionary<string, TranslationHeader> currentTenantTranslationHeaders = translationHeadersRepository.GetTranslationHeadersByTenant(tenant).ToDictionary(d => d.Description, a => a);
            Dictionary<string, Measurement> tenantZeroMeasurements = measurementsRepository.GetMeasurementsByTenant(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, Measurement> currentTenantMeasurements = measurementsRepository.GetMeasurementsByTenant(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, EntityStatus> tenantZeroEntityStatus = entityStatusRepository.GetEntityStatusByTenant(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, EntityStatus> currentTenantEntityStatus = entityStatusRepository.GetEntityStatusByTenant(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, EventType> tenantZeroEventTypes = null;
            if (true)
            {

                tenantZeroEventTypes = new Dictionary<string, EventType>();
                foreach (var d in eventTypeRepository.GetEventTypesByTenant(0).ToList())
                {
                    tenantZeroEventTypes.Add(d.Code + d.ObjectTableId, d);
                }
            }
            else
            {
                tenantZeroEventTypes = eventTypeRepository.GetEventTypesByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            }
            Dictionary<string, EventType> currentTenantEventTypes = eventTypeRepository.GetEventTypesByTenant(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, Rank> tenantZeroRanks = rankRepository.GetRanks(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, Rank> currentTenantRanks = rankRepository.GetRanks(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, DocumentTypePM> tenantZeroDocumentTypes = documentTypeQuery.GetDocumentTypePMsByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, DocumentType> currentTenantDocumentTypes = documentTypeRepository.GetDocumentTypes(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            List<DocumentTypeCustomField> tenantZeroCustomFields = documentTypeCustomFieldRepository.GetDocumentTypeCustomFields(0).ToList();
            Dictionary<string, CreditCardType> tenantZeroCreditCardTypes = creditCardTypeRepositoryRepository.GetCreditCardTypes(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, CreditCardType> currentTenantCreditCardTypes = creditCardTypeRepositoryRepository.GetCreditCardTypes(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, MoveType> tenantZeroMoveTypes = moveTypeRepository.GetMoveTypesByTenant(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, MoveType> currentTenantMoveTypes = moveTypeRepository.GetMoveTypesByTenant(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, EmailAlertSetting> tenantZeroEmailAlertSettings = emailAlertSettingRepository.GetEmailAlertSettings(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, EmailAlertSetting> currentEmailAlertSettings = emailAlertSettingRepository.GetEmailAlertSettings(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, JournalActionType> tenantZeroJournalActionTypes = journalActionTypeRepository.GetAll(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, JournalActionType> currentJournalActionTypes = journalActionTypeRepository.GetAll(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, TaxWithholdingAssessOffice> tenantZeroTaxWithholdingAssessOffices = taxWithholdingAssessOfficeRepository.GetAll(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, TaxWithholdingAssessOffice> currentTaxWithholdingAssessOffices = taxWithholdingAssessOfficeRepository.GetAll(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, AccountingCompanyType> tenantZeroAccountingCompanyTypes = accountingCompanyTypeRepository.GetAll(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, AccountingCompanyType> currentAccountingCompanyTypes = accountingCompanyTypeRepository.GetAll(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, WithholdingTaxDeductionType> tenantZeroWithholdingTaxDeductionTypes = withholdingTaxDeductionTypeRepository.GetAll(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, WithholdingTaxDeductionType> currentWithholdingTaxDeductionTypes = withholdingTaxDeductionTypeRepository.GetAll(tenant).ToDictionary(d => d.Code, a => a);

            Dictionary<string, ChargesGroup> tenantZeroChargesGroups = chargesGroupRepository.GetChargesGroups(0).ToDictionary(d => d.Code, a => a);
            Dictionary<string, ChargesGroup> currentTenantChargesGroups = chargesGroupRepository.GetChargesGroups(tenant).ToDictionary(d => d.Code, a => a);


            #endregion

            if (tenant == 0)
            {
                #region

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                switch (message.ToLower())
                {
                      
                      case "updatetenantzeronew":
                        {
                            MetaDataUpdateClass updateClass = new MetaDataUpdateClass();
                            updateClass.UpgradeClosedTablesForTenantZero();

							InfrastructureModelUpdateClass inframodelUpdateClass = new InfrastructureModelUpdateClass();
							inframodelUpdateClass.LoadObjectsTenantZero(context);


							ShipmentsModelUpdateClass shipmentModelUpdateClass = new ShipmentsModelUpdateClass();
                            shipmentModelUpdateClass.LoadObjectsTenantZero(context);

                            QuoteModelUpdateClass quotemodelUpdateClass = new QuoteModelUpdateClass();
                            quotemodelUpdateClass.LoadObjectsTenantZero(context);

                            InvoiceModelUpdateClass invoicemodelUpdateClass = new InvoiceModelUpdateClass();
                            invoicemodelUpdateClass.LoadObjectsTenantZero(context);

                            CommonDataModelUpdateClass commonmodelUpdateClass = new CommonDataModelUpdateClass();
                            commonmodelUpdateClass.LoadObjectsTenantZero(context);

                          
                            GlobalModelUpdateClass globalmodelUpdateClass = new GlobalModelUpdateClass();
                            globalmodelUpdateClass.LoadObjectsTenantZero(context);

							InfrastructureUpdateClass modelUpdateClass = new InfrastructureUpdateClass();
							modelUpdateClass.LoadObjectsTenantZero(context);

							updateClass.LoadUpdateTenantZero(context, false);

                            //updateClass.LoadOtherFields(context);
                            updateClass.LoadTranslationHeaders();
                            updateClass.LoadMeasurements();
                            updateClass.LoadCreditCardTypes();
                            updateClass.LoadMoveTypes();
							//updateClass.loadQueries();
							//updateClass.loadScreens();
							//updateClass.LoadObjectTableTabs();
							context.SaveChanges();

							updateClass.LoadRolesAndFeatures(0);
                            updateClass.LoadObjectTableHelperControls();
                            updateClass.LoadEntityStatus();
                            updateClass.LoadEventTypes();
                            updateClass.LoadRanks();
                            updateClass.LoadMenustables();
                            updateClass.LoadDefaultReports();
                            updateClass.LoadHelpResources();
                            updateClass.CreateMasterCounter(0);
                            updateClass.LoadEmailAlertSettings();
                            if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1")
                            {
                                updateClass.UpdateShipmentLogboxAuomationObjectFields(context);
                            }
                         

                            break;
                        }
                    case "updatetenantzero":
                        {
                            MetaDataUpdateClass updateClass = new MetaDataUpdateClass();
                            if (LogitudeSettings.WorkEnvironment == "customs")
                            {
                                updateClass.LoadUpdateTenantZero(context);
                                updateClass.LoadOtherFields(context);
                                updateClass.LoadTranslationHeaders();
                                updateClass.loadQueries();
                                updateClass.loadScreens();
                                updateClass.LoadObjectTableTabs();
                                updateClass.LoadObjectTableHelperControls();
                                updateClass.LoadEntityStatus();
                                updateClass.LoadEventTypes();
                                updateClass.LoadMenustables();
                            }
                            else
                            {
                                //updateClass.UpgradeClosedTablesForTenantZero();

                                ////ShipmentsModelUpdateClass shipmentModelUpdateClass = new ShipmentsModelUpdateClass();
                                ////shipmentModelUpdateClass.LoadObjectsTenantZero(context);

                                ////QuoteModelUpdateClass quotemodelUpdateClass = new QuoteModelUpdateClass();
                                ////quotemodelUpdateClass.LoadObjectsTenantZero(context);

                                ////InvoiceModelUpdateClass invoicemodelUpdateClass = new InvoiceModelUpdateClass();
                                ////invoicemodelUpdateClass.LoadObjectsTenantZero(context);

                                ////CommonDataModelUpdateClass commonmodelUpdateClass = new CommonDataModelUpdateClass();
                                ////commonmodelUpdateClass.LoadObjectsTenantZero(context);

                                ////InfrastructureModelUpdateClass inframodelUpdateClass = new InfrastructureModelUpdateClass();
                                ////inframodelUpdateClass.LoadObjectsTenantZero(context);

                                ////GlobalModelUpdateClass globalmodelUpdateClass = new GlobalModelUpdateClass();
                                ////globalmodelUpdateClass.LoadObjectsTenantZero(context);


                                //updateClass.LoadUpdateTenantZero(context, true);

                                //updateClass.LoadOtherFields(context);
                                //updateClass.LoadTranslationHeaders();
                                //updateClass.LoadMeasurements();
                                //updateClass.LoadCreditCardTypes();
                                //updateClass.LoadMoveTypes();
                                //updateClass.loadQueries();
                                //updateClass.loadScreens();
                                //updateClass.LoadObjectTableTabs();
                                //updateClass.LoadObjectTableHelperControls();
                                //updateClass.LoadEntityStatus();
                                //updateClass.LoadEventTypes();
                                //updateClass.LoadRanks();
                                //updateClass.LoadMenustables();
                                //updateClass.LoadDefaultReports();
                                //updateClass.LoadHelpResources();
                                //updateClass.CreateMasterCounter(0);
                                //updateClass.LoadEmailAlertSettings();
                            }
                            break;
                        }

                    case "customs":
                        {
                            CustomsUpdateClass customUpdate = new CustomsUpdateClass();//generated
                            customUpdate.LoadObjectsTenantZero(context);//generated


                            ForCourier();


                            CustomUpdate updateClass = new CustomUpdate();

                            updateClass.UpgradeClosedTablesForTenantZero();
                            updateClass.LoadUpdateTenantZero(context);
                            updateClass.LoadOtherFields(context);
                            updateClass.loadQueries();
                            updateClass.loadScreens();
                            updateClass.LoadObjectTableTabs();
                            updateClass.LoadObjectTableHelperControls();
                            updateClass.LoadMenustables();
                            updateClass.LoadEventTypes();
                            updateClass.FillTransportModeTable();
                            updateClass.FillTapagTypeTable();


                            updateClass.FillCustomsRequestsSheetStatusTable();
                            updateClass.FillCustomsNotificationDefinitions();

                            updateClass.FillCustomsInterfaceSendOptions();
                            updateClass.FillCustomsInterfaceManagements();


                            updateClass.FillAssigneeNotificationTypeTable();
                            updateClass.FillLastReleaseFromWarehouseTable();
                            updateClass.FillVehicleStatusTable();
                            updateClass.FillVehicleSafetyAccessoryInstallationTypeTable();
                            updateClass.FillCustomerIdentifyType();
                            updateClass.FillCustomsVerificationStatusTypes();
                            updateClass.FillSignatureTypeTable();
                            updateClass.FillCertificateStatus();
                            updateClass.FillAccumalationStateTable();
                            updateClass.FillStorageStatus();
                            updateClass.FillMAWBTypeTable();
                            updateClass.FillCourierCustomStatus();
                            updateClass.FillManifestCargoStatusTable();
                            updateClass.FillAcceptanceStatus();
                            updateClass.FillMamanStatus();
                            updateClass.FillPendingErrorPlaceTable();
                            //updateClass.FillCourierDeclarationStatus();
                            //updateClass.FillCourierManifestStatus();
                            //updateClass.FillCourierPaymentStatus();
                            updateClass.FillMamanSpecialActionTable();
                            updateClass.FillMamanSpecialActionStatusTable();



                            break;
                        }

                    case "crm":
                        {
                            CRMUpdateClass cRMUpdateClass = new CRMUpdateClass();
                            cRMUpdateClass.LoadObjectsTenantZero(context);

                            CRMUpdate updateClass = new CRMUpdate();
                            //updateClass.UpgradeClosedTablesForTenantZero();
                            updateClass.LoadUpdateTenantZero(context);
                            updateClass.LoadOtherFields(context);
                            //updateClass.loadQueries();
                            //updateClass.loadScreens();
                            //updateClass.LoadObjectTableTabs();
                            updateClass.LoadObjectTableHelperControls();
                            updateClass.LoadMenustables();
                            //updateClass.LoadEventTypes();
                            updateClass.LoadOpportunityClosingReasons();
                            updateClass.LoadOpportunityTypes();
                            break;
                        }

                    case "booking":
                        {
                            BookingLibUpdateClass bookingLibUpdateClass = new BookingLibUpdateClass();
                            bookingLibUpdateClass.LoadObjectsTenantZero(context);

                            BookingUpdate updateClass = new BookingUpdate();
                            //updateClass.UpgradeClosedTablesForTenantZero();
                            updateClass.LoadUpdateTenantZero(context);
                            updateClass.LoadOtherFields(context);
                            //updateClass.loadQueries();
                            //updateClass.loadScreens();
                            //updateClass.LoadObjectTableTabs();
                            updateClass.LoadObjectTableHelperControls();
                            updateClass.LoadMenustables();
                            //updateClass.LoadEventTypes();
                            break;
                        }

                    case "social":
                        {
                            SocialUpdateClass socialUpdateClass = new SocialUpdateClass();
                            socialUpdateClass.LoadObjectsTenantZero(context);

                            break;
                        }

                    case "warehouse":
                        {
                            WarehouseLibUpdateClass warehouseLibUpdateClass = new WarehouseLibUpdateClass();
                            warehouseLibUpdateClass.LoadObjectsTenantZero(context);
                            WarehouseUpdate updateClass = new WarehouseUpdate();
                            updateClass.LoadRolesAndFeatures(0);
                            updateClass.CreateTableCounters();
                            updateClass.LoadOtherFields(context);
                            break;
                        }
                    case "timemanagement":
                        {
                            TimeManagementUpdateClass timeManagementUpdateClass = new TimeManagementUpdateClass();
                            timeManagementUpdateClass.LoadObjectsTenantZero(context);

                            TimeManagementUpdate updateClass = new TimeManagementUpdate();
                            //updateClass.loadScreens();
                            break;
                        }


                    case "tariffmodule":
                        {
                            TariffModuleUpdateClass tariffModuleUpdateClass = new TariffModuleUpdateClass();
                            tariffModuleUpdateClass.LoadObjectsTenantZero(context);

                            TariffModuleUpdate updateClass = new TariffModuleUpdate();
                            //updateClass.loadScreens();
                            break;
                        }


                    case "accounting":
                        {
                            AccountingUpdateClass accountingUpdateClass = new AccountingUpdateClass();
                            accountingUpdateClass.LoadObjectsTenantZero(context);

                            AccountingUpdate updateClass = new AccountingUpdate();
                            //updateClass.UpgradeClosedTablesForTenantZero();
                            updateClass.LoadUpdateTenantZero(context);
                            updateClass.LoadOtherFields(context);
                            //updateClass.loadQueries();
                            //updateClass.loadScreens();
                            //updateClass.LoadObjectTableTabs(); 
                            updateClass.LoadObjectTableHelperControls();
                            updateClass.LoadMenustables();
                            //updateClass.LoadEventTypes();
                            updateClass.FillTaxWithholdingAssessOffice();
                            //updateClass.FillAccountingCompanyType();
                            //updateClass.FillTaxWithholdingAssessOffice();
                            updateClass.FillWithholdingTaxDeductionTypes();
                            
                            break;
                        }
                    case "shipment":
                        {
                            ShipmentsModelUpdateClass shipmentModelUpdateClass = new ShipmentsModelUpdateClass();
                            shipmentModelUpdateClass.LoadObjectsTenantZero(context);

                            if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1")
                            {
                                MetaDataUpdateClass updateClass = new MetaDataUpdateClass();
                                updateClass.UpdateShipmentLogboxAuomationObjectFields(context);
                            }

                            break;
                        }
                    case "quote":
                        {
                            QuoteModelUpdateClass modelUpdateClass = new QuoteModelUpdateClass();
                            modelUpdateClass.LoadObjectsTenantZero(context);
                            break;
                        }
                    case "invoice":
                        {
                            InvoiceModelUpdateClass modelUpdateClass = new InvoiceModelUpdateClass();
                            modelUpdateClass.LoadObjectsTenantZero(context);
                            break;
                        }
                    case "common":
                        {
                            CommonDataModelUpdateClass modelUpdateClass = new CommonDataModelUpdateClass();
                            modelUpdateClass.LoadObjectsTenantZero(context);
                            break;
                        }
                    case "infrastructure":
                        {
                            InfrastructureModelUpdateClass modelUpdateClass = new InfrastructureModelUpdateClass();
                            modelUpdateClass.LoadObjectsTenantZero(context);
                            break;
                        }

                    case "infrastructurem":
                        {
                            InfrastructureUpdateClass modelUpdateClass = new InfrastructureUpdateClass();
                            modelUpdateClass.LoadObjectsTenantZero(context);
                            break;
                        }
                    case "global":
                        {
                            GlobalModelUpdateClass modelUpdateClass = new GlobalModelUpdateClass();
                            modelUpdateClass.LoadObjectsTenantZero(context);
                            break;
                        }


                    case "converttemplatefromxmaltohtml":
                        {
                            ConvertTemplateHelper convertTemplateHelper = new ConvertTemplateHelper();
                           convertTemplateHelper.ConvertDocumentTypeTemplateXMLToHtml("All");
           
                            break;
                        }

                    case "convertsometemplatefromxmaltohtml":
                        {
                            ConvertTemplateHelper convertTemplateHelper = new ConvertTemplateHelper();
                            convertTemplateHelper.ConvertDocumentTypeTemplateXMLToHtml("Some");

                            break;
                        }


                    case "convertsignaturefromxmaltohtml":
                        {
                            ConvertTemplateHelper convertTemplateHelper = new ConvertTemplateHelper();
                            convertTemplateHelper.ConvertContactSignatureXMLToHtml();
                           

                            break;
                        }

                    case "copyreportonalltenant":
                        {
                            ReportHelper reportHelper = new ReportHelper();
                            reportHelper.CopyReports();

                            break;
                        }

                    case "encryptiondocument":
                        {
                            DocumentHelper documentHelper = new DocumentHelper();
                            documentHelper.EncryptionDocument();
                            break;
                        }


                    case "updatereportlocalnames":
                        {
                            ReportHelper reportHelper = new ReportHelper();
                            reportHelper.UpdateReportLocalNames();
                            break;
                        }

                    case "all":
                        {
                            //Tenant 0
                            MetaDataUpdateClass updateClass = new MetaDataUpdateClass();

                            updateClass.UpgradeClosedTablesForTenantZero();
                            updateClass.LoadUpdateTenantZero(context);
                            updateClass.LoadOtherFields(context);
                            updateClass.LoadTranslationHeaders();
                            updateClass.LoadMeasurements();
                            updateClass.LoadCreditCardTypes();
                            updateClass.LoadMoveTypes();
                            updateClass.loadQueries();
                            updateClass.loadScreens();
                            updateClass.LoadObjectTableTabs();
                            updateClass.LoadObjectTableHelperControls();
                            updateClass.LoadEntityStatus();
                            updateClass.LoadEventTypes();
                            updateClass.LoadRanks();
                            updateClass.LoadMenustables();
                            updateClass.LoadDefaultReports();
                            updateClass.LoadHelpResources();
                            updateClass.CreateMasterCounter(0);
                            updateClass.LoadEmailAlertSettings();

                            // customs
                            CustomsUpdateClass customUpdate = new CustomsUpdateClass();//generated
                            customUpdate.LoadObjectsTenantZero(context);//generated

                            CustomUpdate customUpdateClass = new CustomUpdate();
                            customUpdateClass.UpgradeClosedTablesForTenantZero();
                            customUpdateClass.LoadUpdateTenantZero(context);
                            customUpdateClass.LoadOtherFields(context);
                            customUpdateClass.loadQueries();
                            customUpdateClass.loadScreens();
                            customUpdateClass.LoadObjectTableTabs();
                            customUpdateClass.LoadObjectTableHelperControls();
                            customUpdateClass.LoadMenustables();
                            customUpdateClass.LoadEventTypes();

                            //CRM
                            CRMUpdateClass cRMUpdateClass = new CRMUpdateClass();
                            cRMUpdateClass.LoadObjectsTenantZero(context);

                            CRMUpdate cRMUpdate = new CRMUpdate();
                            cRMUpdate.UpgradeClosedTablesForTenantZero();
                            cRMUpdate.LoadUpdateTenantZero(context);
                            cRMUpdate.LoadOtherFields(context);
                            cRMUpdate.loadQueries();
                            cRMUpdate.loadScreens();
                            cRMUpdate.LoadObjectTableTabs();
                            cRMUpdate.LoadObjectTableHelperControls();
                            cRMUpdate.LoadMenustables();
                            cRMUpdate.LoadEventTypes();
                            cRMUpdate.LoadOpportunityClosingReasons();
                            cRMUpdate.LoadOpportunityTypes();

                            // social
                            SocialUpdateClass socialUpdateClass = new SocialUpdateClass();
                            socialUpdateClass.LoadObjectsTenantZero(context);


                           //warehouse
                            WarehouseLibUpdateClass warehouseLibUpdateClass = new WarehouseLibUpdateClass();
                            warehouseLibUpdateClass.LoadObjectsTenantZero(context);
                            WarehouseUpdate warehouseUpdate = new WarehouseUpdate();
                            warehouseUpdate.LoadRolesAndFeatures(0);
                            warehouseUpdate.CreateTableCounters();
                            warehouseUpdate.LoadOtherFields(context);


                            //accounting
                            AccountingUpdateClass accountingUpdateClass = new AccountingUpdateClass();
                            accountingUpdateClass.LoadObjectsTenantZero(context);

                            AccountingUpdate accountingUpdate = new AccountingUpdate();
                            accountingUpdate.UpgradeClosedTablesForTenantZero();
                            accountingUpdate.LoadUpdateTenantZero(context);
                            accountingUpdate.LoadOtherFields(context);
                            accountingUpdate.loadQueries();
                            accountingUpdate.loadScreens();
                            accountingUpdate.LoadObjectTableTabs();
                            accountingUpdate.LoadObjectTableHelperControls();
                            accountingUpdate.LoadMenustables();
                            accountingUpdate.LoadEventTypes();
                            accountingUpdate.CreateCounters(tenant);
                            //Booking
                            BookingLibUpdateClass bookingLibUpdateClass = new BookingLibUpdateClass();
                            bookingLibUpdateClass.LoadObjectsTenantZero(context);

                            BookingUpdate bookingUpdateClass = new BookingUpdate();
                            bookingUpdateClass.UpgradeClosedTablesForTenantZero();
                            bookingUpdateClass.LoadUpdateTenantZero(context);
                            bookingUpdateClass.LoadOtherFields(context);
                            bookingUpdateClass.loadQueries();
                            bookingUpdateClass.loadScreens();
                            bookingUpdateClass.LoadObjectTableTabs();
                            bookingUpdateClass.LoadObjectTableHelperControls();
                            bookingUpdateClass.LoadMenustables();
                            bookingUpdateClass.LoadEventTypes();

                            //Time Management

                            TimeManagementUpdateClass timeManagementUpdateClass = new TimeManagementUpdateClass();
                            timeManagementUpdateClass.LoadObjectsTenantZero(context);

                            TimeManagementUpdate timeManagementUpdate = new TimeManagementUpdate();
                            timeManagementUpdate.loadScreens();



                            //Tariff Module

                            TariffModuleUpdateClass tariffModuleUpdateClass = new TariffModuleUpdateClass();
                            tariffModuleUpdateClass.LoadObjectsTenantZero(context);

                            TariffModuleUpdate tariffModuleUpdate = new TariffModuleUpdate();
                            tariffModuleUpdate.loadScreens();

                            // New Infrastructure 
                            InfrastructureUpdateClass modelUpdateClass = new InfrastructureUpdateClass();
                            modelUpdateClass.LoadObjectsTenantZero(context);

                            break;
                        }
                }

                

                //Update version will be done by script : Jalal

                if (LogitudeSettings.DeploymentStage == "Dev" || LogitudeSettings.IsCostomsDeploy)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                    {
                        GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
                        GlobalTenant globaltenant = globalTenantRepository.GetGlobalTenantsByTenant(0);
                        globaltenant.Version = globaltenant.Version + 1;
                        globalTenantRepository.Update(globaltenant);
                        globalTenantRepository.SubmitChanges();
                        scope.Complete();
                    }

                   
                }

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                AzureLog.SaveLogsInStorage("(" + message + ")" + " Update Tenant 0 Elapsed Time : " + ts.ToString(), "P", DateTime.Now, "", "", 0, null, null, null);

                TableLastUpdateClass.UpdateAllClosedTablesHistory();
                TableLastUpdateClass.UpdateSystemMetaDataHistory();

                //TenantsUpdateClass.GenerateBackupData();
                #endregion
            }

            else
            {
                using (TransactionScope scop = TransactionFactory.GetNewTransaction(new TimeSpan(2, 5, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 5, 0)))
                {
                    #region Update methods

                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    //UpdateTranslationHeaders(tenant, tenantZeroTranslationHeaders, currentTenantTranslationHeaders, translationHeadersRepository);
                    UpdateMeasurements(tenant, tenantZeroMeasurements, currentTenantMeasurements, measurementsRepository);

                    //===========================

                    TenantRepository tenantRep = new TenantRepository(commonContext);
                    Tenant currentTenant = tenantRep.GetSingleTenant(tenant);
                    bool isHybridTenant = false;
                    if (currentTenant != null)
                    {
                        isHybridTenant = currentTenant.IsHybrid;
                    }

                    //====================================

                    UpdateEntityStatus(tenant, tenantZeroEntityStatus, currentTenantEntityStatus, entityStatusRepository, isHybridTenant);
                    UpdateEventTypes(tenant, tenantZeroEventTypes, currentTenantEventTypes, eventTypeRepository, tenantZeroEntityStatus, currentTenantEntityStatus, isHybridTenant);
                  
                    //UpdateRanks(tenant, tenantZeroRanks, currentTenantRanks, rankRepository);
                    UpdateDocumentTypes(tenant, documentTypeRepository, documentTypeCopyRepository, tenantZeroDocumentTypes, currentTenantDocumentTypes, documentTypeCustomFieldRepository, tenantZeroCustomFields, documentTypeTemplateRepository);
                    //UpdateCreditCardTypes(tenant, tenantZeroCreditCardTypes, currentTenantCreditCardTypes, creditCardTypeRepositoryRepository);
                    //UpdateMoveTypes(tenant, tenantZeroMoveTypes, currentTenantMoveTypes, moveTypeRepository);
                    UpdateEmailAlertSettings(tenant, tenantZeroEmailAlertSettings, currentEmailAlertSettings, emailAlertSettingRepository);
                    ReportHelper reportHelper = new ReportHelper();
                    reportHelper.UpdateReports(tenant);

                    AutomationHelper automationHelper = new AutomationHelper();
                    automationHelper.CopyAutomationFromTenantZeroToMyTenant(tenant, tenantZeroDocumentTypes.Values.ToList());

                    if (!LogitudeSettings.IsCostomsDeploy)
                    // what do u think ?? ok i suppose
                    // but ihab yesterday said : if we can ..we shold do it ?!?!?
                    // well lets ,ok lets ??? 
                    ///wde/what ask ihab again ?
                    //no we will tell ihab that we did so :)
                    //!!!! goood !!!!!!
                    // lets do it next branch >> mean next next (unknown time :) :) )
                    // deal :)
                    // i will leave this remarks !!!
                    {
                        UpdateFullAccSettings(tenant, fullAccSettingRepository);
                        UpdateJournalActionTypes(tenant, tenantZeroJournalActionTypes, currentJournalActionTypes, journalActionTypeRepository);

                        UpdateTaxWithholdingAssessingOffices(tenant, tenantZeroTaxWithholdingAssessOffices, currentTaxWithholdingAssessOffices, taxWithholdingAssessOfficeRepository);
                        UpdateAccountingCompanyTypes(tenant, tenantZeroAccountingCompanyTypes, currentAccountingCompanyTypes, accountingCompanyTypeRepository);
                        UpdateWithholdingTaxDeductionTypes(tenant, tenantZeroWithholdingTaxDeductionTypes, currentWithholdingTaxDeductionTypes, withholdingTaxDeductionTypeRepository);
                    }
                    
                    AddSystemUserForTenant(tenant);
                    GeneralDomainService generalDomain = new GeneralDomainService();
                    UpdateTenantVersion(tenant, tenantRepository);

                    //UpdateScreenFields(tenant, context);

                    



                    scop.Complete();

                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    AzureLog.SaveLogsInStorage("Update Tenant " + tenant + "Elapsed Time :" + ts.ToString(), "P", DateTime.Now, "", "", tenant, null, null, null);

                    #endregion
                }
            }
        }

        private static void ForCourier()
        {
            GlobalDBRepository globalDbRep = new GlobalDBRepository();
            var db = globalDbRep.GetGlobalDBs().First();
            var commonDataContext = new CommonDataContext(DatabaseInitializer.GetConnection(db.DBConnection));
            MetaDataUpdateClass.CustomsInterfaces(commonDataContext);//courier 
        }

        private static void UpdateChargesGroups(int tenant, ChargesGroupRepository chargesGroupRepository, Dictionary<string, ChargesGroup> tenantZeroChargesGroups, Dictionary<string, ChargesGroup> currentTenantChargesGroups)
        {
            foreach (ChargesGroup chargesGroup in tenantZeroChargesGroups.Values)
            {

                bool codeExists = (from a in currentTenantChargesGroups.Values
                                   where a.Code.Trim().ToUpper() == chargesGroup.Code.Trim().ToUpper()
                                   select a).Any();
                
                    if (!codeExists)
                    {
              
                        ChargesGroup newchargesGroup = new ChargesGroup()
                        {
                            Id = IdCounter.GetNumber("ChargesGroup", tenant).ToString(),
                            Tenant = tenant,
                            Code = chargesGroup.Code,
                            Name = chargesGroup.Name,
                            LocalName = chargesGroup.LocalName,
                            SearchFields = chargesGroup.SearchFields,
                          
                        };

                        chargesGroupRepository.Add(newchargesGroup);

                    }

                
            }

            chargesGroupRepository.SubmitChanges();
        }



        #region DataBackUp

        public static void BuildObjectTablesZipFilesData(bool savetodisk = false,bool includeCustoms=false)
        {

                ObjectFieldQuery objectFieldsQuery = new ObjectFieldQuery(0);
                ObjectTableQuery objectTabelQuery = new ObjectTableQuery(0);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(0);
                TextCodeQuery textCodeQuery = new TextCodeQuery(0);
                List<ObjectTable> ObjectTableList = null;
            if (includeCustoms)
            {
                ObjectTableList = objectTabelRepository.GetObjectsByTenant(0).Where(t => t.Name.Contains("Customs.")).ToList();
            }
            else
            {
                if (LogitudeSettings.WorkEnvironment == "customs")
                {
                    ObjectTableList = objectTabelRepository.GetObjectsByTenant(0).Where(t => !t.Name.Contains("Customs.")).ToList();
                }
                else
                {
                    ObjectTableList = objectTabelRepository.GetObjectsByTenant(0).Where(t => !t.Name.Contains("Customs.")).ToList();
                }

            }
                List<TextCodePM> textCodePMLists = textCodeQuery.GetTextCodePMsByTenant(0).ToList();
                List<ObjectFieldPM> objectFieldLists = objectFieldsQuery.GetObjectFieldPMsByTenant(0, 0).ToList();

                Dictionary<string, byte[]> cachedObjectFieldsJosnByte = new Dictionary<string, byte[]>();
                Dictionary<string, byte[]> cachedTextCodesJosnByte = new Dictionary<string, byte[]>();
                Dictionary<string, byte[]> cachedCloseTableJosnByte = new Dictionary<string, byte[]>();


                foreach (ObjectTable objectTable in ObjectTableList)
                {

                    List<ObjectFieldPM> fieldsList = objectFieldLists.Where(d => d.ObjectTableId == objectTable.Id).ToList();
                    if (fieldsList != null)
                    {
                        var josn = LogitudeXmlSerializer.SerializeObjectToJosnString(fieldsList);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(josn);
                        cachedObjectFieldsJosnByte.Add(objectTable.Name, buffer);
                    }

                    List<TextCodePM> textcodes = new List<TextCodePM>();//textCodePMLists.Where(d => d.ObjectTableId == objectTable.Id).ToList();
                    //Contact.O.TableDescription Contact.F.SearchFields
                    //string descritionTextCode = objectTable.Name + ".O.TableDescription";
                    //string searchFieldsTextCode = objectTable.Name + ".F.SearchFields";
                    if (objectTable.Name == "General")
                        textcodes = textCodePMLists.Where(d => d.ObjectTableId == objectTable.Id || ((d.TextCodeTypeCode == "T" || d.Code.Contains(".O.TableDescription") || d.Code.Contains(".F.SearchFields") || d.Code == d.ObjectTableName + "Description") && d.ObjectTableId != objectTable.Id)).ToList();
                    else
                        textcodes = textCodePMLists.Where(d => d.ObjectTableId == objectTable.Id).ToList();


                    if (textcodes != null)
                    {
                        var josn = LogitudeXmlSerializer.SerializeObjectToJosnString(textcodes);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(josn);
                        cachedTextCodesJosnByte.Add(objectTable.Name, buffer);
                    }

                    if (objectTable.IsClosed && objectTable.CacheOnClient)
                    {
                        var data = TableQueryReflector.GetTableListData(objectTable.Name);//TenantsUpdateClass.GetDataFromCloseTable(objectTable.Name);
                        if (data != null)
                        {
                            var josn = LogitudeXmlSerializer.SerializeObjectToJosnString(data);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(josn);
                            cachedCloseTableJosnByte.Add(objectTable.Name, buffer);
                        }
                        else
                        {
                            //File.AppendAllText(@"C:\TestFolder\not_generated_closed.txt", objectTable.Name + Environment.NewLine);

                        }
                    }

                }


                foreach (ObjectTable objectTable in ObjectTableList)//Where(d => d.IsClosed == false && d.IsComposition == false)// 
                {
                    List<string> tableNames = new List<string>();
                    Dictionary<string, byte[]> dataList = new Dictionary<string, byte[]>();
                    //Object Field
                    byte[] bytejosn = GetByteDateByKey(cachedObjectFieldsJosnByte, objectTable.Name);
                    if (bytejosn != null) dataList.Add(objectTable.Name + "_" + "ObjectFields", CompressionFileData(objectTable.Name + "_" + "Fields", bytejosn));

                    //Text Code
                    bytejosn = GetByteDateByKey(cachedTextCodesJosnByte, objectTable.Name);
                    if (bytejosn != null) dataList.Add(objectTable.Name + "_" + "TextCodes", CompressionFileData(objectTable.Name + "_" + "Codes", bytejosn));

                    tableNames.Add(objectTable.Name);
                    List<ObjectFieldPM> fieldsList = objectFieldLists.Where(d => d.ObjectTableId == objectTable.Id && (d.DataTypeCode == "LookUp" || d.IsMulti)).ToList();

                    foreach (ObjectFieldPM field in fieldsList)
                    {
                        string tablename = field.IsMulti ? field.ObjectTable_MultiTableName : field.ObjectTable_LookUpTableName;
                        if (!tableNames.Contains(tablename))
                        {
                            //Object Field
                            bytejosn = GetByteDateByKey(cachedObjectFieldsJosnByte, tablename);
                            if (bytejosn != null) dataList.Add(tablename + "_" + "ObjectFields", CompressionFileData(tablename + "_" + "Fields", bytejosn));

                            //Text Code
                            bytejosn = GetByteDateByKey(cachedTextCodesJosnByte, tablename);
                            if (bytejosn != null) dataList.Add(tablename + "_" + "TextCodes", CompressionFileData(tablename + "_" + "Codes", bytejosn));

                            //CloseTable
                            if (field.DataTypeCode == "LookUp")
                            {
                                ObjectTable table = ObjectTableList.Where(d => d.Id == field.LookUpTableId).FirstOrDefault();
                                if (table != null && table.IsClosed)
                                {
                                    bytejosn = GetByteDateByKey(cachedCloseTableJosnByte, table.Name);
                                if (bytejosn != null) dataList.Add(table.Name + "_" + "ClosedData", CompressionFileData(table.Name + "_" + "Closed", bytejosn));//+ "_" + "Closed"
                                }
                            }
                            tableNames.Add(tablename);
                        }
                    }


                    if (objectTable.IsClosed)
                    {
                        bytejosn = GetByteDateByKey(cachedCloseTableJosnByte, objectTable.Name);
                        //if (bytejosn == null)
                        //{
                        //    var data = TenantsUpdateClass.GetDataFromCloseTable(objectTable.Name);
                        //    if (data != null)
                        //    {
                        //        var josn = LogitudeXmlSerializer.SerializeObjectToJosnString(data);
                        //        bytejosn = System.Text.Encoding.UTF8.GetBytes(josn);
                        //        cachedCloseTableJosnByte.Add(objectTable.Name, bytejosn);
                        //    }
                        //}


                        if (bytejosn != null) dataList.Add(objectTable.Name + "_" + "ClosedData", CompressionFileData(objectTable.Name + "_" + "Closed", bytejosn));
                    }


                    objectTable.EntityResource = CompressionData(objectTable.Name, dataList,savetodisk);
                    objectTable.EntityResourceLastUpdate = DateTime.UtcNow;
                    objectTabelRepository.Update(objectTable);
                }

                objectTabelRepository.SubmitChanges();
                TableLastUpdateClass.UpdateSystemMetaDataHistory();


        }

        public static byte[] CompressionData(string listKey, Dictionary<string, byte[]> dataBackList,bool saveetodisk = false)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3);
            byte[] bytes = null;
            foreach (string key in dataBackList.Keys)
            {
                var newEntry = new ZipEntry(key + ".zip");
                newEntry.DateTime = DateTime.Now;

                zipStream.PutNextEntry(newEntry);

                bytes = dataBackList[key];

                MemoryStream inStream = new MemoryStream(bytes);
                long inStreamLength = inStream.Length;
                if (inStreamLength < 200)
                {
                    inStreamLength = 200;
                }

                StreamUtils.Copy(inStream, zipStream, new byte[inStreamLength]);
                inStream.Close();
                zipStream.CloseEntry();

            }

            zipStream.IsStreamOwner = false;
            zipStream.Close();
            outputMemStream.Position = 0;
            if (saveetodisk)
            {
                
                string appPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\ZipFiles\"; // <---
                if (Directory.Exists(appPath) == false)                                              // <---
                {                                                                                    // <---
                    Directory.CreateDirectory(appPath);                                              // <---
                }                                                                                    // <---

                appPath += listKey + ".zip";

                System.IO.File.WriteAllBytes(appPath, outputMemStream.ToArray());
            }
            return outputMemStream.ToArray();

        }

        public static byte[] CompressionFileData(string fileName, byte[] fileData)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3);


            var newEntry = new ZipEntry(fileName + ".json");
            newEntry.DateTime = DateTime.Now;

            zipStream.PutNextEntry(newEntry);



            MemoryStream inStream = new MemoryStream(fileData);
            long inStreamLength = inStream.Length;
            if (inStreamLength < 200)
            {
                inStreamLength = 200;
            }

            StreamUtils.Copy(inStream, zipStream, new byte[inStreamLength]);
            inStream.Close();
            zipStream.CloseEntry();



            zipStream.IsStreamOwner = false;
            zipStream.Close();
            outputMemStream.Position = 0;

            //System.IO.File.WriteAllBytes(@"C:\TestFolder\" + listKey + ".zip", outputMemStream.ToArray());
            return outputMemStream.ToArray();

        }

        private static byte[] GetByteDateByKey(Dictionary<string, byte[]> list, string key)
        {
            byte[] bytetable = null; 
            if (list != null)
            {
                bytetable = list.Where(d => d.Key == key).Select(d => d.Value).FirstOrDefault();
            }
            return bytetable;
        }

        public static object GetDataFromCloseTable(string objectTableName)
        {

            CommonDataDomainService commonDataDomainService = null;
            GeneralDomainService generalService = null;
            QuotesDomainService quotesDomainService = null;
            WebFreightDomainService webFreightDomainService = null;
            ShipmentsDomainService shipmentsDomainService = null;
            InvoiceDomainService invoiceDomainService = null;

            object result = null;
            int tenant = 0;
            string methodName = "Get" + objectTableName + "Lists";
            object context = null;
            if (commonDataDomainService == null)
            {
                commonDataDomainService = new CommonDataDomainService(CommonDataContext.GetContext(tenant));
            }

            MethodInfo insideMethodInfo = commonDataDomainService.GetType().GetMethod(methodName);

            if (insideMethodInfo != null)
            {
                context = commonDataDomainService;
            }

            if (generalService == null)
            {
                generalService = new GeneralDomainService(WebFreightContext.GetContext(tenant));
            }

            if (insideMethodInfo == null)
            {
                insideMethodInfo = generalService.GetType().GetMethod(methodName);

                if (insideMethodInfo != null)
                {
                    context = generalService;
                }
            }

            if (webFreightDomainService == null)
            {
                webFreightDomainService = new WebFreightDomainService(WebFreightContext.GetContext(tenant));
            }

            if (insideMethodInfo == null)
            {
                insideMethodInfo = webFreightDomainService.GetType().GetMethod(methodName);

                if (insideMethodInfo != null)
                {
                    context = webFreightDomainService;
                }
            }

            if (quotesDomainService == null)
            {
                quotesDomainService = new QuotesDomainService();
            }

            if (insideMethodInfo == null)
            {
                insideMethodInfo = quotesDomainService.GetType().GetMethod(methodName);

                if (insideMethodInfo != null)
                {
                    context = quotesDomainService;
                }
            }

            if (shipmentsDomainService == null)
            {
                shipmentsDomainService = new ShipmentsDomainService(ShipmentsContext.GetContext(tenant));
            }
            if (insideMethodInfo == null)
            {
                insideMethodInfo = shipmentsDomainService.GetType().GetMethod(methodName);

                if (insideMethodInfo != null)
                {
                    context = shipmentsDomainService;
                }
            }

            if (invoiceDomainService == null)
            {
                invoiceDomainService = new InvoiceDomainService();
            }
            if (insideMethodInfo == null)
            {
                insideMethodInfo = invoiceDomainService.GetType().GetMethod(methodName);

                if (insideMethodInfo != null)
                {
                    context = invoiceDomainService;
                }
            }

            if (insideMethodInfo != null)
            {
                object[] parameters = new object[] { tenant };
                result = insideMethodInfo.Invoke(context, parameters);
            }

            return result;
        }



        public static void DownloadEntityResource()
          {
            
              ObjectTableRepository objectTabelRepository = new ObjectTableRepository(0);

              List<ObjectTable> ObjectTableList = objectTabelRepository.GetObjectsByTenant(0).Where(t => !t.Name.Contains("Customs.")).ToList();

              foreach (ObjectTable table in ObjectTableList)
              {
                  if (table.EntityResource !=null)
                  {
                      System.IO.File.WriteAllBytes(@"C:\TestFolder\" + table.Name + ".zip", table.EntityResource);
                  }

              }

          }
      




        #endregion


        public static void UpdateScreenFields(int tenant, IWebFreightContext context)
        {
            ScreensRepository screensReposiroy = new ScreensRepository(context);
            ScreenFieldsRepository screenFieldsRepository = new ScreenFieldsRepository(context);
            List<ScreenModification> screenModifications = context.ScreenModifications.Where(t => t.Tenant == tenant).ToList();
            List<Screen> tenantZeroScreens = screensReposiroy.GetScreensByTenant(0).ToList();
            List<ScreenField> screenFields = screenFieldsRepository.GetScreenFieldsByTenant(tenant).ToList();
            foreach (Screen screen in tenantZeroScreens)
            {
                ScreenModification screenMod = screenModifications.Where(s => s.ScreenId == screen.Id).FirstOrDefault();
                List<ScreenField> fields = screenFields.Where(f => f.ScreenId == screen.Id).ToList();
                if (fields.Count > 0)
                {
                    int columnsNumber = screenMod != null ? screenMod.NumberOfColumns : screen.NumberOfColumns;
                    int rowsNumber = screenMod != null ? screenMod.NumberOfRows : screen.NumberOfRows;
                    for (int col = 0; col < columnsNumber; col++)
                    {
                        List<ScreenField> colFields = fields.Where(f => f.Column == col).OrderBy(f => f.Row).ToList();
                        int r = 0;
                        foreach (ScreenField field in colFields)
                        {
                            if (field.Row != r)
                            {
                                field.Row = r;
                            }

                            r++;
                        }
                    }

                    int newRowsNumber = fields.Max(f => f.Row) + 1;
                    if (newRowsNumber > rowsNumber)
                    {
                        if (screenMod != null)
                        {
                            screenMod.NumberOfRows = newRowsNumber;
                        }
                        else
                        {
                            screenMod = new ScreenModification()
                            {
                                Id = IdCounter.GetNumber("ScreenModification", tenant),
                                ScreenId = screen.Id,
                                Tenant = tenant,
                                NumberOfColumns = screen.NumberOfColumns,
                                NumberOfRows = newRowsNumber,
                            };

                            context.ScreenModifications.Add(screenMod);
                        }
                    }
                }
            }

            context.SaveChanges();
        }

        public static string AddSystemUserForTenant(int tenant)
        {
            string email = "system@tenant" + tenant + ".com";
            string name = "System";
            string phoneNumber = "9999999";
            ContactRepository contactRepository = new ContactRepository(tenant);
            if (contactRepository.CheckEmailAvailabilityForTenant(email, tenant))
            {
                return "";
            }
            RoleRepository theRoleRepository = new RoleRepository(tenant);
            UserRepository theUserRepository = new UserRepository(tenant);
            BranchRepository branchRepository = new BranchRepository(tenant);
            DepartmentRepository departmentRepository = new DepartmentRepository(tenant);
            ContactTenantRepository contactTenantRepository = new ContactTenantRepository(tenant);
            ContactTenantRoleRepository contactTenantRoleRepository = new ContactTenantRoleRepository(tenant);

            BranchQuery branchQuery = new BranchQuery(branchRepository);
            Branch branch = branchQuery.GetFirstBranchForTenant(tenant);
            DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            Department department = departmentQuery.GetFirstDepartmentForTenant(tenant);
            UserPM user = new UserPM();
            user.Tenant = tenant;
            user.InternetAccess = true;
            user.Email = email;

            user.Password = PasswordGenerator.Generate(8);
            user.EnglishName = name;
            user.Birthday = DateTime.Now;
            user.Fax = "";
            user.Mobile = "";
            user.LocalName = name;
            user.Anniversary = DateTime.Now;
            user.DepartmentId = department != null ? department.Id : null;
            user.BranchId = branch != null ? branch.Id : null;
            user.BusinessPhone = phoneNumber;
            user.SignupRole = true;
            user.SearchFields = email + "," + name + "," + phoneNumber;
            user.UserType = "S";
            user.BusinessUnitId = tenant.ToString();
            InsertUser(user, theUserRepository, contactRepository, theRoleRepository, contactTenantRoleRepository, contactTenantRepository);

            return user.Password;
        }

        public static void MapUserToContact(UserPM user, Contact contact)
        {
            contact.Anniversary = user.Anniversary;
            contact.Birthday = user.Birthday;
            contact.BusinessPhone = user.BusinessPhone;
            contact.Email = user.Email;
            contact.EnglishName = user.EnglishName;
            contact.FacebookId = user.FacebookId;
            contact.Fax = user.Fax;
            contact.InActive = user.InActive;
            contact.LocalName = user.LocalName;
            contact.Mobile = user.Mobile;
            //contact.Password = user.Password;
            contact.Notes = user.Notes;
            contact.Tenant = user.Tenant;
            contact.SearchFields = user.SearchFields;
            contact.DisplayGettingStarted = true;

            contact.DontShowLocalLabels = LogitudeSettings.WorkEnvironment == "customs" ? false : true;
        }

        public static void MapUserUserPM(UserPM userPM, User user)
        {
            user.BranchId = userPM.BranchId;
            user.DepartmentId = userPM.DepartmentId;
            user.Notes = userPM.Notes;
            user.Tenant = userPM.Tenant;
            user.SearchFields = userPM.SearchFields;
            user.BusinessUnitId = userPM.BusinessUnitId;
        }

        public static void InsertUser(UserPM user, UserRepository usersRepository, ContactRepository contactsRepository, RoleRepository rolesRepository, ContactTenantRoleRepository contactTenantRolesRepository, ContactTenantRepository contactTenantsRepository)
        {
            user.Email = user.Email.ToLower();
            Contact newContact = new Contact();
            MapUserToContact(user, newContact);

            newContact.Email = newContact.Email.ToLower();
            Contact adminContact = contactsRepository.GetSingleContactByEmail("admin@fnarsoft.com", 0);
            if (adminContact != null)
            {
                newContact.Signature = adminContact.Signature;
                newContact.SignatureHtml = adminContact.SignatureHtml;
            }

            #region insertContact
            newContact.Id = IdCounter.GetNumber("Contact", user.Tenant).ToString();
            newContact.ComputedKey = (!string.IsNullOrEmpty(newContact.Email) ? newContact.Email : newContact.Id);
            //newContact.MustChangePassword = true;
            newContact.UserType = user.UserType;

            if (string.IsNullOrEmpty(user.Password))
            {
                user.Password = "123";
            }

            //newContact.Password = PasswordGenerator.GetHashedPassword(user.Email, user.Password);

            ContactTenant newContactTenant = new ContactTenant()
            {
                Id = IdCounter.GetNumber("ContactTenant", user.Tenant).ToString(),
                TenantId = newContact.Tenant,
                ContactId = newContact.Id,
            };

            contactsRepository.Add(newContact);
            RoleQuery roleQuery = new RoleQuery(rolesRepository);

            #region admin role for signup
            if (user.SignupRole)
            {
                RolePM adimnrole = roleQuery.GetSinglePMByName("Administrator", 0);

                ContactTenantRole admincontactTenantRole = new ContactTenantRole()
                {
                    ContactTenantId = newContactTenant.Id,
                    Id = IdCounter.GetNumber("ContactTenantRole", newContact.Tenant).ToString(),
                    RoleId = adimnrole.Id,
                    Tenant = newContact.Tenant,
                };
                contactTenantRolesRepository.Add(admincontactTenantRole);
            }
            #endregion

            if (!string.IsNullOrEmpty(newContact.Email))
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == newContact.Email).FirstOrDefault();
                    if (contactPassword == null)
                    {
                        string newPassword = PasswordGenerator.GetBCryptHashedPassword(user.Email, user.Password);

                        contactPassword = new ContactPassword()
                        {
                            Email = newContact.Email,
                            Password = newPassword,
                            IsLocked = false,
                            NumberOfRetries = 0,
                            MustChangePassword = true,
                            IsBCrypt = true,

                        };

                        globalContext.ContactPasswords.Add(contactPassword);
                    }
                    GlobalContactRepository globalContactRep = new GlobalContactRepository(globalContext);

                    bool globalContactExists = (from a in globalContactRep.GetGlobalContactByTenant(newContact.Tenant)
                                                where a.Email == newContact.Email
                                                select a).Any();
                    if (!globalContactExists)
                    {

                        GlobalContact gcontact = new GlobalContact() { Email = newContact.Email, Id = newContact.Id, GlobalTenantId = newContact.Tenant, IsUser = true, };

                        globalContactRep.Add(gcontact);
                        globalContactRep.SubmitChanges();

                        scope.Complete();

                    }
                }
                contactTenantsRepository.Add(newContactTenant);
            }
            #endregion

            User newUser = new User();
            newUser.Id = newContact.Id;
            user.Id = newUser.Id;
            MapUserUserPM(user, newUser);
            usersRepository.Add(newUser);
            contactsRepository.SubmitChanges();
            usersRepository.SubmitChanges();
            contactTenantsRepository.SubmitChanges();
            contactTenantRolesRepository.SubmitChanges();
        }

        public static void UpdateDocumentTypes(int tenant, DocumentTypeRepository documentTypeRepository, DocumentTypeCopyRepository documentTypeCopyRepository, Dictionary<string, DocumentTypePM> tenantZeroDocumentTypes, Dictionary<string, DocumentType> currentTenantDocumentTypes, DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository, List<DocumentTypeCustomField> tenantZeroCustomFields, DocumentTypeTemplateRepository documentTypeTemplateRepository)
        {
            foreach (DocumentTypePM docType in tenantZeroDocumentTypes.Values)
            {
                AutomationHelper automationHelper = new AutomationHelper();
                List<string> automationDocumentTypeIds = automationHelper.GetAutomationDocumentTypeIds(tenant);
                if ((!docType.InActive && docType.IsCopiedAtSignup && docType.IsEnabledForCustomers) || automationDocumentTypeIds.Contains(docType.Id))
                {
                    DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(documentTypeTemplateRepository);


                    if (!currentTenantDocumentTypes.Keys.Contains(docType.Code + docType.ObjectTableId))
                    {
                        bool codeExists = (from a in currentTenantDocumentTypes.Values
                                           where a.Code.Trim().ToUpper() == docType.Code.Trim().ToUpper()
                                           select a).Any();
                        if (!codeExists)
                        {
                            List<DocumentTypeCustomField> zeroCustomFields = tenantZeroCustomFields.Where(d => d.DocumentTypeId == docType.Id).ToList();
                            DocumentType newDocType = new DocumentType()
                            {
                                Id = IdCounter.GetNumber("DocumentType", tenant).ToString(),
                                Code = docType.Code.Trim(),
                                Name = docType.Name,
                                IsOcean = docType.IsOcean,
                                IsAir = docType.IsAir,
                                IsInland = docType.IsInland,
                                IsDocIn = docType.IsDocIn,
                                IsDocOut = false,
                                FollowUpTypeId = docType.FollowUpTypeId,
                                Tenant = tenant,
                                ObjectTableId = docType.ObjectTableId,
                                SearchFields = docType.SearchFields,
                                IsMaster = docType.IsMaster,
                                IsDirect = docType.IsDirect,
                                IsHouse = docType.IsHouse,
                                TemplateFormatCode = docType.TemplateFormatCode,
                                IsCustomerView = docType.IsCustomerView,
                                IsAgentView = docType.IsAgentView,
                                DocumentTypeCategoryCode = docType.DocumentTypeCategoryCode,
                                CountryCode = docType.CountryCode,
                                Subject = docType.Subject,
                                IsEnabledForCustomers = true,
                                Notes = docType.Notes,
                                DocumentTypeDefaultHTMLTemplateId = docType.DocumentTypeDefaultHTMLTemplateId,
                                DocumentTypeDefaultReportTemplateId = docType.DocumentTypeDefaultReportTemplateId,
                                IsSystemAdditionalPrintingFields = docType.IsSystemAdditionalPrintingFields,
                                PrintingFieldsScreenCode = docType.PrintingFieldsScreenCode,

                            };
                            documentTypeRepository.Add(newDocType);

                            foreach (DocumentTypeCopyPM copy in docType.DocumentTypeCopies)
                            {
                                if (!copy.InActive)
                                {
                                    DocumentTypeCopy newCopy = new DocumentTypeCopy()
                                    {
                                        Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                                        Code = copy.Code,
                                        Name = copy.Name,
                                        Tenant = tenant,
                                        IndexOrder = copy.IndexOrder,
                                        IsSelectedByDefault = copy.IsSelectedByDefault,
                                        DocumentTypeId = newDocType.Id,
                                    };
                                    documentTypeCopyRepository.Add(newCopy);
                                }
                            }
                            foreach (DocumentTypeCustomField customField in zeroCustomFields)
                            {
                                DocumentTypeCustomField newCustomField = new DocumentTypeCustomField()
                                {
                                    DocumentTypeId = newDocType.Id,
                                    DefaultValue = customField.DefaultValue,
                                    FieldCode = customField.FieldCode,
                                    FieldDataTypeCode = customField.FieldDataTypeCode,
                                    Id = IdCounter.GetNumber("DocumentTypeCustomField", tenant).ToString(),
                                    InActive = customField.InActive,
                                    IndexOrder = customField.IndexOrder,
                                    IsRequired = customField.IsRequired,
                                    MultiLine = customField.MultiLine,
                                    Name = customField.Name,
                                    Tenant = tenant,
                                };
                                documentTypeCustomFieldRepository.Add(newCustomField);
                            }

                            List<DocumentTypeTemplatePM> documentTypeTemplateList = documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeId(docType.Id, 0).ToList();
                            foreach (DocumentTypeTemplatePM a in documentTypeTemplateList)
                            {
                                DocumentTypePM documenttype = tenantZeroDocumentTypes.Values.Where(d => d.Id == a.DocumentTypeId && d.Tenant == 0).FirstOrDefault();

                                bool isDefault = (from doc in tenantZeroDocumentTypes.Values
                                                  where doc.DocumentTypeDefaultHTMLTemplateId == a.Id || doc.DocumentTypeDefaultReportTemplateId == a.Id
                                                  select doc).Any();


                                if ((a.IsEnabledForCustomers && a.IsCopiedAtSignup) || automationDocumentTypeIds.Contains(docType.Id))
                                {
                                    DocumentTypeTemplate newtemplate = new DocumentTypeTemplate()
                                    {
                                        Id = IdCounter.GetNumber("DocumentTypeTemplate", tenant).ToString(),
                                        Tenant = tenant,
                                        TemplateBody = a.TemplateBody,
                                        TemplateType = a.TemplateType,
                                        HorizontalShift = a.HorizontalShift,
                                        InActive = a.InActive,
                                        Description = a.Description,
                                        DocumentTypeId = newDocType.Id,
                                        EditorTool = a.EditorTool,
                                        CountryCode = a.CountryCode,
                                        Subject = a.CountryCode,
                                        Language = a.Language,
                                        OriginalTemplateId = a.Id,
                                        VerticalShift = a.VerticalShift,
                                        InternalRemarks = a.InternalRemarks,
                                        IsEnabledForCustomers = true,
                                        TemplateBodyHtml = a.TemplateBodyHtml,
                                        TemplateFooterHtml = a.TemplateFooterHtml,
                                        TemplateHeaderHtml = a.TemplateHeaderHtml,
                                       TemplateFooterHeight = a.TemplateFooterHeight,
                                       TemplateHeaderHeight = a.TemplateHeaderHeight,
                                       CC  = a.CC,
                                       From = a.From,
                                       ReplyTo = a.ReplyTo,
                                       
                                       

                                    };
                                    if (isDefault)
                                    {
                                        if (newtemplate.TemplateType == "P")
                                        {
                                            newDocType.DocumentTypeDefaultReportTemplateId = newtemplate.Id;
                                        }
                                        else
                                        {
                                            newDocType.DocumentTypeDefaultHTMLTemplateId = newtemplate.Id;
                                        }
                                    }
                                    newDocType.IsDocOut = documenttype.IsDocOut;

                                    documentTypeTemplateRepository.Add(newtemplate);


                                }
                            }

                        }
                    }

                }
                try
                {
                    documentTypeRepository.SubmitChanges();
                    documentTypeCopyRepository.SubmitChanges();
                    documentTypeCustomFieldRepository.SubmitChanges();
                    documentTypeTemplateRepository.SubmitChanges();
                }
                catch (Exception ee)
                {
                    AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Error);
                    throw;
                }
            }
        }

       

        private static void UpdateTenantVersion(int tenant, TenantRepository tenantRepository)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
                GlobalTenant currentglobaltenant = globalTenantRepository.GetGlobalTenantsByTenant(tenant);
                GlobalTenant globaltenant = globalTenantRepository.GetGlobalTenantsByTenant(0);
                currentglobaltenant.Version = globaltenant.Version;
                globalTenantRepository.Update(currentglobaltenant);
                globalTenantRepository.SubmitChanges();
                scope.Complete();
            }

            //Tenant CurrentTenant = TenantRepository.GetSingleTenant(tenant);
            //Tenant ZeroTenant = TenantRepository.GetSingleTenant(0);
            //CurrentTenant.Version = ZeroTenant.Version;
            //TenantRepository.Update(CurrentTenant);
            //TenantRepository.SubmitChanges();
        }

        private static void UpdateTranslationHeaders(int tenant, Dictionary<string, TranslationHeader> tenantZeroTranslationHeader, Dictionary<string, TranslationHeader> currentTenantTranslationHeaders, TranslationHeaderRepository translationHeadersRepository)
        {
            foreach (TranslationHeader header in tenantZeroTranslationHeader.Values)
            {
                if (!currentTenantTranslationHeaders.Keys.Contains(header.Description))
                {
                    TranslationHeader newTranslationHeader = new TranslationHeader()
                    {
                        Description = header.Description,
                        Code = header.Code,//IdCounter.GetNumber("TranslationHeader").ToString(),

                    };
                    translationHeadersRepository.Add(newTranslationHeader);
                }
            }
            translationHeadersRepository.SubmitChanges();
        }

        private static void UpdateMeasurements(int tenant, Dictionary<string, Measurement> tenantZeroMeasurements, Dictionary<string, Measurement> currentTenantMeasurements, MeasurementRepository measurementRepository)
        {
            foreach (Measurement measurement in tenantZeroMeasurements.Values)
            {
                if (currentTenantMeasurements.Keys.Contains(measurement.Code))
                {
                    Measurement updatedMeasurement = currentTenantMeasurements[measurement.Code];
                    updatedMeasurement.InActive = measurement.InActive;
                    updatedMeasurement.IsContainer = measurement.IsContainer;
                    updatedMeasurement.IsContainerMeasurement = measurement.IsContainerMeasurement;
                    updatedMeasurement.Name = measurement.Name;
                    updatedMeasurement.ShortName = measurement.ShortName;
                    updatedMeasurement.Tenant = tenant;
                    updatedMeasurement.SearchFields = measurement.SearchFields;
                    measurementRepository.Update(updatedMeasurement);
                }
                else
                {
                    Measurement newMeasurement = new Measurement()
                    {
                        Tenant = tenant,
                        ShortName = measurement.ShortName,
                        Name = measurement.Name,
                        IsContainerMeasurement = measurement.IsContainerMeasurement,
                        IsContainer = measurement.IsContainer,
                        InActive = measurement.InActive,
                        Code = measurement.Code,
                        SearchFields = measurement.SearchFields,
                        Id = IdCounter.GetNumber("Measurement", tenant).ToString(),

                    };
                    measurementRepository.Add(newMeasurement);
                }
            }
            measurementRepository.SubmitChanges();
        }

        private static void UpdateCreditCardTypes(int tenant, Dictionary<string, CreditCardType> tenantZeroCreditCardTypes, Dictionary<string, CreditCardType> currentTenantCreditCardTypes, CreditCardTypeRepository creditCardTypeRepository)
        {
            foreach (CreditCardType creditCardType in tenantZeroCreditCardTypes.Values)
            {
                if (currentTenantCreditCardTypes.Keys.Contains(creditCardType.Code))
                {
                    CreditCardType updatedCreditCardType = currentTenantCreditCardTypes[creditCardType.Code];

                    updatedCreditCardType.Name = creditCardType.Name;
                    updatedCreditCardType.Tenant = tenant;
                    updatedCreditCardType.SearchFields = creditCardType.SearchFields;
                    creditCardTypeRepository.Update(updatedCreditCardType);
                }
                else
                {
                    CreditCardType newCreditCardType = new CreditCardType()
                    {
                        Tenant = tenant,
                        Name = creditCardType.Name,
                        Code = creditCardType.Code,
                        SearchFields = creditCardType.SearchFields,
                        Id = IdCounter.GetNumber("CreditCardType", tenant).ToString(),

                    };
                    creditCardTypeRepository.Add(newCreditCardType);
                }
            }
            creditCardTypeRepository.SubmitChanges();
        }

        private static void UpdateMoveTypes(int tenant, Dictionary<string, MoveType> tenantZeroMoveTypes, Dictionary<string, MoveType> currentTenantMoveTypes, MoveTypeRepository moveTypeRepository)
        {
            foreach (MoveType moveType in tenantZeroMoveTypes.Values)
            {
                if (currentTenantMoveTypes.Keys.Contains(moveType.Code))
                {
                    MoveType updatedMoveType = currentTenantMoveTypes[moveType.Code];

                    updatedMoveType.MoveTypeEnglishName = moveType.MoveTypeEnglishName;
                    updatedMoveType.MoveTypeLocalName = moveType.MoveTypeLocalName;
                    updatedMoveType.AddedManually = moveType.AddedManually;
                    updatedMoveType.InActive = moveType.InActive;
                    updatedMoveType.TransportModeId = moveType.TransportModeId;
                    updatedMoveType.Tenant = tenant;
                    updatedMoveType.SearchFields = moveType.SearchFields;
                    moveTypeRepository.Update(updatedMoveType);
                }
                else
                {
                    MoveType newMoveType = new MoveType()
                    {
                        Tenant = tenant,
                        MoveTypeEnglishName = moveType.MoveTypeEnglishName,
                        MoveTypeLocalName = moveType.MoveTypeLocalName,
                        AddedManually = moveType.AddedManually,
                        InActive = moveType.InActive,
                        TransportModeId = moveType.TransportModeId,
                        Code = moveType.Code,
                        SearchFields = moveType.SearchFields,
                        Id = IdCounter.GetNumber("MoveType", tenant).ToString(),

                    };
                    moveTypeRepository.Add(newMoveType);
                }
            }
            moveTypeRepository.SubmitChanges();
        }

        private static void UpdateEntityStatus(int tenant, Dictionary<string, EntityStatus> tenantZeroEntityStatus, Dictionary<string, EntityStatus> currentTenantEntityStatus, EntityStatusRepository entityStatusRepository,bool isHybridTenant)
        {

            string shipmentTableId = ObjectTableRepository.GetObjectTableByName("Shipment");
            foreach (EntityStatus entityStatus in tenantZeroEntityStatus.Values)
            {
                if (isHybridTenant)
                {
                    if (entityStatus.ObjectTableId == shipmentTableId)
                        continue;
                }

                if (currentTenantEntityStatus.Keys.Contains(entityStatus.Code))
                {
                    EntityStatus updatedEntityStatus = currentTenantEntityStatus[entityStatus.Code];
                    updatedEntityStatus.Name = entityStatus.Name;
                    updatedEntityStatus.DisplayName = !string.IsNullOrEmpty(entityStatus.DisplayName) ? entityStatus.DisplayName : entityStatus.Name;  
                    updatedEntityStatus.ObjectTableId = entityStatus.ObjectTableId;
                    updatedEntityStatus.Tenant = tenant;
                    updatedEntityStatus.StatusWeight = entityStatus.StatusWeight;
                    updatedEntityStatus.InActive = entityStatus.InActive;
                    updatedEntityStatus.SearchFields = entityStatus.SearchFields;
                    entityStatusRepository.Update(updatedEntityStatus);
                }
                else
                {
                    EntityStatus newEntityStatus = new EntityStatus()
                    {
                        Tenant = tenant,
                        ObjectTableId = entityStatus.ObjectTableId,
                        StatusWeight = entityStatus.StatusWeight,
                        Name = entityStatus.Name,
                        DisplayName = !string.IsNullOrEmpty(entityStatus.DisplayName) ? entityStatus.DisplayName : entityStatus.Name,
                        InActive = entityStatus.InActive,
                        Code = entityStatus.Code,
                        SearchFields = entityStatus.SearchFields,
                        Id = IdCounter.GetNumber("EntityStatus", tenant).ToString(),
                    };
                    entityStatusRepository.Add(newEntityStatus);
                    currentTenantEntityStatus.Add(newEntityStatus.Code, newEntityStatus);
                }

            }
            entityStatusRepository.SubmitChanges();
        }

        private static void UpdateEventTypes(int tenant, Dictionary<string, EventType> tenantZeroEventTypes, Dictionary<string, EventType> currentTenantEventTypes, EventTypeRepository eventTypesRepository, Dictionary<string, EntityStatus> tenantZeroEntityStatus, Dictionary<string, EntityStatus> currentTenantEntityStatus, bool isHybridTenant)
        {
            string shipmentTableId = ObjectTableRepository.GetObjectTableByName("Shipment");

            foreach (EventType eventType in tenantZeroEventTypes.Values)
            {
                if (isHybridTenant)
                {
                    if (eventType.ObjectTableId == shipmentTableId)
                        continue;
                }

                EntityStatus tenantZeroEntityStatu = tenantZeroEntityStatus.Values.Where(d => d.Id == eventType.EntityStatusId).FirstOrDefault();
                EntityStatus currentTenantEntityStatu = null;

                if (tenantZeroEntityStatu != null)
                {
                    currentTenantEntityStatu = currentTenantEntityStatus[tenantZeroEntityStatu.Code];
                }

               
                if (currentTenantEventTypes.Keys.Contains(eventType.Code + eventType.ObjectTableId))
                {

                    
                    EventType updatedEventType = currentTenantEventTypes[eventType.Code + eventType.ObjectTableId];

                    updatedEventType.EnglishName = eventType.EnglishName;
                    updatedEventType.AddedManually = eventType.AddedManually;
                    updatedEventType.EntityStatusId = currentTenantEntityStatu != null ? currentTenantEntityStatu.Id : null;
                    updatedEventType.FollowUpEnglishName = eventType.FollowUpEnglishName;
                    updatedEventType.FollowUpLocalName = eventType.FollowUpLocalName;
                    updatedEventType.InActive = eventType.InActive;
                    updatedEventType.IsFollowUp = eventType.IsFollowUp;
                    updatedEventType.IsManualEntry = eventType.IsManualEntry;
                    updatedEventType.LocalName = eventType.LocalName;
                    updatedEventType.ManualActivatedFollowUp = eventType.ManualActivatedFollowUp;
                    updatedEventType.ObjectTableId = eventType.ObjectTableId;
                    updatedEventType.ShortView = eventType.ShortView;
                    updatedEventType.Tenant = tenant;
                    updatedEventType.SearchFields = eventType.SearchFields;
                    updatedEventType.EventTypeCategoryCode = eventType.EventTypeCategoryCode;
                    updatedEventType.AllowedInAutomation = eventType.AllowedInAutomation;
                    eventTypesRepository.Update(updatedEventType);
                }

                else
                {
                    EventType newEventType = new EventType()
                    {
                        Tenant = tenant,
                        ShortView = eventType.ShortView,
                        ObjectTableId = eventType.ObjectTableId,
                        ManualActivatedFollowUp = eventType.ManualActivatedFollowUp,
                        LocalName = eventType.LocalName,
                        IsManualEntry = eventType.IsManualEntry,
                        IsFollowUp = eventType.IsFollowUp,
                        InActive = eventType.InActive,
                        FollowUpLocalName = eventType.FollowUpLocalName,
                        FollowUpEnglishName = eventType.FollowUpEnglishName,
                        EntityStatusId = currentTenantEntityStatu != null ? currentTenantEntityStatu.Id : null,
                        AddedManually = eventType.AddedManually,
                        Code = eventType.Code,
                        EnglishName = eventType.EnglishName,
                        SearchFields = eventType.SearchFields,
                        Id = IdCounter.GetNumber("EventType", tenant).ToString(),
                        EventTypeCategoryCode = eventType.EventTypeCategoryCode,
                        AllowedInAutomation = eventType.AllowedInAutomation,
                    };

                    eventTypesRepository.Add(newEventType);
                }
            }

            eventTypesRepository.SubmitChanges();
        }

        private static void UpdateRanks(int tenant, Dictionary<string, Rank> tenantZeroRanks, Dictionary<string, Rank> currentTenantRanks, RankRepository rankRepository)
        {
            foreach (Rank rank in tenantZeroRanks.Values)
            {
                if (currentTenantRanks.Keys.Contains(rank.Code))
                {
                    Rank updatedRank = currentTenantRanks[rank.Code];
                    updatedRank.Name = rank.Name;
                    updatedRank.Tenant = tenant;
                    updatedRank.SearchFields = rank.SearchFields;
                    rankRepository.Update(updatedRank);
                }
                else
                {
                    Rank newRank = new Rank()
                    {
                        Tenant = tenant,
                        Name = rank.Name,
                        Code = rank.Code,
                        Id = IdCounter.GetNumber("Rank", tenant).ToString(),
                        SearchFields = rank.SearchFields,
                    };
                    rankRepository.Add(newRank);
                }

            }
            rankRepository.SubmitChanges();
        }

        private static void UpdateEmailAlertSettings(int tenant, Dictionary<string, EmailAlertSetting> tenantZeroEmailSettings, Dictionary<string, EmailAlertSetting> currentEmailSettings, EmailAlertSettingRepository emailAlertSettingRepository)
        {
            foreach (EmailAlertSetting setting in tenantZeroEmailSettings.Values)
            {
                if (currentEmailSettings.Keys.Contains(setting.Code))
                {
                    EmailAlertSetting updatedSetting = currentEmailSettings[setting.Code];
                    updatedSetting.ObjectTableId = setting.ObjectTableId;
                    updatedSetting.Tenant = tenant;
                    updatedSetting.SettingLevelCode = setting.SettingLevelCode;
                    updatedSetting.Description = setting.Description;
                    updatedSetting.IndexOrder = setting.IndexOrder;
                    //updatedSetting.To = setting.To;
                    //updatedSetting.InActive = setting.InActive;
                    emailAlertSettingRepository.Update(updatedSetting);
                }
                else
                {
                    EmailAlertSetting newAlert = new EmailAlertSetting()
                    {
                        Tenant = tenant,
                        ObjectTableId = setting.ObjectTableId,
                        Code = setting.Code,
                        Id = IdCounter.GetNumber("EmailAlertSetting", tenant).ToString(),
                        SettingLevelCode = setting.SettingLevelCode,
                        To = setting.To,
                        InActive = setting.InActive,
                        Description = setting.Description,
                        IndexOrder = setting.IndexOrder,
                    };
                    emailAlertSettingRepository.Add(newAlert);
                }

            }

            emailAlertSettingRepository.SubmitChanges();
        }

        public static void AddBatchServicesDefinitions()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {

                MetaDataUpdateClass updateClass = new MetaDataUpdateClass();
                updateClass.FillBatchServicesDefinitions();
                scope.Complete();
            }
        }

        public static void UpdateFullAccSettings(int tenant, FullAccountingSettingRepository repo)
        {
            if (repo.GetSingleFullAccountingSetting(tenant) == null)
            {
                FullAccountingSetting entity = new FullAccountingSetting();
                entity.Id = tenant.ToString();
                entity.Tenant = tenant;
                repo.Add(entity);
                repo.SubmitChanges();
            }
        }
        private static void UpdateTaxWithholdingAssessingOffices(int tenant, Dictionary<string, TaxWithholdingAssessOffice> tenantZeroTaxWithholdingAssessingOffices, Dictionary<string, TaxWithholdingAssessOffice> currentTaxWithholdingAssessingOffices, TaxWithholdingAssessOfficeRepository taxWithholdingAssessOfficeRepository)
        {

            string TaxWithholdingAssessOfficeTableId = ObjectTableRepository.GetObjectTableByName("TaxWithholdingAssessOffice");
            foreach (TaxWithholdingAssessOffice office in tenantZeroTaxWithholdingAssessingOffices.Values)
            {

                if (currentTaxWithholdingAssessingOffices.Keys.Contains(office.Code))
                {
                    TaxWithholdingAssessOffice taxWithholdingAssessOffice = currentTaxWithholdingAssessingOffices[office.Code];
                    taxWithholdingAssessOffice.Code = office.Code;
                    taxWithholdingAssessOffice.Name = office.Name;
                    taxWithholdingAssessOffice.LocalName = office.LocalName;
                    taxWithholdingAssessOffice.Tenant = tenant;
                    taxWithholdingAssessOffice.Inactive = office.Inactive;
                    taxWithholdingAssessOffice.SearchFields = office.SearchFields;

                    taxWithholdingAssessOfficeRepository.Update(taxWithholdingAssessOffice);
                }
                else
                {
                    TaxWithholdingAssessOffice newTaxWithholdingAssessOffice = new TaxWithholdingAssessOffice()
                    {
                        Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", tenant).ToString(),
                        Code = office.Code,
                        Name = office.Name,
                        LocalName = office.LocalName,
                        Tenant = tenant,
                        Inactive = office.Inactive,
                        SearchFields = office.SearchFields,

                    };
                    taxWithholdingAssessOfficeRepository.Add(newTaxWithholdingAssessOffice);
                    currentTaxWithholdingAssessingOffices.Add(newTaxWithholdingAssessOffice.Code, newTaxWithholdingAssessOffice);
                }

            }
            taxWithholdingAssessOfficeRepository.SubmitChanges();
        }
        private static void UpdateAccountingCompanyTypes(int tenant, Dictionary<string, AccountingCompanyType> tenantZeroAccountingCompanyTypes, Dictionary<string, AccountingCompanyType> currentAccountingCompanyTypes, AccountingCompanyTypeRepository accountingCompanyTypeRepository)
        {

            string AccountingCompanyTypeTableId = ObjectTableRepository.GetObjectTableByName("AccountingCompanyType");
            foreach (AccountingCompanyType type in tenantZeroAccountingCompanyTypes.Values)
            {

                if (currentAccountingCompanyTypes.Keys.Contains(type.Code))
                {
                    AccountingCompanyType accountingCompanyType = currentAccountingCompanyTypes[type.Code];
                    accountingCompanyType.Code = type.Code;
                    accountingCompanyType.EnglishName = type.EnglishName;
                    accountingCompanyType.LocalName = type.LocalName;
                    accountingCompanyType.Tenant = tenant;
                    accountingCompanyType.Inactive = type.Inactive;
                    accountingCompanyType.SearchFields = type.SearchFields;

                    accountingCompanyTypeRepository.Update(accountingCompanyType);
                }
                else
                {
                    AccountingCompanyType newAccountingCompanyType = new AccountingCompanyType()
                    {
                        Id = IdCounter.GetNumber("AccountingCompanyType", tenant).ToString(),
                        Code = type.Code,
                        EnglishName = type.EnglishName,
                        LocalName = type.LocalName,
                        Tenant = tenant,
                        Inactive = type.Inactive,
                        SearchFields = type.SearchFields,

                    };
                    accountingCompanyTypeRepository.Add(newAccountingCompanyType);
                    currentAccountingCompanyTypes.Add(newAccountingCompanyType.Code, newAccountingCompanyType);
                }

            }
            accountingCompanyTypeRepository.SubmitChanges();
        }
        private static void UpdateWithholdingTaxDeductionTypes(int tenant, Dictionary<string, WithholdingTaxDeductionType> tenantZeroWithholdingTaxDeductionTypes, Dictionary<string, WithholdingTaxDeductionType> currentWithholdingTaxDeductionTypes, WithholdingTaxDeductionTypeRepository withholdingTaxDeductionTypeRepository)
        {

            string WithholdingTaxDeductionTypeTableId = ObjectTableRepository.GetObjectTableByName("WithholdingTaxDeductionType");
            foreach (WithholdingTaxDeductionType type in tenantZeroWithholdingTaxDeductionTypes.Values)
            {

                if (currentWithholdingTaxDeductionTypes.Keys.Contains(type.Code))
                {
                    WithholdingTaxDeductionType withholdingTaxDeductionType = currentWithholdingTaxDeductionTypes[type.Code];
                    withholdingTaxDeductionType.Code = type.Code;
                    withholdingTaxDeductionType.EnglishName = type.EnglishName;
                    withholdingTaxDeductionType.LocalName = type.LocalName;
                    withholdingTaxDeductionType.Tenant = tenant;
                    withholdingTaxDeductionType.Inactive = type.Inactive;
                    withholdingTaxDeductionType.SearchFields = type.SearchFields;

                    withholdingTaxDeductionTypeRepository.Update(withholdingTaxDeductionType);
                }
                else
                {
                    WithholdingTaxDeductionType newWithholdingTaxDeductionType = new WithholdingTaxDeductionType()
                    {
                        Id = IdCounter.GetNumber("WithholdingTaxDeductionType", tenant).ToString(),
                        Code = type.Code,
                        EnglishName = type.EnglishName,
                        LocalName = type.LocalName,
                        Tenant = tenant,
                        Inactive = type.Inactive,
                        SearchFields = type.SearchFields,

                    };
                    withholdingTaxDeductionTypeRepository.Add(newWithholdingTaxDeductionType);
                    currentWithholdingTaxDeductionTypes.Add(newWithholdingTaxDeductionType.Code, newWithholdingTaxDeductionType);
                }

            }
            withholdingTaxDeductionTypeRepository.SubmitChanges();
        }


        private static void UpdateJournalActionTypes(int tenant, Dictionary<string, JournalActionType> tenantZeroJournalActionTypes, Dictionary<string, JournalActionType> currentJournalActionTypes, JournalActionTypeRepository journalActionTypeRepository)
        {

            string journalActionTypeTableId = ObjectTableRepository.GetObjectTableByName("JournalActionType");
            foreach (JournalActionType actionType in tenantZeroJournalActionTypes.Values)
            {

                if (currentJournalActionTypes.Keys.Contains(actionType.Code))
                {
                    JournalActionType journalActionType = currentJournalActionTypes[actionType.Id];
                    journalActionType.Code = actionType.Code;
                    journalActionType.EnglishName = actionType.EnglishName;
                    journalActionType.LocalName = actionType.LocalName;
                    journalActionType.Tenant = tenant;
                    journalActionType.Inactive = actionType.Inactive;
                    journalActionType.SearchFields = actionType.SearchFields;
                    journalActionTypeRepository.Update(journalActionType);
                }
                else
                {
                    JournalActionType newJournalActionType = new JournalActionType()
                    {
                        Id = IdCounter.GetNumber("JournalActionType", tenant).ToString(),
                        Code = actionType.Code,
                        EnglishName = actionType.EnglishName,
                        LocalName = actionType.LocalName,
                        Tenant = tenant,
                        Inactive = actionType.Inactive,
                        SearchFields = actionType.SearchFields,

                    };
                    journalActionTypeRepository.Add(newJournalActionType);
                    currentJournalActionTypes.Add(newJournalActionType.Code, newJournalActionType);
                }

            }
            journalActionTypeRepository.SubmitChanges();
        }
    }
}

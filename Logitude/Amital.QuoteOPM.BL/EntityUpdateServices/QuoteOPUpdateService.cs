using Amital.QuoteOPM.BL.Tools;
using Amital.QuoteOPM.BL.Tools.Behaviours;
using Amital.QuoteOPM.BL.Tools.EntityService;
using Amital.QuoteOPM.BL.Tools.Initializers;
using Amital.QuoteOPM.BL.Tools.Validating;
using Amital.QuoteOPM.Data;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.ExternalService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
    public partial class QuoteOPUpdateService : EntityUpdateService<QuoteOP, QuoteOPPM, EntityPM>
    {
        private Tenant loggedTenant;

        //C:\C21R01\Logitude\Logitude.BL\QuoteModel\Tools\EntityService\QuoteService.cs
        private ICommonDataContext myCommonContext;
        private QuoteOPServiceInitializer _Initializer;
        private bool isAdhoc;
        private bool isInlandDomestic;
        private bool isLCLQuote;
        private bool isFCLQuote;
        private List<VatType> allVatTypes;
        private bool isEnableMultiPercentageVATTypes;
        private List<VatTypePercentagePM> allVatPercentages;
        private bool isNewEntity = false;
        private QuoteOPPM entityPM;

        protected override void OnCreating(QuoteOPPM entityPM, EntityPM entityParentPM)
        {
            this.isNewEntity = true;
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant).Date;

            EntityPM.Id = IdCounter.GetNumber("QuoteOP", entityPM.Tenant).ToString();
            string resolveLoggingUserEmail;
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact contact;
            GetLogUser(entityPM, out resolveLoggingUserEmail, out contact);
            entityPM.CreatedByUserId = contact.Id;
            ;
            entityPM.OpenDate = entityPM.IsHybrid ? entityPM.OpenDate : todayDateTime;
            entityPM.LastStageDate = todayDateTime;
            //entityPM.OpenDate;
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            //entityPoco.Tenant = entityPM.Tenant;
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DirectionId);
            //entityPoco.DirectionId = entityPM.DirectionId;
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ProductCode);
            //entityPoco.ProductCode = entityPM.ProductCode;
            OnCreateGetQuoteSettings(entityPM);
            OnCreateInitializeStage(entityPM);
            if (!entityPM.IsHybrid)
            {
                entityPM.QuoteNumber = TableCounter.GetNumber(entityPM.Tenant, "QTOP", entityPM.DirectionId, entityPM.TransportModeId);
            }
            if (entityPM.IsCreatedFromTicket)
            {
                entityPM.RequestDate = entityPM.TicketCreateDate;
            }
            else
            {
                entityPM.RequestDate = entityPM.OpenDate;
            }
            InitQuoteOPServiceInitializer(entityPM, resolveLoggingUserEmail);
            
            QuoteOPValidating.Validate(entityPM, null, true, myCommonContext);

            foreach (var itemPM in entityPM.FollowUps)
            {
                //no FU 4U this.CreateQuoteFollowUp(itemPM);
            }

            base.OnCreating(entityPM, entityParentPM);
        }

        private void InitQuoteOPServiceInitializer(QuoteOPPM entityPM, string resolveLoggingUserEmail)
        {
            if (_Initializer!=null)
            {
                return;
            }
            _Initializer =  new QuoteOPServiceInitializer(this.MainContext as IQuoteOPMContext, entityPM.Tenant, resolveLoggingUserEmail);

            _Initializer.Initialize();
            this.loggedTenant = _Initializer.LoggedTenant; //TenantRepository.GetSingleTenant(tenant, false);
            this.myCommonContext = _Initializer.CommonContext; //CommonDataContext.GetContext(tenant);
            _Initializer.InitializeEntity(entityPM);
            _Initializer.HandleBehaviours();
        }

        private static void GetLogUser(QuoteOPPM entityPM, out string resolveLoggingUserEmail, out Simplog.Data.CommonDataModel.EntityPOCOs.Contact contact)
        {
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            resolveLoggingUserEmail = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            contact = contactRep.GetSingleContactByEmail(resolveLoggingUserEmail, entityPM.Tenant);
        }

        private void OnCreateInitializeStage(QuoteOPPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert  /*isNewEntity*/)
            {
                QuoteOPStage myStage = null;
                string myStageId = null;
                DateTime? stageDueDate = null;
                var stageRepository = new QuoteOPStageRepository(entityPM.Tenant);

                if (!string.IsNullOrEmpty(entityPM.StageId))
                {
                    myStage = stageRepository.GetSingleQuoteOPStage(entityPM.StageId, entityPM.Tenant);
                }

                else
                {
                    myStage = stageRepository.GetSingleQuoteOPStageByCode("QTCR", entityPM.Tenant);
                }

                if (myStage != null)
                {
                    myStageId = myStage.Id;

                    if (myStage.MaxDays != null)
                    {
                        DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                        stageDueDate = todayDateTime.Value.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                    }
                }

                entityPM.StageId = myStageId;
                entityPM.StageDueDate = stageDueDate;
            }
        }
        private void OnCreateGetQuoteSettings(QuoteOPPM entityPM)
        {
            var iQuoteSettingRepository = new QuoteOPSettingRepository(this.MainContext as IQuoteOPMContext);
            var iQuoteSetting = iQuoteSettingRepository.GetSingleQuoteSetting(entityPM.Tenant);

            if (iQuoteSetting != null)
            {
                if (!entityPM.IsCopy)
                {
                    entityPM.IsSaleCurrencySameAsCost = iQuoteSetting.IsSaleAsCostCurrency;
                    entityPM.IsMultiCurrency = iQuoteSetting.IsMultiCurrency;
                }
            }
        }


        protected override void UpdateComposition(QuoteOPPM entityPM)
        {
            if (_Initializer.QuoteComputedFieldPM!=null)
            {
                var quoteOPComputedFieldUpdateService = new QuoteOPComputedFieldUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
                quoteOPComputedFieldUpdateService.Update(_Initializer.QuoteComputedFieldPM, false);

            }

            var quoteOPPackageUpdateService = new QuoteOPPackageUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            quoteOPPackageUpdateService.UpdateMulti(entityPM.QuotePackages, entityPM.DeletedQuotePackages, entityPM, false);

            QuoteOPChargeUpdateService quoteOPChargeUpdateService = new QuoteOPChargeUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            quoteOPChargeUpdateService.UpdateMulti(entityPM.QuoteCharges, entityPM.DeletedQuoteCharges, entityPM, false);


            

            //if (entityPM.IsChargeBySteps)
            {


            }



            base.UpdateComposition(entityPM);
        }


        protected override void OnUpdating(QuoteOPPM entityPM)
        {
            base.OnUpdating(entityPM);
        }
        protected override void OnUpdating(QuoteOPPM entityPM, QuoteOP entityPoco)
        {
            this.entityPM = entityPM;
            this._EntityPoco4Validate = entityPoco;
            if (!entityPoco.IsCancelled || !entityPM.IsCancelled)
            {
                string resolveLoggingUserEmail;
                Simplog.Data.CommonDataModel.EntityPOCOs.Contact contact;
                GetLogUser(entityPM, out resolveLoggingUserEmail, out contact);

                InitQuoteOPServiceInitializer(entityPM, resolveLoggingUserEmail);
                this.OnUpdating_InitializeComponent(entityPM);

                



                //Composition this.UpdateQuoteChargesCollection();
#warning  todo ?? this.UpdateQuoteFollowUpsCollection();
                //Composition this.UpdateQuotePackageCollection();
#warning  todo !! this.UpdateQuoteDocumentVersionCollection();

                this.UpdateTotalVats_TODO_CHANGE2UpdateService(); 

                //QuoteTracing.Trace(entityPM, entityPoco, initializer.LoggedContactId, isNewEntity);

                ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPoco.Tenant);
                var objecttable = objecttableRepository.GetObjectTableByName("QuoteOP", 0, true);

                if (entityPM.QuoteTypeCode != "P")
                {
                    if (!entityPM.DontExportQuotationsToIntegratedSystem)
                    {
                        SentQuoteStatusMessageToUnifreight(objecttable.Id,entityPoco);
                    }

#warning            ???? SendQuoteToIntegratedSystem(objecttable.Id);
                }
                EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = entityPoco, EntityPM = entityPM, OldEntityPM = new QuoteOPPM(), AutomationType = "OnUpdate", ObjectTableName = "QuoteOP", Tenant = entityPM.Tenant, EntityId = entityPM.Id, EntityAutomationMappingPMFields = new EntityAutomationQuoteOPMappingPMFields() });

                //QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);

                //entityRepository.Update(entityPoco);
                //entityRepository.SubmitChanges();
                //quoteComputedFieldRepository.Update(quoteComputedFieldEntityPOCO);
                //quoteComputedFieldRepository.SubmitChanges();
#warning            ????                 followUpRepository.SubmitChanges();

                //map poco 2 pm this.GetForeignFields(entityPM, entityPoco);
#warning LastModified is IsRowVersion() is sqlserver todo change in oracle !!
                int lastMod = 0;
                int.TryParse(entityPM.LastModified, out lastMod);
                lastMod++;
                //entityPM.LastModified = lastMod.ToString();

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "QuoteOP");
                ActivityLogger.AddAcitivityLog(entityPM.Id, objecttable.Id, entityPM.Tenant, "U", _Initializer.LoggedContactId);

                entityAutomationService.RunAutomation();
            }

            else
            {
                // cancelled quotes
                // in case follow ups added
                // from client
#if noQuoteFollowUpPM


                int indexComp = 0;
                if (quoteFollowUpsChangeSet != null)
                {
                    foreach (QuoteFollowUpPM item in quoteFollowUpsChangeSet)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            indexComp++;
                            item.Id = "QuoteFollowUpPM_" + indexComp;
                        }
                    }
                }

            if (entityPM.IsCancelled)
            {
                List<FollowUp> allFollowupLists = this.followUpRepository.GetFollowUpsByQuoteId(entityPM.Id, entityPM.Tenant);
                foreach (FollowUp item in allFollowupLists)
                {
                    followUpRepository.Remove(item);
                }

                followUpRepository.SubmitChanges();

                entityPM.FollowUps = new List<QuoteFollowUpPM>();
            }



            this.quoteFollowUpUpdateService = new QuoteFollowUpUpdateService(entityPM, entityPM.Tenant); 


            quoteFollowUpUpdateService.RefreshFollowUps(); 
#endif
            }
            if (string.IsNullOrWhiteSpace(entityPM.StageId))
            {
#warning entityPM.StageId IS MUST NO BL ??
                entityPM.StageId = "-1";
            }
            base.OnUpdating(entityPM, entityPoco);
        }
        private bool CanSendQuoteToIntegratedSystem(QuoteOP entityPoco)
        {
            int tenant = entityPM.Tenant;
            bool canSendQuote = false;

            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            if (tenantPM.ExportQuotationsToIntegratedSystem)
            {
                var myQuoteStageRepository = new QuoteOPStageRepository(tenant);
                var quoteStageSend = myQuoteStageRepository.GetQuoteOPStages(tenant).Where(d => d.Code == "QTST").FirstOrDefault();
                var quoteStageAccepted = myQuoteStageRepository.GetQuoteOPStages(tenant).Where(d => d.Code == "QTAC").FirstOrDefault();

                bool quoteStatusChangedToSent = (quoteStageSend != null && quoteStageSend.Id == entityPM.StageId && entityPoco.StageId != quoteStageSend.Id);
                bool quoteStatusChangedAccept = (quoteStageAccepted != null && quoteStageAccepted.Id == entityPM.StageId && entityPoco.StageId != quoteStageAccepted.Id);

                if ((tenantPM?.TransferQuotationsToUnifreightTrigger == "OnSend" && quoteStatusChangedToSent)
                    || (tenantPM?.TransferQuotationsToUnifreightTrigger == "OnAccept" && quoteStatusChangedAccept))
                {
                    canSendQuote = true;
                }
            }

            return canSendQuote;
        }

        public void SentQuoteStatusMessageToUnifreight(string objectTableId, QuoteOP entityPoco)
        {
            int tenant = entityPM.Tenant;
            //TenantQuery tenantQuery = new TenantQuery(tenant);
            //TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);

            if (CanSendQuoteToIntegratedSystem(entityPoco))
            {
                bool IsQuoteStageChange = false;
                if (entityPM.StageId != entityPoco.StageId || entityPM.IsCancelled)
                {
                    TraceEventRepository traceEventRepository = new TraceEventRepository(entityPM.Tenant);
                    QuoteStatus quoteStatus = new QuoteStatus();
                    quoteStatus.Stage = new Stage();
                    quoteStatus.QuoteDeclineReason = new QuoteDeclineReason();
                    quoteStatus.QuoteCancelNote = "";
                    quoteStatus.QuoteNumber = entityPM.QuoteNumber;
                    quoteStatus.DueDate = entityPM.StageDueDate;
                    #region IsCancelled 
                    if (entityPM.IsCancelled)
                    {
                        EventTypeRepository eventTypeRepository = new EventTypeRepository(entityPM.Tenant);
                        string eventTypeId = eventTypeRepository.GetSingleEventTypeIdByCode("CLQT", entityPM.Tenant);
                        if (!string.IsNullOrEmpty(eventTypeId))
                        {
                            TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventTypeId, entityPM.Tenant);
                            if (traceEvent != null)
                            {
                                quoteStatus.IsQuoteCancel = true;
                                quoteStatus.QuoteCancelDate = traceEvent.EventDateTime;
                                quoteStatus.QuoteCancelNote = traceEvent.Notes;
                            }
                        }
                    }
                    #endregion

                    #region StatusChange
                    if (entityPoco.StageId != entityPM.StageId)
                    {
                        var quoteStageRepository = new QuoteOPStageRepository(entityPM.Tenant);
                        var stage = quoteStageRepository.GetSingleQuoteOPStage(entityPM.StageId, entityPM.Tenant);
                        if (stage != null)
                        {
                            if (stage.Code == "QTAC" || stage.Code == "QTST" || stage.Code == "QTDC")
                            {
                                IsQuoteStageChange = true;
                                quoteStatus.Stage.Code = stage.Code;
                                quoteStatus.Stage.Id = stage.Id;
                                quoteStatus.Stage.Name = stage.Name;
                                quoteStatus.Stage.StageDate = entityPM.LastStageDate;
                                if (stage.Code == "QTAC") quoteStatus.QuoteAcceptNote = entityPM.EventNote;
                                if (stage.Code == "QTDC")
                                {
                                    var closingReasonRepository = new QuoteOPClosingReasonRepository(tenant);
                                    var myQuoteClosingReason = closingReasonRepository.GetSingleQuoteOPClosingReason(entityPM.QuoteClosingReasonId, tenant);
                                    if (myQuoteClosingReason != null)
                                    {
                                        quoteStatus.QuoteDeclineReason.Code = myQuoteClosingReason.Code;
                                        quoteStatus.QuoteDeclineReason.Name = myQuoteClosingReason.Name;
                                        quoteStatus.QuoteDeclineReason.Note = entityPM.EventNote;
                                    }
                                }


                            }
                        }

                    }
                    #endregion

                    #region Send Quote Status Message 
                    if (entityPM.IsCancelled || IsQuoteStageChange)
                    {
                        string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(quoteStatus);

                        CommunicationsParams tasklogParams = new CommunicationsParams()
                        {
                            Tenant = entityPM.Tenant,
                            CommunicationLogTypeCode = "Q",
                            QueueName = "externaltasksqueue" + entityPM.Tenant + 1,
                            Priority = 1,
                            InOut = "O",
                            Status = "W",
                            LoggingUserId = entityPM.UpdatedByUserId,
                            LoggingObjectTableId = objectTableId,
                            LoggingEntityId = entityPM.Id,
                            Subject = "Quote Status Change",
                            FolderName = "ExternalTasksQueue",
                        };

                        List<QueueTask> queueTasks = new List<QueueTask>();
                        queueTasks.Add(
                            new QueueTask()
                            {
                                Action = "QuoteStatusMessageSentToUnifreight",
                                Parameters = new List<Parameter>()
                                {
                                    new Parameter{ Name = "QuoteStatusMetaData", Order = 1,Value =  xmlstring},
                                }
                            });

                        tasklogParams.ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks);
                        Communications.AddCommunicationLog(tasklogParams);
                    }
                    #endregion
                }
            }
        }

        private void UpdateTotalVats_TODO_CHANGE2UpdateService()
        {
#warning UpdateTotalVats_TODO_CHANGE2UpdateService!!!!!
            int tenant = entityPM.Tenant;
            var quoteTotalVATRepository = new QuoteOPTotalVATRepository(entityPM.Tenant);
            List<QuoteOPTotalVAT> dbTotalVats = quoteTotalVATRepository.GetTotalVATs(entityPM.Id, entityPM.Tenant).ToList();

            foreach (var item in dbTotalVats)
            {
                quoteTotalVATRepository.Remove(item);
            }

            if (entityPM.QuoteTypeCode == "A")
            {
                if (entityPM.IsChargesByVAT)
                {
                    var myDataLines = this.entityPM.QuoteCharges.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null && d.IsAllIN == false).ToList();

                    if (myDataLines.Count > 0)
                    {
                        double? myProfitCurrencyRate = null;
                        RatesTableQuery myQuery = new RatesTableQuery(tenant);
                        LastRate lastRate = myQuery.GetLastRecordByValueDate(tenant, _Initializer.LoggedTenant.ProfitCurrencyId, _Initializer.LoggedTenant.CurrencyId, entityPM.UpdateDate);
                        if (lastRate != null)
                        {
                            myProfitCurrencyRate = MethodHelper.Round(lastRate.Rate, 5);
                        }

                        List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == tenant select d).ToList();

                        List<QuoteTotalsClass> group_Source = new List<QuoteTotalsClass>();

                        foreach (var item in myDataLines)
                        {
                            #region
                            VatType lineVatType = this.allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                            if (lineVatType != null)
                            {
                                if (!lineVatType.IsMultiPercentage)
                                {
                                    QuoteTotalsClass newItem = new QuoteTotalsClass()
                                    {
                                        Id = item.VatTypeId,
                                        VatTypeId = item.VatTypeId,
                                        VatTypePercentage = item.VatPercentage,
                                        QuoteCurrencyAmount = item.SaleAmountInSaleCurrency,
                                        LocalCurrencyAmount = item.SaleTotalAmountLocal,
                                        ExternalVATCard = lineVatType.ReceivablesExternalId,
                                        ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                                    };

                                    if (item.IsRegionalTax)
                                    {
                                        newItem.QuoteCurrencyAmount = item.SaleAmountInSaleCurrency + item.SaleAmountInSaleCurrency * (entityPM.RegionalTaxPercentage / 100);
                                        newItem.LocalCurrencyAmount = item.SaleTotalAmountLocal + item.SaleTotalAmountLocal * (entityPM.RegionalTaxPercentage / 100);

                                        QuoteTotalsClass newRegionalTaxItem = new QuoteTotalsClass()
                                        {
                                            Id = entityPM.RegionalTaxId,
                                            VatTypeId = entityPM.RegionalTaxId,
                                            VatTypePercentage = entityPM.RegionalTaxPercentage,
                                            QuoteCurrencyAmount = item.SaleAmountInSaleCurrency,
                                            LocalCurrencyAmount = item.SaleTotalAmountLocal,
                                            ExternalVATCard = newItem.ExternalVATCard,
                                            ExternalTAXItemId = newItem.ExternalTAXItemId,
                                            //IsRegionalTax = true,
                                        };

                                        group_Source.Add(newRegionalTaxItem);
                                    }

                                    group_Source.Add(newItem);
                                }

                                else
                                {
                                    List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();
                                    foreach (VATTypesGroup itemGroup in myVatGroups)
                                    {
                                        QuoteTotalsClass newItem = new QuoteTotalsClass()
                                        {
                                            Id = itemGroup.SingleVATTypeId,
                                            VatTypeId = itemGroup.SingleVATTypeId,
                                            QuoteCurrencyAmount = item.SaleAmountInSaleCurrency,
                                            LocalCurrencyAmount = item.SaleTotalAmountLocal,
                                        };

                                        VatType vatType = this.allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                        if (vatType != null)
                                        {
                                            newItem.ExternalVATCard = vatType.ReceivablesExternalId;
                                            newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                        }

                                        VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                        if (myPercentagePM != null)
                                        {
                                            newItem.VatTypePercentage = myPercentagePM.Percentage;
                                        }

                                        group_Source.Add(newItem);
                                    }
                                }
                            }
                            #endregion
                        }

                        List<QuoteTotalsClass> group_data =
                            (from items in group_Source
                             group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVATCard, items.ExternalTAXItemId } into g
                             select new QuoteTotalsClass()
                             {
                                 Id = g.Key.VatTypeId,
                                 VatTypeId = g.Key.VatTypeId,
                                 VatTypePercentage = g.Key.VatTypePercentage,
                                 ExternalVATCard = g.Key.ExternalVATCard,
                                 ExternalTAXItemId = g.Key.ExternalTAXItemId,
                                 QuoteCurrencyAmount = g.Sum(s => s.QuoteCurrencyAmount),
                                 LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                             }).ToList();

                        foreach (var item in group_data)
                        {
                            if (entityPM.SaleCurrencyId == _Initializer.LoggedTenant.ProfitCurrencyId)
                            {
                                item.ProfitCurrencyAmount = item.QuoteCurrencyAmount;
                            }

                            else if (entityPM.SaleCurrencyId == _Initializer.LoggedTenant.CurrencyId)
                            {
                                item.ProfitCurrencyAmount = item.LocalCurrencyAmount;
                            }

                            else
                            {
                                item.ProfitCurrencyAmount = MethodHelper.Roundd(item.LocalCurrencyAmount / myProfitCurrencyRate, 2);
                            }

                            var record = new QuoteOPTotalVAT()
                            {
                                Id = IdCounter.GetNumber("QuoteTotalVAT", entityPM.Tenant).ToString(),
                                Tenant = entityPM.Tenant,
                                QuoteOPId = entityPM.Id,
                                VatOPTypeId = item.Id,
                                VatPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                                LocalCurrencyVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                                QuoteCurrencyVatableAmount = MethodHelper.Roundd(item.QuoteCurrencyAmount, 2),
                                ProfitCurrencyVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                                ExternalVATCard = item.ExternalVATCard,
                                ExternalTAXItemId = item.ExternalTAXItemId
                            };

                            record.LocalCurrencyVATAmount = MethodHelper.Round((record.LocalCurrencyVatableAmount * record.VatPercent / 100), 2);
                            record.QuoteCurrencyVATAmount = MethodHelper.Round((record.QuoteCurrencyVatableAmount * record.VatPercent / 100), 2);
                            record.ProfitCurrencyVATAmount = MethodHelper.Round((record.ProfitCurrencyVatableAmount * record.VatPercent / 100), 2);
                            quoteTotalVATRepository.Add(record);
                        }
                    }
                }
            }
        }
        private void OnUpdating_InitializeComponent(QuoteOPPM entityPM)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant).Date;

            this.entityPM.MarkFollowUpsAsDone = false;

            this.isAdhoc = entityPM.QuoteTypeCode == "A" ? true : false;
            this.isInlandDomestic = (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I");

            if (string.IsNullOrEmpty(entityPM.ShipmentTypeId) && entityPM.TransportModeId == "A")
            {
                entityPM.ShipmentTypeId = "Air";
            }

            this.isLCLQuote = false;
            if (entityPM.TransportModeId.ToUpper() == "A")
            {
                this.isLCLQuote = true;
            }

            else if (entityPM.TransportModeId.ToUpper() == "O" && entityPM.ShipmentTypeId.ToUpper() == "LCLD")
            {
                this.isLCLQuote = true;
            }

            else if (entityPM.TransportModeId.ToUpper() == "I" && entityPM.ShipmentTypeId.ToUpper() == "LTL")
            {
                this.isLCLQuote = true;
            }

            this.isFCLQuote = !this.isLCLQuote;

            this.InitializeVATs(entityPM.Tenant, entityPM.QuoteTypeCode);

           

            entityPM.UpdatedByUserId = _Initializer.LoggedContactId;
            entityPM.UpdateDate = todayDateTime;

            if (entityPM.Ratio == null)
            {
                entityPM.Ratio = (entityPM.TransportModeId == "A") ? 6 : 1;
            }

            if (string.IsNullOrEmpty(entityPM.RatingCode))
            {
                entityPM.RatingCode = "N";
            }

            InitializePartners(entityPM);
            InitializeInlandDomestic(entityPM);
            InitializePickupDelivery(entityPM);
            SetCustomerDateFields(entityPM, EntityPOCO);
            ComputeChargesSaleFieldsInSaleCurrency(entityPM);
            ComputeCountryForStatisticsId(entityPM);

            if (!entityPM.IsHybrid)
            {
                InitializeSubject(entityPM);
            }

            InitializeExpirationValues(entityPM);
            InitializeAutomaticallyClose(entityPM, EntityPOCO);
            InitializeQuoteConversionProcess(entityPM);

            ///this.ComputeExpectedProfit();
            this.ComputeProfit();
#warning  using ShipmentsModel /// this.FillDefaultSubType();
        }


        private void ComputeProfit()
        {
            if (entityPM.EstimateProfit != null && entityPM.ExchangeRate != null)
            {
                entityPM.EstimatedProfitInLocal = MethodHelper.Round(entityPM.EstimateProfit * entityPM.ExchangeRate, 2);
                entityPM.EstimatedProfitInProfit = MethodHelper.Round(entityPM.EstimatedProfitInLocal / entityPM.ProfitExchangeRate, 2);
            }
        }
        private void InitializeQuoteConversionProcess(QuoteOPPM entityPM)
        {
            if (entityPM.ConvertToLCL || entityPM.ConvertToFCL)
            {
                DeletePackagesAndCharges(entityPM);

                if (entityPM.ConvertToFCL)
                {
                    this.isLCLQuote = false;
                    entityPM.ShipmentTypeId = "FCLD";
                }
                else
                {
                    this.isLCLQuote = true;
                    entityPM.ShipmentTypeId = "LCLD";
                }
                this.GenerateDefaultCharges();
            }

            if (entityPM.ConvertTransportMode)
            {
                DeletePackagesAndCharges(entityPM);
                this.GenerateDefaultCharges();
            }
        }
        private void DeletePackagesAndCharges(QuoteOPPM entityPM)
        {
            entityPM.NumberOfPackages = null;
            entityPM.PackageType1Id = null;
            entityPM.PackageType1Quantity = null;
            entityPM.PackageType2Id = null;
            entityPM.PackageType2Quantity = null;
            entityPM.PackageType3Id = null;
            entityPM.PackageType3Quantity = null;
            entityPM.PackageType4Id = null;
            entityPM.PackageType4Quantity = null;
            entityPM.PackageType5Id = null;
            entityPM.PackageType5Quantity = null;
            entityPM.TEU = null;
            entityPM.NumberOfContainers = null;
            entityPM.GrossWeight = null;
            entityPM.ChargeableWeight = null;
            entityPM.VolumetricWeight = null;
            entityPM.Volume = null;
            entityPM.EstimateProfit = null;
            entityPM.EstimatedProfitInLocal = null;
            entityPM.EstimatedProfitInProfit = null;
#warning    entityPM.EstimateProfitInSaleCurrency = null;

            foreach (var pm in entityPM.QuotePackages)
            {
                pm.ChangeSetOp = ChangeSetOperation.Delete;
                //this.DeleteQuotePackage(pm);
            }

            foreach (var pm in entityPM.QuoteCharges)
            {
                pm.ChangeSetOp = ChangeSetOperation.Delete;
                pm.QuoteOPChargePriceSteps.ForEach(r => r.ChangeSetOp = ChangeSetOperation.Delete);
                //this.DeleteQuoteChargeUp(pm);
            }

            //??entityPM.QuotePackages.Clear();
            //??entityPM.QuoteCharges.Clear();
        }
        private void InitializeAutomaticallyClose(QuoteOPPM entityPM, QuoteOP entityPoco)
        {
            if (entityPM.IsAutomaticallyClosed)
            {
                if (isNewEntity)
                {
                    if (entityPM.AutomaticallyCloseDate != null)
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant).Date;
                        if (entityPM.AutomaticallyCloseDate.Value.Date <= todayDateTime)
                        {
                            this.CloseEntityAutomatically(entityPM);
                        }
                    }
                }

                else if (!entityPoco.IsAutomaticallyClosed)
                {
                    if (entityPM.AutomaticallyCloseDate != null)
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant).Date;
                        if (entityPM.AutomaticallyCloseDate.Value.Date <= todayDateTime)
                        {
                            this.CloseEntityAutomatically(entityPM);
                        }
                    }
                }
            }
        }
        private void CloseEntityAutomatically(QuoteOPPM entityPM)
        {
            var quoteClosingReasonRepository = new QuoteOPClosingReasonRepository(entityPM.Tenant);
            var quoteClosing = quoteClosingReasonRepository.GetSingleQuoteOPClosingReasonByCode("XQ", entityPM.Tenant);
            entityPM.QuoteClosingReasonId = quoteClosing.Id;
            entityPM.IsClosed = true;
            entityPM.QuoteClosingReasonCode = "XQ";
            entityPM.ActionType = "Decline";
        }
        private void InitializeExpirationValues(QuoteOPPM entityPM)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.ExpirationDate != null && entityPM.ExpirationDays == null)
            {
                entityPM.ExpirationDays = (Convert.ToDateTime(entityPM.ExpirationDate) - todayDateTime.Date).Days;
            }

            if (entityPM.ExpirationDays != null && entityPM.ExpirationDate == null)
            {
                entityPM.ExpirationDate = todayDateTime.AddDays(Convert.ToDouble(entityPM.ExpirationDays));
            }
        }
        private void InitializeSubject(QuoteOPPM entityPM)
        {

            bool isAutomaticUpdate = true;

            if (_Initializer.LoggedTenant.IsQuoteSubjectEdited && entityPM.IsSubjectEdited)
            {
                isAutomaticUpdate = false;
            }

            if (isAutomaticUpdate)
            {
                var iSubjectService = new QuoteOPSubjectService(entityPM);
                entityPM.Subject = iSubjectService.GetSubject(); ;
            }
        }

            private void ComputeCountryForStatisticsId(QuoteOPPM entityPM)
        {
            string fromPortId = entityPM.FromPortId;
            string toPortId = entityPM.ToPortId;

            //Import
            if (entityPM.DirectionId == "I")
            {
                PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, fromPortId, true);
                if (port != null)
                {
                    entityPM.CountryForStatisticsId = port.CountryId;
                }
            }

            //Export
            else if (entityPM.DirectionId == "E")
            {
                bool assigned = false;
                if (entityPM.IncludeDelivery)
                {
                    if (!string.IsNullOrEmpty(entityPM.ToAddressCountryId))
                    {
                        entityPM.CountryForStatisticsId = entityPM.ToAddressCountryId;
                        assigned = true;
                    }
                }

                if (!assigned)
                {
                    PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, toPortId, true);
                    if (port != null)
                    {
                        entityPM.CountryForStatisticsId = port.CountryId;
                    }
                }
            }

            //Domestic
            else if (entityPM.DirectionId == "D")
            {
                if (entityPM.TransportModeId == "I")
                {
                    if (!string.IsNullOrEmpty(entityPM.ToPartnerAddressId))
                    {
                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        Address toAddress = addressRepository.GetSingleAddress(entityPM.ToPartnerAddressId, entityPM.Tenant);
                        if (toAddress != null)
                        {
                            entityPM.CountryForStatisticsId = toAddress.CountryId;
                        }
                    }
                }

                else
                {
                    PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, toPortId, true);
                    if (port != null)
                    {
                        entityPM.CountryForStatisticsId = port.CountryId;
                    }
                }
            }

            //Drop
            else if (entityPM.DirectionId == "R")
            {
                PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, toPortId, true);
                if (port != null)
                {
                    entityPM.CountryForStatisticsId = port.CountryId;
                }
            }
        }
        private void ComputeChargesSaleFieldsInSaleCurrency(QuoteOPPM entityPM)
        {
            var allCharges = new List<QuoteOPChargePM>();

            if (this.isNewEntity)
            {
                allCharges = entityPM.QuoteCharges;
            }

            else
            {
                allCharges = entityPM.DeletedQuoteCharges.Union(entityPM.QuoteCharges.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList()).ToList();
            }

            foreach (var item in allCharges)
            {
                item.SaleUnitPriceInSaleCurrency = MethodHelper.Round(item.SaleUnitPrice * item.SaleExchangeRate / entityPM.ExchangeRate, 3);
                item.SaleUnitPrice1InSaleCurrency = MethodHelper.Round(item.SaleContainerType1UnitPrice * item.SaleExchangeRate / entityPM.ExchangeRate, 3);
                item.SaleUnitPrice2InSaleCurrency = MethodHelper.Round(item.SaleContainerType2UnitPrice * item.SaleExchangeRate / entityPM.ExchangeRate, 3);
                item.SaleUnitPrice3InSaleCurrency = MethodHelper.Round(item.SaleContainerType3UnitPrice * item.SaleExchangeRate / entityPM.ExchangeRate, 3);
                item.SaleUnitPrice4InSaleCurrency = MethodHelper.Round(item.SaleContainerType4UnitPrice * item.SaleExchangeRate / entityPM.ExchangeRate, 3);
                item.SaleUnitPrice5InSaleCurrency = MethodHelper.Round(item.SaleContainerType5UnitPrice * item.SaleExchangeRate / entityPM.ExchangeRate, 3);
                item.SaleAmountInSaleCurrency = MethodHelper.Round(item.SaleTotalAmount * item.SaleExchangeRate / entityPM.ExchangeRate, 2);
            }
        }
        private void SetCustomerDateFields(QuoteOPPM entityPM, QuoteOP entityPOCO)
        {
            int myTenant = entityPM.Tenant;
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                if (!string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    CustomerRepository customerRepository = new CustomerRepository(myTenant);
                    Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, myTenant, false);
                    if (customer != null)
                    {
                        customer.LastQuoteDate = myDate;
                        customer.LastInteractionDate = myDate;
                        customerRepository.Update(customer);
                        customerRepository.SubmitChanges();
                    }
                }
            }

            else
            {
                if (entityPM.CustomerId != entityPOCO.CustomerId)
                {
                    CustomerRepository customerRepository = new CustomerRepository(myTenant);

                    if (!string.IsNullOrEmpty(entityPM.CustomerId))
                    {
                        Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, myTenant, false);
                        if (customer != null)
                        {
                            customer.LastQuoteDate = myDate;
                            customer.LastInteractionDate = myDate;
                            customerRepository.Update(customer);
                            customerRepository.SubmitChanges();
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.CustomerId))
                    {
                        var myQuoteRepository = new QuoteOPRepository(myTenant);
                        var oldCustomerEntities = myQuoteRepository.GetQuotesByCustomerId(entityPOCO.CustomerId, myTenant);

                        if (oldCustomerEntities != null)
                        {
                            if (oldCustomerEntities.Count() > 0)
                            {
                                DateTime? oldestDate = oldCustomerEntities.OrderByDescending(d => d.OpenDate).FirstOrDefault().OpenDate;
                                if (oldestDate != null)
                                {
                                    Customer customer = customerRepository.GetSingleCustomer(entityPOCO.CustomerId, myTenant, false);
                                    if (customer != null)
                                    {
                                        customer.LastQuoteDate = oldestDate;

                                        customer.LastInteractionDate = customerRepository.ComputeLastInteractionDate(customer, oldestDate);

                                        customerRepository.Update(customer);
                                        customerRepository.SubmitChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void InitializePickupDelivery(QuoteOPPM entityPM)
        {
            if (!isInlandDomestic)
            {
                if (entityPM.IncludePickUp)
                {
                    if (!string.IsNullOrEmpty(entityPM.PickUpAddressId))
                    {
                        entityPM.FromAddressCity = null;
                        entityPM.FromAddressZipCode = null;
                        entityPM.FromAddressCountryId = null;
                    }
                }

                else
                {
                    entityPM.FromAddressCity = null;
                    entityPM.FromAddressZipCode = null;
                    entityPM.FromAddressCountryId = null;
                    entityPM.PickUpAddressId = null;
                    entityPM.PickupLocation = null;
                }


                if (entityPM.IncludeDelivery)
                {
                    if (!string.IsNullOrEmpty(entityPM.DeliveryAddressId))
                    {
                        entityPM.ToAddressCity = null;
                        entityPM.ToAddressZipCode = null;
                        entityPM.ToAddressCountryId = null;
                    }
                }

                else
                {
                    entityPM.ToAddressCity = null;
                    entityPM.ToAddressZipCode = null;
                    entityPM.ToAddressCountryId = null;
                    entityPM.DeliveryAddressId = null;
                    entityPM.DeliveryLocation = null;
                }
            }
        }
        private void InitializeInlandDomestic(QuoteOPPM entityPM)
        {
            if (isInlandDomestic)
            {
                AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                entityPM.FromPortId = null;
                entityPM.ToPortId = null;
                entityPM.IncludePickUp = false;
                entityPM.IncludeDelivery = false;

                if (!string.IsNullOrEmpty(entityPM.ShipperId))
                {
                    if (string.IsNullOrEmpty(entityPM.FromPartnerId))
                    {
                        entityPM.FromPartnerId = entityPM.ShipperId;
                    }

                    if (string.IsNullOrEmpty(entityPM.FromPartnerAddressId))
                    {
                        
                        Address adr = addressRepository.GetMainAddressByCardId(entityPM.ShipperId, entityPM.Tenant);
                        if (adr != null)
                        {
                            entityPM.FromPartnerAddressId = adr.Id;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
                {
                    if (string.IsNullOrEmpty(entityPM.ToPartnerId))
                    {
                        entityPM.ToPartnerId = entityPM.ConsigneeId;
                    }

                    if (string.IsNullOrEmpty(entityPM.ToPartnerAddressId))
                    {
                        Address adr = addressRepository.GetMainAddressByCardId(entityPM.ConsigneeId, entityPM.Tenant);
                        if (adr != null)
                        {
                            entityPM.ToPartnerAddressId = adr.Id;
                        }
                    }
                }
            }
        }
        private void InitializePartners(QuoteOPPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.ShipperId))
            {
                entityPM.ShipperName = null;
                entityPM.ShipperNote = null;
                entityPM.ShipperContactId = null;
                entityPM.ShipperReference1 = null;
                entityPM.ShipperReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.ConsigneeId))
            {
                entityPM.ConsigneeName = null;
                entityPM.ConsigneeNote = null;
                entityPM.ConsigneeContactId = null;
                entityPM.ConsigneeReference1 = null;
                entityPM.ConsigneeReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.AgentId))
            {
                entityPM.AgentName = null;
                entityPM.AgentContactId = null;
                entityPM.AgentAddressId = null;
                entityPM.AgentReference1 = null;
                entityPM.AgentReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.NotifyId))
            {
                entityPM.NotifyName = null;
                entityPM.NotifyContactId = null;
                entityPM.NotifyAddressId = null;
                entityPM.NotifyNote = null;
            }

            if (string.IsNullOrEmpty(entityPM.CustomerId))
            {
                entityPM.CustomerName = null;
                entityPM.CustomerContactId = null;
                entityPM.CustomerReference1 = null;
                entityPM.CustomerReference2 = null;
                entityPM.CustomerNote = null;
            }

            if (string.IsNullOrEmpty(entityPM.QuoteCustomerTypeCode))
            {
                if (entityPM.DirectionId == "I")
                {
                    entityPM.QuoteCustomerTypeCode = "CON";
                }

                else
                {
                    entityPM.QuoteCustomerTypeCode = "SHI";
                }
            }

            else
            {
                CardRepository cardRepository = new CardRepository(entityPM.Tenant);
                Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
                if (customer != null)
                {
                    entityPM.CustomerName = customer.EnglishName;
                }
            }

            
        }
        private void InitializeVATs(int tenant, string quoteTypeCode)
        {
            this.allVatTypes = (from d in myCommonContext.VatTypes
                                where d.Tenant == tenant
                                select d).ToList();

            AccountingSetting accountingSetting = (from a in myCommonContext.AccountingSettings
                                                   where a.Id == tenant
                                                   select a).FirstOrDefault();

            if (accountingSetting != null)
            {
                this.isEnableMultiPercentageVATTypes = accountingSetting.EnableMultiPercentageVATTypes;
            }

            if (quoteTypeCode == "A")
            {
                VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
                var myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
                this.allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
            }
        }
        QuoteOP _EntityPoco4Validate = null;
        protected override void Validate(QuoteOPPM entityPM)
        {
            QuoteOPValidating.Validate(entityPM, _EntityPoco4Validate, entityPM.ChangeSetOp == ChangeSetOperation.Insert, myCommonContext);
            base.Validate(entityPM);
        }

        protected override void Trace(QuoteOPPM entityPM, QuoteOP entityPoco, string changesXml)
        //public static void Trace(QuotePM entityPM, Quote entityPoco, string loggedContactId, bool isNewEntity)
        {
            string resolveLoggingUserEmail;
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact contact;
            GetLogUser(entityPM, out resolveLoggingUserEmail, out contact);

            string loggedContactId = contact?.Id;     
            bool isNewEntity = entityPM.ChangeSetOp == ChangeSetOperation.Insert;

            int tenant = entityPM.Tenant;
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode("QuoteOP", entityPM.Tenant);
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            QuoteOPStageRepository myQuoteStageRepository = new QuoteOPStageRepository(tenant);
            IQueryable<QuoteOPStage> myStages = myQuoteStageRepository.GetQuoteOPStages(tenant);

#warning ONPREMISE_No Email Alert !!
            var quoteEmailAlert = new QuoteOPEmailAlert();

            if (isNewEntity)
            {
                if (entityPM.SalesmanUserId != entityPM.UpdatedByUserId)
                {
                    quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "OQTA", true);
                }

                quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "GNQT", true);

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "CRQT",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                });

                if (entityPM.IsCopy)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "CFAQ",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "QuoteOP",
                        Notes = "Copied from Quote number: " + entityPM.BaseShipmentNumber,
                    });
                }

                //if (!string.IsNullOrEmpty(entityPM.OpportunityId))
                //{
                //    EventTracer.CreateTraceEvent(new EventTracerArgs()
                //    {
                //        Tenant = tenant,
                //        EventTypeCode = "QTOP",
                //        UserId = loggedContactId,
                //        EntityId = entityPM.OpportunityId,
                //        ObjectTableName = "Opportunity",
                //        Notes = "Quote: " + entityPM.QuoteNumber + " Added",
                //    });
                //}
            }

            else
            {
                if (entityPM.SalesmanUserId != entityPoco.SalesmanUserId && entityPM.SalesmanUserId != entityPM.UpdatedByUserId)
                {
                    quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "OQTA", false);
                }

                if (!entityPM.MarkFollowUpsAsDone)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "UPQT",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "QuoteOP",
                    });
                }

                //if (!string.IsNullOrEmpty(entityPM.OpportunityId) && string.IsNullOrEmpty(entityPoco.OpportunityId))
                //{
                //    EventTracer.CreateTraceEvent(new EventTracerArgs()
                //    {
                //        Tenant = tenant,
                //        EventTypeCode = "QTOP",
                //        UserId = loggedContactId,
                //        EntityId = entityPM.OpportunityId,
                //        ObjectTableName = "Opportunity",
                //        Notes = "Quote: " + entityPM.QuoteNumber + " Added",
                //    });
                //}

                //if (string.IsNullOrEmpty(entityPM.OpportunityId) && !string.IsNullOrEmpty(entityPoco.OpportunityId))
                //{
                //    EventTracer.CreateTraceEvent(new EventTracerArgs()
                //    {
                //        Tenant = tenant,
                //        EventTypeCode = "QTOP",
                //        UserId = loggedContactId,
                //        EntityId = entityPM.OpportunityId,
                //        ObjectTableName = "Opportunity",
                //        Notes = "Quote: " + entityPM.QuoteNumber + " Deleted",
                //    });
                //}

                if (entityPM.StageDueDate != entityPoco.StageDueDate)
                {
                    string myEventNotes = "";
                    myEventNotes += "Previous stage due date: " + (entityPoco.StageDueDate == null ? "" : entityPoco.StageDueDate.Value.ToShortDateString());
                    myEventNotes += "\n";
                    myEventNotes += "New stage due date: " + (entityPM.StageDueDate == null ? "" : entityPM.StageDueDate.Value.ToShortDateString());
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QUSG",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "QuoteOP",
                        Notes = myEventNotes,
                    });
                }

                if (entityPM.IncotermId != entityPoco.IncotermId)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "ICUP",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "QuoteOP",
                        Notes = "",
                    });
                }
            }



            if (entityPM.ActionType == "SentToCustomerFromQuotation")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "SASC",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                    Notes = entityPM.EventNote,
                });

                entityPM.SentDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                var myCreateStage = myStages.Where(d => d.Code == "QTCR").FirstOrDefault();
                var myDraftStage = myStages.Where(d => d.Code == "QTDR").FirstOrDefault();

                var myStage = myStages.Where(d => d.Code == "QTST").FirstOrDefault();
                if (myStage != null)
                {
                    if (entityPM.StageId == myCreateStage.Id || entityPM.StageId == myDraftStage.Id)
                    {
                        entityPM.StageId = myStage.Id;
                        entityPM.StageName = myStage.Name;
                        entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                        if (myStage.MaxDays != null)
                        {
                            entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                        }
                    }

                }
            }

            if (entityPM.ActionType == "SetAsSentToCustomer")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "SASC",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                    Notes = string.IsNullOrEmpty(entityPM.ExternalEntityNumber) ? entityPM.EventNote : ("Quote sent from Ticket " + entityPM.ExternalEntityNumber),
                });

                entityPM.SentDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                var myStage = myStages.Where(d => d.Code == "QTST").FirstOrDefault();
                if (myStage != null)
                {
                    entityPM.StageId = myStage.Id;
                    entityPM.StageName = myStage.Name;
                    entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                    if (myStage.MaxDays != null)
                    {
                        entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                    }
                }
            }

            if (entityPM.ActionType == "ReturnInProgress")
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "RQTD",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                    Notes = entityPM.EventNote,
                });

                var myStage = myStages.Where(d => d.Code == "QTDR").FirstOrDefault();
                if (myStage != null)
                {
                    entityPM.StageId = myStage.Id;
                    entityPM.StageName = myStage.Name;
                    entityPM.AcceptedDate = null;
                    entityPM.DeclinedDate = null;
                    entityPM.IsClosed = false;
                    entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                    if (myStage.MaxDays != null)
                    {
                        entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                    }
                }

                entityPM.QuoteClosingReasonId = null;
                entityPM.QuoteClosingReasonCode = null;
                entityPM.IsAutomaticallyClosed = false;
                entityPM.AutomaticallyCloseDate = null;
                entityPM.AutomaticallyCloseDays = null;
            }

            if (!entityPoco.IsClosed && entityPM.IsClosed)
            {
                if (entityPM.ActionType == "Accept")
                {
                    string traceEventNotes = entityPM.EventNote;
                    if (!string.IsNullOrEmpty(entityPM.QuoteClosingReasonId))
                    {
                        var closingReasonRepository = new QuoteOPClosingReasonRepository(tenant);
                        var myQuoteClosingReason = closingReasonRepository.GetSingleQuoteOPClosingReason(entityPM.QuoteClosingReasonId, tenant);
                        if (myQuoteClosingReason != null)
                        {
                            traceEventNotes = myQuoteClosingReason.Name;
                        }
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QTCP",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "QuoteOP",
                        Notes = traceEventNotes,
                    });

                    var myStage = myStages.Where(d => d.Code == "QTAC").FirstOrDefault();
                    if (myStage != null)
                    {
                        entityPM.StageId = myStage.Id;
                        entityPM.StageName = myStage.Name;
                        entityPM.AcceptedDate = todayDateTime;
                        entityPM.IsClosed = true;
                        entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                        if (myStage.MaxDays != null)
                        {
                            entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                        }

                        quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "GQTA", false);
                    }
                }

                else if (entityPM.ActionType == "Decline")
                {
                    string traceEventNotes = entityPM.EventNote;
                    if (!string.IsNullOrEmpty(entityPM.QuoteClosingReasonId))
                    {
                        var closingReasonRepository = new QuoteOPClosingReasonRepository(tenant);
                        var myQuoteClosingReason = closingReasonRepository.GetSingleQuoteOPClosingReason(entityPM.QuoteClosingReasonId, tenant);
                        if (myQuoteClosingReason != null)
                        {
                            traceEventNotes = myQuoteClosingReason.Name;
                        }
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = "QTDL",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "QuoteOP",
                        Notes = traceEventNotes,
                    });

                    var myStage = myStages.Where(d => d.Code == "QTDC").FirstOrDefault();
                    if (myStage != null)
                    {
                        entityPM.StageId = myStage.Id;
                        entityPM.StageName = myStage.Name;
                        entityPM.DeclinedDate = todayDate;
                        entityPM.IsClosed = true;
                        entityPM.LastStageDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                        if (myStage.MaxDays != null)
                        {
                            entityPM.StageDueDate = todayDate.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                        }

                        quoteEmailAlert.SendEmailAlert(entityPM, entityPoco, entityPM.Tenant, "GQTD", false);
                    }
                }
            }

            if (entityPoco.IsCancelled && !entityPM.IsCancelled)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "RAQT",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                    Notes = entityPM.EventNote,
                });
            }

            if (!entityPoco.IsCancelled && entityPM.IsCancelled)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "CLQT",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                    Notes = entityPM.EventNote,
                });
            }

            if (entityPM.ConvertToLCL)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "oLCL",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                    Notes = entityPM.EventNote,
                });
            }

            if (entityPM.ConvertToFCL)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "oFCL",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                    Notes = entityPM.EventNote,
                });
            }

            if (entityPM.ConvertTransportMode)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "QCTM",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteOP",
                    Notes = entityPM.EventNote,
                });
            }
        
            base.Trace(entityPM, entityPoco, changesXml);
        }
        
    }
    public class QuoteTotalsClass
    {
        [Key]
        public string Id { get; set; }
        public string VatTypeId { get; set; }
        public double? VatTypePercentage { get; set; }
        public string RowLabel { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? QuoteCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public string VatTypeCell { get; set; }
    }
}

using System.Collections.Generic;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System.Linq;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.CustomFields;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ShippingLineService
    {
        bool isNewEntity;
        private int tenant;
        private ShippingLinePM entityPM;
        public ShippingLine entityPOCO { get; set; }
        private Card entityCard;
        private Contact loggedContact;
        private ICommonDataContext objectContext;
        private ShippingLineRepository entityRepository;
        private CardRepository cardRepository;
        private CardQuery cardQuery;
        private ContactRepository contactRepository;
        private CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository;

        public ShippingLineService(ICommonDataContext objectContext, ShippingLinePM entityPM, string loggedContactId)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ShippingLineRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardQuery = new CardQuery(cardRepository);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
            this.loggedContact = contactRepository.GetSingleContact(loggedContactId, tenant);
        }
        public ShippingLineService(ICommonDataContext objectContext, int tenant)
        {

            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ShippingLineRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardQuery = new CardQuery(cardRepository);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            string email = HttpContext.Current.User.Identity.Name;
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        private List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrencyChangeSet;
        public void SetChangeSet(List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrencyChangeSet)
        {

            this.cardExternalCodeByCurrencyChangeSet = cardExternalCodeByCurrencyChangeSet;
        }

        public void Create(ShippingLinePM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = true;

            this.entityPM.Id = IdCounter.GetNumber("Card", tenant).ToString();

            this.entityCard = new Card()
            {
                Id = entityPM.Id,
                Tenant = tenant,
                PartnerTypeId = "SL",
                UploadingUniqueKey = entityPM.UploadingUniqueKey,
            };
            
            this.entityPOCO = new ShippingLine()
            {
                Id = entityPM.Id,
                Tenant = tenant,
            };

            this.InitializeComponent();

            if (!entityPM.IsHybrid)
            {
                ShippingLineTracing.Trace(entityPM, entityPOCO, isNewEntity);
            }
          
            foreach (CardExternalCodeByCurrencyPM item in entityPM.CardExternalCodeByCurrencies)
            {
                this.CreateCardExternalCodeByCurrency(item);
            }

            ShippingLineMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
            ShippingLineValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

            cardRepository.Add(entityCard);
            entityRepository.Add(entityPOCO);            
            entityRepository.SubmitChanges();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ShippingLine", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<ShippingLinePM> { entityPM }.Cast<object>().ToList() }).Update();

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "ShippingLine");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Carrier");
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {
                RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
            }
            AddCardKafkaQueueMessage();
        }

        public void Update(ShippingLinePM entityPM, bool mapComposition = false)
        {
            this.entityPM = entityPM;
            this.isNewEntity = false;

            this.entityPOCO = entityRepository.GetSingleShippingLine(entityPM.Id, tenant);
            this.entityCard = cardRepository.GetSingleCard(entityPM.Id, tenant);

            this.InitializeComponent();

            if (mapComposition)
            {
                this.SetChangeSet(this.entityPM.CardExternalCodeByCurrencies);
            }

            if (CacheManager.CacheWrapper != null)
            {
                string entityName = "Card" + entityPOCO.Id + entityPOCO.Tenant;
                string entityPmName = "CardPM" + entityPOCO.Id + entityPOCO.Tenant;

                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }
            }


            if (!entityPM.IsHybrid)
            {
                ShippingLineTracing.Trace(entityPM, entityPOCO, isNewEntity);
            }          

            this.UpdateCardExternalCodeByCurrencyCollection();

            ShippingLineMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
            ShippingLineValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

            cardRepository.Update(entityCard);
            entityRepository.Update(entityPOCO);
            entityRepository.SubmitChanges();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ShippingLine", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<ShippingLinePM> { entityPM }.Cast<object>().ToList() }).Update();

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "ShippingLine");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Carrier");
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {
                RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
            }
            AddCardKafkaQueueMessage();
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdateDate = entityPM.CreateDate;
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;

                if (string.IsNullOrEmpty(entityPM.SCACCode))
                {
                    entityPM.SCACCode = entityPM.Code;
                }
            }

            else
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdatedByUserId = loggedContact.Id;

                if (tenant == 0)
                {
                    if (entityPM.IsINTTRARegistered != entityPOCO.IsINTTRARegistered || entityPM.INTTRARegistrationNotes != entityPOCO.INTTRARegistrationNotes)
                    {
                        if (this.entityCard != null)
                        {
                            List<ShippingLine> allShippingLines = entityRepository.GetAllTenantsShippingLinesByCode(this.entityCard.Code).ToList();

                            if (allShippingLines.Count > 0)
                            {
                                foreach (ShippingLine item in allShippingLines)
                                {
                                    if (item.Tenant != 0)
                                    {
                                        item.IsINTTRARegistered = entityPM.IsINTTRARegistered;
                                        item.INTTRARegistrationNotes = entityPM.INTTRARegistrationNotes;
                                        entityRepository.Update(item);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            this.InitializeCardFields();
            this.ComputeContactFields();
        }

        private void InitializeCardFields()
        {
            if (isNewEntity)
            {

            }

            else
            {
                entityPM.CityName = entityCard.CityName;
                entityPM.CountryId = entityCard.CountryId;
                entityPM.CountryCode = entityCard.CountryCode;
                entityPM.CountryName = entityCard.CountryName;
            }
            entityCard.EmailForSendingSingArinvoice = entityPM.Card.EmailForSendingSingArinvoice;
            entityCard.SendingInterestReport = entityPM.Card.SendingInterestReport;
        }

        private void ComputeContactFields()
        {
            if (!string.IsNullOrEmpty(entityPM.PrimaryContactId))
            {
                ContactRepository contactRepository = new ContactRepository(objectContext);
                Contact contact = contactRepository.GetSingleContact(entityPM.PrimaryContactId, entityPM.Tenant);
                if (contact != null)
                {
                    entityPM.PrimaryContactName = contact.EnglishName;
                    entityPM.PrimaryContactEmail = contact.Email;
                    entityPM.PrimaryContactPhone = contact.BusinessPhone;
                }
            }
        }

        private void UpdateCardExternalCodeByCurrencyCollection()
        {
            if (cardExternalCodeByCurrencyChangeSet != null)
            {
                foreach (CardExternalCodeByCurrencyPM itemPM in cardExternalCodeByCurrencyChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void CreateCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CardExternalCodeByCurrency", tenant).ToString();
            itemPM.CardId = this.entityPM.Id;
            itemPM.Tenant = tenant;
            itemPM.CurrencyId = itemPM.CurrencyId;
            itemPM.ExternalRecievableTableId = itemPM.ExternalRecievableTableId;
            itemPM.ExternalPayableTableId = itemPM.ExternalPayableTableId;
            CardExternalCodeByCurrency itemPoco = new CardExternalCodeByCurrency()
            {
                Id = itemPM.Id,
                CardId = itemPM.CardId,
                CurrencyId = itemPM.CurrencyId,
                ExternalRecievableTableId = itemPM.ExternalRecievableTableId,
                ExternalPayableTableId = itemPM.ExternalPayableTableId,
                Tenant = tenant,
            };

            CardExternalCodeByCurrencyMapping.MapEntity(itemPM, itemPoco, true);
            cardExternalCodeByCurrencyRepository.Add(itemPoco);
        }
        private void UpdateCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            CardExternalCodeByCurrency itemPoco = cardExternalCodeByCurrencyRepository.GetSingleCardExternalCodeByCurrency(itemPM.Id, tenant);
            CardExternalCodeByCurrencyMapping.MapEntity(itemPM, itemPoco, false);

            cardExternalCodeByCurrencyRepository.Update(itemPoco);
        }
        private void DeleteCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            CardExternalCodeByCurrency itemPoco = cardExternalCodeByCurrencyRepository.GetSingleCardExternalCodeByCurrency(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                cardExternalCodeByCurrencyRepository.Remove(itemPoco);
            }
        }

        private void AddCardKafkaQueueMessage()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
            {
                return;
            }
            AddKafkaQueueMessage();
        }

        private void AddKafkaQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("CToolLookups", 0);
            var queueMessage = new Dictionary<string, string>() {
                { "Entity", "Card" },
                { "EntityId", entityPM.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }
        public void Submit()
        {
            cardExternalCodeByCurrencyRepository.SubmitChanges();
        }
    }
}

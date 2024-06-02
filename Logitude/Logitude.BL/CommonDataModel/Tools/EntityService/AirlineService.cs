using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.CustomFields;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AirlineService
    {
        bool isNewEntity;
        private int tenant;
        private AirlinePM entityPM;
        public Airline entityPOCO { get; set; }
        private Card entityCard;
        private Contact loggedContact;
        private CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository;
        
        private ICommonDataContext objectContext;
        private AirlineRepository entityRepository;
        private CardRepository cardRepository;
        private CardQuery cardQuery;
        private ContactRepository contactRepository;
        public AirlineService(ICommonDataContext objectContext, AirlinePM entityPM, string loggedContactId)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AirlineRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardQuery = new CardQuery(cardRepository);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
            this.loggedContact = contactRepository.GetSingleContact(loggedContactId, tenant);
        }

        public AirlineService(ICommonDataContext objectContext, int tenant)
        {
            
            this.tenant =tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AirlineRepository(objectContext);
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
        
        public void Create(AirlinePM entityPM)
        {
            this.entityPM = entityPM;
            bool exist = (from a in entityRepository.GetAirlines(entityPM.Tenant)
                          where a.Card.Code == entityPM.Code && a.Tenant == entityPM.Tenant
                          select a).Any();
            if (!exist)
            {
                this.isNewEntity = true;

                entityPM.Id = IdCounter.GetNumber("Card", entityPM.Tenant).ToString();
                entityPM.Tenant = tenant;

                this.entityCard = new Card()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                    PartnerTypeId = "AL",
                    UploadingUniqueKey = entityPM.UploadingUniqueKey,
                };

                this.entityPOCO = new Airline()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                    LimitedLength = true,
                };

                if (tenant == 0)
                {
                    this.entityPM.CheckDigit = true;
                    this.entityPM.LimitedLength = true;
                }

                this.InitializeComponent();


                foreach (CardExternalCodeByCurrencyPM item in entityPM.CardExternalCodeByCurrencies)
                {
                    this.CreateCardExternalCodeByCurrency(item);
                }

                if (!entityPM.IsHybrid)
                {
                    AirlineTracing.Trace(entityPM, entityPOCO, isNewEntity);
                }

                AirlineMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
                AirlineValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

                cardRepository.Add(entityCard);
                entityRepository.Add(entityPOCO);
                entityRepository.SubmitChanges();
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Airline", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<AirlinePM> { entityPM }.Cast<object>().ToList() }).Update();

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Airline");
                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Carrier");
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                if (dbms != "oracle")
                {
                    RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
                }
                AddCardKafkaQueueMessage();
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", tenant);
                msg = msg.Replace("%Entity", "Airline");
                throw new Exception(msg);
            }
        }

        public void Update(AirlinePM entityPM, bool mapComposition = false)
        {
            this.entityPM = entityPM;
            //CardPM c = cardQuery.GetSinglePM(entityPM.Id, entityPM.Tenant);
            CardPM c = cardQuery.GetSingleCarrierCard(entityPM.Id, entityPM.Tenant, false);
            
            bool exist = (from a in entityRepository.GetAirlines(entityPM.Tenant)
                          where a.Card.Code == c.Code
                          && a.Id != entityPM.Id
                          && a.Tenant == entityPM.Tenant
                          select a).Any();

            if (!exist)
            {
                this.isNewEntity = false;

                this.entityPOCO = entityRepository.GetSingleAirline(entityPM.Id, tenant);
                this.entityCard = cardRepository.GetSingleCard(entityPM.Id, entityPM.Tenant);

                this.InitializeComponent();

                if (mapComposition)
                {
                    this.SetChangeSet(this.entityPM.CardExternalCodeByCurrencies);                    
                }

                if (CacheManager.CacheWrapper != null)
                {
                    string entityName = "Card" + entityPM.Id + entityPM.Tenant;
                    string entityPmName = "CardPM" + entityPM.Id + entityPM.Tenant;

                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }

                    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmName);
                    }
                }

                this.UpdateCardExternalCodeByCurrencyCollection();
                this.UpdateHasAdaptations();

                if (!entityPM.IsHybrid)
                {
                    AirlineTracing.Trace(entityPM, entityPOCO, isNewEntity);
                }

                AirlineMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
                AirlineValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

                cardRepository.Update(entityCard);
                entityRepository.Update(entityPOCO);
                entityRepository.SubmitChanges();
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Airline", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<AirlinePM> { entityPM }.Cast<object>().ToList() }).Update();

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Airline");
                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Carrier");
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                if (dbms != "oracle")
                {
                    RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
                }
                AddCardKafkaQueueMessage();
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "Airline");
                throw new Exception(msg);
            }
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdateDate = entityPM.CreateDate;
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            else
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdatedByUserId = loggedContact.Id;
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
            entityCard.EmailForSendingSingArinvoice = entityPM.Card?.EmailForSendingSingArinvoice;
            entityCard.SendingInterestReport = entityPM.Card != null ? entityPM.Card.SendingInterestReport : entityCard.SendingInterestReport; 
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

        private void UpdateHasAdaptations()
        {
            entityPM.HasAdaptations = false;

            if (entityPM.IsManagingProduct)
            {
                entityPM.HasAdaptations = true;
            }

            else
            {
                if (entityPM.IsProductMandatory)
                {
                    entityPM.HasAdaptations = true;
                }

                else
                {
                    if (entityPM.IsDescriptionOfGoodsFromList)
                    {
                        entityPM.HasAdaptations = true;
                    }

                    else
                    {
                        IATACodeRepository iataCodeRepository = new IATACodeRepository(tenant);
                        IQueryable<IATACode> iataCodes = iataCodeRepository.GetIATACodesByAirlineId(entityPM.Id);

                        if (iataCodes.Count() > 0)
                        {
                            entityPM.HasAdaptations = true;
                        }

                        else
                        {
                            AWBSpecialHandlingCodeRepository handlingRepository = new AWBSpecialHandlingCodeRepository(tenant);
                            IQueryable<AWBSpecialHandlingCode> handlingCodes = handlingRepository.GetHandlingCodesByAirlineId(entityPM.Id);

                            if (handlingCodes.Count() > 0)
                            {
                                entityPM.HasAdaptations = true;
                            }

                            else
                            {
                                BookingProductRepository productRepository = new BookingProductRepository(tenant);
                                IQueryable<BookingProduct> products = productRepository.GetBookingProductsByAirlineId(entityPM.Id);

                                if (products.Count() > 0)
                                {
                                    entityPM.HasAdaptations = true;
                                }

                                else
                                {
                                    CommodityRepository commodityRepository = new CommodityRepository(tenant);
                                    IQueryable<Commodity> commodities = commodityRepository.GetCommoditiesByAirlineId(entityPM.Id, tenant);

                                    if (commodities.Count() > 0)
                                    {
                                        entityPM.HasAdaptations = true;
                                    }
                                }
                            }
                        }
                    }
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

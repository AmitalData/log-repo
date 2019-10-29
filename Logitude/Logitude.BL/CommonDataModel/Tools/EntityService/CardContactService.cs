using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CardContactService
    {

        bool isNewEntity;
        private int tenant;
        public CardContact Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CardContactPM entityPm;
        private ICommonDataContext objectContext;
        private CardContactRepository entityRepository;
        private CardContactProductRepository productRepository;
        private CardContactAdditionalServiceRepository cardContactAdditionalServiceRepository;
        public CardContactService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CardContactRepository(objectContext);
            this.productRepository = new CardContactProductRepository(objectContext);
            this.cardContactAdditionalServiceRepository = new CardContactAdditionalServiceRepository(objectContext);
        }

        private List<CardContactProductPM> productsChangeSet;
        private List<CardContactAdditionalServicePM> additionalServicesChangeSet;
        public void SetChangeSet(List<CardContactProductPM> productsChangeSet, List<CardContactAdditionalServicePM> additionalServicesChangeSet = null)
        {
            this.productsChangeSet = productsChangeSet;
            this.additionalServicesChangeSet = additionalServicesChangeSet;
        }

        public void Create(CardContactPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("CardContact", tenant).ToString();
            this.Poco = new CardContact();
            this.Poco.Id = this.entityPm.Id;

            foreach (CardContactProductPM item in entityPM.CardContactProducts)
            {
                this.CreateProduct(item);
            }

            foreach (CardContactAdditionalServicePM item in entityPM.CardContactAdditionalServices)
            {
                this.CreateAdditionalService(item);
            }

            CardContactValidating.Validate(entityPM);
            if (!entityPM.IsHybrid)
            {
                CardContactTracing.Trace(entityPM, Poco, isNewEntity);
            }

            CardContactMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CardContactPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCardContact(entityPM.Id , entityPm.Tenant);

            this.UpdateProductsCollection();
            this.UpdateAdditionalServicesCollection();

            CardContactValidating.Validate(entityPM);
            if (!entityPM.IsHybrid)
            {
                CardContactTracing.Trace(entityPM, Poco, isNewEntity);
            }

            CardContactMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void UpdateProductsCollection()
        {
            if (productsChangeSet != null)
            {
                foreach (CardContactProductPM itemPM in productsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateAdditionalServicesCollection()
        {
            if (additionalServicesChangeSet != null)
            {
                foreach (CardContactAdditionalServicePM itemPM in additionalServicesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateAdditionalService(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateAdditionalService(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteAdditionalService(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void CreateProduct(CardContactProductPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CardContactProduct", tenant).ToString();
            itemPM.CardContactId = this.entityPm.Id;
            itemPM.Tenant = tenant;

            CardContactProduct itemPoco = new CardContactProduct()
            {
                Id = itemPM.Id,
                CardContactId = itemPM.CardContactId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant
            };

            CardContactProductMapping.MapEntity(itemPM, itemPoco, true);
            productRepository.Add(itemPoco);
        }
        private void UpdateProduct(CardContactProductPM itemPM)
        {
            CardContactProduct itemPoco = productRepository.GetSingleCardContactProduct(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                CardContactProductMapping.MapEntity(itemPM, itemPoco, false);
                productRepository.Update(itemPoco);
            }
        }
        private void DeleteProduct(CardContactProductPM itemPM)
        {
            CardContactProduct itemPoco = productRepository.GetSingleCardContactProduct(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                productRepository.Remove(itemPoco);
            }
        }

        private void CreateAdditionalService(CardContactAdditionalServicePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CardContactAdditionalService", tenant).ToString();
            itemPM.CardContactId = this.entityPm.Id;
            itemPM.Tenant = tenant;

            CardContactAdditionalService itemPoco = new CardContactAdditionalService()
            {
                Id = itemPM.Id,
                CardContactId = itemPM.CardContactId,
                AdditionalServiceId = itemPM.AdditionalServiceId,
                Tenant = tenant
            };

            CardContactAdditionalServiceMapping.MapEntity(itemPM, itemPoco, true);
            cardContactAdditionalServiceRepository.Add(itemPoco);
        }
        private void UpdateAdditionalService(CardContactAdditionalServicePM itemPM)
        {
            CardContactAdditionalService itemPoco = cardContactAdditionalServiceRepository.GetSingleCardContactAdditionalService(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                CardContactAdditionalServiceMapping.MapEntity(itemPM, itemPoco, false);
                cardContactAdditionalServiceRepository.Update(itemPoco);
            }
        }
        private void DeleteAdditionalService(CardContactAdditionalServicePM itemPM)
        {
            CardContactAdditionalService itemPoco = cardContactAdditionalServiceRepository.GetSingleCardContactAdditionalService(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                cardContactAdditionalServiceRepository.Remove(itemPoco);
            }
        }
    }
}

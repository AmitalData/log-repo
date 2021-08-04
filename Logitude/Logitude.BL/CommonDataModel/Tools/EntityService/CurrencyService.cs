using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CurrencyService
    {
        bool isNewEntity;
        private int tenant;
        public Currency Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CurrencyPM entityPm;
        private ICommonDataContext objectContext;
        private CurrencyRepository entityRepository;
        public CurrencyService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CurrencyRepository(objectContext);
        }

        public void Create(CurrencyPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("Currency", tenant).ToString();
            this.Poco = new Currency();
            this.Poco.Id = entityPM.Id;

            CurrencyValidating.Validate(entityPM);
            if (!entityPM.IsHybrid)
            {
                CurrencyTracing.Trace(entityPM, Poco, isNewEntity);
            }

            CurrencyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
            AddPortKafkaQueueMessage();
        }


        public void Update(CurrencyPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCurrency(entityPM.Id, entityPm.Tenant);

            CurrencyValidating.Validate(entityPM);
            if (!entityPM.IsHybrid)
            {
                CurrencyTracing.Trace(entityPM, Poco, isNewEntity);
            }

            CurrencyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            CurrencyValidating.Validate(entityPM);
            AddPortKafkaQueueMessage();
        }

        public string CopyCurrencyToTenant(string currencyId, int tenant)
        {
            entityRepository = new CurrencyRepository(objectContext);
            Currency zeroCurrency = entityRepository.GetSingleCurrency(currencyId, 0);
            CurrencyPM tenantCurrency = new CurrencyPM()
            {
                Code = zeroCurrency.Code,
                AccountingExternalCode = zeroCurrency.AccountingExternalCode,
                AddedManually = zeroCurrency.AddedManually,
                EnglishName = zeroCurrency.EnglishName,
                InActive = zeroCurrency.InActive,
                LocalName = zeroCurrency.LocalName,
                Notes = zeroCurrency.Notes,
                SearchFields = zeroCurrency.SearchFields,
                Tenant = tenant,
                Sign = zeroCurrency.Sign,
            };
            this.Create(tenantCurrency);
            return tenantCurrency.Id;
        }
        private void AddPortKafkaQueueMessage()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("CTL", entityPm.Tenant))
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
                { "Entity", "Currency" },
                { "EntityId", entityPm.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }
    }
}

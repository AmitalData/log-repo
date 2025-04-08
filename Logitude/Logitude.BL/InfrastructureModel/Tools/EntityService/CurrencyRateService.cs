using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class CurrencyRateService
    {
        bool isNewEntity;
        private int tenant;
        public CurrencyRate Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CurrencyRatePM entityPM;
        private IWebFreightContext objectContext;
        private CurrencyRateRepository entityRepository;
        public CurrencyRateService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CurrencyRateRepository(objectContext);
        }

        public void Create(List<CurrencyRatePM> currencyRates, RatesTablePM ratesTablePM)
        {
            foreach (var currencyRate in currencyRates)
            {
                currencyRate.ExchangeRateId = ratesTablePM.Id;
                Create(currencyRate);
            }
            entityRepository.SubmitChanges();
        }

        public void Create(CurrencyRatePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("RatesTable", tenant).ToString();
            this.Poco = new CurrencyRate();
            this.Poco.Id = this.entityPM.Id;

            CurrencyRateValidating.Validate(theEntityPm);
            CurrencyRateTracing.Trace(theEntityPm, Poco, isNewEntity);
            CurrencyRateMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
        }

        public void Update(CurrencyRatePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingle(theEntityPm.Id, theEntityPm.Tenant);

            CurrencyRateValidating.Validate(theEntityPm);
            CurrencyRateTracing.Trace(theEntityPm, Poco, isNewEntity);
            CurrencyRateMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}

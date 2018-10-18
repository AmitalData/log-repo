using Logitude.Server.Tools.Counters;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.DataMapping;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteTemplateExcludedSectionService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteTemplateExcludedSection Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateExcludedSectionPM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateExcludedSectionRepository entityRepository;

        public QuoteTemplateExcludedSectionService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateExcludedSectionRepository(objectContext);
        }

        public void Create(QuoteTemplateExcludedSectionPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateExcludedSection", tenant).ToString();
            this.Poco = new QuoteTemplateExcludedSection();
            this.Poco.Id = this.entityPm.Id;

            //DepartmentValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateExcludedSectiontTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateExcludedSectionMapping.MappingQuoteTemplateExcludedSection(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(QuoteTemplateExcludedSectionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplateExcludedSection(entityPM.Id, entityPm.Tenant);

            string entityName = "QuoteTemplateExcludedSection" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "QuoteTemplateExcludedSectionPM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            //QuoteTemplateExcludedSectionValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateExcludedSectionTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateExcludedSectionMapping.MappingQuoteTemplateExcludedSection(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
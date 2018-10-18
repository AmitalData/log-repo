using System.Collections.Generic;
using System.Linq;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteTemplateService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteTemplate Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplatePM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateRepository entityRepository;

        public QuoteTemplateService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateRepository(objectContext);
        }

        public void Create(QuoteTemplatePM entityPM)
        {
            List<QuoteTemplate> quoteTemplate;
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplate", tenant).ToString();
            this.Poco = new QuoteTemplate();
            this.Poco.Id = this.entityPm.Id;

            this.entityPm.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);


            QuoteTemplateMapping.MappingQuoteTemplate(entityPM, Poco, isNewEntity);

            if (entityPM.IsDefault == true)
            {
                quoteTemplate = entityRepository.GetQuoteTemplateByTenant(entityPM.Tenant, null).ToList();
                quoteTemplate = quoteTemplate.Where(d => d.TemplateTypeCode == entityPM.TemplateTypeCode).ToList();
                foreach (QuoteTemplate quoteTemplatee in quoteTemplate)
                {
                    if (quoteTemplatee.Id != entityPM.Id)
                    {
                        quoteTemplatee.IsDefault = false;
                    }
                }
            }
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteTemplatePM entityPM)
        {
            List<QuoteTemplate> quoteTemplate;
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplate(entityPM.Id, entityPm.Tenant);

            this.entityPm.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            this.entityPm.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            string entityName = "QuoteTemplate" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "QuoteTemplatePM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateMapping.MappingQuoteTemplate(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);

            if (entityPM.IsDefault == true)
            {
                quoteTemplate = entityRepository.GetQuoteTemplateByTenant(entityPM.Tenant, null).ToList();
                quoteTemplate = quoteTemplate.Where(d => d.TemplateTypeCode == entityPM.TemplateTypeCode).ToList();

                QuoteTemplate template = quoteTemplate.Where(d => d.IsDefault == true).FirstOrDefault();

                if (template != null)
                {
                    foreach (QuoteTemplate quoteTemplatee in quoteTemplate)
                    {
                        if (quoteTemplatee.Id != entityPM.Id)
                        {
                            quoteTemplatee.IsDefault = false;
                        }
                    }
                }
            }
            entityRepository.SubmitChanges();
        }
    }
}

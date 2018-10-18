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
    public class QuoteTemplateHeaderFieldService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteTemplateHeaderField Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateHeaderFieldPM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateHeaderFieldRepository entityRepository;

        public QuoteTemplateHeaderFieldService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateHeaderFieldRepository(objectContext);
        }

        public void Create(QuoteTemplateHeaderFieldPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateHeaderField", tenant).ToString();
            this.Poco = new QuoteTemplateHeaderField();
            this.Poco.Id = this.entityPm.Id;

            //DepartmentValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateHeaderFieldtTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateHeaderFieldMapping.MappingQuoteTemplateHeaderField(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(QuoteTemplateHeaderFieldPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplateHeaderField(entityPM.Id, entityPm.Tenant);

            string entityName = "QuoteTemplateHeaderField" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "QuoteTemplateHeaderFieldPM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            //QuoteTemplateHeaderFieldValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateHeaderFieldTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateHeaderFieldMapping.MappingQuoteTemplateHeaderField(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
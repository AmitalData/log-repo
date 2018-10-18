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
    public class QuoteTemplateDetailsFieldService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteTemplateDetailsField Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateDetailsFieldPM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateDetailsFieldRepository entityRepository;

        public QuoteTemplateDetailsFieldService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateDetailsFieldRepository(objectContext);
        }

        public void Create(QuoteTemplateDetailsFieldPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateDetailsField", tenant).ToString();
            this.Poco = new QuoteTemplateDetailsField();
            this.Poco.Id = this.entityPm.Id;

            //DepartmentValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateDetailsFieldtTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateDetailsFieldMapping.MappingQuoteTemplateDetailsField(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(QuoteTemplateDetailsFieldPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplateDetailsField(entityPM.Id, entityPm.Tenant);

            string entityName = "QuoteTemplateDetailsField" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "QuoteTemplateDetailsFieldPM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            //QuoteTemplateDetailsFieldValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateDetailsFieldTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateDetailsFieldMapping.MappingQuoteTemplateDetailsField(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
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
    public class QuoteTemplateSectionTypeService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteTemplateSectionType Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateSectionTypePM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateSectionTypeRepository entityRepository;

        public QuoteTemplateSectionTypeService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateSectionTypeRepository(objectContext);
        }

        public void Create(QuoteTemplateSectionTypePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            //this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateSectionType", tenant).ToString();
            this.Poco = new QuoteTemplateSectionType();
            this.Poco.Code = this.entityPm.Code;

            //DepartmentValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateSectionTypetTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateSectionTypeMapping.MappingQuoteTemplateSectionType(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(QuoteTemplateSectionTypePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplateSectionType(entityPM.Code , false);

            string entityName = "QuoteTemplateSectionType" + entityPM.Code ;
            string entityPmName = "QuoteTemplateSectionTypePM" + entityPM.Code ;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateSectionTypeMapping.MappingQuoteTemplateSectionType(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}

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
    public class QuoteTemplateTextCodeService
    {

        
        bool isNewEntity;
        private int tenant;
        public QuoteTemplateTextCode Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateTextCodePM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateTextCodeRepository entityRepository;

        public QuoteTemplateTextCodeService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateTextCodeRepository(objectContext);
        }



        public void Create(QuoteTemplateTextCodePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateTextCode", tenant).ToString();
            this.Poco = new QuoteTemplateTextCode();
            this.Poco.Id = this.entityPm.Id;
            QuoteTemplateTextCodeMapping.MappingQuoteTemplateTextCode(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }



        public void Update(QuoteTemplateTextCodePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplateTextCode(entityPM.Id, entityPm.Tenant, false);

            string entityName = "QuoteTemplateTextCode" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "QuoteTemplateTextCodePM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            //QuoteTemplateSettingValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateSettingTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateTextCodeMapping.MappingQuoteTemplateTextCode(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}
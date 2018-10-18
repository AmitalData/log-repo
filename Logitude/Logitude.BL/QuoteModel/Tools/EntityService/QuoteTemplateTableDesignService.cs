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
    public class QuoteTemplateTableDesignService
    {

        bool isNewEntity;
        private int tenant;
        public QuoteTemplateTableDesign Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateTableDesignPM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateTableDesignRepository entityRepository;

        public QuoteTemplateTableDesignService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateTableDesignRepository(objectContext);
        }

        public void Create(QuoteTemplateTableDesignPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateTableDesign", tenant).ToString();
            this.Poco = new QuoteTemplateTableDesign();
            this.Poco.Id = this.entityPm.Id;



            QuoteTemplateTableDesignMapping.MappingQuoteTemplateTableDesign(entityPM, Poco, isNewEntity);


            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }
     
        public void Update(QuoteTemplateTableDesignPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplateTableDesign(entityPM.Id, entityPm.Tenant , false);

            string entityName = "QuoteTemplateTableDesign" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "QuoteTemplateTableDesignPM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            //QuoteTemplateTableDesignSettingValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateTableDesignSettingTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateTableDesignMapping.MappingQuoteTemplateTableDesign(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}


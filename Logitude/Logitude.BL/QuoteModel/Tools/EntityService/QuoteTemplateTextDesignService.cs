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
    public class QuoteTemplateTextDesignService
    {



        bool isNewEntity;
        private int tenant;
        public QuoteTemplateTextDesign Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateTextDesignPM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateTextDesignRepository entityRepository;

        public QuoteTemplateTextDesignService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateTextDesignRepository(objectContext);
        }

        public void Create(QuoteTemplateTextDesignPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateTextDesign", tenant).ToString();
            this.Poco = new QuoteTemplateTextDesign();
            this.Poco.Id = this.entityPm.Id;



            QuoteTemplateTextDesignMapping.MappingQuoteTemplateTextDesign(entityPM, Poco, isNewEntity);


            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }
     
        public void Update(QuoteTemplateTextDesignPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplateTextDesign(entityPM.Id, entityPm.Tenant , false);

            string entityName = "QuoteTemplateTextDesign" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "QuoteTemplateTextDesignPM" + entityPM.Id + entityPM.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            //QuoteTemplateTextDesignSettingValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateTextDesignSettingTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateTextDesignMapping.MappingQuoteTemplateTextDesign(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}



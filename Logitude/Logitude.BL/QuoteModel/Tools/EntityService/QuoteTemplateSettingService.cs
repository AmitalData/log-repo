using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.Server.Tools;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteTemplateSettingService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteTemplateSetting Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateSettingPM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateSettingRepository entityRepository;

        public QuoteTemplateSettingService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateSettingRepository(objectContext);
        }

        public void Create(QuoteTemplateSettingPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateSetting", tenant).ToString();
            this.entityPm.XMLData = GetXMLDataFromSettingPM(entityPM);
            this.Poco = new QuoteTemplateSetting();
            this.Poco.Id = this.entityPm.Id;

            //DepartmentValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateSettingtTracing.Trace(entityPM, Poco, isNewEntity);
            //}
            QuoteTemplateSettingMapping.MappingQuoteTemplateSetting(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }
     
        private string GetXMLDataFromSettingPM(QuoteTemplateSettingPM entityPM)
        {
            QuoteTemplateSettingDataBuilder quoteTemplateSettingDataBuilder = new QuoteTemplateSettingDataBuilder(entityPM);
            return quoteTemplateSettingDataBuilder.SerializeNewQuoteTemplateSettingDataToXmlString();
        }
        public void Update(QuoteTemplateSettingPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.entityPm.XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(entityPM.QuoteTemplateSettingData);
            this.Poco = entityRepository.GetSingleQuoteTemplateSetting(entityPM.Id, entityPm.Tenant);

            string entityName = "QuoteTemplateSetting" + entityPM.Id + entityPM.Tenant;
            string entityPmName = "QuoteTemplateSettingPM" + entityPM.Id + entityPM.Tenant;
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
            QuoteTemplateSettingMapping.MappingQuoteTemplateSetting(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
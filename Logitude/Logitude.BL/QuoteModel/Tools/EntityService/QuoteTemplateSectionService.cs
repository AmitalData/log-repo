using Logitude.BL.QuoteModel.EntityOtherServices;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteTemplateSectionService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteTemplateSection Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteTemplateSectionPM entityPm;
        private IQuotesContext objectContext;
        private QuoteTemplateSectionRepository entityRepository;

        public QuoteTemplateSectionService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteTemplateSectionRepository(objectContext);
        }

        public void Create(QuoteTemplateSectionPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("QuoteTemplateSection", tenant).ToString();
            this.Poco = new QuoteTemplateSection();
            this.Poco.Id = this.entityPm.Id;

            QuoteTemplateEntityService quoteTemplateService = new QuoteTemplateEntityService();

            if (entityPM.QuoteTemplateSectionTypeCode != "PH" && entityPM.QuoteTemplateSectionTypeCode != "PF" && entityPM.QuoteTemplateSectionTypeCode != "PC" && entityPM.QuoteTemplateSectionTypeCode != "PP")
            {
                if (entityPM.Templatedata != null)
                {
                    entityPm.SectionDocId = quoteTemplateService.UploadQuoteTemplateSectionDataFile(entityPM.Templatedata, null, entityPM.Tenant);
                }
            }

            QuoteTemplateSectionMapping.MappingQuoteTemplateSection(entityPM, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(QuoteTemplateSectionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteTemplateSection(entityPM.Id, entityPm.Tenant, false);

            if (entityPM.IschangeBodySection == true)
            {
                if (entityPM.Templatedata != null)
                {
                    QuoteTemplateEntityService quoteTemplateService = new QuoteTemplateEntityService();
                    if (!string.IsNullOrEmpty(entityPM.QuoteId))
                    {
                        QuoteTemplateSectionModification modification = entityRepository.GetQuoteTemplateSectionModification(entityPM.Id, entityPM.QuoteId, entityPM.Tenant);
                        if (modification == null)
                        {
                            string docId = quoteTemplateService.UploadQuoteTemplateSectionDataFile(entityPM.Templatedata, null, entityPM.Tenant);
                            modification = new QuoteTemplateSectionModification()
                            {
                                Id = IdCounter.GetNumber("QuoteTemplateSectionModification", tenant).ToString(),
                                QuoteId = entityPM.QuoteId,
                                QuoteTemplateSectionId = entityPM.Id,
                                Tenant = entityPM.Tenant,
                                SectionDocId = docId,
                            };

                            entityRepository.quotesContext.QuoteTemplateSectionModifications.Add(modification);
                        }
                        else
                        {
                            //quoteTemplateService.UploadQuoteTemplateSectionDataFile(entityPM.Templatedata, modification.SectionDocId, entityPM.Tenant);
                            string docId = quoteTemplateService.UploadQuoteTemplateSectionDataFile(entityPM.Templatedata, null , entityPM.Tenant);
                            modification.SectionDocId = docId; // fix for old reports showing the modified sections for all quotes!
                        }

                       
                    }
                    else
                    {
                        quoteTemplateService.UploadQuoteTemplateSectionDataFile(entityPM.Templatedata, entityPM.SectionDocId, entityPM.Tenant);
                    }
                }
            }

            QuoteTemplateSectionMapping.MappingQuoteTemplateSection(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            entityPM.Templatedata = null;
        }
    }
}

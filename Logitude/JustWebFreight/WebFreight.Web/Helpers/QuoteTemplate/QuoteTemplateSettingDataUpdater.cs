using Logitude.BL.QuoteModel.DataContracts;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.Helpers.QuoteTemplate
{
    public class QuoteTemplateSettingDataUpdater
    {
        public void UpdateAllQuoteTempolateSettings()
        {
            IQuotesContext quotesContext = QuotesContext.GetContext(0);
            QuoteTemplateSettingRepository quoteTemplateSettingRepository = new QuoteTemplateSettingRepository(quotesContext);
            List<QuoteTemplateSetting> allQuoteTemplateSettings = quoteTemplateSettingRepository.All();
            const int numberOfSubmittedEntitiesInEachLoop = 50;

            SplitListIntoNList(allQuoteTemplateSettings, numberOfSubmittedEntitiesInEachLoop).ToList().ForEach(entityList =>
            {
                UpdateNumberOfEntites(entityList, quoteTemplateSettingRepository);
                quoteTemplateSettingRepository.SubmitChanges();
            });
        }

        private IEnumerable<List<T>> SplitListIntoNList<T>(List<T> fullList, int nSize)
        {
            for (int i = 0; i < fullList.Count; i += nSize)
            {
                yield return fullList.GetRange(i, Math.Min(nSize, fullList.Count - i)).ToList();
            }
        }

        private void UpdateNumberOfEntites(List<QuoteTemplateSetting> entityList, QuoteTemplateSettingRepository quoteTemplateSettingRepository)
        {
            entityList.ForEach(quoteTemplateSetting =>
            {
                UpdateQuoteTemplateSetting(quoteTemplateSettingRepository, quoteTemplateSetting);
            });
        }

        private void UpdateQuoteTemplateSetting(QuoteTemplateSettingRepository quoteTemplateSettingRepository, QuoteTemplateSetting quoteTemplateSetting)
        {
            QuoteTemplateSettingDataBuilder quoteTemplateSettingDataBuilder = new QuoteTemplateSettingDataBuilder(quoteTemplateSetting);
            if(string.IsNullOrEmpty(quoteTemplateSetting.XMLData))
                quoteTemplateSetting.XMLData = quoteTemplateSettingDataBuilder.SerializeNewQuoteTemplateSettingDataToXmlString();
            quoteTemplateSettingRepository.Update(quoteTemplateSetting);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddTranslationHeaders
    {
        public static void AddTranslationHeader(TranslationHeaderDetails translationHeaderDetails, TranslationHeaderRepository translationHeaderRepository, Dictionary<string, TranslationHeader> tenantTranslationHeaders)
        {
            if (tenantTranslationHeaders.Keys.Contains(translationHeaderDetails.Code))
            {
                TranslationHeader translationHeader = tenantTranslationHeaders[translationHeaderDetails.Code];
                translationHeader.Description = translationHeaderDetails.Description;

            }
            else
            {
                TranslationHeader newTranslationHeader = new TranslationHeader()
                {
                    Description = translationHeaderDetails.Description,
                    Code = translationHeaderDetails.Code,//IdCounter.GetNumber("TranslationHeader").ToString(),
                    

                };
                translationHeaderRepository.Add(newTranslationHeader);
            }
        }
    }
}
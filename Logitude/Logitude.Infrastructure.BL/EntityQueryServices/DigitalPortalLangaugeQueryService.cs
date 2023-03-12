using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalPortalLangaugeQueryService
    {
        public List<DigitalPortalLanguageList> GetDigitalPortalLanguagesQuery(string langCode = "")
        {
            var digitalPortalLanguageRepository = new DigitalPortalLanguageRepository(0);
            var digitalPortalLanguages = digitalPortalLanguageRepository.GetDigitalPortalLanguages(langCode)
                                                                        .Select(x => new DigitalPortalLanguageList
                                                                        {
                                                                            Code = x.Code,
                                                                            Name = x.Name,
                                                                            DisplayText = x.DisplayText,
                                                                            SearchFields = x.SearchFields
                                                                        })
                                                                        .ToList();
            return digitalPortalLanguages;
        }
    }
}
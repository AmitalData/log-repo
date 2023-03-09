using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
    public partial class DigitalPortalLanguageRepository:IRepository<DigitalPortalLanguage>
    {

        public IQueryable<DigitalPortalLanguage> GetDigitalPortalLanguages(string lang)
        {
            return context.DigitalPortalLanguages
                          .Where(a => string.IsNullOrEmpty(lang) || a.Code.Equals(lang));
        }

        public List<DigitalPortalLanguage> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
    }
} 
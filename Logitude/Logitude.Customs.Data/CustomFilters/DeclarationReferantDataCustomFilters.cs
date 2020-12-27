using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.CustomFilters
{
    class DeclarationReferantDataCustomFilters
    {
        public IQueryable<DeclarationReferantData> GetFilteredQuery(IQueryable<DeclarationReferantData> queryableData)
        {
            queryableData = queryableData.Where(d => (d.DeclarationId == null));
            return queryableData; 
        }
    }
}

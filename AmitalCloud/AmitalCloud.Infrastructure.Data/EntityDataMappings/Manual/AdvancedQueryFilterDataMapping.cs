using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.EntityDataMappings
{

    public class AdvancedQueryFilterDataMapping : IMapping<AdvancedQueryFilterPM, AdvancedQueryFilter, AdvancedQueryFilterList>, IMappingEncodeBase64NVARCHARFields<AdvancedQueryFilterPM>
    {
        public void CustomPMToPOCO(AdvancedQueryFilterPM entityPM, AdvancedQueryFilter entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void CustomPOCOToPM(AdvancedQueryFilterPM entityPM, AdvancedQueryFilter entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void EncodeBase64NVARCHARFields(AdvancedQueryFilterPM entityPM)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AdvancedQueryFilterList> GetIqueryableList(IQueryable<AdvancedQueryFilter> iQueryable)
        {
            throw new NotImplementedException();
        }

        public void PMToOldPM(AdvancedQueryFilterPM entityPM, AdvancedQueryFilterPM oldEntityPM)
        {
            throw new NotImplementedException();
        }

        public void PMToPOCO(AdvancedQueryFilterPM entityPM, AdvancedQueryFilter entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void POCOToList(AdvancedQueryFilter entityPOCO, AdvancedQueryFilterList entityList)
        {
            throw new NotImplementedException();
        }

        public void POCOToPM(AdvancedQueryFilterPM entityPM, AdvancedQueryFilter entityPOCO)
        {
            throw new NotImplementedException();
        }
    }

}

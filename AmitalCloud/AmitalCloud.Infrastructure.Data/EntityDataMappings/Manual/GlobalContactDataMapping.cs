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

    public class GlobalContactDataMapping : IMapping<GlobalContactPM, GlobalContact, GlobalContactList>, IMappingEncodeBase64NVARCHARFields<GlobalContactPM>
    {
        public void CustomPMToPOCO(GlobalContactPM entityPM, GlobalContact entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GlobalContactPM entityPM, GlobalContact entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void EncodeBase64NVARCHARFields(GlobalContactPM entityPM)
        {
            throw new NotImplementedException();
        }

        public IQueryable<GlobalContactList> GetIqueryableList(IQueryable<GlobalContact> iQueryable)
        {
            throw new NotImplementedException();
        }

        public void PMToOldPM(GlobalContactPM entityPM, GlobalContactPM oldEntityPM)
        {
            throw new NotImplementedException();
        }

        public void PMToPOCO(GlobalContactPM entityPM, GlobalContact entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void POCOToList(GlobalContact entityPOCO, GlobalContactList entityList)
        {
            throw new NotImplementedException();
        }

        public void POCOToPM(GlobalContactPM entityPM, GlobalContact entityPOCO)
        {
            throw new NotImplementedException();
        }
    }

}

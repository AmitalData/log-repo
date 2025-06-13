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
    public class ScreenFieldDataMapping : IMapping<ScreenFieldPM, ScreenField, ScreenFieldList>, IMappingEncodeBase64NVARCHARFields<ScreenFieldPM>
    {
        public void CustomPMToPOCO(ScreenFieldPM entityPM, ScreenField entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ScreenFieldPM entityPM, ScreenField entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void EncodeBase64NVARCHARFields(ScreenFieldPM entityPM)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ScreenFieldList> GetIqueryableList(IQueryable<ScreenField> iQueryable)
        {
            throw new NotImplementedException();
        }

        public void PMToOldPM(ScreenFieldPM entityPM, ScreenFieldPM oldEntityPM)
        {
            throw new NotImplementedException();
        }

        public void PMToPOCO(ScreenFieldPM entityPM, ScreenField entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void POCOToList(ScreenField entityPOCO, ScreenFieldList entityList)
        {
            throw new NotImplementedException();
        }

        public void POCOToPM(ScreenFieldPM entityPM, ScreenField entityPOCO)
        {
            throw new NotImplementedException();
        }
    }

}

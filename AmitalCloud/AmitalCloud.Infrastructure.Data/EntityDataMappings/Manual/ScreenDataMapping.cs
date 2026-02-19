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
    public class ScreenDataMapping : IMapping<ScreenPM, Screen, ScreenList>, IMappingEncodeBase64NVARCHARFields<ScreenPM>
    {
        public void CustomPMToPOCO(ScreenPM entityPM, Screen entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ScreenPM entityPM, Screen entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void EncodeBase64NVARCHARFields(ScreenPM entityPM)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ScreenList> GetIqueryableList(IQueryable<Screen> iQueryable)
        {
            throw new NotImplementedException();
        }

        public void PMToOldPM(ScreenPM entityPM, ScreenPM oldEntityPM)
        {
            throw new NotImplementedException();
        }

        public void PMToPOCO(ScreenPM entityPM, Screen entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void POCOToList(Screen entityPOCO, ScreenList entityList)
        {
            throw new NotImplementedException();
        }

        public void POCOToPM(ScreenPM entityPM, Screen entityPOCO)
        {
            throw new NotImplementedException();
        }
    }

}

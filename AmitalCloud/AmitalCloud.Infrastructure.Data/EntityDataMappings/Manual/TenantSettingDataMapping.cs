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
    public class TenantSettingDataMapping : IMapping<TenantSettingPM, TenantSetting, TenantSettingList>, IMappingEncodeBase64NVARCHARFields<TenantSettingPM>
    {
        public void CustomPMToPOCO(TenantSettingPM entityPM, TenantSetting entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void CustomPOCOToPM(TenantSettingPM entityPM, TenantSetting entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void EncodeBase64NVARCHARFields(TenantSettingPM entityPM)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TenantSettingList> GetIqueryableList(IQueryable<TenantSetting> iQueryable)
        {
            throw new NotImplementedException();
        }

        public void PMToOldPM(TenantSettingPM entityPM, TenantSettingPM oldEntityPM)
        {
            throw new NotImplementedException();
        }

        public void PMToPOCO(TenantSettingPM entityPM, TenantSetting entityPOCO)
        {
            throw new NotImplementedException();
        }

        public void POCOToList(TenantSetting entityPOCO, TenantSettingList entityList)
        {
            throw new NotImplementedException();
        }

        public void POCOToPM(TenantSettingPM entityPM, TenantSetting entityPOCO)
        {
            throw new NotImplementedException();
        }
    }
}

using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class INTTRASettingQuery
    {
        INTTRASettingRepository repository;       
        public INTTRASettingQuery(int tenant)
        {
            repository = new INTTRASettingRepository(tenant);
        }
        public INTTRASettingQuery(INTTRASettingRepository myRepository)
        {
            repository = myRepository;
        }

        public INTTRASettingPM GetSinglePM(int tenant)
        {
            INTTRASettingPM result = null;
            INTTRASetting entityPOCO = repository.GetSingleByTenant(tenant);
            if (entityPOCO != null)
            {
                result = new INTTRASettingPM()
                {
                    Id = entityPOCO.Id,
                    Tenant = entityPOCO.Tenant,
                    INTTRASettingModeCode = entityPOCO.INTTRASettingModeCode,
                    InSettingsId = entityPOCO.InSettingsId,
                    OutSettingsId = entityPOCO.OutSettingsId,
                    INTTRAId = entityPOCO.INTTRAId,
                    INTTRAAlias = entityPOCO.INTTRAAlias,
                    InSettingsHost = entityPOCO.InFTPDetail == null ? null : entityPOCO.InFTPDetail.Host,
                    OutSettingsHost = entityPOCO.OutFTPDetail == null ? null : entityPOCO.OutFTPDetail.Host,
                };
            }

            return result;
        }
    }
}

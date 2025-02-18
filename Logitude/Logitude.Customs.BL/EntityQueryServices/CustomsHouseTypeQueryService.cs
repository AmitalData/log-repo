using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.Contracts;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsHouseTypeQueryService : EntityQueryService<CustomsHouseType, CustomsHouseTypeKeys, CustomsHouseTypePM, object, CustomsHouseTypeKeys>
        , ICanGetAllClosedTable<CustomsHouseTypePM>, 
        ICanGetAllWithAdditionalClosedTable<CustomsHouseTypePM>
    {


        public CustomsHouseTypePM GetHouseTypewithAdditional(string code, int tenant)
        {
            CustomsHouseTypePM houseType = null;
            if (!string.IsNullOrWhiteSpace(code))
            {
                CustomsHouseType type = repository.GetSingle(new CustomsHouseTypeKeys() { Code = code });
                CustomsHouseTypeAdditionalRepository additionalRepository = new CustomsHouseTypeAdditionalRepository(context);
                CustomsHouseTypeAdditional additional = additionalRepository.GetSingleAdditionalByCode(code, tenant);
                if (type == null)
                {
                    throw new Exception("CustomsHouseType code does not exist in DB !!! code =" +code  );
                }
                houseType = new CustomsHouseTypePM()
                                                 {
                                                     Code = type.Code,
                                                     EnglishName = type.EnglishName,
                                                     LocalName = type.LocalName,

                                                     Tenant = tenant
                                                 };
                if (additional != null)
                {
                    houseType.TransportModeId = additional.TransportModeId;
                    houseType.TransportModeName = additional.TransportMode != null ? additional.TransportMode.LocalName : null;
                    houseType.UnloadPortCode = additional.UnloadPortCode;
                    houseType.UnloadPortName = additional.UnloadingSiteType != null ? additional.UnloadingSiteType.LocalName : null;
                }
            }
            return houseType;
        }


        public List<CustomsHouseTypePM> GetAllWithAdditional(int tenant)
        {
            var additionalRepository = new CustomsHouseTypeAdditionalRepository(context);
            var q =
                (from h in repository.GetAll()
                 join ha in additionalRepository.GetAll(tenant)
                 on h.Code equals ha.Code into haJ
                 from ha in haJ.DefaultIfEmpty()
                 select new CustomsHouseTypePM()
                 {
                     Code = h.Code,
                     EnglishName = h.EnglishName,
                     LocalName = h.LocalName,

                     ///Calc
                     Tenant = tenant,
                     TransportModeId = ha == null ? null : ha.TransportModeId,
                     UnloadPortCode = ha == null ? null : ha.UnloadPortCode
                 }

                );
            var l = q.ToList();
            return l;
        }

        public List<CustomsHouseTypePM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }
    }
}

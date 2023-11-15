
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class GovernmentProcedureTypeQueryService : ICanGetAllClosedTable<GovernmentProcedureTypePM>
    {
        public List<GovernmentProcedureTypePM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            //var cts = pms.Select(rec => rec as IClosedTable).ToList();
            return pms;
        }
		public GovernmentProcedureTypePM GetSingleGovernmentProcedureTypeWithTenant(string code, int tenant)
		{

			string key = $"GetSingleGovernmentProcedureTypeWithTenant({code}, {tenant})";
			return Simplog.Server.Infrastructure.Helpers.CacheManager.GetOrInsertNewObject<GovernmentProcedureTypePM>(key, () =>
			{
				return GetSingleGovernmentProcedureTypeWithTenantReal(code, tenant);
			});
		}
		public GovernmentProcedureTypePM GetSingleGovernmentProcedureTypeWithTenantReal(string code, int tenant)
		{
			GovernmentProcedureTypePM governmentProcedureType = null;

			if (!string.IsNullOrWhiteSpace(code))
			{
				GovernmentProcedureType GovernmentProcedureType = repository.GetSingleGovernmentProcedureType(new GovernmentProcedureTypeKeys() { Code = code });

				GovernmentProcTypeTenantRepository governmentProcTypeTenantRep = new GovernmentProcTypeTenantRepository(context);
				GovernmentProcTypeTenant GovernmentProcedureTypeTenant = governmentProcTypeTenantRep.GetSingleByCode(code, tenant);
				if (GovernmentProcedureType != null)
				{
					governmentProcedureType = new GovernmentProcedureTypePM()
					{
						Code = GovernmentProcedureType.Code,
						EnglishName = GovernmentProcedureType.EnglishName,
						LocalName = GovernmentProcedureType.LocalName,
						IsImport = GovernmentProcedureType.IsImport,
						IndexOrder = GovernmentProcedureType.IndexOrder,
						IsExport = GovernmentProcedureType.IsExport,
						ShortProcedure = GovernmentProcedureType.ShortProcedure,
						SearchFields = GovernmentProcedureType.SearchFields,
						Inactive = GovernmentProcedureType.Inactive,

					};

					if (GovernmentProcedureTypeTenant != null)
					{
						governmentProcedureType.IsExport = GovernmentProcedureTypeTenant.IsExport;
						governmentProcedureType.IsImport = GovernmentProcedureTypeTenant.IsImport;
						governmentProcedureType.IndexOrder = GovernmentProcedureTypeTenant.IndexOrder;
						governmentProcedureType.Tenant = tenant;
					}
				}
			}
			return governmentProcedureType;
		}
	}
}

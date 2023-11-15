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
    public partial class CustomsCountryQueryService : ICanGetAllClosedTable<CustomsCountryPM>
    {
        public List<CustomsCountryPM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public CustomsCountryPM GetSingleByMalamID(string MalamID)
        {
            var poco = repository.GetSingleByMalamID(MalamID);
            var pm = this.GetEntityPM(poco);
            return pm;
        }
	
		public List<CustomsCountryPM> GetCustomsCountryByTenant(int tenant)
		{
			var customsCountries = repository.GetAll().ToList();
			return customsCountries.Select(poco => this.GetEntityPM(poco)).ToList();

		}

		public CustomsCountryPM GetSingleCustomsCountryWithTenant(string code, int tenant)
		{

			string key = $"GetSingleCustomsCountryWithTenant({code}, {tenant})";
			return Simplog.Server.Infrastructure.Helpers.CacheManager.GetOrInsertNewObject<CustomsCountryPM>(key, () =>
			{
				return GetSingleCustomsCountryWithTenantReal(code, tenant);
			});
		}
		public CustomsCountryPM GetSingleCustomsCountryWithTenantReal(string code, int tenant)
		{
			CustomsCountryPM customsCountry = null;

			if (!string.IsNullOrWhiteSpace(code))
			{
				CustomsCountry CustomsCountry = repository.GetSingleCustomsCountry(new CustomsCountryKeys() { Code = code });

				CustomsCountryTenantRepository customsCountryTenantRep = new CustomsCountryTenantRepository(context);
				CustomsCountryTenant customsCountryTenant = customsCountryTenantRep.GetSingleByCode(code, tenant);
				if (CustomsCountry != null)
				{
					customsCountry = new CustomsCountryPM()
					{
						Code = CustomsCountry.Code,
						EnglishName = CustomsCountry.EnglishName,
						LocalName = CustomsCountry.LocalName,
						TarriffCode = CustomsCountry.TarriffCode,
						SearchFields = CustomsCountry.SearchFields,
						Inactive = CustomsCountry.Inactive,
						MalamId = CustomsCountry.MalamId,

					};

					if (customsCountryTenant != null)
					{
						customsCountry.TarriffCode = customsCountryTenant.TarriffCode;
						customsCountry.MalamId = customsCountryTenant.MalamId;
						customsCountry.Tenant = tenant;
					}
				}
			}
			return customsCountry;
		}


	}

}
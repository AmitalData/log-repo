using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public partial class CityQueryService
    {
        public CountryCityPM CityCustomDataMappingAndValidatin(City MyEntity, string countryId, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                CountryCityQuery query = new CountryCityQuery(Tenant);
                CountryCityPM temp = null;

                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }

                if (!string.IsNullOrEmpty(MyEntity.Code) && !string.IsNullOrEmpty(countryId))
                {
                    temp = query.GetSinglePMByCodeAndCountryId(MyEntity.Code, countryId, Tenant);
                }
                if (temp == null)
                {
                    throw new ApplicationException("City with Code " + MyEntity.Code + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.Code;
                }
                
                return temp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

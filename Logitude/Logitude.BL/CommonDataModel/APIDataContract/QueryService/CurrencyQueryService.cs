using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   public partial class CurrencyQueryService
    {
        public CurrencyPM CurrencyCustomDataMappingAndValidatin(Currency MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new CurrencyPM();
                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }
                else
                {
                    temp = query.GetSingleCurrencyByCode(MyEntity.Code, Tenant);
                }
                if (temp == null)
                {
                    throw new ApplicationException("Currency with Id " + MyEntity.Id + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.Code;
                }
                if (string.IsNullOrEmpty(temp.EnglishName))
                {
                    temp.EnglishName = MyEntity.EnglishName;
                }
                if (string.IsNullOrEmpty(temp.LocalName))
                {
                    temp.LocalName = MyEntity.LocalName;
                }
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public Currency CurrencyCustomDataMapping(string Id, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                CurrencyQueryService CurrencyService0 = new CurrencyQueryService(Tenant);
                var Currency = CurrencyService0.GetCurrencyById(Id, Tenant,ComputingPartnerName);
                return Currency;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
  public partial  class ChargesTypeQueryService
    {

        public ChargesTypePM ChargesTypeCustomDataMappingAndValidatin(ChargesType MyEntity, int Tenant, string ComputingPartnerName = "")
        {

            try
            {
                var temp = new ChargesTypePM();

                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }

                else
                {
                    temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
                }


                if (temp == null)
                {
                    throw new ApplicationException("ChargesType with code " + MyEntity.Code + " doesn't exist");
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



        public ChargesType ChargesTypeCustomDataMapping(string Id, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                ChargesTypeQueryService ChargesTypeService0 = new ChargesTypeQueryService(Tenant);
                var ChargeType = ChargesTypeService0.GetChargesTypeById(Id, Tenant,ComputingPartnerName);
                return ChargeType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

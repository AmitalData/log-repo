using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   public partial class UserQueryService
    {
        public UserPM UserCustomDataMappingAndValidatin(User MyEntity, int Tenant, string ComputingPartnerName = "")
        {

            try
            {
                var temp = new UserPM();

                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }

                else
                {
                    temp = query.GetSingleUserPMByCode(MyEntity.ExternalCode, Tenant, false);
                }


                if (temp == null)
                {
                    throw new ApplicationException("User with Id " + MyEntity.Id + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.ExternalCode;
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



        public User UserCustomDataMapping(string Id, int Tenant)
        {
            try
            {

                UserQueryService UserService0 = new UserQueryService(Tenant);
                var ChargeType = UserService0.GetUserById(Id, Tenant);
                return ChargeType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   public partial class VatTypeQueryService
    {
        public VatTypePM VatTypeCustomDataMappingAndValidatin(VatType MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new VatTypePM();
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
                    throw new ApplicationException("VatType with Id " + MyEntity.Id + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }

                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public VatType VatTypeCustomDataMapping(string Id, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                VatTypeQueryService VatTypeService0 = new VatTypeQueryService(Tenant);
                var VatType = VatTypeService0.GetVatTypeById(Id, Tenant,ComputingPartnerName);
                return VatType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}

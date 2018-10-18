using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServiceExt
{
    public class GLAccountQueryServiceExt : IGLAccountQueryServiceExt
    {
        public GLAccountQueryServiceExt()
        {

        }

        public GLAccountPM GetSingleGLAccountPM(string id, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            return query.GetSinglePM(id, tenant);
        }



        public GLAccount GetGLAccountById(string Id, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            try
            {


                var temp = query.GetSinglePM(Id, tenant);
                if (temp == null)
                    throw new ApplicationException("GLAccount with Id " + Id + " doesn't exist");

                return GLAccountDataMapping(temp, tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public GLAccount GLAccountDataMapping(GLAccountPM MyEntityPM, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var temp = new GLAccount();
                temp.Id = MyEntityPM.Id;
                temp.EnglishName = MyEntityPM.EnglishName;
                temp.LocalName = MyEntityPM.LocalName;
               
                //if (MyEntityPM.MainAddressId != null)
                //{
                //    AddressQueryService AddressService0 = new AddressQueryService(Tenant);
                //    temp.MainAddress = AddressService0.GetAddressById(MyEntityPM.MainAddressId, Tenant);

                //}

                temp.VatNumber = MyEntityPM.VatNumber;
             
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}

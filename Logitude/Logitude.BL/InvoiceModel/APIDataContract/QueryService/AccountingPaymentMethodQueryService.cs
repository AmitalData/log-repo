using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.InvoiceModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
   public partial class AccountingPaymentMethodQueryService
    {

        public AccountingPaymentMethod AccountingPaymentMethodCustomDataMapping(string id, int tenant)
        {
            try
            {
                
                var entity = new AccountingPaymentMethodPM();
                if (!string.IsNullOrEmpty(id))
                {
                    entity = query.GetSinglePaymentMethodPM(id, tenant);
                }

                if (entity == null)
                {
                    throw new ApplicationException("AccountingPaymentMethod with code " + id + " doesn't exist");
                }
                var temp = new AccountingPaymentMethod();
                temp.Id = entity.Id;
                temp.Tenant = entity.Tenant;
                temp.LogitudeCode = entity.Code;
                temp.Name = entity.Name;
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public AccountingPaymentMethodPM AccountingPaymentMethodCustomDataMappingAndValidatin(AccountingPaymentMethod MyEntity, int  tenant)
        {

            try
            {
                var temp = new AccountingPaymentMethodPM();
                if (!string.IsNullOrEmpty(MyEntity.LogitudeCode))
                {
                    temp = query.GetSinglePaymentMethodPMByCode(MyEntity.LogitudeCode, tenant);
                }

                if (temp == null)
                {
                    throw new ApplicationException("AccountingPaymentMethod with code " + MyEntity.LogitudeCode + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                temp.Tenant = MyEntity.Tenant;
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.LogitudeCode;
                }
                temp.Name = MyEntity.Name;
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}

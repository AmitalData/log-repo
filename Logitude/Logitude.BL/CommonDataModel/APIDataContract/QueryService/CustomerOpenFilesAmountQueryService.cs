using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   public partial class CustomerOpenFilesAmountQueryService
    {
        public CustomerOpenFilesAmountPM SetCustomerId(CustomerOpenFilesAmountPM customerOpenFilesAmount) {
            CustomerQuery customerQuery = new CustomerQuery(customerOpenFilesAmount.Tenant);
            CustomerPM customer = customerQuery.GetSingleCustomerPMByCode(customerOpenFilesAmount.CustomerCode, customerOpenFilesAmount.Tenant);
            if(customer == null)
            {
                throw new ApplicationException("Customer with code " + customerOpenFilesAmount.CustomerCode + " doesn't exist");
            }
            else
            {
                customerOpenFilesAmount.CustomerId = customer.Id;
                return customerOpenFilesAmount;
            }



        }


    }
}

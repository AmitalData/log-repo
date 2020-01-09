using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
  public  class CustomerOpenFilesAmountQuery
    {
        CustomerOpenFilesAmountRepository repository;

        public CustomerOpenFilesAmountQuery()
        {
            repository = new CustomerOpenFilesAmountRepository();
        }

        public CustomerOpenFilesAmountQuery(int tenant)
        {
            repository = new CustomerOpenFilesAmountRepository(tenant);
        }

        public CustomerOpenFilesAmountQuery(CustomerOpenFilesAmountRepository CustomerOpenFilesAmountRepository)
        {
            repository = CustomerOpenFilesAmountRepository;
        }

        public CustomerOpenFilesAmountPM GetSinglePMByCustomerId(string customerId,  int tenant)
        {
            CustomerOpenFilesAmountPM customerOpenFilesAmount = (from a in repository.context.CustomerOpenFilesAmounts
                                                    where a.Tenant == tenant && a.CustomerId == customerId 
                                                  
                                                    select new CustomerOpenFilesAmountPM()
                                                    {
                                                        CustomerId = a.CustomerId,
                                                         TotalOpenFilesAmount= a.TotalOpenFilesAmount,
                                                        Tenant = a.Tenant,
                                                      
                                                    }).FirstOrDefault();

            
            return customerOpenFilesAmount;
        }

     


    }
}

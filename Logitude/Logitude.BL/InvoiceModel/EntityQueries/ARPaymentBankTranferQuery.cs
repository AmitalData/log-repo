using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
   public class ARPaymentBankTranferQuery
    {
        ARPaymentBankTranferRepository repository;
        public ARPaymentBankTranferQuery()
        {
            repository = new ARPaymentBankTranferRepository();
        }


        public ARPaymentBankTranferQuery(int tenant)
        {
            repository = new ARPaymentBankTranferRepository(tenant);
        }

        public ARPaymentBankTranferQuery(ARPaymentBankTranferRepository arPaymentMethodRepository)
        {
            repository = arPaymentMethodRepository;
        }

        public ARPaymentBankTranferPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.ARPaymentBankTranfers
                    where a.Id == id && a.Tenant == tenant
                    select new ARPaymentBankTranferPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,


                    }).FirstOrDefault();
        }


    }
}

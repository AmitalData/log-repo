using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
   public class ARPaymentChequeStatusReplicaQuery
    {


        ARPaymentChequeStatusReplicaRepository repository;
        public ARPaymentChequeStatusReplicaQuery()
        {
            repository = new ARPaymentChequeStatusReplicaRepository();
        }


        public ARPaymentChequeStatusReplicaQuery(int tenant)
        {
            repository = new ARPaymentChequeStatusReplicaRepository(tenant);
        }

        public ARPaymentChequeStatusReplicaQuery(ARPaymentChequeStatusReplicaRepository aRPaymentChequeStatusReplicaRepository)
        {
            repository = aRPaymentChequeStatusReplicaRepository;
        }

        public ARPaymentChequeStatusReplicaPM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.ARPaymentChequeStatusReplicas
                    where a.Code == code 
                    select new ARPaymentChequeStatusReplicaPM()
                    {
                        
                        Code = a.Code,
                       
                        SearchFields = a.SearchFields,
                       EnglishName = a.EnglishName,
                        Inactive = a.Inactive,
                       LocalName = a.LocalName
                    }).FirstOrDefault();
        }
        public ARPaymentChequeStatusReplicaPM GetSinglePM(string code)
        {
            return (from a in repository.context.ARPaymentChequeStatusReplicas
                    where a.Code == code
                    select new ARPaymentChequeStatusReplicaPM()
                    {

                        Code = a.Code,

                        SearchFields = a.SearchFields,
                        EnglishName = a.EnglishName,
                        Inactive = a.Inactive,
                        LocalName = a.LocalName
                    }).FirstOrDefault();
        }



    }
}

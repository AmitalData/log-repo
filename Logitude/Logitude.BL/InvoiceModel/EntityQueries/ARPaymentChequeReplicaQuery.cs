using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
   public class ARPaymentChequeReplicaQuery
    {


        ARPaymentChequeReplicaRepository repository;
        public ARPaymentChequeReplicaQuery()
        {
            repository = new ARPaymentChequeReplicaRepository();
        }


        public ARPaymentChequeReplicaQuery(int tenant)
        {
            repository = new ARPaymentChequeReplicaRepository(tenant);
        }

        public ARPaymentChequeReplicaQuery(ARPaymentChequeReplicaRepository arPaymentMethodRepository)
        {
            repository = arPaymentMethodRepository;
        }

        public ARPaymentChequeReplicaPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.ARPaymentChequeReplicas
                    where a.Id == id && a.Tenant == tenant
                    select new ARPaymentChequeReplicaPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                       
                        
                    }).FirstOrDefault();
        }

        public List<ARPaymentChequeReplicaPM> GetARPaymentChequeReplicaPMsByPaymentId(string paymentId, int tenant)
        {

            bool showLocal = !GetLoggedContact(tenant).DontShowLocal;
            List<ARPaymentChequeReplica> paymentCheques = repository.GetARPaymentChequeReplicas(paymentId, tenant).ToList();
            return (from a in paymentCheques
                    where a.PaymentId == paymentId && a.Tenant == tenant
                    select new ARPaymentChequeReplicaPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        BankAccount = a.BankAccount,
                        BankBranch = a.BankBranch ,
                        ForeignAmount = a.ForeignAmount ,
                        LocalAmount = a.LocalAmount ,
                        ValueDate = a.ValueDate ,
                        PaymentId = a.PaymentId ,
                        ChequeNumber = a.ChequeNumber ,
                        LineNumber = a.LineNumber ,
                        BankId = a.BankId,
                        StatusCode= a.StatusCode,
                        CurrencyId = a.CurrencyId,
                        StatusName = showLocal ? a.ARPaymentChequeStatusReplica.LocalName : a.ARPaymentChequeStatusReplica.EnglishName,
                      
                    }).OrderBy(d => d.LineNumber).ToList();

        }

        public bool ChequeIfPaymentChequeReplicaExist(string paymentId,string chequeNo, int LineNo, int tenant)
        {

          return repository.ChequeIfPaymentChequeReplicaExist(paymentId, chequeNo, LineNo, tenant);
          

        }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }
}

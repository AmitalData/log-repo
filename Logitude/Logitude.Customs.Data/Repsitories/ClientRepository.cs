 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class ClientRepository:IRepository<Client>
   {
        
		public List<Client> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public string GetIdByCode(int tenant, string code)
        {
            if (String.IsNullOrWhiteSpace(code)) return "";

            // fix the teudat zeut length
            code = code.PadLeft(9, '0');

            return
                   (
                  from rec in context.Clients
                  where rec.Code== code && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public string GetIdByCodeOrPassport(int tenant, string code, string passport)
        {
            // if (String.IsNullOrWhiteSpace(code)) return "";
            code = code.PadLeft(9, '0');
            return
                  (
                  from rec in context.Clients
                  where (rec.Code == code || (rec.PassportNumber == passport && !string.IsNullOrEmpty(passport))) && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public string GetIdByPassportNumberOrCountry(int tenant, string passportNumber, string passportCountryCode)
        {
            if (string.IsNullOrEmpty(passportNumber) && string.IsNullOrEmpty(passportCountryCode)) return "";
            return
                  (
                  from rec in context.Clients
                  where (rec.PassportNumber == passportNumber && rec.PassportCountryCode == passportCountryCode) && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public Client GetSingleClientByCode(string code, int Tenant)
        {
            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            Client client = (from a in context.Clients
                             where a.Code == code
                             && a.Tenant == Tenant
                             select a).FirstOrDefault();
            return client;

        }

        public List<Client> GetAllLocalClients(int tenant)
        {
            return (
                  from rec in context.Clients
                  where !string.IsNullOrEmpty(rec.Code) && rec.Tenant == tenant
                  select rec
                  ).ToList();
        }
        public List<Client> GetAllLocalClientsIsConcurrencyGUID(int tenant)
        {
            return (
                  from rec in context.Clients
                  where !string.IsNullOrEmpty(rec.Code) && rec.Tenant == tenant
                  select rec
                  ).ToList();
        }

        public List<Client> GetAllClientsPOAExpire(int tenant)
        {
            var date = DateTime.Now.AddDays(30);
            var clients = from client in context.Clients
                               where !string.IsNullOrEmpty(client.Code) && client.Tenant == tenant && client.IsExportPoaActive == true && (client.IsPOAExpireReminderSent == null || client.IsPOAExpireReminderSent == false)
                               where !context.ClientsPoas.Any(poa=>poa.ClientId == client.Id && date <= poa.EndDate)
                               select client;
            return clients.ToList();
        }
    }

}
   
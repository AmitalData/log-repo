 
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
                  where (rec.PassportNumber == passportNumber || rec.PassportCountryCode == passportCountryCode) && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public Client GetSingleClientByCode(string code, int Tenant)
        {

            Client client = (from a in context.Clients
                             where a.Code == code
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
    }

}
   
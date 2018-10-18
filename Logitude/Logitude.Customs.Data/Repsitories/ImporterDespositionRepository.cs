 
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
   public partial class ImporterDespositionRepository:IRepository<ImporterDesposition>
   {
        
		public List<ImporterDesposition> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public string GetImporterDespositionByDepositionNumber(string depositionNumber, int tenant)
        {
            if (string.IsNullOrWhiteSpace(depositionNumber)) return null;
            return
                  (
                  from rec in context.ImporterDespositions
                  where rec.DepositionNumber == depositionNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public List<ImporterDesposition> GetImporterDespositionByDepositionNumberImporterVendor(string depositionNumber, string importerId, string vendorID, int tenant)
        {
            if (string.IsNullOrWhiteSpace(depositionNumber)) return null;
            return
                  (
                  from rec in context.ImporterDespositions
                  where rec.DepositionNumber == depositionNumber &&
                  rec.ImporterlId == importerId &&
                  rec.VendorID == vendorID &&
                  rec.Tenant == tenant
                  select rec
                  ).ToList();
        }

        public IQueryable<ImporterDesposition> GetImporterDespositions(string vendorId, string importerId, int tenant)
        {


            IQueryable<ImporterDesposition> despositions;

            despositions = (from a in context.ImporterDespositions
                            where a.VendorID == vendorId && a.ImporterlId == importerId && a.Tenant == tenant
                            orderby  a.EndDate descending
                            select a);
            return despositions;


        }


    }

}


   
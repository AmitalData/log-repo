 
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
   public partial class TapagConnectionTableRepository:IRepository<TapagConnectionTable>
   {
        
		public List<TapagConnectionTable> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        

        public List<TapagConnectionTable> GetDearationTapagConnectionTables(string declarationId, string tapagId,  int tenant)
        {
            List<TapagConnectionTable> connections = new List<TapagConnectionTable>();
            if (!string.IsNullOrEmpty(declarationId))
            {
                connections =  (from a in context.TapagConnectionTables
                        where a.DeclarationId == declarationId && a.Tenant == tenant
                        select a).ToList();
            }
            else if (!string.IsNullOrEmpty(tapagId))
            {
                connections =  (from a in context.TapagConnectionTables
                        where a.TapagId == tapagId && a.Tenant == tenant
                        select a).ToList();
            }

            return connections;
        }


        public string GetTapagIdByFileAndNumeral(string fileNumber, int numeral, int tenant)
        {
            if (string.IsNullOrEmpty(fileNumber) && numeral <= 0) return ""; 
            return
                  (
                  from rec in context.TapagConnectionTables
                  where rec.CustomsTapagFile == fileNumber && rec.CustomsNumeral == numeral && rec.Tenant == tenant
                  select rec.TapagId
                  )
                  .FirstOrDefault();
        }

        public string GetTapagIdByRequestFileNumber(string requestFileNumber, int tenant)
        {
            if (string.IsNullOrEmpty(requestFileNumber)) return "";
            return
                  (
                  from rec in context.TapagConnectionTables
                  where rec.RequestFileNumber == requestFileNumber && rec.Tenant == tenant
                  select rec.TapagId
                  )
                  .FirstOrDefault();
        }

        public TapagConnectionTable GetTapagConnectionByFileAndNumeral(string fileNumber, int numeral, int tenant)
        {
            if (string.IsNullOrEmpty(fileNumber) && numeral <= 0) return null;
            return
                  (
                  from rec in context.TapagConnectionTables
                  where rec.CustomsTapagFile == fileNumber && rec.CustomsNumeral == numeral && rec.Tenant == tenant
                  select rec
                  )
                  .FirstOrDefault();
        }

        public List<TapagConnectionTable> GetTapagConnectionByTapagId(string tapagId, int tenant)
        {
            if (string.IsNullOrEmpty(tapagId)) return null;
            return
                  (
                  from rec in context.TapagConnectionTables
                  where rec.TapagId == tapagId && rec.Tenant == tenant
                  select rec
                  )
                  .ToList();
        }

   }

}
   
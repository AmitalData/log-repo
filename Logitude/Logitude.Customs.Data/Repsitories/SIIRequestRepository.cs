 
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
using System.Data.Entity;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SIIRequestRepository:IRepository<SIIRequest>
   {
        
		public List<SIIRequest> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

       
        public SiiAgg GetAggregateForSii(int tenant, string declarationId)
        {
            return (from d in context.Declarations
                    where d.Tenant == tenant && d.Id == declarationId

                    from c in context.Clients
                             .Where(c => c.Tenant == tenant && c.Code == d.ImporterId)
                             .DefaultIfEmpty()

                    from rd in context.DeclarationReferantDatas.Include("Vessel")
                             .Where(r => r.Tenant == tenant && r.DeclarationId == d.Id)
                             .DefaultIfEmpty()
                    from con in context.Consignments
                               .Where(con => con.Tenant == tenant && con.DeclarationId == d.Id)
                               .OrderBy(con => con.SequenceNumeric)
                               .Take(1)
                               .DefaultIfEmpty()

                    select new SiiAgg
                    {
                        ImporterInternalId = c == null ? null : c.Id,
                        ImporterCode = d.ImporterCode,
                        VesselLocalName = rd.Vessel != null
                                     ? (rd.VesselCode.LocalName ?? rd.VesselCode.EnglishName)
                                     : null,
                        ManifestNumber = con == null ? null : con.ManifestNumber,
                        UnloadDate = con == null ? null : (DateTime?)con.UnloadDate
                    })
                    .AsNoTracking()
                    .FirstOrDefault();
        }
        public class SiiAgg
        {
            public string ImporterInternalId { get; set; }   // can be null
            public string ImporterCode { get; set; }
            public string VesselCode { get; set; }
            public string VesselLocalName { get; set; }
            public string ManifestNumber { get; set; }
            public DateTime? UnloadDate { get; set; }
        }

    }


}
   
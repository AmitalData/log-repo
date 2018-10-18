 
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
   public partial class CustomsDocumentsDefinitionRepository:IRepository<CustomsDocumentsDefinition>
   {
        
		public List<CustomsDocumentsDefinition> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CustomsDocumentsDefinition> GetCustomsDocumentsDefinitionsForDeclaration(string cargoTypeCode, string processTypeCode, string transportTypeCode, int tenant)
        {
            return (from a in context.CustomsDocumentsDefinitions
                    where (a.CargoTypeCode == cargoTypeCode || a.CargoTypeCode == null) && (a.ProcessTypeCode == processTypeCode || a.ProcessTypeCode == null) && 
                    (a.TransportationTypeCode == transportTypeCode || a.TransportationTypeCode == null) && a.Inactive == false && a.Tenant == tenant
                    orderby a.DocumentTypeCode, a.ProcessTypeCode , a.CargoTypeCode , a.TransportationTypeCode 
                    select a).ToList();
        }
    }

}
   
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class DigitalPreDefinedComponentRepository:IRepository<DigitalPreDefinedComponent>
   {

        public IQueryable<DigitalPreDefinedComponent> GetDigitalPreDefinedComponents(int tenant, string objectTableId, string name)
        {
            return context.DigitalPreDefinedComponents
                          .Where(a => (a.Tenant == tenant || a.Tenant == 0)
                                      && (a.ObjectTableId.Equals(objectTableId) || string.IsNullOrEmpty(a.ObjectTableId))
                                      && (string.IsNullOrEmpty(name) || a.Name.Equals(name)));
        }

        public List<DigitalPreDefinedComponent> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   
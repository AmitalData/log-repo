 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class EmployeeGroupRepository:IRepository<EmployeeGroup>
   {        
		public List<EmployeeGroup> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public bool IsEmployeeGroupExists(int tenant)
        {
            return context.EmployeeGroups.Where(d => d.Tenant == tenant && d.Name == "Unassigned Tickets").Any();
        }

        public EmployeeGroup GetEmployeeGroupByName(string name ,int tenant)
        {
            EmployeeGroup entity = (from a in context.EmployeeGroups
                                  where a.Tenant == tenant && a.Name == name
                                  select a).FirstOrDefault();
            return entity;
        }

   }
}
   
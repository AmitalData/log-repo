 
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
   public partial class EmployeeGroupLineRepository:IRepository<EmployeeGroupLine>
   {        
		public List<EmployeeGroupLine> GetMulti(EntityKeyFields entityKeys)
        {
            EmployeeGroupKeys myEntityKeys = entityKeys as EmployeeGroupKeys;
            return (from a in context.EmployeeGroupLines where a.EmployeeGroupId == myEntityKeys.Id select a).ToList();
        }

        public EmployeeGroupLine GetEmployeeGroupLinesByGroupId(string groupId, int tenant)
        {
            return (from a in context.EmployeeGroupLines where a.EmployeeGroupId == groupId && a.Tenant == tenant && a.IsDefaultOwner==true select a).FirstOrDefault();
        }
   }
}
   
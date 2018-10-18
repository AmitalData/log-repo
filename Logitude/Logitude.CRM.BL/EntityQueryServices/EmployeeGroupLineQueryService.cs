using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class EmployeeGroupLineQueryService
    {
        public EmployeeGroupLinePM GetEmployeeGroupLinesByGroupId(string groupId, int tenant)
        {
            ICRMContext context = MainContext as ICRMContext;
            EmployeeGroupLinePM myOwner = (from a in context.EmployeeGroupLines
                                                     where a.Tenant == tenant && a.EmployeeGroupId == groupId && a.IsDefaultOwner == true
                                                     select new EmployeeGroupLinePM()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant, 
                                                         EmployeeGroupId = a.EmployeeGroupId,
                                                         UserId=a.UserId,
                                                         IsDefaultOwner = a.IsDefaultOwner,
                                                     }).FirstOrDefault();

            return myOwner;
        }
    }
}

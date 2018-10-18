using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class EmployeeGroupQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, EmployeeGroupPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            EmployeeGroupKeys employeeGroupKeys = entityKeys as EmployeeGroupKeys;

            EmployeeGroupLineQueryService queryService = new EmployeeGroupLineQueryService(context);
            entityPM.EmployeeGroupLines = queryService.GetMulti(employeeGroupKeys, true);
        }

        public List<EmployeeGroupPM> GetAllEmployeeGroupsByTenant(int tenant)
        {
            List<EmployeeGroupPM> query = (from a in context.EmployeeGroups
                                            where a.Tenant == tenant && a.Inactive == false
                                           select new EmployeeGroupPM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                CreateDate = a.CreateDate,
                                                CreatedByUserId = a.CreatedByUserId,
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                SearchFields = a.SearchFields,
                                                Name = a.Name,
                                                Description = a.Description,
                                                Inactive = a.Inactive,
                                                ManagerUserId = a.ManagerUserId,

                                            }).ToList();

            foreach (EmployeeGroupPM item in query)
            {
                item.EmployeeGroupLines = (from a in context.EmployeeGroupLines
                                           where a.EmployeeGroupId == item.Id && a.Tenant == item.Tenant
                                           select new EmployeeGroupLinePM()
                                           {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               EmployeeGroupId = a.EmployeeGroupId,
                                               UserId = a.UserId,
                                               IsDefaultOwner = a.IsDefaultOwner,

                                           }).ToList();
            }

            return query;
        }

    }
}

 
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
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class BIReportFolderRepository:IRepository<BIReportFolder>
   {
        
		public List<BIReportFolder> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<BIReportFolderList> GetFoldersByPermittedUser(string loggedUserId, List<string> permittedFolders, int tenant)
        {
            List<BIReportFolderList> query = (from a in context.BIReportFolders
                                                    where a.Tenant == tenant && (a.CreatedByUserId == loggedUserId || a.PermissionForAll || permittedFolders.Contains(a.Id))
                                                    select new BIReportFolderList()
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
                                                        Index = a.Index,
                                                        PermissionForAll = a.PermissionForAll,
                                                        PermittedByUserId = a.PermittedByUserId
                                                    }).ToList();
            return query;
        }

    }

}
   
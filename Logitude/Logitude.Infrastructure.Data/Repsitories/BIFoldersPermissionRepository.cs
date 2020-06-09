 
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
   public partial class BIFoldersPermissionRepository:IRepository<BIFoldersPermission>
   {
        
		public List<BIFoldersPermission> GetMulti(EntityKeyFields entityKeys)
        {
            BIReportFolderKeys myEntityKeys = entityKeys as BIReportFolderKeys;
            return (from a in context.BIFoldersPermissions where a.FolderId == myEntityKeys.Id select a).ToList();
        }

        public List<string> GetFoldersIdsByPermittedUserId(string permittedUserId)
        {
            List<string> foldersIds = (from a in context.BIFoldersPermissions where a.UserId == permittedUserId select a.FolderId).ToList();
            return foldersIds;
        }

    }

}
   
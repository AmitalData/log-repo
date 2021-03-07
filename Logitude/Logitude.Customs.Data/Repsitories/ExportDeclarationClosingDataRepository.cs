 
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
   public partial class ExportDeclarationClosingDataRepository:IRepository<ExportDeclarationClosingData>
   {
        
		public List<ExportDeclarationClosingData> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public ExportDeclarationClosingData getByDecId(string id,int tenant)
        {
            {
                return (from a in context.ExportDeclarationClosingDatas
                        where a.DeclarationId == id && a.Tenant == tenant
                        select a).FirstOrDefault();
            }
        }

    }

}
   
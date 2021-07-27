 
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
   public partial class ContainerizationRepository:IRepository<Containerization>
   {
        
		public List<Containerization> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public int GetContainerizationNumber(int tenant)
        {
            var list = (from a in context.Containerizations
                        where a.Tenant == tenant && a.ContainerizationNumber != null
                        select a.ContainerizationNumber).ToList();

            int max = 0;

            if (list.Count() != 0)
                max = list.Select(int.Parse).ToList().Max();

            return max+1;
        }
        public List<string> GetContainerizationExportFiles(int tenant,string exportFiles)
        {
            return (from a in context.Declarations
                    where a.Tenant == tenant && exportFiles.Contains(a.Id)
                    select a.ExportFile).Distinct().ToList();
        }
        public List<string> GetContainerizationImporters(int tenant, string importers)
        {
            return (from a in context.Declarations
                    where a.Tenant == tenant && importers.Contains(a.Id)
                    select a.CustomerId).Distinct().ToList();
        }
    }

}
   
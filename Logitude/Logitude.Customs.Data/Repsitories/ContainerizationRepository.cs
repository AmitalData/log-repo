 
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
using Simplog.Data.CommonDataModel.EntityPOCOs;

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
        public List<Card> GetContainerizationImporters(int tenant, string importers)
        {
            return (from a in context.Declarations
                    where a.Tenant == tenant && importers.Contains(a.Id)
                    select a.CustomerCard).Distinct().ToList();
        }
        public List<ContainerizationDetails> GetContainerizationByKeys(int tenant, List<string> Keys)
        {
             var query = (from a in context.Containerizations
                    where a.Tenant == tenant && Keys.Contains((a.CargoTypeCode.ToLower() + a.ManifestNumber.ToLower() + a.SecondCargoID.ToLower() + a.ThirdCargoID.ToLower()).ToString())
                    select a).ToList();
            return query.Select(cont => new ContainerizationDetails
                {
                    Id = cont.Id,
                    Tenant = cont.Tenant,
                    ContainerizationNumber = cont.ContainerizationNumber
                }).Distinct().OrderBy(c => c.ContainerizationNumber).ToList();
        }
        public class ContainerizationDetails
        {
            public  string Id { get; set; }
            public int Tenant { get; set; }
            public string ContainerizationNumber { get; set; }
            
        }

    }

}
   
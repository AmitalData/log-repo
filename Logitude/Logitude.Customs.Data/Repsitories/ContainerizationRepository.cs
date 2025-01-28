 
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
using Logitude.Customs.Data.EntityMapping;
using System.Data.Entity;


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

        public ConKeys GetcontainerizationById(string exportContainerizationID, int tenant)
        {
            var query = (from a in context.Containerizations
                         where a.Id == exportContainerizationID && a.Tenant == tenant
                         select a).Select(y => new ConKeys
                         {
                             Id=y.Id,
                             CargoTypeCode = y.CargoTypeCode,
                             ManifestNumber = y.ManifestNumber,
                             SecondCargoID = y.SecondCargoID,
                             ThirdCargoID = y.ThirdCargoID

                         }).FirstOrDefault(); 
            return query;
        }
        public List<Containerization> GetContainerizationsByIds(string ids, int tenant)
        {
            
            return  (from a in context.Containerizations.Include("ContainerizationStatusCode").Include("ContainerizationHataraStatus")
                     where a.Tenant == tenant && ids.Contains((a.Id))
                         select a).ToList();
               
        }


        public class ConKeys
        {
            public string Id { get; set; }
            public string CargoTypeCode { get; set; }
            public string ManifestNumber { get; set; }
            public string SecondCargoID { get; set; }
            public string ThirdCargoID { get; set; }

        }
        public class ContainerizationDetails
        {
            public  string Id { get; set; }
            public int Tenant { get; set; }
            public string ContainerizationNumber { get; set; }
            
        }





    }

}
   
 
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
using Simplog.Server.Infrastructure.Helpers;
namespace Logitude.Customs.Data.Repsitories
{
   public partial class PhysicalCheckRepository:IRepository<PhysicalCheck>
   {
        
		public List<PhysicalCheck> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public string  GetIdByCheckId(string CheckId, int tenant)
        {
            if (String.IsNullOrWhiteSpace(CheckId)) return null;
            return (from a in context.PhysicalChecks
                    where a.CheckId == CheckId && a.Tenant == tenant
                    select a.Id).FirstOrDefault();
        }

        public List<PhysicalCheck> GetPhysicalChecksByDeclarationId(string declarationId, int tenant)
        {

            return (from a in context.PhysicalChecks.Include("Declaration").Include("StorageSite").Include("CheckSite").Include("CheckQueueType").Include("Operation").Include("Declaration.CustomerCard")
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).ToList();
        }

        //<--- Yuval Chalup 17.11.2014 TASK-9089
        public List<PhysicalCheck> GethysicalCheckByDeclarationIdOnly(string declarationId, int tenant)
        {

            return (from a in context.PhysicalChecks
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).ToList();
        }
       //Yuval Chalup 17.11.2014 TASK-9089 --->

   }
}
   
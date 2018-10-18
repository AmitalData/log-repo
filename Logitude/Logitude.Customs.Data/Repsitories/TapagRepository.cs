 
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
   public partial class TapagRepository:IRepository<Tapag>
   {
        
		public List<Tapag> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public Tapag GetSingleTapagByLeadingFileNumber(string leadingFileNumber, int tenant)
        {
            Tapag tapag = (from a in context.Tapags
                           where a.LeadingFileNumber == leadingFileNumber && a.Tenant == tenant
                           select a).FirstOrDefault();

            return tapag;
        }

        public List<Tapag> GetTapagsByListOfIds(List<string> ids)
        {


            List<Tapag> tapags = (from a in context.Tapags.Include("TapagType").Include("Importer").Include("CustomsBranch").Include("ProfessionUnitType")
                                   where ids.Contains(a.Id) 
                                       select a).ToList();

            return tapags;

           
        }

   }

}
   
 
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
   public partial class CustomsPartnerFtpRepository:IRepository<CustomsPartnerFtp>
   {
        
		public List<CustomsPartnerFtp> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public List<CustomsPartnerFtp> GetAllTenantBy(string InterfaceName, string PartnerCode, string TypeCode)
        {
            var q=(from r in context.CustomsPartnerFtps
            where r.InterfaceName == InterfaceName && r.PartnerCode == PartnerCode && r.TypeCode == TypeCode
            select r);
            return q.ToList();
                 
            
        }
   }

}
   
 
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
   public partial class CustomsCountryRepository:IRepository<CustomsCountry>
   {
        
		public List<CustomsCountry> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public CustomsCountry GetSingleByMalamID(string MalamID)
        {
            return (from a in context.CustomsCountries
                    where a.MalamId == MalamID
                    select a).FirstOrDefault();
        }
		public CustomsCountry GetSingleCustomsCountry(EntityKeyFields entityKeys)
		{
			CustomsCountryKeys keys = entityKeys as CustomsCountryKeys;
			return (from a in context.CustomsCountries
					where a.Code == keys.Code
					select a).FirstOrDefault();
		}
	}

}
   
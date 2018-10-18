 
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
using System.Linq;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsHouseTypeRepository:IRepository<CustomsHouseType>
   {
        
		public List<CustomsHouseType> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        //public CustomsHouseType GetCustomsHouseTypeByCode(string code)
        //{
        //  CustomsHouseType houseType =  (from a in context.CustomsHouseTypes
        //                                 join s in context.CustomsHouseTypeAdditionals
        //                                 on a.Code equals s.Code
        //                                select a).FirstOrDefault();

        

          
                
        //}

   }

}
   
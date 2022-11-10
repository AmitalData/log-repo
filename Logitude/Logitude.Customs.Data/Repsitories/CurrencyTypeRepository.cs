 
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
using Simplog.Data.CommonDataModel;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CurrencyTypeRepository:IRepository<CurrencyType>
   {
        
        public List<CurrencyType> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public ICustomContext customsDataContext
        {
            get { return this.context; }
        }

    }

}
   
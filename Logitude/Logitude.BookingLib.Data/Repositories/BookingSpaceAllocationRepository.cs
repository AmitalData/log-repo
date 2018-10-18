 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.BookingLib.Data.Repositories
{
   public partial class BookingSpaceAllocationRepository:IRepository<BookingSpaceAllocation>
   {
        
		public List<BookingSpaceAllocation> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   
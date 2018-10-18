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
   public partial class BookingProductRepository:IRepository<BookingProduct>
   {        
		public List<BookingProduct> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public IQueryable<BookingProduct> GetBookingProductsByAirlineId(string airlineId)
        {
            return context.BookingProducts.Where(a => a.AirlineId == airlineId);
        }
   }
}
   
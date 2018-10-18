 
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
   public partial class BookingAnswerRepository:IRepository<BookingAnswer>
   {
       public List<BookingAnswer> GetMulti(EntityKeyFields entityKeys)
       {
           BookingKeys myEntityKeys = entityKeys as BookingKeys;
           return (from a in context.BookingAnswers where a.BookingId == myEntityKeys.Id select a).ToList();
       }

       public IQueryable<BookingAnswer> GetBookingAnswersForBookingTenant(string bookingId, int tenant)
       {
           IQueryable<BookingAnswer> bookingAnswers = from a in context.BookingAnswers where a.Tenant == tenant && a.BookingId == bookingId select a;
           return bookingAnswers;
       }

   }

}
   
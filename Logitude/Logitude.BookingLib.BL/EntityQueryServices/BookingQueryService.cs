using Logitude.BookingLib.BL.DataContracts;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityKeys;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.EntityQueryServices
{
    public partial class BookingQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, BookingPM entityPM)
        {
            IBookingContext context = MainContext as IBookingContext;
            BookingKeys bookingKeys = entityKeys as BookingKeys;

            BookingPackageQueryService queryService = new BookingPackageQueryService(context);
            entityPM.BookingPackages = queryService.GetMulti(bookingKeys, true);

            BookingAnswerQueryService mBookingAnswerQueryService = new BookingAnswerQueryService(context);
            entityPM.BookingAnswers = mBookingAnswerQueryService.GetMulti(bookingKeys, true);
        }

        public List<BookingChartingClass> GetBookingsDashBoard(int tenant)
        {
            List<BookingChartingClass> myResult = new List<BookingChartingClass>();
            IBookingContext context = MainContext as IBookingContext;

            IQueryable<Booking> dataSourceQuery =
                (from d in context.Bookings
                 where d.Tenant == tenant
                 && !d.IsCancelled
                 select d);

            if (dataSourceQuery != null)
            {
                IQueryable<Booking> data_WAT = dataSourceQuery.Where(d => d.BookingStatusCode == "CRT");
                IQueryable<Booking> data_WAC = dataSourceQuery.Where(d => d.WaitingForResponse);
                IQueryable<Booking> data_CNF = dataSourceQuery.Where(d => d.BookingStatusCode == "CNF");
                IQueryable<Booking> data_ERR = dataSourceQuery.Where(d => d.HasErrors);

                List<BookingChartingClass> myData = new List<BookingChartingClass>();

                myData =
                    (from d in dataSourceQuery
                     group d by new { d.MainCarriageCarrierId, d.MainCarriageCarrier.EnglishName } into g
                     select new BookingChartingClass()
                     {
                         Id = g.Key.MainCarriageCarrierId,
                         StringProperty = g.Key.EnglishName,
                         MainCarriageCarrierId = g.Key.MainCarriageCarrierId,
                         IntegerProperty = g.Count()
                     })
                     .ToList();


                foreach (BookingChartingClass item in myData)
                {
                    myResult.Add(new BookingChartingClass()
                    {
                        Id = item.Id + ":WAT",
                        DataTypeCode = "WAT",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_WAT.Where(d => d.MainCarriageCarrierId == item.Id).Count(),
                        MainCarriageCarrierId = item.Id,
                    });

                    myResult.Add(new BookingChartingClass()
                    {
                        Id = item.Id + ":WAC",
                        DataTypeCode = "WAC",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_WAC.Where(d => d.MainCarriageCarrierId == item.Id).Count(),
                        MainCarriageCarrierId = item.Id,
                    });

                    myResult.Add(new BookingChartingClass()
                    {
                        Id = item.Id + ":CNF",
                        DataTypeCode = "CNF",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_CNF.Where(d => d.MainCarriageCarrierId == item.Id).Count(),
                        MainCarriageCarrierId = item.Id,
                    });

                    myResult.Add(new BookingChartingClass()
                    {
                        Id = item.Id + ":ERR",
                        DataTypeCode = "ERR",
                        StringProperty = item.StringProperty,
                        IntegerProperty = data_ERR.Where(d => d.MainCarriageCarrierId == item.Id).Count(),
                        MainCarriageCarrierId = item.Id,
                    });
                }
            }

            return myResult;
        }
    }
}

	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.BookingLib.Data.Repositories;

namespace Logitude.BookingLib.Data.EntityListQueryServices
{ 

    public partial class BookingAnswerListQueryService
    {
	    private IQueryable<BookingAnswerList> GetIqueryableList(IQueryable<BookingAnswer> iQueryable)
        {
			throw new NotImplementedException();
		}

        public List<BookingAnswerList> GetBookingAnswerListByBookingId(string myBookingId, int tenant)
        {
            List<BookingAnswerList> myResult = new List<BookingAnswerList>();
            BookingAnswerRepository entityRepository = new BookingAnswerRepository(tenant);
            IQueryable<BookingAnswer> entities = entityRepository.GetBookingAnswersForBookingTenant(myBookingId, tenant);

            myResult = (from d in entities
                        select new BookingAnswerList()
                        {
                            Id = d.Id,
                            Tenant = d.Tenant,
                            BookingId = d.BookingId,
                            CarrierId = d.CarrierId,
                            CreateDate = d.CreateDate,
                            BookingSpaceAllocationCode = d.BookingSpaceAllocationCode,
                            CommunicationLogId = d.CommunicationLogId,
                            DescriptionOfGoods = d.DescriptionOfGoods,
                            Destination = d.Destination,
                            ETD = d.ETD,
                            FlightNumber = d.FlightNumber,                            
                            NumberOfPieces = d.NumberOfPieces,
                            Origin = d.Origin,
                            OtherServicesInformation = d.OtherServicesInformation,
                            StatusCode = d.StatusCode,
                            Weight = d.Weight,
                            WeightUnitCode = d.WeightUnitCode,
                            //Master = "",
                        }).ToList();

            return myResult;
        }

		private IQueryable<BookingAnswer> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BookingAnswer> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

		private IQueryable<BookingAnswer> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<BookingAnswer> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}


}
	
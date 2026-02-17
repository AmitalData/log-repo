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

namespace Logitude.BookingLib.Data.EntityListQueryServices
{ 

    public partial class BookingSpaceAllocationListQueryService
    {
        private IQueryable<BookingSpaceAllocationList> GetIqueryableList(IQueryable<BookingSpaceAllocation> iQueryable)
        {
            IQueryable<BookingSpaceAllocationList> query = (from a in iQueryable
                                                            select new BookingSpaceAllocationList()
                                                  {
                                                      Code = a.Code,
                                                      Name = a.Name,
                                                      SearchFields = a.SearchFields,
                                                      IsSelectable = a.IsSelectable,
                                                  });
            return query;
        }

		private IQueryable<BookingSpaceAllocation> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BookingSpaceAllocation> iQueryable)
        {
            return iQueryable;
		}

		private IQueryable<BookingSpaceAllocation> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<BookingSpaceAllocation> iQueryable)
        {
			return iQueryable;
		}
	}


}
	
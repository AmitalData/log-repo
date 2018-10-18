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

    public partial class BookingAnswerStatusListQueryService
    {
	    private IQueryable<BookingAnswerStatusList> GetIqueryableList(IQueryable<BookingAnswerStatus> iQueryable)
        {
            return (from a in iQueryable
                    select new BookingAnswerStatusList()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    });
        }

		private IQueryable<BookingAnswerStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BookingAnswerStatus> iQueryable)
        {
            return iQueryable;

        }

		private IQueryable<BookingAnswerStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<BookingAnswerStatus> iQueryable)
        {
			return iQueryable;
		}
	}


}
	
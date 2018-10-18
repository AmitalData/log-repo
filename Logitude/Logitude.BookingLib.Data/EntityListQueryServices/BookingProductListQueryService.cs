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

    public partial class BookingProductListQueryService
    {
        private IQueryable<BookingProductList> GetIqueryableList(IQueryable<BookingProduct> iQueryable)
        {
            IQueryable<BookingProductList> query = (from a in iQueryable
                                                    select new BookingProductList()
                                                    {

                                                        Id = a.Id,

                                                        Code = a.Code,

                                                        Name = a.Name,

                                                        SearchFields = a.SearchFields,

                                                        AirlineId = a.AirlineId,

                                                        InActive = a.InActive,

                                                    });
            return query;
        }

        private IQueryable<BookingProduct> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BookingProduct> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<BookingProduct> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BookingProduct> iQueryable)
        {
            return iQueryable;
        }

    }


}
	
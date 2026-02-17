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
    public partial class BookingLevelListQueryService
    {   
        private IQueryable<BookingLevelList> GetIqueryableList(IQueryable<BookingLevel> iQueryable)
        {
            IQueryable<BookingLevelList> query = (from a in iQueryable
                                                            select new BookingLevelList()
                                                            {
                                                                Code = a.Code,
                                                                Name = a.Name,
                                                                SearchFields = a.SearchFields,
                                                            });
            return query;
        }

        private IQueryable<BookingLevel> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BookingLevel> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<BookingLevel> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BookingLevel> iQueryable)
        {
            return iQueryable;
        }
	}
}
	
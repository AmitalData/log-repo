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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class TapagConnectionTableListQueryService
    {
	    private IQueryable<TapagConnectionTableList> GetIqueryableList(IQueryable<TapagConnectionTable> iQueryable)
        {
            IQueryable<TapagConnectionTableList> query = (from a in iQueryable
                                                          select new TapagConnectionTableList()
                                                                 {
                                                                     DeclarationId = a.DeclarationId,
                                                                     TapagId = a.TapagId,
                                                                     Tenant = a.Tenant,
                                                                     CustomsNumeral = a.CustomsNumeral,
                                                                     CustomsTapagFile = a.CustomsTapagFile,

                                                                     RequestFileNumber = a.RequestFileNumber,

                                                                 });
            return query;
		}

        private IQueryable<TapagConnectionTable> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TapagConnectionTable> iQueryable, int tenant)
        {
            return iQueryable;
        }

       
	}


}
	
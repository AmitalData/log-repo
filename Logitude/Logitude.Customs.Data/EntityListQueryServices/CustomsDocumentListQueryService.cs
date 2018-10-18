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

    public partial class CustomsDocumentListQueryService
    {
	    private IQueryable<CustomsDocumentList> GetIqueryableList(IQueryable<CustomsDocument> iQueryable)
        {
            IQueryable<CustomsDocumentList> query = (from a in iQueryable
                                                     select new CustomsDocumentList()
                                                        {

                                                            DocumentsFilingId = a.DocumentsFilingId,
                                                            DocumentRemarks = a.DocumentRemarks,
                                                            DocumentStatusCode = a.DocumentStatusCode,
                                                            CustomsDocId=a.CustomsDocId,
                                                            Tenant = a.Tenant,

                                                        });
            return query;
		}

        private IQueryable<CustomsDocument> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsDocument> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	
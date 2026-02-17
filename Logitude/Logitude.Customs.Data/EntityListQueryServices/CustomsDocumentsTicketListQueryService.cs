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

    public partial class CustomsDocumentsTicketListQueryService
    {
	    private IQueryable<CustomsDocumentsTicketList> GetIqueryableList(IQueryable<CustomsDocumentsTicket> iQueryable)
        {
            IQueryable<CustomsDocumentsTicketList> query = (from a in iQueryable
                                                            select new CustomsDocumentsTicketList()
                                                            {
                                                                DocumentsFilingId = a.DocumentsFilingId,
                                                                DocumentTypeCode = a.DocumentTypeCode,
                                                                Id = a.Id,
                                                                Tenant = a.Tenant,
                                                                Remarks = a.Remarks,
                                                                IsSendMandatory = a.IsSendMandatory,
                                                            });
            return query;
        }

        private IQueryable<CustomsDocumentsTicket> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsDocumentsTicket> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }


}
	
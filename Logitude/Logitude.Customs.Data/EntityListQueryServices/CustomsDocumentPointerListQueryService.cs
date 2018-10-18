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

    public partial class CustomsDocumentPointerListQueryService
    {
	    private IQueryable<CustomsDocumentPointerList> GetIqueryableList(IQueryable<CustomsDocumentPointer> iQueryable)
        {
            IQueryable<CustomsDocumentPointerList> query = (from a in iQueryable.Include("CustomDocumentType")
                                                            select new CustomsDocumentPointerList()
                                                     {

                                                         Child1EntityCode = a.Child1EntityCode,
                                                         Child1EntityId = a.Child1EntityId,
                                                         Child2EntityCode = a.Child2EntityCode,
                                                         Child2EntityId = a.Child2EntityId,
                                                         Child3EntityCode = a.Child3EntityCode,
                                                         Child3EntityId = a.Child3EntityId,
                                                         //DocumentTypeCode = a.DocumentTypeCode,
                                                         //DocumentTypeName = a.CustomDocumentType.LocalName,
                                                         Id = a.Id,
                                                         ParentEntityCode = a.ParentEntityCode,
                                                         ParentEntityId = a.ParentEntityId,
                                                         //RequiredDocID = a.RequiredDocID,
                                                         Tenant = a.Tenant,
                                                         CustomsDocumentsTicketId=a.CustomsDocumentsTicketId,
                                                         //UploadApproved = a.UploadApproved,
                                                     });
            return query;
		}

        private IQueryable<CustomsDocumentPointer> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsDocumentPointer> iQueryable, int tenant)
        {
            return iQueryable;
		}

    }


}
	
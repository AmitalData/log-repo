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

    public partial class CustomsRequiredFieldListQueryService
    {
	    private IQueryable<CustomsRequiredFieldList> GetIqueryableList(IQueryable<CustomsRequiredField> iQueryable)
        {
            IQueryable<CustomsRequiredFieldList> query = (from a in iQueryable.Include("ObjectField")
                                                          select new CustomsRequiredFieldList()
                                                             {
                                                                 ObjectFieldName = a.ObjectField.FieldName,
                                                                 ObjectTableId = a.ObjectTableId,
                                                                 Id = a.Id,
                                                                 ObjectfieldId = a.ObjectfieldId,
                                                                 ObjectfieldCode = a.ObjectfieldCode,
                                                                 Tenant = a.Tenant,
                                                                 IsExport=a.IsExport,
                                                                 IsImport=a.IsImport
                                                             });
            return query;
		}

        private IQueryable<CustomsRequiredField> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsRequiredField> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	
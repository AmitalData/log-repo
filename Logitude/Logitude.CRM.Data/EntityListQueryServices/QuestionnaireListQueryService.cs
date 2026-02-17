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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class QuestionnaireListQueryService
    {
	    private IQueryable<QuestionnaireList> GetIqueryableList(IQueryable<Questionnaire> iQueryable)
        {
            IQueryable<QuestionnaireList> query = (from a in iQueryable
                                                   select new QuestionnaireList()
                                              {
                                               Id = a.Id,   
                                              Name = a.Name,
                                              Tenant  = a.Tenant,
                                              CreateDate = a.CreateDate,
                                              CreatedByUserId = a.CreatedByUserId,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                               VersionNumber = a.VersionNumber,
                                               InActive = a.InActive,
                                               SearchFields = a.SearchFields,
                                              });
            return query;
		}

		private IQueryable<Questionnaire> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Questionnaire> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<Questionnaire> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Questionnaire> iQueryable,int tenant)
        {
            return iQueryable;
		}
	}


}
	
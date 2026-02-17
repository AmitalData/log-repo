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

    public partial class QuestionnaireAnswerLineListQueryService
    {
	    private IQueryable<QuestionnaireAnswerLineList> GetIqueryableList(IQueryable<QuestionnaireAnswerLine> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<QuestionnaireAnswerLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuestionnaireAnswerLine> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

		private IQueryable<QuestionnaireAnswerLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuestionnaireAnswerLine> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}


}
	
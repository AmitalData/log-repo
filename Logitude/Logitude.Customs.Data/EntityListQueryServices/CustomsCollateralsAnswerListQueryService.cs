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

    public partial class CustomsCollateralsAnswerListQueryService
    {
	    private IQueryable<CustomsCollateralsAnswerList> GetIqueryableList(IQueryable<CustomsCollateralsAnswer> iQueryable)
        {
            IQueryable<CustomsCollateralsAnswerList> query = (from a in iQueryable
                                                              select new CustomsCollateralsAnswerList()
                                                       {

                                                           Remarks = a.Remarks,
                                                           AllocatedAmount = a.AllocatedAmount,
                                                           AnswerEntityTypeCode = a.AnswerEntityTypeCode,
                                                           AnswerForCollateralStatusCode = a.AnswerForCollateralStatusCode,
                                                           CustomsCollateralId = a.CustomsCollateralId,
                                                           CustomsNumeral = a.CustomsNumeral,
                                                           CustomsTapgFile = a.CustomsTapgFile,
                                                           Errors = a.Errors,
                                                           LineNumber = a.LineNumber,
                                                           Tenant = a.Tenant,
                                                           TapagId = a.TapagId,
                                                           RequestFileAmount = a.RequestFileAmount,
                                                           NewFileRequest = a.NewFileRequest,
                                                           RequestFileTypeCode = a.RequestFileTypeCode,
                                                           IsClosed = a.IsClosed,
                                                           RequestedTapagNumeral = a.RequestedTapagNumeral
                                                       });
            return query;
		}

        private IQueryable<CustomsCollateralsAnswer> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsCollateralsAnswer> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
	}


}
	
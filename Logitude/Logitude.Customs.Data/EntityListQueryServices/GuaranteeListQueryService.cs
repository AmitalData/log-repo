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

    public partial class GuaranteeListQueryService
    {
	    private IQueryable<GuaranteeList> GetIqueryableList(IQueryable<Guarantee> iQueryable)
        {
            IQueryable<GuaranteeList> query = (from a in iQueryable.Include("EntityTypeLookup")
                                               select new GuaranteeList()
                                                     {
                                                       
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                      //   ClientActivityCode = a.ClientActivityCode,
                                                         BirthDate = a.BirthDate,
                                                         BrandNumber = a.BrandNumber,
                                                         CustomEntityNumber = a.CustomEntityNumber,
                                                         CustomEntityTypeCode = a.CustomEntityTypeCode,
                                                         CustomEntityTypeName = a.EntityTypeLookup != null? a.EntityTypeLookup.LocalName: null,
                                                         EngineNumber = a.EngineNumber,
                                                         GuaranteeExternalNumber = a.GuaranteeExternalNumber,
                                                         GuaranteeRequestNumber= a.GuaranteeRequestNumber,
                                                         GuaranteeRequestStatusCode = a.GuaranteeRequestStatusCode,
                                                         GuaranteeValidityDate = a.GuaranteeValidityDate,
                                                         LawyerNumber = a.LawyerNumber,
                                                         MsgID = a.MsgID,
                                                         NumeralRequest = a.NumeralRequest,
                                                         RequestValidityDate = a.RequestValidityDate,
                                                         TapagID = a.TapagID,
                                                         UpdateDate = a.UpdateDate,
                                                         VehicleChassisNumber = a.VehicleChassisNumber,

                                                     });
            return query;
		}

        private IQueryable<Guarantee> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Guarantee> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	
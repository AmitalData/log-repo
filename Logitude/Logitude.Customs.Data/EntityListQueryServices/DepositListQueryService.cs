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

    public partial class DepositListQueryService
    {
	    private IQueryable<DepositList> GetIqueryableList(IQueryable<Deposit> iQueryable)
        {
            IQueryable<DepositList> query = (from a in iQueryable
                                             select new DepositList()
                                                      {
                                                          DepositAmount = a.DepositAmount,
                                                       BirthDate = a.BirthDate,
                                                       DepositEssenceTypeCode = a.DepositEssenceTypeCode,
                                                       DepositValidityDate = a.DepositValidityDate,
                                                       EngineNumber = a.EngineNumber,
                                                       EntityNumber = a.EntityNumber,
                                                       EntityTypeCode = a.EntityTypeCode,
                                                       Id = a.Id,
                                                       LawyerNumber = a.LawyerNumber,
                                                       PaymentNumber = a.PaymentNumber,
                                                     
                                                       RequestValidityDate = a.RequestValidityDate,
                                                       TapagID = a.TapagID,
                                                       TradeMarkNumber = a.TradeMarkNumber,
                                                       VehicleChassisNumber = a.VehicleChassisNumber,
                                                          Tenant = a.Tenant,


                                                      });
            return query;
		}

        private IQueryable<Deposit> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Deposit> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	
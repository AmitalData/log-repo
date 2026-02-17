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

using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityLists;

namespace Logitude.TariffModule.Data.EntityListQueryServices
{

    public partial class TariffSettingListQueryService
    {
        private IQueryable<TariffSettingList> GetIqueryableList(IQueryable<TariffSetting> iQueryable)
        {
            IQueryable<TariffSettingList> query = (from a in iQueryable
                                                   select new TariffSettingList()
                                                   {

                                                       Id = a.Id,
                                                       DefaultWarningPercentage = a.DefaultWarningPercentage,
                                                       Tenant = a.Tenant,
                                                       AirDefaultStepsId = a.AirDefaultStepsId,
                                                       LCLDefaultStepsId = a.LCLDefaultStepsId,
                                                   });
            return query;
        }

        private IQueryable<TariffSetting> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TariffSetting> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<TariffSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<TariffSetting> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	
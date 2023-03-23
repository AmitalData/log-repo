using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.Models;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using System.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CountryCurrencyUpdateService : EntityUpdateService<CountryCurrency, CountryCurrencyPM, EntityPM>
    {

        public void FastDelete(string CountryId, int tenant)
        {
            (Repository as Logitude.Customs.Data.Repsitories.CountryCurrencyRepository).FastDelete(CountryId, tenant);
        }
    }
}

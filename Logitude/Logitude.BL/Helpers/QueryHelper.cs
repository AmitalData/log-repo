using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public static class QueryHelper
    {
        public static void AddPortToSearchFields(ref string mySearchFields, int tenant, string myPortId)
        {
            if (!string.IsNullOrEmpty(myPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, myPortId, true);
                if (myPort != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myPort.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myPort.EnglishName);
                }
            }
        }
        public static void AddFullPortToSearchFields(ref string mySearchFields, int tenant, string myPortId)
        {
            if (!string.IsNullOrEmpty(myPortId))
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, myPortId, true);
                if (myPort != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myPort.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myPort.EnglishName);

                    Country myCountry = CountryRepository.GetSingleCountry(myPort.CountryId, tenant, true);
                    if (myCountry != null)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, myCountry.Code);
                        MethodHelper.AddToSearchFields(ref mySearchFields, myCountry.EnglishName);
                    }
                }
            }
        }

    }
}

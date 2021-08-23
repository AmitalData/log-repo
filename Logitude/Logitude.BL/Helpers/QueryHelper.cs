using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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


        public static void AddCardToSearchFields(ref string mySearchFields,  int tenant , string cardId)
        {
            if (!string.IsNullOrEmpty(cardId))
            {
                Card myCard = CardRepository.GetSingleCard(cardId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
        }





    }
}

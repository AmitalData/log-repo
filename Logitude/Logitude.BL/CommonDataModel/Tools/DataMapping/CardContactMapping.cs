using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CardContactMapping
    {
        public static void MapEntity(CardContactPM cardContactPm, CardContact cardContact, bool isNewState)
        {
            cardContact.CardId = cardContactPm.CardId;
            cardContact.ContactId = cardContactPm.ContactId;
            cardContact.Tenant = cardContactPm.Tenant;
            cardContact.InternetAccess = cardContactPm.InternetAccess;
            cardContact.LastLoginDate = cardContactPm.LastLoginDate;
            cardContact.IsAirExport = cardContactPm.IsAirExport;
            cardContact.IsAirImport = cardContactPm.IsAirImport;
            cardContact.IsInlandExport = cardContactPm.IsInlandExport;
            cardContact.IsInlandImport = cardContactPm.IsInlandImport;
            cardContact.IsOceanExport = cardContactPm.IsOceanExport;
            cardContact.IsOceanImport = cardContactPm.IsOceanImport;
            cardContact.IsAll = cardContactPm.IsAll;
            cardContact.IsInlandDomestic = cardContactPm.IsInlandDomestic;
            cardContact.IsCustomsImport = cardContactPm.IsCustomsImport;
          
        }
    }
}

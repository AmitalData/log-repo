using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class PortMapping
    {
        public static void MapEntity(PortPM portPM, Port port, bool isNewState)
        {
            if (isNewState)
            {
                port.Tenant = portPM.Tenant;
            }

            port.CountryId = portPM.CountryId;
            port.AddedManually = portPM.AddedManually;
            port.Code = portPM.Code;
            port.EnglishName = portPM.EnglishName;
            port.Field1 = portPM.Field1;
            port.Field2 = portPM.Field2;
            port.Field3 = portPM.Field3;
            port.Field4 = portPM.Field4;
            port.Field5 = portPM.Field5;
            port.Field6 = portPM.Field6;
            port.Field7 = portPM.Field7;
            port.Field8 = portPM.Field8;
            port.Field9 = portPM.Field9;
            port.Field10 = portPM.Field10;
            port.InActive = portPM.InActive;
            port.IsAir = portPM.IsAir;
            port.IsInland = portPM.IsInland;
            port.IsOcean = portPM.IsOcean;
            port.Latitude = portPM.Latitude;
            port.LocalName = portPM.LocalName;
            port.Longtitude = portPM.Longtitude;
            port.Notes = portPM.Notes;            
            port.StateId = portPM.StateId;
            port.CombinedCode = portPM.CountryCode+ portPM.Code;
            port.StateName = portPM.StateName;
            port.StateCode = portPM.StateCode;
            port.CountryCode = portPM.CountryCode;
            port.CountryName = portPM.CountryName;
            port.PortTimeZoneCode = portPM.PortTimeZoneCode;

            BuildSearchFields(portPM, port);
        }

        private static void BuildSearchFields(PortPM entityPM, Port entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);            
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);

            if (!string.IsNullOrEmpty(entityPM.CountryId))
            {
                Country myCountry = CountryRepository.GetSingleCountry(entityPM.CountryId, entityPM.Tenant, true);
                if (myCountry != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCountry.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCountry.EnglishName);
                }
            }
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CombinedCode);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
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
    public class TarrifHeaderMapping
    {
        public static void MapEntity(TarrifHeaderPM tarrifHeaderPm, TarrifHeader tarrifHeader, bool isNewState)
        {
            tarrifHeader.CardId = tarrifHeaderPm.CardId;
            tarrifHeader.CreateDate = tarrifHeaderPm.CreateDate;
            tarrifHeader.FromDate = tarrifHeaderPm.FromDate;
            tarrifHeader.InActive = tarrifHeaderPm.InActive;
            tarrifHeader.Notes = tarrifHeaderPm.Notes;
            tarrifHeader.TarrifTypeCode = tarrifHeaderPm.TarrifTypeCode;
            tarrifHeader.ToDate = tarrifHeaderPm.ToDate;
            tarrifHeader.TransitTimeNotes = tarrifHeaderPm.TransitTimeNotes;
            tarrifHeader.Tenant = tarrifHeaderPm.Tenant;
        }
    }
}
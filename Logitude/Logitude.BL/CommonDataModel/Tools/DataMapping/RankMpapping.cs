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
    public class RankMpapping
    {
        public static void MapEntity(RankPM rankPM, Rank rank, bool isNewState)
        {
            rank.Name = rankPM.Name;
            rank.Tenant = rankPM.Tenant;
            rank.Code = rankPM.Code;
            rank.SearchFields = rankPM.Code + "," + rankPM.Name;
        }
    }
}
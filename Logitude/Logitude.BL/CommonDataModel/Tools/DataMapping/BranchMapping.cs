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
    public class BranchMapping
    {
        public static void MapEntity(BranchPM entityPM, Branch poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;
            }

            poco.EnglishName = entityPM.EnglishName;           
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.Notes = entityPM.Notes;                
            poco.Code = entityPM.Code;
            poco.ExternalId = entityPM.ExternalId;
            poco.AddressId = entityPM.AddressId;
            poco.Signature = entityPM.Signature;
            poco.Code = entityPM.Code;
            poco.INTTRAId = entityPM.INTTRAId;
            poco.INTTRAAlias = entityPM.INTTRAAlias;
            poco.INTTRAContactId = entityPM.INTTRAContactId;
			poco.CounterCode = entityPM.CounterCode;

			BuildSearchField(entityPM, poco);
        }

        private static void BuildSearchField(BranchPM entityPM, Branch entityPoco)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;  
        }
    }
}
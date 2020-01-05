using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class AirlineMessagingRuleMapping
    {
        public static void MapEntity(AirlineMessagingRulePM entityPM, AirlineMessagingRule poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Tenant = entityPM.Tenant;
                poco.CreateDate = entityPM.CreateDate;
                poco.CreatedByUserId = entityPM.CreatedByUserId;
            }

            poco.AirlineId = entityPM.AirlineId;
            poco.MessageTypeCode = entityPM.MessageTypeCode;
            poco.RuleFieldId = entityPM.RuleFieldId;
            poco.IsMandatoryForSending = entityPM.IsMandatoryForSending;
            poco.MaxSize = entityPM.MaxSize;
            poco.InActive = entityPM.InActive;
            poco.UpdateDate = entityPM.UpdateDate;
            poco.UpdatedByUserId = entityPM.UpdatedByUserId;
            poco.RuleFieldCode = entityPM.RuleFieldCode;
        }
    }
}

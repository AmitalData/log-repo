using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.BL.Helpers
{
    public static class AutomaticPosting
    {
        public static void CreatePost(string entityId, string objectTableName, string userId, string entityDescription, string myBodyText, bool isAutomatic, int tenant)
        {
            ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
            ObjectTable table = tableRep.GetObjectTableByName(objectTableName, 0, true);

            myBodyText = TruncateLongString(myBodyText, 4000);
            entityDescription = TruncateLongString(entityDescription, 250);

            PostPM entityPm = new PostPM()
            {
                BodyText = myBodyText,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CreatedById = userId,
                Tenant = tenant,
                ObjectTableId = table.Id,
                EntityId = entityId,
                UpdatedByUserId = userId,
                EntityDescription = entityDescription,
                IsAutomatic = isAutomatic,
            };

            ISocialContext socialContext = SocialContext.GetContext(entityPm.Tenant);

            PostUpdateService service = new PostUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            service.Update(entityPm, true);
        }

        public static string TruncateLongString(string str, int maxLength)
        {
            if (!string.IsNullOrEmpty(str))

                return str.Substring(0, Math.Min(str.Length, maxLength));

            else
                return str;
        }
    }
}

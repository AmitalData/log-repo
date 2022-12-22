using Logitude.Infrastructure.BL.EntityQueryServices;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.Controllers.DigitalPortal.Helpers
{
    public class DigitalFieldSecuritesHelper
    {
        public List<DigitalFeildSecurityObject> GitDigitalSecuritesFeilds(string objectTableId, string profileId, int tenant)
        {
            var digitalFieldSecurityQuery = new DigitalFieldSecurityQueryService(tenant);
            var digitalFieldSecurity = digitalFieldSecurityQuery.GetDigitalFieldSecurityQuery(0, objectTableId, profileId);
            var defaultDigitalFieldSecurity = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(digitalFieldSecurity.DefaultSettings);
            var customDigitalFeildSecurityObject = new List<DigitalFeildSecurityObject>();

            if (tenant != 0)
            {
                var customDigitalFieldSecurityList = digitalFieldSecurityQuery.GetDigitalFieldSecurityQuery(tenant, objectTableId, profileId);

                if (customDigitalFieldSecurityList != null)
                {
                    customDigitalFeildSecurityObject = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(customDigitalFieldSecurityList.DefaultSettings);

                    foreach (var item in customDigitalFeildSecurityObject)
                    {
                        var temp = defaultDigitalFieldSecurity.FirstOrDefault(a => a.FieldCode.Equals(item.FieldCode));

                        if (temp != null)
                        {
                            temp.HasPersmission = true;
                        }
                    }
                }
            }

            if (!customDigitalFeildSecurityObject.Any())
            {
                defaultDigitalFieldSecurity.ForEach(a => a.HasPersmission = true);
            }

            return defaultDigitalFieldSecurity;
        }
    }
}
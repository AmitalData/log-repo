 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
    public partial class DigitalProfileRepository : IRepository<DigitalProfile>
    {

        public List<DigitalProfile> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public IQueryable<DigitalProfile> GetDigitalProfiles(int tenant, string profileName = "")
        {
            return context.DigitalProfiles
                          .Where(a => a.Tenant == tenant
                                      && (string.IsNullOrEmpty(profileName)
                                           || a.Name.Equals(profileName)));
        }

        public DigitalProfile GetDigitalProfileByName(int tenant, string profileName)
        {
            return context.DigitalProfiles
                          .Where(a => a.Tenant == tenant && a.Name.Equals(profileName)).FirstOrDefault();
        }
    }
}
   
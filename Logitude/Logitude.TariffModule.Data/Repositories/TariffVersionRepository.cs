 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TariffModule.Data.Repositories
{
   public partial class TariffVersionRepository:IRepository<TariffVersion>
   {

        public List<TariffVersion> GetMulti(EntityKeyFields entityKeys)
        {
            TariffKeys myEntityKeys = entityKeys as TariffKeys;
            return (from a in context.TariffVersions where a.TariffId == myEntityKeys.Id select a).ToList();
        }

    }

}
   
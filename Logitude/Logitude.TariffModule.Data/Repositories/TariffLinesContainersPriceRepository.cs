 
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
   public partial class TariffLinesContainersPriceRepository:IRepository<TariffLinesContainersPrice>
   {
        public List<TariffLinesContainersPrice> GetMulti(EntityKeyFields entityKeys)
        {
            TariffLineKeys myEntityKeys = entityKeys as TariffLineKeys;
            return (from a in context.TariffLinesContainersPrices where a.TariffLineId == myEntityKeys.Id select a).ToList();
        }
    }
}
   
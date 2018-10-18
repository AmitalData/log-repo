using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsCountryQueryService : ICanGetAllClosedTable<CustomsCountryPM>
    {
        public List<CustomsCountryPM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public CustomsCountryPM GetSingleByMalamID(string MalamID)
        {
            var poco = repository.GetSingleByMalamID(MalamID);
            var pm = this.GetEntityPM(poco);
            return pm;
        }
    }

}
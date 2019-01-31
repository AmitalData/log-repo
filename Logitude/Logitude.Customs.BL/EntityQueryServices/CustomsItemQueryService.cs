using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsItemQueryService
    {
        public List<CustomsItemPM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public CustomsItemPM GetCustomsItemByClassificationCode(string classificationCode)
        {
            CustomsItemPM pm = null;
          
            var poco = repository.GetCustomsItemByClassificationCode(classificationCode);

            if (poco != null)
            {
                 pm = this.GetEntityPM(poco);
            }
            return pm;
        }

    }
}

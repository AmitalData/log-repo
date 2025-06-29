using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
     public partial class MeasurmentUnitQueryService
    {
         public MeasurmentUnitPM GetMeasurmentUnitByMalamId
            //(int? malamId)
            (int malamId)
        {
             MeasurmentUnitPM pm = null;
             var poco = repository.GetMeasurementUnitByMalamId(malamId);

             if (poco != null)
             {

                 pm = this.GetEntityPM(poco);
             }
             return pm;
         }

        public IQueryable<MeasurmentUnitPM> GetMeasurmentUnitPMs()
        {
            IQueryable<MeasurmentUnit> pocos = repository.GetAll();
            IQueryable<MeasurmentUnitPM> query = from a in pocos
                                                 select new MeasurmentUnitPM
                                                 {
                                                     Code = a.Code,
                                                     EnglishName = a.EnglishName,
                                                     LocalName = a.LocalName,
                                                     Inactive = a.Inactive,
                                                 };
            return query;
        }


    }
}

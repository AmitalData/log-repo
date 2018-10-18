
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class MeasurmentUnitRepository : IRepository<MeasurmentUnit>
    {

        public List<MeasurmentUnit> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public MeasurmentUnit GetMeasurementUnitByMalamId//(int? malamId)
            (int malamId)
        {
            MeasurmentUnit entity = null;
            var key = "MeasurementUnit," + malamId.ToString();
            entity = CacheManager.GetOrInsertNewObject<MeasurmentUnit>(key,
                () =>
                {
                    var measurmentUnits = (from a in context.MeasurmentUnits
                              where a.MalamId == malamId
                              select a).FirstOrDefault();
                    return measurmentUnits;
                }
                );
            return entity;

            //if (CacheManager.CacheWrapper != null)
            //{

            //    if (CacheManager.CacheWrapper.Get("MeasurementUnit") == null)
            //    {
            //        entity = (from a in context.MeasurmentUnits
            //                  where a.MalamId == malamId
            //                  select a).FirstOrDefault();


            //        if (CacheManager.CacheWrapper.Get("MeasurementUnit") == null && entity != null)
            //        {
            //            CacheManager.CacheWrapper.Insert("MeasurementUnit", entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //        }

            //    }
            //    else
            //    {
            //        entity = (MeasurmentUnit)CacheManager.CacheWrapper.Get("MeasurementUnit");
            //    }

            //}

            //return entity;

        }
    }

}

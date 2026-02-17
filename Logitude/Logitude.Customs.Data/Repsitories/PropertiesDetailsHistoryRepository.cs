
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
   public partial class PropertiesDetailsHistoryRepository:IRepository<PropertiesDetailsHistory>
   {
        
		public List<PropertiesDetailsHistory> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public PropertiesDetailsHistory GetPropertiesDetailsHistoryByCustomsItemIdMostAccurate(string customsItemId)
        {
            var key = "PropertiesDetailsHistory," + customsItemId.ToString();
            var propertiesDetailsHistory = CacheManager.GetOrInsertNewObject<PropertiesDetailsHistory>(key,
                () =>
                {
                    var toDay = DateTime.Now.Date;
                    var list = (from a in context.PropertiesDetailsHistorys
                                where a.CustomsItemID == customsItemId
                                select a).ToList();

                    ////var AccurateRow = list.FirstOrDefault(r => r.StartDate >= toDay && r.EndDate > toDay);
                    ////var AccurateRow = list.FirstOrDefault(r => r.StartDate <= toDay);
                    //var AccurateRow = list.FirstOrDefault(r => r.StartDate <= toDay && r.EndDate > toDay);
                    //if (AccurateRow != null)
                    //{
                    //    return AccurateRow;
                    //}
                    ///var latestRow = list.OrderByDescending(r => r.EndDate).FirstOrDefault();
                   //return latestRow;

                    if (list == null)
                    {
                        return null;
                    }
                    var ValidRows = list.Where(r => r.StartDate <= toDay && r.EndDate > toDay);
                    PropertiesDetailsHistory latestRow;
                    if (ValidRows != null)
                    {
                        latestRow = ValidRows.OrderByDescending(r => r.StartDate).FirstOrDefault();
                    }
                    else
                    {
                        latestRow = list.OrderByDescending(r => r.StartDate).FirstOrDefault();
                    }
                    return latestRow;
                });
            return propertiesDetailsHistory;

            //return (from a in context.PropertiesDetailsHistorys
            //    where a.CustomsItemID == customsItemId
            //    select a).FirstOrDefault();
        }
    }

}
   
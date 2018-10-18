using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsClosedTableQueryService
    {
        public List<CustomsClosedTablePM> GetCustomsClosedTables(int tenant)
        {
            List<CustomsClosedTable> closedTables = repository.GetExistedClosedTables();
            List<CustomsClosedTablePM> closedTablePMs = (from a in closedTables
                                                         select new CustomsClosedTablePM()
                                                         {
                                                             CustomsLocalName = a.CustomsLocalName,
                                                             CustomsName = a.CustomsName,
                                                             DbName = a.DbName,
                                                             Existed = a.Existed,
                                                             Id = a.Id,
                                                             LastUpdateDate = a.LastUpdateDate,
                                                             ObjectTableId = a.ObjectTableId,
                                                             StatusCode = a.StatusCode,

                                                         }).ToList();
            return closedTablePMs;
        }
    }
}

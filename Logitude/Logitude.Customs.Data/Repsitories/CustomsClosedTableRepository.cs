 
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

namespace Logitude.Customs.Data.Repsitories
{
    public partial class CustomsClosedTableRepository : IRepository<CustomsClosedTable>
    {

        public List<CustomsClosedTable> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public List<CustomsClosedTable> GetExistedClosedTables()
        {
            return (from a in context.CustomsClosedTables
                    where a.Existed
                    select a).ToList();
        }
        public CustomsClosedTable  GetCustomsClosedTableByObjectTableId(string ObjectTableId)
        {
            return (from a in context.CustomsClosedTables
                    where a.ObjectTableId == ObjectTableId
                    select a).FirstOrDefault();
        }

        public string GetObjectTableIdById(string id)
        {
            return (from a in context.CustomsClosedTables
                    where a.Id == id
                    select a.ObjectTableId).FirstOrDefault();
        }
    }

}
   
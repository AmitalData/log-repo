using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.ClosedTable
{
    public class StatusFieldTypeDetails : StatusFieldType, Logitude.Customs.Def.ClosedTable.ICloseTable<StatusFieldType, StatusFieldTypeDetails>
    {

        public StatusFieldTypeDetails()
        {
        }
        public StatusFieldTypeDetails(StatusFieldType StatusFieldType)
        {
            this.Code = StatusFieldType.Code;
            this.Name = StatusFieldType.Name;
            this.SearchFields = StatusFieldType.SearchFields;

        }
        public List<StatusFieldTypeDetails> GetAll()
        {
            var all = new List<StatusFieldTypeDetails>() ;

            all.Add(new StatusFieldTypeDetails()
            {
                Code = "1",
                Name = " קוד סטטוס",
                SearchFields = " קוד סטטוס,1",
            });

            all.Add(new StatusFieldTypeDetails()
            {
                Code = "2",
                Name = "תאריך ושעת סטטוס",
                SearchFields = "תאריך ושעת סטטוס,2",
            });

            all.Add(new StatusFieldTypeDetails()
            {
                Code = "3",
                Name = "הערות",
                SearchFields = "הערות,3",
            });

            return all;
        }

        public void MapPoco(StatusFieldType newPoco)
        {
            newPoco.Code = this.Code;
            newPoco.Name = this.Name;
            newPoco.SearchFields = GetSearchFields(this);
        }

        public string GetSearchFields(StatusFieldType rec)
        {
            return String.Concat(rec.Code, ",", rec.Name, ",");
        }
    }
}

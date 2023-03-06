using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.ClosedTable
{
    public class ContainerizationStatusCodeDetails : ContainerizationStatusCode, Logitude.Customs.Def.ClosedTable.ICloseTable<ContainerizationStatusCode, ContainerizationStatusCodeDetails>
    {

        public ContainerizationStatusCodeDetails()
        {
        }
        public ContainerizationStatusCodeDetails(ContainerizationStatusCode containerizationStatusCode)
        {
            this.Code = containerizationStatusCode.Code;
            this.Name = containerizationStatusCode.Name;
            this.SearchFields = containerizationStatusCode.SearchFields;

        }
        public List<ContainerizationStatusCodeDetails> GetAll()
        {
            var all = new List<ContainerizationStatusCodeDetails>() ;
            
            all.Add(new ContainerizationStatusCodeDetails()
            {
                Code = "1",
                Name = "המכלה תקינה, טרם הותרה",
                SearchFields = "המכלה תקינה, טרם הותרה",
            });

            all.Add(new ContainerizationStatusCodeDetails()
            {
                Code = "2",
                Name = "המכלה שגויה",
                SearchFields = "המכלה שגויה",
            });

            all.Add(new ContainerizationStatusCodeDetails()
            {
                Code = "3",
                Name = "המכלה מבוטלת",
                SearchFields = "המכלה מבוטלת",
            });

            all.Add(new ContainerizationStatusCodeDetails()
            {
                Code = "4",
                Name = "המכלה לא נקלטה במכס",
                SearchFields = "המכלה לא נקלטה במכס",
            });

            return all;
        }

        public void MapPoco(ContainerizationStatusCode newPoco)
        {
            newPoco.Code = this.Code;
            newPoco.Name = this.Name;
            newPoco.SearchFields = GetSearchFields(this);
        }

        public string GetSearchFields(ContainerizationStatusCode rec)
        {
            return String.Concat(rec.Code, ",", rec.Name, ",");
        }
    }
}

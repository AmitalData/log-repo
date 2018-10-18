
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.BookingLib.BL.EntityDataMappings
{   
   public partial class BookingProductDataMapping: IMapping<BookingProductPM, BookingProduct>
   {
        public void CustomPMToPOCO(BookingProductPM entityPM, BookingProduct entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert ? true : false);
        }

        public void CustomPOCOToPM(BookingProductPM entityPM, BookingProduct entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(BookingProductPM entityPM, BookingProduct entityPOCO, bool p)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Code : mySearchFields + "," + entityPM.Code;
            }

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Name : mySearchFields + "," + entityPM.Name;
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields; 
        }
   }


}
   
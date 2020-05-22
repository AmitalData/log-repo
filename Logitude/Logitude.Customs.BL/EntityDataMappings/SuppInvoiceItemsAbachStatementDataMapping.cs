
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SuppInvoiceItemsAbachStatementDataMapping: IMapping<SuppInvoiceItemsAbachStatementPM, SuppInvoiceItemsAbachStatement>
   {

        public void CustomPMToPOCO(SuppInvoiceItemsAbachStatementPM entityPM, SuppInvoiceItemsAbachStatement entityPOCO)
        {
            entityPOCO.DeclarationId = entityPM.DeclarationId;
            entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
            entityPOCO.InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber;
            entityPOCO.SequenceNumeric = entityPM.SequenceNumeric;
        }

        public void CustomPOCOToPM(SuppInvoiceItemsAbachStatementPM entityPM, SuppInvoiceItemsAbachStatement entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   
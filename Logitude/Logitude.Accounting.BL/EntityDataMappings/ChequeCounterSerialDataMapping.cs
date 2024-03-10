
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{

    public partial class ChequeCounterSerialDataMapping : IMapping<ChequeCounterSerialPM, ChequeCounterSerial>
    {
        public void CustomPMToPOCO(ChequeCounterSerialPM entityPM, ChequeCounterSerial entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.BankAccountId);
            AddPOCOPropertyName(POCOPropertyNames.ChequeCounterEnd);
            AddPOCOPropertyName(POCOPropertyNames.ChequeCounterBegin);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.SeriesId);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.BankAccountId = entityPM.BankAccountId;
                entityPOCO.ChequeCounterEnd = entityPM.ChequeCounterEnd;
                entityPOCO.ChequeCounterBegin = entityPM.ChequeCounterBegin;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.SeriesId = entityPM.SeriesId;
            }
        }

        public void CustomPOCOToPM(ChequeCounterSerialPM entityPM, ChequeCounterSerial entityPOCO)
        {
            //throw new NotImplementedException();
        }

    }
}
   
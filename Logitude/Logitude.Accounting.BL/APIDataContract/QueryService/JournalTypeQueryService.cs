using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
  public partial  class JournalTypeQueryService
    {
     

        public JournalType GetJournalTypeByCode(string JournalTypeID, int Tenant)
        {
            try
            {


                var temp = query.GetSinglePM(JournalTypeID, Tenant);
                if (temp == null)
                    throw new ApplicationException("JournalType with JournalTypeID " + JournalTypeID + " doesn't exist");

                return JournalTypeDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
    }
}

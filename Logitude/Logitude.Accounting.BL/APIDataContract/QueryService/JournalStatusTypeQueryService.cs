using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
  public partial  class JournalStatusTypeQueryService
    {

        public JournalStatusType GetJournalStatusTypeByCode(string JournalStatusID, int Tenant)
        {
            try
            {


                var temp = query.GetSinglePM(JournalStatusID, Tenant);
                if (temp == null)
                    throw new ApplicationException("JournalStatusType with JournalStatusID " + JournalStatusID + " doesn't exist");

                return JournalStatusTypeDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



    }
}

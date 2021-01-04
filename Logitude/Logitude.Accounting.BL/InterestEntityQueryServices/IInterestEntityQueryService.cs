using Logitude.Accounting.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.InterestEntityQueryServices
{
    public interface IInterestEntityQueryService 
    {
        InterestEntityResult GetInterestEntity(InterestTransactionList interestTransactionLists);
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public static class DbEntityFunctions
    {
        public static DateTime? TruncateTime(DateTime? myArgs)
        {
            DateTime? myResult = null;

            if (myArgs != null)
            {
                myResult = DbFunctions.TruncateTime(myArgs);
            }

            return myResult;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public class FieldDbVaildUtil
    {
        public static void Validate(int? val, int size, string tableField) //tableField ="CUMSHGRPM.QUANTITY"
        {
            if (!val.HasValue || size==null)
            {
                return;
            }
            if (val.ToString().Length > size)
            {
                throw new Exception("FieldDbVaildUtil(): Field " + tableField + " value (" + val.ToString() + ")  is to long. Max length is " + size.ToString() + " digits.");
            }
        }
    }
}

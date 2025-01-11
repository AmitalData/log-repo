using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.DBHelpers
{
    public class StoredProcedureParam
    {
        public string ParamName { get; set; }
        public SqlDbType ParamDBType { get; set; }
        public object Value { get; set; }
        public ParameterDirection Direction { get; set; }
        public int ParamSize { get; set; }
    }
}

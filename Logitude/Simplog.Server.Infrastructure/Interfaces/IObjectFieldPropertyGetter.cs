using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Interfaces
{
    public interface IObjectFieldPropertyGetter
    {
        string GetObjectFieldType(string fieldName ,string objectTableName,int tenant);
    }
}

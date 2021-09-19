using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Models.Api
{
    public class ViewResponse<T>
    {
        public T Result { get; set; }
        public int Count { get; set; }
        public long TookMS { get; set; }
    }
}

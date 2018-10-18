using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.CustomsMessaging.ResponseData
{
    public abstract class ResponseDataBase
    {
        public bool HasException { get; set; }
        public string ExceptionMessage { get; set; }
        public bool Succeeded { get; set; }
       
    }
}

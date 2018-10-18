using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.StorageService
{
    public class Response
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public string InnerErrorMessage { get; set; }

        public object Result { get; set; }
    }
}
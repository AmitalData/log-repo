using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core
{
    public class IntegrationTestException
    {
        public string ErrorType { get; set; }
        public string ShortErrorMessage { get; set; }
        public string ErrorMessage { get; set; }

    }
}

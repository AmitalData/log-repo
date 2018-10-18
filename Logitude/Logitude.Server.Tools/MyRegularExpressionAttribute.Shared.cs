using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    public class MyRegularExpressionAttribute : RegularExpressionAttribute
    {
        public MyRegularExpressionAttribute(string pattern) : base(pattern) { }

        protected new int MatchTimeoutInMilliseconds { get; set; }
    }
}

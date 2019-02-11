using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Security
{
    public class AutenticationException:Exception
    {
		public AutenticationException()
		{
		}

		public AutenticationException(string message)
			: base(message)
		{
		}

		public AutenticationException(string message, Exception inner)
	  : base(message, inner)
		{
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.FTP
{
	public class FTPServiceResponse
	{
		public string HasError { get; set; }
		public string ErrorMessage { get; set; }
		public string LogMessage { get; set; }
	}
}

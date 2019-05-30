using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class AppDataUtil
    {
        private static string _ApplicationPhysicalPath;

        public static void Init(string ApplicationPhysicalPath)
        {
            //HostingEnvironment.ApplicationPhysicalPath
            _ApplicationPhysicalPath = ApplicationPhysicalPath;
        }
        public string GetProdInfo()
        {
            try
            {
                var filePath = Path.Combine(_ApplicationPhysicalPath, @"App_Data\ProductInfo.txt");
                var aa=File.ReadAllText(filePath);
                return aa;
            }
            catch (Exception)
            {

                return "";
            }
        }
    }
}

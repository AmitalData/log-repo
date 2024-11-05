using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.Helpers.CheckHealthHelper;

namespace Logitude.Customs.BL.BL
{
    public class CheckHealthService
    {
        public void CheckHealth()
        {
            try
            {
                CheckHealthRepository checkHealthHelper = new CheckHealthRepository();
                 checkHealthHelper.CheckHealth();
            }
            catch (Exception e)
            {
                throw new Exception("Health check failed", e);
            }
        }
    }
}

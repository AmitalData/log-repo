using Logitude.Extensions;

namespace WebFreight.Web.Controllers.DigitalPortal.Helpers
{
    public class DigitalMasterHouseResponse
    {
        public DigitalMasterHouse Master { get; set; }

        public PagedResult<DigitalMasterHouse> Houses { get; set; }
    }
}

using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data.EntityKeys;
namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class BIReportQueryService: EntityQueryService<BIReport,BIReportKeys,BIReportPM,object,BIReportKeys>
   {
        public bool DoesReportExist(string name, string folderId, int tenant)
        {
            return repository.DoesReportExist(name, folderId, tenant);
        }
   }
}
	 
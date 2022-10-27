using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.Logbox
{
    class CustomerTenantsURLService
    {
        public static string Get()
        {
            IGlobalContext objectContext = GlobalContext.GetContext();
            SettingRepository settingRepository = new SettingRepository(objectContext);
            SettingQuery settingQuery = new SettingQuery(settingRepository);

            return settingQuery.GetSinglePM().CustomerTenantsURL.TrimEnd('/') + "/api/";
        }
    }
}

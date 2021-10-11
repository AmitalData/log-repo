using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using System.Threading;
using Simplog.Data.CommonDataModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.DataContracts;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Resolvers;
using Simplog.Data.Helpers;

namespace WebFreight.Web.Security
{
    public class CargoTrackingSecurityService
    {
        private const int limitMinutes = 1;
        private const int limitCount = 5;
        public static Dictionary<string,List<CargoTrackingEntitySearch>> EntitySearchesDictionary = new Dictionary<string, List<CargoTrackingEntitySearch>>();
        //public static List<CargoTrackingEntitySearch> EntitySearches = new List<CargoTrackingEntitySearch>();
		public static void RecordSearch(int tenant)
		{
            var ip = AuthenticationUtil.GetIP4Address();
            List<CargoTrackingEntitySearch>  ipSearches;
            EntitySearchesDictionary.TryGetValue(ip, out ipSearches);
            if (ipSearches == null)
            {
                ipSearches = new List<CargoTrackingEntitySearch>();
                EntitySearchesDictionary.Add(ip,ipSearches);
            }

            ipSearches.Add(new CargoTrackingEntitySearch()
            {
                IPAddress = ip,
                DateSearched = TenantServerConfigration.GetCurrentDateTime(tenant)
            });
        }
        public static int GetRecentRecords(int tenant)
        {
            var ip = AuthenticationUtil.GetIP4Address();

            List<CargoTrackingEntitySearch> ipSearches;
            EntitySearchesDictionary.TryGetValue(ip, out ipSearches);
            if (ipSearches == null)
                return 0;

            var currentDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            var limitDateTime = currentDateTime.AddMinutes(limitMinutes*-1);

            ipSearches.RemoveAll(s => s.DateSearched < limitDateTime);

            return ipSearches.Count();
        }
        public static bool CheckRequestsLimit(int tenant)
        {
            var count = GetRecentRecords(tenant);
            return count >= limitCount;
        }
    }

    public class CargoTrackingEntitySearch
    {
        public string IPAddress { get; set; }
        public DateTime DateSearched { get; set; }
    }
}

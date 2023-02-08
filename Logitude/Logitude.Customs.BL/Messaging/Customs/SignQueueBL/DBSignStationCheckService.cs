using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
  
    internal class DBSignStationCheckService
    {
        private const bool UseCompanySign_AsPersonalDefault = false;
        private const int CacheTimeoutInMinutes = 2;
        private static DateTime LastRetrievedAt = DateTime.MinValue;
        private static List<SignStation> StationLookup = null;

        

        private static List<SignStation> GetSignStations()
        {
            int tenant = 0;
            if (StationLookup == null || DateTime.Now.Subtract(LastRetrievedAt) > TimeSpan.FromMinutes(CacheTimeoutInMinutes))
            {
                var signStationRepository = new SignStationRepository(tenant);
                StationLookup = signStationRepository.GetAllAvailable(CacheTimeoutInMinutes);
                LastRetrievedAt = DateTime.Now;
            }

            return StationLookup;
        }

        public SignStation GetValidSignStation(int tenant, string personId, bool isPersonalSign)
        {
            string customsAgentId = CustomsSettingQueryService.GetSettingByTenant(tenant).CustomsAgentId;

            
            List<SignStation> signStations = GetSignStations();

            if (UseCompanySign_AsPersonalDefault)//joker !!!-everything gona be all right ?
            {
                return signStations.FirstOrDefault(s => s.IsCompanySignOn && s.CustomsAgentId == customsAgentId);
            }

            return isPersonalSign
                ? signStations.FirstOrDefault(s => s.IsPersonalSignOn && s.CustomsAgentId == customsAgentId && s.PersonId == personId)
                : signStations.FirstOrDefault(s => s.IsCompanySignOn && s.CustomsAgentId == customsAgentId);
        }

        
    }
}

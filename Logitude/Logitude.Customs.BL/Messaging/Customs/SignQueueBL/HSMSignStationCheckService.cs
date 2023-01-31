using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.SignQueueBL
{
    public class HSMSignStationCheckService
    {
        static ConcurrentDictionary<string, HSMCheckResult> _StationLookup = new ConcurrentDictionary<string, HSMCheckResult>();
        public HSMCheckResult CheckIfHSMIsValid(int tenant, bool forcePersonalSign)
        {
            var validStation=_StationLookup.Values
                .Where(r=>r.Tenant== tenant)
                .Where(r=> forcePersonalSign ? r.MySignStationList.IsPersonalDefault : r.MySignStationList.IsCompanySignOn)
                .Where(r=> DateTime.Now.Subtract( r.CheckAt) < TimeSpan.FromMinutes(3))
                .ToList();
            if (validStation.Any())
            {
                //TODO get random

                return validStation.First();
            }


            var signQueueHSMService = new SignQueueHSMService();
            var signStations = signQueueHSMService.GetHSMAllCertificates(tenant, false)
                .Where(r => forcePersonalSign ? r.IsPersonalDefault : r.IsCompanySignOn)
                .ToList();

            if (signStations.Count == 0)
            {
                var text = forcePersonalSign ? "Personal" : "Company";
                return new HSMCheckResult { Success = false, ErrorMessage = $"Must have at least one HSM Sign Station that Sign {text}" };
            }

            var tasks = signStations.Select(r => Task.Run(() =>
            { return CheckOne(r, tenant, forcePersonalSign); })
            ).ToList();

            foreach (var task in tasks)
            {
                task.Wait();
                if (task.Result.Success)
                {
                    return task.Result;
                }
            }

            return new HSMCheckResult() { Success = false, ErrorMessage = "No found valid HSM Sign Station" };
        }

        private HSMCheckResult CheckOne(MySignStationList mySignStationList , int tenant, bool forcePersonalSign)
        {
            HSMCheckResult hSMCheckResult = new HSMCheckResult(mySignStationList, tenant) ;

            try
            {
                var hSMSignFileService = new HSMSignFileService();
                var res=hSMSignFileService.SignCustomsRequest(tenant, "T" + Guid.NewGuid(), mySignStationList.PersonId,
                    forcePersonalSign ? "P" : "C", mySignStationList.CustomsAgentId,
                    UTF8Encoding.UTF8.GetBytes("<r></r>")
                    );
                hSMCheckResult.Success = true;
            }
            catch (Exception ee)
            {

                hSMCheckResult.ErrorMessage = ee.ToString();
                hSMCheckResult.Success = false;

            }


            _StationLookup.AddOrUpdate(
                                hSMCheckResult.MySignStationList.PersonId , hSMCheckResult,
                                (keyToUpdate, existingValue) =>
                                {
                                    return hSMCheckResult;
                                });

            return hSMCheckResult;

        }
    }
    public class HSMCheckResult
    {
        public HSMCheckResult()
        {

        }

        public HSMCheckResult(MySignStationList mySignStationList, int tenant) 
        {
            this.CheckAt = DateTime.Now;
            MySignStationList = mySignStationList;
            Tenant = tenant;
        }

        public bool Success { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public DateTime CheckAt { get; }
        public MySignStationList MySignStationList { get; }
        public int Tenant { get; }
    }

}

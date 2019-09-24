using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.AgentPaymentReplyServiceReference;

namespace WebFreight.Web.CustomWebServices.Testers
{
    public class Tester
    {
        public static void GetSingleDeclarationPMByNumber()
        {
            var myDeclarationRepository = new DeclarationRepository(1);
            var q = myDeclarationRepository.GetSingleDeclarationPMByNumber("xcc", 1);
            var res=q.ToList();

        }

        public static void TestNOWait()
        {
            using (var scope = TransactionFactory.GetNewTransaction())
            {
                //using (var _AmitalContext1 = AmitalContext.GetContext(1))
                var _AmitalContext1 = AmitalContext.GetContext(1);
                {
                    //AmitalContext.SetOracleMonitor();
                    var myCCUFILEMQueryService = new CCUFILEMQueryService(_AmitalContext1);
                    int? FILENO1 = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO_forUpdateNOWAIT(4180411244);
                }
                //using (var _AmitalContext = AmitalContext.GetContext(1))
                var _AmitalContext = AmitalContext.GetContext(1);
                {
                    //AmitalContext.SetOracleMonitor();
                    var myCCUFILEMQueryService = new CCUFILEMQueryService(_AmitalContext);
                    int? FILENO1 = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO_forUpdateNOWAIT(4180411244);
                }
            }
        }

        public static void  DeSerializeObject3052(string customsResponseXml)
        {
            var customsResponse = XmlGenericUtil<TSH_MSG7_AgentPaymentReply>.DeSerializeObject(customsResponseXml);

        }
    }
}
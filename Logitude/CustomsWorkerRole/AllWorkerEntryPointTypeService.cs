using CommunicationWorkerRole;
using System.Collections.Generic;
using System.Linq;

namespace CustomsWorkerRole
{
    public class AllWorkerEntryPointTypeService
    {
        public static List<Logitude.Server.Tools.WorkerEntryPoint> GetAllWorkerEntryPointType()
        {
#if false



            
       

                        
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('SendDataToExternalServicesWR', 'SendDataToExternalServicesWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('SendDataToExternalServicesWR', '0', '1');


            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CommunicationLogWorkerRoleWinService', 'CommunicationLogWorkerRoleWinService');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CommunicationLogWorkerRoleWinService', '0', '1');


            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('DownloadDcaMessageSheetWR', 'DownloadDcaMessageSheetWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('DownloadDcaMessageSheetWR', '0', '1');

            
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsMessagingSheetWR', 'CustomsMessagingSheetWR');
            /
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsMessagingSheetWR', '0', '1');
    
            /

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandGetCustomRequestWR', 'CustomsCommandGetCustomRequestWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandGetCustomRequestWR', '0', '7');

           

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandSendDCAWR', 'CustomsCommandSendDCAWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandSendDCAWR', '0', '7');

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandSendDCAUploadStatusWR', 'CustomsCommandSendDCAUploadStatusWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandSendDCAUploadStatusWR', '0', '7');
            

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandSendWSReceiveCorrelationWR', 'CustomsCommandSendWSReceiveCorrelationWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandSendWSReceiveCorrelationWR', '0', '7');

            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandDownloadDcaReceiveCorrelationWR', 'CustomsCommandDownloadDcaReceiveCorrelationWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandDownloadDcaReceiveCorrelationWR', '0', '7');
            
           
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONS" (CODE, CLASSNAME) VALUES ('CustomsCommandAnalyzeResponseWR', 'CustomsCommandAnalyzeResponseWR');
            INSERT INTO "AMINET_GLOBAL"."BATCHSERVICESDEFINITIONMODS" VALUES ('CustomsCommandAnalyzeResponseWR', '0', '7');



#endif


            return new Logitude.Server.Tools.WorkerEntryPoint[] {
                new   SendDataToExternalServicesWR() ,
                new   CustomsMessagingSheetWR() ,
                new   DownloadDcaMessageSheetWR() ,
                new   CustomsCommandGetCustomRequestWR() ,
                new   CustomsCommandSendDCAWR() ,
                new   CustomsCommandSendDCAUploadStatusWR() ,
                new   CustomsCommandSendWSReceiveCorrelationWR() ,
                new   CustomsCommandDownloadDcaReceiveCorrelationWR() ,
                new   CustomsCommandAnalyzeResponseWR(),
                new IndexSearchWorkerRole()
            }.ToList();
        }
    }
}

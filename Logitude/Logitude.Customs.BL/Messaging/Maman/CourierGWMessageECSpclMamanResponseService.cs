
//https://maman.wsfreeze.co.il/WebAPIExt/Help/Api/POST-api-baldar-CreateECSpclMessgae
//https://docs.google.com/document/d/11_pcjrqx4f8pQBUnz2JMZDhxl4b23dRBRYdl4XKLd-I/edit



using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Utils;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class CourierGWMessageECSpclMamanResponseService : IWebAPIMessage2MamanAnalyzer///using  by SendWEBAPIMessage2MamanWRWR
    {



        public void AnalyzeResponse(Courier2MamanCommSettings settings, string webAPIResultString)
        {

            var responeECSpclMamanData = ProxyUtil.JsonConvertDeserializeTyped<ECSpclMamanMessage>(webAPIResultString);
            
            LogMessagingUtil.Instance.AppendLine($"AnalyzeResponse(ResponseStatusCode={responeECSpclMamanData.ResponseStatusCode},{responeECSpclMamanData.ResponseStatusMsg})");
            var context = CustomContext.GetContext(settings.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            var declarationPM = myDeclarationQueryService.GetSingle(settings.DeclarationId, false, false);
            declarationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;



            switch (responeECSpclMamanData.ResponseStatusCode)
            {
                case 0:
                    {
                        declarationPM.MamanStatusCode = "1";
                    }
                    break;
                case 1:
                    {
                        declarationPM.MamanStatusCode = "2";
                    }
                    break;
                default:
                    declarationPM.MamanStatusCode = responeECSpclMamanData.ResponseStatusCode.ToString();//???        
                    break;
            }


            declarationPM.MamanErrorXml = responeECSpclMamanData.ResponseStatusCode.ToString() + "," + responeECSpclMamanData.ResponseStatusMsg ?? "";

            using (var scope = TransactionFactory.GetNewTransaction())
            {
                var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), settings.Tenant);
                myDeclarationUpdateService.Update(declarationPM, true);
                scope.Complete();
            }
        }

        
    }
   

}

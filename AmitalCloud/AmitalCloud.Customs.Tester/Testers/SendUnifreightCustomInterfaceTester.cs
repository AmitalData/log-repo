using AmitalCloud.Infrastructure.Domain.Enums;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using System.Collections.Generic;

namespace Logitude.CustomsMessaging.Testers
{
    public class SendUnifreightCustomInterfaceTester
    {
        public static void Tester()
        {
            var context = CustomContext.GetContext(1);
            var myQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), 1);
            var declarationPM = myQueryService.GetSingle("1-15", false, false);
            if (declarationPM.TransportModeId == "A")
            {
                declarationPM.TransportModeId = "O";
            }
            else
            {
                declarationPM.TransportModeId = "A";
            }
            declarationPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationUpdateService.Update(declarationPM, true);

        }
    }
}

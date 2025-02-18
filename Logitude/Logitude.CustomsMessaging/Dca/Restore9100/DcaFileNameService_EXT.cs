using Logitude.Customs.BL.Utils;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;

namespace Logitude.CustomsMessaging.Dca.Restore9100
{
    public static class DcaFileNameService_EXT
    {
        public static 
            (InterfaceTenantDefinitionManagementPM messageDCA, DCAFileModel dcaFile) GetInterfaceTenantDefinitionManagementPM(
            this List<InterfaceTenantDefinitionManagementPM> interfaceListDCA ,
            string filename )
        {
            var myFileName = System.IO.Path.GetFileName(filename);
            var dcaFile = DCAFilePraser.GetDCAFileModel(myFileName);
            InterfaceTenantDefinitionManagementPM messageDCA = interfaceListDCA.FirstOrDefault(
rec => IsStart(rec.InterfaceManagement.DcaPrefixName, myFileName) ||
IsStart(rec.InterfaceManagement.DcaPrefixName2, myFileName) ||
IsStart(rec.InterfaceManagement.DcaPrefixName3, myFileName) ||
IsStart(rec.InterfaceManagement.DcaPrefixName4, myFileName)
);
            if (messageDCA==null)
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"file:{myFileName} not exist in Interface library ");
            }
            return (messageDCA, dcaFile);

        }
        //
        public static  
            string GetMainMessagingServiceCode(this List<InterfaceTenantDefinitionManagementPM> interfaceListDCA, InterfaceTenantDefinitionManagementPM messageDCA)
        {
            var currMessagingService = messageDCA.Code;


            var mainOutInterface = interfaceListDCA.FirstOrDefault(rec =>
                rec.InterfaceManagement.ResponseInterfaceCode == messageDCA.InterfaceManagement.Code);
            if (mainOutInterface != null)
            {
                currMessagingService = mainOutInterface.Code;
            }
            else
            {

            }
            return currMessagingService;
        }

        private static bool IsStart(string curDcaPrefixName, string dcaFile)
        {
            if (!String.IsNullOrWhiteSpace(curDcaPrefixName))
            {
                if (dcaFile.StartsWith(curDcaPrefixName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;

        }


        public static List<OutgoingMessageInterface> GetOutgoingMessageInterfaceList(this List<InterfaceTenantDefinitionManagementPM> interfaceListDCA, List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage> newImportMessages)
        {
            var newImportMessagesWithIncludeDcaPrefixName = new List<OutgoingMessageInterface>();
            newImportMessages.ForEach(r =>
            {

                var res = interfaceListDCA.GetInterfaceTenantDefinitionManagementPM(r.Filename);
                if (res.messageDCA != null)
                {
                    newImportMessagesWithIncludeDcaPrefixName.Add(new OutgoingMessageInterface
                    {
                        response = r,
                        messageDCA = res.messageDCA,
                        dcaFile = res.dcaFile


                    });
                }
            });
            return newImportMessagesWithIncludeDcaPrefixName;
        }
    }
    public class OutgoingMessageInterface
    {
        internal NG_9101_MSG_OutgoingMessageResponseOutgoingMessage response;
        internal InterfaceTenantDefinitionManagementPM messageDCA;
        internal DCAFileModel dcaFile;
    }
}

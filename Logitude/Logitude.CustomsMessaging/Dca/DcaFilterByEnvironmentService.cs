using Logitude.BL.Security;
using Logitude.Customs.BL.Utils;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;

namespace Logitude.CustomsMessaging.Dca
{
    public enum DcaFilterByEnvironment
    {
        ImportAndExport,
        Export,
        Import

    }

    public partial class DcaFilterByEnvironmentService
    {
        static Dictionary<int, DcaFilterByEnvironment> dca = new Dictionary<int, DcaFilterByEnvironment>();
#if false
        DcaFilterByEnvironment GetDCAEnvPerTenant_(int tenant)
        {
            //var table = Simplog.Data.InfrastructureModel.Repositories.ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            if (dca.ContainsKey(tenant))
            {
                return dca[tenant];
            }
            //string email = AuthenticationUtil.ResolveUserIdentityName(tenant);
            try
            {
                SecurityUtility.CheckFeature("Customs.Declaration", "IIGEXPORTIMPORTDECLARATION", tenant);
                InjectionUtil.Instance.CheckContactFeature(objectTableName: "Customs.Declaration", featureCode: "IIGEXPORTIMPORTDECLARATION", tenant, email);
                dca[tenant] = DcaFilterByEnvironment.ImportAndExport;
                Logger.LogMe("DcaFilterByEnvironment.ImportAndExport", false, "DcaFilterByEnvironment");
            }
            catch (Logitude.Server.Tools.Helpers.SecurityException)
            {
                try
                {
                    InjectionUtil.Instance.CheckContactFeature(objectTableName: "Customs.Declaration", featureCode: "EXPORTDECLARATIONPSCREEN", tenant, email);
                    dca[tenant] = DcaFilterByEnvironment.Export;
                    Logger.LogMe("DcaFilterByEnvironment.Export", false, "DcaFilterByEnvironment");
                }
                catch (Logitude.Server.Tools.Helpers.SecurityException)
                {

                    dca[tenant] = DcaFilterByEnvironment.Import;
                    Logger.LogMe("DcaFilterByEnvironment.Import", false, "DcaFilterByEnvironment");
                }

            }
            catch (System.Exception ex)
            {
                Logger.LogMe(ex.ToString(), true, "DcaFilterByEnvironment");

                Logger.LogMe(ex.ToString(), false, "DcaFilterByEnvironment");
                Logger.LogMe("DcaFilterByEnvironment.Import  DUE CRASH EROR", false, "DcaFilterByEnvironment");
                dca[tenant] = DcaFilterByEnvironment.Import;

            }

            return dca[tenant];

        }

#endif
        public DcaFilterByEnvironment GetDCAEnvPerTenant(int tenant)
        {
            if (dca.ContainsKey(tenant))
            {
                return dca[tenant];
            }
            //string email = AuthenticationUtil.ResolveUserIdentityName(tenant);
            try
            {
                bool isEI = false;
                bool isE = false;
                bool isI = false;
                if (isEI= SecurityUtility.CheckFeature("Customs.Declaration", "IIGEXPORTIMPORTDECLARATION", tenant) == true)
                {
                    dca[tenant] = DcaFilterByEnvironment.ImportAndExport;
                    Logger.LogMe("DcaFilterByEnvironment.ImportAndExport", false, "DcaFilterByEnvironment");

                }else if (isE= SecurityUtility.CheckFeature("Customs.Declaration", "EXPORTDECLARATIONPSCREEN", tenant) == true)
                {
                    dca[tenant] = DcaFilterByEnvironment.Export;
                    Logger.LogMe("DcaFilterByEnvironment.Export", false, "DcaFilterByEnvironment");
                }
                else
                {
                    isI = true;
                    dca[tenant] = DcaFilterByEnvironment.Import;
                    Logger.LogMe("DcaFilterByEnvironment.Import", false, "DcaFilterByEnvironment");
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogMe(ex.ToString(), true, "DcaFilterByEnvironment");

                Logger.LogMe(ex.ToString(), false, "DcaFilterByEnvironment");
                Logger.LogMe("DcaFilterByEnvironment.Import  DUE CRASH EROR", false, "DcaFilterByEnvironment");
                dca[tenant] = DcaFilterByEnvironment.Import;

            }

            return dca[tenant];

        }

        internal FilterByEnvironmentOutGoingResult FilterByEnvironmentOutGoing(
            int tenant,
            List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage> outgoingMessage
            )
        {
            var sbLocal = new StringBuilder();
            outgoingMessage = outgoingMessage ?? new List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage>();
            int before = outgoingMessage.Count;
            if (before == 0)
            {
                return new FilterByEnvironmentOutGoingResult(outgoingMessage, sbLocal);
            }
            sbLocal.Append($"outgoingMessage_before:{before}");

            switch (GetDCAEnvPerTenant(tenant))
            {
                case DcaFilterByEnvironment.ImportAndExport:
                    sbLocal.Append(";ImportAndExport No Filter");
                    outgoingMessage = outgoingMessage.ToList();
                    break;
                case DcaFilterByEnvironment.Export:
                    sbLocal.Append(";FilterBy:Contains(_EX_)");
                    outgoingMessage = outgoingMessage.Where(r => r.Filename.Contains("_EX_")).ToList();

                    break;
                case DcaFilterByEnvironment.Import:
                    sbLocal.Append(";FilterBy:!!NOT!!!Contains(_EX_)");
                    outgoingMessage = outgoingMessage.Where(r => !r.Filename.Contains("_EX_")).ToList();

                    break;
                default:
                    break;
            }
            int after = outgoingMessage.Count;
            sbLocal.Append($";outgoingMessage_after:{after}");
            if (after == 0)
            {
                Logger.LogMe("after == 0!!!" + sbLocal.ToString(), false, "DcaFilterByEnvironment_Warning");
            }

            return new FilterByEnvironmentOutGoingResult(outgoingMessage, sbLocal);
        }

        internal FilterByEnvironmentListOfDCAFileResult FilterByEnvironmentListOfDCAFile(int tenant, List<DCAFileModel> listOfDCAFile)
        {
            var sbLocal = new StringBuilder();
            listOfDCAFile = listOfDCAFile ?? new List<DCAFileModel>();
            int before = listOfDCAFile.Count;
            if (before == 0)
            {
                return new FilterByEnvironmentListOfDCAFileResult(listOfDCAFile, sbLocal);
            }
            sbLocal.Append($"outgoingMessage_before:{before}");
            switch (GetDCAEnvPerTenant(tenant))
            {
                case DcaFilterByEnvironment.ImportAndExport:
                    sbLocal.Append(";ImportAndExport No Filter");
                    listOfDCAFile = listOfDCAFile.ToList();

                    break;
                case DcaFilterByEnvironment.Export:
                    sbLocal.Append(";FilterBy:Contains(_EX_)");
                    listOfDCAFile = listOfDCAFile.Where(r => r.SelectedFileDownload.Contains("_EX_")).ToList();

                    break;
                case DcaFilterByEnvironment.Import:
                    sbLocal.Append(";FilterBy:!!NOT!!!Contains(_EX_)");
                    listOfDCAFile = listOfDCAFile.Where(r => !r.SelectedFileDownload.Contains("_EX_")).ToList();

                    break;
                default:
                    break;
            }

            int after = listOfDCAFile.Count;
            sbLocal.Append($";outgoingMessage_after:{after}");
            if (after == 0)
            {
                Logger.LogMe("after == 0!!!" + sbLocal.ToString(), false, "DcaFilterByEnvironment_Warning");
            }
            Debug.WriteLine(sbLocal.ToString());
            return new FilterByEnvironmentListOfDCAFileResult(listOfDCAFile, sbLocal);
        }

        
    }

    public class FilterByEnvironmentOutGoingResult
    {
        //private List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage> outgoingMessage;
        //private StringBuilder sbLocal;

        public FilterByEnvironmentOutGoingResult(List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage> outgoingMessage, StringBuilder sbLocal)
        {
            this.OutgoingMessage = outgoingMessage;
            this.SbLocal = sbLocal;
        }

        public List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage> OutgoingMessage { get; }
        public StringBuilder SbLocal { get; }
    }
    public class FilterByEnvironmentListOfDCAFileResult
    {
        public FilterByEnvironmentListOfDCAFileResult(List<DCAFileModel> listOfDCAFile, StringBuilder sbLocal)
        {
            ListOfDCAFile = listOfDCAFile;
            SbLocal = sbLocal;
        }

        public List<DCAFileModel> ListOfDCAFile { get; }
        public StringBuilder SbLocal { get; }
    }
}

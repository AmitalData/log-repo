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
        static int _Write00LogCounter = 11;//force write immdiatly 

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
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo("DcaFilterByEnvironment.ImportAndExport" + ":" + "DcaFilterByEnvironment");

                }else if (isE= SecurityUtility.CheckFeature("Customs.Declaration", "EXPORTDECLARATIONPSCREEN", tenant) == true)
                {
                    dca[tenant] = DcaFilterByEnvironment.Export;
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo("DcaFilterByEnvironment.Export" + ":" + "DcaFilterByEnvironment");
                }
                else
                {
                    isI = true;
                    dca[tenant] = DcaFilterByEnvironment.Import;
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo("DcaFilterByEnvironment.Import" + ":" + "DcaFilterByEnvironment");
                }
            }
            catch (System.Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, "DcaFilterByEnvironment");

                
                
                dca[tenant] = DcaFilterByEnvironment.Import;

            }

            return dca[tenant];

        }

        internal FilterByEnvironmentOutGoingResult FilterByEnvironmentOutGoing(
            int tenant,
            List<NG_9101_MSG_OutgoingMessageResponseOutgoingMessage> outgoingMessage, List<string> PrefixExportEnvironment

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
                    string[] message2Take = { "_EX_", "2791" };
                    outgoingMessage = outgoingMessage.Where(r => message2Take.Any(b => r.Filename.Contains(b)) || PrefixExportEnvironment.Any(prefix => r.Filename.ToUpper().Contains(prefix.ToUpper()))).ToList();

                    break;
                case DcaFilterByEnvironment.Import:
                    sbLocal.Append(";FilterBy:!!NOT!!!Contains(_EX_)");
                    string[] blockedParts = { "_EX_", "2281", "2791" };
                    outgoingMessage = outgoingMessage.Where(r => !blockedParts.Any(b => r.Filename.Contains(b))).ToList();
                    break;
                default:
                    break;
            }
            int after = outgoingMessage.Count;
            sbLocal.Append($";outgoingMessage_after:{after}");
            if (after == 0)
            {
                WriteLogAfter0(tenant, sbLocal);
            }
            else
            {
                var list = outgoingMessage.Select(r => r.Filename).ToList();
                sbLocal.AppendLine(string.Join(Environment.NewLine, list));

                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(sbLocal.ToString()+":"+ $"Tenant_{tenant}_DcaFilterByEnvironment");
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
                WriteLogAfter0(tenant, sbLocal);
            }
            else
            {
                var list=listOfDCAFile.Select(r => r.SelectedFileDownload).ToList();
                sbLocal.AppendLine(string.Join(Environment.NewLine, list));
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo(sbLocal.ToString() + $"Tenant_{tenant}_DcaFilterByEnvironment");
            }
            //Debug.WriteLine(sbLocal.ToString());
            return new FilterByEnvironmentListOfDCAFileResult(listOfDCAFile, sbLocal);
        }

        private static void WriteLogAfter0(int tenant, StringBuilder sbLocal)
        {
            if (_Write00LogCounter > 10)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo("after == 0!!!" + sbLocal.ToString() + $"Tenant_{tenant}_DcaFilterByEnvironment_Warning");
                _Write00LogCounter = 0;
            }
            _Write00LogCounter++;
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

using AmitalCloud.Infrastructure.Domain.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CommunicationLogStep
    {
        [Key]
        public string CommunicationLogId { get; set; }
        [Key]
        public int StepNumber { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public int Retries { get; set; }
        //itzik public string ParamIn1 { get; set; }
        //itzik public string ParamOut1 { get; set; }
        public string Status { get; set; }
        public string Log { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string DocumentId { get; set; }




        [ForeignKey("CommunicationLogId")]
        public CommunicationLog CommunicationLog { get; set; }
        [ForeignKey("Status")]
        public CommunicationStatusType CommunicationStatusType { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
        public bool IsLogCompress { get; set; }
    }

    public static class CommunicationLogStepExt
    {
        public static string LogNormalized(this CommunicationLogStep communicationLogStep)
        {
            if (!communicationLogStep.IsLogCompress)
            {
                return communicationLogStep.Log;
            }
            var decompress = InjectionUtil.Instance.DeCompressText(communicationLogStep.Log);
            return decompress;
        }

        public static string SetASCompressLog(this CommunicationLogStep communicationLogStep, string txt)
        {

            var compressText = InjectionUtil.Instance.CompressText(txt);
            communicationLogStep.Log = compressText;
            communicationLogStep.IsLogCompress = true;
            return compressText;
        }
        public static void Append2Log(this CommunicationLog communicationLog, string AppendString2Log)
        {
            var sb1 = new StringBuilder(communicationLog.Logs);
            sb1.AppendLine(AppendString2Log);
            communicationLog.Logs = sb1.ToString().GetLast((8000 - 1));

        }
        static string GetLast(this string myString, int maxLength)
        {
            myString = myString ?? "";
            var len = myString.Length;
            if (len < maxLength)
            {
                return myString;
            }
            //return _StringBuilder.ToString().Substring(0, maxLength - 1);
            return myString.Substring(len - maxLength);

        }


    }
}

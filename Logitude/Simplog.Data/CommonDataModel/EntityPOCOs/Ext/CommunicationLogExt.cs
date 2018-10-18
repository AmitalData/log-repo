using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public static class CommunicationLogExt
    {
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

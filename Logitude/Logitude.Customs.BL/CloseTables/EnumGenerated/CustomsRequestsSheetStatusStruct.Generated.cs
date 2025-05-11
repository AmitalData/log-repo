

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables
{
    public struct CustomsRequestsSheetStatusValues
    {
        public const string Created = "1";
        public const string SendFailed = "15";
        public const string InProcess = "2";
        public const string Sent = "20";
        public const string Received = "21";
        public const string ReceivedFailed = "22";
        public const string SentResponseWillArriveViaSafe = "23";
        public const string AnalyzeFailed = "25";
        public const string InProcess_3 = "3";
        public const string Analyzed = "30";
        public const string AnalyzeFailed_4 = "4";
        public const string WaitingForSigning = "5";
        public const string Cancelled_6 = "6";
        public const string Cancelled = "99";
    }
 }
 

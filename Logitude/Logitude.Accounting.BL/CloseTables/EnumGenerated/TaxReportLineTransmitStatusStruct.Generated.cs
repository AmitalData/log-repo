

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CloseTables
{
    public struct TaxReportLineTransmitStatusValues
    {
        public const string ForTransmit = "1";
        public const string NotForTransmitForThisReport = "2";
        public const string NotForTransmitAtAll = "3";
        public const string WithoutTransmit = "0";
        public const string TransmitEvenIfDuplicate = "4";

    }
 }
 

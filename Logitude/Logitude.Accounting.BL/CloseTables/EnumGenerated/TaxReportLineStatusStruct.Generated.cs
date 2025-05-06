

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CloseTables
{
    public struct TaxReportLineStatusValues
    {
        public const string MissingVatNo = "1";
        public const string InvoiceNumberIsNotValid = "3";
        public const string InvoiceAmountIsNotValid = "4";
        public const string DuplicateThereIsAnotherTransactionWithTheSameVATNoAndReference = "5";
        public const string ReadyForTransmit = "6";
        public const string WrongVATNumber = "2";
        public const string InvoiceNotPreviouslyReported = "7";
        public const string TheVATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed = "9";
        public const string SmallCashAPinvoiceFromThePreviousMonth = "10";
        public const string MissingConfirmationNumber = "11";
    }
 }
 

using System;

namespace Logitude.FullAccounting.Test.Models
{
    public static class FullAccountingUrls
    {
        public static string PostReturnCheque(PostReturnChequeArgs postReturnChequeArgs)
        {
            return $"BankDeposit/PostReturnCheque?bankDepositId={postReturnChequeArgs.BankDepositId}&arpChequeId={postReturnChequeArgs.ARPChequeId}&returnType={postReturnChequeArgs.ReturnType}&notes={postReturnChequeArgs.Notes}";
        }

    }
}
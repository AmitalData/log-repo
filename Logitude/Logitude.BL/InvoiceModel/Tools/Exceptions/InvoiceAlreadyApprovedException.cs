using System;

namespace Logitude.BL.InvoiceModel.Tools.Exceptions
{
    public class InvoiceAlreadyApprovedException : Exception
    {
        public InvoiceAlreadyApprovedException() { }

        public InvoiceAlreadyApprovedException(string message)
            : base(message) { }
    }
}

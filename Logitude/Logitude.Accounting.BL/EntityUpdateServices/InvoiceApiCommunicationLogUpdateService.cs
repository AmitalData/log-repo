using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Accounting.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
     public partial class InvoiceApiCommunicationLogUpdateService
    {

        public void UpdateCommunicationStatus(InvoiceApiCommunicationLogPM invoiceApiCommunicationLog,int tenant, string step, string status, string exception=null)
        {
            try
            {
                if (invoiceApiCommunicationLog != null && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(step))
                {
                    invoiceApiCommunicationLog.Step = step;
                    invoiceApiCommunicationLog.StatusCode = status;
                    invoiceApiCommunicationLog.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    invoiceApiCommunicationLog.Exception = exception;                           
                    Update(invoiceApiCommunicationLog, true);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

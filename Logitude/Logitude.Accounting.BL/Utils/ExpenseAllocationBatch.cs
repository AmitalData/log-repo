using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Accounting.BL.Utils
{
    public class ExpenseAllocationBatch
    {

        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public ExpenseAllocationBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }


        public void Execute(int tenant)
        {
            IInvoiceContext objectContext = InvoiceContext.GetContext(tenant);

            using (var scope = TransactionFactory.GetNewTransaction())
            {
                var expenseAllocationFlowRepository = new ExpenseAllocationFlowRepository(objectContext);
                var expenseAllocationFlowService = new ExpenseAllocationFlowService(objectContext, tenant);
                var today = DateTime.UtcNow.Date;
                var recordsToProcess = expenseAllocationFlowRepository.GetListByDate(tenant, today).ToList();
                foreach (var record in recordsToProcess)
                {
                    try
                    {
                        expenseAllocationFlowService.RunTaskNow(record);
                        _ResponseText += $"Expense Allocation Flow {record.Id} processed successfully.\n";
                    }
                    catch (Exception ex)
                    {
                        _ResponseText += $"Error processing Expense Allocation Flow {record.Id}: {ex.Message}\n";
                        _StatusCode = HttpStatusCode.InternalServerError;
                    }
                }
                scope.Complete();

            }


        }




    }
}


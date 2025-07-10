
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityLists;
using WebFreight.Web.DataContracts;
using System.IO;
using System.Net.Http.Headers;
using System.Data;
using Simplog.Data.InvoiceModel.Repositories;


namespace WebFreight.Web.Controllers.InvoiceModel.Extended
{
    public class ARInvoiceExtendedController : ApiController
    {
        public HttpResponseMessage GetInvoiceSequenceStatus(string fromDate, string toDate)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                var fromDate1 = Convert.ToDateTime(fromDate);
                var toDate1 = Convert.ToDateTime(toDate);

                ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(tenant);
                List<ARInvoiceList> InvoiceSequence = invoiceQuery.GetInvoiceSequenceStatus(tenant , fromDate1, toDate1);
                ServiceResponse response = new ServiceResponse();
               
                response.Count = InvoiceSequence.Count;
           
                response.Result = InvoiceSequence;
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetInvoiceSequenceStatus2Excel(string fromDate, string toDate, int tenant)
        {
            try
            {
                var result = this.ExportInvoiceSequenceStatusReport(fromDate, toDate, tenant);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new MemoryStream(result));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName =
                    Guid.NewGuid().ToString() + "_ExportInvoiceSequenceStatusReport.xls";
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        

        public byte[] ExportInvoiceSequenceStatusReport(string fromDate, string toDate, int tenant)
        {


            var fromDate1 = Convert.ToDateTime(fromDate);
            var toDate1 = Convert.ToDateTime(toDate);

            ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(tenant);
            List<ARInvoiceList> InvoiceSequence = invoiceQuery.GetInvoiceSequenceStatus(tenant, fromDate1, toDate1);

            DataTable dt = null;
            var settingCol = new BITabularViewSettings() { Columns = new List<Column>() };
            dt = new DataTable("Invoice Sequence");

            settingCol.Columns.Add(new Column() { Index = 1, Code = "InvoiceSeries", Name = "InvoiceSeries", DataTypeCode = "String", Width = 100, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("ARInvoice.F.InvoiceSeries", tenant, true), ColumnName = "InvoiceSeries", DataType = System.Type.GetType("System.String") });

            settingCol.Columns.Add(new Column() { Index = 2, Code = "InvoiceNumberPart", Name = "InvoiceNumberPart", DataTypeCode = "String", Width = 200, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("ARInvoice.F.InvoiceNumberPart", tenant, true), ColumnName = "InvoiceNumberPart", DataType = "".GetType() });

            settingCol.Columns.Add(new Column() { Index = 3, Code = "InvoiceDate", Name = "InvoiceDate", DataTypeCode = "DateTime", Width = 90, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("ARInvoice.F.InvoiceDate", tenant, true), ColumnName = "InvoiceDate", DataType = DateTime.Now.GetType() });

            settingCol.Columns.Add(new Column() { Index = 4, Code = "InvoiceNumber", Name = "InvoiceNumber", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("ARInvoice.O.OriginalInvoiceNumber", tenant, true), ColumnName = "InvoiceNumber", DataType = "".GetType() });

            settingCol.Columns.Add(new Column() { Index = 5, Code = "SequenceStatus", Name = "SequenceStatus", DataTypeCode = "String", Width = 90, });
            dt.Columns.Add(new DataColumn() { Caption = TextCodesTranslator.TranslateText("ARInvoice.F.SequenceStatus", tenant, true), ColumnName = "SequenceStatus", DataType = "".GetType() });


            InvoiceSequence.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.InvoiceSeries;
                newrow[1] = r.InvoiceNumberPart;
                newrow[2] = r.InvoiceDate;
                newrow[3] = r.InvoiceNumber;
                newrow[4] = r.SequenceStatus;

                dt.Rows.Add(newrow);
            });
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, tenant, settingCol);
            return res;
        }
        [HttpPut]
        public HttpResponseMessage UpdateIsApproveDoneInARInvocie(string invoiceId, bool approvalInProgress)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;

                var repo = new ARInvoiceRepository(tenant);
                var invoice = repo.GetSingle(invoiceId, tenant);
                invoice.ApprovalInProgress = approvalInProgress;
                repo.Update(invoice);
                repo.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Field updated successfully.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}
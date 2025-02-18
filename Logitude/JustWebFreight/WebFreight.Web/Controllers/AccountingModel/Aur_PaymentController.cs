using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.Accounting.Def.Validators;
using Logitude.Server.Tools;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Logitude.Server.Tools.Counters;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using System.Linq.Dynamic.Core;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class Aur_PaymentController : ApiController
    {
        [HttpPost]
        [Route("map")]
        public HttpResponseMessage MapAurPayments([FromBody] List<AurPaymentModel> models)
        {
            if (models == null || !models.Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
            }

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];

                if (string.IsNullOrEmpty(token))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token is missing.");
                }

                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (authToken == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid token.");
                }

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;
                var accountingContext = AccountingContext.GetContext(tenant);

                if (models.Any(model => model.paymentItem != null && model.paymentItem.Any()))
                {
                    var aurPayment = models.Select(model => new Aur_PaymentPM
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = tenant,
                        CreateDate = model.paymentItem.FirstOrDefault()?.CreateDate ?? DateTime.MinValue,
                        DraftNumber = model.paymentItem.FirstOrDefault()?.DraftNumber,
                        ForMonth = model.paymentItem.FirstOrDefault()?.ForMonth,
                        SaleOrder = model.paymentItem.FirstOrDefault()?.SaleOrder,
                        Customer = model.paymentItem.FirstOrDefault()?.Customer,
                        CustomerReference1 = model.paymentItem.FirstOrDefault()?.CustomerReference1,
                        CustomerReference2 = model.paymentItem.FirstOrDefault()?.CustomerReference2,
                        InvoiceType = model.paymentItem.FirstOrDefault()?.InvoiceType,
                        PaymentItem = model.paymentItem?.Select(item => new Aur_PaymentItemPM
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Line = item.Line,
                            PaymentSequence = item.PaymentSequence,
                            QuoteId = item.QuoteId,
                            Project = item.Project,
                            ProjectNumber = item.ProjectNumber,
                            SectionType = item.SectionType,
                            BaseAmount = item.BaseAmount
                        }).ToList(),
                        Items = model.ItemPM?.Select(item => new Aur_ItemPM
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Line = item.Line,
                            SalesOrderid = item.SalesOrderid,
                            RelatedContract = item.RelatedContract,
                            ProductNumber = item.ProductNumber,
                            ProductName = item.ProductName,
                            PricePerUnit = item.PricePerUnit,
                            Quantity = item.Quantity,
                            Discount = item.Discount,
                            BaseAmount = item.BaseAmount,
                            Tax = item.Tax,
                            ExtendedAmount = item.ExtendedAmount
                        }).ToList(),
                        TimeSheets = model.timesheetPM?.Select(item => new Aur_TimesheetPM
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Line = item.Line,
                            Name = item.Name,
                            UserReport = item.UserReport,
                            ExecutionDate = item.ExecutionDate,
                            RelatedProject = item.RelatedProject,
                            ProjectNumber = item.ProjectNumber,
                            ApprovesRelatedWork = item.ApprovesRelatedWork,
                            CRMRelatedWork = item.CRMRelatedWork,
                            CRMContactperson = item.CRMContactperson,
                            ConfirmRequestCRM = item.ConfirmRequestCRM,
                            RelatedTask = item.RelatedTask,
                            WorkType = item.WorkType,
                            CompletedEffort = item.CompletedEffort,
                            BillableHours = item.BillableHours,
                            EmployeeType = item.EmployeeType,
                            HourlyRate = item.HourlyRate,
                        }).ToList()
                    }).FirstOrDefault();

                    // Check if a record with the same DraftNumber already exists
                    var existingPayment = accountingContext.Aur_Payments
                        .Any(p => p.DraftNumber == aurPayment.DraftNumber);

                    if (existingPayment)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Conflict, "A record with the same DraftNumber already exists.");
                    }

                    Aur_PaymentUpdateService aurPaymentUpdateService = new Aur_PaymentUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                    aurPaymentUpdateService.Update(aurPayment, true);
                }

                return Request.CreateResponse(HttpStatusCode.Created, "AurPaymentModel mapped and created successfully.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }

public class AurPaymentModel
{
    public List<AurPaymentItem> paymentItem { get; set; }
    public List<Aur_ItemPM> ItemPM { get; set; }
    public List<Aur_TimesheetPM> timesheetPM { get; set; }
}

    public class AurPaymentItem
    {
        //payment
        public DateTime CreateDate { get; set; }
        public string DraftNumber { get; set; }
        public string ForMonth { get; set; }
        public string SaleOrder { get; set; }
        public string Customer { get; set; }
        public string CustomerReference1 { get; set; }
        public string CustomerReference2 { get; set; }
        public string InvoiceType { get; set; }

        //items
        public int Line { get; set; }
        public int? PaymentSequence { get; set; }
        public string QuoteId { get; set; }
        public string Project { get; set; }
        public string ProjectNumber { get; set; }
        public string SectionType { get; set; }
        public decimal BaseAmount { get; set; }
    }

}
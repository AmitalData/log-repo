using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake1DF_NG_2754_MSG10004_ImportDeclarationResponse :FakeResponseBase
    {
        public DF_NG_2754_MSG10004_ImportDeclarationResponse CallWS(
            DF_MSG10000_ImportDeclaration customRequest,
            GenericRequestParams requestParams,
            out string exceptionMessage)
        {
            exceptionMessage = "";
            var response = new DF_NG_2754_MSG10004_ImportDeclarationResponse()
            {
                Response = new Response()
            };
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);
            if (string.IsNullOrWhiteSpace(customRequest.Declaration.ID.Value))
            {
                response.Response.Declaration.ID.Value = customRequest.Declaration.ID.Value;
                response.ResponseContentHeader.ApplicationID = Convert.ToInt32(myQueryService.GetIdByDeclarationNumber(customRequest.Declaration.ID.Value, requestParams.Tenant));
                // 12 => טיוטה שגויה
                response.Response.Status.NameCode.Value = "12";
                response.Response.Status.EffectiveDateTime = DateTime.Now.ToString();

                //Declaration
                response.Response.Declaration.DMExtensions.TaxationDateTime = customRequest.Declaration.DMExtensions.TaxationDateTime;
                response.Response.Declaration.DMExtensions.AgentFileReferenceID = customRequest.Declaration.DMExtensions.AgentFileReferenceID;
                response.Response.Declaration.DMExtensions.ExternalDeclarationID.Value = customRequest.Declaration.DMExtensions.ExternalDeclarationID.Value;
                //response.Response.Declaration.DMExtensions.VersionID=   // ( 0.6 from example)
                response.Response.Declaration.DMExtensions.ExpenseLoadingFactor = customRequest.Declaration.DMExtensions.ExpenseLoadingFactor;
                //  Data is not found on request
                /*response.Response.Declaration.DMExtensions.CustomsValueComponent.TotalDealValueAmountNIS = customRequest.Declaration.DMExtensions.CustomsValueComponent.TotalDealValueAmountNIS;
                 response.Response.Declaration.DMExtensions.CustomsValueComponent.CifValueNIS = customRequest.Declaration.DMExtensions.CustomsValueComponent.CifValueNIS;
                 response.Response.Declaration.DMExtensions.CustomsValueComponent.TaxAssessedAmount.Value = customRequest.Declaration.DMExtensions.CustomsValueComponent.TaxAssessedAmount.Value;
                 response.Response.Declaration.DMExtensions.CustomsValueComponent.TotalMADDealValueAmountNIS.Value = customRequest.Declaration.DMExtensions.CustomsValueComponent.TotalMADDealValueAmountNIS.Value;
                response.Response.Declaration.DMExtensions.CustomsValueComponent.TotalMADDealValueAmountNIS.Value = customRequest.Declaration.DMExtensions.CustomsValueComponent.TotalDealValueAmountNIS.Value;
                */
                response.Response.Declaration.Importer = customRequest.Declaration.Importer;
                response.Response.Declaration.GovernmentProcedure.CurrentCode = customRequest.Declaration.GovernmentProcedure.CurrentCode;
                response.Response.Declaration.GoodsShipment = customRequest.Declaration.GoodsShipment;

                // new dutytaxfee
                //response.Response.Declaration.DutyTaxFee=
                response.Response.Declaration.Agent = customRequest.Declaration.Agent;
                response.Response.Declaration.TypeCode.Value = customRequest.Declaration.TypeCode.Value;
                response.Response.Declaration.DeclarationOfficeID = customRequest.Declaration.DeclarationOfficeID;
                response.Response.Declaration.AcceptanceDateTime = DateTime.Now.ToString();
                response.Response.Declaration.IssueDateTime = DateTime.Now.ToString();



                // Errors
                response.Response.Error[0].ValidationCode.listVersionID = "4";
                response.Response.Error[0].ValidationCode.name = "עבור מטען \"‎1-11111111-114-12345678‬\" טרם התקבל מסר קליטה מאף אתר שבפיקוח המכס\"> 2186";
                response.Response.Error[0].Pointer[0].DocumentSectionCode.Value = "42A";
               //response.Response.Error[0].Pointer[0].DocumentSectionCode.

            }

            switch (requestParams.TestCase.Code)
            {
                case "ConstraintA":
                    {

                    }
                    break;
                default:
                    break;
            }
            return response;

        }

    }
}

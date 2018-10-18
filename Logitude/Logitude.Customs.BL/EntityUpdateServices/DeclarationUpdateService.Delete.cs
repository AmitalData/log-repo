using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Models;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationUpdateService
    {
        private ICustomContext context1;
        private ICustomContext context2;
        private ICustomContext context3;
        private ICustomContext context4;

        

        



        //Delete the declaration including all tables belows
        public void DeleteDeclaration(DeclarationPM MyDeclarationPM)
        {
            context1 = CustomContext.GetContext(ResolvedTenant());
            var myDeclarationQueryService = new DeclarationQueryService(context1);

            //Mark to delete the Declaration
            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
            MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Delete;

            //Mark to delete the Suppliers invoices
            declarationUpdateService.MarkToDeleteSupplierInvoice(MyDeclarationPM);
            ////_sbLog.AppendLine("MarkToDeleteSupplierInvoice:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            //Mark to delete the Consignment
            declarationUpdateService.MarkToDeleteConsignment(MyDeclarationPM);
 
            //Mark to delete Declaration Constraints
            foreach (var constraints in MyDeclarationPM.DeclarationConstraints)
            {
                constraints.ChangeSetOp = ChangeSetOperation.Delete;
            }

            //Mark to delete Payments
            context2 = CustomContext.GetContext(MyDeclarationPM.Tenant);
            var declarationPaymentQueryService = new DeclarationPaymentQueryService(context2);
            var declarationPaymentPM = declarationPaymentQueryService.GetSingle(MyDeclarationPM.Id, true, false);
            MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Delete;
            foreach (var methods in declarationPaymentPM.DeclarationPaymentMethods)
            {
                methods.ChangeSetOp = ChangeSetOperation.Delete;
            }
            foreach (var protests in declarationPaymentPM.DeclarationPaymentProtests)
            {
                protests.ChangeSetOp = ChangeSetOperation.Delete;
            }

            ////Mark to delete CustomsDocumentPointers
            //context3 = CustomContext.GetContext(MyDeclarationPM.Tenant);
            //var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context3);
            //var customsDocumentPointerPM = customsDocumentPointerQueryService.GetSingle(MyDeclarationPM.Id, true, false);
            //customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Delete;
 
            ////Mark to delete CustomsCollateral
            //context4 = CustomContext.GetContext(MyDeclarationPM.Tenant);
            //var customsCollateralQueryService = new CustomsCollateralQueryService(context3);
            //var customsCollateralQueryServicePM = customsCollateralQueryService.GetSingle(MyDeclarationPM.Id, true, false);
            //customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Delete;
            //foreach (var answer in customsCollateralQueryServicePM.CustomsCollateralsAnswers)
            //{
            //    answer.ChangeSetOp = ChangeSetOperation.Delete;
            //}
            //foreach (var condition in customsCollateralQueryServicePM.CustomsCollateralsConditions)
            //{
            //    condition.ChangeSetOp = ChangeSetOperation.Delete;
            //}




            //Update
            declarationUpdateService.Update(MyDeclarationPM, true);
        }

        private int ResolvedTenant()
        {
            if (string.IsNullOrWhiteSpace(this.Tenant.ToString()))
            {
                throw new Exception("Tenant is missing");
            }
            int tenant = 1;
            if (int.TryParse(this.Tenant.ToString(), out tenant))
            {
                return tenant;
            }
            throw new Exception("Tenant (" + this.Tenant.ToString() + ") is not int");
        }
    }
}

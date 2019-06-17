using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
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
        //START - Mark to delete Supplier Invoice including all tables below
        public Boolean markChangeSetOperationDeleteOnly = false;
        public void MarkToDeleteSupplierInvoice(DeclarationPM declarationPM)
        {
            foreach (var si in declarationPM.SupplierInvoices)
            {
                if (markChangeSetOperationDeleteOnly)
                {
                    if(si.ChangeSetOp == ChangeSetOperation.Delete)
                    {
                        MarkToDeleteSupplierInvoiceModifications(si);
                        MarkToDeleteSupplierInvoiceItems(si);
                        MarkToDeleteSupplierInvoiceFreightAmounts(si);
                    }
                }
                else
                {
                    MarkToDeleteSupplierInvoiceModifications(si);
                    MarkToDeleteSupplierInvoiceItems(si);
                    MarkToDeleteSupplierInvoiceFreightAmounts(si);
                    si.ChangeSetOp = ChangeSetOperation.Delete;
                }
            }
        }

        private void MarkToDeleteSupplierInvoiceItems(SupplierInvoicePM supplierInvoice)
        {
            foreach (var sii in supplierInvoice.SupplierInvoiceItems)
            {
                sii.ChangeSetOp = ChangeSetOperation.Delete;
                MarkToDeleteSupplierInvoiceItemsConnectedDeclarations(sii);
                MarkToDeleteSupplierInvoiceItemsDescriptions(sii);
                MarkToDeleteSupplierInvoiceItemsModifications(sii);
                MarkToDeleteSupplierInvoiceItemsProcessTypes(sii);
                MarkToDeleteSupplierInvoiceItemsProductIdentifications(sii);
                MarkToDeleteSupplierInvoiceItemsSerialNumbers(sii);
                MarkToDeleteSupplierInvioceItemsCertificates(sii);
                MarkToDeleteSupplierInvoiceItemsTaxes(sii);

                MarkToDeleteSupplierInvoiceItemsLevys(sii);
                MarkToDeleteSupplierInvoiceItemsVehicles(sii);
                MarkToDeleteSupplierInvoiceItemsModVehicles(sii);

            }
        }

        private void MarkToDeleteSupplierInvoiceItemsModVehicles(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemModVehicles)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemsVehicles(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemVehicles)
            {
                MarkToDeleteSupplierInvoiceItemVehicleMods(item);
                MarkToDeleteSupplierInvoiceItemVehicleAdds(item); // moran 14.3.16 - AMI-55746
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemVehicleMods(SupplierInvoiceItemVehiclePM item)
        {
            foreach (var vehicleMod in item.SupplierInvoiceItemVehicleMods)
            {
                vehicleMod.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemVehicleAdds(SupplierInvoiceItemVehiclePM item) // moran 14.3.16 - AMI-55746
        {
            foreach (var vehicleMod in item.SupplierInvoiceItemVehicleAdds)
            {
                vehicleMod.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemsLevys(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemLevies)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemsTaxes(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemTaxes)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
                //MarkToDeleteSupplierInvoiceItemsTaxesModifications(item);
            }
        }

        //private void MarkToDeleteSupplierInvoiceItemsTaxesModifications(SupplierInvoiceItemsTaxPM siit)
        //{
        //    foreach (var item in siit.SupplierInvoiceItemsTaxesMods)
        //    {
        //        item.ChangeSetOp = ChangeSetOperation.Delete;
        //    }
        //}

        private void MarkToDeleteSupplierInvoiceItemsSerialNumbers(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemsSerialNums)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceFreightAmounts(SupplierInvoicePM supplierInvoice)
        {
            foreach (var item in supplierInvoice.SupplierInvoiceFreightAmounts)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvioceItemsCertificates(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvioceItemCertificats)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }
        private void MarkToDeleteSupplierInvoiceItemsProcessTypes(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemProcesTypes)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemsProductIdentifications(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemsProdIdents)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemsModifications(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemsConDeclars)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemsDescriptions(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemsConDeclars)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceItemsConnectedDeclarations(SupplierInvoiceItemPM sii)
        {
            foreach (var item in sii.SupplierInvoiceItemsConDeclars)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteSupplierInvoiceModifications(SupplierInvoicePM supplierInvoice)
        {
            foreach (var sim in supplierInvoice.SupplierInvoiceModifications) // moran 1.10.15 - Task 17070 - change from items to Modifications
            {
                sim.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }
        //END - Mark to delete Supplier Invoice including all tables below

        //START - Mark to delete Consignment including all tables below
        public void MarkToDeleteConsignment(DeclarationPM declarationPM)
        {
            foreach (var consignment in declarationPM.Consignments)
            {
                MarkToDeleteConsignmentConsignmentPackages(consignment);
                MarkToDeleteConsignmentConsignmentInternalTransitions(consignment);

                consignment.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteConsignmentConsignmentInternalTransitions(ConsignmentPM consignment)
        {
            foreach (var sim in consignment.ConsignmentInternalTransitions)
            {
                sim.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }

        private void MarkToDeleteConsignmentConsignmentPackages(ConsignmentPM consignment)
        {
            foreach (var sim in consignment.ConsignmentPackages)
            {
                sim.ChangeSetOp = ChangeSetOperation.Delete;
            }
        }
        //END - Mark to delete Consignment including all tables below

        public void DeclarationFastDelete(DeclarationPM declarationPM)
        {
            var dbContext = CustomContext.GetContext(ResolvedTenant());
            
            var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            myDeclarationTaxUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var myDeclarationConstraintUpdateService = new DeclarationConstraintUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            myDeclarationConstraintUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            DeclarationConsignmentsFastDelete(declarationPM,dbContext);
            DeclarationSupplierInvoicesFastDelete(declarationPM,dbContext);

            var myDeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            myDeclarationUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as DeclarationRepository).FastDeleteMulti(entityKeyFields);
        }

        public void DeclarationSupplierInvoicesFastDelete(DeclarationPM declarationPM, ICustomContext dbContext)
        {
 	        var mySupplierInvoiceModificationUpdateService = new SupplierInvoiceModificationUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceModificationUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceFreightAmountUpdateService = new SupplierInvoiceFreightAmountUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceFreightAmountUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            DeclarationSupplierInvoiceItemsFastDelete(declarationPM,dbContext);

            var mySupplierInvoiceInvoiceUpdateService = new SupplierInvoiceUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

        }

        public void DeclarationSupplierInvoiceItemsFastDelete(DeclarationPM declarationPM, ICustomContext dbContext)
        {

 	        var mySupplierInvoiceItemsConDeclarUpdateService = new SupplierInvoiceItemsConDeclarUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceItemsConDeclarUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemsDescriptUpdateService = new SupplierInvoiceItemsDescriptUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemsDescriptUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

 	        var mySupplierInvoiceItemsModUpdateService = new SupplierInvoiceItemsModUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceItemsModUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemProcesTypeUpdateService = new SupplierInvoiceItemProcesTypeUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemProcesTypeUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemsProdIdentUpdateService = new SupplierInvoiceItemsProdIdentUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemsProdIdentUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemsSerialNumUpdateService = new SupplierInvoiceItemsSerialNumUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemsSerialNumUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvioceItemCertificatUpdateService = new SupplierInvioceItemCertificatUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvioceItemCertificatUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemsTaxUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemsLevyUpdateService = new SupplierInvoiceItemsLevyUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemsLevyUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemVehicleModUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceItemVehicleAddUpdateService = new SupplierInvoiceItemVehicleAddUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant); // moran 14.3.16 - AMI-55746
            mySupplierInvoiceItemVehicleAddUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemVehicleUpdateService = new SupplierInvoiceItemVehicleUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemVehicleUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceInvoiceItemModVehicleUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var mySupplierInvoiceItemUpdateService = new SupplierInvoiceItemUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            mySupplierInvoiceItemUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });
        }


        public void DeclarationConsignmentsFastDelete(DeclarationPM declarationPM, ICustomContext dbContext)
        {
 	        var myConsignmentPackageUpdateService = new ConsignmentPackageUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            myConsignmentPackageUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var myConsignmentInternalTransitionUpdateService = new ConsignmentInternalTransitionUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            myConsignmentInternalTransitionUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });

            var myConsignmentUpdateService = new ConsignmentUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            myConsignmentUpdateService.FastDeleteComposition(new Data.EntityKeys.DeclarationKeys() { Id = declarationPM.Id });
        }

        public void DeclarationSupplierInvoiceItemsParentsFastDelete(DeclarationPM declarationPM)
        {
            var dbContext = CustomContext.GetContext(ResolvedTenant());

            foreach (var si in declarationPM.SupplierInvoices)
            {
                var mySupplierInvoiceUpdateService = new SupplierInvoiceUpdateService(dbContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
                mySupplierInvoiceUpdateService.DeclarationSupplierInvoiceItemsParentsFastDelete(si, dbContext);
            }
        }
    }
}

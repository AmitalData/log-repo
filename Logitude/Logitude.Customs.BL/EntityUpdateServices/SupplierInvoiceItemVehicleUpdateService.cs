using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Utils;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class SupplierInvoiceItemVehicleUpdateService
    {
        int? maxCounter;
        protected override void OnCreating(SupplierInvoiceItemVehiclePM entityPM, SupplierInvoiceItemPM entityParentPM)
       {

            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.InvoiceItemLineNumber = entityParentPM.LineNumber;
            entityPM.LineNumber = CodeCounter.GetNumber("Customs.SupplierInvoiceItemVehicle", entityPM.Tenant);


            ICustomContext _Context = MainContext as CustomContext;
            SupplierInvoiceItemVehicleQueryService query = new SupplierInvoiceItemVehicleQueryService(_Context);
            if (!maxCounter.HasValue)
            {
                maxCounter = query.GetMaxCounterKey(entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.InvoiceItemLineNumber, entityPM.Tenant);
            }
            entityPM.LineNumber = maxCounter.Value + 1;
            maxCounter = entityPM.LineNumber;


            base.OnCreating(entityPM, entityParentPM);
       }
        // moran 12.10.15 Task 17209 -->
        //protected override void OnUpdating(EntityPMs.SupplierInvoiceItemVehiclePM entityPM)
        //{
        //    base.OnUpdating(entityPM);
        //}

        protected override void OnUpdating(SupplierInvoiceItemVehiclePM entityPM, SupplierInvoiceItemVehicle entityPOCO)
        {
            try
            {


                ICustomContext customContext = this.MainContext as CustomContext;
                VehicleUpdateService vehicleUpdateService = new VehicleUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
                VehicleQueryService vehicleQueryService = new VehicleQueryService(customContext);
                VehiclePM vehiclePM = vehicleQueryService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(entityPM.VehicleChassisNumber, entityPM.RichbitFileNumber, entityPM.Tenant);

                if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                {
                    if (vehiclePM != null)
                    {
                        vehiclePM.DeclarationId = entityPM.DeclarationId;
                        vehiclePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        vehicleUpdateService.Update(vehiclePM, false);
                    }
                }

                if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
                {
                    if (entityPM.VehicleChassisNumber != entityPOCO.VehicleChassisNumber || entityPM.RichbitFileNumber != entityPOCO.RichbitFileNumber) // only if the vehicle chassie number or richbitfilenumber is changed.
                    {
                        VehiclePM oldVehiclePM = vehicleQueryService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(entityPOCO.VehicleChassisNumber, entityPOCO.RichbitFileNumber, entityPOCO.Tenant);
                        if (vehiclePM != null)
                        {
                            if (oldVehiclePM != null && oldVehiclePM.Id != vehiclePM.Id)
                            {
                                oldVehiclePM.DeclarationId = null;//the vehicle disconnected from declaration since the entitypm differs from the poco.
                                oldVehiclePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                vehicleUpdateService.Update(oldVehiclePM, false);
                            }

                            if (oldVehiclePM == null || (oldVehiclePM != null && oldVehiclePM.Id != vehiclePM.Id))
                            {
                                vehiclePM.DeclarationId = entityPM.DeclarationId;
                                vehiclePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                vehicleUpdateService.Update(vehiclePM, false);
                            }
                        }
                        else if(oldVehiclePM != null)
                        {
                            oldVehiclePM.DeclarationId = null;//the vehicle disconnected from declaration since the entitypm differs from the poco.
                            oldVehiclePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                            vehicleUpdateService.Update(oldVehiclePM, false);
                        }

                        DateTime stopLogAt = new DateTime(2020, 06, 01);
                        string logData = "";
                        if (entityPM.RichbitFileNumber != entityPOCO.RichbitFileNumber)
                        {
                            var loggedUser = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                            logData = $"entityPM.RichbitFileNumber(New value)={entityPM.RichbitFileNumber},entityPOCO.RichbitFileNumber(Old value)={entityPOCO.RichbitFileNumber}, User name={loggedUser}";
                            LogitudeSettings.HandleLogMe("RichbitFileNumber changed " + logData, false, "SupplierInvoiceItemUpdate.RichbitFileNumber", stopLogAt);
                        }

                    }
                }

                if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Delete) // this case the vehicle item is deleted , the vehicle declaration id must be cleared
                {
                    if (vehiclePM != null)
                    {
                        vehiclePM.DeclarationId = null;
                        vehiclePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        vehicleUpdateService.Update(vehiclePM, false);
                    }
                }

                base.OnUpdating(entityPM, entityPOCO);
            }
            finally
            {
                var myLogChangesService = new LogChangesService();
                myLogChangesService.
                    LogIt<SupplierInvoiceItemVehiclePM, SupplierInvoiceItemVehicle>("2018062018HD312280.LogUntilDateyyyyMMdd",entityPM, entityPOCO);

            }
        }

        protected override void UpdateComposition(SupplierInvoiceItemVehiclePM entityPM)
       {
           SupplierInvoiceItemVehicleModUpdateService supplierInvoiceItemVehicleModificationUpdateService = new SupplierInvoiceItemVehicleModUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
           supplierInvoiceItemVehicleModificationUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemVehicleMods, entityPM.DeletedSupplierInvoiceItemVehicleMods, entityPM, false);

           SupplierInvoiceItemVehicleAddUpdateService supplierInvoiceItemVehicleAdditionalUpdateService = new SupplierInvoiceItemVehicleAddUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
           supplierInvoiceItemVehicleAdditionalUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemVehicleAdds, entityPM.DeletedSupplierInvoiceItemVehicleAdds, entityPM, false); // moran 14.3.16 - AMI-55746
           
           base.UpdateComposition(entityPM);
       }
        // moran 12.10.15 Task 17209 <--
       public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
       {
           (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemVehicleRepository).FastDeleteMulti(entityKeyFields);
       }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemVehicleRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}

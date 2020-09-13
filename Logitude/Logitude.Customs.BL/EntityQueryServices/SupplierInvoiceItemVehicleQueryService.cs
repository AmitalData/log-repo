using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceItemVehicleQueryService
    {
        // moran 18.10.15 - Task 17209 -->
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, SupplierInvoiceItemVehiclePM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            SupplierInvoiceItemVehicleKeys supplierInvoiceItemVehicleKeys = entityKeys as SupplierInvoiceItemVehicleKeys;

            SupplierInvoiceItemVehicleModQueryService supplierInvoiceItemVehicleModificationQueryService = new SupplierInvoiceItemVehicleModQueryService(context);
            entityPM.SupplierInvoiceItemVehicleMods = supplierInvoiceItemVehicleModificationQueryService.GetMulti(supplierInvoiceItemVehicleKeys, false);

            //SupplierInvoiceItemVehicleAddKeys supplierInvoiceItemVehicleAddKeys = entityKeys as SupplierInvoiceItemVehicleAddKeys;
            SupplierInvoiceItemVehicleAddQueryService supplierInvoiceItemVehicleAdditionalQueryService = new SupplierInvoiceItemVehicleAddQueryService(context); // moran 14.3.16 - AMI-55746
            //entityPM.SupplierInvoiceItemVehicleAdds = supplierInvoiceItemVehicleAdditionalQueryService.GetMulti(supplierInvoiceItemVehicleKeys, false);
            entityPM.SupplierInvoiceItemVehicleAdds = supplierInvoiceItemVehicleAdditionalQueryService.GetMulti(supplierInvoiceItemVehicleKeys, false);

            base.GetComposition(entityKeys, entityPM);
        }
        // moran 18.10.15 - Task 17209 <--

        public List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehiclesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvoiceItemVehicle> supplierInvoiceItemVehicles = repository.GetSupplierInvoiceItemVehiclesForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclePMs = (from a in supplierInvoiceItemVehicles
                                                                                select new SupplierInvoiceItemVehiclePM()
                                                                                {
                                                                                    DeclarationId = a.DeclarationId,
                                                                                    InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                    RichbitFileNumber = a.RichbitFileNumber,
                                                                                    SequenceNumeric = a.SequenceNumeric,
                                                                                    VehicleChassisNumber = a.VehicleChassisNumber,
                                                                                    VehicleId = a.VehicleId,
                                                                                    VehicleTypeCode = a.VehicleTypeCode,
                                                                                    InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                    LineNumber = a.LineNumber,
                                                                                    Tenant = a.Tenant,
                                                                                    ExcludeFromInterface = a.ExcludeFromInterface,
                                                                                    VehicleTypeName = a.VehicleType != null ? a.VehicleType.LocalName:null

                                                                                }).ToList();
            SupplierInvoiceItemVehicleModQueryService supplierInvoiceItemVehicleModificationQueryService = new SupplierInvoiceItemVehicleModQueryService(context);
            SupplierInvoiceItemVehicleAddQueryService supplierInvoiceItemVehicleAdditionalQueryService = new SupplierInvoiceItemVehicleAddQueryService(context); // moran 14.3.16 - AMI-55746
            
            foreach (SupplierInvoiceItemVehiclePM vehicle in supplierInvoiceItemVehiclePMs)
            {
                List<SupplierInvoiceItemVehicleModPM> modifications = supplierInvoiceItemVehicleModificationQueryService.GetSupplierInvoiceItemVehicleModsForSupplierInvoice(declarationId, invoiceCounterKey, vehicle.InvoiceItemLineNumber, tenant);
                vehicle.SupplierInvoiceItemVehicleMods = modifications.Where(d => d.VehicleLineNumber == vehicle.LineNumber).ToList();

                List<SupplierInvoiceItemVehicleAddPM> additionals = supplierInvoiceItemVehicleAdditionalQueryService.GetSupplierInvoiceItemVehicleAddsForSupplierInvoice(declarationId, invoiceCounterKey, vehicle.InvoiceItemLineNumber, tenant);
                vehicle.SupplierInvoiceItemVehicleAdds = additionals.Where(d => d.LineNumber == vehicle.LineNumber).ToList(); // moran 14.3.16 - AMI-55746
            }
            return supplierInvoiceItemVehiclePMs;

        }

        public List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehiclesForSupplierInvoiceItem(string declarationId, int invoiceCounterKey,int lineNumber, int tenant)
        {
            List<SupplierInvoiceItemVehicle> supplierInvoiceItemVehicles = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = declarationId, CounterKey = invoiceCounterKey, LineNumber = lineNumber });
            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclePMs = (from a in supplierInvoiceItemVehicles
                                                                                select new SupplierInvoiceItemVehiclePM()
                                                                                {
                                                                                    DeclarationId = a.DeclarationId,
                                                                                    InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                    RichbitFileNumber = a.RichbitFileNumber,
                                                                                    SequenceNumeric = a.SequenceNumeric,
                                                                                    VehicleChassisNumber = a.VehicleChassisNumber,
                                                                                    VehicleId = a.VehicleId,
                                                                                    VehicleTypeCode = a.VehicleTypeCode,
                                                                                    InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                    LineNumber = a.LineNumber,
                                                                                    Tenant = a.Tenant,
                                                                                    ExcludeFromInterface = a.ExcludeFromInterface,

                                                                                }).ToList();
            SupplierInvoiceItemVehicleModQueryService supplierInvoiceItemVehicleModificationQueryService = new SupplierInvoiceItemVehicleModQueryService(context);
            SupplierInvoiceItemVehicleAddQueryService supplierInvoiceItemVehicleAdditionalQueryService = new SupplierInvoiceItemVehicleAddQueryService(context); // moran 14.3.16 - AMI-55746

            foreach (SupplierInvoiceItemVehiclePM vehicle in supplierInvoiceItemVehiclePMs)
            {
                List<SupplierInvoiceItemVehicleModPM> modifications = supplierInvoiceItemVehicleModificationQueryService.GetSupplierInvoiceItemVehicleModsForSupplierInvoice(declarationId, invoiceCounterKey, vehicle.InvoiceItemLineNumber, tenant);
                vehicle.SupplierInvoiceItemVehicleMods = modifications.Where(d => d.VehicleLineNumber == vehicle.LineNumber).ToList();

                List<SupplierInvoiceItemVehicleAddPM> additionals = supplierInvoiceItemVehicleAdditionalQueryService.GetSupplierInvoiceItemVehicleAddsForSupplierInvoice(declarationId, invoiceCounterKey, vehicle.InvoiceItemLineNumber, tenant);
                vehicle.SupplierInvoiceItemVehicleAdds = additionals.Where(d => d.LineNumber == vehicle.LineNumber).ToList(); // moran 14.3.16 - AMI-55746
            }
            return supplierInvoiceItemVehiclePMs;

        }

        public List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehiclesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvoiceItemVehicle> supplierInvoiceItemVehicles = repository.GetSupplierInvoiceItemVehiclesForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey,itemsLineNumbers, tenant);

            SupplierInvoiceItemVehicleDataMapping mapping = new SupplierInvoiceItemVehicleDataMapping();

            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclePMs = new List<SupplierInvoiceItemVehiclePM>();
            foreach (SupplierInvoiceItemVehicle value in supplierInvoiceItemVehicles)
            {
                SupplierInvoiceItemVehiclePM valuepm = new SupplierInvoiceItemVehiclePM();
                mapping.CustomPOCOToPM(valuepm, value);
                mapping.POCOToPM(valuepm, value);
                supplierInvoiceItemVehiclePMs.Add(valuepm);
            }
            
            //List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclePMs = (from a in supplierInvoiceItemVehicles
            //                                                                    select new SupplierInvoiceItemVehiclePM()
            //                                                                    {
            //                                                                        DeclarationId = a.DeclarationId,
            //                                                                        InvoiceItemLineNumber = a.InvoiceItemLineNumber,
            //                                                                        RichbitFileNumber = a.RichbitFileNumber,
            //                                                                        SequenceNumeric = a.SequenceNumeric,
            //                                                                        VehicleChassisNumber = a.VehicleChassisNumber,
            //                                                                        VehicleId = a.VehicleId,
            //                                                                        VehicleTypeCode = a.VehicleTypeCode,
            //                                                                        InvoiceCounterKey = a.InvoiceCounterKey,
            //                                                                        LineNumber = a.LineNumber,
            //                                                                        Tenant = a.Tenant,
            //                                                                        ExcludeFromInterface = a.ExcludeFromInterface,
            //                                                                    }).ToList();

            SupplierInvoiceItemVehicleModQueryService supplierInvoiceItemVehicleModificationQueryService = new SupplierInvoiceItemVehicleModQueryService(context);
            SupplierInvoiceItemVehicleAddQueryService supplierInvoiceItemVehicleAdditionalQueryService = new SupplierInvoiceItemVehicleAddQueryService(context); // moran 14.3.16 - AMI-55746

            foreach (SupplierInvoiceItemVehiclePM vehicle in supplierInvoiceItemVehiclePMs)
            {
                List<SupplierInvoiceItemVehicleModPM> modifications = supplierInvoiceItemVehicleModificationQueryService.GetSupplierInvoiceItemVehicleModsForSupplierInvoice(declarationId, invoiceCounterKey, vehicle.InvoiceItemLineNumber, tenant);
                vehicle.SupplierInvoiceItemVehicleMods = modifications.Where(d => d.VehicleLineNumber == vehicle.LineNumber).ToList();

                List<SupplierInvoiceItemVehicleAddPM> additionals = supplierInvoiceItemVehicleAdditionalQueryService.GetSupplierInvoiceItemVehicleAddsForSupplierInvoice(declarationId, invoiceCounterKey, vehicle.InvoiceItemLineNumber, tenant);
                vehicle.SupplierInvoiceItemVehicleAdds = additionals.Where(d => d.LineNumber == vehicle.LineNumber).ToList(); // moran 14.3.16 - AMI-55746
            }
            return supplierInvoiceItemVehiclePMs;

        }

        public List<SupplierInvoiceItemVehiclePM> GetAllSupplierInvoiceItemVehiclesForDeclaration(string declarationId, int tenant)
        {
            List<SupplierInvoiceItemVehicle> supplierInvoiceItemVehicles = repository.GetAllSupplierInvoiceItemVehiclesForDeclaration(declarationId, tenant);
            List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclePMs = (from a in supplierInvoiceItemVehicles
                                                                                select new SupplierInvoiceItemVehiclePM()
                                                                                {
                                                                                    DeclarationId = a.DeclarationId,
                                                                                    InvoiceItemLineNumber = a.InvoiceItemLineNumber,
                                                                                    RichbitFileNumber = a.RichbitFileNumber,
                                                                                    SequenceNumeric = a.SequenceNumeric,
                                                                                    VehicleChassisNumber = a.VehicleChassisNumber,
                                                                                    VehicleId = a.VehicleId,
                                                                                    VehicleTypeCode = a.VehicleTypeCode,
                                                                                    InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                    LineNumber = a.LineNumber,
                                                                                    Tenant = a.Tenant,
                                                                                    ExcludeFromInterface = a.ExcludeFromInterface,

                                                                                }).ToList();
            return supplierInvoiceItemVehiclePMs;

        }

        public int? GetMaxCounterKey(string declarationId, int invoiceCounterKey, int invoiceItemLineNum, int tenant)
        {
            return repository.GetMaxCounterKey(declarationId, invoiceCounterKey, invoiceItemLineNum, tenant);
        }


    }
}

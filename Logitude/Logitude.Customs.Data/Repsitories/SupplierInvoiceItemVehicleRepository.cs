
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SupplierInvoiceItemVehicleRepository:IRepository<SupplierInvoiceItemVehicle>
   {
        
		public List<SupplierInvoiceItemVehicle> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceItemKeys supplierInvoiceItemKeys = entityKeys as SupplierInvoiceItemKeys;

            return (from a in context.SupplierInvoiceItemVehicles
                    where a.DeclarationId == supplierInvoiceItemKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceItemKeys.CounterKey && a.InvoiceItemLineNumber == supplierInvoiceItemKeys.LineNumber
                    select a).ToList();
        }

        public List<SupplierInvoiceItemVehicle> GetSupplierInvoiceItemVehiclesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            //return (from a in context.SupplierInvoiceItemVehicles
            //        where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
            //        select a).ToList();

            var q = (from a in context.SupplierInvoiceItemVehicles.Include("VehicleType")
                     where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
                     select a);
            if (FilterLine != null)
            {
                q = q.Where(r => FilterLine.Contains(r.InvoiceItemLineNumber));
            }
            return q.ToList();
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemVehicle>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        public List<SupplierInvoiceItemVehicle> GetSupplierInvoiceItemVehiclesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            return (from a in context.SupplierInvoiceItemVehicles
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey&& itemsLineNumbers.Contains(a.InvoiceItemLineNumber)
                    select a).OrderBy(d=>d.SequenceNumeric).ToList();
        }

        public List<SupplierInvoiceItemVehicle> GetAllSupplierInvoiceItemVehiclesForDeclaration(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoiceItemVehicles
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.VehicleId != null
                    select a).ToList();
        }

        public List<DeclarationVehicleModification> GetDeclarationVehicleModifications(string declarationId, string chassisNumber, string adjustmentType, int tenant)
        {
            IQueryable<SupplierInvoiceItemVehicle> vehicles;
            IQueryable<SupplierInvoiceItemVehicleMod> vehicleMods;
            if (!string.IsNullOrEmpty(chassisNumber))
            {
                vehicles = (from a in context.SupplierInvoiceItemVehicles
                            where a.DeclarationId == declarationId && a.VehicleChassisNumber == chassisNumber && a.Tenant == tenant
                            select a);
            }

            else
            {
                vehicles = (from a in context.SupplierInvoiceItemVehicles
                            where a.DeclarationId == declarationId && a.Tenant == tenant
                            select a);
            }

            if (!string.IsNullOrEmpty(adjustmentType))
            {
                vehicleMods = (from a in context.SupplierInvoiceItemVehicleMods
                               where a.DeclarationId == declarationId && a.AdjustmentTypeCode == adjustmentType && a.Tenant == tenant
                               select a);
            }

            else
            {
                vehicleMods = (from a in context.SupplierInvoiceItemVehicleMods
                               where a.DeclarationId == declarationId && a.Tenant == tenant
                               select a);
            }

            int count = 0;
            List<DeclarationVehicleModification> declarationVehicleMods = (from m in vehicleMods
                                                                           join a in vehicles
                                                                           on new { p1 = m.DeclarationId, p2 = m.InvoiceCounterKey, p3 = m.InvoiceItemLineNumber, p4 = m.VehicleLineNumber } equals new { p1 = a.DeclarationId, p2 = a.InvoiceCounterKey, p3 = a.InvoiceItemLineNumber, p4 = a.LineNumber }


                                                                           select new DeclarationVehicleModification()
                                                                           {
                                                                               AdjustmentType = m.AdjustmentTypeCode,
                                                                                DeductAmount = m.DeductAmount,
                                                                               ChassisNumber = a.VehicleChassisNumber,
                                                                               VehicleNumber = a.RichbitFileNumber,

                                                                              Id= Guid.NewGuid(),
                                                                           }).ToList();




            return declarationVehicleMods;

            
          

        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemVehicle>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.InvoiceCounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.InvoiceItemLineNumber));

        }

        public int? GetMaxCounterKey(string declarationId, int invoiceCounterKey, int invoiceItemLineNum, int tenant)
        {
            return (from a in context.SupplierInvoiceItemVehicles
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && a.InvoiceItemLineNumber == invoiceItemLineNum && a.Tenant == tenant
                    select a).Max(d => (int?)d.LineNumber) ?? 0;
        }

        public List<SupplierInvoiceItemVehicle> GetSupplierInvoiceItemVehiclesByRichbitNumbers(string[] richbitFileNumbers, int tenant)
        {
           List<SupplierInvoiceItemVehicle> vehicles = (from a in context.SupplierInvoiceItemVehicles
                                                  join d in context.Declarations on a.DeclarationId equals d.Id
                                                  where richbitFileNumbers.Contains(a.RichbitFileNumber) && a.Tenant == tenant && d.IsCancelled == false
                                                  select a).ToList();
            return vehicles;

            //return (from a in context.SupplierInvoiceItemVehicles
            //        where richbitFileNumbers.Contains(a.RichbitFileNumber) && a.Tenant == tenant
            //        select a).ToList();
        }

        public SupplierInvoiceItemVehicle GetSupplierInvoiceItemVehicleByRichbitNumber(string richbitFileNumber, string declarationId, int tenant)
        {
            SupplierInvoiceItemVehicle vehicle=(from a in context.SupplierInvoiceItemVehicles
                    join d in context.Declarations on a.DeclarationId equals d.Id
                    where richbitFileNumber == a.RichbitFileNumber 
                    && a.Tenant == tenant 
                    && d.IsCancelled == false 
                    && a.DeclarationId == declarationId
                                                select a).FirstOrDefault();
            return vehicle;
        }


    }

}
   
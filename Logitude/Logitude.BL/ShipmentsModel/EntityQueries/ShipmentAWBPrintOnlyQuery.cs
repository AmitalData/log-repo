using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentAWBPrintOnlyQuery
    {
        ShipmentAWBPrintOnlyRepository repository;
         
        public ShipmentAWBPrintOnlyQuery(int tenant)
        {
            repository = new ShipmentAWBPrintOnlyRepository(tenant);
        }

        public ShipmentAWBPrintOnlyQuery(ShipmentAWBPrintOnlyRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentAWBPrintOnlyPM GetSingleShipmentAWBPrintOnlyPM(string id)
        {
            ShipmentAWBPrintOnly a = repository.context.ShipmentAWBPrintOnlies.Include("DueType").Include("Currency").Include("IATACode").Include("Measurement").Where(d => d.Id == id).FirstOrDefault();
            ShipmentAWBPrintOnlyPM pm = null;
            if (a != null)
            {
                pm = new ShipmentAWBPrintOnlyPM()
                {
                    Amount = a.Amount,
                    IATACodeId = a.IATACodeId,
                    MeasurementId = a.MeasurementId,
                    CurrencyId = a.CurrencyId,
                    DueTypeCode = a.DueTypeCode,
                    ExchangeRate = a.ExchangeRate,
                    Id = a.Id,
                    PrepaidCollectId = a.PrepaidCollectId,
                    Quantity = a.Quantity,
                    ShipmentId = a.ShipmentId,
                    Tenant = a.Tenant,
                    UnitPrice = a.UnitPrice,                     
                    DueTypeName = a.DueType.Name,
                    CurrencyCode = a.Currency.Code,
                    IATACodeName = a.IATACode.Name,
                    MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                };
            }

            return pm;
        }

        public IQueryable<ShipmentAWBPrintOnlyPM> GetShipmentAWBPrintOnlyPMsByTenant(int tenant)
        {
            List<ShipmentAWBPrintOnly> pocOs = repository.context.ShipmentAWBPrintOnlies.Include("DueType").Include("Currency").Include("IATACode").Include("Measurement").Where(d => d.Tenant == tenant).ToList();
            List<ShipmentAWBPrintOnlyPM> result = new List<ShipmentAWBPrintOnlyPM>();

            foreach (ShipmentAWBPrintOnly a in pocOs)
            {
                ShipmentAWBPrintOnlyPM pm = new ShipmentAWBPrintOnlyPM()
                {
                    Amount = a.Amount,
                    CurrencyId = a.CurrencyId,
                    IATACodeId = a.IATACodeId,
                    MeasurementId = a.MeasurementId,
                    DueTypeCode = a.DueTypeCode,
                    ExchangeRate = a.ExchangeRate,
                    Id = a.Id,
                    PrepaidCollectId = a.PrepaidCollectId,
                    Quantity = a.Quantity,
                    ShipmentId = a.ShipmentId,
                    Tenant = a.Tenant,
                    UnitPrice = a.UnitPrice,
                    DueTypeName = a.DueType.Name,
                    CurrencyCode = a.Currency.Code,
                    IATACodeName = a.IATACode.Name,
                    MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                };

                result.Add(pm);
            }

            return result.AsQueryable<ShipmentAWBPrintOnlyPM>();
        }

        public List<ShipmentAWBPrintOnlyPM> GetShipmentAWBPrintOnlyPMsByShipment(string shipmentId, int tenant)
        {
            List<ShipmentAWBPrintOnlyPM> result = (from a in repository.context.ShipmentAWBPrintOnlies.Include("DueType").Include("Currency").Include("IATACode").Include("Measurement")
                                                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                   select new ShipmentAWBPrintOnlyPM()
                                                   {
                                                       Amount = a.Amount,
                                                       CurrencyId = a.CurrencyId,
                                                       IATACodeId = a.IATACodeId,
                                                       MeasurementId = a.MeasurementId,
                                                       DueTypeCode = a.DueTypeCode,
                                                       ExchangeRate = a.ExchangeRate,
                                                       Id = a.Id,
                                                       PrepaidCollectId = a.PrepaidCollectId,
                                                       Quantity = a.Quantity,
                                                       ShipmentId = a.ShipmentId,
                                                       Tenant = a.Tenant,
                                                       UnitPrice = a.UnitPrice,
                                                       DueTypeName = a.DueType.Name,
                                                       CurrencyCode = a.Currency.Code,
                                                       IATACodeName = a.IATACode.Name,
                                                       MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                                                   }).ToList();


            return result;
        }
    }
}
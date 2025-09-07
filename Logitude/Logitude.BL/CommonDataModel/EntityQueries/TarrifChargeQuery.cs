using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TarrifChargeQuery
    {
        TarrifChargeRepository repository;



        public TarrifChargeQuery(int tenant)
        {
            repository = new TarrifChargeRepository(tenant);
        }

        public TarrifChargeQuery(TarrifChargeRepository repository)
        {
            this.repository = repository;
        }

        public TarrifChargePM GetSingleTarrifChargePM(string id)
        {
            TarrifCharge a = repository.context.TarrifCharges.Include("ChargesType").Include("Currency").Include("Measurement").Where(d => d.Id == id).FirstOrDefault();
            TarrifChargePM pm = new TarrifChargePM()
            {
                Id = a.Id,
                ChargesTypeId = a.ChargesTypeId,
                TarrifHeaderId = a.TarrifHeaderId,
                Tenant = a.Tenant,
                CurrencyId = a.CurrencyId,
                MaxPrice = a.MaxPrice,
                MeasurementId = a.MeasurementId,
                MinPrice = a.MinPrice,
                UnitPrice = a.UnitPrice,
                ChargesTypeCode = a.ChargesType == null ? null : a.ChargesType.Code,
                ChargesTypeName = a.ChargesType == null ? null : a.ChargesType.EnglishName,
                CurrencyCode = a.Currency == null ? null : a.Currency.Code,
                MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                ChargesTypeString = a.ChargesType == null ? null : "(" + a.ChargesType.Code + ") " + a.ChargesType.EnglishName,
            };

            return pm;
        }


        public TarrifChargePM GetSinglePM(string id,int tenant = 0)
        {
            TarrifCharge a = repository.context.TarrifCharges.Include("ChargesType").Include("Currency").Include("Measurement").Where(d => d.Id == id).FirstOrDefault();
            TarrifChargePM pm = new TarrifChargePM()
            {
                Id = a.Id,
                ChargesTypeId = a.ChargesTypeId,
                TarrifHeaderId = a.TarrifHeaderId,
                Tenant = a.Tenant,
                CurrencyId = a.CurrencyId,
                MaxPrice = a.MaxPrice,
                MeasurementId = a.MeasurementId,
                MinPrice = a.MinPrice,
                UnitPrice = a.UnitPrice,
                ChargesTypeCode = a.ChargesType == null ? null : a.ChargesType.Code,
                ChargesTypeName = a.ChargesType == null ? null : a.ChargesType.EnglishName,
                CurrencyCode = a.Currency == null ? null : a.Currency.Code,
                MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                ChargesTypeString = a.ChargesType == null ? null : "(" + a.ChargesType.Code + ") " + a.ChargesType.EnglishName,
            };

            return pm;
        }


        public IQueryable<TarrifChargePM> GetTarrifChargePMsByTenant(int tenant)
        {
            return from a in repository.context.TarrifCharges.Include("ChargesType").Include("Currency").Include("Measurement")
                   where a.Tenant == tenant
                   select new TarrifChargePM()
                   {
                       Id = a.Id,
                       ChargesTypeId = a.ChargesTypeId,
                       TarrifHeaderId = a.TarrifHeaderId,
                       Tenant = a.Tenant,
                       CurrencyId = a.CurrencyId,
                       MaxPrice = a.MaxPrice,
                       MeasurementId = a.MeasurementId,
                       MinPrice = a.MinPrice,
                       UnitPrice = a.UnitPrice,
                       ChargesTypeCode = a.ChargesType == null ? null : a.ChargesType.Code,
                       ChargesTypeName = a.ChargesType == null ? null : a.ChargesType.EnglishName,
                       CurrencyCode = a.Currency == null ? null : a.Currency.Code,
                       MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                       ChargesTypeString = a.ChargesType == null ? null : "(" + a.ChargesType.Code + ") " + a.ChargesType.EnglishName,
                   };
        }
        
        public IQueryable<TarrifChargePM> GetTarrifChargesByTarrifHeaderId(string tarrifHeaderId, int tenant)
        {
            return from a in repository.context.TarrifCharges.Include("ChargesType").Include("Currency").Include("Measurement")
                   where a.Tenant == tenant
                   && a.TarrifHeaderId == tarrifHeaderId
                   select new TarrifChargePM()
                   {
                       Id = a.Id,
                       ChargesTypeId = a.ChargesTypeId,
                       TarrifHeaderId = a.TarrifHeaderId,
                       Tenant = a.Tenant,
                       CurrencyId = a.CurrencyId,
                       MaxPrice = a.MaxPrice,
                       MeasurementId = a.MeasurementId,
                       MinPrice = a.MinPrice,
                       UnitPrice = a.UnitPrice,
                       ChargesTypeCode = a.ChargesType == null ? null : a.ChargesType.Code,
                       ChargesTypeName = a.ChargesType == null ? null : a.ChargesType.EnglishName,
                       CurrencyCode = a.Currency == null ? null : a.Currency.Code,
                       MeasurementCode = a.Measurement == null ? null : a.Measurement.Code,
                       ChargesTypeString = a.ChargesType == null ? null : "(" + a.ChargesType.Code + ") " + a.ChargesType.EnglishName,
                   };
        }

        public List<TarrifChargePM> GetTarrifChargesByCardId(string cardId, int tenant)
        {
            List<TarrifChargePM> result = new List<TarrifChargePM>();

            TarrifHeaderQuery tarrifHeaderQuery = new TarrifHeaderQuery(tenant);
            List<TarrifHeaderPM> headers = tarrifHeaderQuery.GetTarrifHeadersByCardIdAndTypeCode(cardId, "S", true, tenant).ToList();
            foreach (TarrifHeaderPM item in headers)
            {
                List<TarrifChargePM> list = this.GetTarrifChargesByTarrifHeaderId(item.Id, tenant).ToList();
                result.AddRange(list);
            }

            return result;
        }

        public IQueryable<TarrifChargeList> GetIQueryableEntityList(IQueryable<TarrifCharge> pocos)
        {
            return from a in pocos
                   select new TarrifChargeList()
                   {
                       Id = a.Id,
                       ChargesTypeId = a.ChargesTypeId,
                       TarrifHeaderId = a.TarrifHeaderId,
                       Tenant = a.Tenant,
                       CurrencyId = a.CurrencyId,
                       MaxPrice = a.MaxPrice,
                       MeasurementId = a.MeasurementId,
                       MinPrice = a.MinPrice,
                       UnitPrice = a.UnitPrice,
                     
                   };
        }
    }
}
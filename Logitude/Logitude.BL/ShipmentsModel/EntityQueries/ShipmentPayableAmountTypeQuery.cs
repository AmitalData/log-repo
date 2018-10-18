using System;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPayableAmountTypeQuery
    {
        ShipmentPayableAmountTypeRepository repository;
         
        public ShipmentPayableAmountTypeQuery(int tenant)
        {
            repository = new ShipmentPayableAmountTypeRepository(tenant);
        }

        public ShipmentPayableAmountTypeQuery(ShipmentPayableAmountTypeRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentPayableAmountTypePM GetSingleShipmentPayableAmountTypePM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "ShipmentPayableAmountTypePM" + code;
                ShipmentPayableAmountTypePM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.ShipmentPayableAmountTypes

                                              select new ShipmentPayableAmountTypePM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,

                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "ShipmentPayableAmountTypePM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ShipmentPayableAmountTypePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (ShipmentPayableAmountTypePM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.ShipmentPayableAmountTypes
                              where a.Code == code
                              select new ShipmentPayableAmountTypePM()
                              {
                                  Code = a.Code,
                                  Name = a.Name,

                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }
    }
}
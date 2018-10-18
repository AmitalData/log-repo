using System;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentPayableStatusQuery
    {
        ShipmentPayableStatusRepository repository;

        public ShipmentPayableStatusQuery(int tenant)
        {
            repository = new ShipmentPayableStatusRepository(tenant);
        }

        public ShipmentPayableStatusQuery(ShipmentPayableStatusRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<ShipmentPayableStatusPM> GetStatusTypePMs()
        {
            return from a in repository.context.ShipmentPayableStatus
                   select new ShipmentPayableStatusPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, };
        }

        public ShipmentPayableStatusPM GetSingleShipmentPayableStatusPM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "ShipmentPayableStatusPM" + code;
                ShipmentPayableStatusPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.ShipmentPayableStatus

                                              select new ShipmentPayableStatusPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "ShipmentPayableStatusPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ShipmentPayableStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (ShipmentPayableStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.ShipmentPayableStatus
                              where a.Code == code
                              select new ShipmentPayableStatusPM()
                              {
                                  Code = a.Code,
                                  Name = a.Name,
                                  SearchFields = a.SearchFields
                              }).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public IQueryable<EntityLists.ShipmentPayableStatusList> GetIQueryableEntityList(IQueryable<Simplog.Data.ShipmentsModel.EntityPOCOs.ShipmentPayableStatus> entityPocos)
        {
            IQueryable<ShipmentPayableStatusList> result = (from a in entityPocos
                                                            select new ShipmentPayableStatusList()
                                                          {
                                                              Code = a.Code,
                                                              Name = a.Name,
                                                              SearchFields = a.SearchFields
                                                          });
            return result;
        }
    }
}
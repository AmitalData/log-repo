using System;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentReceivableStatusQuery
    {
        ShipmentReceivableStatusRepository repository;

        public ShipmentReceivableStatusQuery(int tenant)
        {
            repository = new ShipmentReceivableStatusRepository(tenant);
        }

        public ShipmentReceivableStatusQuery(ShipmentReceivableStatusRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<ShipmentReceivableStatusPM> GetStatusTypePMs()
        {
            return from a in repository.context.ShipmentReceivableStatus
                   select new ShipmentReceivableStatusPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, };
        }

        public ShipmentReceivableStatusPM GetSingleShipmentReceivableStatusPM(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "ShipmentReceivableStatusPM" + code;
                ShipmentReceivableStatusPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.ShipmentReceivableStatus

                                              select new ShipmentReceivableStatusPM()
                                              {
                                                  Code = a.Code,
                                                  Name = a.Name,
                                                  SearchFields = a.SearchFields
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "ShipmentReceivableStatusPM" + s.Code;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, DateTime.UtcNow.AddHours(30), TimeSpan.Zero);
                            }
                        }
                        entity = (ShipmentReceivableStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (ShipmentReceivableStatusPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.ShipmentReceivableStatus
                              where a.Code == code
                              select new ShipmentReceivableStatusPM()
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

        public IQueryable<EntityLists.ShipmentReceivableStatusList> GetIQueryableEntityList(IQueryable<Simplog.Data.ShipmentsModel.EntityPOCOs.ShipmentReceivableStatus> iQueryable)
        {
            IQueryable<ShipmentReceivableStatusList> result = (from a in iQueryable
                                                               select new ShipmentReceivableStatusList()
                                                            {
                                                                Code = a.Code,
                                                                Name = a.Name,
                                                                SearchFields = a.SearchFields
                                                            });
            return result;
        }
    }
}
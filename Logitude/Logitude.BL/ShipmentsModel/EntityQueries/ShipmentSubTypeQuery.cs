using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentSubTypeQuery
    {
        ShipmentSubTypeRepository repository;

        public ShipmentSubTypeQuery()
        {
            repository = new ShipmentSubTypeRepository();
        }

        public ShipmentSubTypeQuery(int tenant)
        {
            repository = new ShipmentSubTypeRepository(tenant);
        }

        public ShipmentSubTypeQuery(ShipmentSubTypeRepository quoteQuery)
        {
            repository = quoteQuery;
        }

        public ShipmentSubTypePM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.ShipmentSubTypes.Include("ShipmentType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Id == id && a.Tenant == tenant
                    select new ShipmentSubTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ShipmentTypeCode = a.ShipmentTypeCode,
                        ShipmentTypeName = a.ShipmentType == null ? null : a.ShipmentType.Name,
                        CreateDate = a.CreateDate,
                        UpdateDate = a.UpdateDate,
                        CreatedByUserId = a.CreatedByUserId,
                        UpdatedByUserId = a.UpdatedByUserId,
                        Inactive = a.Inactive,
                        CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                        UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                        IsManuallyAdded = a.IsManuallyAdded,
                    }).FirstOrDefault();
        }

        public IQueryable<ShipmentSubTypeList> GetIQueryableEntityList(IQueryable<ShipmentSubType> iQueryable)
        {
            IQueryable<ShipmentSubTypeList> result = from a in iQueryable
                                                        select new ShipmentSubTypeList()
                                                        {
                                                            Code = a.Code,
                                                            Name = a.Name,
                                                            SearchFields = a.SearchFields,
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            ShipmentTypeCode = a.ShipmentTypeCode,
                                                            ShipmentTypeName = a.ShipmentType == null ? null : a.ShipmentType.Name,
                                                            CreateDate = a.CreateDate,
                                                            UpdateDate = a.UpdateDate,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            Inactive = a.Inactive,
                                                            CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                                                            UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                                                            IsManuallyAdded = a.IsManuallyAdded,
                                                        };
            return result;
        }
    }
}

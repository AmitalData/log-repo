using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
namespace Simplog.Data.InfrastructureModel.Repositories
{
    public interface IObjectTableRepository
    {
        void Add(Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable entity);
        System.Collections.Generic.List<Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable> All();
        Simplog.Data.InfrastructureModel.IWebFreightContext context { get; }
        Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable GetFirstObjectTable(int tenant);
        System.Collections.Generic.List<Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys);
        System.Linq.IQueryable<Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable> GetObjects();
        System.Linq.IQueryable<Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable> GetObjectsByTenant(int tenant);
        System.Linq.IQueryable<Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable> GetObjectsByTenantOrTenantZero(int tenant);
        Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable GetObjectTableById(string id, int tenant);
        Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable GetObjectTableByName(string name, int tenant, bool getFromCache, int contextTenant= 0);
        string GetObjectTableIdByName(string tablename);
        Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys);
        Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable GetSingleObjectTable(string id, int tenant, bool getFromCache);
        bool IsObjectTableMaster(string objectTableId, bool getFromCache);
        bool IsObjectTableShipment(string objectTableId, bool getFromCache);
        void Remove(Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable entity);
        void SubmitChanges();
        void Update(Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable entity);
    }
}

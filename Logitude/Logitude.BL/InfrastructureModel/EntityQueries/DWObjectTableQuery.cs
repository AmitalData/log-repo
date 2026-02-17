

using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DWObjectTableQuery
    {
        DWObjectTableRepository repository;

        public DWObjectTableQuery()
        {
            repository = new DWObjectTableRepository();
        }

        public DWObjectTableQuery(int tenant)
        {
            repository = new DWObjectTableRepository(tenant);
        }

        public DWObjectTableQuery(DWObjectTableRepository DWObjectTableRepository)
        {
            repository = DWObjectTableRepository;
        }

        public DWObjectTablePM GetSingleDWObjectTablePM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectTables
                    where a.Id == id && a.Tenant == tenant
                    select new DWObjectTablePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        TypeCode =a.TypeCode,
                        Code = a.Code,
                        IsClosed=  a.IsClosed,
                        DefaultFilterBy = a.DefaultFilterBy
                    }).FirstOrDefault();
        }


        public IQueryable<DWObjectTablePM> GetDWObjectTablePMsByTenant(int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectTables
                    where a.Tenant == tenant
                    select new DWObjectTablePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        TypeCode = a.TypeCode,
                        Code = a.Code,
                        IsClosed = a.IsClosed,
                        DefaultFilterBy = a.DefaultFilterBy
                    }
                  );
        }

        public DWObjectTablePM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectTables
                    where a.Code == id && (a.Tenant == tenant || a.Tenant == 0)
                    select new DWObjectTablePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        TypeCode = a.TypeCode,
                        Code = a.Code,
                        IsClosed = a.IsClosed,
                        DefaultFilterBy = a.DefaultFilterBy
                    }).FirstOrDefault();
        }

        public IQueryable<DWObjectTablePM> GetDWObjectTablePMs(int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectTables
                    where a.Tenant == tenant
                    select new DWObjectTablePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        TypeCode = a.TypeCode,
                        Code = a.Code,
                        IsClosed = a.IsClosed,
                        DefaultFilterBy = a.DefaultFilterBy
                    });
        }

        public IQueryable<DWObjectTableList> GetIQueryableEntityList(IQueryable<DWObjectTable> iQueryable)
        {
            IQueryable<DWObjectTableList> result = from a in iQueryable
                                              select new DWObjectTableList()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  Name = a.Name,
                                                  TypeCode = a.TypeCode,
                                                  Code = a.Code,
                                                  IsClosed = a.IsClosed,
                                                  DefaultFilterBy = a.DefaultFilterBy
                                              };

            return result;
        }

        public string GetDWObjectTableIdByCode(string code, int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectTables
                    where a.Tenant == tenant && a.Code == code
                    select a.Id).FirstOrDefault();


        }

    }
}
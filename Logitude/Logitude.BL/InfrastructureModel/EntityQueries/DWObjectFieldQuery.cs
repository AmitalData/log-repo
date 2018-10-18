

using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DWObjectFieldQuery
    {
        DWObjectFieldRepository repository;
        
        public DWObjectFieldQuery()
        {
            repository = new DWObjectFieldRepository();
        }

        public DWObjectFieldQuery(int tenant)
        {
            repository = new DWObjectFieldRepository(tenant);
        }

        public DWObjectFieldQuery(DWObjectFieldRepository DWObjectFieldRepository)
        {
            repository = DWObjectFieldRepository;
        }

        public DWObjectFieldPM GetSingleDWObjectFieldPM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.Id == id && a.Tenant == tenant
                    select new DWObjectFieldPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        Code = a.Code,
                        DimensionTableCode = a.DimensionTableCode,
                        DataTypeCode = a.DataTypeCode,
                        DWObjectTableCode = a.DWObjectTableCode,
                        IsRequiered = a.IsRequired,
                        MaxLength = a.MaxLength,
                        MinLength = a.MinLength,
                        IsPrimaryKey = a.IsPrimaryKey,
                        IsMeasurement = a.IsMeasurement,
                        AggregationTypeCode = a.AggregationTypeCode,
                    }).FirstOrDefault();
        }


        public IQueryable<DWObjectFieldPM> GetDWObjectFieldPMsByTenant(int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.Tenant == tenant
                    select new DWObjectFieldPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        Code = a.Code,
                        DimensionTableCode = a.DimensionTableCode,
                        DataTypeCode = a.DataTypeCode,
                        DWObjectTableCode = a.DWObjectTableCode,
                        IsRequiered = a.IsRequired,
                        MaxLength = a.MaxLength,
                        MinLength = a.MinLength,
                        IsPrimaryKey = a.IsPrimaryKey,
                        IsMeasurement = a.IsMeasurement,
                        AggregationTypeCode = a.AggregationTypeCode,
                    }
                  );
        }

        public IQueryable<DWObjectFieldPM> GetDWObjectFieldPMsByDWObjectTabelAndTenant(int tenant,string dwotCode)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.Tenant == tenant && a.DWObjectTableCode == dwotCode
                    select new DWObjectFieldPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        Code = a.Code,
                        DimensionTableCode = a.DimensionTableCode,
                        DataTypeCode = a.DataTypeCode,
                        DWObjectTableCode = a.DWObjectTableCode,
                        IsRequiered = a.IsRequired,
                        MaxLength = a.MaxLength,
                        MinLength = a.MinLength,
                        IsPrimaryKey = a.IsPrimaryKey,
                        IsMeasurement = a.IsMeasurement,
                        AggregationTypeCode = a.AggregationTypeCode,
                    }
                  );
        }

        public DWObjectFieldPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.Id == id && a.Tenant == tenant
                    select new DWObjectFieldPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        Code = a.Code,
                        DimensionTableCode = a.DimensionTableCode,
                        DataTypeCode = a.DataTypeCode,
                        DWObjectTableCode = a.DWObjectTableCode,
                        IsRequiered = a.IsRequired,
                        MaxLength = a.MaxLength,
                        MinLength = a.MinLength,
                        IsPrimaryKey = a.IsPrimaryKey,
                    }).FirstOrDefault();
        }

        public IQueryable<DWObjectFieldPM> GetDWObjectFieldPMs(int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.Tenant == tenant
                    select new DWObjectFieldPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Name = a.Name,
                        Code = a.Code,
                        DimensionTableCode = a.DimensionTableCode,
                        DataTypeCode = a.DataTypeCode,
                        DWObjectTableCode = a.DWObjectTableCode,
                        IsRequiered = a.IsRequired,
                        MaxLength = a.MaxLength,
                        MinLength = a.MinLength,
                        IsPrimaryKey = a.IsPrimaryKey,
                        IsMeasurement = a.IsMeasurement,
                        AggregationTypeCode = a.AggregationTypeCode,
                    });
        }

        public IQueryable<DWObjectFieldList> GetIQueryableEntityList(IQueryable<DWObjectField> iQueryable)
        {
            IQueryable<DWObjectFieldList> result = from a in iQueryable
                                                   select new DWObjectFieldList()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       Name = a.Name,
                                                       Code = a.Code,
                                                       DimensionTableCode = a.DimensionTableCode,
                                                       DataTypeCode = a.DataTypeCode,
                                                       DWObjectTableCode = a.DWObjectTableCode,
                                                       IsRequiered = a.IsRequired,
                                                       MaxLength = a.MaxLength,
                                                       MinLength = a.MinLength,
                                                       IsMeasurement = a.IsMeasurement,
                                                       AggregationTypeCode = a.AggregationTypeCode,
                                                   };

            return result;
        }

        public string GetDWObjectFieldIdByCode(string code, int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.Tenant == tenant && a.Code == code
                    select a.Id).FirstOrDefault();


        }

    }
}

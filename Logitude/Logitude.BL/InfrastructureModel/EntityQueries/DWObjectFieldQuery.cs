

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
                        DisplayInQueryBuilder = a.DisplayInQueryBuilder,
                        Category1 = a.Category1,
                        Category2 = a.Category2
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
                        DisplayInQueryBuilder = a.DisplayInQueryBuilder,
                        Category1 = a.Category1,
                        Category2 = a.Category2
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
                        DisplayInQueryBuilder = a.DisplayInQueryBuilder,
                        Category1 = a.Category1,
                        Category2 = a.Category2
                    }
                  );
        }

        public List<DWObjectFieldPM> GetDWObjectFieldWithChildrenFieldsPMsByDWObjectTabelAndTenant(int tenant, string dwotCode)
        {
            var TempList = (from a in repository.webFreightContext.DWObjectFields
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
                                DisplayInQueryBuilder = a.DisplayInQueryBuilder,
                                DisplayName = a.Name,
                                Category1 = a.Category1,
                                Category2 = a.Category2

                            }
                  );
            var Parents = TempList.Where(a => a.DimensionTableCode != null).ToList();
            var FinalList = TempList.Where(a => a.DimensionTableCode == null).ToList();
            foreach (var item in Parents)
            {
                var TempInnerList = (from a in repository.webFreightContext.DWObjectFields
                                     where a.Tenant == tenant && a.DWObjectTableCode == item.DimensionTableCode
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
                                         DisplayInQueryBuilder = a.DisplayInQueryBuilder,
                                         DisplayName = a.DWObjectTableCode.Replace("DIM_","") + "." + a.Name,
                                         Category1 = a.Category1,
                                         Category2 = a.Category2
                                     }
                  ).ToList();
                FinalList = FinalList.Concat(TempInnerList).ToList();
            }
            return FinalList;
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
                        DisplayInQueryBuilder = a.DisplayInQueryBuilder,
                        Category1 = a.Category1,
                        Category2 = a.Category2
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
                        DisplayInQueryBuilder = a.DisplayInQueryBuilder,
                        Category1 = a.Category1,
                        Category2 = a.Category2
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
                                                       DisplayInQueryBuilder = a.DisplayInQueryBuilder,
                                                       Category1 = a.Category1,
                                                       Category2 = a.Category2
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

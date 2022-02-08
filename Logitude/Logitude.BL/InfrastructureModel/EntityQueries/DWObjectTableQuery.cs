

using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;
using System;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DWObjectTableQuery
    {
        DWObjectTableRepository repository;
        private readonly Dictionary<string, string> FactTablesFeatureToggle = new Dictionary<string, string>
        {
            {"BIF","Invoices"}, 
            {"IDS", "Inland Domestic Shipments"},
            {"OIC", "Containers"}
        };

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
                        TypeCode = a.TypeCode,
                        Code = a.Code,
                        IsClosed = a.IsClosed,
                        DefaultFilterBy = a.DefaultFilterBy,
                        DataViewName = a.DataViewName,
                        HasPivotColumn = a.HasPivotColumn,
                        PivotFieldCode = a.PivotFieldCode,
                        AdditionalFactCode = a.AdditionalFactCode,
                        AdditionalFactForeignKey = a.AdditionalFactForeignKey,
                        RecordType = a.RecordType,
                        ParentFactCode = a.ParentFactCode,
                        DisplayName = a.DisplayName,
                        ObjectTableName = a.ObjectTableName ,
                        HasCustomFields = a.HasCustomFields , 
                        MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                        AdditionalFactRelationType = a.AdditionalFactRelationType,
                        Description = a.Description,

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
                        DefaultFilterBy = a.DefaultFilterBy,
                        DataViewName = a.DataViewName,
                        HasPivotColumn = a.HasPivotColumn,
                        PivotFieldCode = a.PivotFieldCode,
                        AdditionalFactCode = a.AdditionalFactCode,
                        AdditionalFactForeignKey = a.AdditionalFactForeignKey,
                        RecordType = a.RecordType,
                        ParentFactCode = a.ParentFactCode,
                        DisplayName = a.DisplayName,
                        ObjectTableName = a.ObjectTableName,
                        HasCustomFields = a.HasCustomFields,
                        MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                        AdditionalFactRelationType = a.AdditionalFactRelationType,
                        Description = a.Description,
                    });
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
                        DefaultFilterBy = a.DefaultFilterBy,
                        DataViewName = a.DataViewName,
                        HasPivotColumn = a.HasPivotColumn,
                        PivotFieldCode = a.PivotFieldCode,
                        AdditionalFactCode = a.AdditionalFactCode,
                        AdditionalFactForeignKey = a.AdditionalFactForeignKey,
                        RecordType = a.RecordType,
                        ParentFactCode = a.ParentFactCode,
                        DisplayName = a.DisplayName,
                        ObjectTableName = a.ObjectTableName,
                        HasCustomFields = a.HasCustomFields,
                        MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                        AdditionalFactRelationType = a.AdditionalFactRelationType,
                        AdditionalConditions = a.AdditionalConditions,
                        Description = a.Description,

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
                        DefaultFilterBy = a.DefaultFilterBy,
                        DataViewName = a.DataViewName,
                        HasPivotColumn = a.HasPivotColumn,
                        PivotFieldCode = a.PivotFieldCode,
                        AdditionalFactCode = a.AdditionalFactCode,
                        AdditionalFactForeignKey = a.AdditionalFactForeignKey,
                        RecordType = a.RecordType,
                        ParentFactCode = a.ParentFactCode,
                        DisplayName = a.DisplayName,
                        ObjectTableName = a.ObjectTableName,
                        HasCustomFields = a.HasCustomFields,
                        MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                        AdditionalFactRelationType = a.AdditionalFactRelationType,
                        Description = a.Description,

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
                                                       DefaultFilterBy = a.DefaultFilterBy,
                                                       DataViewName = a.DataViewName,
                                                       HasPivotColumn = a.HasPivotColumn,
                                                       PivotFieldCode = a.PivotFieldCode,
                                                       AdditionalFactCode = a.AdditionalFactCode,
                                                       AdditionalFactForeignKey = a.AdditionalFactForeignKey,
                                                       RecordType = a.RecordType,
                                                       ParentFactCode = a.ParentFactCode,
                                                       DisplayName = a.DisplayName,
                                                       ObjectTableName = a.ObjectTableName,
                                                       HasCustomFields = a.HasCustomFields,
                                                       MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                                       AdditionalFactRelationType = a.AdditionalFactRelationType,
                                                       Description = a.Description,

                                                   };

            return result;
        }

        public string GetDWObjectTableIdByCode(string code, int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectTables
                    where a.Tenant == tenant && a.Code == code
                    select a.Id).FirstOrDefault();


        }

        public List<ShortFactTableDetails> GetFactTablesNames()
        {
            return (from a in repository.webFreightContext.DWObjectTables
                    where a.Tenant == 0 && a.TypeCode == "Fact"
                    select new ShortFactTableDetails()
                    { 
                        DisplayName = a.DisplayName,
                        Code = a.Code,
                        Description = a.Description
                    }).ToList();
        }

        public List<ShortFactTableDetails> CheckFactTablesFeatureToggle(List<ShortFactTableDetails> dwFactTablesNames, int tenant)
        {
            foreach (KeyValuePair<string, string> factTable in FactTablesFeatureToggle)
            { 
                if (!FeatureToggleHelper.HasFeatureToggle(factTable.Key, tenant))
                {
                    RemoveFactTable(factTable.Value, dwFactTablesNames);
                }
            }
            return dwFactTablesNames;
        }
         
        private static void RemoveFactTable(string factTableName, List<ShortFactTableDetails> dwFactTablesNames)
        {
            var factTale = dwFactTablesNames.SingleOrDefault(s => s.DisplayName == factTableName);
            if (factTale != null)
            {
                dwFactTablesNames.Remove(factTale);
            }
        } 

}



public class ShortFactTableDetails
    { 
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
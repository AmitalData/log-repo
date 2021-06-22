

using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;
using System;
using Logitude.BL.Helpers;

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
            DWObjectFieldPM dWObjectFieldPM   = (from a in repository.webFreightContext.DWObjectFields
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
                                                   //Category1 = a.Category1,
                                                   //Category2 = a.Category2,
                                                   LOVAdditionalColumns = a.LOVAdditionalColumns,
                                                   HideTree = a.HideTree,
                                                   CannotFilter = a.CannotFilter,
                                                   HelpText = a.HelpText,
                                                   IsCustom = a.IsCustom,
                                                   OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                                                   ViewFieldDisplayName = a.ViewFieldDisplayName,
                                                   DontDisplayInView = a.DontDisplayInView,
                                                   DimensionDataViewName  = a.DimensionDataViewName,
                                                   IsMultipleSelection = a.IsMultipleSelection,
                                                   RecordType = a.RecordType,
                                               }).FirstOrDefault();


            //if (dWObjectFieldPM != null)
            //{
            //    ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(tenant);
            //    var objectField = objectFieldQuery.GetObjectFieldByFieldCode(dWObjectFieldPM.OriginalObjectFieldCode, tenant);
            //    if (objectField != null)
            //    {
            //        dWObjectFieldPM.FullNameTextCodeDefaultText = objectField.FullNameTextCodeDefaultText;
            //    }
            //}
            return dWObjectFieldPM;
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
                        //Category1 = a.Category1,
                        //Category2 = a.Category2,
                        LOVAdditionalColumns = a.LOVAdditionalColumns,
                        HideTree = a.HideTree,
                        CannotFilter = a.CannotFilter,
                        HelpText = a.HelpText,
                        IsCustom = a.IsCustom,
                        OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                        ViewFieldDisplayName = a.ViewFieldDisplayName,
                        DontDisplayInView = a.DontDisplayInView,
                        DimensionDataViewName = a.DimensionDataViewName,
                        IsMultipleSelection = a.IsMultipleSelection,
                        RecordType = a.RecordType,


                    });
        }

        public List<DWObjectFieldPM> GetDWObjectFieldPMsByDWObjectTabelAndTenant(int tenant,string dwotCode)
        {
            List<DWObjectFieldPM> result = (from a in repository.webFreightContext.DWObjectFields
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
                                                //Category1 = a.Category1,
                                                //Category2 = a.Category2,
                                                LOVAdditionalColumns = a.LOVAdditionalColumns,
                                                HideTree = a.HideTree,
                                                CannotFilter = a.CannotFilter,
                                                HelpText = a.HelpText,
                                                IsCustom = a.IsCustom,
                                                OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                                                ViewFieldDisplayName = a.ViewFieldDisplayName,
                                                DontDisplayInView = a.DontDisplayInView,
                                                DimensionDataViewName = a.DimensionDataViewName,
                                                IsMultipleSelection = a.IsMultipleSelection,
                                                RecordType = a.RecordType,


                                            }).ToList();

            return SetDWFullNameTextCode(tenant, result);

        }

        public List<DWObjectFieldPM> GetDWObjectFieldWithChildrenFieldsPMsByTenant(int tenant)
        {
            DWObjectTableQuery dwObjectTableQuery = new DWObjectTableQuery(tenant);
            List<DWObjectFieldPM> FinalList = new List<DWObjectFieldPM>();
            List<string> dwObjectTablesCodes = dwObjectTableQuery.GetDWObjectTablePMs(tenant).Where(dwTable => dwTable.TypeCode == "Fact" && string.IsNullOrEmpty(dwTable.ParentFactCode)).Select(dwTable => dwTable.Code).ToList();
            dwObjectTablesCodes.ForEach(dwTableCode => {
                FinalList.AddRange(GetDWObjectFieldWithChildrenFieldsPMsByDWObjectTabelAndTenant(tenant, dwTableCode));
            });

            return FinalList;
        }

        public List<DWObjectFieldPM> GetDWObjectFieldWithChildrenFieldsPMsByDWObjectTabelAndTenant(int tenant, string dwotCode)
        {
                var TempList = new DWObjectFieldAdditionalFactService(new DWObjectFieldAdditionalFactArgs() { FactTableCode = dwotCode, Tenant = tenant}).DWObjectFieldPMs;
                var FinalList = TempList.Where(a => a.DimensionTableCode == null).ToList();
                var Parents = TempList.Where(a => a.DimensionTableCode != null).ToList();
                List<string> dimensionTable = Parents.GroupBy(d => d.DimensionTableCode).Select(d => d.First().DimensionTableCode).ToList();
                dimensionTable.Add("DIM_CustomPickLists");
            //Parents.Add(new DWObjectFieldPM() { DimensionTableCode = "DIM_CustomPickLists", Id = "123" });
            IEnumerable<IGrouping<string, DWObjectFieldPM>> DWObjectFieldPMDimensionGroups = GetDWObjectFieldPMDimensionListsGroups(tenant, dimensionTable);

                foreach (var parent in Parents)
                {
                    var tempInnerList = new List<DWObjectFieldPM>();
                    var dWObjectFieldPMDimensionGroup = DWObjectFieldPMDimensionGroups.Where(d => d.Key == parent.DimensionTableCode).FirstOrDefault();
                    if (dWObjectFieldPMDimensionGroup != null)
                    {
                        string parentfieldName = parent.Name;
                        foreach (DWObjectFieldPM item in dWObjectFieldPMDimensionGroup.ToList().Where( d=> (string.IsNullOrEmpty(d.RecordType) || (!string.IsNullOrEmpty(d.RecordType) && d.RecordType.Split(',').Contains(parentfieldName)))))
                        {
                            DWObjectFieldPM dWObjectFieldPM = GetNewInstanceFromDWObjectFieldPM(parent, item, dwotCode);
                            tempInnerList.Add(dWObjectFieldPM);
                        }

                        FinalList = FinalList.Concat(tempInnerList).OrderBy(a => a.DisplayName).ToList();
                    }
                }


            return SetDWFullNameTextCode(tenant, FinalList);
        }

        public IQueryable<DWObjectFieldPM> GetDWObjectFieldByDWObjectTableCode(int tenant, string dwotCode)
        {
            var TempList = (from a in repository.webFreightContext.DWObjectFields
                            where a.Tenant == tenant && a.DWObjectTableCode == dwotCode &&  a.CannotFilter == false
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
                                //Category1 = a.Category1,
                                //Category2 = a.Category2,
                                LOVAdditionalColumns = a.LOVAdditionalColumns,
                                HideTree = a.HideTree,
                                CannotFilter = a.CannotFilter,
                                HelpText = a.HelpText,
                                IsCustom = a.IsCustom,
                                OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                                ViewFieldDisplayName = a.ViewFieldDisplayName,
                                DontDisplayInView = a.DontDisplayInView,
                                DimensionDataViewName = a.DimensionDataViewName,
                                IsMultipleSelection = a.IsMultipleSelection,
                                RecordType = a.RecordType,


                            });
            return TempList;
        }

        private static DWObjectFieldPM GetNewInstanceFromDWObjectFieldPM(DWObjectFieldPM parent, DWObjectFieldPM item, string dwotCode)
        {
            return new DWObjectFieldPM()
            {
                Id = item.Id,
                Tenant = item.Tenant,
                Name = item.Name,
                Code = item.Code,
                DimensionTableCode = parent.DimensionTableCode,
                DataTypeCode = item.DataTypeCode,
                DWObjectTableCode = item.DWObjectTableCode,
                IsRequiered = item.IsRequiered,
                MaxLength = item.MaxLength,
                MinLength = item.MinLength,
                IsPrimaryKey = item.IsPrimaryKey,
                IsMeasurement = item.IsMeasurement,
                AggregationTypeCode = item.AggregationTypeCode,
                DisplayInQueryBuilder = item.DisplayInQueryBuilder,
                LOVAdditionalColumns = item.LOVAdditionalColumns,
                HideTree = item.HideTree,
                CannotFilter = item.CannotFilter,
                HelpText = item.HelpText,
                IsCustom = item.IsCustom,
                DimensionTableDisplayName = parent.Name,
                OriginalObjectFieldCode = item.OriginalObjectFieldCode,
                PartnerOriginalObjectFieldCode = parent.OriginalObjectFieldCode,
                DisplayName = item.Name,
                ViewFieldDisplayName = item.ViewFieldDisplayName,
                DontDisplayInView = item.DontDisplayInView,
                DimensionDataViewName = item.DimensionDataViewName,
                IsMultipleSelection = item.IsMultipleSelection,
                RecordType = item.RecordType,
                FactTableCode = dwotCode,


            };
        }

        private IEnumerable<IGrouping<string, DWObjectFieldPM>> GetDWObjectFieldPMDimensionListsGroups(int tenant ,List<string> dimensionTableLists)
        {
            IEnumerable<IGrouping<string, DWObjectFieldPM>> list = (from a in repository.webFreightContext.DWObjectFields
                                                                    where a.Tenant == tenant && dimensionTableLists.Contains(a.DWObjectTableCode) && a.DisplayInQueryBuilder == true
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
                                                                        LOVAdditionalColumns = a.LOVAdditionalColumns,
                                                                        HideTree = a.HideTree,
                                                                        CannotFilter = a.CannotFilter,
                                                                        HelpText = a.HelpText,
                                                                        IsCustom = a.IsCustom,
                                                                        OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                                                                        ViewFieldDisplayName = a.ViewFieldDisplayName,
                                                                        DontDisplayInView = a.DontDisplayInView,
                                                                        DimensionDataViewName = a.DimensionDataViewName,
                                                                        IsMultipleSelection = a.IsMultipleSelection,
                                                                        RecordType = a.RecordType,


                                                                    }
                   ).ToList().GroupBy(d => d.DWObjectTableCode);

            return list;

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
                        //Category1 = a.Category1,
                        //Category2 = a.Category2,
                        LOVAdditionalColumns = a.LOVAdditionalColumns,
                        HideTree = a.HideTree,
                        CannotFilter = a.CannotFilter,
                        HelpText = a.HelpText,
                        IsCustom = a.IsCustom,
                        OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                        ViewFieldDisplayName = a.ViewFieldDisplayName,
                        DontDisplayInView = a.DontDisplayInView,
                        DimensionDataViewName = a.DimensionDataViewName,
                        IsMultipleSelection = a.IsMultipleSelection,
                        RecordType = a.RecordType,



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
                        //Category1 = a.Category1,
                        //Category2 = a.Category2,
                        LOVAdditionalColumns = a.LOVAdditionalColumns,
                        HideTree = a.HideTree,
                        CannotFilter = a.CannotFilter,
                        HelpText = a.HelpText,
                        IsCustom = a.IsCustom,
                        OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                        ViewFieldDisplayName = a.ViewFieldDisplayName,
                        DontDisplayInView = a.DontDisplayInView,
                        DimensionDataViewName = a.DimensionDataViewName,
                        IsMultipleSelection = a.IsMultipleSelection,
                        RecordType = a.RecordType,



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
                                                       //Category1 = a.Category1,
                                                       //Category2 = a.Category2,
                                                       LOVAdditionalColumns = a.LOVAdditionalColumns,
                                                       HideTree = a.HideTree,
                                                       CannotFilter = a.CannotFilter,
                                                       HelpText = a.HelpText,
                                                       IsCustom = a.IsCustom,
                                                       OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                                                       ViewFieldDisplayName = a.ViewFieldDisplayName,
                                                       DontDisplayInView = a.DontDisplayInView,
                                                       DimensionDataViewName = a.DimensionDataViewName,
                                                       IsMultipleSelection = a.IsMultipleSelection,
                                                       RecordType = a.RecordType,



                                                   };

            return result;
        }

        public string GetDWObjectFieldIdByCode(string code, int tenant)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.Tenant == tenant && a.Code == code
                    select a.Id).FirstOrDefault();


        }

        public DWObjectFieldPM GetPrimaryKeyFieldForDWObjectTable(string DWObjectTableCode)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.DWObjectTableCode == DWObjectTableCode && a.IsPrimaryKey == true
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
                        //Category1 = a.Category1,
                        //Category2 = a.Category2,
                        LOVAdditionalColumns = a.LOVAdditionalColumns,
                        HideTree = a.HideTree,
                        CannotFilter = a.CannotFilter,
                        HelpText = a.HelpText,
                        IsCustom = a.IsCustom,
                        OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                        ViewFieldDisplayName = a.ViewFieldDisplayName,
                        DontDisplayInView = a.DontDisplayInView,
                        DimensionDataViewName = a.DimensionDataViewName,
                        IsMultipleSelection = a.IsMultipleSelection,
                        RecordType = a.RecordType,



                    }).FirstOrDefault();
        }

        public string GetDWObjectFieldCodeByNameDimTable(string DWDimTableCode,string DWOFName)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.DimensionTableCode == DWDimTableCode && a.Name == DWOFName
                    select a.Code).FirstOrDefault();
        }

        public string GetFactTableCode(string DWDimTableCode)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.DimensionTableCode == DWDimTableCode && a.IsPrimaryKey == true
                    select a.DWObjectTableCode).FirstOrDefault();
        }

        public DWObjectFieldPM GetFactKeyFieldForDWDimTable(string DWFactTableCode,string DWDimTableCode)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.DimensionTableCode == DWDimTableCode && a.DWObjectTableCode == DWFactTableCode && (a.DataTypeCode.ToLower() == "dimension" || a.DataTypeCode.ToLower() == "lookup")
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
                        //Category1 = a.Category1,
                        //Category2 = a.Category2,
                        LOVAdditionalColumns = a.LOVAdditionalColumns,
                        HideTree = a.HideTree,
                        CannotFilter = a.CannotFilter,
                        HelpText = a.HelpText,
                        IsCustom = a.IsCustom,
                        OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                        ViewFieldDisplayName = a.ViewFieldDisplayName,
                        DontDisplayInView = a.DontDisplayInView,
                        DimensionDataViewName = a.DimensionDataViewName,
                        IsMultipleSelection = a.IsMultipleSelection,
                        RecordType = a.RecordType,



                    }).FirstOrDefault();
        }

        public List<DWObjectFieldPM> GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(int tenant, string dwotCode , string recordType)
        {


            List<DWObjectFieldPM> results = (from aa in repository.webFreightContext.DWObjectFieldCategories
                                             join a in repository.webFreightContext.DWObjectFields on aa.DWObjectFieldCode equals a.Code
                                             join b in repository.webFreightContext.DWCategories on aa.DWCategoryCode equals b.Code
                                             where a.Tenant == tenant && a.DWObjectTableCode == dwotCode && aa.DWObjectTableCode == dwotCode && (string.IsNullOrEmpty(a.RecordType) || (!string.IsNullOrEmpty(a.RecordType) && a.RecordType.IndexOf(recordType)>-1))
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
                                                 //Category1 = a.Category1,
                                                 //Category2 = a.Category2,
                                                 LOVAdditionalColumns = a.LOVAdditionalColumns,
                                                 Category = aa.DWCategory.Name,
                                                 CategoryIndex = b.Index,
                                                 HideTree = a.HideTree,
                                                 CannotFilter = a.CannotFilter,
                                                 HelpText = a.HelpText,
                                                 IsCustom = a.IsCustom,
                                                 OriginalObjectFieldCode = a.OriginalObjectFieldCode,
                                                 ViewFieldDisplayName = a.ViewFieldDisplayName,
                                                 DontDisplayInView = a.DontDisplayInView,
                                                 DimensionDataViewName = a.DimensionDataViewName,
                                                 IsMultipleSelection = a.IsMultipleSelection,
                                                 RecordType = a.RecordType,


                                             }
                  ).ToList();

            results = SetDWFullNameTextCode(tenant, results);

            return results;
        }

        private  List<DWObjectFieldPM> SetDWFullNameTextCode(int tenant, List<DWObjectFieldPM> dWObjectFieldPMs)
        {
            List<DWObjectFieldPM> results = dWObjectFieldPMs;
            ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(tenant);
            var objectFields = objectFieldQuery.GetObjectFieldsUsedInDWData(tenant);
            if (objectFields.Count > 0)
            {
                foreach (DWObjectFieldPM dWObjectFieldPM in results.Where(d => !string.IsNullOrEmpty(d.OriginalObjectFieldCode) || !string.IsNullOrEmpty(d.PartnerOriginalObjectFieldCode)))
                {
                    var objectField = objectFields.Where(d => d.FieldCode == dWObjectFieldPM.OriginalObjectFieldCode).FirstOrDefault();
                    if (objectField != null) dWObjectFieldPM.FullNameTextCodeCode = objectField.FullNameTextCodeCode;

                    var partnerObjectField = objectFields.Where(d => d.FieldCode == dWObjectFieldPM.PartnerOriginalObjectFieldCode).FirstOrDefault();
                    if (partnerObjectField != null)
                    {
                        dWObjectFieldPM.PartnerFullNameTextCodeCode = partnerObjectField.FullNameTextCodeCode;
                    }

                }
            }
            return results;
        }

        public DWObjectField GetDWObjectFieldByDimTable(string DWDimTableCode)
        {
            return (from a in repository.webFreightContext.DWObjectFields
                    where a.DimensionTableCode == DWDimTableCode
                    select a).FirstOrDefault();
        }
    }
}

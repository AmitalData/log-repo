using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class DWObjectFieldQuery
    {
        readonly Repository<DWObjectField> repository;
        readonly IAmitalCloudContext context;

        public DWObjectFieldQuery(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
            repository = new Repository<DWObjectField>(context);
        }

        public List<DWObjectFieldPM> GetDWObjectFieldWithChildrenFieldsPMsByTenant(int tenant)
        {
            List<DWObjectFieldPM> FinalList = new List<DWObjectFieldPM>();
            List<string> dwObjectTablesCodes = new Repository<DWObjectTable>(context).GetMulti(a => a.Tenant == tenant, a => new DWObjectTablePM(a)).Where(dwTable => dwTable.TypeCode == "Fact" && string.IsNullOrEmpty(dwTable.ParentFactCode)).Select(dwTable => dwTable.Code).ToList();

            Parallel.ForEach(dwObjectTablesCodes, (dwTableCode) =>
            {
                FinalList.AddRange(GetDWObjectFieldWithChildrenFieldsPMsByDWObjectTabelAndTenant(tenant, dwTableCode));
            });

            return FinalList;
        }

        public List<DWObjectFieldPM> GetDWObjectFieldWithChildrenFieldsPMsByDWObjectTabelAndTenant(int tenant, string dwotCode)
        {
            var TempList = new DWObjectFieldAdditionalFactService(new DWObjectFieldAdditionalFactArgs() { FactTableCode = dwotCode, Tenant = tenant }).DWObjectFieldPMs;
            var FinalList = TempList.Where(a => a.DimensionTableCode == null).ToList();
            var Parents = TempList.Where(a => a.DimensionTableCode != null).ToList();
            List<string> dimensionTable = Parents.GroupBy(d => d.DimensionTableCode).Select(d => d.First().DimensionTableCode).ToList();
            dimensionTable.Add("DIM_CustomPickLists");
            IEnumerable<IGrouping<string, DWObjectFieldPM>> DWObjectFieldPMDimensionGroups = GetDWObjectFieldPMDimensionListsGroups(tenant, dimensionTable);

            foreach (var parent in Parents)
            {
                var tempInnerList = new List<DWObjectFieldPM>();
                var dWObjectFieldPMDimensionGroup = DWObjectFieldPMDimensionGroups.Where(d => d.Key == parent.DimensionTableCode).FirstOrDefault();
                if (dWObjectFieldPMDimensionGroup != null)
                {
                    string parentfieldName = parent.Name;
                    foreach (DWObjectFieldPM item in dWObjectFieldPMDimensionGroup.ToList().Where(d => (string.IsNullOrEmpty(d.RecordType) || (!string.IsNullOrEmpty(d.RecordType) && d.RecordType.Split(',').Contains(parentfieldName)))))
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
            var TempList = repository.GetQueryable().Where(a => a.Tenant == tenant && a.DWObjectTableCode == dwotCode && a.CannotFilter == false).Select(a => new DWObjectFieldPM(a));
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
                UseUnitSelection = item.UseUnitSelection,
                RecordType = item.RecordType,
                FactTableCode = dwotCode,
            };
        }

        private IEnumerable<IGrouping<string, DWObjectFieldPM>> GetDWObjectFieldPMDimensionListsGroups(int tenant, List<string> dimensionTableLists)
        {
            IEnumerable<IGrouping<string, DWObjectFieldPM>> list = repository.GetMulti(a => a.Tenant == tenant && dimensionTableLists.Contains(a.DWObjectTableCode) && a.DisplayInQueryBuilder == true, a => new DWObjectFieldPM(a)).ToList().GroupBy(d => d.DWObjectTableCode);
            return list;
        }

        public List<DWObjectFieldPM> GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(int tenant, string dwotCode, string recordType)
        {
            Repository<DWObjectFieldCategories> DWObjectFieldCategoriesRepo = new Repository<DWObjectFieldCategories>(context);
            Repository<DWCategories> DWCategoriesRepo = new Repository<DWCategories>(context);

            List<DWObjectFieldPM> results = (from aa in DWObjectFieldCategoriesRepo.GetQueryable()
                                             join a in repository.GetQueryable() on aa.DWObjectFieldCode equals a.Code
                                             join b in DWCategoriesRepo.GetQueryable() on aa.DWCategoryCode equals b.Code
                                             where a.Tenant == tenant && a.DWObjectTableCode == dwotCode && aa.DWObjectTableCode == dwotCode && (string.IsNullOrEmpty(a.RecordType) || (!string.IsNullOrEmpty(a.RecordType) && a.RecordType.IndexOf(recordType) > -1))
                                             select new DWObjectFieldPM(a)
                                             {
                                                 Category = aa.DWCategories.Name,
                                                 CategoryIndex = b.Index,
                                             }).ToList();

            results = SetDWFullNameTextCode(tenant, results);

            return results;
        }

        private List<DWObjectFieldPM> SetDWFullNameTextCode(int tenant, List<DWObjectFieldPM> dWObjectFieldPMs)
        {
            List<DWObjectFieldPM> results = dWObjectFieldPMs;

            Repository<ObjectField> objectFieldRepo = new Repository<ObjectField>(context);
            var objectFields = objectFieldRepo.GetMulti(a => (a.Tenant == tenant || a.Tenant == 0) && a.CopyToDW);

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
    }
}

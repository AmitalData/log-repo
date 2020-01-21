using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ObjectFieldRepository : IRepository<ObjectField>
    {
        IWebFreightContext webFreightContext;
        public ObjectFieldRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }

        public ObjectFieldRepository()
        {
            //Context=new WebFreightContext(); 
        }
        public ObjectFieldRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<ObjectField> GetObjectFields()
        {
            return context.ObjectFields;
        }
        public IQueryable<ObjectField> GetObjectFields(int tenant)
        {
            return context.ObjectFields.Where(t => t.Tenant == tenant);
        }

        public static List<ObjectField> GetObjectFieldsByObjectTableName(string objectTableName, int tenant)
        {

            string zerolistName = "tabletenantzeroobjectfields" + objectTableName.ToLower();
            string tenantListName = "tabletenantobjectfields" + objectTableName.ToLower() + tenant;

            List<ObjectField> result = new List<ObjectField>();

            List<ObjectField> currentTenantObjectFields = new List<ObjectField>();
            List<ObjectField> zeroTenantObjectFields = new List<ObjectField>();

            bool isWRCacheALLOWED = false;

            var iAppSettings = System.Configuration.ConfigurationManager.AppSettings;
            if (iAppSettings != null)
            {
                if (iAppSettings["WRCacheALLOWED"] != null)
                {
                    string iValueText = iAppSettings["WRCacheALLOWED"] + "";
                    if (!string.IsNullOrEmpty(iValueText))
                    {
                        if (iValueText.ToLower() == "true")
                        {
                            isWRCacheALLOWED = true;
                        }
                    }
                }
            }

            #region Current Tenant Fields

            if (tenant != 0)
            {
                if (HttpContext.Current != null || isWRCacheALLOWED)
                {
                    if (CacheManager.CacheWrapper.Get(tenantListName) == null)
                    {

                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IWebFreightContext context = WebFreightContext.GetContext(tenant);
                            currentTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                                         where (a.Tenant == tenant) && a.ObjectTable.Name == objectTableName && a.InActive == false
                                                         select a).ToList();

                            scope.Complete();
                        }



                        CacheManager.CacheWrapper.Insert(tenantListName, currentTenantObjectFields, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                    else
                    {
                        currentTenantObjectFields = (List<ObjectField>)CacheManager.CacheWrapper.Get(tenantListName);
                    }
                }
                else
                {

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(tenant);
                        currentTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                                     where (a.Tenant == tenant) && a.ObjectTable.Name == objectTableName && a.InActive == false
                                                     select a).ToList();

                        scope.Complete();
                    }

                }
            }


            #endregion

            #region Tenant Zero Fields

            if (HttpContext.Current != null || isWRCacheALLOWED)
            {
                if (CacheManager.CacheWrapper.Get(zerolistName) == null)
                {

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(0);
                        zeroTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                                  where (a.Tenant == 0) && a.ObjectTable.Name == objectTableName && a.InActive == false
                                                  select a).ToList();

                        scope.Complete();
                    }



                    CacheManager.CacheWrapper.Insert(zerolistName, zeroTenantObjectFields, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                }
                else
                {
                    zeroTenantObjectFields = (List<ObjectField>)CacheManager.CacheWrapper.Get(zerolistName);
                }
            }
            else
            {

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IWebFreightContext context = WebFreightContext.GetContext(0);
                    zeroTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                              where (a.Tenant == 0) && a.ObjectTable.Name == objectTableName && a.InActive == false
                                              select a).ToList();

                    scope.Complete();
                }

            }
            #endregion


            result = zeroTenantObjectFields.Concat(currentTenantObjectFields).ToList();



            return result;


        }

        public List<ObjectField> GetCustomObjectFields(string objectTableId, int tenant)
        {
            return context.ObjectFields.Where(t => t.Tenant == tenant && t.ObjectTableId == objectTableId && t.IsCustom == true).ToList();
        }

        public ObjectField GetSingleObjectField(string id, int tenant)
        {
            return (from a in context.ObjectFields
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public string GetObjectFieldCodeById(string id, int tenant)
        {
            return (from a in context.ObjectFields
                    where a.Id == id && a.Tenant == tenant
                    select a.FieldCode).FirstOrDefault();
        }


        public List<ObjectField> GetAutomationObjectFieldsByObjectTableId(string objectTableId, int tenant)
        {

            string zerolistName = "tabletenantzeroAutomationConditionsObjectFields" + objectTableId.ToLower();
            string tenantListName = "tabletenantAutomationConditionsObjectFields" + objectTableId.ToLower() + tenant;

            List<ObjectField> result = new List<ObjectField>();

            List<ObjectField> currentTenantObjectFields = new List<ObjectField>();
            List<ObjectField> zeroTenantObjectFields = new List<ObjectField>();

            bool isWRCacheALLOWED = false;

            var iAppSettings = System.Configuration.ConfigurationManager.AppSettings;
            if (iAppSettings != null)
            {
                if (iAppSettings["WRCacheALLOWED"] != null)
                {
                    string iValueText = iAppSettings["WRCacheALLOWED"] + "";
                    if (!string.IsNullOrEmpty(iValueText))
                    {
                        if (iValueText.ToLower() == "true")
                        {
                            isWRCacheALLOWED = true;
                        }
                    }
                }
            }

            #region Current Tenant Fields

            if (tenant != 0)
            {
                if (HttpContext.Current != null || isWRCacheALLOWED)
                {
                    if (CacheManager.CacheWrapper.Get(tenantListName) == null)
                    {

                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {

                            currentTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                                         where (a.Tenant == tenant) && a.ObjectTableId == objectTableId && a.InActive == false && (a.AllowedinAutomationConditions == true || a.DisplayInAutomationAsEnitity == true || a.AutomationEmailRecipient == true || a.IsCustom || a.FieldName == "DescriptionOfGoods" || a.FieldName == "MainCarriageFinalDestinationETA" || a.FieldName == "MainCarriageFinalDestinationATA" || a.FieldName == "MainCarriageETD" || a.FieldName == "MainCarriageATD")
                                                         select a).ToList();
                            currentTenantObjectFields = currentTenantObjectFields.Concat(GetEntityAutomationObjectFields(tenant, currentTenantObjectFields)).ToList();
                            scope.Complete();
                        }

                        CacheManager.CacheWrapper.Insert(tenantListName, currentTenantObjectFields, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                    else
                    {
                        currentTenantObjectFields = (List<ObjectField>)CacheManager.CacheWrapper.Get(tenantListName);
                    }
                }
                else
                {

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {

                        currentTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                                     where (a.Tenant == tenant) && a.ObjectTableId == objectTableId && a.InActive == false && (a.AllowedinAutomationConditions == true || a.DisplayInAutomationAsEnitity == true || a.AutomationEmailRecipient == true || a.IsCustom || a.FieldName == "DescriptionOfGoods" || a.FieldName == "MainCarriageFinalDestinationETA" || a.FieldName == "MainCarriageFinalDestinationATA" || a.FieldName == "MainCarriageETD" || a.FieldName == "MainCarriageATD")
                                                     select a).ToList();
                        currentTenantObjectFields = currentTenantObjectFields.Concat(GetEntityAutomationObjectFields(tenant, currentTenantObjectFields)).ToList();

                        scope.Complete();
                    }

                }
            }


            #endregion

            #region Tenant Zero Fields

            if (HttpContext.Current != null || isWRCacheALLOWED)
            {
                if (CacheManager.CacheWrapper.Get(zerolistName) == null)
                {

                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        zeroTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                                  where (a.Tenant == 0) && a.ObjectTableId == objectTableId && a.InActive == false && (a.AllowedinAutomationConditions == true || a.DisplayInAutomationAsEnitity == true || a.AutomationEmailRecipient == true || a.IsCustom || a.FieldName == "DescriptionOfGoods" || a.FieldName == "MainCarriageFinalDestinationETA" || a.FieldName == "MainCarriageFinalDestinationATA" || a.FieldName == "MainCarriageETD" || a.FieldName == "MainCarriageATD")
                                                  select a).ToList();
                        zeroTenantObjectFields = zeroTenantObjectFields.Concat(GetEntityAutomationObjectFields(0, zeroTenantObjectFields)).ToList();

                        scope.Complete();
                    }



                    CacheManager.CacheWrapper.Insert(zerolistName, zeroTenantObjectFields, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                }
                else
                {
                    zeroTenantObjectFields = (List<ObjectField>)CacheManager.CacheWrapper.Get(zerolistName);
                }
            }
            else
            {

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {

                    zeroTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                              where (a.Tenant == 0) && a.ObjectTableId == objectTableId && a.InActive == false && (a.AllowedinAutomationConditions == true || a.DisplayInAutomationAsEnitity == true || a.AutomationEmailRecipient == true || a.IsCustom || a.FieldName == "DescriptionOfGoods" || a.FieldName == "MainCarriageFinalDestinationETA" || a.FieldName == "MainCarriageFinalDestinationATA" || a.FieldName == "MainCarriageETD" || a.FieldName == "MainCarriageATD")
                                              select a).ToList();
                    zeroTenantObjectFields = zeroTenantObjectFields.Concat(GetEntityAutomationObjectFields(0, zeroTenantObjectFields)).ToList();

                    scope.Complete();
                }

            }
            #endregion


            result = zeroTenantObjectFields.Concat(currentTenantObjectFields).ToList();





            return result;


        }
        private List<ObjectField> GetEntityAutomationObjectFields(int tenant, List<ObjectField> currentTenantObjectFields)
        {
            List<ObjectField> automationEntityObjectFieldLists = new List<ObjectField>();
            if (currentTenantObjectFields != null && currentTenantObjectFields.Count > 0)
            {
                List<string> entityAutomationObjectTableIds = currentTenantObjectFields.Where(d => d.DisplayInAutomationAsEnitity && !string.IsNullOrEmpty(d.LookUpTableId)).GroupBy(d => d.LookUpTableId).Select(d => d.First().LookUpTableId).ToList();
                if (entityAutomationObjectTableIds.Count > 0)
                {
                    automationEntityObjectFieldLists = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                                        where (a.Tenant == tenant) && entityAutomationObjectTableIds.Contains(a.ObjectTableId) && a.AllowedinAutomationConditions == true
                                                        select a).ToList();
                }
            }

            return automationEntityObjectFieldLists;
        }

        public static List<ObjectField> GetCustomObjectFieldsByObjectTableName(string objectTableName, int tenant)
        {
            string objectFieldsListName = objectTableName.ToLower() + "customobjectfields" + tenant;

            bool isWRCacheALLOWED = false;

            var iAppSettings = System.Configuration.ConfigurationManager.AppSettings;
            if (iAppSettings != null)
            {
                if (iAppSettings["WRCacheALLOWED"] != null)
                {
                    string iValueText = iAppSettings["WRCacheALLOWED"] + "";
                    if (!string.IsNullOrEmpty(iValueText))
                    {
                        if (iValueText.ToLower() == "true")
                        {
                            isWRCacheALLOWED = true;
                        }
                    }
                }
            }
            List<ObjectField> objectfields = new List<ObjectField>();
            if (HttpContext.Current != null || isWRCacheALLOWED)
            {
                if (CacheManager.CacheWrapper.Get(objectFieldsListName) == null)
                {
                    IWebFreightContext context = WebFreightContext.GetContext(tenant);
                    objectfields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                    where a.Tenant == tenant && a.ObjectTable.Name == objectTableName && a.IsCustom == true && a.InActive == false
                                    select a).ToList();

                    CacheManager.CacheWrapper.Insert(objectFieldsListName, objectfields, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                }
                else
                {
                    objectfields = (List<ObjectField>)CacheManager.CacheWrapper.Get(objectFieldsListName);
                }
            }
            else
            {
                IWebFreightContext context = WebFreightContext.GetContext(tenant);
                objectfields = (from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                where a.Tenant == tenant && a.ObjectTable.Name == objectTableName && a.IsCustom == true && a.InActive == false
                                select a).ToList();
            }

            return objectfields;
        }



        public IQueryable<ObjectField> GetPMObjectFieldsByObjectTableName(string objectTableName, int tenant)
        {
            IQueryable<ObjectField> objectfields = from a in context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable")
                                                   where a.Tenant == tenant && a.ObjectTable.Name == objectTableName && a.DisplayInList == false && a.IsCustomFilter == false && a.InActive == false
                                                   select a;
            return objectfields;
        }



        public List<ObjectField> GetPMObjectFieldsByObjectTableId(string objectTableId, int tenant)
        {
            IQueryable<ObjectField> objectfields = from a in context.ObjectFields
                                                   where a.ObjectTableId == objectTableId
                                                   select a;
            return objectfields.ToList();
        }


        //public List<ObjectFieldPM> GetFixedFilteredObjectFields(int tenant, string queryId)
        //{
        //    List<ObjectFieldPM> objectfields = (from q in context.QueryFilters
        //                                        where q.Tenant == tenant && q.Query.Id == queryId 
        //                                            select q).Select(a => new ObjectFieldPM()
        //                                            {
        //                                                AutomaticField = a.ObjectField.AutomaticField,
        //                                                CanFilter = a.ObjectField.CanFilter,
        //                                                ConverterName = a.ObjectField.ConverterName,
        //                                                DataTemplateName = a.ObjectField.DataTemplateName,
        //                                                DataTypeCode = a.ObjectField.DataTypeCode,
        //                                                DependencyFilter1Type = a.ObjectField.DependencyFilter1Type,
        //                                                DependencyFilter1Value = a.ObjectField.DependencyFilter1Value,
        //                                                DependencyFilter2Type = a.ObjectField.DependencyFilter2Type,
        //                                                DependencyFilter2Value = a.ObjectField.DependencyFilter2Value,
        //                                                DisplayInList = a.ObjectField.DisplayInList,
        //                                                DisplayInLookUpIndex = a.ObjectField.DisplayInLookUpIndex,
        //                                                DisplayInSearchWindowFilters = a.ObjectField.DisplayInSearchWindowFilters,
        //                                                DisplayInSearchWindowFiltersIndex = a.ObjectField.DisplayInSearchWindowFiltersIndex,
        //                                                DisplayInSearchWindowList = a.ObjectField.DisplayInSearchWindowList,
        //                                                DisplayInSearchWindowListIndex = a.ObjectField.DisplayInSearchWindowListIndex,
        //                                                DisplayOnLookUp = a.ObjectField.DisplayOnLookUp,
        //                                                DisplayOnly = a.ObjectField.DisplayOnly,
        //                                                FullNameTextCodeId = a.ObjectField.FullNameTextCodeId,
        //                                                FieldName = a.ObjectField.FieldName,
        //                                                ShortNameTextCodeId = a.ObjectField.ShortNameTextCodeId,
        //                                                HelpTextCodeId = a.ObjectField.HelpTextCodeId,
        //                                                Id = a.ObjectField.Id,
        //                                                IsCustom = a.ObjectField.IsCustom,
        //                                                IsCustomFilter = a.ObjectField.IsCustomFilter,
        //                                                IsMulti = a.ObjectField.IsMulti,
        //                                                IsRequiered = a.ObjectField.IsRequiered,
        //                                                IsTimeFrameFilter = a.ObjectField.IsTimeFrameFilter,
        //                                                ListTextCodeId = a.ObjectField.ListTextCodeId,
        //                                                ListPropertyPath = a.ObjectField.ListPropertyPath,
        //                                                LookUpControlName = a.ObjectField.LookUpControlName,
        //                                                LookUpTableId = a.ObjectField.LookUpTableId,
        //                                                MaxLength = a.ObjectField.MaxLength,
        //                                                MinLength = a.ObjectField.MinLength,
        //                                                MultiLine = a.ObjectField.MultiLine,
        //                                                MultiTableId = a.ObjectField.MultiTableId,
        //                                                ObjectTableId = a.ObjectField.ObjectTableId,
        //                                                ObjectTableName = a.ObjectField.ObjectTable.Name,
        //                                                Operator = a.ObjectField.Operator,
        //                                                PMPropertyPath = a.ObjectField.PMPropertyPath,
        //                                                SystemMaxLength = a.ObjectField.SystemMaxLength,
        //                                                SystemRequired = a.ObjectField.SystemRequired,
        //                                                Tenant = a.ObjectField.Tenant,
        //                                                UniqueField = a.ObjectField.UniqueField,
        //                                                ObjectTable_LookUpTableName = a.ObjectField.ObjectTable_LookUpTable != null ? a.ObjectField.ObjectTable_LookUpTable.Name : null,
        //                                                FullNameTextCodeDefaultText = a.ObjectField.FullNameTextCode != null ? a.ObjectField.FullNameTextCode.DefaultText : null,
        //                                                FullNameTextCodeCode = a.ObjectField.FullNameTextCode != null ? a.ObjectField.FullNameTextCode.Code : null,
        //                                                ShortNameTextCodeCode = a.ObjectField.ShortNameTextCode != null ? a.ObjectField.ShortNameTextCode.Code : null,
        //                                                HelpTextTextCodeCode = a.ObjectField.HelpTextCode != null ? a.ObjectField.HelpTextCode.Code : null,
        //                                                ListTextCodeCode = a.ObjectField.ListTextCode != null ? a.ObjectField.ListTextCode.Code : null,
        //                                                ObjectTable_MultiTableName = a.ObjectField.ObjectTable_MultiTable != null ? a.ObjectField.ObjectTable_MultiTable.Name : null,
        //                                                ListTextCodeDefaultText = a.ObjectField.ListTextCode != null ? a.ObjectField.ListTextCode.DefaultText : null,
        //                                                HelpTextCodeDefaultText = a.ObjectField.HelpTextCode != null ? a.ObjectField.HelpTextCode.DefaultText : null,
        //                                                ValidForQuerySection2 = a.ObjectField.ValidForQuerySection2,
        //                                                ValidForQuerySection1 = a.ObjectField.ValidForQuerySection1,
        //                                                IsRestrictable = a.ObjectField.IsRestrictable,
        //                                                DisplayInEntityVariables = a.ObjectField.DisplayInEntityVariables,
        //                                                DigitsAfterPoint = a.ObjectField.DigitsAfterPoint,
        //                                                TextCase = a.ObjectField.TextCase,
        //                                                DisplayInLookupColumnSize = a.ObjectField.DisplayInLookupColumnSize,
        //                                                ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
        //                                            }).ToList();

        //    ObjectFieldValidationRepository objectFieldValidationRepository = new ObjectFieldValidationRepository(Context);
        //    List<ObjectFieldValidationPM> objectFieldValidations = objectFieldValidationRepository.GetObjectFieldValidationPMsByTenant(tenant).ToList();

        //    foreach (ObjectFieldPM objectField in objectfields)
        //    {

        //        objectField.ObjectFieldValidations = (from d in objectFieldValidations
        //                                              where d.ObjectFieldId == objectField.Id
        //                                              select d).ToList();
        //    }

        //    return objectfields;
        //}



        public IQueryable<ObjectField> GetObjectFieldsByTenant(int tenant)
        {
            return from a in context.ObjectFields//.Include("ListFieldLableTextCode").Include("FieldLableTextCode").Include("ObjectTable")
                   where a.Tenant == tenant
                   select a;
            //return  context.ObjectFields.Where(d => d.Tenant == tenant);
        }
        public ObjectField GetSingleObjectField(string id)
        {
            return (from a in context.ObjectFields
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public ObjectFieldModification GetObjectFieldModificationByObjectField(string objectfieldCode, int tenant)
        {
            return (from a in context.ObjectFieldModifications
                    where a.Tenant == tenant && a.ObjectFieldCode == objectfieldCode
                    select a).FirstOrDefault();
        }

        public ObjectFieldModification GetLastObjectFieldModificationByTenant(int tenant)
        {
            return (from a in context.ObjectFieldModifications
                    where a.Tenant == tenant && a.UpdateDateGMT != null
                    select a).OrderByDescending(a => a.UpdateDateGMT).FirstOrDefault();
        }

        public List<ObjectFieldModification> GetAllObjectFieldModificationByTenant(int tenant)
        {
            return (from a in context.ObjectFieldModifications
                    where a.Tenant == tenant
                    select a).ToList();
        }

        public void Add(ObjectField entity)
        {
            context.ObjectFields.Add(entity);

        }

        public void Remove(ObjectField entity)
        {
            context.ObjectFields.Attach(entity);
            context.ObjectFields.Remove(entity);
        }

        public void Update(ObjectField entity)
        {
            context.ObjectFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ObjectField> All()
        {
            return context.ObjectFields.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public ObjectField GetSingleObjectFieldById(string id, int tenant)
        {
            var field = (from a in context.ObjectFields.Include("FullNameTextCode").Include("ListTextCode")
                         where a.Id == id
                         select a).FirstOrDefault();
            if (field == null)
                field = (from a in context.ObjectFields.Include("FullNameTextCode").Include("ListTextCode")
                         where a.FieldCode == id
                         select a).FirstOrDefault();

            return field;
        }

        public ObjectField GetSingleObjectFieldByCode(string code, int tenant)
        {
            return (from a in context.ObjectFields.Include("FullNameTextCode").Include("ListTextCode")
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public ObjectField GetSingleObjectFieldByCode(string code, string objectTableId, int tenant)
        {
            return (from a in context.ObjectFields
                    where a.Code == code && a.ObjectTableId == objectTableId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public ObjectField GetSingleObjectFieldByFieldName(string fieldName, string objectTableId, int tenant)
        {
            return (from a in context.ObjectFields
                    where a.FieldName == fieldName && a.ObjectTableId == objectTableId
                    select a).FirstOrDefault();
        }

        public IQueryable<ObjectField> GetObjectFieldsFromTenanZeroAndMyTenant(int tenant)
        {
            return context.ObjectFields.Include("FullNameTextCode").Include("ListTextCode").Where(t => t.Tenant == tenant || t.Tenant == 0);
        }


        public List<ObjectField> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ObjectField GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }



        public IQueryable<ObjectField> GetObjectFieldsByFieldsNamesAndObjectTable(List<string> fieldsNames, string objectTableId)
        {
            return from a in context.ObjectFields
                   where a.ObjectTableId == objectTableId && fieldsNames.Contains(a.FieldName) && a.Tenant == 0
                   select a;

        }




    }
}

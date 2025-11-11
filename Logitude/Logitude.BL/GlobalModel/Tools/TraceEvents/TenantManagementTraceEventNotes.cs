using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Logitude.BL.GlobalModel.Tools.TraceEvents
{
    internal class TenantManagementTraceEventNotes
    {
        public static string CreateNotes(TenantManagementPM entityPM, TenantManagement poco)
        {
            int tenantId = entityPM.Id;
            StringBuilder notes = new StringBuilder();
            PropertyInfo[] properties = typeof(TenantManagementPM).GetProperties();

            GlobalTenant globalTenant = new GlobalTenantRepository().GetGlobalTenantsByTenant(tenantId);
            Tenant tenant = new TenantRepository(tenantId).GetSingleTenant(tenantId);
            LogBoxTenantSetting LBtenantsetting = new LogBoxTenantSettingRepository(tenantId).GetSingleLBTenant(tenantId);

            PackageRepository packageRepository = new PackageRepository(tenantId);

            foreach (PropertyInfo property in properties)
            {
                Type propertyType = property.PropertyType;
                if (property.Name == "SearchFields")
                    continue;

                if (propertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(propertyType))
                {
                    notes.AppendLine(CollectionsEqual(entityPM, property));
                    continue;
                }
                object pocoInstance;
                if (tenant == null)
                    continue;
                if (LBtenantsetting == null)
                     pocoInstance = new object[] { poco, tenant, globalTenant }.FirstOrDefault(x => x.GetType().GetProperty(property.Name) != null);
                else
                     pocoInstance = new object[] { poco, tenant, globalTenant, LBtenantsetting }.FirstOrDefault(x => x.GetType().GetProperty(property.Name) != null);
                if (pocoInstance == null)
                    continue;

                object pocoValue = pocoInstance.GetType().GetProperty(property.Name).GetValue(pocoInstance);
                object entityValue = property.GetValue(entityPM);

                if (IsPrimitive(propertyType) && !Equals(entityValue, pocoValue))
                    AddFieldChangeToNotes(property, pocoInstance, pocoValue, entityValue, tenantId, notes);
            }

            string result = notes.ToString().Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            return result.Trim();
        }

        private static bool IsPrimitive(Type propertyType) =>
            propertyType.IsPrimitive || propertyType.IsEnum || propertyType == typeof(string) || propertyType == typeof(DateTime) || propertyType == typeof(DateTime?);

        private static void AddFieldChangeToNotes(PropertyInfo property, object pocoInstance, object pocoValue, object entityValue, int tenantId, StringBuilder notes)
        {
            string fieldName = property.Name;
            string tableName = pocoInstance.GetType().BaseType.Name;
            ObjectFieldPM objectField = new ObjectFieldQuery(tenantId).GetObjectFieldPMsByObjectTableName(tableName, tenantId).FirstOrDefault(x => x.FieldName == fieldName);

            if (objectField == null)
                return;
            string description = objectField.FullNameTextCodeDefaultText;



            if (objectField.LookUpTableId != null)
            {
                string lookupTableName = new ObjectTableRepository(tenantId).GetObjectTableById(objectField.LookUpTableId, tenantId).Name;

                object newEntity = entityValue == null ? null : GetEntityByKey(lookupTableName, entityValue, tenantId);
                object oldEntity = pocoValue == null ? null : GetEntityByKey(lookupTableName, pocoValue, tenantId);

                IEnumerable<PropertyInfo> fieldsProperties = new ObjectFieldQuery(tenantId).GetObjectFieldPMsByObjectTableName(lookupTableName, tenantId)
                    .Where(field => field.DisplayOnLookUp || field.DisplayOnLookUpLocal)
                    .Select(field => (newEntity ?? oldEntity).GetType().GetProperty(field.FieldName))
                    .Where(x => x != null);

                List<string> oldValues = new List<string>();
                List<string> newValues = new List<string>();
                foreach (PropertyInfo prop in fieldsProperties)
                {
                    if (oldEntity != null)
                        oldValues.Add(prop.GetValue(oldEntity).ToString());
                    if (newEntity != null)
                        newValues.Add(prop.GetValue(newEntity).ToString());
                }

                if (newValues.All(val => val != entityValue?.ToString()) && oldValues.All(val => val != pocoValue?.ToString()))
                {
                    oldValues.Add(pocoValue?.ToString());
                    newValues.Add(entityValue?.ToString());
                }
                if (oldEntity != null)
                    pocoValue = string.Join(" ", oldValues);
                if (newEntity != null)
                    entityValue = string.Join(" ", newValues);
            }

            notes.AppendLine($"FIELD {property.Name}={description} Updated from {pocoValue} to {entityValue}, ");
        }

        public static object GetEntityByKey(string entityName, object keyValue, int tenantId)
        {
            if (string.IsNullOrWhiteSpace(entityName)) throw new ArgumentNullException(nameof(entityName));
            if (keyValue == null) throw new ArgumentNullException(nameof(keyValue));

            DbContext[] contexts = new DbContext[]
            {
                GlobalContext.GetContext(tenantId) as GlobalContext,
                WebFreightContext.GetContext(tenantId) as WebFreightContext,
                CommonDataContext.GetContext(tenantId) as CommonDataContext
            };

            EntityType entityType = null;
            DbContext context = null;
            foreach (DbContext _context in contexts)
            {
                entityType = GetEntityType(entityName, _context);
                context = _context;
                if (entityType != null)
                    break;
            }

            if (entityType == null)
                throw new ArgumentException($"Entity type '{entityName}' not found in the DbContext.");

            EdmProperty keyProperty = entityType.KeyProperties.FirstOrDefault();
            if (keyProperty == null)
                throw new InvalidOperationException($"Entity '{entityName}' does not have a primary key.");

            List<Type> types = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("Microsoft") || assembly.FullName.StartsWith("System"))
                    continue;
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Add the types that DID load
                    types.AddRange(ex.Types.Where(t => t != null));

                    // Optional: log detailed loader exceptions
                    foreach (var loaderEx in ex.LoaderExceptions)
                    {
                        // Replace with your logging mechanism
                        Console.WriteLine($"LoaderException: {loaderEx.Message}");
                    }
                }
                catch (Exception ex)
                {
                    // Log other unexpected exceptions (optional)
                    Console.WriteLine($"General Exception loading types from {assembly.FullName}: {ex.Message}");
                }
            }


            Type entityClrType = types.Where(t => t.Name == entityName && t.FullName.Contains("EntityPOCOs")).Last();

            if (entityClrType == null)
                throw new InvalidOperationException($"CLR type for entity '{entityName}' not found.");

            DbSet dbSet = context.Set(entityClrType);

            ParameterExpression parameter = Expression.Parameter(entityClrType, "e");
            MemberExpression property = Expression.Property(parameter, keyProperty.Name);
            BinaryExpression condition = Expression.Equal(property, Expression.Constant(keyValue));
            LambdaExpression lambda = Expression.Lambda(condition, parameter);

            MethodInfo method = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "SingleOrDefault" && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityClrType);

            object result = method.Invoke(null, new object[] { dbSet, lambda });
            return result;
        }

        private static EntityType GetEntityType(string entityName, DbContext context)
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            ReadOnlyCollection<EntityType> tables = objectContext.MetadataWorkspace
                .GetItems<EntityType>(DataSpace.CSpace);
            EntityType entityType = objectContext.MetadataWorkspace
                .GetItems<EntityType>(DataSpace.CSpace)
                .FirstOrDefault(e => e.Name == entityName);
            return entityType;
        }

        private static string CollectionsEqual(TenantManagementPM entityPM, PropertyInfo property)
        {
            StringBuilder notes = new StringBuilder();

            if (property.Name == "TenantManagementLicenses" && entityPM.TenantManagementLicenses.Any(a => a.ChangeSetOp != ChangeSetOperation.None))
                notes.AppendLine("Additional packages were chenage");

            else if (property.Name == "AddOns")
            {
                List<TenantAddOnPM> addOnsChanged = entityPM.AddOns.Where(a => a.ChangeSetOp != ChangeSetOperation.None).ToList();
                addOnsChanged.ForEach(addOn =>
                {
                    PackagePM packege = new PackageQuery(entityPM.Id).GetSinglePM(addOn.PackageCode);

                    if (addOn.ChangeSetOp == ChangeSetOperation.Insert)
                        notes.AppendLine($"AddOn {addOn.Id} package {addOn.PackageCode} {packege.Name} was added");
                    else if (addOn.ChangeSetOp == ChangeSetOperation.Delete)
                        notes.AppendLine($"AddOn {addOn.Id} package {addOn.PackageCode} {packege.Name} was removed");
                    else if (addOn.ChangeSetOp == ChangeSetOperation.Update)
                    {
                        TenantAddOn oldAddOn = new TenantAddOnRepository(entityPM.Id).GetSingleTenantAddOn(addOn.Id);
                        PackagePM oldPackege = new PackageQuery(entityPM.Id).GetSinglePM(oldAddOn.PackageCode);
                        notes.AppendLine($"AddOn {addOn.Id} update from package {oldAddOn.PackageCode} {oldPackege.Name} to {addOn.PackageCode} {packege.Name}");
                    }
                });
            }

            return notes.ToString();
        }
    }
}

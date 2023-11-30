using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Logitude.BL.Helpers.CustomFieldsResolver;
using Logitude.Server.Tools.CustomFields;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.Helpers
{
    public class CustomFieldResolver
    {
        public Object lockMe = new Object();
        public Dictionary<string, object> definedObjects = new Dictionary<string, object>();
        public CultureInfo en = new CultureInfo("en-US");
        public bool UsingParallelMechanisim;
        public List<CustomPickList> customPickLists;
        public List<LookUpFieldDataStorage> LookUpFieldsDataStorage;
        public CustomFieldResolver(int tenant)
        {
            CustomPickListRepository customPickListRepository = new CustomPickListRepository(tenant);
            customPickLists = customPickListRepository.GetCustomPickListsCash(tenant);
            LookUpFieldsDataStorage = new List<LookUpFieldDataStorage>();
        }
        public void SetFieldValue(CustomFieldResolverArgs customFieldResolverArgs)
        {
            CustomFieldSingleSetter.SetFieldValue(customFieldResolverArgs);
        }

        public void SetCustomFieldsValues(string objectTableName, int tenant, List<object> listQuery)
        {

            CustomFieldMultiSetter.SetCustomFieldsValues(objectTableName, tenant, listQuery);
        }

        public void SetDataProviderCustomFieldsValues(string objectTableName, int tenant, Object entity, Object provider, string propertyIdientifier = null)
        {
            CustomFieldMultiSetter.SetDataProviderCustomFieldsValues(new CustomFieldDataProviderSetterValueArgs { 
                objectTableName = objectTableName,
                tenant = tenant,
                entity = entity,
                provider = provider,
                propertyIdientifier = propertyIdientifier
            });
        }

        public string GetFieldValue(object entity, ObjectField objectField, int tenant, bool externalAPICall = false)
        {
            return CustomFieldGetter.Get(new CustomFieldGetterArgs { 
                entity = entity,
                objectField = objectField,
                tenant = tenant,
                customFieldResolver = this,
                externalAPICall = externalAPICall,
            });
        }

        public string GetFieldValue2(object value, ObjectField objectField, int tenant)
        {
            return CustomFieldGetter.Get2(value, objectField, tenant, this);
        }
    }

    public class CustomFieldResolverArgs
    {
        public string ObjectTableName { get; set; }
        public object EntityPM { get; set; }
        public string FieldCode { get; set; }
        public string FieldValue { get; set; }
        public int Tenant { get; set; }
    }

    public class LookUpFieldValueGetterArgs
    {
        public ObjectField ObjectField { get; set; }
        public int Tenant { get; set; }
        public bool ExternalAPICall { get; set; }
        public object Value { get; set; }
        public string InsideTypePath { get; set; }
        public Type InsideEntityType { get; set; }
    }

    public class LookUpFieldDataStorage
    {
        public string FieldValue { get; set; }
        public object Value { get; set; }
        public string LookUpTableId { get; set; }
        public int Tenant { get; set; }
    }
}

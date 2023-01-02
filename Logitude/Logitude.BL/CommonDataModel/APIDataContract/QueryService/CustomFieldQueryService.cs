using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.Reflection;
using Logitude.BL.Helpers;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public class CustomFieldQueryService
    {
        IWebFreightContext context;
        ObjectFieldRepository fieldsRepository;
        ObjectTableRepository tablesRepository;
        public string TableName { get; private set; }
        public ObjectTable Table { get; set; }
        List<ObjectField> TableCustomFields = new List<ObjectField>();
        public CustomFieldQueryService(int tenant, string tableName)
        {
            this.TableName = tableName;
            context = WebFreightContext.GetContext(tenant);
            fieldsRepository = new ObjectFieldRepository(context);
            tablesRepository = new ObjectTableRepository(context);

            this.Table = tablesRepository.GetObjectTableByName(tableName, tenant, true);
            this.TableCustomFields = fieldsRepository.GetCustomObjectFields(this.Table.Id, tenant);

        }

        public List<CustomField> CustomFieldCustomDataMapping(object entityPM, int tenant)
        {
            List<CustomField> customFields = new List<CustomField>();
            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);

            foreach (ObjectField field in this.TableCustomFields)
            {
                string stringValue = customFieldResolver.GetFieldValue(entityPM, field, tenant, true);
                if (!string.IsNullOrEmpty(stringValue))
                {
                    customFields.Add(new CustomField() { Code = field.Code, Value = stringValue });
                }
            }

            return customFields;
        }

        public void CustomFieldCustomDataMappingAndValidatin(List<CustomField> fields, object entity, int tenant)
        {
            foreach (CustomField field in fields)
            {
                ObjectField obField = this.TableCustomFields.FirstOrDefault(f => f.Code == field.Code);
                if (obField != null)
                {
                    object resultValue = " ";
                    PropertyInfo propertyPathPi = entity.GetType().GetProperty(obField.FieldName);
                    if (propertyPathPi != null)
                    {
                        string strValue = FieldValueResolver.GetCustomFieldStringValueForAPI(obField, field.Value, tenant);
                        CustomFieldClass classvalue = propertyPathPi.GetValue(entity, null) as CustomFieldClass;
                        if (classvalue != null)
                        {
                            classvalue.Value = strValue;
                        }
                        else
                        {
                            classvalue = new CustomFieldClass(obField.FieldName, this.TableName, strValue);
                        }

                        propertyPathPi.SetValue(entity, classvalue, null);
                    }
                }
                else
                {
                    throw new Exception("Couldn't find a Custom Field with code: " + field.Code);
                }
            }
        
        }

    }
}

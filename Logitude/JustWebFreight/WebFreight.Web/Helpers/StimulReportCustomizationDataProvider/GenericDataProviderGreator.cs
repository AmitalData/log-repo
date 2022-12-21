using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.Helpers.StimulReportCustomizationDataProvider
{
    public class DocumentDataProviderGreator: FieldDataProviderService
    {
        private List<Field> fields;
        private DocumentDataProviderArgs documentDataProviderArgs;


        public DocumentDataProviderGreator(DocumentDataProviderArgs documentDataProviderArgs) : base(documentDataProviderArgs)
        {
            this.documentDataProviderArgs = documentDataProviderArgs;
            if (documentDataProviderArgs.DataProvider != null)
            {
                documentDataProviderArgs.Type = documentDataProviderArgs.DataProvider.GetType();
            }
        }

        public DocumentDataProvider Create(bool withData = false)
        {
            fields = GetFields();
            Type type = GenericClassCreator.Create(documentDataProviderArgs.Type.Name, fields);
            if(!withData) return new DocumentDataProvider() { Type = type , Name = type.Name.Split('_')[0] };
            var newDataProvider = Activator.CreateInstance(type);
            MapCurrentDataProviderValueToNewDataProvider(documentDataProviderArgs.DataProvider, newDataProvider, type);
            new CustomChildDataProviderService(fields, documentDataProviderArgs ,objectTable.Name).Set(newDataProvider);
            new CustomFieldDataProviderService(fields, documentDataProviderArgs, objectTable.Name).Set(newDataProvider);

            return new DocumentDataProvider() { Type = type, Name = type.Name.Split('_')[0], BusinessObjectValue = newDataProvider };
        }




        private void MapCurrentDataProviderValueToNewDataProvider(object currentDataProvider, object newDataProvider, Type type)
        {
            var currentDataProviderProperties = currentDataProvider.GetType().GetProperties().ToDictionary(e => e.Name, e => e);
            var newDataProviderProperties = type.GetProperties().Where(d=> currentDataProviderProperties.ContainsKey(d.Name)).ToList();

            foreach (var property in newDataProviderProperties)
            {
                property.SetValue(newDataProvider, currentDataProviderProperties[property.Name].GetValue(currentDataProvider));
            }
        }


    }

    public class DocumentDataProviderArgs
    {
        public object DataProvider { get; set; }
        public Type Type { get; set; }
        public string EntityId { get; set; }
        public object EntityPM { get; set; }
        public DocumentTypeTemplatePM DocumentTypeTemplatePM { get; set; }
        public string Category { get; set; }

        


    }
    public class DocumentDataProvider
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public object BusinessObjectValue { get; set; }
    }
}
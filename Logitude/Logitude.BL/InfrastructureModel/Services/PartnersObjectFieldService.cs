using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.InfrastructureModel.Services
{
    class PartnersObjectFieldService
    {
        private IWebFreightContext objectContext;
        ObjectFieldService objectFieldService;
        ObjectFieldQuery objectFieldQuery;
        int tenant;
        string[] partnerObjectTableTypes;
        private List<string> relatedPartnerObjectTables;
        private List<string> partnersObjectTableIds;

        public PartnersObjectFieldService(IWebFreightContext objectContext, int tenant)
        {
            this.objectContext = objectContext;
            objectFieldService = new ObjectFieldService(objectContext, tenant);
            this.tenant = tenant;
            objectFieldQuery = new ObjectFieldQuery(tenant);
            partnerObjectTableTypes = new string[] {"AccountingPartner", "Agent", "Airline", "CustomClearance", "CustomAgent", "CustomsShipper", "Coloader", "Customer", "Freelancer", "PotentialCustomer", "Participant",
            "ShippingAgent", "ShippingLine", "Trucker", "Vendor", "Warehouse" };
            relatedPartnerObjectTables = new List<string>();
            partnersObjectTableIds = new List<string>();
        }
        public void Create(ObjectFieldPM objectField)
        {
            if (string.IsNullOrEmpty(objectField.RelatedEntities))
                return;
            relatedPartnerObjectTables = objectField.RelatedEntities.Split(',').Where(d=> !string.IsNullOrEmpty(d)).ToList();
            foreach(string partnerObjectTable in relatedPartnerObjectTables)
            {
                CreateObjectField(objectField, partnerObjectTable);
            }
        }

        private void CreateObjectField(ObjectFieldPM objectField, string partner)
        {
            objectField.ObjectTableId = ObjectTableQuery.GetObjectTableByCode(partner, 0)?.Id;
            if (String.IsNullOrEmpty(objectField.ObjectTableId))
                return;
            objectField.IsRelatedEntity = true;
            objectField.FullNameTextCodeCode = objectField.FullNameTextCodeDefaultText;
            objectField.HelpTextCodeCode = objectField.HelpTextCodeDefaultText;
            objectField.ListTextCodeCode = objectField.ListTextCodeDefaultText;
            objectFieldService.Create(objectField);
        }

        public void Update(ObjectFieldPM objectField)
        {
            if (string.IsNullOrEmpty(objectField.RelatedEntities))
            return;
            relatedPartnerObjectTables = objectField.RelatedEntities.Split(',').Where(d => !string.IsNullOrEmpty(d)).ToList();
            foreach (string partnerObjectTable in relatedPartnerObjectTables)
            {
                partnersObjectTableIds.Add(ObjectTableQuery.GetObjectTableByCode(partnerObjectTable, tenant)?.Id);
            }


            List<ObjectFieldPM> partnersObjectFields = objectFieldQuery.GetObjectFieldByTenant(tenant).Where(field => partnerObjectTableTypes.Contains(field.ObjectTableName) && field.FieldName == objectField.FieldName).ToList();
            UpdatePartnersObjectField(objectField, partnersObjectFields);

           AddNewPartnersObjectField(objectField, partnersObjectFields);
        }

        private void AddNewPartnersObjectField(ObjectFieldPM objectField, List<ObjectFieldPM> partnersObjectFields)
        {
            var partnerObjectTableIds = partnersObjectTableIds.Where(d => !partnersObjectFields.Select(field => field.ObjectTableId).ToList().Contains(d)).ToList();
            foreach (string partnerObjectTableId in partnerObjectTableIds)
            {
                objectField.ObjectTableId = partnerObjectTableId;
                objectField.ObjectTableName = ObjectTableQuery.GetSingleObjectTableById(partnerObjectTableId, 0).Name;
                objectField.IsRelatedEntity = true;
                objectField.FullNameTextCodeCode = objectField.FullNameTextCodeDefaultText;
                objectField.HelpTextCodeCode = objectField.HelpTextCodeDefaultText;
                objectField.ListTextCodeCode = objectField.ListTextCodeDefaultText;
                objectFieldService.Create(objectField);
            }
        }

        private void UpdatePartnersObjectField(ObjectFieldPM objectField, List<ObjectFieldPM> partnersObjectFields)
        {
            foreach (var partnerObjectField in partnersObjectFields)
            {
                ObjectFieldPM mappedField = MapPartnerObjectField(objectField, partnerObjectField);
                objectFieldService.Update(mappedField, false);
            }
        }

        private ObjectFieldPM MapPartnerObjectField(ObjectFieldPM objectField, ObjectFieldPM partnerObjectField)
        {
            partnerObjectField.FullNameTextCodeDefaultText = objectField.FullNameTextCodeDefaultText;
            partnerObjectField.ListTextCodeDefaultText = objectField.ListTextCodeDefaultText;
            partnerObjectField.HelpTextCodeDefaultText = objectField.HelpTextCodeDefaultText;
            partnerObjectField.IsRequiered = objectField.IsRequiered;
            partnerObjectField.MaxLength = objectField.MaxLength;
            partnerObjectField.MinLength = objectField.MinLength;
            partnerObjectField.MultiLine = objectField.MultiLine;
            partnerObjectField.DisplayOnly = objectField.DisplayOnly;
            partnerObjectField.DefaultAdditionalTreeFilters = objectField.DefaultAdditionalTreeFilters;
            return partnerObjectField;
        }
    }
}

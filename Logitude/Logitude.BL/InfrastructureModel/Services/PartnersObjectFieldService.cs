using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Services
{
    class PartnersObjectFieldService
    {
        private IWebFreightContext objectContext;
        ObjectFieldService objectFieldService;
        ObjectFieldQuery objectFieldQuery;
        int tenant;
        string[] partnerObjectTableTypes;

        public PartnersObjectFieldService(IWebFreightContext objectContext, int tenant)
        {
            this.objectContext = objectContext;
            objectFieldService = new ObjectFieldService(objectContext, tenant);
            this.tenant = tenant;
            objectFieldQuery = new ObjectFieldQuery(tenant);
            partnerObjectTableTypes = new string[] {"AccountingPartner", "Agent", "AirLine", "CustomClearance", "CustomAgent", "CustomsShipper", "Coloader", "Customer", "Freelancer", "PotentialCustomer", "Participant",
    "ShippingAgent", "ShippingLine", "Trucker", "Vendor", "Warehouse" };
        }
        public void Create(ObjectFieldPM objectField)
        {
            string[] partners = objectField.RelatedEntities.Split(',');
            foreach(string partner in partners)
            {
                if(!String.IsNullOrEmpty(partner))
                {
                    MapAndCreateNewPartnerField(objectField, partner);
                }
            }
        }

        private void MapAndCreateNewPartnerField(ObjectFieldPM objectField, string partner)
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
            string[] partners = objectField.RelatedEntities.Split(',');
            List<string> selectedPartnersIds = new List<string>();
            foreach (string partner in partners)
            {
                if (!String.IsNullOrEmpty(partner))
                {
                    selectedPartnersIds.Add(ObjectTableQuery.GetObjectTableByCode(partner, tenant)?.Id);
                }
            }
            //List<ObjectField> AvailableFieldsCopiesToUpdate = objectContext.ObjectFields
            //                                                               .Where(Field => Field.FieldName == objectField.FieldName && selectedPartnersIds.Contains(Field.ObjectTableId))
            //                                                               .Select(Field => Field)
            //                                                               .ToList();
            List<ObjectFieldPM> AvailableFieldsCopiesToUpdate = objectFieldQuery.GetObjectFieldByTenant(tenant).Where(field => partnerObjectTableTypes.Contains(field.ObjectTableName) && field.FieldName == objectField.FieldName).ToList();
            List<string> updatedObjectFieldsTablesIDs = new List<string>();
            UpdateAvailableCopies(objectField, AvailableFieldsCopiesToUpdate, updatedObjectFieldsTablesIDs);

            CreateFieldForNewSelectedPartners(objectField, selectedPartnersIds, updatedObjectFieldsTablesIDs);
        }

        private void CreateFieldForNewSelectedPartners(ObjectFieldPM objectField, List<string> selectedPartnersIds, List<string> updatedObjectFieldsTablesIDs)
        {
            var newPartnersIds = selectedPartnersIds.Where(partnerId => !updatedObjectFieldsTablesIDs.Contains(partnerId)).ToList();
            foreach (string partnerId in newPartnersIds)
            {
                if (!String.IsNullOrEmpty(partnerId))
                {
                    objectField.ObjectTableId = partnerId;
                    objectField.ObjectTableName = ObjectTableQuery.GetSingleObjectTableById(partnerId, 0).Name;
                    objectField.IsRelatedEntity = true;
                    objectField.FullNameTextCodeCode = objectField.FullNameTextCodeDefaultText;
                    objectField.HelpTextCodeCode = objectField.HelpTextCodeDefaultText;
                    objectField.ListTextCodeCode = objectField.ListTextCodeDefaultText;
                    objectFieldService.Create(objectField);
                }
            }
        }

        private void UpdateAvailableCopies(ObjectFieldPM objectField, List<ObjectFieldPM> AvailableFieldsCopiesToUpdate, List<string> updatedObjectFieldsTablesIDs)
        {

            foreach (var field in AvailableFieldsCopiesToUpdate)
            {
                ObjectFieldPM mappedField = GetNewUpdatedPM(objectField, field, updatedObjectFieldsTablesIDs);
                objectFieldService.Update(mappedField, false);
            }
        }

        private ObjectFieldPM GetNewUpdatedPM(ObjectFieldPM objectField, ObjectFieldPM field, List<string> updatedObjectFieldsTablesIDs)
        {
            field.FullNameTextCodeDefaultText = objectField.FullNameTextCodeDefaultText;
            field.ListTextCodeDefaultText = objectField.ListTextCodeDefaultText;
            field.HelpTextCodeDefaultText = objectField.HelpTextCodeDefaultText;
            field.FullNameTextCodeCode = objectField.FullNameTextCodeCode;
            field.ListTextCodeCode = objectField.ListTextCodeCode;
            field.HelpTextCodeCode = objectField.HelpTextCodeDefaultText;
            field.IsRequiered = objectField.IsRequiered;
            field.MaxLength = objectField.MaxLength;
            field.MinLength = objectField.MinLength;
            field.MultiLine = objectField.MultiLine;
            field.DisplayOnly = objectField.DisplayOnly;
            field.DefaultAdditionalTreeFilters = objectField.DefaultAdditionalTreeFilters;
            field.IsCustom = true;
            //ObjectFieldPM mappedField = new ObjectFieldPM()
            //{
            //    //Id = field.Id,
            //    //ObjectTableId = field.ObjectTableId,
            //    //ObjectTableName = ObjectTableQuery.GetSingleObjectTableById(field.ObjectTableId, 0).Name,
            //    //Tenant = field.Tenant,
            //    //FieldName = field.FieldName,
            //    //FieldCode = field.FieldCode,
            //    //Code = field.Code,
            //    //CustomPickListCode = field.CustomPickListCode,
            //    //LookUpTableId = field.LookUpTableId,
            //    //DataTypeCode = field.DataTypeCode,
            //    //DigitsAfterPoint = field.DigitsAfterPoint,
            //    //NumberOfDigits = field.NumberOfDigits,
            //    //CanFilter = objectField.CanFilter,
            //    //FullNameTextCodeId = objectField.FullNameTextCodeId,
            //    //ListTextCodeId = objectField.ListTextCodeId,
            //    //HelpTextCodeId = objectField.HelpTextCodeId,
            //    //FullNameTextCodeCode = objectField.FullNameTextCodeDefaultText,
            //    //HelpTextCodeCode = objectField.HelpTextCodeDefaultText,
            //    //ListTextCodeCode = objectField.ListTextCodeDefaultText,
            //    FullNameTextCodeDefaultText = objectField.FullNameTextCodeDefaultText,
            //    ListTextCodeDefaultText = objectField.ListTextCodeDefaultText,
            //    HelpTextCodeDefaultText = objectField.HelpTextCodeDefaultText,
            //    FullNameTextCodeCode = objectField.FullNameTextCodeCode,
            //    ListTextCodeCode = objectField.ListTextCodeCode,
            //    HelpTextCodeCode = objectField.HelpTextCodeDefaultText,
            //    IsRequiered = objectField.IsRequiered,
            //    MaxLength = objectField.MaxLength,
            //    MinLength = objectField.MinLength,
            //    MultiLine = objectField.MultiLine,
            //    DisplayOnly = objectField.DisplayOnly,
            //    DefaultAdditionalTreeFilters = objectField.DefaultAdditionalTreeFilters,
            //    IsCustom = true,
            //};
            updatedObjectFieldsTablesIDs.Add(field.ObjectTableId);
            return field;
        }
    }
}

using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class ARInvoiceAutomationService
    {
        private ARInvoicePM entityPM = null;
        private ARInvoice poco = null;
        private string objectTableName = string.Empty;
        private List<ObjectFieldPM> automationObjectFields = null;
        private int tenant;

        public ARInvoiceAutomationService(ARInvoicePM entityPM, ARInvoice poco)
        {
            this.entityPM = entityPM;
            this.poco = poco;
            this.tenant = entityPM.Tenant;
            this.objectTableName = "ARInvoice";
            this.automationObjectFields = GetObjectFieldsUsedInAutomation();
        }


        public void RunAutomation(string automationType)
        {
            EntityChangeHelper entityChangeHelper = new EntityChangeHelper();
            if (automationType == "OnCreate")
            {
                entityChangeHelper.AddEntityChange(entityPM, null, automationType, "", objectTableName, DateTime.Now);
            }
            else
            {
                ARInvoiceChangeTracking aRInvoiceChangeTracking = BuildARInvoiceChangeTracking();
                entityChangeHelper.AddEntityChange(entityPM, aRInvoiceChangeTracking.ChangeTrackingPM, automationType, aRInvoiceChangeTracking.EntityChangeFieldXml, objectTableName, DateTime.Now);
            }
        }

        private string GetAutomationProessType()
        {
            return poco == null ? "OnCreate" : "OnUpdate";
        }

        private List<ObjectFieldPM> GetObjectFieldsUsedInAutomation()
        {
            List<ObjectFieldPM> objectFieldLists = new List<ObjectFieldPM>();
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objecttable != null)
            {
                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(tenant);
                objectFieldLists = objectFieldQuery.GetObjectFieldsAllowedinAutomationConditionsPMsByObjectTableId(objecttable.Id, tenant);
            }
            return objectFieldLists;
        }

        private ARInvoiceChangeTracking BuildARInvoiceChangeTracking()
        {
            ARInvoiceChangeTracking arInvoiceChangeTracking = new ARInvoiceChangeTracking() { ChangeTrackingPM = new ARInvoicePM() };
            MapARInvoiceToARInvoicePMForAutomation(arInvoiceChangeTracking.ChangeTrackingPM, poco);
            arInvoiceChangeTracking.NotifyPropertyChangeValuesLists = BuildChangedProperties(entityPM, arInvoiceChangeTracking.ChangeTrackingPM);
            arInvoiceChangeTracking.EntityChangeFieldXml = EntityPMChangeTrackingHelper.GetChangesDetectedXml(arInvoiceChangeTracking.NotifyPropertyChangeValuesLists);
            return arInvoiceChangeTracking;
        }

        private void MapARInvoiceToARInvoicePMForAutomation(ARInvoicePM changeTrackingPM, ARInvoice poco)
        {
            if (automationObjectFields != null)
            {
                foreach (ObjectFieldPM objectFieldPM in automationObjectFields)
                {
                    if (objectFieldPM.FieldName == "BillToId")
                    {

                    }
                    object value = GetPropertyValue(poco, objectFieldPM.FieldName);
                    PropertyInfo propInfo = changeTrackingPM.GetType().GetProperty(objectFieldPM.FieldName);
                    propInfo.SetValue(changeTrackingPM, value, null);
                }
            }

        }

        private object GetPropertyValue(object entity, string fieldName)
        {
            Type type = entity.GetType();
            PropertyInfo propertyInf = type.GetProperty(fieldName);
            object value = null;
            if (propertyInf != null)
            {
                value = propertyInf.GetValue(entity, null);
            }
            return value;
        }

        public List<NotifyPropertyChangeValues> BuildChangedProperties(ARInvoicePM entityPM, ARInvoicePM changeTrackingPM)
        {
            List<NotifyPropertyChangeValues> notifyPropertyChangeValuesLists = new List<NotifyPropertyChangeValues>();
            if (automationObjectFields != null)
            {
                foreach (ObjectFieldPM objectFieldPM in automationObjectFields)
                {

                    if(objectFieldPM.FieldName == "BillToId")
                    {

                    }
                    object oldValue = GetPropertyValue(changeTrackingPM, objectFieldPM.FieldName);
                    object newValue = GetPropertyValue(entityPM, objectFieldPM.FieldName);
                    NotifyPropertyChangeValues notifyPropertyChangeValues = GetNotifyPropertyChangeValues(new NotifyPropertyChangeArgs() { PropertyName = objectFieldPM.FieldName, PropertyType = objectFieldPM.DataTypeCode, OldValue = oldValue, NewValue = newValue, IsCustom = objectFieldPM.IsCustom });
                    if (notifyPropertyChangeValues != null) notifyPropertyChangeValuesLists.Add(notifyPropertyChangeValues);
                }
            }
            return notifyPropertyChangeValuesLists;

        }


        private NotifyPropertyChangeValues GetNotifyPropertyChangeValues(NotifyPropertyChangeArgs args)
        {
            NotifyPropertyChangeValues notifyPropertyChangeValues = null;
            string oldValue = GetValueFromObject(args.OldValue, args.IsCustom);
            string newValue = GetValueFromObject(args.NewValue, args.IsCustom);
            if (oldValue != newValue)
            {
                notifyPropertyChangeValues = new NotifyPropertyChangeValues() { PropertyName = args.PropertyName, OldValue = args.OldValue, NewValue = args.NewValue, PropertyType =args.IsCustom ? "CustomFieldClass" : args.PropertyType };
            }
            return notifyPropertyChangeValues;
        }

        private string GetValueFromObject(object value, bool isCustom)
        {
            string result = "";
            if (value != null)
            {
                object objectValue = isCustom ? GetPropertyValue(value, "Value") : value;
                if (objectValue != null)
                {
                    result = value.ToString();
                }
            }
            return result;
        }

    }

    public class ARInvoiceChangeTracking
    {
        public ARInvoicePM ChangeTrackingPM { get; set; }
        public string EntityChangeFieldXml { get; set; }
        public List<NotifyPropertyChangeValues> NotifyPropertyChangeValuesLists { get; set; }
    }

    public class NotifyPropertyChangeArgs
    {
        public string PropertyName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public bool IsCustom { get; set; }
        public string PropertyType { get; set; }

        


    }

}

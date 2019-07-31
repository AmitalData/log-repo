using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class EntityChangeAutomationHelper
    {

        public EntityChangeAutomationsSummary GetEntityChangeAutomationsSummary(string entitychangeId, string objectTableName, int tenant)
        {
            EntityChangeQuery EntityChangeQuery = new EntityChangeQuery(tenant);
            EntityChangePM entityChangePM = EntityChangeQuery.GetSinglePM(entitychangeId, tenant);

            List<ObjectField> objectFieldLists = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant);
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            EntityChangeAutomationsSummary entityChangeAutomationsSummary = new EntityChangeAutomationsSummary();
            List<ChangeField> changeFieldsList = new List<ChangeField>();

            List<EntityChangeAutomation> entityChangeAutomation = new List<EntityChangeAutomation>();

            if (entityChangePM != null)
            {

                #region EntityChangesFieldsXml


                if (!string.IsNullOrEmpty(entityChangePM.ChangesFieldsXml))
                {
                    r root = LogitudeXmlSerializer.DeserializeObject<r>(entityChangePM.ChangesFieldsXml);
                    List<c> ChangesFields = root.cs;

                    foreach (c field in ChangesFields)
                    {
                        ObjectField objectField = objectFieldLists.Where(d => d.FieldName == field.f).FirstOrDefault();

                        if (objectField != null && objectField.FieldName != "UpdateDate")
                        {
                            #region Resolve Value
                            string newFieldValue = "";
                            string oldFieldValue = "";

                            if (objectField.IsCustom)
                            {
                                newFieldValue = field.n;
                                oldFieldValue = field.o;
                            }
                            else
                            {
                                newFieldValue = WebFreight.Web.Helpers.FieldValueResolver.GetFieldStringValue(objectField, field.n);
                                oldFieldValue = WebFreight.Web.Helpers.FieldValueResolver.GetFieldStringValue(objectField, field.o);
                            }
                            #endregion

                            ChangeField changeField = new ChangeField()
                            {
                                FieldName = TranslateTextsClass.Translate(objectField.FullNameTextCode.Code, tenant),
                                NewValue = customFieldResolver.GetFieldValue2(newFieldValue, objectField, tenant),
                                OldValue = customFieldResolver.GetFieldValue2(oldFieldValue, objectField, tenant),
                                Via = entityChangePM.CreateByUserName,
                            };

                            if (objectField.FieldName == "CreateDate")
                            {
                                DateTime? newValue = DateTime.Parse(changeField.NewValue);
                                changeField.NewValue = newValue.Value.Date.ToShortDateString();

                                newValue = DateTime.Parse(changeField.OldValue);
                                changeField.OldValue = newValue.Value.Date.ToShortDateString();

                            }

                            changeFieldsList.Add(changeField);
                        }
                    }


                }

                if (!string.IsNullOrEmpty(entityChangePM.ChangesAutomationFieldsXml))
                {
                    r root = LogitudeXmlSerializer.DeserializeObject<r>(entityChangePM.ChangesAutomationFieldsXml);
                    List<c> ChangesFields = root.cs;

                    foreach (c field in ChangesFields)
                    {
                        ObjectField objectField = objectFieldLists.Where(d => d.Id == field.f).FirstOrDefault();

                        if (objectField != null && objectField.FieldName != "UpdateDate")
                        {
                            string newFieldValue = "";
                            string oldFieldValue = "";
                            if (objectField.IsCustom)
                            {
                                newFieldValue = field.n;
                                oldFieldValue = field.o;
                            }
                            else
                            {
                                newFieldValue = WebFreight.Web.Helpers.FieldValueResolver.GetFieldStringValue(objectField, field.n);
                                oldFieldValue = WebFreight.Web.Helpers.FieldValueResolver.GetFieldStringValue(objectField, field.o);
                            }
                            string newValue = string.Empty;
                            string oldValue = string.Empty;
                            string fieldName = string.Empty;
                            if (objectField.FieldName == "SLAId" && objectField.DataTypeCode =="Text" && objectTableName == "Ticket")
                            {
                                SLAHeaderRepository sLAHeaderRepository = new SLAHeaderRepository(tenant);
                                newValue = sLAHeaderRepository.GetSLAHeaderNameById(tenant, newFieldValue);
                                oldValue = sLAHeaderRepository.GetSLAHeaderNameById(tenant, oldFieldValue);
                                fieldName = "SLA";
                            }
                            else
                            {
                                newValue = customFieldResolver.GetFieldValue2(newFieldValue, objectField, tenant);
                                oldValue = customFieldResolver.GetFieldValue2(oldFieldValue, objectField, tenant);
                                fieldName = TranslateTextsClass.Translate(objectField.FullNameTextCode.Code, tenant);
                            }

                            ChangeField changeField = new ChangeField()
                            {
                                FieldName = fieldName,
                                NewValue = newValue,
                                OldValue = oldValue,
                                Via = "Automation"
                            };
                            changeFieldsList.Add(changeField);
                        }
                    }


                }
                #endregion


                entityChangeAutomation = CreateEntityChangeAutomation(entityChangePM, "Set");

                foreach (EntityChangeAutomation item in CreateEntityChangeAutomation(entityChangePM, "Email"))
                {
                    entityChangeAutomation.Add(item);
                }

                foreach (EntityChangeAutomation item in CreateEntityChangeAutomation(entityChangePM, "FollowUp"))
                {
                    entityChangeAutomation.Add(item);
                }

                foreach (EntityChangeAutomation item in CreateEntityChangeAutomation(entityChangePM, "SetSLA"))
                {
                    entityChangeAutomation.Add(item);
                }



                entityChangeAutomation = entityChangeAutomation.OrderByDescending(d => d.CreateDate).ToList();

                entityChangeAutomationsSummary.Id = entityChangePM.Id;

            }

            entityChangeAutomationsSummary.ChangeFieldsList = changeFieldsList;
            entityChangeAutomationsSummary.EntityChangeAutomationList = entityChangeAutomation;
            return entityChangeAutomationsSummary;
        }

        public  List<EntityChangeAutomation> CreateEntityChangeAutomation(EntityChangePM entityChangePM, string type)
        {
            List<EntityChangeAutomation> entityChangeAutomation = new List<EntityChangeAutomation>();

            if (type == "Set")
            {
                if (!string.IsNullOrEmpty(entityChangePM.SetAutomationSsucceedXml))
                {
                    entityChangeAutomation = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.SetAutomationSsucceedXml);
                }

                if (!string.IsNullOrEmpty(entityChangePM.SetAutomationFailedXml))
                {
                    if (entityChangeAutomation != null && entityChangeAutomation.Count > 0)
                    {
                        List<EntityChangeAutomation> entityChangesAutomationFailedXml = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.SetAutomationFailedXml);
                        foreach (EntityChangeAutomation item in entityChangesAutomationFailedXml)
                        {
                            entityChangeAutomation.Add(item);
                        }
                    }
                    else
                    {
                        entityChangeAutomation = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.SetAutomationFailedXml);
                    }
                }
            }

           else if (type == "Email")
            {
                if (!string.IsNullOrEmpty(entityChangePM.EmailAutomationSsucceedXml))
                {
                    entityChangeAutomation = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.EmailAutomationSsucceedXml);
                }

                if (!string.IsNullOrEmpty(entityChangePM.EmailAutomationFailedXml))
                {
                    if (entityChangeAutomation != null && entityChangeAutomation.Count > 0)
                    {
                        List<EntityChangeAutomation> entityChangesAutomationFailedXml = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.EmailAutomationFailedXml);
                        foreach (EntityChangeAutomation item in entityChangesAutomationFailedXml)
                        {
                            entityChangeAutomation.Add(item);
                        }
                    }
                    else
                    {
                        entityChangeAutomation = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.EmailAutomationFailedXml);
                    }
                }
            }

            else if (type == "FollowUp")
            {
                if (!string.IsNullOrEmpty(entityChangePM.FollowUpAutomationSsucceedXml))
                {
                    entityChangeAutomation = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.FollowUpAutomationSsucceedXml);
                }

                if (!string.IsNullOrEmpty(entityChangePM.FollowUpAutomationFailedXml))
                {
                    if (entityChangeAutomation != null && entityChangeAutomation.Count > 0)
                    {
                        List<EntityChangeAutomation> entityChangesAutomationFailedXml = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.FollowUpAutomationFailedXml);
                        foreach (EntityChangeAutomation item in entityChangesAutomationFailedXml)
                        {
                            entityChangeAutomation.Add(item);
                        }
                    }
                    else
                    {
                        entityChangeAutomation = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.FollowUpAutomationFailedXml);
                    }
                }
            }

            else if (type == "SetSLA")
            {
                if (!string.IsNullOrEmpty(entityChangePM.SetSLAAutomationSsucceedXml))
                {
                    entityChangeAutomation = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.SetSLAAutomationSsucceedXml);
                }

                if (!string.IsNullOrEmpty(entityChangePM.SetSLAAutomationFailedXml))
                {
                    if (entityChangeAutomation != null && entityChangeAutomation.Count > 0)
                    {
                        List<EntityChangeAutomation> entityChangesAutomationFailedXml = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.SetSLAAutomationFailedXml);
                        foreach (EntityChangeAutomation item in entityChangesAutomationFailedXml)
                        {
                            entityChangeAutomation.Add(item);
                        }
                    }
                    else
                    {
                        entityChangeAutomation = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangePM.SetSLAAutomationFailedXml);
                    }
                }
            }

            return entityChangeAutomation;
        }




    }
}
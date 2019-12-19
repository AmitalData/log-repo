using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ObjectFieldService
    {
        bool isNewEntity;
        private int tenant;
        public ObjectField Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ObjectFieldPM entityPM;
        private IWebFreightContext objectContext;
        private ObjectFieldRepository entityRepository;
        private TextCodeRepository textCodeRepository;
        private ObjectTableRepository objectTableRepository;
        private ObjectFieldValidationRepository objectFieldValidationRepository;
        public ObjectFieldService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ObjectFieldRepository(objectContext);
        }

        public void Create(ObjectFieldPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.Poco = new ObjectField();
            this.Poco.Id = this.entityPM.Id;
            if (string.IsNullOrEmpty(this.entityPM.Code))
            {
                this.entityPM.Code = this.entityPM.FieldName;
            }
                    
            textCodeRepository = new TextCodeRepository(ObjectContext);
            objectTableRepository = new ObjectTableRepository(ObjectContext);
            objectFieldValidationRepository = new ObjectFieldValidationRepository(ObjectContext);
            string objectTableName = objectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, 0, false).Name;
            string tenantListName = "tabletenantobjectfields" + objectTableName.ToLower() + tenant;
            string zerolistAutomationObjectFields = "tabletenantzeroAutomationConditionsObjectFields" + theEntityPm.ObjectTableId.ToLower();
            string tenantListAutomationObjectFields = "tabletenantAutomationConditionsObjectFields" + theEntityPm.ObjectTableId.ToLower() + theEntityPm.Tenant;
			string objectFieldsListName = objectTableName.ToLower() + "customobjectfields" + theEntityPm.Tenant;

			if (CacheManager.CacheWrapper.Get(zerolistAutomationObjectFields) != null) CacheManager.CacheWrapper.Invalidate(zerolistAutomationObjectFields);
            if (CacheManager.CacheWrapper.Get(tenantListAutomationObjectFields) != null) CacheManager.CacheWrapper.Invalidate(tenantListAutomationObjectFields);
            if (CacheManager.CacheWrapper.Get(tenantListName) != null) CacheManager.CacheWrapper.Invalidate(tenantListName);
			if (CacheManager.CacheWrapper.Get(objectFieldsListName) != null) CacheManager.CacheWrapper.Invalidate(objectFieldsListName);


			if (theEntityPm.IsCustom)
            {
                if (!string.IsNullOrEmpty(theEntityPm.Code))
                {
                    bool exists = entityRepository.GetSingleObjectFieldByCode(theEntityPm.Code, theEntityPm.ObjectTableId, theEntityPm.Tenant) != null ? true : false;
                    if(exists)
                        throw new ApplicationException("An Object Field with the same code already exists");
                }
                else
                {
                    throw new ApplicationException("Code Field is required");
                }

                #region
                tenant = theEntityPm.Tenant;
                string ObjectTableId = theEntityPm.ObjectTableId;
                ObjectTable ObjectTable = objectTableRepository.GetObjects().Where(o => o.Id == ObjectTableId).FirstOrDefault();
               

                List<ObjectField> list = entityRepository.GetObjectFieldsByTenant(tenant).Where(o => o.ObjectTableId == ObjectTableId && o.IsCustom == true).ToList<ObjectField>();

                int count = 0;
                int allowedCount = 10;
                if (list != null)
                {
                    count = list.Count;
                }

                if(ObjectTable.Name == "Shipment" || ObjectTable.Name == "Master")
                {
                    allowedCount = 40;
                }

                if (count < allowedCount)
                {
                    TextCode newCustomFieldTextCode = new TextCode();
                    ObjectTable ob = objectTableRepository.GetObjects().Where(o => o.Id == theEntityPm.ObjectTableId).FirstOrDefault();

                    newCustomFieldTextCode.Code = ob.Name + ".Field" + (count + 1).ToString();
                    newCustomFieldTextCode.Tenant = theEntityPm.Tenant;
                    newCustomFieldTextCode.TextCodeTypeCode = "F";
                    newCustomFieldTextCode.ObjectTableId = theEntityPm.ObjectTableId;
                    newCustomFieldTextCode.DefaultText = theEntityPm.FullNameTextCodeId;
                    newCustomFieldTextCode.Id = IdCounter.GetNumber("TextCode", theEntityPm.Tenant).ToString();
                    textCodeRepository.Add(newCustomFieldTextCode);
                    theEntityPm.FullNameTextCodeId = newCustomFieldTextCode.Id;
                    theEntityPm.FullNameTextCodeCode = newCustomFieldTextCode.Code;

                    if (!string.IsNullOrEmpty(theEntityPm.HelpTextCodeId))
                    {
                        TextCode newHelpTextCode = new TextCode();
                        newHelpTextCode.TextCodeTypeCode = "H";
                        newHelpTextCode.ObjectTableId = theEntityPm.ObjectTableId;
                        newHelpTextCode.DefaultText = theEntityPm.HelpTextCodeId;
                        newHelpTextCode.Id = IdCounter.GetNumber("TextCode", theEntityPm.Tenant).ToString();
                        newHelpTextCode.Tenant = theEntityPm.Tenant;
                        newHelpTextCode.Code = ob.Name + ".Field" + (count + 1).ToString() + ".HelpText";
                        textCodeRepository.Add(newHelpTextCode);
                        theEntityPm.HelpTextCodeId = newHelpTextCode.Id;
                        theEntityPm.HelpTextCodeCode = newHelpTextCode.Code;
                        theEntityPm.HelpTextCodeDefaultText = newHelpTextCode.DefaultText;
                    }

                    if (!string.IsNullOrEmpty(theEntityPm.ListTextCodeId) && theEntityPm.DisplayInList)
                    {
                        TextCode listFieldLableTextCode = new TextCode();
                        listFieldLableTextCode.Code = ob.Name + ".Field" + (count + 1).ToString() + "ListLable";
                        listFieldLableTextCode.DefaultText = theEntityPm.ListTextCodeId;
                        listFieldLableTextCode.Id = IdCounter.GetNumber("TextCode", theEntityPm.Tenant).ToString();
                        listFieldLableTextCode.ObjectTableId = theEntityPm.ObjectTableId;
                        listFieldLableTextCode.Tenant = theEntityPm.Tenant;
                        listFieldLableTextCode.TextCodeTypeCode = "CH";
                        textCodeRepository.Add(listFieldLableTextCode);
                        theEntityPm.ListTextCodeId = listFieldLableTextCode.Id;
                        theEntityPm.ListTextCodeCode = listFieldLableTextCode.Code;
                        theEntityPm.ListTextCodeDefaultText = listFieldLableTextCode.DefaultText;
                    }

                    theEntityPm.FullNameTextCodeCode = newCustomFieldTextCode.Code;
                    theEntityPm.FullNameTextCodeDefaultText = newCustomFieldTextCode.DefaultText;

                    string fieldname = "Field" + (count + 1).ToString();
                    theEntityPm.FieldName = fieldname;
                    theEntityPm.FieldCode = ObjectTable.Name +"." + tenant.ToString() +  "." + theEntityPm.FieldName;
                    theEntityPm.PMPropertyPath = fieldname;
                    theEntityPm.ListPropertyPath = fieldname;
                    theEntityPm.DisplayInList = true;
                }

                else if (count >= allowedCount)
                {
                    return;
                }

                theEntityPm.Id = IdCounter.GetNumber("ObjectField", theEntityPm.Tenant).ToString();

                this.Poco.Id = theEntityPm.Id;
                ObjectFieldMapping.MapEntity(theEntityPm, Poco, true, null);

                if (theEntityPm.ObjectFieldValidations != null)
                {
                    foreach (ObjectFieldValidationPM fieldValidation in theEntityPm.ObjectFieldValidations)
                    {
                        ObjectFieldValidation newFieldValidation = new ObjectFieldValidation()
                        {
                            Id = IdCounter.GetNumber("ObjectFieldValidation", theEntityPm.Tenant).ToString(),
                            ObjectFieldId = theEntityPm.Id,
                            Tenant = fieldValidation.Tenant,
                            ValidationExpression = fieldValidation.ValidationExpression,
                            ErrorMessage = fieldValidation.ErrorMessage,
                            ValidationOrder = fieldValidation.ValidationOrder,
                            Condition = fieldValidation.Condition,
                            Code = fieldValidation.Code,
                        };

                        fieldValidation.Id = newFieldValidation.Id;
                        fieldValidation.ObjectFieldId = newFieldValidation.ObjectFieldId;
                        objectFieldValidationRepository.Add(newFieldValidation);
                    }
                }

                this.Poco.IsCustom = true;
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
                #endregion
            }

            else
            {
                #region
                theEntityPm.Id = IdCounter.GetNumber("ObjectField", theEntityPm.Tenant).ToString();
              
                this.Poco.Id = theEntityPm.Id;
                ObjectFieldMapping.MapEntity(theEntityPm, Poco, true, null);

                if (theEntityPm.ObjectFieldValidations != null)
                {
                    foreach (ObjectFieldValidationPM fieldValidation in theEntityPm.ObjectFieldValidations)
                    {
                        ObjectFieldValidation newFieldValidation = new ObjectFieldValidation()
                        {
                            Id = IdCounter.GetNumber("ObjectFieldValidation", theEntityPm.Tenant).ToString(),
                            ObjectFieldId = theEntityPm.Id,
                            Tenant = fieldValidation.Tenant,
                            ValidationExpression = fieldValidation.ValidationExpression,
                            ErrorMessage = fieldValidation.ErrorMessage,
                            ValidationOrder = fieldValidation.ValidationOrder,
                            Condition = fieldValidation.Condition,
                            Code = fieldValidation.Code,
                        };

                        fieldValidation.Id = newFieldValidation.Id;
                        fieldValidation.ObjectFieldId = newFieldValidation.ObjectFieldId;
                        objectFieldValidationRepository.Add(newFieldValidation);
                    }
                }
                ObjectFieldTracing.Trace(theEntityPm, Poco, true);
                ObjectFieldValidating.Validate(theEntityPm);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();

                #endregion
            }

        }

        public void Update(ObjectFieldPM theEntityPm , List<ObjectFieldValidationPM> objectfieldValidationList = null)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectField(theEntityPm.Id);




            TextCodeRepository textCodeRepository = new TextCodeRepository(ObjectContext);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(ObjectContext);
            ObjectFieldValidationRepository objectFieldValidationRepository = new ObjectFieldValidationRepository(ObjectContext);

            string objectTableName = objectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, 0, false).Name;
            string tenantListName = "tabletenantobjectfields" + objectTableName.ToLower() + tenant;

            string zerolistAutomationObjectFields = "tabletenantzeroAutomationConditionsObjectFields" + theEntityPm.ObjectTableId.ToLower();
            string tenantListAutomationObjectFields = "tabletenantAutomationConditionsObjectFields" + theEntityPm.ObjectTableId.ToLower() + theEntityPm.Tenant;
			string objectFieldsListName = objectTableName.ToLower() + "customobjectfields" + theEntityPm.Tenant;

			if (CacheManager.CacheWrapper.Get(zerolistAutomationObjectFields) != null) CacheManager.CacheWrapper.Invalidate(zerolistAutomationObjectFields);
            if (CacheManager.CacheWrapper.Get(tenantListAutomationObjectFields) != null) CacheManager.CacheWrapper.Invalidate(tenantListAutomationObjectFields);
            if (CacheManager.CacheWrapper.Get(tenantListName) != null) CacheManager.CacheWrapper.Invalidate(tenantListName);
			if (CacheManager.CacheWrapper.Get(objectFieldsListName) != null) CacheManager.CacheWrapper.Invalidate(objectFieldsListName);
 

			if (!string.IsNullOrEmpty(theEntityPm.FullNameTextCodeId))
            {
                TextCode textCode = textCodeRepository.GetTextCodes().Where(o => o.Id == theEntityPm.FullNameTextCodeId).FirstOrDefault();
                if (textCode != null)
                {
                    if (textCode.DefaultText != theEntityPm.FullNameTextCodeDefaultText)
                    {
                        textCode.DefaultText = theEntityPm.FullNameTextCodeDefaultText;
                    }
                }
            }


            if (!string.IsNullOrEmpty(theEntityPm.ListTextCodeId) && !string.IsNullOrEmpty(theEntityPm.ListTextCodeDefaultText))
            {
                TextCode textCode = textCodeRepository.GetTextCodes().Where(o => o.Id == theEntityPm.ListTextCodeId).FirstOrDefault();
                if (textCode != null)
                {
                    if (textCode.DefaultText != theEntityPm.ListTextCodeDefaultText)
                    {
                        textCode.DefaultText = theEntityPm.FullNameTextCodeDefaultText;
                    }
                }
            }

            

            if (!string.IsNullOrEmpty(theEntityPm.HelpTextCodeCode))
            {
                TextCode helpTextCode = textCodeRepository.GetTextCodes().Where(o => o.Id == theEntityPm.HelpTextCodeCode).FirstOrDefault();
                if (helpTextCode == null)
                {
                    ObjectTable ob = objectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, 0, true);

                    TextCode newHelpTextCode = new TextCode();
                    newHelpTextCode.TextCodeTypeCode = "H";
                    newHelpTextCode.ObjectTableId = theEntityPm.ObjectTableId;
                    newHelpTextCode.DefaultText = theEntityPm.HelpTextCodeId;
                    newHelpTextCode.Id = IdCounter.GetNumber("TextCode", theEntityPm.Tenant).ToString();
                    newHelpTextCode.Tenant = theEntityPm.Tenant;
                    newHelpTextCode.Code = ob.Name + "." + theEntityPm.FieldName + ".HelpText";
                    textCodeRepository.Add(newHelpTextCode);
                    theEntityPm.HelpTextCodeId = newHelpTextCode.Id;
                    theEntityPm.HelpTextCodeCode = newHelpTextCode.Code;
                }

                else
                {
                    if (helpTextCode.DefaultText != theEntityPm.HelpTextCodeDefaultText)
                    {
                        helpTextCode.DefaultText = theEntityPm.HelpTextCodeDefaultText;
                    }
                }
            }

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int currentTenant = authToken.Tenant;


            ObjectFieldModification mod = entityRepository.GetObjectFieldModificationByObjectField(theEntityPm.Id, currentTenant);
            if ((theEntityPm.IsRequiered != this.Poco.IsRequiered) || (theEntityPm.MinLength != this.Poco.MinLength) || (theEntityPm.MaxLength != this.Poco.MaxLength))
            {
                if (mod == null && this.Poco.Tenant == 0)
                {
                    mod = new ObjectFieldModification() { Id = IdCounter.GetNumber("ObjectFieldModification", theEntityPm.Tenant), ObjectFieldId = this.Poco.Id, IsRequired = theEntityPm.IsRequiered, MaxLength = theEntityPm.MaxLength, MinLength = theEntityPm.MinLength, Tenant = currentTenant, UpdateDateGMT = DateTime.UtcNow };
                    this.ObjectContext.ObjectFieldModifications.Add(mod);
                }
            }

            ObjectFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity, mod);

            ObjectFieldValidationService service = new ObjectFieldValidationService(objectContext, theEntityPm.Tenant);

            if (objectfieldValidationList != null)
            {
                #region ObjectFieldValidations
                foreach (ObjectFieldValidationPM r in objectfieldValidationList)
                {

                    switch (r.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {

                                service.Create(r);
                                //r.Id = IdCounter.GetNumber("ObjectFieldValidation", theEntityPm.Tenant).ToString();
                                //ObjectFieldValidation newObjectFieldValidation = new ObjectFieldValidation();
                                //newObjectFieldValidation.Id = r.Id;
                                //ObjectFieldValidationMapping.MapEntity(r, newObjectFieldValidation, isNewEntity);
                                //objectFieldValidationRepository.Add(newObjectFieldValidation);
                                break;
                            }
                        case ChangeSetOperation.Update:
                            {
                                service.Update(r);
                                //ObjectFieldValidation objectFieldValidation = objectFieldValidationRepository.GetSingleObjectFieldValidation(r.Id, theEntityPm.Tenant);
                                //ObjectFieldValidationMapping.MapEntity(r, objectFieldValidation, isNewEntity);
                                //objectFieldValidationRepository.Update(objectFieldValidation);
                                break;
                            }
                        case ChangeSetOperation.Delete:
                            {
                                ObjectFieldValidation objectFieldValidation = objectFieldValidationRepository.GetSingleObjectFieldValidation(r.Id, theEntityPm.Tenant);
                                objectFieldValidationRepository.Remove(objectFieldValidation);
                                break;
                            }
                        case ChangeSetOperation.None:
                            {
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                # endregion
            }
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            ObjectFieldValidating.Validate(theEntityPm);
            ObjectFieldTracing.Trace(theEntityPm, Poco, isNewEntity);

            //TableLastUpdateClass.UpdateSystemMetaDataHistory(true);
          
        }

        public void Update(ObjectFieldPM theEntityPm, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectField(theEntityPm.Id);




            TextCodeRepository textCodeRepository = new TextCodeRepository(ObjectContext);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(ObjectContext);
            ObjectFieldValidationRepository objectFieldValidationRepository = new ObjectFieldValidationRepository(ObjectContext);



			string objectTableName = objectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, 0, false).Name;
			string tenantListName = "tabletenantobjectfields" + objectTableName.ToLower() + tenant;
			string zerolistAutomationObjectFields = "tabletenantzeroAutomationConditionsObjectFields" + theEntityPm.ObjectTableId.ToLower();
			string tenantListAutomationObjectFields = "tabletenantAutomationConditionsObjectFields" + theEntityPm.ObjectTableId.ToLower() + theEntityPm.Tenant;
			string objectFieldsListName = objectTableName.ToLower() + "customobjectfields" + theEntityPm.Tenant;

			if (CacheManager.CacheWrapper.Get(zerolistAutomationObjectFields) != null) CacheManager.CacheWrapper.Invalidate(zerolistAutomationObjectFields);
			if (CacheManager.CacheWrapper.Get(tenantListAutomationObjectFields) != null) CacheManager.CacheWrapper.Invalidate(tenantListAutomationObjectFields);
			if (CacheManager.CacheWrapper.Get(tenantListName) != null) CacheManager.CacheWrapper.Invalidate(tenantListName);
			if (CacheManager.CacheWrapper.Get(objectFieldsListName) != null) CacheManager.CacheWrapper.Invalidate(objectFieldsListName);

			if (!string.IsNullOrEmpty(theEntityPm.FullNameTextCodeCode))
            {
                TextCode textCode = textCodeRepository.GetTextCodes().Where(o => o.Code == theEntityPm.FullNameTextCodeCode).FirstOrDefault();
                if (textCode != null)
                {
                    if (textCode.DefaultText != theEntityPm.FullNameTextCodeDefaultText)
                    {
                        textCode.DefaultText = theEntityPm.FullNameTextCodeDefaultText;
                    }
                }
            }


            if (!string.IsNullOrEmpty(theEntityPm.ListTextCodeCode) && !string.IsNullOrEmpty(theEntityPm.ListTextCodeDefaultText))
            {
                TextCode textCode = textCodeRepository.GetTextCodes().Where(o => o.Code == theEntityPm.ListTextCodeCode).FirstOrDefault();
                if (textCode != null)
                {
                    if (textCode.DefaultText != theEntityPm.ListTextCodeDefaultText)
                    {
                        textCode.DefaultText = theEntityPm.FullNameTextCodeDefaultText;
                    }
                }
            }



            if (!string.IsNullOrEmpty(theEntityPm.HelpTextCodeCode))
            {
                TextCode helpTextCode = textCodeRepository.GetTextCodes().Where(o => o.Id == theEntityPm.HelpTextCodeCode).FirstOrDefault();
                if (helpTextCode == null)
                {
                    ObjectTable ob = objectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, 0, true);

                    TextCode newHelpTextCode = new TextCode();
                    newHelpTextCode.TextCodeTypeCode = "H";
                    newHelpTextCode.ObjectTableId = theEntityPm.ObjectTableId;
                    newHelpTextCode.DefaultText = theEntityPm.HelpTextCodeId;
                    newHelpTextCode.Id = IdCounter.GetNumber("TextCode", theEntityPm.Tenant).ToString();
                    newHelpTextCode.Tenant = theEntityPm.Tenant;
                    newHelpTextCode.Code = ob.Name + "." + theEntityPm.FieldName + ".HelpText";
                    textCodeRepository.Add(newHelpTextCode);
                    theEntityPm.HelpTextCodeId = newHelpTextCode.Id;
                    theEntityPm.HelpTextCodeCode = newHelpTextCode.Code;
                }

                else
                {
                    if (helpTextCode.DefaultText != theEntityPm.HelpTextCodeDefaultText)
                    {
                        helpTextCode.DefaultText = theEntityPm.HelpTextCodeDefaultText;
                    }
                }
            }

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int currentTenant = authToken.Tenant;

            ObjectFieldModification mod = entityRepository.GetObjectFieldModificationByObjectField(theEntityPm.Id, currentTenant);
            if ((theEntityPm.IsRequiered != this.Poco.IsRequiered) || (theEntityPm.MinLength != this.Poco.MinLength) || (theEntityPm.MaxLength != this.Poco.MaxLength))
            {
                if (mod == null && this.Poco.Tenant == 0)
                {
                   
                     mod = new ObjectFieldModification()
                    {
                        Id = IdCounter.GetNumber("ObjectFieldModification", theEntityPm.Tenant),
                        ObjectFieldId = this.Poco.Id,
                        IsRequired = theEntityPm.IsRequiered,
                        MaxLength = theEntityPm.MaxLength,
                        MinLength = theEntityPm.MinLength,
                        Tenant = currentTenant,
                        UpdateDateGMT = DateTime.UtcNow
                    };
                    this.ObjectContext.ObjectFieldModifications.Add(mod);
                }
            }
            

            ObjectFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity, mod);

            ObjectFieldValidationService service = new ObjectFieldValidationService(objectContext, theEntityPm.Tenant);

            if (theEntityPm.ObjectFieldValidations != null)
            {
                #region ObjectFieldValidations
                foreach (ObjectFieldValidationPM r in theEntityPm.ObjectFieldValidations)
                {

                    switch (r.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {

                                service.Create(r);
                                //r.Id = IdCounter.GetNumber("ObjectFieldValidation", theEntityPm.Tenant).ToString();
                                //ObjectFieldValidation newObjectFieldValidation = new ObjectFieldValidation();
                                //newObjectFieldValidation.Id = r.Id;
                                //ObjectFieldValidationMapping.MapEntity(r, newObjectFieldValidation, isNewEntity);
                                //objectFieldValidationRepository.Add(newObjectFieldValidation);
                                break;
                            }
                        case ChangeSetOperation.Update:
                            {
                                service.Update(r);
                                //ObjectFieldValidation objectFieldValidation = objectFieldValidationRepository.GetSingleObjectFieldValidation(r.Id, theEntityPm.Tenant);
                                //ObjectFieldValidationMapping.MapEntity(r, objectFieldValidation, isNewEntity);
                                //objectFieldValidationRepository.Update(objectFieldValidation);
                                break;
                            }
                        case ChangeSetOperation.Delete:
                            {
                                ObjectFieldValidation objectFieldValidation = objectFieldValidationRepository.GetSingleObjectFieldValidation(r.Id, theEntityPm.Tenant);
                                objectFieldValidationRepository.Remove(objectFieldValidation);
                                break;
                            }
                        case ChangeSetOperation.None:
                            {
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                # endregion
            }
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            ObjectFieldValidating.Validate(theEntityPm);
            ObjectFieldTracing.Trace(theEntityPm, Poco, isNewEntity);

            //TableLastUpdateClass.UpdateSystemMetaDataHistory(true);

        }
    }
}
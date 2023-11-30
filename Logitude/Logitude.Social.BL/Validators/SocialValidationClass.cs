using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Logitude.Social.BL.Validators
{
    public partial class SocialValidationClass : ISocialValidateContext
    {
        bool ready;
        //WebFreightContext Context;
        List<ObjectField> objectFieldList;
        private  string errorMessage = "";
        int tenant = 0;
        //ObjectFieldsRepository ObjectFieldsRep ;
        public SocialValidationClass(string objectTableName, int tenant)
        {
           // return;
            //string reporistry = "objectfieldsrepository" + tenant;
            //if (HttpContext.Current.Cache.Get(reporistry) == null)
            //{
            //    ObjectFieldsRep = new ObjectFieldsRepository(tenant);
            //    HttpContext.Current.Cache.Insert(reporistry, ObjectFieldsRep, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //}
            //else
            //{
            //    ObjectFieldsRep = (ObjectFieldsRepository)HttpContext.Current.Cache.Get(reporistry);

            //}
            this.tenant = tenant;

            //string ObjectFieldsListName = ObjectTableName.ToLower() + "objectfields" + tenant;
            //if (HttpContext.Current.Cache.Get(ObjectFieldsListName) == null)
            //{

                objectFieldList = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant).ToList(); //Context.Where(d => d.ObjectTable.Name == objectTableName && (d.Tenant == tenant || d.Tenant == 0)).ToList();
            //    HttpContext.Current.Cache.Insert(ObjectFieldsListName, ObjectFieldList, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //}
            //else
            //{
            //    ObjectFieldList = (List<ObjectField>)HttpContext.Current.Cache.Get(ObjectFieldsListName);
            //}

        }

        public bool IsValid(object value,object instance,string propertyName)
        {
           // return true;
            bool valid = true;
            errorMessage = "";
            Type type = instance.GetType();
            object typeProp = null;
         //   FieldValidator fieldValidator = new FieldValidator(tenant);
         

            string objectType = "";
            
            if (typeProp != null)
            {
                objectType = typeProp.ToString();
            }
            else
            {
                objectType = type.Name.ToString();
                if (objectType.Contains("FollowUp"))
                {
                    objectType = "FollowUp";
                }
                else if (objectType.Contains("PM"))
                {
                    objectType = objectType.Substring(0, objectType.Length - 2);
                }

            
            }

           
            ObjectField field = (from a in objectFieldList
                                 where a.ObjectTable.Name == objectType && a.FieldName == propertyName
                                 select a).FirstOrDefault();

            if (field != null)
            {
              
                if (field.IsRequiered)
                {
                    //if (field.IsCustom && value != null)
                    //{
                    //    CustomFieldClass fieldClass = value as CustomFieldClass;
                    //    value = fieldClass.Value;
                    //}


                    if (value == null)
                    {
                        valid = false;
                    }
                    else if (value is string)
                    {
                        if (String.IsNullOrEmpty(value.ToString()))
                        {
                            valid = false;

                        }
                    }
                    else
                    {
                        valid = true;
                    }
                }

                if (field.DataTypeCode == "Text") // server side
                {
                    //if (field.IsCustom && value != null)
                    //{
                    //    CustomFieldClass fieldClass = value as CustomFieldClass;
                    //    value = fieldClass.Value;
                    //}

                    string valueString = value != null ? value.ToString() : "";
                    if (FieldValueValidator.IsNotValidMinMaxValue(field, valueString))
                    {
                        valid = false;
                    }
                }

                if (field.DataTypeCode == "Decimal" && field.NumberOfDigits != 0)
                {
                    string valueString = value != null ? value.ToString() : "";
                    if (!string.IsNullOrEmpty(valueString))
                    {
                        string[] digits = valueString.Split('.');
                        int beforepointlength = digits[0].Length;
                        int afterpointlength = 0;
                        if (digits.Length > 1)
                        {
                            afterpointlength = digits[1].Length;
                        }

                        if (beforepointlength > field.NumberOfDigits)
                        {
                            valid = false;

                        }

                        if (afterpointlength > field.DigitsAfterPoint)
                        {
                            valid = false;

                        }



                    }
                }


                if (value != null)
                {
                    //ValidationFieldResult result = fieldValidator.ValidateField(field, value, instance, tenant);

                    //if (!result.Valid)
                    //{
                    //    valid = result.Valid;
                    //    errorMessage = result.ErrorMessage;
                    //}
                }
            }         

            return valid;            
        }
        
        public string GetErrorMessage(object value,object instance, string propertyName)
        {
            string error = "";
            Type type = instance.GetType();
            object typeProp = null;
           

            string objectType = "";
            if (typeProp != null)
            {
                objectType = typeProp.ToString();
            }
            else
            {
                objectType = type.Name.ToString();
                if (objectType.Contains("FollowUp"))
                {
                    objectType = "FollowUp";
                }
                else if (objectType.Contains("PM"))
                {
                    objectType = objectType.Substring(0, objectType.Length - 2);
                }

            }

           
            ObjectField field = (from a in objectFieldList
                                 where a.ObjectTable.Name == objectType && a.FieldName == propertyName
                                 select a).FirstOrDefault();

            if (field.IsRequiered)
            {

                if (value == null)
                {
                    error = "";//TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", field.FullNameTextCode.Code, null, null, field.Tenant);
                }
                if (string.IsNullOrEmpty(error)&&value!=null)
                {
                    if (value is string)
                    {
                        string valueString = value as string;
                        if (String.IsNullOrEmpty(value.ToString()))
                        {
                            error = "";//TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", field.FullNameTextCode.Code, null, null, field.Tenant);

                        }

                       
                    }


                }

            }

            
                if (field.DataTypeCode == "Text")
                {

                    //if (field.IsCustom && value != null)
                    //{
                    //    CustomFieldClass fieldClass = value as CustomFieldClass;
                    //    value = fieldClass.Value;
                    //}

                    string valueString = value != null? value.ToString():"";
                    if (FieldValueValidator.IsNotValidMinMaxValue(field, valueString))
                    {
                        error = "";//TranslateTextsClass.GetTranslation("General.M.MinMax", field.FullNameTextCode.Code, field.MinLength.ToString(), field.MaxLength.ToString(), field.Tenant);
                    }
                }


                if (field.DataTypeCode == "Decimal" && field.NumberOfDigits != 0)
                {
                    string valueString = value != null ? value.ToString() : "";
                    if (!string.IsNullOrEmpty(valueString))
                    {
                        string[] digits = valueString.Split('.');
                        int beforepointlength = digits[0].Length;
                        int afterpointlength = 0;
                        if (digits.Length > 1)
                        {
                            afterpointlength = digits[1].Length;
                        }

                        if (beforepointlength > field.NumberOfDigits)
                        {

                            error = "Maximum number of digits is " + field.NumberOfDigits;

                        }

                        if (afterpointlength > field.DigitsAfterPoint)
                        {

                            error = "Maximum number of digits after point " + field.DigitsAfterPoint;

                        }

                    }
                }



                if (!String.IsNullOrEmpty(errorMessage))
                {
                    error = errorMessage;
                }

             
    

            return error;
        }
    }
}
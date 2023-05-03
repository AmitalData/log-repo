using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System.Collections.Generic;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.Validators
{
    public class DocumentTypeClassLevelValidator
    {
        public static ValidationResult ValidateClass(DocumentTypePM documentType, ValidationContext context)
        {

            if (!string.IsNullOrEmpty(documentType.FileName))
            {
                string s = ('"').ToString();
                /// : * ? " < > 
                //char[] specialCharacterLists = { "", '1', '2' };
                List<string> speCharLists = new List<string>();
                speCharLists.Add("|");
                speCharLists.Add("/");
                speCharLists.Add("\\");
                speCharLists.Add(":");
                speCharLists.Add("*");
                speCharLists.Add("?");
                speCharLists.Add("<");
                speCharLists.Add(">");
                speCharLists.Add(('"').ToString());

                foreach (string specialCharacter in speCharLists)
                {
                    if(documentType.FileName.Contains(specialCharacter))
                    {
                        return new ValidationResult("FileName field can't contain any of the following characters:" + speCharLists[0] + speCharLists[1] + speCharLists[2] + speCharLists[3] + speCharLists[4] + speCharLists[5] + speCharLists[6] + speCharLists[7] + speCharLists[8] );
                    }
                }

                if (documentType.FileName.Contains(" "))
                {
                    return new ValidationResult("FileName field can't contain spaces");
                }



            }


            ObjectTable objectTable = GetObjectTable(documentType);

            if(objectTable == null)
            {
                return new ValidationResult("Object Table is Required");
            }

            if (!string.IsNullOrEmpty(documentType.ObjectTableName) && !objectTable.AvailableInDocumentTypes)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("DocumentType.M.TableNameDoesNotExist", documentType.Tenant));
            }

            if (!string.IsNullOrEmpty(documentType.ObjectTableName) && objectTable.IsCustom && !documentType.IsDocIn)
            {
                return new ValidationResult("Please choose Doc in");
            }
            if (documentType.IsDocOut)
            {
                if (documentType.TemplateFormatCode == null)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("DocumentType.M.ChooseDocumentTypeFormat", documentType.Tenant));

                }
            }
            else
            {
                if (!documentType.IsDocOut && !documentType.IsDocIn)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("DocumentType.M.EntityLevelvalidation", documentType.Tenant));
                }
            }



            return null;
        }

        private static ObjectTable GetObjectTable(DocumentTypePM documentType)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(documentType.Tenant);
            if (!string.IsNullOrEmpty(documentType.ObjectTableId)) return objectTableRepository.GetObjectTableById(documentType.ObjectTableId, documentType.Tenant);
            if (!string.IsNullOrEmpty(documentType.ObjectTableName)) return objectTableRepository.GetObjectTableByName(documentType.ObjectTableName, documentType.Tenant, true);
            return null;
        }

    }
}

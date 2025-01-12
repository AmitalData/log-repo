using AmitalCloud.Infrastructure.APITools.ApiV1;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using System;

namespace AmitalCloud.Infrastructure.APITools.Validators
{
    public class DocumentsFilingValidating
    {
        public static void Validate(DocumentsFilingPM entityPM)
        {

        }

        public static void Validate(DocumentsFiling documentsFiling)
        {
            string validations = string.Empty;

            if (string.IsNullOrEmpty(documentsFiling.EntityNumber))
            {
                validations = AddValidatingMessage(validations, "EntityNumber is required!");
            }

            if (documentsFiling.EntityType == null)
            {
                validations = AddValidatingMessage(validations, "EntityType is required!");
            }

            if (documentsFiling.EntityType != null && string.IsNullOrEmpty(documentsFiling.EntityType.Name))
            {
                validations = AddValidatingMessage(validations, "EntityType name is required!");
            }

            if (documentsFiling.DocumentType == null)
            {
                validations = AddValidatingMessage(validations, "DocumentType is required!");
            }

            if (documentsFiling.DocumentType != null && string.IsNullOrEmpty(documentsFiling.DocumentType.Code))
            {
                validations = AddValidatingMessage(validations, "DocumentType code is required!");
            }

            if (string.IsNullOrEmpty(documentsFiling.BlobName))
            {
                validations = AddValidatingMessage(validations, "BlobName is required!");
            }

            if (string.IsNullOrEmpty(documentsFiling.BlobId))
            {
                validations = AddValidatingMessage(validations, "BlobId is required!");
            }

            if (!string.IsNullOrEmpty(validations))
                throw new ApplicationException(validations);
        }

        private static string AddValidatingMessage(string validations, string message)
        {
            if (string.IsNullOrEmpty(validations))
                return message;

            return validations + " and " + message;
        }
    }

}

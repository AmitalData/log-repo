using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.Validators
{
    public class DocumentTypeTransModeValidator
    {
        public static ValidationResult IsDocumentTypeValid(DocumentTypePM documentType, ValidationContext context)
        {
            if (documentType.ObjectTableName == "Shipment" || documentType.ObjectTableName == "Quote")
            {

                bool valid = ((documentType.IsAir) || (documentType.IsOcean) || (documentType.IsInland));

                if (!valid)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("DocumentType.M.ChooseDocumentTypeTransportation", documentType.Tenant));
                }
            }
            return null;
        }
    }
}

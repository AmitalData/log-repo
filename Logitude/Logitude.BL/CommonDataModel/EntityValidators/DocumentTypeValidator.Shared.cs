using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityValidators
{
    public class DocumentTypeValidator
    {
        public static ValidationResult IsDocumentTypeValid(
            DocumentType document,
            ValidationContext context)
        {
            bool valid = ((document.IsAir) || (document.IsOcean) || (document.IsInland));

            if (!valid)
            {
                return new ValidationResult(TextCodesTranslator.TranslateText("DocumentType.M.ChooseDocumentTypeTransportation", document.Tenant));
            }

            return null;
        }
    }
}
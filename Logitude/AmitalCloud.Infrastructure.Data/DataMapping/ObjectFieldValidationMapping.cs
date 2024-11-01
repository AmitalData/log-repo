using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class ObjectFieldValidationMapping
    {
        public static void MapEntity(ObjectFieldValidationPM objectFieldValidationPM, ObjectFieldValidation objectFieldValidation, bool isNewState)
        {
            objectFieldValidation.ObjectFieldId = objectFieldValidationPM.ObjectFieldId;
            objectFieldValidation.Tenant = objectFieldValidationPM.Tenant;
            objectFieldValidation.ValidationExpression = objectFieldValidationPM.ValidationExpression;
            objectFieldValidation.ErrorMessage = objectFieldValidationPM.ErrorMessage;
            objectFieldValidation.ValidationOrder = objectFieldValidationPM.ValidationOrder;
            objectFieldValidation.Condition = objectFieldValidationPM.Condition;
            objectFieldValidation.Code = objectFieldValidationPM.Code;
            objectFieldValidation.ObjectFieldCode = objectFieldValidationPM.ObjectFieldCode;
        }
    }
}
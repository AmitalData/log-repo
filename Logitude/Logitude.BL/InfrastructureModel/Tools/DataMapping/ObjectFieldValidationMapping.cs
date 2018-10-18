using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
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
        }
    }
}
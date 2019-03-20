using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityQueryServicesExt
{
    public interface ICustomsRequiredFieldsValidatorExt
    {
        bool CheckRequiredFieldErrorsForCourierDeclaration(string declarationId, int tenant);
    }
}

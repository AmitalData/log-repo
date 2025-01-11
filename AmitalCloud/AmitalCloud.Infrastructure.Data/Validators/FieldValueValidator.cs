using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Validators
{ 
    public class FieldValueValidator
    {
        public static bool IsNotValidMinMaxValue(ObjectField objetField, string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (objetField == null) return false;
            if (objetField.IsMaxLength) return false;

            return (value.Length > objetField.MaxLength && objetField.MaxLength != 0) || value.Length < objetField.MinLength;
        }
    }
}

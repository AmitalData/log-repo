using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Interfaces
{
    public interface IRulesValidator
    {
        void Initialize(int tenant);
        List<ObjectTableRuleField> ValidateAllRequiredFieldRules(object entity, string objectTableName, int tenant);
        bool ValidateEntityRules(object entity, string objectTableName, int tenant, ref string outputMessage);
        bool ApplyDuplicationRules(object entity, string objectTableName, int tenant, ref string outputMessage);
    }
}

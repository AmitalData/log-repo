using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.EntityPMs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class CounterDefinitionMapping
    {
        public static void MapEntity(CounterDefinitionPM counterDefinitionPM, CounterDefinition counterDefinition, bool isNewState)
        {
            counterDefinition.Prefix = counterDefinitionPM.Prefix;
            counterDefinition.CounterId = counterDefinitionPM.CounterId;
            counterDefinition.UniquePerPrefix = counterDefinitionPM.UniquePerPrefix;
            counterDefinition.Tenant = counterDefinitionPM.Tenant;
            counterDefinition.Parameter1 = counterDefinitionPM.Parameter1;
            counterDefinition.Parameter2 = counterDefinitionPM.Parameter2;
            counterDefinition.StartNumber = counterDefinitionPM.StartNumber;
            counterDefinition.CounterSize = counterDefinitionPM.CounterSize;
            counterDefinition.Suffix = counterDefinitionPM.Suffix;
            counterDefinition.InActive = counterDefinitionPM.InActive;
            counterDefinition.UsePerBranch = counterDefinitionPM.UsePerBranch;
            counterDefinition.IsCustomized = counterDefinitionPM.IsCustomized;

        }
    }
}
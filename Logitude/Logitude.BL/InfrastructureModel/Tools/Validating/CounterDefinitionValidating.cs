using System;
using System.Linq;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.Tools.Validating
{
    public class CounterDefinitionValidating
    {
        public static void Validate(CounterDefinitionPM entityPM)
        {
            //if (entityPM.UniquePerPrefix)
            //{
            //    IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityPM.Tenant);
            //    CounterRepository counterRepository = new CounterRepository(webFreightContext);
            //    ObjectTabelRepository tableRepository = new ObjectTabelRepository(webFreightContext);
            //    Counter counter = counterRepository.GetSingleCounter(entityPM.CounterId, entityPM.Tenant);
            //    ObjectTable table = tableRepository.GetSingleObjectTable(counter.ObjectTableId, 0, true);
            //    if (table.Name == "ARInvoice")
            //    {
            //        if ((entityPM.StartNumber + entityPM.Prefix).Length > 20)
            //        {
            //            throw new ApplicationException("Maximum length allowed for [Startnumber + Prefix] is 20");
            //        }
            //    }
            //    else
            //    {
            //        if ((entityPM.StartNumber + entityPM.Prefix).Length > 15)
            //        {
            //            throw new ApplicationException("Maximum length allowed for [Startnumber + Prefix] is 15");
            //        }
            //    }
            //}
        }
    }
}
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Services.CustomizedCounter
{
    public class CustomizedARInvoiceCounterService
    {
        private CounterDefinitionRepository counterDefinitionRepository;
        private IWebFreightContext context ;

        private int tenant;
        private CounterDefinitionService counterDefinitionService;
        private CounterDefinitionQuery counterDefinitionQuery;

        public CustomizedARInvoiceCounterService(int tenant, CounterDefinitionService counterDefinitionService)
        {
            this.tenant = tenant;
            this.counterDefinitionService = counterDefinitionService;
            context = WebFreightContext.GetContext(tenant);
            counterDefinitionRepository = new CounterDefinitionRepository(context);
            counterDefinitionQuery = new CounterDefinitionQuery(tenant);

        }

        public void HandleCustomizedCounterDefinitions(List<CounterDefinitionPM> counterDefinitions, string counterId)
        {
            if (counterDefinitions == null || counterDefinitions.Count == 0) return;
            HandleUpdatingCounterDefinitions(counterDefinitions, counterId);
            HandleAddingCounterDefinitions(counterDefinitions);
        }
        private void HandleUpdatingCounterDefinitions(List<CounterDefinitionPM> counterDefinitions, string counterId)
        {
            List<CounterDefinitionPM> existingCounterDefinitions = counterDefinitionQuery.GetCustomizedCounterDefinitionsByCounterId(counterId, tenant).ToList();
            if (existingCounterDefinitions == null || existingCounterDefinitions.Count == 0) return;
            foreach (CounterDefinitionPM counterDefinitionPM in counterDefinitions)
            {
                if (existingCounterDefinitions.Where(def => def.Parameter2 == counterDefinitionPM.Parameter2).Any())
                {
                    counterDefinitionService.Update(counterDefinitionPM);
                    counterDefinitionPM.IsAdded = true;
                }
            }
        }
        private void HandleAddingCounterDefinitions(List<CounterDefinitionPM> counterDefinitions)
        {
            List<CounterDefinitionPM> newCounterDefinitions = counterDefinitions.Where(def => !def.IsAdded).ToList();
            foreach (CounterDefinitionPM counterDefinitionPM in newCounterDefinitions)
            {
                counterDefinitionService.Create(counterDefinitionPM);
                counterDefinitionPM.IsAdded = true;
            }
        }
        //private void HandleActiveCounterDefinitions(List<CounterDefinitionPM> counterDefinitions)
        //{
        //    List<CounterDefinitionPM> activeCounterdefinitions = counterDefinitions.Where(def => !def.InActive).ToList();
        //    if (activeCounterdefinitions == null || activeCounterdefinitions.Count == 0) return;
        //    foreach(CounterDefinitionPM counterDefinitionPM in activeCounterdefinitions)
        //    {
        //        if (counterDefinitionPM.Id == null)
        //        {
        //            counterDefinitionService.Create(counterDefinitionPM);
        //        }

        //        else
        //        {
        //            counterDefinitionService.Update(counterDefinitionPM);
        //        }
        //    }
        //}

        //private void HandleInActiveCounterDefinitions(List<CounterDefinitionPM> counterDefinitions)
        //{
        //    List<CounterDefinitionPM> inActiveCounterdefinitions = counterDefinitions.Where(def => def.InActive).ToList();
        //    if (inActiveCounterdefinitions == null || inActiveCounterdefinitions.Count == 0) return;
        //    RemoveInActiveCounterDefinitions(inActiveCounterdefinitions);
        //}

        //private void RemoveInActiveCounterDefinitions(List<CounterDefinitionPM> inActiveCounterdefinitions)
        //{
        //    foreach(CounterDefinitionPM counterDefinitionPM in inActiveCounterdefinitions)
        //    {
        //        RemoveCounterDefinitionPM(counterDefinitionPM);
        //    }
        //}

        //private void RemoveCounterDefinitionPM(CounterDefinitionPM counterDefinitionPM)
        //{
        //    if (string.IsNullOrEmpty(counterDefinitionPM.Id)) return;
        //    CounterDefinition counterDefinition = counterDefinitionRepository.GetSingleCounterDefinition(counterDefinitionPM.Id, counterDefinitionPM.Tenant);
        //    if (counterDefinition == null) return;
        //    counterDefinitionRepository.Remove(counterDefinition);
        //}
    }
}

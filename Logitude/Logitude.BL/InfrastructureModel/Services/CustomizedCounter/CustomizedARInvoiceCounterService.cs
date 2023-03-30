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

        public void Run(List<CounterDefinitionPM> counterDefinitions, string counterId)
        {
            if (counterDefinitions == null || counterDefinitions.Count == 0) return;
            UpdateCustomizedCounterDefinitions(counterDefinitions, counterId);
            AddCustomizedCounterDefinitions(counterDefinitions);
        }
        private void UpdateCustomizedCounterDefinitions(List<CounterDefinitionPM> counterDefinitions, string counterId)
        {
            List<CounterDefinitionPM> existingCounterDefinitions = counterDefinitionQuery.GetCustomizedCounterDefinitionsByCounterId(counterId, tenant).ToList();
            if (existingCounterDefinitions == null || existingCounterDefinitions.Count == 0) return;
            foreach (CounterDefinitionPM counterDefinitionPM in counterDefinitions)
            {
                UpdateCustomizedCounterDefnition(existingCounterDefinitions, counterDefinitionPM);
            }
        }

        private void UpdateCustomizedCounterDefnition(List<CounterDefinitionPM> existingCounterDefinitions, CounterDefinitionPM counterDefinitionPM)
        {
            if (!existingCounterDefinitions.Where(def => def.Parameter2 == counterDefinitionPM.Parameter2).Any()) return;
            counterDefinitionService.Update(counterDefinitionPM);
            counterDefinitionPM.IsAdded = true;
        }

        private void AddCustomizedCounterDefinitions(List<CounterDefinitionPM> counterDefinitions)
        {
            List<CounterDefinitionPM> newCounterDefinitions = counterDefinitions.Where(def => !def.IsAdded).ToList();
            foreach (CounterDefinitionPM counterDefinitionPM in newCounterDefinitions)
            {
                CreateCustomizedCounterDefinition(counterDefinitionPM);
            }
        }

        private void CreateCustomizedCounterDefinition(CounterDefinitionPM counterDefinitionPM)
        {
            counterDefinitionService.Create(counterDefinitionPM);
            counterDefinitionPM.IsAdded = true;
        }

        public void RemoveCustomizedCounterDefinitionsByCounterId(string counterId)
        {
            List<CounterDefinition> customizedCounterDefinitions = counterDefinitionRepository.GetCounterDefinitionsByCounterId(counterId, tenant).Where(def => def.IsCustomized).ToList();
            if (customizedCounterDefinitions == null || customizedCounterDefinitions.Count == 0) return;

            foreach (CounterDefinition counterDefinition in customizedCounterDefinitions)
            {
                counterDefinitionRepository.Remove(counterDefinition);
            }
            counterDefinitionRepository.SubmitChanges();
        }


    }
}

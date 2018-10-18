using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.EntityService;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddBatchServicesDefinitions
    {
        public static void AddBatchServicesDefinition(BatchServicesDefinitionPM batchServicesDefinition, BatchServicesDefinitionRepository batchServicesDefinitionRepository)
        {
            Dictionary<string, BatchServicesDefinition> BatchServicesDefinitions = batchServicesDefinitionRepository.GetAllBatchServicesDefinitions().ToDictionary(d => d.Code, a => a);
           
            if (!BatchServicesDefinitions.Keys.Contains(batchServicesDefinition.Code))
            {
                BatchServicesDefinitionService service = new BatchServicesDefinitionService(GlobalContext.GetContext());
                service.Create(batchServicesDefinition);
                //batchServicesDefinitionRepository.Add(batchServicesDefinition);
            }
            else
            {
                BatchServicesDefinitionService service = new BatchServicesDefinitionService(GlobalContext.GetContext());
                service.ModifyDefinitions(batchServicesDefinition);
            }
            
        }
    }
}
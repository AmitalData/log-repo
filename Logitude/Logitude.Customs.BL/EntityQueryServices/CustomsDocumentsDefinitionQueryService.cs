using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.Data;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsDocumentsDefinitionQueryService : EntityQueryService<CustomsDocumentsDefinition, CustomsDocumentsDefinitionKeys, CustomsDocumentsDefinitionPM, object, CustomsDocumentsDefinitionKeys>
    {
        /*
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, CustomsDocumentsDefinitionPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomsDocumentsDefinitionKeys keys = entityKeys as CustomsDocumentsDefinitionKeys;
            
        }
        */
        
        public List<CustomsDocumentsDefinitionPM> GetCustomsDocumentsDefinitionsForDeclaration(string cargoTypeCode, string processTypeCode, string transportTypeCode, int tenant)
        {
            if(String.IsNullOrWhiteSpace(cargoTypeCode) || String.IsNullOrWhiteSpace(processTypeCode) || String.IsNullOrWhiteSpace(transportTypeCode))
            {
                return null;
            }
            
            List <CustomsDocumentsDefinition> CustomsDocumentsDefinitions = repository.GetCustomsDocumentsDefinitionsForDeclaration(cargoTypeCode, processTypeCode, transportTypeCode, tenant);
            List<CustomsDocumentsDefinitionPM> CustomsDocumentsDefinitionPMs = new List<CustomsDocumentsDefinitionPM>();
            string docType = null;
            foreach (CustomsDocumentsDefinition definition in CustomsDocumentsDefinitions)
            {
                if(definition.DocumentTypeCode != docType)
                {
                    docType = definition.DocumentTypeCode;
                    CustomsDocumentsDefinitionPM DefinitionPM = new CustomsDocumentsDefinitionPM();
                    DefinitionPM.DocumentTypeCode = definition.DocumentTypeCode;
                    DefinitionPM.Mandatory = definition.Mandatory;
                    CustomsDocumentsDefinitionPMs.Add(DefinitionPM);
                }                
            }
            return CustomsDocumentsDefinitionPMs;
        }

    }
}

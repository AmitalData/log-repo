using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityDataMappings;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ConsignmentQueryService : EntityQueryService<Consignment, ConsignmentKeys, ConsignmentPM, DeclarationPM, DeclarationKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, ConsignmentPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ConsignmentKeys consignmentKeys = entityKeys as ConsignmentKeys;
            ConsignmentPackageQueryService consignmentPackageQueryService = new ConsignmentPackageQueryService(context);

            entityPM.ConsignmentPackages = consignmentPackageQueryService.GetMulti(consignmentKeys, true);

            if (entityPM.ConsignmentPackages != null)
            {
                if (entityPM.ConsignmentPackages.Count > 0)
                {
                    entityPM.ConsignmentPackagLastLineNumber = entityPM.ConsignmentPackages.Max(m => m.LineNumber);
                }
            }

            ConsignmentInternalTransitionQueryService consignmentInternalTransitionQueryService = new ConsignmentInternalTransitionQueryService(context);

            entityPM.ConsignmentInternalTransitions = consignmentInternalTransitionQueryService.GetMulti(consignmentKeys, true);
            if (entityPM.ConsignmentInternalTransitions != null)
            {
                if (entityPM.ConsignmentInternalTransitions.Count > 0)
                {
                    entityPM.ConsignmentInternalTransitionLastLineNumber = entityPM.ConsignmentInternalTransitions.Max(m => m.LineNumber);
                }
            }

            base.GetComposition(entityKeys, entityPM);
        }

        public Consignment GetConsignmentByIdentifiers(string cargoTypeCode, string manifestNumber, string secondCargoID, int tenant)
        {
            if (String.IsNullOrWhiteSpace(manifestNumber) || String.IsNullOrWhiteSpace(secondCargoID)) return null;
            return repository.GetConsignmentByIdentifiers(cargoTypeCode, manifestNumber, secondCargoID, tenant);
        }

        public int? GetMaxCounterKey(string declarationId, int tenant)
        {
            return repository.GetMaxCounterKey(declarationId, tenant);
        }

        public string GetDeclarationIdByConsignmentCargoId(string manifestNumber, string secondCargoID, string thirdCargoID, int tenant)
        {
            if (String.IsNullOrWhiteSpace(manifestNumber) && String.IsNullOrWhiteSpace(secondCargoID) && String.IsNullOrWhiteSpace(thirdCargoID)) return null;
            return repository.GetDeclarationIdByConsignmentCargoId(manifestNumber, secondCargoID, thirdCargoID, tenant);
        }
        public string GetDeclarationIdBythirdCargoID(string thirdCargoID, int tenant, List<string> idList = null)
        {
            return repository.GetDeclarationIdBythirdCargoID(thirdCargoID, tenant, idList);
        }


        public List<Consignment> GetConsgnmentByDeclarationId(string declarationId, int tenant)
        {
            return repository.GetConsgnmentByDeclarationId(declarationId, tenant);
        }

        public List<Consignment> GetConsgnmentByDeclarationIdForDataMapping( string declarationId, int tenant)
        {
            return repository.GetConsgnmentByDeclarationIdForDataMapping(declarationId, tenant);
        }
   


        public List<ConsignmentPM> GetConsigmentByExportContainerizationID(string containerizationId, int tenant)
        {
            var query = repository.GetConsigmentByExportContainerizationID(containerizationId, tenant);
            List<Consignment> Consignments = query.ToList();
            ConsignmentDataMapping mappings = new ConsignmentDataMapping();
            List<ConsignmentPM> ConsignmentPMs = new List<ConsignmentPM>();
            foreach (Consignment consignment in Consignments)
            {
                ConsignmentPM ConsignmentPM = new ConsignmentPM();
                mappings.CustomPOCOToPM(ConsignmentPM, consignment);
                mappings.POCOToPM(ConsignmentPM, consignment);
                GetComposition(new ConsignmentKeys() { DeclarationId = consignment.DeclarationId, }, ConsignmentPM);
                ConsignmentPMs.Add(ConsignmentPM);
            }
            return ConsignmentPMs;
        }


    }
}

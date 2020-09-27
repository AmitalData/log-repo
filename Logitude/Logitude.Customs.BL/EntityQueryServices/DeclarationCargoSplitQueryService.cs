using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeclarationCargoSplitQueryService
    {

        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, DeclarationCargoSplitPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            DeclarationCargoSplitKeys DeclarationCargoSplitKeys = entityKeys as DeclarationCargoSplitKeys;
            DecCargoSplitConQueryService decCargoSplitConQueryService = new DecCargoSplitConQueryService(context);
            entityPM.DecCargoSplitCons = decCargoSplitConQueryService.GetMulti(DeclarationCargoSplitKeys, true);

            if (entityPM.DecCargoSplitCons != null)
            {
                if (entityPM.DecCargoSplitCons.Count > 0)
                {
                    entityPM.DecCargoSplitConLastLineNumber = entityPM.DecCargoSplitCons.Max(m => m.LineNumber);
                }
            }

            DecCargoSplitCargoIdentifierQueryService decCargoSplitCargoIdentifierQueryService = new DecCargoSplitCargoIdentifierQueryService(context);
            entityPM.DecCargoSplitCargoIdentifiers = decCargoSplitCargoIdentifierQueryService.GetMulti(DeclarationCargoSplitKeys, true);
            if (entityPM.DecCargoSplitCargoIdentifiers != null)
            {
                if (entityPM.DecCargoSplitCargoIdentifiers.Count > 0)
                {
                    entityPM.DecCargoSplitCargoIdentifierLastLineNumber = entityPM.DecCargoSplitCargoIdentifiers.Max(m => m.LineNumber);
                }
            }
        }

        public string GetIdByDeclarationCargoSplitRequestNumber(string declarationCargoSplitRequestNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationCargoSplitRequestNumber)) return "";
            return repository.GetIdByDeclarationCargoSplitRequestNumber(declarationCargoSplitRequestNumber, tenant);
        }

        public List<DeclarationCargoSplitPM> GetDeclarationCargoSplitsList(string declarationId, int tenant)
        {
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(tenant);

            var declarationPMs = declarationQueryService.GetDeclarationAmendmentsById(tenant, declarationId);

            List<string> declarationIds = new List<string>();
            declarationPMs.ForEach(x => declarationIds.Add(x.Id));
            declarationIds.Add(declarationId);

            List<DeclarationCargoSplit> DeclarationCargoSplits = repository.GetDeclarationCargoSplitsList(declarationIds, tenant);
            List<DeclarationCargoSplitPM> DeclarationCargoSplitList = new List<DeclarationCargoSplitPM>();
            if (DeclarationCargoSplits != null)
            {
                /*
                foreach (var DeclarationCargoSplitItem in DeclarationCargoSplits)
                {
                    DeclarationCargoSplitPM DeclarationCargoSplitPM = this.GetSingle(DeclarationCargoSplitItem.Id,true,false);
                    DeclarationCargoSplitList.Add(DeclarationCargoSplitPM);
                }
                */
                var pocos = DeclarationCargoSplits.ToList();
                var pmList = pocos.Select(poco => this.GetEntityPM(poco, true, new DeclarationCargoSplitKeys() { Id = poco.Id } ))
                   .ToList();
                DeclarationCargoSplitList = pmList;
            }


            return DeclarationCargoSplitList;
        }

        public string GetIdByCargoIdentifiers(string cargoIdentifierKey1, string cargoIdentifierKey2, string cargoIdentifierKey3, int cargoIdentifierType, int tenant)
        {
            if (String.IsNullOrWhiteSpace(cargoIdentifierKey1) || String.IsNullOrWhiteSpace(cargoIdentifierKey2) || String.IsNullOrWhiteSpace(cargoIdentifierKey3)) return "";
            return repository.GetIdByCargoIdentifiers(cargoIdentifierKey1, cargoIdentifierKey2, cargoIdentifierKey3, cargoIdentifierType, tenant);
        }
    }
}

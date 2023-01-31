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
    public partial class ConsignmentPackageQueryService : EntityQueryService<ConsignmentPackage, ConsignmentPackageKeys, ConsignmentPackagePM, ConsignmentPM, ConsignmentKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, ConsignmentPackagePM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ConsignmentPackageKeys consignmentPackageKeys = entityKeys as ConsignmentPackageKeys;
            ConsignmentPackDangerQueryService consignmentPackDangerQueryService = new ConsignmentPackDangerQueryService(context);

            entityPM.ConsignmentPackDangers = consignmentPackDangerQueryService.GetMulti(consignmentPackageKeys, false);

        }

        public List<ConsignmentPackagePM> GetConsignmentPackagesForDeclaration(string declarationId, bool getComposition = false)
        {
            var mappings = new ConsignmentPackageDataMapping();
            var consignmentPackagesPMs = new List<ConsignmentPackagePM>();
            List<ConsignmentPackage> listConPackages = repository.GetConsignmentPackagesByDeclaration(declarationId);

            foreach (ConsignmentPackage invoice in listConPackages)
            {
                var consignmentPackagesPM = new ConsignmentPackagePM();

                mappings.CustomPOCOToPM(consignmentPackagesPM, invoice);
                mappings.POCOToPM(consignmentPackagesPM, invoice);

                if (getComposition)
                    GetComposition(new ConsignmentPackageKeys() { DeclarationId = consignmentPackagesPM.DeclarationId, ConsignmentNumber = consignmentPackagesPM.ConsignmentNumber, LineNumber = consignmentPackagesPM.LineNumber }, consignmentPackagesPM);

                consignmentPackagesPMs.Add(consignmentPackagesPM);
            }

            return consignmentPackagesPMs.OrderBy(d => d.SequenceNumeric).ToList();
        }
    }
}

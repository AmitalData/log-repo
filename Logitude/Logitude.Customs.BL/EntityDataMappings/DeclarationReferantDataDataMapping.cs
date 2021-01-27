
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{

    public partial class DeclarationReferantDataDataMapping : IMapping<DeclarationReferantDataPM, DeclarationReferantData>
    {

        public void CustomPMToPOCO(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO)
        {
            entityPOCO.DeclarationId = entityPM.DeclarationId;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(DeclarationReferantDataPM entityPM, DeclarationReferantData entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private static void BuildSearchFields(DeclarationReferantDataPM entityPM, DeclarationReferantData poco, bool isNewEntity)
        {
            string result = "";
            DeclarationQueryService declarationQuery = new DeclarationQueryService(poco.Tenant);
            DeclarationPM declaration = declarationQuery.GetSingle(entityPM.DeclarationId, false, false);
            if (declaration != null)
            {
                if (!string.IsNullOrEmpty(declaration.DeclarationNumber))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.DeclarationNumber : result + "," + declaration.DeclarationNumber;
                }

                if (!string.IsNullOrEmpty(declaration.CustomFileNo))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.CustomFileNo : result + "," + declaration.CustomFileNo;
                }
                if (!string.IsNullOrEmpty(declaration.CustomerId))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.CustomerId : result + "," + declaration.CustomerName;
                }
                ConsignmentQueryService cosigmentQuery = new ConsignmentQueryService(poco.Tenant);
                ConsignmentPM consignment = cosigmentQuery.GetSingle(entityPM.DeclarationId,1, false, false);
                if (consignment != null)
                {
                    if (!string.IsNullOrEmpty(consignment.ManifestNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? consignment.ManifestNumber : result + "," + consignment.ManifestNumber;
                    }
                    if (!string.IsNullOrEmpty(consignment.SecondCargoID))
                    {
                        result = string.IsNullOrEmpty(result) ? consignment.SecondCargoID : result + "," + consignment.SecondCargoID;
                    }
                    if (!string.IsNullOrEmpty(consignment.ThirdCargoID))
                    {
                        result = string.IsNullOrEmpty(result) ? consignment.ThirdCargoID : result + "," + consignment.ThirdCargoID;
                    }
                }
            }
            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
    }


}

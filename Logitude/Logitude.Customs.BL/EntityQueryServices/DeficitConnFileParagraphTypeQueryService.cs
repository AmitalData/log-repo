using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeficitConnFileParagraphTypeQueryService : EntityQueryService<DeficitConnFileParagraphType, DeficitConnFileParagraphTypeKeys, DeficitConnFileParagraphTypePM, object, DeficitConnFileParagraphTypeKeys>

    {

        public List<DeficitConnFileParagraphTypeList> GetDeficitConnectedFileParagraphTypesByDeclarationId(string declarationId, string deficitId, int tenant)
        {
            List<DeficitConnFileParagraphType> deficitConnectedFileParagraphTypes = repository.GetDeficitConnectedFileParagraphTypesByDeclarationId(declarationId,deficitId, tenant);
            List<DeficitConnFileParagraphTypeList> deficitConnectedFileParagraphTypeLists = new List<DeficitConnFileParagraphTypeList>();
            foreach (DeficitConnFileParagraphType item in deficitConnectedFileParagraphTypes)
            {
                DeficitConnFileParagraphTypeList deficitConnectedFileParagraphTypeList = new DeficitConnFileParagraphTypeList()
                {
                    Amount = item.Amount,
                    DeclarationId = item.DeclarationId,
                    DeficitId = item.DeficitId,
                    ParagraphTypeCode = item.ParagraphTypeCode,
                    Tenant = item.Tenant,
                    ParagraphTypeName = item.ParagraphType != null? item.ParagraphType.LocalName : null,
                };

                deficitConnectedFileParagraphTypeLists.Add(deficitConnectedFileParagraphTypeList);
            }

            return deficitConnectedFileParagraphTypeLists;
        }
    }
}

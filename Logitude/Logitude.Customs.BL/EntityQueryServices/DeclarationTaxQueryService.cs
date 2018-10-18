using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeclarationTaxQueryService
    {
        public List<DeclarationTaxPM> GetDeclarationTaxesForDeclarationId(string declarationId, int tenant)
        {
            IQueryable<DeclarationTax> taxes = repository.GetDeclarationTaxesForDeclarationId(declarationId, tenant);
            List<DeclarationTaxPM> taxpms = (from a in taxes
                                             select new DeclarationTaxPM()
                                             {
                                                 DeclarationId = a.DeclarationId,
                                                 DeferredTaxAmount = a.DeferredTaxAmount,
                                                 TaxBaseAmount = a.TaxBaseAmount,
                                                 TaxTypeCode = a.TaxTypeCode,
                                                 Tenant = a.Tenant,
                                                 TotalAmount = a.TotalAmount,
                                                 TaxTypeName = a.ParagraphType.LocalName,
                                             }).ToList();
            return taxpms;
        }
    }
}

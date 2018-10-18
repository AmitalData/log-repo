using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceModificationQueryService:EntityQueryService<SupplierInvoiceModification, SupplierInvoiceModificationKeys, SupplierInvoiceModificationPM, SupplierInvoicePM, SupplierInvoiceKeys>
    {
        public List<SupplierInvoiceModificationPM> GetSupplierInvoiceModificationsForDeclaration(string declarationId)
        {
            ICustomContext context = MainContext as CustomContext;
            List<SupplierInvoiceModification> mods = this.repository.GetSupplierInvoiceModificationsForDeclaration(declarationId);
            List<SupplierInvoiceModificationPM> modPMs = new List<SupplierInvoiceModificationPM>();
            SupplierInvoiceModificationDataMapping mappings = new SupplierInvoiceModificationDataMapping();
            foreach (SupplierInvoiceModification mod in mods)
            {
                SupplierInvoiceModificationPM modPM = new SupplierInvoiceModificationPM();
                mappings.CustomPOCOToPM(modPM, mod);
                mappings.POCOToPM(modPM, mod);
                modPMs.Add(modPM);
            }
            return modPMs;

        }

        public List<SupplierInvoiceModificationPM> GetSupplierInvoiceModificationsForInvoice(string declarationId,int counterKey)
        {
            ICustomContext context = MainContext as CustomContext;
            List<SupplierInvoiceModification> mods = this.repository.GetMulti(new SupplierInvoiceKeys() { DeclarationId = declarationId, InvoiceCounterKey = counterKey });
            List<SupplierInvoiceModificationPM> modPMs = new List<SupplierInvoiceModificationPM>();
            SupplierInvoiceModificationDataMapping mappings = new SupplierInvoiceModificationDataMapping();
            foreach (SupplierInvoiceModification mod in mods)
            {
                SupplierInvoiceModificationPM modPM = new SupplierInvoiceModificationPM();
                mappings.CustomPOCOToPM(modPM, mod);
                mappings.POCOToPM(modPM, mod);
                modPMs.Add(modPM);
            }
            return modPMs;

        }
    }
}

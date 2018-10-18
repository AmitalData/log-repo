using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class VendorCommunicationUpdateService : EntityUpdateService<VendorCommunication, VendorCommunicationPM, CustomsVendorPM>
    {
        protected override void OnCreating(VendorCommunicationPM entityPM, CustomsVendorPM entityParentPM)
        {
            entityPM.VendorId = entityParentPM.Id;
            entityParentPM.LastLineNumber += 1;
            entityPM.LineNumber = entityParentPM.LastLineNumber;

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}

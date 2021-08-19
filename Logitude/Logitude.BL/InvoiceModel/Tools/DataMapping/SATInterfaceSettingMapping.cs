using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.DataMapping
{
    public class SATInterfaceSettingMapping
    {
        public static void MapEntity(SATInterfaceSettingPM entityPM, SATInterfaceSetting entity, bool isNewState)
        {
            if (isNewState)
            {
                //entity.Id = entityPM.Id;
                entity.Tenant = entityPM.Tenant;

            }

            entity.SATInterfaceCode = entityPM.SATInterfaceCode;
            entity.Token = entityPM.Token;
            entity.ActivationDate = entityPM.ActivationDate;
            entity.MetodoPagoCode = entityPM.MetodoPagoCode;
            entity.IsARInvoiceTransferEnabled = entityPM.IsARInvoiceTransferEnabled;
            entity.IsCartaPorteTransferEnabled = entityPM.IsCartaPorteTransferEnabled;

        }
    }
}

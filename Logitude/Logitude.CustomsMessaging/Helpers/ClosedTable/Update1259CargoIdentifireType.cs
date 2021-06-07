using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class Update1259CargoIdentifireType : ClosedTableGenericService<CargoIdentifireTypePM>
    {
        public Update1259CargoIdentifireType(ICustomContext CustomContext,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<CargoIdentifireTypePM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<CargoIdentifireTypePM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CargoIdentifireTypePM curDbPM)
        {
            var gov = mehesTableRow.MyCargoIdentifireType ?? new CargoIdentifireType();
            return base.IsEqual(mehesTableRow, curDbPM)
                && curDbPM.IsForDeclarationExport == gov.IsForDeclarationExport 
                && gov.IsForDeclarationImport == curDbPM.IsForDeclarationImport
                 && gov.IsForManifest == curDbPM.IsForManifest
                  && gov.IsKey2Mandatory == curDbPM.IsKey2Mandatory
                   && gov.IsKey3Mandatory == curDbPM.IsKey3Mandatory
                    && gov.CargoIdentifierKey1Name == curDbPM.CargoIdentifierKey1Name
                     && gov.CargoIdentifierKey2Name == curDbPM.CargoIdentifierKey2Name
                      && gov.CargoIdentifierKey3Name == curDbPM.CargoIdentifierKey3Name;
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CargoIdentifireTypePM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyCargoIdentifireType ?? new CargoIdentifireType();
            curDbPM.IsForDeclarationExport = gov.IsForDeclarationExport;
            curDbPM.IsForDeclarationImport = gov.IsForDeclarationImport;
            curDbPM.IsForManifest = gov.IsForManifest;
            curDbPM.IsKey2Mandatory = gov.IsKey2Mandatory; 
            curDbPM.IsKey3Mandatory = gov.IsKey3Mandatory;
            curDbPM.CargoIdentifierKey1Name = gov.CargoIdentifierKey1Name;
            curDbPM.CargoIdentifierKey2Name = gov.CargoIdentifierKey2Name;
            curDbPM.CargoIdentifierKey3Name = gov.CargoIdentifierKey3Name;
        }
    }
}


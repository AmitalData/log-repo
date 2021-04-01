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
    public class Update1366ContainerType : ClosedTableGenericService<ContainerTypePM>
    {
        public Update1366ContainerType(ICustomContext CustomContext,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<ContainerTypePM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<ContainerTypePM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, ContainerTypePM curDbPM)
        {
            var gov = mehesTableRow.MyContainerType ?? new ContainerType();
            return base.IsEqual(mehesTableRow, curDbPM);
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, ContainerTypePM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyContainerType ?? new ContainerType();
            curDbPM.IsNeedSeal = gov.IsNeedSeal;
            curDbPM.IsIsoTankContainer = gov.IsIsoTankContainer;
            curDbPM.IsAerial = gov.IsAerial;


        }
    }
}


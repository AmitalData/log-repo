using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class Update1344InternationalSite : ClosedTableGenericService<InternationalSitePM>
    {
        private int _TotalSaved;
        
        public Update1344InternationalSite(ICustomContext CustomContext,
            ///string tableId,
            //SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<InternationalSitePM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<InternationalSitePM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }
        protected override bool SuppressMakeInactiveDueUpdatePatch()
        {
            switch (this.RealTableName)
            {
                case "1344":
                    {
                        return false;
                    }
                    break;
                case "2653":
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }
            
            return base.SuppressMakeInactiveDueUpdatePatch();
        }

        protected override int SaveMyChanges()
        {
            var saved = base.SaveMyChanges();
            _TotalSaved = _TotalSaved + saved;
            if (_TotalSaved > 650)
            {
                //var t = Transaction.Current.TransactionInformation.CreationTime;
                var cs = this._CustomContext as CustomContext;
                if (false)
                {
                    var local = new List<InternationalSite>(cs.InternationalSites.Local);
                    foreach (var item in local)
                    {
                        cs.Entry(item).State = System.Data.Entity.EntityState.Detached; 
                    }
                }
                else
                {
                    cs.Dispose();
                    this._CustomContext = CustomContext.GetContext(this._Tenant);
                }
                
                _TotalSaved = 0;
            }
            return saved;
        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, InternationalSitePM curDbPM)
        {
            var gov = mehesTableRow.MyInternationalSite ?? new InternationalSiteP();
            return base.IsEqual(mehesTableRow, curDbPM) && curDbPM.CountryTypeCode == gov.CountryTypeCode;
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, InternationalSitePM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyInternationalSite ?? new InternationalSiteP();
            curDbPM.CountryTypeCode = gov.CountryTypeCode;

        }

        public string RealTableName { get; set; }
    }
}

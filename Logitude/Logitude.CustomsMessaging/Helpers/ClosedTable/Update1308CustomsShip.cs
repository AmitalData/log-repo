
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
    public class Update1308CustomsShip : ClosedTableGenericService<CustomsShipPM>
    {
        private int _TotalSaved;

        public Update1308CustomsShip(ICustomContext CustomContext, List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows, Func<ICustomContext, ICanUpdateClosedTable<CustomsShipPM>> CreateNewUpdateServiceFunc, Func<ICustomContext, ICanGetAllClosedTable<CustomsShipPM>> GetAllDbPMFunc, int tenant, bool forceUpdateUnChanged) : base(CustomContext, mehesTableRows, CreateNewUpdateServiceFunc, GetAllDbPMFunc, tenant, forceUpdateUnChanged)
           
        {
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
                    var local = new List<CustomsShip>(cs.CustomsShips.Local);
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

       
        

    }
}

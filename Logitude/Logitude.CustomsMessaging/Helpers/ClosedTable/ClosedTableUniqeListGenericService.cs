using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class ClosedTableUniqeListGenericService
    {

        public List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> MakeUniqeList(
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows

            )
        {


            mehesTableRows.ForEach(
                rec =>
                {
                    ManipulateRow(rec);
                });
            mehesTableRows = mehesTableRows.OrderBy(rec => RealKey(rec)).ToList();


            var distinctMehesTableRows = mehesTableRows
  .GroupBy(rec => RealKey(rec))
  .Select(g => g.Last())
  .ToList();
            int keyIsNullOrWhiteSpace = distinctMehesTableRows.RemoveAll(rec => string.IsNullOrWhiteSpace(rec.id));
            if (keyIsNullOrWhiteSpace > 0)
            {
                LogMessagingUtil.Instance.AppendLine(@" Remove All rows that is   IsNullOrWhiteSpace Count = " + keyIsNullOrWhiteSpace);
            }
            if (mehesTableRows.Count != distinctMehesTableRows.Count)
            {

                var repaet5 = mehesTableRows
                .GroupBy(rec => RealKey(rec))
                .Where(grp => grp.Count() > 1)
                .Select(grp => grp.Key).Take(5);
                var l = string.Join(",", repaet5);
                LogMessagingUtil.Instance.AppendLine(
@" Distinct  !!!! המכס שלח רשומות בעלי קוד (מפתח ) זהה
_MehesTableRows.Count = " + mehesTableRows.Count.ToString() + @"  
While distinctMehesTableRows.Count =" + distinctMehesTableRows.Count.ToString() + @"
Example:" + l);

                //throw new System.Exception("המכס שלח רשומות בעלי קוד (מפתח ) זהה");
                mehesTableRows = distinctMehesTableRows;
            }
            return mehesTableRows;
        }

        public List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> MakeUniqeList(
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> mehesTableRows
            )
        {

            var l = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
            foreach (var item in mehesTableRows)
            {
                l.Add(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew( item));
            }
            var uniqeList = new List < SYSTBL_NG_9001_MSG_SystemTablesResponseTableData > (MakeUniqeList(l));
            return uniqeList;
        }

        protected virtual string RealKey(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData rec)
        {
            return rec.id;
        }

        protected virtual void ManipulateRow(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData rec)
        {
            rec.id = rec.id.ToUpper();
        }

    }
}

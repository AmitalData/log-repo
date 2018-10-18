using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.CustomsMessaging.CustomsBook
{
    public class CustomsItemDetailsHistoryImport
    {
        public void MultiUpsert(List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> mehesCustomsItemDetailsHistoryRows, int tenant)
        {
            TransactionScope scope = null;
            var logContext = CustomContext.GetContext(tenant);
            try
            {
                var customsItemDetailsHistoryQueryService = new CustomsItemDetailsHistoryQueryService(logContext);
                var customsItemDetailsHistoryUpdateService = new CustomsItemDetailsHistoryUpdateService(logContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                List<CustomsItemDetailsHistoryPM> dbListCustomsItemDetailsHistoryRows = customsItemDetailsHistoryQueryService.GetAll();
                var i = 0;

                scope = TransactionFactory.GetNewTransaction();
                if (mehesCustomsItemDetailsHistoryRows != null)
                {
                    foreach (var mehesCustomsItemDetailsHistoryRow in mehesCustomsItemDetailsHistoryRows)
                    {
                        bool doUpdate = true;
                        var currentDBListCustomsItemDetailsHistoryRow = new CustomsItemDetailsHistoryPM();
                        if (dbListCustomsItemDetailsHistoryRows == null || dbListCustomsItemDetailsHistoryRows.Count() == 0)
                        {
                            currentDBListCustomsItemDetailsHistoryRow.ID = mehesCustomsItemDetailsHistoryRow.ID.ToString();
                            currentDBListCustomsItemDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        else
                        {
                            currentDBListCustomsItemDetailsHistoryRow = dbListCustomsItemDetailsHistoryRows.FirstOrDefault(rec => rec.ID == mehesCustomsItemDetailsHistoryRow.ID.ToString());
                            if (currentDBListCustomsItemDetailsHistoryRow == null)
                            {
                                currentDBListCustomsItemDetailsHistoryRow = new CustomsItemDetailsHistoryPM();
                                currentDBListCustomsItemDetailsHistoryRow.ID = mehesCustomsItemDetailsHistoryRow.ID.ToString();
                                currentDBListCustomsItemDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Insert;
                            }
                            else
                            {
                                doUpdate = CompareMehesToDBCustomsItemDetailsHistory(mehesCustomsItemDetailsHistoryRow, currentDBListCustomsItemDetailsHistoryRow);
                                currentDBListCustomsItemDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Update;
                            }
                        }

                        if (doUpdate)
                        {
                            currentDBListCustomsItemDetailsHistoryRow.CustomsItemID = mehesCustomsItemDetailsHistoryRow.CustomsItemID.ToString();
                            currentDBListCustomsItemDetailsHistoryRow.Title = mehesCustomsItemDetailsHistoryRow.Title;
                            currentDBListCustomsItemDetailsHistoryRow.EntityStatusID = mehesCustomsItemDetailsHistoryRow.EntityStatusID;
                            if (mehesCustomsItemDetailsHistoryRow.StartDate.HasValue)
                            {
                                currentDBListCustomsItemDetailsHistoryRow.StartDate = mehesCustomsItemDetailsHistoryRow.StartDate.Value;
                            }
                            if (mehesCustomsItemDetailsHistoryRow.EndDate.HasValue)
                            {
                                currentDBListCustomsItemDetailsHistoryRow.EndDate = mehesCustomsItemDetailsHistoryRow.EndDate.Value;
                            }

                            customsItemDetailsHistoryUpdateService.Update(currentDBListCustomsItemDetailsHistoryRow, false);
                            i++;
                            if (i > 100)
                            {
                                i = 0;
                                logContext.SaveChanges();
                                scope.Complete();
                                scope.Dispose();
                                scope = TransactionFactory.GetNewTransaction();
                            }
                        }
                    }
                }
            }
            finally
            {
                logContext.SaveChanges();
                scope.Complete();
                scope.Dispose();
            }
        }

        private static bool CompareMehesToDBCustomsItemDetailsHistory(UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory mehesCustomsItemDetailsHistoryRow, CustomsItemDetailsHistoryPM currentDBListCustomsItemDetailsHistoryRow)
        {
            if (mehesCustomsItemDetailsHistoryRow.Title != currentDBListCustomsItemDetailsHistoryRow.Title ||
                mehesCustomsItemDetailsHistoryRow.StartDate != currentDBListCustomsItemDetailsHistoryRow.StartDate ||
                mehesCustomsItemDetailsHistoryRow.EndDate != currentDBListCustomsItemDetailsHistoryRow.EndDate ||
                mehesCustomsItemDetailsHistoryRow.EntityStatusID != currentDBListCustomsItemDetailsHistoryRow.EntityStatusID)
            {
                return true;
            }

            return false;
        }
    }
}

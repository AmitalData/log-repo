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
    public class PropertiesDetailsHistoryImport
    {
        public void MultiUpsert(List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> mehesPropertiesDetailsHistoryRows, int tenant)
        {
            TransactionScope scope = null;
            var logContext = CustomContext.GetContext(tenant);

            try
            {
                var propertiesDetailsHistoryQueryService = new PropertiesDetailsHistoryQueryService(logContext);
                var propertiesDetailsHistoryUpdateService = new PropertiesDetailsHistoryUpdateService(logContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                List<PropertiesDetailsHistoryPM> dbListPropertiesDetailsHistoryRows = propertiesDetailsHistoryQueryService.GetAll();


                var i = 0;

                scope = TransactionFactory.GetNewTransaction();

                if (mehesPropertiesDetailsHistoryRows != null)
                {
                    foreach (var mehesPropertiesDetailsHistoryRow in mehesPropertiesDetailsHistoryRows)
                    {
                        bool doUpdate = true;
                        var currentDBListPropertiesDetailsHistoryRow = new PropertiesDetailsHistoryPM();
                        if (dbListPropertiesDetailsHistoryRows == null || dbListPropertiesDetailsHistoryRows.Count() == 0)
                        {
                            currentDBListPropertiesDetailsHistoryRow.ID = mehesPropertiesDetailsHistoryRow.ID.ToString();
                            currentDBListPropertiesDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        else
                        {
                            currentDBListPropertiesDetailsHistoryRow = dbListPropertiesDetailsHistoryRows.FirstOrDefault(rec => rec.ID == mehesPropertiesDetailsHistoryRow.ID.ToString());
                            if (currentDBListPropertiesDetailsHistoryRow == null)
                            {
                                currentDBListPropertiesDetailsHistoryRow = new PropertiesDetailsHistoryPM();
                                currentDBListPropertiesDetailsHistoryRow.ID = mehesPropertiesDetailsHistoryRow.ID.ToString();
                                currentDBListPropertiesDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Insert;
                            }
                            else
                            {
                                doUpdate = CompareMehesToDBPropertiesDetailsHistory(mehesPropertiesDetailsHistoryRow, currentDBListPropertiesDetailsHistoryRow);
                                currentDBListPropertiesDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Update;
                            }
                        }

                        if (doUpdate)
                        {
                            currentDBListPropertiesDetailsHistoryRow.CustomsItemID = mehesPropertiesDetailsHistoryRow.CustomsItemID.ToString();
                            currentDBListPropertiesDetailsHistoryRow.EntityStatusID = mehesPropertiesDetailsHistoryRow.EntityStatusID;
                            if (mehesPropertiesDetailsHistoryRow.StartDate.HasValue)
                            {
                                currentDBListPropertiesDetailsHistoryRow.StartDate = mehesPropertiesDetailsHistoryRow.StartDate.Value;
                            }
                            if (mehesPropertiesDetailsHistoryRow.EndDate.HasValue)
                            {
                                currentDBListPropertiesDetailsHistoryRow.EndDate = mehesPropertiesDetailsHistoryRow.EndDate.Value;
                            }
                            if (mehesPropertiesDetailsHistoryRow.MeasurementUnitID.HasValue)
                            {
                                currentDBListPropertiesDetailsHistoryRow.MeasurementUnitID = mehesPropertiesDetailsHistoryRow.MeasurementUnitID.Value;
                            }

                            propertiesDetailsHistoryUpdateService.Update(currentDBListPropertiesDetailsHistoryRow, false);
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


        private static bool CompareMehesToDBPropertiesDetailsHistory(UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory mehesPropertiesDetailsHistoryRow, PropertiesDetailsHistoryPM currentDBListPropertiesDetailsHistoryRow)
        {
            if (mehesPropertiesDetailsHistoryRow.StartDate != currentDBListPropertiesDetailsHistoryRow.StartDate ||
                mehesPropertiesDetailsHistoryRow.EndDate != currentDBListPropertiesDetailsHistoryRow.EndDate ||
                mehesPropertiesDetailsHistoryRow.EntityStatusID != currentDBListPropertiesDetailsHistoryRow.EntityStatusID ||
                mehesPropertiesDetailsHistoryRow.MeasurementUnitID != currentDBListPropertiesDetailsHistoryRow.MeasurementUnitID)
            {
                return true;
            }

            return false;
        }
    }
}

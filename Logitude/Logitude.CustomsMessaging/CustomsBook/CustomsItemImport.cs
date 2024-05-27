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
    public class CustomsItemImport
    {
        public void MultiUpsert(List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem> mehesCustomsItemRows, int tenant)
        {
            TransactionScope scope = null;
            var logContext = CustomContext.GetContext(tenant);
            try
            {
                var customsItemQueryService = new CustomsItemQueryService(logContext);
                var customsItemUpdateService = new CustomsItemUpdateService(logContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                List<CustomsItemPM> dbListCustomsItemRows = customsItemQueryService.GetAll();
                var i = 0;

                scope = TransactionFactory.GetNewTransaction();

                if (mehesCustomsItemRows != null)
                {
                    foreach (var mehesCustomsItemRow in mehesCustomsItemRows)
                    {
                        bool doUpdate = true;
                        var currentDBListCustomsItemRow = new CustomsItemPM();
                        if (dbListCustomsItemRows == null || dbListCustomsItemRows.Count() == 0)
                        {
                            currentDBListCustomsItemRow.ID = mehesCustomsItemRow.ID.ToString();
                            currentDBListCustomsItemRow.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        else
                        {
                            currentDBListCustomsItemRow = dbListCustomsItemRows.FirstOrDefault(rec => rec.ID == mehesCustomsItemRow.ID.ToString());
                            if (currentDBListCustomsItemRow == null)
                            {
                                currentDBListCustomsItemRow = new CustomsItemPM();
                                currentDBListCustomsItemRow.ID = mehesCustomsItemRow.ID.ToString();
                                currentDBListCustomsItemRow.ChangeSetOp = ChangeSetOperation.Insert;
                            }
                            else
                            {
                                doUpdate = CompareMehesToDBCustomsItem(mehesCustomsItemRow, currentDBListCustomsItemRow);
                                currentDBListCustomsItemRow.ChangeSetOp = ChangeSetOperation.Update;
                            }
                        }

                        if (doUpdate)
                        {
                            currentDBListCustomsItemRow.FullClassification = mehesCustomsItemRow.FullClassification;
                            currentDBListCustomsItemRow.CustomsItemHierarchicLocationID = mehesCustomsItemRow.CustomsItemHierarchicLocationID;
                            currentDBListCustomsItemRow.ComputedCheckDigit = mehesCustomsItemRow.ComputedCheckDigit;
                            currentDBListCustomsItemRow.CustomsBookTypeID = mehesCustomsItemRow.CustomsBookTypeID;
                            currentDBListCustomsItemRow.CustomsItemCategoryID = mehesCustomsItemRow.CustomsItemCategoryID;

                            customsItemUpdateService.Update(currentDBListCustomsItemRow, false);
                            i++;
                            if (i > 100)
                            {
                                logContext.SaveChanges();
                                i = 0;
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

        private static bool CompareMehesToDBCustomsItem(UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem mehesCustomsItemRow, CustomsItemPM currentDBListCustomsItemRow)
        {
            if (mehesCustomsItemRow.FullClassification != currentDBListCustomsItemRow.FullClassification ||
                mehesCustomsItemRow.CustomsItemCategoryID != currentDBListCustomsItemRow.CustomsItemCategoryID ||
                mehesCustomsItemRow.CustomsItemHierarchicLocationID != currentDBListCustomsItemRow.CustomsItemHierarchicLocationID ||
                mehesCustomsItemRow.ComputedCheckDigit != currentDBListCustomsItemRow.ComputedCheckDigit)
            {
                return true;
            }

            return false;
        }
    }
}

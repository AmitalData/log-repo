using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.CustomsBook;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Update.Sivug
{
    public class CustomsBookImport
    {
        const int _Tenant = 1;

        public static void CustomsBookUpsert()
        {
            /*
            UPDATING CustomsBook DATABASE IN AMITAL:
            1. LOAD DATA FROM MDB TO SQLDB 
            2. Rename tables: Add 'CustomsBook_' to the current name (CustomsItem --> CustomsBook_CustomsItem)
            3. FOR EACH TABLE WE NEED TO CREATE A KEY
                ALTER TABLE [dbo].CustomsBook_CustomsItem]  ALTER  COLUMN ID INT NOT NULL 
                ALTER TABLE [dbo].CustomsBook_CustomsItem]  ADD PRIMARY KEY CLUSTERED  ( 	[ID] ASC )

                ALTER TABLE [dbo].CustomsBook_CustomsItemDetailsHistory  ALTER  COLUMN ID INT NOT NULL ;
                ALTER TABLE [dbo].CustomsBook_CustomsItemDetailsHistory  ADD PRIMARY KEY CLUSTERED  ( 	[ID] ASC )

                ALTER TABLE [dbo].CustomsBook_PropertiesDetailsHistory  ALTER  COLUMN ID INT NOT NULL ;
                ALTER TABLE [dbo].CustomsBook_PropertiesDetailsHistory  ADD PRIMARY KEY CLUSTERED  ( 	[ID] ASC )
            4.  Trancate CustomsBook tables (For CUSTOMSITEMS need to disable constraine before and reenable them after):
                truncate table "AMINET_MAIN"."CUSTOMSITEMDETAILSHISTORYS" reuse storage
                truncate table "AMINET_MAIN"."PROPERTIESDETAILSHISTORYS" reuse storage

                alter table CUSTOMSITEMDETAILSHISTORYS ENABLE constraint FK_206971240;
                alter table CUSTOMSITEMDETAILSHISTORYS DISABLE constraint FK_206971240;
                truncate table "AMINET_MAIN"."CUSTOMSITEMS" reuse storage 
                alter table PROPERTIESDETAILSHISTORYS ENABLE constraint FK_710877170;
                alter table PROPERTIESDETAILSHISTORYS DISABLE constraint FK_710877170;
            */

            var mehesCustomsItemRows = GetMehesCustomsItemRows();
            List<string> CustomsItemIDList = mehesCustomsItemRows.Select(rec => rec.ID.ToString()).ToList();
            var mehesCustomsItemDetailsHistoryRows = GetMehesCustomsItemDetailsHistoryRows(CustomsItemIDList);
            var mehesPropertiesDetailsHistoryRows = GetMehesPropertiesDetailsHistoryRows(CustomsItemIDList);


            //SetDBCustomsItem(mehesCustomsItemRows);
            //SetDBCustomsItemDetailsHistory(mehesCustomsItemDetailsHistoryRows);
            //SetDBPropertiesDetailsHistory(mehesPropertiesDetailsHistoryRows);

            //CustomsItemImport.MultiUpsert(mehesCustomsItemRows, _Tenant);
            //CustomsItemDetailsHistoryImport.MultiUpsert(mehesCustomsItemDetailsHistoryRows, _Tenant);
            //PropertiesDetailsHistoryImport.MultiUpsert(mehesPropertiesDetailsHistoryRows, _Tenant);

            CustomsItemImport customsItemImport = new Logitude.CustomsMessaging.CustomsBook.CustomsItemImport();
            customsItemImport.MultiUpsert(mehesCustomsItemRows, _Tenant);
            CustomsItemDetailsHistoryImport customsItemDetailsHistoryImport = new Logitude.CustomsMessaging.CustomsBook.CustomsItemDetailsHistoryImport();
            customsItemDetailsHistoryImport.MultiUpsert(mehesCustomsItemDetailsHistoryRows, _Tenant);
            PropertiesDetailsHistoryImport propertiesDetailsHistoryImport = new Logitude.CustomsMessaging.CustomsBook.PropertiesDetailsHistoryImport();
            propertiesDetailsHistoryImport.MultiUpsert(mehesPropertiesDetailsHistoryRows, _Tenant);

        }

        private static List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem> GetMehesCustomsItemRows()
        {
            using (var contx = new CustomsBookEntities())
            {
                //List<CustomsBook_CustomsItem> 
                 var   mehesCustomsItemRows = contx.CustomsBook_CustomsItem
                    .Where(rec => rec.CustomsBookTypeID == 1)
                    .Select(rec => new //CustomsBook_CustomsItem()
                    {
                        ID = rec.ID,
                        FullClassification = rec.FullClassification,
                        CustomsItemHierarchicLocationID = rec.CustomsItemHierarchicLocationID,
                        ComputedCheckDigit = rec.ComputedCheckDigit,
                        CustomsItemCategoryID = rec.CustomsItemCategoryID,
                        CustomsBookTypeID = rec.CustomsBookTypeID
                    })
                    .ToList();
                List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem> listAfter = new List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem>();

                foreach (var item in mehesCustomsItemRows)
                {
                    var newItem = new UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem()
                    {
                        ID = item.ID,
                        FullClassification = item.FullClassification,
                        CustomsItemHierarchicLocationID = item.CustomsItemHierarchicLocationID,
                        ComputedCheckDigit = item.ComputedCheckDigit,
                    };
                    if (item.CustomsItemCategoryID != null)
                    {
                        int resultInt;
                        if (int.TryParse(item.CustomsItemCategoryID.ToString(), out resultInt))
                        {
                            newItem.CustomsItemCategoryID = resultInt;

                        }
                    }
                    listAfter.Add(newItem);
                }

                return listAfter;
            }
        }

        private static List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> GetMehesCustomsItemDetailsHistoryRows(List<string> customsItemIDList)
        {
            using (var contx = new CustomsBookEntities())
            {
                //List<CustomsBook_CustomsItemDetailsHistory> mehesCustomsItemDetailsHistoryRows = new List<CustomsBook_CustomsItemDetailsHistory>();
                var mehesCustomsItemDetailsHistoryRows = contx.CustomsBook_CustomsItemDetailsHistory
                    .Select(item =>new
                    {
                        ID = item.ID,
                        Title = item.Title,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        item.CustomsItemID,
                        item.EntityStatusID
                    })
                    .ToList();
                mehesCustomsItemDetailsHistoryRows = mehesCustomsItemDetailsHistoryRows.Where(rec => customsItemIDList.Contains(rec.CustomsItemID.ToString())).ToList();

                List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> listAfter = new List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory>();

                foreach (var item in mehesCustomsItemDetailsHistoryRows)
                {
                    var newItem = new UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory()
                    {
                        ID = item.ID,
                        Title = item.Title,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                    };
                    int resultInt;
                    if (item.CustomsItemID != null)
                    {
                        if (int.TryParse(item.CustomsItemID.ToString(), out resultInt))
                        {
                            newItem.CustomsItemID = resultInt;

                        }
                    }
                    if (item.EntityStatusID != null)
                    {
                        if (int.TryParse(item.EntityStatusID.ToString(), out resultInt))
                        {
                            newItem.EntityStatusID = resultInt;

                        }
                    }
                    listAfter.Add(newItem);
                }

                return listAfter;
            }
        }

        private static List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> GetMehesPropertiesDetailsHistoryRows(List<string> customsItemIDList)
        {
            using (var contx = new CustomsBookEntities())
            {
                //List<CustomsBook_PropertiesDetailsHistory> mehesPropertiesDetailsHistoryRows = new List<CustomsBook_PropertiesDetailsHistory>();
                var mehesPropertiesDetailsHistoryRows = contx.CustomsBook_PropertiesDetailsHistory
                    .Select( item =>
                    new
                    {
                        ID = item.ID,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        MeasurementUnitID = item.MeasurementUnitID,
                        item.CustomsItemID,
                        item.EntityStatusID
                    })
                    .ToList();
                mehesPropertiesDetailsHistoryRows = mehesPropertiesDetailsHistoryRows.Where(rec => customsItemIDList.Contains(rec.CustomsItemID.ToString())).ToList();
                
                List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> listAfter = new List<UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory>();

                foreach (var item in mehesPropertiesDetailsHistoryRows)
                {
                    var newItem = new UnifreightIIG.Common.CustomsBookServiceReference.CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory()
                    {
                        ID = item.ID,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        MeasurementUnitID = item.MeasurementUnitID,
                    };
                    int resultInt;
                    if (item.CustomsItemID != null)
                    {
                        if (int.TryParse(item.CustomsItemID.ToString(), out resultInt))
                        {
                            newItem.CustomsItemID = resultInt;

                        }
                    }
                    if (item.EntityStatusID != null)
                    {
                        if (int.TryParse(item.EntityStatusID.ToString(), out resultInt))
                        {
                            newItem.EntityStatusID = resultInt;

                        }
                    }
                    listAfter.Add(newItem);
                }

                return listAfter;
            }
        }
    }
}
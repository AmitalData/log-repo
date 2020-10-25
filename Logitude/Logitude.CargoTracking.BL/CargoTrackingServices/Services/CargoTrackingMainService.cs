
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public class CargoTrackingMainService
    {
        public static long timeOut = 100000000000000000;
        public string LastUpdate;
        public RecordUpdated recordUpdated = new RecordUpdated();
        public int MainThreadNumbers = 50;
        //public SqlBulkCopy MainBulk;
        //public SqlBulkCopy MainBulk2;
        //public SqlBulkCopy CopyMainBulk;
        //public SqlBulkCopy CopyMainBulk2;
        //public SqlBulkCopyColumnMappingCollection MainColumnMappings2;
        //public SqlBulkCopyColumnMappingCollection MainColumnMappings;
        //public SqlBulkCopyColumnMappingCollection CopyMainColumnMappings2;
        //public SqlBulkCopyColumnMappingCollection CopyMainColumnMappings;
        //List<DataTable> MainDataTables = new List<DataTable>();
       // List<CargoDeleteRowsArgs> MainDeleteRowsArgs = new List <CargoDeleteRowsArgs>();
        int ThreadsNumber = 0;
        int ThreadsCompleatedWork = 0;
        private object threadLock = new object();
      private static readonly Semaphore WorkLimiter = new Semaphore(100, 100);
        public List<CargoTable> FillCargoTableList()
        {
            List<CargoTable> CargoTableLists = new List<CargoTable>();

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Port",
                FieldsDBName = "Id,Tenant,Code,CountryId,EnglishName,AutomaticLastUpdateDate",
                CT_FieldsDBName = "Id,Code,EnglishName,CountryId,Tenant",
                KeyName = "Id",
                ConditionKey = "Id",
                DBTableName = "Ports",
                CT_TableName = "CargoTrackingPorts",
                Main_CT_TableName = "CargoTrackingPorts",
                Pre_TableName = "Pre_CargoTrackingPorts",
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Card",
                FieldsDBName = "Id,Tenant,Code,LocalName,EnglishName,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,Code,EnglishName,LocalName,Tenant",
                DBTableName = "Cards",
                CT_TableName = "CargoTrackingCards",
                Main_CT_TableName = "CargoTrackingCards",
                Pre_TableName = "Pre_CargoTrackingCards",
                ConditionsNumber = 1,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "TransportModes",
                FieldsDBName = "Id,Name,SearchFields,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,Name,SearchFields",
                DBTableName = "TransportModes",
                CT_TableName = "CargoTrackingTransportModes",
                Main_CT_TableName = "CargoTrackingTransportModes",
                Pre_TableName = "Pre_CargoTrackingTransportModes",
                ConditionsNumber = 1,
                IsClosedTable = true,
            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Countries",
                FieldsDBName = "Id,LocalName,Code,EnglishName,Tenant,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,LocalName,Code,EnglishName,Tenant",
                DBTableName = "Countries",
                CT_TableName = "CargoTrackingCountries",
                Main_CT_TableName = "CargoTrackingCountries",
                Pre_TableName = "Pre_CargoTrackingCountries",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "ShipmentMasterDatas",
                FieldsDBName = "Id,Master,MainCarriageATD,MainCarriageETD,MainCarriageATA,MainCarriageETA,Tenant,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,Tenant,Master,MainCarriageATD,MainCarriageETD,MainCarriageATA,MainCarriageETA",
                DBTableName = "ShipmentMasterDatas",
                CT_TableName = "CargoTrackingShipmentMasters",
                Main_CT_TableName = "CargoTrackingShipmentMasters",
                Pre_TableName = "Pre_CargoTrackingShipmentMasters",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "ShipmentComputedFields",
                FieldsDBName = "Id,FirstPickupATD,FinalDeliveryATA,FinalDeliveryETA,Tenant,AutomaticLastUpdateDate",
                KeyName = "Id",
                ConditionKey = "Id",
                CT_FieldsDBName = "Id,FirstPickupATD,FinalDeliveryATA,Tenant,FinalDeliveryETA",
                DBTableName = "ShipmentComputedFields",
                CT_TableName = "CargoTrackingShipmentComputeds",
                Main_CT_TableName = "CargoTrackingShipmentComputeds",
                Pre_TableName = "Pre_CargoTrackingShipmentComputeds",
                ConditionsNumber = 1,

            });

            CargoTableLists.Add(new CargoTable()
            {
                TableName = "Shipment",
                FieldsDBName = "Id,Tenant,CustomFileNumber,ForwarderShipmentNumber,CustomsDeclarationNumber,ShipperName,CustomerId,TransportModeId,MasterShipmentDataId,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,CustomConnectToShipment,AutomaticLastUpdateDate,ShipmentPickUpIndex,FirstPickupETA,ShipmentLevelCode,CustomsClearanceDate,CustomFileId,CreateDateTime,SecurityKey,ConsigneeName,CustomerReference1,CustomerReference2,FirstPickupETD,WarehouseLegActualEntryDate,WarehouseLegExpectedEntryDate,WarehouseLegRemarks,DeclarationDate,DirectionId,SearchFields",
                KeyName = "Id",
                KeyName2= "Id",
                ConditionKey = "EntityId",
                ConditionKey2 = "ShipmentId",
                DBTableName = "Shipments",
                CT_TableName = "CargoTrackingShipments",
                CT2_TableName = "CargoTrackingShipmentSearches",
                Main_CT_TableName = "CargoTrackingShipments",
                Main_CT2_TableName = "CargoTrackingShipmentSearches",
                CT_FieldsDBName = "Tenant,ShipperName,IsMainRecord,CustomerId,TransportModeId,Master,House,ShipmentNumber,FromPortId,ToPortId,ShipperId,ConsigneeId,GrossWeight,Volume,PickupDone,PickupDate,PickupEstimationDate,FromWarehouseDone,FromWarehouseNotes,DepartureDate,DepartureEstimationDate,ClearanceDone,ClearanceDate,FromWarehouseEstimationDate,FromWarehouseDate,DepartureDone,ArrivalDone,ToWarehouseDate,ToWarehouseEstimationDate,DeliveredEstimationDate,DeliveredDone,DeliveredDate,ArrivalEstimationDate,ToWarehouseDone,ArrivalDate,EntityId,EntityType,ForwardingShipmentHeaderId,CustomsShipmentHeaderId,CurrentMilestoneCode,CurrentMilestoneDate,ToWarehouseNotes,CustomsPaymentDone,CustomsPaymentDate,CreateDate,SecurityKey,ConsigneeName,CustomerReference",
                CT2_FieldsDBName = "Tenant,ShipmentId,SearchFields,ShipmentDate,IsPublic",
                Condition1 = " ((ShipmentLevelCode ='D' or ShipmentLevelCode ='H') and CustomFileId is not null)",
                Condition2 = " ((ShipmentLevelCode !='D' and ShipmentLevelCode !='H') or CustomFileId is null)",
                Pre_TableName = "Pre_CargoTrackingShipments",
                Pre2_TableName = "Pre_CargoTrackingShipmentSearches",
                ConditionsNumber = 2,
            });


            //CargoTableLists.Add(new CargoTable()
            //{
            //    TableName = "Shipments",
            //    FieldsDBName = "Id,Tenant,ShipmentNumber,SecurityKey,SearchFields,CreateDateTime,CustomerReference1,CustomerReference2,AutomaticLastUpdateDate,House,ShipmentLevelCode",
            //    KeyName = "Id",
            //    ConditionKey = "ShipmentId",
            //    CT_FieldsDBName = "Tenant,ShipmentId,SearchFields,ShipmentDate,IsPublic",
            //    DBTableName = "Shipments",
            //    CT_TableName = "CargoTrackingShipmentSearches",
            //    Main_CT_TableName = "CargoTrackingShipmentSearches",
            //    Pre_TableName = "Pre_CargoTrackingShipmentSearches",
            //    ConditionsNumber = 1,

            //});

  

            return CargoTableLists;

        }


        public RecordUpdated UpdateCargoTrackingDataBase(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        { 
            int NumberRecordUpdated = 0;
            int NumberRecordUpdated2 = 0;
            
            //MainDataTables = new List<DataTable>()
            RecordUpdated _recordUpdated = new RecordUpdated();
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            {
                DropTable(cargoTrackingDataBaseArgs.buildCargoArgs);
                CreateCargoTrackingTable(cargoTrackingDataBaseArgs.buildCargoArgs);
                cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName = cargoTrackingDataBaseArgs.buildCargoArgs.Table.Pre_TableName;
                cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT2_TableName = cargoTrackingDataBaseArgs.buildCargoArgs.Table.Pre2_TableName;
                this.MainThreadNumbers = (int)cargoTrackingDataBaseArgs.CargoTrackingArguments.ThreadNumber;
            }

            recordUpdated.IsFromBuild = GetIsIncrementalRunning(cargoTrackingDataBaseArgs.buildCargoArgs.SourceConnectionString);

            if (!recordUpdated.IsFromBuild || cargoTrackingDataBaseArgs.CargoTrackingArguments !=null) {
                
                bool IsUpadteWaterMark = false;
                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition1 == null)
                {
                    cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition = 0;
                    IsUpadteWaterMark = true;
                    _recordUpdated = UpdateCargoTrackingService(cargoTrackingDataBaseArgs, null, IsUpadteWaterMark);
                    NumberRecordUpdated += _recordUpdated.NumberOfRecordUpdated;
                    NumberRecordUpdated2 += _recordUpdated.NumberOfRecordUpdated2;
                }
                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition1 != null)
                {
                    cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition = 1;
                    if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition2 == null)
                    {
                        IsUpadteWaterMark = true;
                    }
                    _recordUpdated =  UpdateCargoTrackingService(cargoTrackingDataBaseArgs, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition1, IsUpadteWaterMark);
                    NumberRecordUpdated += _recordUpdated.NumberOfRecordUpdated;
                    NumberRecordUpdated2 += _recordUpdated.NumberOfRecordUpdated2;
                 }
                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition2 != null)
                {
                    cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition = 2;
                    if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition3 == null)
                    {
                        IsUpadteWaterMark = true;
                    }
                    _recordUpdated = UpdateCargoTrackingService(cargoTrackingDataBaseArgs, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition2, IsUpadteWaterMark);
                    NumberRecordUpdated += _recordUpdated.NumberOfRecordUpdated;
                    NumberRecordUpdated2 += _recordUpdated.NumberOfRecordUpdated2;
 
                }
                if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition3 != null)
                {
                    cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition = 3;
                    IsUpadteWaterMark = true;
                    _recordUpdated = UpdateCargoTrackingService(cargoTrackingDataBaseArgs, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Condition3, IsUpadteWaterMark);
                    NumberRecordUpdated += _recordUpdated.NumberOfRecordUpdated;
                    NumberRecordUpdated2 += _recordUpdated.NumberOfRecordUpdated2;
                }
            }
            recordUpdated.NumberOfRecordUpdated = NumberRecordUpdated;
            recordUpdated.NumberOfRecordUpdated2 = NumberRecordUpdated2;


            return recordUpdated;
        }
        private RecordUpdated UpdateCargoTrackingService(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, string Condition =null, bool IsUpadteWaterMark=false)
        {
            ThreadsNumber = 0;
            ThreadsCompleatedWork = 0;
            ThreadPool.SetMinThreads(1, 1);
           // ThreadPool.SetMaxThreads(100, 100);
            ThreadPool.SetMaxThreads(MainThreadNumbers, MainThreadNumbers);
            RecordUpdated _RecordUpdated = new RecordUpdated();

            BulkDataPreperation bulkDataPreperation = InitializeBulkDataPreperation(cargoTrackingDataBaseArgs);
            bool IsLastRecord = false;
            using (SqlConnection sourceConnection =
                                  new SqlConnection(cargoTrackingDataBaseArgs.buildCargoArgs.SourceConnectionString))
            {
                sourceConnection.Open();
                bulkDataPreperation.sqlDataReader = GetSqlDataReader(cargoTrackingDataBaseArgs, bulkDataPreperation.cargoTable, sourceConnection, Condition);
                DataTable dtSchema = bulkDataPreperation.sqlDataReader.GetSchemaTable();
                List<DataColumn> listCols = new List<DataColumn>();
                bulkDataPreperation.dataTable = new DataTable();

                if (bulkDataPreperation.sqlDataReader.HasRows)
                {
                    if (dtSchema != null)
                    {
                        foreach (DataRow drow in dtSchema.Rows)
                        {
                            DataColumn column = GetCoulmnFromDataRow(drow);
                            listCols.Add(column);
                            bulkDataPreperation.dataTable.Columns.Add(column);
                        }
                    }

                    while (bulkDataPreperation.sqlDataReader.Read())
                    {
                        //while ((ThreadsNumber - MainThreadNumbers) > ThreadsCompleatedWork)
                        //{
                        //    // Wait Threds To Finish Works
                        //}
                        bulkDataPreperation.dataTable = FillDataTableValues(listCols,   bulkDataPreperation, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName);

                        bulkDataPreperation.NumberRecoredTake++;

                        if (bulkDataPreperation.NumberRecoredTake == bulkDataPreperation.MaxRecoredTakeEachTime)
                        {
                            RunBulkThreads(bulkDataPreperation, cargoTrackingDataBaseArgs);
                        }
                        if (bulkDataPreperation.dataTable2!=null && bulkDataPreperation.dataTable2.Rows.Count >= bulkDataPreperation.MaxRecoredTakeEachTime)
                        {
                           RunBulkThreads2(bulkDataPreperation, cargoTrackingDataBaseArgs);
                        }

                        if (ThreadsNumber != 0 && ThreadsNumber % MainThreadNumbers == 0)
                        {
                            while (ThreadsCompleatedWork < ThreadsNumber)
                            {

                            }
                        }
                    }
                    while (ThreadsCompleatedWork < ThreadsNumber)
                    {

                    }


                    if (bulkDataPreperation.dataTable.Rows.Count > 0)
                    {
                        var Lastcolumns = bulkDataPreperation.dataTable.Rows
                                                             .Cast<DataRow>()
                                                             .Select(r => (string)r[bulkDataPreperation.cargoTable.KeyName].ToString())
                                                             .ToList();

                        bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, cargoTrackingDataBaseArgs, bulkDataPreperation.dataTable, Lastcolumns);
                    }

                    if (bulkDataPreperation.dataTable2!=null && bulkDataPreperation.dataTable2.Rows.Count > 0)
                    {
                        var Lastcolumns = bulkDataPreperation.dataTable2.Rows
                                                             .Cast<DataRow>()
                                                             .Select(r => (string)r[bulkDataPreperation.cargoTable.KeyName2].ToString())
                                                             .ToList();

                        bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, cargoTrackingDataBaseArgs, bulkDataPreperation.dataTable2, Lastcolumns,true);
                    }

                    bulkDataPreperation.sqlDataReader.Close();

                   

                }
                else
                {

                    bulkDataPreperation.sqlDataReader.Close();
                }
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
                {
                    Rename_Pre_Tables(cargoTrackingDataBaseArgs.buildCargoArgs, Condition);
                }
                if (IsUpadteWaterMark)
                {
                    UpdateWaterMarkAfterFinishCheck(bulkDataPreperation.cargoTable, bulkDataPreperation.automaticLastUpdateDate, cargoTrackingDataBaseArgs.buildCargoArgs);

                }
                sourceConnection.Close();
            }
            _RecordUpdated.NumberOfRecordUpdated = bulkDataPreperation.NumberOfCoulmnsUpdated;
            _RecordUpdated.NumberOfRecordUpdated2 = bulkDataPreperation.NumberOfCoulmnsUpdated2;

            return _RecordUpdated;

        }


        private void BuildAllIndexesWithConstraient(CargoArgs buildCargoArgs)
        {
            if (buildCargoArgs.Table.CT_TableName == "Pre_CargoTrackingShipments" && buildCargoArgs.Table.CurrentCondition == 2)
            {
                string cmd2 = CreateIndexAndRelations_Pre_Shipments(buildCargoArgs.Table.CT_TableName);
                ExecuteSql(cmd2, buildCargoArgs.DestinationConnectionString);

            }

                
            if (buildCargoArgs.Table.CT2_TableName == "Pre_CargoTrackingShipmentSearches" && buildCargoArgs.Table.CurrentCondition == 2)
            {
                string cmd = CreateIndex_Pre_ShipmentSearchs(buildCargoArgs.Table.CT2_TableName);
                ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
            }
               

        }

        private void RunBulkThreads2(BulkDataPreperation bulkDataPreperation , CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
 
                var columns = bulkDataPreperation.dataTable2.Rows
                         .Cast<DataRow>()
                         .Select(r => (string)r[bulkDataPreperation.cargoTable.KeyName2].ToString())
                         .ToList();
               
                DataTable ThreadDataTable = bulkDataPreperation.dataTable2.Clone();
                foreach (DataRow drtableOld in bulkDataPreperation.dataTable2.Rows)
                {
                    ThreadDataTable.ImportRow(drtableOld);

                }
                bulkDataPreperation.dataTable2.Rows.Clear();

            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                BuildThreadPool(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns, true);
            }
            else
            {
                Thread.Sleep(50);
                WorkLimiter.WaitOne();
                ThreadPool.QueueUserWorkItem(o => BuildThreadPool(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns, true));
            }


            //var thread = new Thread(() =>
            //    {
            //        try
            //        {
            //            ThreadsNumber += 1;
            //            bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns,true);
            //        }
            //        finally
            //        {
            //            ThreadsCompleatedWork += 1;
            //        }

            //    });


            //if (ThreadsNumber != 0 && ThreadsNumber % MainThreadNumbers == 0)
            //{
            //    while (ThreadsCompleatedWork < ThreadsNumber)
            //    {

            //    }
            //    if (ThreadsCompleatedWork >= ThreadsNumber)
            //    {
            //        WriteThreadsData(cargoTrackingDataBaseArgs);
            //    }
            //}


            //thread.Start();
            //    thread.IsBackground = true;

        }

        //private void WriteThreadsData(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        //{
        //   if( cargoTrackingDataBaseArgs.CargoTrackingArguments == null){
        //        for (int i = MainDeleteRowsArgs.Count - 1; i > -1; i--)
        //        {
        //            CargoDeleteRowsArgs RowsArgs = MainDeleteRowsArgs[i];
        //            DeleteRowsFromCargoTables(RowsArgs);
        //            MainDeleteRowsArgs.RemoveAt(i);
        //        }
        //    }
          

        //    for (int i = MainDataTables.Count - 1; i > -1; i--)
        //    {
        //        DataTable data_Table = MainDataTables[i];
        //        using (SqlBulkCopy newBulk =
        //                      new SqlBulkCopy(cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString, SqlBulkCopyOptions.KeepIdentity))
        //        {

                    
        //            newBulk.BulkCopyTimeout = (int)timeOut;
        //            newBulk.EnableStreaming = true;
        //            newBulk.BatchSize = 100000;
        //            if (MainBulk!=null && data_Table.TableName == MainBulk.DestinationTableName)
        //            {
        //                newBulk.DestinationTableName = MainBulk.DestinationTableName;
        //                for (int dd = 0; dd < MainColumnMappings.Count; dd++)
        //                {
        //                    newBulk.ColumnMappings.Add(MainColumnMappings[dd].SourceColumn, MainColumnMappings[dd].DestinationColumn);
        //                }

        //            }
        //            else if (CopyMainBulk!=null && data_Table.TableName == CopyMainBulk.DestinationTableName)
        //            {
        //                newBulk.DestinationTableName = CopyMainBulk.DestinationTableName;
        //                for (int dd = 0; dd < CopyMainColumnMappings.Count; dd++)
        //                {
        //                    newBulk.ColumnMappings.Add(CopyMainColumnMappings[dd].SourceColumn, CopyMainColumnMappings[dd].DestinationColumn);
        //                }

        //            }
        //            else if (MainBulk2!=null && data_Table.TableName == MainBulk2.DestinationTableName)
        //            {
        //                newBulk.DestinationTableName = MainBulk2.DestinationTableName;
        //                for (int dd = 0; dd < MainColumnMappings2.Count; dd++)
        //                {
        //                    newBulk.ColumnMappings.Add(MainColumnMappings2[dd].SourceColumn, MainColumnMappings2[dd].DestinationColumn);
        //                }

        //            }

        //            else if (CopyMainBulk2 != null && data_Table.TableName == CopyMainBulk2.DestinationTableName)
        //            {
        //                newBulk.DestinationTableName = CopyMainBulk2.DestinationTableName;
        //                for (int dd = 0; dd < CopyMainColumnMappings2.Count; dd++)
        //                {
        //                    newBulk.ColumnMappings.Add(CopyMainColumnMappings2[dd].SourceColumn, CopyMainColumnMappings2[dd].DestinationColumn);
        //                }

        //            }

        //            newBulk.WriteToServer(data_Table);
        //        }
               
        //        MainDataTables.RemoveAt(i);
        //     }
        //}

        //public SqlBulkCopy getNewCopyBulk(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        //{
        //    using (SqlBulkCopy newBulk =
        //                       new SqlBulkCopy(cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString, SqlBulkCopyOptions.KeepIdentity))
        //    {

        //        newBulk.DestinationTableName = MainBulk.DestinationTableName;
        //        newBulk.BulkCopyTimeout = (int)timeOut;
        //        newBulk.EnableStreaming = true;
        //        newBulk.BatchSize = 100000;
        //        for (int dd = 0; dd < MainColumnMappings.Count; dd++)
        //        {
        //            newBulk.ColumnMappings.Add(MainColumnMappings[dd].SourceColumn, MainColumnMappings[dd].DestinationColumn);
        //        }
        //        return newBulk;
        //    }

        //    return null;
        //}

        private void RunBulkThreads(BulkDataPreperation bulkDataPreperation, CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {

            
                var columns = bulkDataPreperation.dataTable.Rows
                         .Cast<DataRow>()
                         .Select(r => (string)r[bulkDataPreperation.cargoTable.KeyName].ToString())
                         .ToList();
                var ThreadStardBulding = bulkDataPreperation.NumberRecoredTake;
                DataTable ThreadDataTable = bulkDataPreperation.dataTable.Clone();
                foreach (DataRow drtableOld in bulkDataPreperation.dataTable.Rows)
                {
                    ThreadDataTable.ImportRow(drtableOld);

                }
                bulkDataPreperation.dataTable.Rows.Clear();
                bulkDataPreperation.NumberRecoredTake = 0;

            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
            {
                BuildThreadPool(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns);
            }
            else
            {
                 Thread.Sleep(50);
                 WorkLimiter.WaitOne();
                ThreadPool.QueueUserWorkItem(o => BuildThreadPool(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns));

            }

            //var thread = new Thread(() =>
            //    {
            //        try
            //        {
            //            ThreadsNumber += 1;
            //             
            //            bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns);
            //        }
            //        finally
            //        {
            //            ThreadsCompleatedWork += 1;
            //        }

            //    });

            //if (ThreadsNumber != 0 && ThreadsNumber % MainThreadNumbers == 0)
            //{
            //    while (ThreadsCompleatedWork < ThreadsNumber)
            //    {

            //    }

            //    if (ThreadsCompleatedWork >= ThreadsNumber)
            //    {
            //        WriteThreadsData(cargoTrackingDataBaseArgs);
            //    }
            //}

            //thread.Start();
            //thread.IsBackground = true;


        }
 
        private void BuildThreadPool (BulkDataPreperation bulkDataPreperation, CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, DataTable ThreadDataTable, List<string> columns, bool IsCT2 = false)
        {
            try
            {
                //ThreadsNumber += 1;
                Interlocked.Increment(ref ThreadsNumber);
                bulkDataPreperation = UpdateBulkPreperations(bulkDataPreperation, cargoTrackingDataBaseArgs, ThreadDataTable, columns, IsCT2);
            }
            finally
            {
               
                WorkLimiter.Release();
                Interlocked.Increment(ref ThreadsCompleatedWork);
                //ThreadsCompleatedWork += 1;
            }
        }
        private BulkDataPreperation InitializeBulkDataPreperation(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs)
        {
            DateTime? AutomaticDate = null;
            if (cargoTrackingDataBaseArgs.CargoTrackingArguments != null)
            {
                AutomaticDate = DateTime.Now;
            }
           
            BulkDataPreperation bulkDataPreperation = new BulkDataPreperation()
            {
                cargoTable = cargoTrackingDataBaseArgs.buildCargoArgs.Table,
                MaxRecoredTakeEachTime = cargoTrackingDataBaseArgs.NumberOfBulkPerTime,
                dataTable = null,
                automaticLastUpdateDate = AutomaticDate,
                sqlDataReader = null,
                NumberOfCoulmnsUpdated = 0,
                NumberOfCoulmnsUpdated2 =0,
                NumberRecoredTake = 0,
            };

            return bulkDataPreperation;


        }

        private BulkDataPreperation UpdateBulkPreperations(BulkDataPreperation bulkDataPreperation, CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, DataTable dataTable ,List<string> columns ,bool IsCT2=false)
        {
       
            if (!IsCT2)
               bulkDataPreperation.NumberOfCoulmnsUpdated += columns.Count;
            else
               bulkDataPreperation.NumberOfCoulmnsUpdated2 += columns.Count;

            bulkDataPreperation.cargoTable = PrepareTableParameters(cargoTrackingDataBaseArgs.buildCargoArgs, bulkDataPreperation.cargoTable, columns, IsCT2);
 
            bulkDataPreperation.automaticLastUpdateDate = UpdateBulkValues(cargoTrackingDataBaseArgs, dataTable, bulkDataPreperation.cargoTable, bulkDataPreperation.automaticLastUpdateDate, IsCT2);

          

            return bulkDataPreperation;
        }
        public void Rename_Pre_Tables(CargoArgs buildCargoArgs , string Condition)
        {
         
                BuildAllIndexesWithConstraient(buildCargoArgs);

            if (buildCargoArgs.Table.ConditionsNumber > 1)
            {
                switch (buildCargoArgs.Table.ConditionsNumber)
                {

                    case 2:
                        {
                            if (buildCargoArgs.Table.Condition2 == Condition)
                            {
                                StartRenameCargoTables(buildCargoArgs);
                            }
                            break;
                        }
                    case 3:
                        {
                            if (buildCargoArgs.Table.Condition3 == Condition)
                            {
                                StartRenameCargoTables(buildCargoArgs);
                            }
                            break;
                        }

                }

            }
            else
            {
                StartRenameCargoTables(buildCargoArgs);
            }
         

        }

        private void StartRenameCargoTables(CargoArgs buildCargoArgs)
        {
            string cmd = ChaneNameScript(buildCargoArgs.Table.Main_CT_TableName, buildCargoArgs.Table.Main_CT_TableName + "_SW") + "\n";
            cmd += ChaneNameScript(buildCargoArgs.Table.Pre_TableName, buildCargoArgs.Table.Main_CT_TableName) + "\n";
            cmd += ChaneNameScript(buildCargoArgs.Table.Main_CT_TableName + "_SW", buildCargoArgs.Table.Pre_TableName) + "\n";
            ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
            if (buildCargoArgs.Table.Main_CT2_TableName != null)
            {
                cmd  = ChaneNameScript(buildCargoArgs.Table.Main_CT2_TableName, buildCargoArgs.Table.Main_CT2_TableName + "_SW") + "\n";
                cmd += ChaneNameScript(buildCargoArgs.Table.Pre2_TableName, buildCargoArgs.Table.Main_CT2_TableName) + "\n";
                cmd += ChaneNameScript(buildCargoArgs.Table.Main_CT2_TableName + "_SW", buildCargoArgs.Table.Pre2_TableName) + "\n";
                ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
            }
        }
        private string  ChaneNameScript(string Old, string New)
        {

            string cmd = "EXEC sp_rename '" + Old + "', '" + New  + "' \n ";
            cmd += " exec sp_rename 'PK_"+ Old + "', 'PK_"+ New + "', 'object' \n ";
            if (Old == "CargoTrackingShipments" || New == "CargoTrackingShipments" || New == "Pre_CargoTrackingShipments")
            {
                cmd += " exec sp_rename 'FK_" + Old + "_CargoTrackingHeaderEntityTypes_EntityType', 'FK_" + New + "_CargoTrackingHeaderEntityTypes_EntityType', 'object' \n ";
                cmd += " exec sp_rename 'FK_" + Old + "_CargoTrackingMilestones_CurrentMilestoneCode', 'FK_" + New + "_CargoTrackingMilestones_CurrentMilestoneCode', 'object' \n ";
                cmd += " exec sp_rename 'UQ_" + Old + "_EntityType_EntityId_Tenant', 'UQ_" + New + "_EntityType_EntityId_Tenant', 'object' \n ";

            }
            return cmd;
        }

        private SqlDataReader GetSqlDataReader(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, CargoTable table, SqlConnection sourceConnection , string Condition=null)
        {
            string cmd;
            string condition;
            string fieldName = !string.IsNullOrEmpty(cargoTrackingDataBaseArgs.buildCargoArgs.Table.FieldsDBName) ? cargoTrackingDataBaseArgs.buildCargoArgs.Table.FieldsDBName : "*";
           
            if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition==1)
            {
                fieldName= " C." + fieldName.Replace(",", " ,C.");
                cmd = "SELECT " + fieldName + ", Min(P.Id) as ForwardingIdForCustom,com.ContainersNumbers as ContainersNumbers,com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA ,com.FirstPickupATD as FirstPickupATD, Mas.MainCarriageATD as MainCarriageATD,Mas.Master as Master ,  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA , Mas.MainCarriageETA  as MainCarriageETA " + " FROM dbo." + table.DBTableName + " P JOIN dbo." + table.DBTableName + " C ON P.CustomFileId = C.Id  Left Outer JOIN dbo.ShipmentComputedFields com on com.Id = C.Id  Left outer JOIN dbo.ShipmentMasterDatas Mas on Mas.Id = C.Id ";
                      
                if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
                {
                    LastUpdate = GetTableLastUpdate(cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString);
                    cmd += " where (C.AutomaticLastUpdateDate > '" + LastUpdate + "')";
                }
                else
                {
                    if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                    {
                        cmd += " where C.Tenant=" + cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and C.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate + "' and C.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate + "'";

                    }
                    else
                    {
                        cmd += " where C.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate + "' and C.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate + "'";
                    }
                }

                cmd += " group by " + fieldName + ",com.ContainersNumbers,com.FinalDeliveryETA,com.FinalDeliveryATA,com.FirstPickupATD,Mas.MainCarriageATD,Mas.Master,Mas.MainCarriageETD,Mas.MainCarriageATA,Mas.MainCarriageETA ";
            }
            else if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName == "CargoTrackingShipments" && cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition == 2)
            {
                fieldName = " P." + fieldName.Replace(",", " ,P.");
                cmd = "Select " + fieldName + ",com.ContainersNumbers as ContainersNumbers, com.FinalDeliveryETA as FinalDeliveryETA,com.FinalDeliveryATA as FinalDeliveryATA ,com.FirstPickupATD as FirstPickupATD, Mas.MainCarriageATD as MainCarriageATD, Mas.Master as Master ,  Mas.MainCarriageETD  as MainCarriageETD , Mas.MainCarriageATA  as MainCarriageATA , Mas.MainCarriageETA  as MainCarriageETA FROM dbo. " + table.DBTableName + " P Left Outer JOIN dbo.ShipmentComputedFields com on com.Id = P.Id  Left outer JOIN dbo.ShipmentMasterDatas Mas on Mas.Id = P.Id " + " Where P.Id not in (Select C.Id From  dbo." + table.DBTableName + " SH JOIN dbo." + table.DBTableName + " C ON SH.CustomFileId = C.Id) ";
               

                if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
                {
                    LastUpdate = GetTableLastUpdate(cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString);
                    cmd += " and (P.AutomaticLastUpdateDate > '" + LastUpdate + "')";
                }
                else
                {
                    if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                    {
                        cmd += " and P.Tenant=" + cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and P.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate + "' and P.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate + "'";

                    }
                    else
                    {
                        cmd += " and P.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate + "' and P.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate + "'";
                    }
                }

                cmd += " group by " + fieldName + ",com.ContainersNumbers,com.FinalDeliveryETA,com.FinalDeliveryATA,com.FirstPickupATD,Mas.MainCarriageATD,Mas.MainCarriageETD,Mas.Master,Mas.MainCarriageATA,Mas.MainCarriageETA ";

            }
            else if (cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName == "CargoTrackingShipmentSearches")
            {
                fieldName = " S." + fieldName.Replace(",", " ,S.");
                cmd = "SELECT " + fieldName + ", D.Master as Master " + " FROM dbo." + table.DBTableName + " S left outer JOIN dbo.ShipmentMasterDatas D ON S.Id = D.Id";


                if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
                {
                    LastUpdate = GetTableLastUpdate(cargoTrackingDataBaseArgs.buildCargoArgs.Table.CT_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString);
                    cmd += " where (S.AutomaticLastUpdateDate > '" + LastUpdate + "')";
                }
                else
                {
                    if (cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant != null)
                    {
                        cmd += " where S.Tenant=" + cargoTrackingDataBaseArgs.CargoTrackingArguments.Tenant + " and S.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate + "' and S.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate + "'";

                    }
                    else
                    {
                        cmd += " where S.CreateDateTime >= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.FromDate + "' and S.CreateDateTime <= '" + cargoTrackingDataBaseArgs.CargoTrackingArguments.ToDate + "'";
                    }
                }

            }
            else
            {
                  condition = GetUpdateDataBaseCondition(cargoTrackingDataBaseArgs.buildCargoArgs, cargoTrackingDataBaseArgs.CargoTrackingArguments, Condition);
                  cmd = "SELECT " + fieldName + " " +
                          "FROM dbo." + table.DBTableName + condition;
            }


            SqlCommand commandSourceData = new SqlCommand(cmd, sourceConnection);
            commandSourceData.CommandTimeout = (int)timeOut;
            SqlDataReader reader = commandSourceData.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;

        }

        private DataColumn GetCoulmnFromDataRow(DataRow drow)
        {
            string columnName = System.Convert.ToString(drow["ColumnName"]);
            DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
            column.Unique = (bool)drow["IsUnique"];
            column.AllowDBNull = (bool)drow["AllowDBNull"];
            column.AutoIncrement = (bool)drow["IsAutoIncrement"];

            return column;
        }

        private DataTable FillDataTableValues(List<DataColumn> listCols, BulkDataPreperation bulkDataPreperation, string TableName)
        {
            bool IsRoWValid = CargoTrackingValidateRecordsService.ValidateRecords(TableName, bulkDataPreperation.sqlDataReader);
            if (IsRoWValid)
            {
                DataRow dataRow = bulkDataPreperation.dataTable.NewRow();
                for (int i = 0; i < listCols.Count; i++)
                {
                    dataRow[((DataColumn)listCols[i])] = bulkDataPreperation.sqlDataReader[i];
                }
               
                  
                CargoTrackingSearchService.SearchService(dataRow, bulkDataPreperation, TableName);
                bulkDataPreperation.dataTable.Rows.Add(dataRow);
            }


            return bulkDataPreperation.dataTable;
        }

        private CargoTable PrepareTableParameters(CargoArgs buildCargoArgs, CargoTable table, List<string> columns,bool IsCT2=false)
        {
            table.UpdatedCount = columns != null ? columns.Count() : 0;

            if (IsCT2)
            {
                CargoDeleteRowsArgs DeleteRowsArgs = new CargoDeleteRowsArgs() { TableName = table.CT2_TableName, KeyName = table.ConditionKey2, IdsList = columns, ConnectionString = buildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true };
                //MainDeleteRowsArgs.Add(DeleteRowsArgs);
                table.RefreshIds2 = columns.ToString();
                //table.RefreshIds2 = DeleteRowsFromCargoTables(DeleteRowsArgs);
            }
            else
            {
                CargoDeleteRowsArgs DeleteRowsArgs = new CargoDeleteRowsArgs() { TableName = table.CT_TableName, KeyName = table.ConditionKey, IdsList = columns, ConnectionString = buildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true };
                //MainDeleteRowsArgs.Add(DeleteRowsArgs);
                table.RefreshIds = columns.ToString();
                //table.RefreshIds = DeleteRowsFromCargoTables(new CargoDeleteRowsArgs() { TableName = table.CT_TableName, KeyName = table.ConditionKey, IdsList = columns, ConnectionString = buildCargoArgs.DestinationConnectionString, ReturnDeleteIdsAsString = true });

            }
            return table;
        }

      

        private DateTime? UpdateBulkValues(CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs, DataTable dataTable, CargoTable table, DateTime? automaticLastUpdateDate ,bool IsCT2=false)
        {
            if (!string.IsNullOrEmpty(table.RefreshIds) || !string.IsNullOrEmpty(table.RefreshIds2))
            {

                using (SqlConnection destinationConnection =
                                    new SqlConnection(cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString))
                {
                    destinationConnection.Open();

                    using (SqlBulkCopy bulkCopy =
                               new SqlBulkCopy(cargoTrackingDataBaseArgs.buildCargoArgs.DestinationConnectionString, SqlBulkCopyOptions.KeepIdentity))
                    {
                     
                      

                        try
                        {
                            AutoMapColumns(bulkCopy, dataTable, table, IsCT2);


                            foreach (DataRow dr in dataTable.Rows)
                            {
                                if (!IsCT2)
                                {
                                    CargoTrackingTableLogicService.SetTableLogic(dr, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition);
                                }
                                else
                                {
                                    CargoTrackingTableLogicService.SetTableLogic(dr, cargoTrackingDataBaseArgs.buildCargoArgs.Table.Main_CT2_TableName, cargoTrackingDataBaseArgs.buildCargoArgs.Table.CurrentCondition);
                                }
                                

                            }
                            if (IsCT2)
                                bulkCopy.DestinationTableName = "dbo." + table.CT2_TableName;
                            else
                                bulkCopy.DestinationTableName = "dbo." + table.CT_TableName;

                            bulkCopy.BulkCopyTimeout = (int)timeOut;
                            bulkCopy.EnableStreaming = true;
                            bulkCopy.BatchSize = 100000;
                            //if (IsCT2)
                            //{
                            //    bulkCopy.DestinationTableName =
                            //                            "dbo." + table.CT2_TableName;

                            //    CopyMainBulk2 = MainBulk2;
                            //    MainBulk2 = bulkCopy;
                            //    CopyMainColumnMappings2 = MainColumnMappings2;
                            //    MainColumnMappings2 = MainBulk2.ColumnMappings;
                            //}
                            //else
                            //{
                            //    CopyMainBulk  = MainBulk ;
                            //    MainBulk = bulkCopy;
                            //    CopyMainColumnMappings  = MainColumnMappings ;
                            //    MainColumnMappings = MainBulk.ColumnMappings;

                            //}

                            //dataTable.TableName = bulkCopy.DestinationTableName;
                            //MainDataTables.Add(dataTable);
                            lock (threadLock)
                            {
                                bulkCopy.WriteToServer(dataTable);
                            }
                            //bulkCopy.WriteToServer(dataTable);

                        }

                        finally
                        {
                            if (cargoTrackingDataBaseArgs.CargoTrackingArguments == null)
                            {
                                automaticLastUpdateDate = GetAutomaticLastUpdateDate(automaticLastUpdateDate, dataTable, table.IsClosedTable);

                        }
                            else
                        {
                            automaticLastUpdateDate = null;
                        }
                            destinationConnection.Close();
                    }
                    }

                }

            }
            return automaticLastUpdateDate;
        }

        private DateTime? GetAutomaticLastUpdateDate(DateTime? automaticLastUpdateDate, DataTable dataTable,bool IsClosed)
        {

            if (!IsClosed)
            {
                //try
                //{
                    var MaxUpdate = (DateTime)dataTable.Rows
                                                    .Cast<DataRow>()
                                                    .Max(d => d["AutomaticLastUpdateDate"]);

                    if (MaxUpdate > automaticLastUpdateDate || automaticLastUpdateDate == null)
                    {
                        automaticLastUpdateDate = (DateTime)dataTable.Rows
                       .Cast<DataRow>()
                       .Max(d => d["AutomaticLastUpdateDate"]);
                    }

                //}
                //catch (Exception e)
                //{

                //}
               
            }
           

            return automaticLastUpdateDate;
        }

        private void UpdateWaterMarkAfterFinishCheck(CargoTable table, DateTime? automaticLastUpdateDate, CargoArgs buildCargoArgs)
        {
            if (table != null && table.DBTableName != "CargoTrackingWatermarks")
            {
               
                var lastUpdateDate = string.Empty;
                if (automaticLastUpdateDate != null) lastUpdateDate = automaticLastUpdateDate.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                else lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                UpdateWaterMarksTable(table, lastUpdateDate, buildCargoArgs.DestinationConnectionString);
                table.IsUpdated = true;

            }
        }

        public void AutoMapColumns(SqlBulkCopy sbc, DataTable dt, CargoTable Table , bool IsCT2=false)
        {
            List<string> MappingMatching = new List<string>();
            List<string> MappingDeferCT2_FieldsDBNameence = new List<string>();

            string[] DB_Cols = Table.FieldsDBName.Split(',');
            string[] CTDB_Cols = Table.CT_FieldsDBName.Split(',');
            if (IsCT2)
            {
                   CTDB_Cols = Table.CT2_FieldsDBName.Split(',');

            }
            string CompareDB = "";
            foreach (string CTDB_columns in CTDB_Cols)
            {
                foreach (string DB_columns in DB_Cols)
                {
                    if (CTDB_columns.Equals(DB_columns))
                    {
                        MappingMatching.Add(DB_columns + "," + CTDB_columns);
                        CompareDB += CTDB_columns + ",";
                    }
                }

            }
            foreach (string DB_columns in CTDB_Cols)
            {
                if (!CompareDB.Contains(DB_columns))
                {
                    if (IsCT2)
                    {
                        CargoTrackingCustomMappingService.MappingDB_CTDB(dt, sbc, DB_columns, Table.Main_CT2_TableName);
                    }
                    else
                    {
                        CargoTrackingCustomMappingService.MappingDB_CTDB(dt, sbc, DB_columns, Table.Main_CT_TableName);
                    }
                    
                }
            }

            foreach (string columns in MappingMatching)
            {
                string[] Cols = columns.Split(',');
                string col1 = Cols[0];
                string col2 = Cols[1];
                sbc.ColumnMappings.Add(col1, col2);
            }
        }

        public void CheckAndUpdateWaterMark(string dbSourceConnection, string dbDestenationConnection)
        {

            List<CargoTable> CargoTableLists = FillCargoTableList();
            foreach (CargoTable table in CargoTableLists)
            {
                if (table.DBTableName != "CargoTrackingWatermarks")
                {
                    using (SqlConnection SourceConnection =
                         new SqlConnection(dbSourceConnection))
                    {
                        SourceConnection.Open();

                        SqlCommand commandSourceData = new SqlCommand(
                       "SELECT  TableName" +
                       " FROM dbo.CargoTrackingWatermarks WHERE TableName = '" + table.CT_TableName + "'", SourceConnection);
                        commandSourceData.CommandTimeout = (int)timeOut;
                        SqlDataReader reader = commandSourceData.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            string lastUpdateDate = GetAutomaticLastUpdateDate(table.DBTableName, dbDestenationConnection);
                            if (string.IsNullOrEmpty(lastUpdateDate)) lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                            AddWaterMarksRecord(table, lastUpdateDate, dbSourceConnection);
                            SourceConnection.Close();
                        }
                        else
                        {
                            SourceConnection.Close();

                        }
                    }
                }
            }

        }

        public void UpdateWaterMarksTable(CargoTable table, string date, string connectionString)
        {
            var TodayDate = TenantServerConfigration.GetCurrentDateTime(0);
            string cmd = "update  CargoTrackingWatermarks set LastUpdateDate = '" + date + "',LastRun = '"+ TodayDate + "' where tableName = '" + table.Main_CT_TableName + "'";
            ExecuteSql(cmd, connectionString);

        }

        public void AddWaterMarksRecord(CargoTable table, string date, string connectionString)
        {
            string cmd = "insert into CargoTrackingWatermarks  values('" + table.CT_TableName + "' , NULL,NULL)";
            ExecuteSql(cmd, connectionString);
        }
        public void DeleteWatermarks(string connectionString)
        {
            string cmd = "Delete From CargoTrackingWatermarks";
            ExecuteSql(cmd, connectionString);
        }
        public void UpdateIsIncrementalRunning(int IsRunning, string connectionString)
        {
            string cmd = "Update Tenants set IsIncrementalBuildRunning = " + IsRunning +" Where Id = 0";
            ExecuteSql(cmd, connectionString);
        }
        public void ExecuteSql(string sqlString, string connectionString)
        {

            if (!string.IsNullOrEmpty(sqlString))
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlString, cn);
                    sqlCommand.CommandTimeout = (int)timeOut;
                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public string DeleteRowsFromCargoTables(CargoDeleteRowsArgs deleteRowsArgs)
        {
            int rowsCount = 0;
            StringBuilder allDeletedRows = new StringBuilder();
            StringBuilder deletedRows = new StringBuilder();
            foreach (string id in deleteRowsArgs.IdsList)
            {
                rowsCount += 1;
                deletedRows.Append("'" + id + "'" + ",");
                if (rowsCount == 1000 || (deleteRowsArgs.IdsList.IndexOf(id) == deleteRowsArgs.IdsList.IndexOf(deleteRowsArgs.IdsList.Last())))
                {
                    if (deleteRowsArgs.ReturnDeleteIdsAsString) allDeletedRows.Append(deletedRows.ToString());
                    string cmd = "delete from " + deleteRowsArgs.TableName + " where " + deleteRowsArgs.KeyName + " in " + ("(" + deletedRows.ToString() + ")").Replace(",)", ")");
                    ExecuteSql(cmd, deleteRowsArgs.ConnectionString);
                    rowsCount = 0;
                    deletedRows.Clear();
                }
            }

            return !string.IsNullOrEmpty(allDeletedRows.ToString()) ? ("(" + allDeletedRows.ToString() + ")").Replace(",)", ")") : null;

        }

        public void DropTable(CargoArgs buildCargoArgs)
        {
            string TableName = buildCargoArgs.Table.Pre_TableName;
            string TableName2 = buildCargoArgs.Table.Pre2_TableName;

            string cmd = "If exists (select * from sysobjects where name='" + TableName + "' and xtype='U') " +
                              " BEGIN " +
                              " Drop Table "+ TableName +
                              " END ";

             
            ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);

            if (!string.IsNullOrEmpty(TableName2))
            {
                  cmd = "If exists (select * from sysobjects where name='" + TableName2 + "' and xtype='U') " +
                          " BEGIN " +
                          " Drop Table " + TableName2 +
                          " END ";


                ExecuteSql(cmd, buildCargoArgs.DestinationConnectionString);
            }
        }

        public void CreateCargoTrackingTable(CargoArgs buildCargoArgs)
        {
            string TableName = buildCargoArgs.Table.Pre_TableName;
            string TableName2 = buildCargoArgs.Table.Pre2_TableName;
            string SQL = GetTableStructure(TableName);
            ExecuteSql(SQL, buildCargoArgs.DestinationConnectionString);
            if (!string.IsNullOrEmpty(TableName2))
            {
                SQL = GetTableStructure(TableName2);
                ExecuteSql(SQL, buildCargoArgs.DestinationConnectionString);
            }


         

                
        }

        private string GetTableStructure(string TableName)
        {
            string SQL = null;
            switch (TableName)
            {
                case "Pre_CargoTrackingPorts":
                    {
                        SQL = CreateTable_Pre_Ports(TableName);
                        break;
                    }
                case "Pre_CargoTrackingCards":
                    {
                        SQL = CreateTable_Pre_Cards(TableName);
                        break;
                    }
                case "Pre_CargoTrackingTransportModes":
                    {
                        SQL = CreateTable_Pre_TransportModes(TableName);
                        break;
                    }
                case "Pre_CargoTrackingCountries":
                    {
                        SQL = CreateTable_Pre_Countries(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipments":
                    {
                        SQL = CreateTable_Pre_Shipments(TableName);
                        //SQL += CreateIndexAndRelations_Pre_Shipments(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipmentSearches":
                    {
                        SQL = CreateTable_Pre_ShipmentSearchs(TableName);
                        //SQL += CreateIndex_Pre_ShipmentSearchs(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipmentMasters":
                    {
                        SQL = CreateTable_Pre_ShipmentMasters(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipmentComputeds":
                    {
                        SQL = CreateTable_Pre_ShipmentComputeds(TableName);
                        break;
                    }
            }

            return SQL;
        }
        private string CreateIndex_Pre_ShipmentSearchs(string TableName)
        {
            string cmd = "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_SearchFields_IsPublic] ON [dbo].[" + TableName + "]([Tenant],[SearchFields],[IsPublic])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_ShipmentId] ON [dbo].[" + TableName + "]([ShipmentId])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_SearchFields] ON [dbo].[" + TableName + "]([Tenant],[SearchFields])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_ShipmentId] ON [dbo].[" + TableName + "]([Tenant],[ShipmentId])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_ShipmentId_SearchFields] ON [dbo].[" + TableName + "]([Tenant],[ShipmentId],[SearchFields])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_SearchFields] ON [dbo].[" + TableName + "]([SearchFields])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant] ON [dbo].[" + TableName + "]([Tenant])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_ShipmentDate] ON [dbo].[" + TableName + "]([ShipmentDate])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_IsPublic] ON [dbo].[" + TableName + "]([IsPublic])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Id] ON [dbo].[" + TableName + "]([Id]) \n";
            return cmd;
         }

        private string CreateIndexAndRelations_Pre_Shipments(string TableName)
        {

            string cmd = "ALTER TABLE [dbo].[" + TableName + "] ADD CONSTRAINT [UQ_" + TableName + "_EntityType_EntityId_Tenant] UNIQUE([EntityType],[EntityId],[Tenant])\n";
            cmd += "ALTER TABLE [dbo].[" + TableName + "] ADD CONSTRAINT [FK_" + TableName + "_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_EntityType] ON [dbo].[" + TableName + "]([EntityType])\n";
            cmd += "ALTER TABLE [dbo].[" + TableName + "] ADD CONSTRAINT [FK_" + TableName + "_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_IsMainRecord_EntityId] ON [dbo].[" + TableName + "]([Tenant],[IsMainRecord],[EntityId])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_CurrentMilestoneCode] ON [dbo].[" + TableName + "]([CurrentMilestoneCode])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_EntityId] ON [dbo].[" + TableName + "]([EntityId])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_ShipmentNumber] ON [dbo].[" + TableName + "]([ShipmentNumber])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_CustomerReference] ON [dbo].[" + TableName + "]([CustomerReference])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_House] ON [dbo].[" + TableName + "]([House])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Master] ON [dbo].[" + TableName + "]([Master])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant] ON [dbo].[" + TableName + "]([Tenant])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_SecurityKey] ON [dbo].[" + TableName + "]([SecurityKey])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_SecurityKey] ON [dbo].[" + TableName + "]([Tenant],[SecurityKey])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_EntityId] ON [dbo].[" + TableName + "]([Tenant],[EntityId])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_EntityId_SecurityKey] ON [dbo].[" + TableName + "]([Tenant],[EntityId],[SecurityKey])\n";
            cmd += "CREATE NONCLUSTERED INDEX [IX_" + TableName + "_Tenant_IsMainRecord_EntityId_CustomsShipmentHeaderId] ON [dbo].[" + TableName + "]([Tenant],[IsMainRecord],[EntityId],[CustomsShipmentHeaderId]) \n";
            return cmd;
         }



        private string CreateTable_Pre_Ports(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='"+TableName+"' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE [dbo].[" + TableName + "](" +
                            "[Id] VARCHAR(15) NOT NULL," +
                            "[Tenant] INT NOT NULL,"+
                            "[Code] VARCHAR(3) NOT NULL," +
                            "[EnglishName] VARCHAR(40) NULL," +
                            "[CountryId] VARCHAR(15) NOT NULL," +
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])" +
                            ")" +
                            " End ";

            return cmd;

        }

        private string CreateTable_Pre_Cards(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].["+ TableName + "]("+
                            "[Id] VARCHAR(15) NOT NULL,"+
                            "[Tenant] INT NOT NULL," +
                            "[Code] VARCHAR(15) NOT NULL," +
                            "[EnglishName] VARCHAR(70) NULL,"+
                            "[LocalName] NVARCHAR(100) NULL,"+
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                            ")" +
                            " End ";

            return cmd;

        }

        private string CreateTable_Pre_Countries(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].["+ TableName + "]("+
                            "[Id] VARCHAR(15) NOT NULL,"+
                            "[Tenant] INT NOT NULL," +
                            "[LocalName] NVARCHAR(120) NULL," +
                            "[Code] CHAR(2) NOT NULL,"+
                            "[EnglishName] VARCHAR(120) NOT NULL,"+
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                            ")"+
                            " End ";

            return cmd;

        }

        private string CreateTable_Pre_TransportModes(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                            "BEGIN " +
                            "CREATE TABLE[dbo].["+ TableName + "]("+
                            "[Id] CHAR(1) NOT NULL,"+
                            "[SearchFields] NVARCHAR(1000) NULL,"+
                            "[Name] VARCHAR(10) NOT NULL,"+
                            "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                            ")"+
                            " End ";

            return cmd;

        }

        private string CreateTable_Pre_ShipmentSearchs(string TableName)
        {
    
          string  cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                          "BEGIN " +
                          "CREATE TABLE [dbo].["+ TableName + "](" +
                          "[Tenant] INT NOT NULL," +
                          "[SearchFields] NVARCHAR(1000) NULL," +
                          "[ShipmentDate] DATETIME NOT NULL," +
                          "[Id] INT IDENTITY(1,1) NOT NULL," +
                          "[ShipmentId] VARCHAR(15) NULL," +
                          "[IsPublic] BIT DEFAULT(0) NULL," +
                          "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])" +
                          ") End \n";
            return cmd;

        }


        private string CreateTable_Pre_ShipmentComputeds(string TableName)
        {
            string cmd =  "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                          "BEGIN " +
                          "CREATE TABLE [dbo].[" + TableName + "](" +
                          "[Id] VARCHAR(15) NOT NULL," +
                          "[Tenant] INT NOT NULL," +
                          "[FirstPickupATD] DATETIME NULL," +
                          "[FinalDeliveryATA] DATETIME NULL," +
                          "[FinalDeliveryETA] DATETIME NULL," +
                          "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])" +
                          ")" +
                          " End ";

            return cmd;
        }

        private string CreateTable_Pre_ShipmentMasters(string TableName)
        {
            string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                          "BEGIN " +
                          "CREATE TABLE [dbo].["+ TableName + "](" +
                          "[Id] VARCHAR(16) NOT NULL,"+
                          "[Tenant] INT NOT NULL," +
                          "[Master] VARCHAR(20) NULL," +
                          "[MainCarriageATD] DATETIME NULL," +
                          "[MainCarriageETD] DATETIME NULL,"+
                          "[MainCarriageATA] DATETIME NULL,"+
                          "[MainCarriageETA] DATETIME NULL,"+
                          "CONSTRAINT[PK_"+ TableName + "] PRIMARY KEY([Id])"+
                          ")"+
                          " End ";
            return cmd;
        }

    private string CreateTable_Pre_Shipments(string TableName)
     { 


        string cmd = "If not exists (select * from sysobjects where name='" + TableName + "' and xtype='U')" +
                     "BEGIN " +
                     "CREATE TABLE [dbo].[" + TableName + "](" +
                     "[Tenant] INT NOT NULL," +
                     "[EntityId] VARCHAR(15) NULL," +
                     "[ForwardingShipmentHeaderId] VARCHAR(15) NULL," +
                     "[CustomsShipmentHeaderId] VARCHAR(15) NULL," +
                     "[EntityType] VARCHAR(1) NULL," +
                     "[CurrentMilestoneCode] VARCHAR(2) NULL," +
                     "[CurrentMilestoneDate] DATETIME NULL," +
                     "[CustomerId] VARCHAR(15) NULL," +
                     "[TransportModeId] VARCHAR(15) NULL," +
                     "[Master] VARCHAR(20) NULL," +
                     "[House] VARCHAR(20) NULL," +
                     "[ShipmentNumber] VARCHAR(20) NULL," +
                     "[FromPortId] VARCHAR(15) NULL," +
                     "[ToPortId] VARCHAR(15) NULL," +
                     "[ShipperId] VARCHAR(15) NULL," +
                     "[ConsigneeId] VARCHAR(15) NULL," +
                     "[GrossWeight] FLOAT NULL," +
                     "[Volume] FLOAT NULL," +
                     "[PickupDone] BIT DEFAULT(0) NULL," +
                     "[PickupDate] DATETIME NULL," +
                     "[CreateDate] DATETIME NOT NULL," +
                     "[SecurityKey] VARCHAR(40) NULL," +
                     "[ConsigneeName] VARCHAR(70) NULL," +
                     "[CustomerReference] VARCHAR(101) NULL," +
                     "[IsMainRecord] BIT DEFAULT(0) NOT NULL," +
                     "[PickupEstimationDate] DATETIME NULL," +
                     "[FromWarehouseDate] DATETIME NULL," +
                     "[FromWarehouseEstimationDate] DATETIME NULL," +
                     "[FromWarehouseNotes] NVARCHAR(500) NULL," +
                     "[DepartureDone] BIT DEFAULT(0) NULL," +
                     "[DepartureDate] DATETIME NULL," +
                     "[DepartureEstimationDate] DATETIME NULL," +
                     "[ArrivalDone] BIT DEFAULT(0) NULL," +
                     "[ArrivalDate] DATETIME NULL," +
                     "[ArrivalEstimationDate] DATETIME NULL," +
                     "[ToWarehouseDone] BIT DEFAULT(0) NULL," +
                     "[ToWarehouseDate] DATETIME NULL," +
                     "[ToWarehouseEstimationDate] DATETIME NULL," +
                     "[ToWarehouseNotes] NVARCHAR(32) NULL," +
                     "[CustomsPaymentDone] BIT DEFAULT(0) NULL," +
                     "[CustomsPaymentDate] DATETIME NULL," +
                     "[ClearanceDone] BIT DEFAULT(0) NULL," +
                     "[ClearanceDate] DATETIME NULL," +
                     "[DeliveredDone] BIT DEFAULT(0) NULL," +
                     "[DeliveredDate] DATETIME NULL," +
                     "[DeliveredEstimationDate] DATETIME NULL," +
                     "[FromWarehouseDone] BIT DEFAULT(0) NULL," +
                     "[FirstPickupETD] DATETIME NULL," +
                     "[ShipperName] VARCHAR(70) NULL,"+
                     "[WarehouseLegActualEntryDate] DATETIME NULL," +
                     "[WarehouseLegExpectedEntryDate] DATETIME NULL," +
                     "[WarehouseLegRemarks] NVARCHAR(500) NULL," +
                     "[DeclarationDate] DATETIME NULL," +
                     "[CustomsClearanceDate] DATETIME NULL," +
                     "[Id] INT IDENTITY(1,1) NOT NULL," +
                     "CONSTRAINT[PK_" + TableName + "] PRIMARY KEY([Id])" +
                     ")  End \n";

            return cmd;

        }


        public string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }
      
        private string GetUpdateDataBaseCondition(CargoArgs buildCargoArgs, CargoTrackingArguments CargoTrackingArguments = null, string Condition=null)
        {
            //string LastUpdate = null;
            if (CargoTrackingArguments == null)
            {
                  LastUpdate = GetTableLastUpdate(buildCargoArgs.Table.CT_TableName, buildCargoArgs.DestinationConnectionString);
            }
            BuildWhereConditionArgs buildWhereConditionArgs = new BuildWhereConditionArgs()
            {
                TableName = buildCargoArgs.Table.Main_CT_TableName,
                LastUpdate = LastUpdate,
                CargoTrackingArguments= CargoTrackingArguments,
                Condition= Condition,
            };
            string condition = CargoTrackingTableBuildWhereCondition.BuildWhereCondition(buildWhereConditionArgs, buildCargoArgs.Table.IsClosedTable);
            return condition;
        }


        public string GetTableLastUpdate(string tableName, string connectionString)
        {

 
            string result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "Select top 1 LastUpdateDate from CargoTrackingWatermarks where TableName = '" + tableName + "';", con);
            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    DateTime? datetime = null;
                    var value = reader["LastUpdateDate"];
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.ToString()))
                        {
                            datetime = (DateTime?)(value);
                            if (datetime != null) result = datetime.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                        }

                    }
                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string GetAutomaticLastUpdateDate(string tableName, string connectionString)
        {
            string result = null;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "select MIN(AutomaticLastUpdateDate) -1 AutomaticLastUpdateDate " +
               "FROM dbo." + tableName + " ;", con);

            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    DateTime? datetime = null;
                    var value = reader["AutomaticLastUpdateDate"];
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.ToString()))
                        {
                            datetime = (DateTime?)(value);
                            if (datetime != null) result = datetime.Value.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                        }

                    }
                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public bool GetIsIncrementalRunning(string connectionString)
        {
            bool Result = false;

            SqlConnection con = new SqlConnection(connectionString);

            SqlCommand com = new SqlCommand(
               "Select  IsIncrementalBuildRunning FROM dbo.Tenants Where Id = 0", con);
            try
            {
                com.CommandTimeout = (int)timeOut;
                con.Open();
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();
                    bool IsRunning = false;
                    var value = reader["IsIncrementalBuildRunning"];
                    if (value != null)
                    { 
                            IsRunning = (bool)(value);
                            if (IsRunning != null) Result = IsRunning ;
      

                    }
                }
            }
            finally
            {
                con.Close();
            }
            return Result;
        }


    }

    public class CargoTrackingArguments
    {
        public DateTime? FromDate;
        public DateTime? ToDate;
        public int? Tenant;
        public bool? AllData;
        public int? ThreadNumber;
        public string FormTableName;
    }

    public class RecordUpdated
    {
        public int NumberOfRecordUpdated {get;set;}
        public int NumberOfRecordUpdated2 { get; set; }
        public bool IsFromBuild { get; set; }
    }


    public class BulkDataPreperation
    {
        public DataTable dataTable { get; set; }
        public DataTable dataTable2 { get; set; }
        public SqlDataReader sqlDataReader { get; set; }
        public CargoTable cargoTable { get; set; }
        public DateTime? automaticLastUpdateDate { get; set; }
        public int NumberOfCoulmnsUpdated { get; set; }
        public int NumberOfCoulmnsUpdated2 { get; set; }

        public int MaxRecoredTakeEachTime { get; set; }
        public int NumberRecoredTake { get; set; }
    }

}

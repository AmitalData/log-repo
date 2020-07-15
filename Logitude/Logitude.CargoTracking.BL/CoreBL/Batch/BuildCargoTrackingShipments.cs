  
 
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.CargoTracking.BL.CoreBL.Batch
{
    public class BuildCargoTrackingShipments : BatchTaskExecutionsService
    {
        string sourceConnectionString = string.Empty;
        string destinationConnectionString = string.Empty;
        CargoTrackingMainService cargoTrackingMainService;
        CargoTrackingArgs CargoTrackingArguments;

        public BuildCargoTrackingShipments(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            cargoTrackingMainService = new CargoTrackingMainService();
            BuildConnectionString();
        }

        public override void RunCode()
        {
            CargoTrackingArguments = GetCargoTrackingArgs();
            try
            {
                cargoTrackingMainService.CheckAndUpdateWaterMark(sourceConnectionString);
                AddAllTablesToThread(cargoTrackingMainService.FillCargoTableList());
            }

            catch (Exception e)
            {
                throw new Exception(e.Message+"\n"+e.StackTrace);
            }

        }

 
        
        private CargoTrackingArgs GetCargoTrackingArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(CargoTrackingArgs));
            CargoTrackingArgs  Args = serializer.Deserialize(stringReader) as CargoTrackingArgs;
            return Args;
        }

        private void UpdateCargoDataBase(CargoTable table)
        {
            CargoTrackingArguments CargoTrackingArgs = new CargoTrackingArguments
            {
                FromDate = CargoTrackingArguments.FromDate,
                Tenant = CargoTrackingArguments.Tenant,
                ToDate = CargoTrackingArguments.ToDate,
            };
             cargoTrackingMainService.UpdateCTDataBase(new CargoArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString }, 1000,null, CargoTrackingArgs);
        }


        private void AddAllTablesToThread(List<CargoTable> CargoTableLists)
        {
            foreach (CargoTable table in CargoTableLists)
            {

                UpdateCargoDataBase(table);

            }
        }

        private void BuildConnectionString()
        {
            string[] sourceConnectionArray = "LogitudeMain-Test2,sa,Saas256,logitudetestdb.westeurope.cloudapp.azure.com,.".Split(',');
            string[] destinationConnectionArray = "CargoTracking,sa,Saas256,logitudetestdb.westeurope.cloudapp.azure.com".Split(',');
            sourceConnectionString =  BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            destinationConnectionString =  BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);
        }

        private string GetUpdateDataBaseCondition(CargoArgs buildCargoArgs)
        {

            string condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName = " + "'" + buildCargoArgs.Table.CT_TableName + "')";
            return condition;
        }

        private string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }
    }

    

    public class CargoTrackingArgs
    {
        public DateTime? FromDate;
        public DateTime? ToDate;
        public int? Tenant;

    }
}

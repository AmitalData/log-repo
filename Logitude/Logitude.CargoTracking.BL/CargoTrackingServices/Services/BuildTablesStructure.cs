using Logitude.CargoTracking.BL.CargoTrackingServices.Services.TableStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services
{
    public static class BuildTablesStructure
    {

        public static string GetTableStructure(string TableName)
        {
            string SQL = null;
            switch (TableName)
            {
                case "Pre_CargoTrackingPorts":
                    {
                        SQL = PortTableStrucrue.CreateTable_Pre_Ports(TableName);
                        break;
                    }
                case "Pre_CargoTrackingCards":
                    {
                        SQL = CardTableStrucrue.CreateTable_Pre_Cards(TableName);
                        break;
                    }
                case "Pre_CargoTrackingTransportModes":
                    {
                        SQL = TransportModeTableStrucrue.CreateTable_Pre_TransportModes(TableName);
                        break;
                    }
                case "Pre_CargoTrackingCountries":
                    {
                        SQL = CountriesTableStrucrue.CreateTable_Pre_Countries(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipments":
                    {
                        SQL = ShipmentTableStrucrue.CreateTable_Pre_Shipments(TableName);
                        //SQL += ShipmentTableStrucrue.CreateIndexAndRelations_Pre_Shipments(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipmentSearches":
                    {
                        SQL = ShipmentSearcheTableStrucrue.CreateTable_Pre_ShipmentSearchs(TableName);
                        //SQL += ShipmentSearcheTableStrucrue.CreateIndex_Pre_ShipmentSearchs(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipmentMasters":
                    {
                        SQL = ShipmentMasterTableStrucrue.CreateTable_Pre_ShipmentMasters(TableName);
                        break;
                    }
                case "Pre_CargoTrackingShipmentComputeds":
                    {
                        SQL = ShipmentComputedTableStrucrue.CreateTable_Pre_ShipmentComputeds(TableName);
                        break;
                    }
            }

            return SQL;
        }

 
 
    }
}
